export interface Valve {
    Id: number;
    Hospitalno: number;
    Implant_Position: string, 
    IMPLANT: string, 
    EXPLANT: string, 
    SIZE: string, 
    TYPE: string, 
    SIZE_EXP: string, 
    TYPE_EXP: number, 
    ProcedureType: number, 
    ProcedureAetiology: number, 
    valveDescription: string,
    MODEL: string, 
    MODEL_EXP: string, 
    SERIAL_IMP: string, 
    SERIAL_EXP: string, 
    RING_USED: string, 
    REPAIR_TYPE: string, 
    Memo: string, 
    Combined: number, 
    procedure_id: number;
}
