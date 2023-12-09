using System;

namespace api.DTOs
{
    public class ProcedureListDTO
    {
        public virtual int procedureId { get; set; }
        public virtual DateTime dateOfSurgery { get; set; }
        public virtual String description { get; set; }
        public virtual String completed { get; set; }
        public virtual int patientId { get; set; }
        public int fd_Type { get; set; }
    }
}
