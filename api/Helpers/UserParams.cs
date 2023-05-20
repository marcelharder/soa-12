namespace api.Helpers {

public class UserParams 
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        public int Id { get; set; } 
        public string center_id { get; set; } 
        
    }
}