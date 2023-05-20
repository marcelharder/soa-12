namespace api.DTOs {
    public class CardioForReturn{
        public int id { get; set; }
        public string center_id { get; set; }
        public int registry_id { get; set; }
        public string cassette_id { get; set; }
        public string contributor_id { get; set; }
        public int patient_age { get; set; }
        public int patient_gender { get; set; }
        public string indication { get; set; }
        public int support_mode { get; set; }
        public int time_supported { get; set; }
    }
}