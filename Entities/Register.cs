using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApi.Entities
{
    public class Register
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public string DayIn { get; set; }

        public string DayOut { get; set; }

        public string AfternoonIn { get; set; }

        public string AfternoonOut { get; set; }

    }
}

