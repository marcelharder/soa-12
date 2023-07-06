using System;

namespace api.Entities
{
    public class Class_PostOp
    {
        public int Id { get; set; }
        public int PROCEDURE_ID { get; set; }
        public int PATIENT_ID { get; set; }
        public DateTime ICU_ARRIVAL_DATE { get; set; }
        public DateTime EXTUBATION_DATE { get; set; }
        public DateTime DISCHARGE_DATE { get; set; }
        public DateTime ICU_DISCHARGE_DATE { get; set; }
        public DateTime ICU_ARRIVAL_1_DATE { get; set; }
        public DateTime EXTUBATION_1_DATE { get; set; }
        public DateTime ICU_DISCHARGE_1_DATE { get; set; }
        public DateTime REINTUBATION_DATE { get; set; }
        public string ICU_Stay_1 { get; set; }
        public string ICU_Stay_2 { get; set; }
        public string ICU_Stay_3 { get; set; }
        public string Vent_Stay_1 { get; set; }
        public string Vent_Stay_2 { get; set; }
        public string Vent_Stay_3 { get; set; }
        public string Blood_Products { get; set; }
        public string Autologous_Blood { get; set; }
        public int PC { get; set; }
        public int FFP { get; set; }
        public int Platelets { get; set; }
        public string When_Used { get; set; }
        public string complicatie_1 { get; set; }
        public string complicatie_2 { get; set; }
        public string complicatie_3 { get; set; }
        public string complicatie_4 { get; set; }
        public string complicatie_5 { get; set; }
        public string complicatie_6 { get; set; }
        public string complicatie_7 { get; set; }
        public string complicatie_8 { get; set; }
        public string complicatie_9 { get; set; }
        
       
        public DateTime mortality_date { get; set; }
        public string highest_creatinine { get; set; }
        public string readmitted { get; set; }
        public string reintubated { get; set; }
        public short overleden_na_deze_operatie { get; set; }
        public short dead_location { get; set; }
        public short dead_cause { get; set; }
        public string full_description { get; set; }
        public string activities_discharge { get; set; }
        public short discharge_diagnosis { get; set; }
        public string sent_to { get; set; }


    }
}