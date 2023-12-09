namespace api.DTOs
{
    public class refphysForUpdate
    {
        public int Id { get; set; }
        public int hospital_id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public bool send_email { get; set; }
        public bool active { get; set; }
    }
}
