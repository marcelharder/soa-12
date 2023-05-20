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
    public class ElementaryStatistics : IElementaryStatistics
    {
        private SpecialMaps _sp;
        private DataContext _context;
        private List<Class_Procedure> procedures;

        public ElementaryStatistics(SpecialMaps sp, DataContext context)
        {
            _sp = sp;
            _context = context;
            procedures = new List<Class_Procedure>();
        }
        public async Task<ClassVlad> getAgeDistributionPerHospital(int userId, int hospitalId)
        {
            var help = new List<string>();
            var helpDouble = new List<double>();
            var result = new ClassVlad();

            var list_of_ages = new List<int>();


            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
               "WHERE SelectedSurgeon = " + userId + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
            "WHERE SelectedSurgeon = " + userId).ToList();

            }




            foreach (Class_Procedure cp in procedures)
            {
                Class_Patient p = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == cp.PatientId);
                list_of_ages.Add(p.Age);
            }

            await Task.Run(() =>
            {

                result.caption = "Age distribution in hospital";
                help.Add("0-18");
                help.Add("18-30"); help.Add("31-40"); help.Add("41-50"); help.Add("51-60");
                help.Add("61-70"); help.Add("71-80"); help.Add("81-90");
                result.dataXas = help.ToArray();
                helpDouble.Add(getAge(0, list_of_ages));
                helpDouble.Add(getAge(1, list_of_ages));
                helpDouble.Add(getAge(2, list_of_ages));
                helpDouble.Add(getAge(3, list_of_ages));
                helpDouble.Add(getAge(4, list_of_ages));
                helpDouble.Add(getAge(5, list_of_ages));
                helpDouble.Add(getAge(6, list_of_ages));
                helpDouble.Add(getAge(7, list_of_ages));
                result.dataYas = helpDouble.ToArray();

            });
            return result;
        }
        public async Task<ClassVlad> getCaseMixPerHospital(int userId, int hospitalId)
        {
            var help = new List<string>();
            var list_of_soort = new List<int>();
            var helpDouble = new List<double>();
            var result = new ClassVlad();

            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
               "WHERE SelectedSurgeon = " + userId + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
            "WHERE SelectedSurgeon = " + userId).ToList();

            }

            foreach (Class_Procedure cp in procedures) { list_of_soort.Add(cp.fdType); }
            await Task.Run(() =>
            {
                result.caption = "CaseMix (ALL)";

                help.Add("CABG lima/vsm");
                help.Add("CABG lima/radial");
                help.Add("AVR");
                help.Add("MVR");
                help.Add("AVR/CABG");
                help.Add("MVR/CABG");
                help.Add("MVP");
                help.Add("Other");
                result.dataXas = help.ToArray();

                helpDouble.Add(getSoort(1, list_of_soort));
                helpDouble.Add(getSoort(2, list_of_soort));
                helpDouble.Add(getSoort(3, list_of_soort));
                helpDouble.Add(getSoort(4, list_of_soort));
                helpDouble.Add(getSoort(5, list_of_soort));
                helpDouble.Add(getSoort(6, list_of_soort));
                helpDouble.Add(getSoort(7, list_of_soort));
                helpDouble.Add(getSoort(8, list_of_soort));

                result.dataYas = helpDouble.ToArray();

            });
            return result;
        }
        public async Task<ClassVlad> getCasesPerMonthPerHospital(int currentYear, int userId, int hospitalId)
        {
            var result = new ClassVlad();
            var help_x = new List<string>();
            var helpDouble = new List<double>();
            var list_of_surgery_date = new List<DateTime>();


            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
               "WHERE SelectedSurgeon = " + userId + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
            "WHERE SelectedSurgeon = " + userId).ToList();

            }

            foreach (Class_Procedure cp in procedures) 
            {
                if(cp.DateOfSurgery.Year == currentYear)
                {
                    list_of_surgery_date.Add(cp.DateOfSurgery);
                } 
             }

            await Task.Run(() =>
            {
                result.caption = "Cases per Month in (" + currentYear + ")";
                help_x.Add("jan");
                help_x.Add("feb");
                help_x.Add("mar");
                help_x.Add("apr");
                help_x.Add("may");
                help_x.Add("jun");
                help_x.Add("jul");
                help_x.Add("aug");
                help_x.Add("sep");
                help_x.Add("oct");
                help_x.Add("nov");
                help_x.Add("dec");
                result.dataXas = help_x.ToArray();

                helpDouble.Add(getCasesPerMonth(1, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(2, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(3, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(4, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(5, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(6, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(7, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(8, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(9, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(10, list_of_surgery_date));
                helpDouble.Add(getCasesPerMonth(11, list_of_surgery_date));
                result.dataYas = helpDouble.ToArray();
            });

            return result;
        }
        public async Task<ClassVlad> getCasesPerYearPerHospital(int userId, int hospitalId)
        {
            var result = new ClassVlad();
            var help_x = new List<string>();
            var helpDouble = new List<double>();
            var list_of_surgery_date = new List<DateTime>();

            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
               "WHERE SelectedSurgeon = " + userId + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
            "WHERE SelectedSurgeon = " + userId).ToList();

            }

            foreach (Class_Procedure cp in procedures) { list_of_surgery_date.Add(cp.DateOfSurgery); }

            await Task.Run(() =>
            {
                result.caption = "Cases per Year";
                help_x.Add("2020");
                help_x.Add("2021");
                help_x.Add("2022");
                help_x.Add("2023");
                help_x.Add("2024");
                help_x.Add("2025");

                result.dataXas = help_x.ToArray();

                helpDouble.Add(getCasesPerYear(1, list_of_surgery_date));
                helpDouble.Add(getCasesPerYear(2, list_of_surgery_date));
                helpDouble.Add(getCasesPerYear(3, list_of_surgery_date));
                helpDouble.Add(getCasesPerYear(4, list_of_surgery_date));
                helpDouble.Add(getCasesPerYear(5, list_of_surgery_date));
                helpDouble.Add(getCasesPerYear(6, list_of_surgery_date));

                result.dataYas = helpDouble.ToArray();
            });

            return result;

        }
        public async Task<ClassVlad> getRiskBandsPerHospital(int userId, int hospitalId)
        {
            var result = new ClassVlad();
            var help_x = new List<string>();
            var helpDouble = new List<double>();
            var list_of_risk = new List<double>();
            var list_patientIds = new List<int>();

            if (hospitalId != 0)
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
               "WHERE SelectedSurgeon = " + userId + " AND hospital = " + hospitalId).ToList();
            }
            else
            {
                procedures = _context.Procedures.FromSqlRaw("SELECT * FROM Procedures " +
            "WHERE SelectedSurgeon = " + userId).ToList();

            }

            foreach (Class_Procedure cp in procedures)
            {
                list_patientIds.Add(cp.PatientId);
            }

            foreach (int p in list_patientIds)
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == p);
                list_of_risk.Add(_sp.getDoubleFromLogScore(patient.log_score));
            }

            await Task.Run(() =>
            {
                result.caption = "EuroScore in 5 Riskbands";
                help_x.Add("0-3");
                help_x.Add("3-6");
                help_x.Add("6-9");
                help_x.Add("9-12");
                help_x.Add(">12");

                result.dataXas = help_x.ToArray();

                helpDouble.Add(getRiskBands(1, list_of_risk));
                helpDouble.Add(getRiskBands(2, list_of_risk));
                helpDouble.Add(getRiskBands(3, list_of_risk));
                helpDouble.Add(getRiskBands(4, list_of_risk));
                helpDouble.Add(getRiskBands(5, list_of_risk));

                result.dataYas = helpDouble.ToArray();
            });

            return result;
        }


        private double getAge(int no, List<int> list_of_ages)
        {
            var help = 0.0;
            switch (no)
            {
                case 0: foreach (int a in list_of_ages) { if (0 < a && a < 17) { help++; } }; break;
                case 1: foreach (int a in list_of_ages) { if (18 < a && a < 30) { help++; } }; break;
                case 2: foreach (int a in list_of_ages) { if (31 < a && a < 40) { help++; } }; break;
                case 3: foreach (int a in list_of_ages) { if (41 < a && a < 50) { help++; } }; break;
                case 4: foreach (int a in list_of_ages) { if (51 < a && a < 60) { help++; } }; break;
                case 5: foreach (int a in list_of_ages) { if (61 < a && a < 70) { help++; } }; break;
                case 6: foreach (int a in list_of_ages) { if (71 < a && a < 80) { help++; } }; break;
                case 7: foreach (int a in list_of_ages) { if (81 < a && a < 90) { help++; } }; break;

            }
            return help;
        }
        private double getSoort(int no, List<int> list_of_soort)
        {
            var help = 0.0;
            var common = new List<int>();
            common.Add(1); common.Add(24); common.Add(3); common.Add(4); common.Add(80); common.Add(82); common.Add(41);
            switch (no)
            {
                case 1: foreach (int a in list_of_soort) { if (a == 1) { help++; } }; break;
                case 2: foreach (int a in list_of_soort) { if (a == 24) { help++; } }; break;
                case 3: foreach (int a in list_of_soort) { if (a == 3) { help++; } }; break;
                case 4: foreach (int a in list_of_soort) { if (a == 4) { help++; } }; break;
                case 5: foreach (int a in list_of_soort) { if (a == 80) { help++; } }; break;
                case 6: foreach (int a in list_of_soort) { if (a == 82) { help++; } }; break;
                case 7: foreach (int a in list_of_soort) { if (a == 41) { help++; } }; break;
                case 8: foreach (int a in list_of_soort) { if (!common.Contains(a)) { help++; } }; break;
            }
            return help;
        }
        private double getCasesPerMonth(int no, List<DateTime> list_of_dates)
        {
            var help = 0.0;
            switch (no)
            {
                case 1: foreach (DateTime a in list_of_dates) { if (a.Month == 1) { help++; } }; break;
                case 2: foreach (DateTime a in list_of_dates) { if (a.Month == 2) { help++; } }; break;
                case 3: foreach (DateTime a in list_of_dates) { if (a.Month == 3) { help++; } }; break;
                case 4: foreach (DateTime a in list_of_dates) { if (a.Month == 4) { help++; } }; break;
                case 5: foreach (DateTime a in list_of_dates) { if (a.Month == 5) { help++; } }; break;
                case 6: foreach (DateTime a in list_of_dates) { if (a.Month == 6) { help++; } }; break;
                case 7: foreach (DateTime a in list_of_dates) { if (a.Month == 7) { help++; } }; break;
                case 8: foreach (DateTime a in list_of_dates) { if (a.Month == 8) { help++; } }; break;
                case 9: foreach (DateTime a in list_of_dates) { if (a.Month == 9) { help++; } }; break;
                case 10: foreach (DateTime a in list_of_dates) { if (a.Month == 10) { help++; } }; break;
                case 11: foreach (DateTime a in list_of_dates) { if (a.Month == 11) { help++; } }; break;
                case 12: foreach (DateTime a in list_of_dates) { if (a.Month == 12) { help++; } }; break;

            }
            return help;
        }
        private double getCasesPerYear(int no, List<DateTime> list_of_dates)
        {
            var help = 0.0;
            switch (no)
            {
                case 1: foreach (DateTime a in list_of_dates) { if (a.Year == 2020) { help++; } }; break;
                case 2: foreach (DateTime a in list_of_dates) { if (a.Year == 2021) { help++; } }; break;
                case 3: foreach (DateTime a in list_of_dates) { if (a.Year == 2022) { help++; } }; break;
                case 4: foreach (DateTime a in list_of_dates) { if (a.Year == 2023) { help++; } }; break;
                case 5: foreach (DateTime a in list_of_dates) { if (a.Year == 2024) { help++; } }; break;
                case 6: foreach (DateTime a in list_of_dates) { if (a.Year == 2025) { help++; } }; break;


            }
            return help;
        }
        private double getRiskBands(int no, List<Double> list_of_risk)
        {
            var help = 0.0;
            switch (no)
            {
                case 1: foreach (Double a in list_of_risk) { if (a > 0 && a < 3) { help++; } }; break;
                case 2: foreach (Double a in list_of_risk) { if (a >= 3 && a < 6) { help++; } }; break;
                case 3: foreach (Double a in list_of_risk) { if (a >= 6 && a < 9) { help++; } }; break;
                case 4: foreach (Double a in list_of_risk) { if (a >= 9 && a < 12) { help++; } }; break;
                case 5: foreach (Double a in list_of_risk) { if (a >= 12) { help++; } }; break;
            }
            return help;
        }
    }
}