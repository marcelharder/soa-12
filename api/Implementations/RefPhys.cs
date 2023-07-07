using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Implementations
{

    public class RefPhys : IRefPhys
    {

        private DataContext _context;
        private SpecialMaps _sm;
        

        public RefPhys(DataContext context, SpecialMaps sm)
        {
            _context = context;
            _sm = sm;
          
        }

        public async Task<Class_Ref_Phys> addRefPhys()
        {
            var new_record = new Class_Ref_Phys();
            _context.RefPhys.Add(new_record);
            if (await SaveAll()) {
                return new_record;
            }
            return null;
        }

        public async Task<int> deleteRefPhys(int id)
        {
            var proc = await _context.RefPhys.FirstOrDefaultAsync(u => u.Id == id);
            _context.RefPhys.Remove(proc);
            if (await SaveAll())
            {
                return 1;
            }
            return 0;
        }

        public async Task<int> updateRefPhys(Class_Ref_Phys p)
        {
            _context.RefPhys.Update(p);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Class_Item>> getAllRefPhysInThisHospital(int hospital_id)
        {
            var l = new List<Class_Item>();
            await Task.Run(() =>
            {
                var result = _context.RefPhys.OrderByDescending(u => u.hospital_id).AsQueryable();
                result = result.Where(s => s.hospital_id == hospital_id);
                foreach (Class_Ref_Phys rf in result)  { l.Add(_sm.mapRefPhysToClassItem(rf)); }
            });
            return l;
        }
        async Task<Class_Ref_Phys> IRefPhys.getSpecificRefPhys(int id)
        {
            var result = await _context.RefPhys.FirstOrDefaultAsync(u => u.Id == id);
            return result;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}
