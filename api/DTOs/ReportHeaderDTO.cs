using System;

namespace api.DTOs
{
    public class ReportHeaderDTO
    {
        public virtual int Id { get; set; }
        public virtual string hospital_name { get; set; }
        public virtual string hospital_image { get; set; }
        public virtual string hospital_unit { get; set; }
        public virtual string hospital_dept { get; set; }
        public virtual string hospital_city { get; set; }
        public virtual string hospital_number { get; set; }
        public virtual string patient_name { get; set; }
        public virtual string physician { get; set; }
        public virtual string clinical_unit { get; set; }
        public virtual string title { get; set; }
        public virtual string diagnosis { get; set; }
        public virtual string operation { get; set; }
        public virtual DateTime operation_date { get; set; }
        public virtual string surgeon { get; set; }
        public virtual string surgeon_picture { get; set; }
        public virtual string assistant { get; set; }
        public virtual string anaesthesiologist { get; set; }
         public virtual string perfusionist { get; set; }
        public virtual string Comment_1 { get; set; }
        public virtual string Comment_2 { get; set; }
        public virtual string Comment_3 { get; set; }

    }
}
