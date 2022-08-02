using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Allocations
{
    public class CreateAllocationModel
    {
        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string ProjectName { get; set; }
    }
}

