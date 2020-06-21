using System.Collections.Generic;
using System.Threading.Tasks;
using sensitiveAPI.Entities;

namespace sensitiveAPI.Repository
{
    public interface ISensitiveRepository
    {
        void Add(SensitiveDataEntity sensitiveDataEntity);
        void Update(SensitiveDataEntity sensitiveDataEntity);
        void Remove(SensitiveDataEntity sensitiveDataEntity);
        Task<List<string>> GetAllActiveEncryptionKeyNamesAsync();
        Task<SensitiveDataEntity> GetByIdAsync(long id);
    }
}