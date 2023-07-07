using System;

namespace api.DTOs
{
    public class ReceiveFromInventoryDTO
    {
    public int id  { get; set; }
    public string naam { get; set; }
    public string adres { get; set; }
    public string postalCode { get; set; }
    public string hospitalNo { get; set; }
    public string country { get; set; }
    public string image { get; set; }
    public string refHospitals { get; set; }
    public string standardRef { get; set; }
    public string email { get; set; }
    public string contact { get; set; }
    public string contact_image { get; set; }
    public string telephone { get; set; }
    public string fax { get; set; }
    public string logo { get; set; }
    public string mrnSample { get; set; }
     public string vendors { get; set; }
    public string rp { get; set; }
    public string smS_mobile_number { get; set; }
    public string smS_send_time { get; set; }
    public bool triggerOneMonth { get; set; }
    public bool triggerTwoMonth { get; set; }
    public bool triggerThreeMonth { get; set; }
    public string dbBackend { get; set; }
    }
}
