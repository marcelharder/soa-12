namespace api.DTOs
{
    public class MinInvForReturn
    {
        public int id { get; set; }
        public int procedure_id { get; set; }
        public int strategy { get; set; }
        public int primary_incision { get; set; }
        public int primary_incision_details { get; set; }
        public int number_of_incisions { get; set; }
        public int conversion_to_standard { get; set; }
        public int conversion_details { get; set; }
        public int robot { get; set; }
        public int robot_cabg { get; set; }
        public int robot_aortic { get; set; }
        public int robot_mitral { get; set; }
        public int robot_tricuspid { get; set; }
        public int robot_pulmonary { get; set; }
        public int lima_harvest { get; set; }
        public int vessel { get; set; }
        public int shunt { get; set; }
        public int lad_time { get; set; }
        public int rca_time { get; set; }
        public int cx_time { get; set; }
        public int al_time { get; set; }
        public int suture { get; set; }
        public int acute_flow { get; set; }
        public int acute_flow_details { get; set; }
        public int iabp { get; set; }
        public int iabp_why { get; set; }
        public int iabp_when { get; set; }
    }
}
