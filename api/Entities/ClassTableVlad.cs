namespace api.Entities
{
    public class ClassTableVlad
    {
        public virtual int Id { get; set; }
        public virtual int current_user_id { get; set; }
        public virtual int hospitalId { get; set; }// 0 means all
        public virtual string caption { get; set; }
        public virtual string dataXas { get; set; }
        public virtual string dataYas { get; set; }
    }
}