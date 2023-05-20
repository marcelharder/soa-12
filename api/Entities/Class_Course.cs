using System;
namespace api.Entities
{
    public class Class_Course
    {
        
        public int CourseId { get; set; }
        public string active { get; set; }
        public int level { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string diploma { get; set; }
        public string location { get; set; }
        public DateTime courseDate { get; set; }
        public DateTime endDate { get; set; }
        public float price { get; set; }
        public AppUser user {get; set;}
        public int Id { get; set; }
    }
}