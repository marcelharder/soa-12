using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class smsDTO
    {
       
        public string From { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string api_id { get; set; }
        
    }
}
    
