using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class ButtonPerProcedureSoortDTO
    {
       public int iD { get; set; }
        public string description { get; set; }
        public List<string> button_caption { get; set; }
        public List<string> button_action { get; set; }
        public string aantal_buttons { get; set; }
        public string cpb_used { get; set; }
        public string report_code { get; set; }
        public string weight_of_intervention { get; set; }
    } 
    
}