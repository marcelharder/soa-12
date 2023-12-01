using System;
using Microsoft.AspNetCore.Http;

namespace api.DTOs
{
    public class PdfForCreationDto
    {
        public int documentId { get; set; }
        public string description { get; set; }
        public DateTime dateAdded { get; set; }
        public int type { get; set; }
        public Boolean finished { get; set; }
        public string document_url { get; set; }
        public string publicId { get; set; }
        public int userId { get; set; }

        public PdfForCreationDto()
        {
            dateAdded = DateTime.Now;
        }


    }
}