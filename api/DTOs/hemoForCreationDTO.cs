using Microsoft.AspNetCore.Http;

namespace api.DTOs
{
    public class HemoForCreationDto
    {
       public string url { get; set; } 
       public IFormFile file { get; set; }
       public string description { get; set; }
       public int cardioId { get; set; }

      
    }
}