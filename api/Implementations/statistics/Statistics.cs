using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using api.Helpers;
using api.Interfaces.statistics;
using Microsoft.EntityFrameworkCore;

namespace api.Implementations.statistics
{
     public class Statistics : IStatistics
    {
        private DataContext _context;
        private SpecialMaps _special;

        public Statistics(DataContext context, SpecialMaps special)
        {
            _context = context;
            _special = special;
        }

        public async Task<ClassVlad> getVladAsync(int id, int hospitalId)
        {
            var result = new ClassVlad();
            var yas = new List<double>();
            var xas = new List<string>();
            var aantal_results = 0;

            double _log_score = 0;
            double _point_log_score = 0;
            double _cum_log_score = 0;
            int _dead = 0;

            var procedures = new List<Class_Procedure>();
            var elibleProcedures = new List<Class_Procedure>();
            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
                           "WHERE SelectedSurgeon = " + id + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
                                      "WHERE SelectedSurgeon = " + id).ToList();
            }
            // filter the procedures that are eligible for consideration
            foreach (Class_Procedure p in procedures)
            {
                if (_special.getEligibleForEuroscoreCalculation(p.fdType)) { elibleProcedures.Add(p); }
            }


            foreach (Class_Procedure p in elibleProcedures)
            {
                if (await this.IsDischargedAsync(p.ProcedureId))// filter out patients that are still admitted
                { 
                    _log_score = await getLogScoreAsync(p.PatientId);
                    _dead = await getDead(p.PatientId);
                    if (_log_score != 0.0)
                    {
                        if (await calculate_bonus_malusAsync(_dead, p.ProcedureId))
                        { _point_log_score = _log_score / 100;} // get credit for not killing the patient
                        else
                        { _point_log_score = (_log_score - 100) / 100; // surgical death
                        };
                        _cum_log_score = _cum_log_score + _point_log_score;

                        yas.Add(Math.Round(_cum_log_score, 2));
                        xas.Add(aantal_results.ToString());
                        aantal_results = aantal_results + 1;
                    }
                }
            }
            result.caption = "vlad";
            result.dataXas = xas.ToArray();
            result.dataYas = yas.ToArray();
            return result;
        }
        private async Task<bool> calculate_bonus_malusAsync(int dead, int Id)
        {
            var help = true;
            if (dead == 2) { help = await IsSurgicalDeathAsync(Id);}
            return help;
        }
        private async Task<bool> IsDischargedAsync(int Id)
        {
            var help = false;
            var javascriptStart = new DateTime(1970, 01, 01);
            // check and see if this record exists, if not do nothing it will come sometime anyway
            if (await _context.PostOps.AnyAsync(u => u.PROCEDURE_ID == Id))
            {
                var test = await _context.PostOps.FirstOrDefaultAsync(x => x.PROCEDURE_ID == Id);

                if ((test.mortality_date.Date - javascriptStart.Date).Days > 0) // there is a mortality date filled
                {
                    help = true;
                }
                else
                {
                    if ((test.DISCHARGE_DATE.Date - javascriptStart.Date).Days > 0) // there is a mortality date filled
                    {
                        help = true;
                    }
                }
            }
            return help;
        }
        private async Task<bool> IsSurgicalDeathAsync(int Id)
        {
            var javascriptStart = new DateTime(1970, 01, 01);
            var help = false;
            var selectedProcedure = await _context.Procedures.FirstOrDefaultAsync(x => x.ProcedureId == Id);
            var selectedPostOP = await _context.PostOps.FirstOrDefaultAsync(x => x.PROCEDURE_ID == Id);

            if ((selectedPostOP.mortality_date.Date - selectedProcedure.DateOfSurgery.Date).Days > 30)
            {// dead after more then 30 days so NO surgical mortality
                help = false;
            }
            else
            {
                help = true;
            };

            if (selectedPostOP.activities_discharge != null || selectedPostOP.activities_discharge != "") // patient was dicharged so no surgical death
            { 
                help = false;
            }
            return help;
        }
        private async Task<double> getLogScoreAsync(int patientId)
        {
            double result = 0.0;
            if (await _context.Patients.AnyAsync(u => u.PatientId == patientId))
            {
                var sp = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == patientId);
                var test = sp.log_score; // is iets als 1.34 %
                result = _special.getDoubleFromLogScore(sp.log_score);
            }
            return result;
        }
        private async Task<int> getDead(int patientId)
        {
            int result = 0;
            if (await _context.Patients.AnyAsync(u => u.PatientId == patientId))
            {
                var sp = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == patientId);
                result = sp.dead;
            }
            return result;
        }



    }
}