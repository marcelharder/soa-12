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
        public string To { get; set; }
        public string Text { get; set; }
    }
}
    
