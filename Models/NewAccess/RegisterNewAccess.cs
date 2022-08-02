using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.NewAccess
{
    public class RegisterNewAccess
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Cpf { get; set; }
        
    }
}