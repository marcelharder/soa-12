using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations
{
    public class AorticSurgery: IAorticSurgery
    {
        private DataContext _context;
        public AorticSurgery(DataContext context)
        {
            _context = context;
        }
        public async Task<int> addCAS(Class_Aortic_Surgery help)
        {
             // nog meer initiele waarden kunnen hier
            _context.AorticSurgeries.Add(help);
            if (await SaveAll()) { return 1; }  else  {  return 0; }
        }

        public async Task<int> updateCAS(Class_Aortic_Surgery p)
        {
            _context.Update(p);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> deleteCAS(int id)
        {
            var help = await getSpecificCAS(id);
            return await DeleteAsync(help);
        }

        public async Task<Class_Aortic_Surgery> getSpecificCAS(int id)
        {
            var result = new Class_Aortic_Surgery();
            if (await _context.AorticSurgeries.AnyAsync(u => u.procedure_id == id))// check if there is a record for this procedure
            {
                return await _context.AorticSurgeries.FirstOrDefaultAsync(u => u.procedure_id == id);
            }
            else
            {
                var aos = new Class_Aortic_Surgery();
                aos.procedure_id = id;
               
                _context.Add(aos);
                if (await this.SaveAll())
                {
                    return await _context.AorticSurgeries.FirstOrDefaultAsync(u => u.procedure_id == id);
                }
                else { return null; }
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if (await SaveAll()) { return 1; } else { return 0; }
        }

       
    } 
    }
