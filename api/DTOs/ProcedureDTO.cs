using System;

namespace api.DTOs
{
    public class ProcedureDTO
    {
        public virtual int procedureId { get; set; }
        public virtual DateTime dateOfSurgery { get; set; }
        public virtual String description { get; set; }
        public virtual String refPhys { get; set; }
        public virtual int hospital { get; set; }
        public virtual int patientId { get; set; }
        public int fdType { get; set; }
        public virtual String sequence { get; set; }
        public virtual int selectedSurgeon { get; set; }
        public virtual int selectedResponsibleSurgeon { get; set; }
        public virtual int selectedAnaesthesist { get; set; }
        public virtual int selectedAssistant { get; set; }
        public virtual int selectedPerfusionist { get; set; }
        public virtual int selectedNurse1 { get; set; }
        public virtual int selectedNurse2 { get; set; }
        public virtual Boolean surgeryBeforeNextWorkingDay { get; set; }
        public virtual int selectedTiming { get; set; }
        public virtual int selectedUrgentTiming { get; set; }
        public virtual int selectedEmergencyTiming { get; set; }
        public virtual int selectedStartHr { get; set; }
        public virtual int selectedStartMin { get; set; }
        public virtual int selectedStopHr { get; set; }
        public virtual int selectedStopMin { get; set; }
        public virtual int totalTime { get; set; }
        public virtual int selectedInotropes { get; set; }
        public virtual int selectedPacemaker { get; set; }
        public virtual int selectedPericard { get; set; }
        public virtual int selectedPleura { get; set; }
        public virtual String comment1 { get; set; }
        public virtual String comment2 { get; set; }
        public virtual String comment3 { get; set; }
    }
}