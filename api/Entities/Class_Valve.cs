using System;

namespace api.Entities
{
    public class Class_Valve
    {
        public int Id { get; set; }
        public string Implant_Position {get; set;}
        public string IMPLANT { get; set; }
        public string EXPLANT { get; set; }
        public string SIZE { get; set; }
        public string TYPE { get; set; }
        public string SIZE_EXP { get; set; }
        public Nullable<int> TYPE_EXP { get; set; }
        public string ProcedureType { get; set; }
        public string ProcedureAetiology { get; set; }
        public string MODEL { get; set; }
        public string MODEL_EXP { get; set; }
        public string SERIAL_IMP { get; set; }
        public string SERIAL_EXP { get; set; }
        public string RING_USED { get; set; }
        public string REPAIR_TYPE { get; set; }
        public string Memo { get; set; }
        public string Combined { get; set; }
        public Class_Procedure procedure {get; set;}
        public int ProcedureId {get; set;}

    }
}