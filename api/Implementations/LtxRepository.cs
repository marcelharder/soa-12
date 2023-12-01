
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class LtxRepository: ILtxRepository
    {
         private DataContext _context;
        public LtxRepository(DataContext context)
        {
            _context = context;
        }

        public Task<int> addLTX(Class_LTX p)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
             _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }

        public async Task<int> deleteLTX(int id)
        {
            var help = await GetSpecificLTX(id);
            return await DeleteAsync(help);
        }

        public List<Class_LTX> GetLTX(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Class_LTX> GetSpecificLTX(int id)
        {
            var result = new Class_LTX();
            if (await _context.LTXs.AnyAsync(u => u.PROCEDURE_ID == id))// check if there is a record for this procedure
            {
                return await _context.LTXs.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
            }
            else
            {
                var aos = new Class_LTX();
                aos.PROCEDURE_ID = id;

                aos.AcceptorProcedureStart = DateTime.UtcNow;
                aos.DonorProcedureStart = DateTime.UtcNow;
                aos.DonorStartIschemia = DateTime.UtcNow;
                aos.DonorStartReperfusion = DateTime.UtcNow;

                _context.Add(aos);
                if (await this.SaveAll())
                {
                    return await _context.LTXs.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
                }
                else { return null; }
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> updateLTX(Class_LTX p)
        {
            _context.Update(p);
            await _context.SaveChangesAsync();
            return 1;
        }
    }
}