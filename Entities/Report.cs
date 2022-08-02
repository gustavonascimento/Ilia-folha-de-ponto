using System;
namespace WebApi.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public string Mounth { get; set; }

        public string WorkedHours { get; set; }

        public string ExtendedHours { get; set; }

        public string DueHours { get; set; }

        public Register Register { get; set; }

        public Allocation Allocation { get; set; }

    }
}

