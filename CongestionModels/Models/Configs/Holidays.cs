using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongestionModels.Models.Configs
{
    public record Holidays
    {
        public List<DateOnly> Dates { get; set; }
    }
}
