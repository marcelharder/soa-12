using System;


namespace api.DTOs
{
    public class UserForUpdateDto
    {
        public int Id { get; set; }
        public int hospital_id { get; set; }
        public string worked_in { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string gender { get; set; }
        public string photoUrl { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public DateTime paidTill { get; set; }
        public string introduction { get; set; }
        public string lookingFor { get; set; }
        public string email {get; set;}
        public string interests { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string knownAs {get; set;}
        public string iban { get; set; }
        public string mobile { get; set; }
        public string databaseRole { get; set; }
        public string country { get; set; }
        public bool active { get; set; }
        public bool ltk { get; set; }
    }
}