
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class MinInv: IMinInv
    {
        private DataContext _context;
        public MinInv(DataContext context)
        {
            _context = context;
        }
        public async Task<int> addMIN(Class_minInv help)
        {
            // nog meer initiele waarden kunnen hier

            _context.MinInvs.Add(help);
            if (await SaveAll()) { return 1; } else { return 0; }
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }

        public async Task<int> deleteMIN(int id){var help = await getSpecificMIN(id);return await DeleteAsync(help);}

        public async Task<Class_minInv> getSpecificMIN(int id)
        {
            var result = new Class_minInv();
            if (await _context.MinInvs.AnyAsync(u => u.PROCEDURE_ID == id))// check if there is a record for this procedure
            {
                return await _context.MinInvs.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
            }
            else
            {
                var aos = new Class_minInv();
                aos.PROCEDURE_ID = id;

                _context.Add(aos);
                if (await this.SaveAll())
                {
                    return await _context.MinInvs.FirstOrDefaultAsync(u => u.PROCEDURE_ID == id);
                }
                else { return null; }
            }
        }

        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }
        public async Task<int> updateMIN(Class_minInv p) { _context.Update(p);  await _context.SaveChangesAsync(); return 1; } 
    }
}