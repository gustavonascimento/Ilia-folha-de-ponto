using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Allocation
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string ProjectName { get; set; }
    }
}

