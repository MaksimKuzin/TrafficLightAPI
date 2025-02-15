using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.Models
{
    public class Sequence
    {
        public string Id { get; set; } = string.Empty;
        public string? Start { get;set; }
        public string? Missing { get;set; }

        public List<Observation> Observations { get; set; } = new List<Observation>();

    }
}
