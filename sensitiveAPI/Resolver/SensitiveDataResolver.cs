using System;
using System.Text.Json;
using System.Threading.Tasks;
using sensitiveAPI.Configuration;
using sensitiveAPI.Entities;
using sensitiveAPI.Helper;
using sensitiveAPI.Models;
using sensitiveAPI.Repository;

namespace sensitiveAPI.Resolver
{
    public class SensitiveDataResolver
    {
        private readonly DataProtector dataProtector;
        private readonly SensitiveRepository sensitiveRepository;
        private readonly IEncryptionKeysConfig config;

        public SensitiveDataResolver(
            DataProtector dataProtector,
            SensitiveRepository sensitiveRepository,
            IEncryptionKeysConfig config)
        {
            this.dataProtector = dataProtector;
            this.sensitiveRepository = sensitiveRepository;
            this.config = config;
        }

        /// <summary>
        /// Ask for clear text of data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>cleartext</returns>
        public virtual async Task<DecryptedOutput> GetDecryptDataByIdAsync(string id)

        {
            var entity = await sensitiveRepository.GetByIdAsync(Int64.Parse(id));
            if (entity == null)
            {
                throw new NotFoundException(
                    $"Could not find data with ID {id}");
            }

            var data = JsonSerializer.Deserialize<AesModel>(entity.Data);

            var encryptionKey = config.GetKeyByName(entity.EncryptionKeyName);

            if (string.IsNullOrWhiteSpace(encryptionKey))
            {
                throw new MissingEncryptionKeyException($"Could not retrieve {entity.EncryptionKeyName}");
            }

            var cleartext = dataProtector.Decrypt(encryptionKey, data);

            return new DecryptedOutput()
            {
                Id = id,
                Value = cleartext
            };
        }

        /// <summary>
        /// Remove data by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task RemoveDataAsync(string id)
        {
            
            var entity = await sensitiveRepository.GetByIdAsync(Int64.Parse(id));
            sensitiveRepository.Remove(entity);
        }

        /// <summary>
        /// Send a clear text and encrypt and save in database
        /// </summary>
        /// <param name="cleartext">The data to protect</param>
        /// <returns>The ID of the projected blob</returns>
        public virtual async Task<EncryptedOutput> EncryptDataAsync(string cleartext)
        {
            var encryptionKey = config.GetCurrentKey();

            if (string.IsNullOrWhiteSpace(encryptionKey))
            {
                throw new MissingEncryptionKeyException("Could not retrieve Current Encryption Key.");
            }

            var data = new SensitiveDataEntity
            {
                EncryptionKeyName = config.CurrentKeyName,
                Data = dataProtector.Encrypt(config.GetCurrentKey(), cleartext)
            };

            var savedId = sensitiveRepository.Add(data);
            return new EncryptedOutput()
            {
                Id = savedId.ToString()
            };
        }

        /// <summary>
        /// editing encrypted data by id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task ReplaceEncryptDataAsync(UpdateInput input)
        {
            var entity = await sensitiveRepository.GetByIdAsync(Int64.Parse(input.Id));
            if (entity == null)
            {
                throw new NotFoundException(
                    $"Could not find data with ID {input.Id}.");
            }

            var encryptionKey = config.GetCurrentKey();
            if (string.IsNullOrWhiteSpace(encryptionKey))
            {
                throw new MissingEncryptionKeyException($"Could not retrieve {entity.EncryptionKeyName}.");
            }

            var newData = new SensitiveDataEntity
            {
                Id = Int64.Parse(input.Id),
                Data = dataProtector.Encrypt(encryptionKey, input.ClearText),
                EncryptionKeyName = config.CurrentKeyName
            };

            sensitiveRepository.Update(newData);
        }
    }
}