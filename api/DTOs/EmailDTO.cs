namespace api.DTOs
{
    public class EmailDTO
    {
        public int id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string soort { get; set; }
        public string subject { get; set; }
        public string phone { get; set; }
        public string body { get; set; }
        public string hours { get; set; }
        public string surgeon { get; set; }
        public string surgeon_image { get; set; }
        public string ref_phys { get; set; }
        public string hospital { get; set; }
        public string callback { get; set; }
        public string message { get; set; }


    }
}
