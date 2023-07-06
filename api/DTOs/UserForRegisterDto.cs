using System;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(80, MinimumLength = 6, ErrorMessage="Password should be minimum 6 and max 80 char")]
        public string password { get; set; }
        public string country {get; set;}
        public string city {get; set;}
        [Required] public string currentHospital {get; set;}
        public string email {get; set;}
        public string gender {get; set;}
        public string knownAs {get; set;}
        public string mobile {get; set;}
        public bool active {get; set;}
        public bool ltk {get; set;}
       
       

    }
}