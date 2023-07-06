export interface Ltx {
    id: number;
    PROCEDURE_ID: number;
    Indication: string;
    TypeOfTX: string;
    startHr01: string;
    startHr02: string;
    startHr03: string;
    startHr04: string;
    startMin01: string;
    startMin02: string;
    startMin03: string;
    startMin04: string;
    AcceptorProcedureStart: Date;
    DonorProcedureStart: Date;
    DonorStartIschemia: Date;
    DonorStartReperfusion: Date;
    }