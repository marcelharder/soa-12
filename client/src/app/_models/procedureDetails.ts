
export interface ProcedureDetails {
    procedureId: number;
    description: string;
    dateOfSurgery: Date;
    hospital: number;
    patientId: number;
    fdType: number;
    sequence: string;
    refPhys: string;
    selectedSurgeon: number;
    selectedResponsibleSurgeon: number;
    selectedAnaesthesist: number;
    selectedAssistant: number;
    selectedPerfusionist: number;
    selectedNurse1: number;
    selectedNurse2: number;
    surgeryBeforeNextWorkingDay: boolean;
    selectedTiming: number;
    selectedUrgentTiming: number;
    selectedEmergencyTiming: number;
    selectedStartHr: number;
    selectedStartMin: number;
    selectedStopHr: number;
    selectedStopMin: number;
    totalTime: number;
    selectedInotropes: number;
    selectedPacemaker: number;
    selectedPericard: number;
    selectedPleura: number;
    comment1: string;
    comment2: string;
    comment3: string;
}
