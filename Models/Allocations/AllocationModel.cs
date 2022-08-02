using System;
namespace WebApi.Models.Allocations
{
    public class AllocationModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ProjectName { get; set; }
    }
}

