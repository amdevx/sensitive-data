using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sensitiveAPI.Data;
using sensitiveAPI.Entities;

namespace sensitiveAPI.Repository
{
    public class SensitiveRepository : ISensitiveRepository
    {
        private readonly SensitiveContext _context;

        public SensitiveRepository(SensitiveContext context)
        {
            _context = context;
        }

        public void Add(SensitiveDataEntity sensitiveDataEntity)
        {
            _context.SensitiveDataEntities.Add(sensitiveDataEntity);
            _context.SaveChanges();
        }

        public void Update(SensitiveDataEntity sensitiveDataEntity)
        {
            _context.SensitiveDataEntities.Update(sensitiveDataEntity);
            _context.SaveChanges();
        }

        public void Remove(SensitiveDataEntity sensitiveDataEntity)
        {
            _context.SensitiveDataEntities.Remove(sensitiveDataEntity);
            _context.SaveChanges();
        }

        public Task<List<string>> GetAllActiveEncryptionKeyNamesAsync()
        {
            return _context.SensitiveDataEntities.Select(x => x.EncryptionKeyName).Distinct().ToListAsync();
        }

        public Task<SensitiveDataEntity> GetByIdAsync(long id)
        {
            return _context.SensitiveDataEntities.FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}