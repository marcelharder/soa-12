using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace api.Entities
{
    public class Class_Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string MRN {get; set;}
        public int EuroScoreNo {get; set;}
        public int Age {get; set;}
        public virtual int soort_procedure { get; set; }
        public virtual int Id { get; set; }
        public virtual int dead { get; set; }
        public virtual String gender { get; set; }
        public virtual String extra_cardiac_arteriopathy { get; set; }
        public virtual String poor_mobility { get; set; }
        public virtual String previous_cardiac_surgery { get; set; }
        public virtual String IsPreviousIntervention { get; set; }
        public virtual String copd { get; set; }
        public virtual String active_endocarditis { get; set; }
        public virtual Boolean critical_preoperative_state { get; set; }
        public virtual String diabetes_on_insulin { get; set; }
        public virtual String NYHA { get; set; }
        public virtual String CCS { get; set; }
        public virtual String LVEF { get; set; }
        public virtual String recent_mi { get; set; }
        public virtual String NOPM { get; set; }
        public virtual String systolic_pa_pressure { get; set; }
        public virtual String timing { get; set; }
        public virtual String reason_urgent { get; set; }
        public virtual String reason_emergent { get; set; }
        public virtual String weight_of_intervention { get; set; }
        public virtual String surgery_on_thoracic_aorta { get; set; }
        public virtual String weight { get; set; }
        public virtual String height { get; set; }
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
        public ICollection<Class_Procedure> procedures { get; set; }
       
        public Class_Patient()
        {
            procedures = new Collection<Class_Procedure>();
        }
       
    }
}