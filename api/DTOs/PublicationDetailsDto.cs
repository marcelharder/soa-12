using System;

namespace api.DTOs{

public class PublicationDetailsDto
{
     public int PublicationId { get; set; }
    public int userId { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Volume { get; set; }
    public string Issue { get; set; }
    public string PlaceOfPublication { get; set; }
    public string Publisher { get; set; }
    public string Editor { get; set; }
    public DateTime DateOfPublication { get; set; }
    public string URL { get; set; }
    public string DOI { get; set; }


}
}