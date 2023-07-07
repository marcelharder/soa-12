using System;
namespace api.Entities
{
    public class Class_Epa
    {
    
    public int EpaId {get; set;}
    public string name {get; set;}
    public string category {get; set;}
    public int year {get; set;}
    public Boolean finished {get; set;}
    public DateTime created {get; set;}
    public string image {get; set;}
    public DateTime date_started {get; set;}
    public DateTime date_finished {get; set;}
    public string grade {get; set;}
    public string option_1 {get; set;}
    public string option_2 {get; set;}
    public string option_3 {get; set;}
    public string option_4 {get; set;}
    public string option_5 {get; set;}
    public string option_6 {get; set;}
    public string option_7 {get; set;} 
    public AppUser user {get; set;}
    public int Id {get; set;}




    }
}