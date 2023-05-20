using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.interfaces.reports;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations.reports
{
      public class Suggestion : ISuggestion
    {

        SpecialReportMaps _special;
        SpecialMaps _sp;
        private readonly IHttpContextAccessor _httpContextAccessor;
        DataContext _context;

        public Suggestion(
           DataContext context,
           SpecialReportMaps special,
           SpecialMaps sp,
           IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _special = special;
            _sp = sp;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<int> AddIndividualSuggestion(Class_Suggestion cs)
        {
            _context.Suggestions.Add(cs);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<List<Class_Item>> GetAllIndividualSuggestions()
        {
            var currentUserId = _sp.getCurrentUserId();
            var help = new List<Class_Item>();
            var result = await _context.Suggestions.Where(x => x.user == currentUserId.ToString()).ToListAsync();
            foreach (Class_Suggestion sug in result) { help.Add(_special.mapSuggestionToClassItem(sug)); }
            return help;

        }
        public async Task<Class_Suggestion> GetIndividualSuggestion(int id)
        {
            var result = new Class_Suggestion();
            var check = await _context.Suggestions
                .Where(x => x.user == _sp.getCurrentUserId().ToString())
                .Where(x => x.soort == id).AnyAsync();
            if (check)
            {
                result = await _context.Suggestions
                     .Where(x => x.user == _sp.getCurrentUserId().ToString())
                     .Where(x => x.soort == id).FirstOrDefaultAsync();
            }
            else
            {
                result.user = _sp.getCurrentUserId().ToString();
                result.soort = id;
            }
            return result;
        }
        public async Task<int> updateSuggestion(Class_Suggestion cs) {  _context.Suggestions.Update(cs); if (await SaveAll()) { return 1; } else { return 0; } }
        public async Task<bool> SaveAll() { return await _context.SaveChangesAsync() > 0; }
    }

}