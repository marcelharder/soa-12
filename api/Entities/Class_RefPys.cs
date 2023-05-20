namespace api.Entities
{
    public class Class_Ref_Phys
    {
        public virtual int Id { get; set; }
        public virtual int hospital_id { get; set; }
        public virtual string name { get; set; }
        public virtual string image { get; set; }
        public virtual string address { get; set; }
        public virtual string street { get; set; }
        public virtual string postcode { get; set; }
        public virtual string city { get; set; }
        public virtual string state { get; set; }
        public virtual string country { get; set; }
        public virtual string tel { get; set; }
        public virtual string fax { get; set; }
        public virtual string email { get; set; }
        public virtual bool send_email { get; set; }
        public virtual bool active { get; set; }
    }
}