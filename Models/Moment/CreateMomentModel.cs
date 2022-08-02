using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Moment
{
    public class CreateMomentModel
    {
        [Required]
        public string DateTime { get; set; }

    }
}
