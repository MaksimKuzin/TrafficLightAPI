using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public string? Segments {  get; set; }
        public string Color {  get; set; } = string.Empty;
        public string? PossibleNumbers { get; set; }
        public string? Masks { get; set; }

        public string SequenceId { get; set; } = string.Empty;  
        public Sequence Sequence { get; set; } = new();

    }
}
