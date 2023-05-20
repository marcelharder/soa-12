using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class SubscriptionExtensionRequestDTO
    {
        [Required]
        public string userId { get; set; }
        public string additionalComments { get; set; }
        public string extensionPeriod { get; set; }
        public string subject { get; set; }
        public string to { get; set; }
        public string userName { get; set; }
    }
}