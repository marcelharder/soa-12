using System;

namespace api.DTOs
{
    public class PostOpDetailsDTO
    {
        public virtual int id { get; set; }
        public int patient_id { get; set; }
        public int procedure_id { get; set; }
        public DateTime ICU_ARRIVAL_DATE { get; set; }
        public int ICU_ARRIVAL_DATE_HOURS { get; set; }
         public int ICU_ARRIVAL_DATE_MINUTES { get; set; }
        public DateTime EXTUBATION_DATE { get; set; }
        public int EXTUBATION_DATE_HOURS { get; set; }
        public int EXTUBATION_DATE_MINUTES { get; set; }
        public DateTime DISCHARGE_DATE { get; set; }
        public DateTime ICU_DISCHARGE_DATE { get; set; }
        public int ICU_DISCHARGE_DATE_HOURS { get; set; }
        public int ICU_DISCHARGE_DATE_MINUTES { get; set; }
        public DateTime ICU_ARRIVAL_1_DATE { get; set; }
        public DateTime EXTUBATION_1_DATE { get; set; }
        public DateTime ICU_DISCHARGE_1_DATE { get; set; }
        public DateTime REINTUBATION_DATE { get; set; }
        
        public string icu_stay_1 { get; set; }
        public string icu_stay_2 { get; set; }
        public string icu_stay_3 { get; set; }
        public string vent_stay_1 { get; set; }
        public string vent_stay_2 { get; set; }
        public string vent_stay_3 { get; set; }
        public string blood_products { get; set; }
        public string autologous_Blood { get; set; }
        public int pc { get; set; }
        public int ffp { get; set; }
        public int platelets { get; set; }
        public string when_used { get; set; }
        public string complicatie_1 { get; set; }
        public string complicatie_2 { get; set; }
        public string complicatie_3 { get; set; }
        public string complicatie_4 { get; set; }
        public string complicatie_5 { get; set; }
        public string complicatie_6 { get; set; }
        public string complicatie_7 { get; set; }
        public string complicatie_8 { get; set; }
        public string complicatie_9 { get; set; }
        public string activities_discharge { get; set; }
        public string discharge_diagnosis { get; set; }
        public string full_description { get; set; }
        public string sent_to { get; set; }
        public string overleden_na_deze_operatie { get; set; }
        public int dead_location { get; set; }
        public string dead_cause { get; set; }
        public string mortality_date_string { get; set; }
        public string highest_creatinine { get; set; }
        public string readmitted { get; set; }
        public string reintubated { get; set; }

    }

}