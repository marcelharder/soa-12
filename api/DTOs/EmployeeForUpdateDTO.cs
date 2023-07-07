namespace api.DTOs
{
    public class EmployeeForUpdateDTO
    {
        public virtual int id { get; set; }
        public virtual string name { get; set; }
        public virtual string active { get; set; }
        public virtual string image { get; set; }
        public virtual string profession { get; set; }
        public virtual string user_name { get; set; }
        public virtual string password { get; set; }
        public virtual string liscense_to_kill { get; set; }
        public virtual int selected_hospital_id { get; set; }
    }
}
