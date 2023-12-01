namespace api.Entities
{
    public class Class_Aortic_Surgery
    {
      public int Id { get; set; }
      public int procedure_id { get; set; }
      public bool aneurysm{get; set;}
      public string aneurysm_type{get; set;}
      public bool dissection{get; set;}
      public string dissection_onset{get; set;}
      public string dissection_type{get; set;}
      public bool coarctation{get; set;}
      public bool other_congenital{get; set;}
      public string pathology{get; set;}
      public string indication{get; set;}
      public string operative_technique{get; set;}
      public string range{get; set;}
      public string stent_graft_technique{get; set;}
    }
}