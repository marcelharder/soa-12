using System;

namespace api.Entities
{
    public class Class_LTX
    {

        public int Id { get; set; }
        public int PROCEDURE_ID { get; set; }
        public string Indication { get; set; }
        public string TypeOfTX { get; set; }

        public string startHr01 { get; set; }
        public string startHr02 { get; set; }
        public string startHr03 { get; set; }
        public string startHr04 { get; set; }
        public string startMin01 { get; set; }
        public string startMin02 { get; set; }
        public string startMin03 { get; set; }
        public string startMin04 { get; set; }


        public DateTime AcceptorProcedureStart  { get; set; }
        public DateTime DonorProcedureStart  { get; set; }
        public DateTime DonorStartIschemia  { get; set; }
        public DateTime DonorStartReperfusion  { get; set; }

    }
}       
