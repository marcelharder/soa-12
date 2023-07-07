using System.ComponentModel.DataAnnotations;
namespace api.Entities
{
    public class Class_Valve_Code
    {
        [Key]
        public virtual int codeId { get; set; }
        public string code { get; set; }
        public int soort { get; set; }
        public int valveTypeId { get; set; }
        public string description { get; set; }
        public string position { get; set; }
        public string type { get; set; }
        public Class_Hospital hospital { get; set; }
        public int hospitalId { get; set;}
        

    }
}