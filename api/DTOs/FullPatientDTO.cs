using System;


namespace api.DTOs
{
    public class FullPatientDTO
    {
        public int patientId { get; set; }
        public string mrn { get; set; }
        public int euroScoreNo { get; set; }
        public int age { get; set; }
        public virtual int soort_procedure { get; set; }
        public virtual int id { get; set; }
        public virtual int dead { get; set; }
        public virtual int gender { get; set; }
        public virtual String extra_cardiac_arteriopathy { get; set; }
        public virtual String poor_mobility { get; set; }
        public virtual String previous_cardiac_surgery { get; set; }
        public virtual String IsPreviousIntervention { get; set; }
        public virtual String copd { get; set; }
        public virtual String active_endocarditis { get; set; }
        public virtual Boolean critical_preoperative_state { get; set; }
        public virtual String diabetes_on_insulin { get; set; }
        public virtual int nyha { get; set; }
        public virtual String ccs { get; set; }
        public virtual String lvef { get; set; }
        public virtual String recent_mi { get; set; }
        public virtual String nopm { get; set; }
        public virtual int systolic_pa_pressure { get; set; }
        public virtual int timing { get; set; }
        public virtual String reason_urgent { get; set; }
        public virtual String reason_emergent { get; set; }
        public virtual int weight_of_intervention { get; set; }
        public virtual String surgery_on_thoracic_aorta { get; set; }
        public virtual int weight { get; set; }
        public virtual int height { get; set; }
        public virtual int creat_number { get; set; }
        public virtual Boolean dialysis { get; set; }
        public virtual Boolean crit_shock { get; set; }
        public virtual Boolean crit_inotropes { get; set; }
        public virtual Boolean crit_arrythmia { get; set; }
        public virtual Boolean crit_resuscitation { get; set; }
        public virtual Boolean crit_iabp { get; set; }
        public virtual Boolean crit_ventilated { get; set; }
        public virtual Boolean crit_renal_failure { get; set; }
        public virtual Boolean crit_pacemaker { get; set; }
        public virtual String log_score { get; set; }
      
       
    }
}
