using System;

namespace api.DTOs
{
    public class EpaDetailsDto
    {
    public int EpaId {get; set;}
    public string name {get; set;}
    public string category {get; set;}
    public int year {get; set;}
    public DateTime created {get; set;}
    public string image {get; set;}
    public int Id {get; set;}
    public int userId {get; set;}
    public DateTime started {get; set;}
    public bool finished {get; set;}
    public string grade {get; set;}
    public bool KBP {get; set;}
    public bool OSATS {get; set;}
    public bool Beoordeling_360 {get; set;}
    public bool CAT_CAL {get; set;}
    public bool Examen {get; set;}
    public string option_6 {get; set;}
    public string option_7 {get; set;}
    }
}