using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ResendClass
    {
        [Required]
        public string Receiptnumber { get; set; }
    }
}
