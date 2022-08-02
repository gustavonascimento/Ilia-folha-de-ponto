using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{
    public class AcceptUserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool Aceite { get; set; }

    }
}
