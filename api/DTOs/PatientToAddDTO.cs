namespace api.DTOs
{
    public class PatientToAddDTO
    {
        public string mrn { get; set; }
        public int age { get; set; }
        public virtual string gender { get; set; }
        public virtual int creat_number { get; set; }
        public virtual int weight { get; set; }
        public virtual int height { get; set; }
        
    }
}