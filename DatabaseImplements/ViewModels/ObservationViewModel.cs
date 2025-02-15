using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.ViewModels
{
    public class ObservationViewModel
    {
        public int Id { get; set; }
        public string[]? Segments { get; set; }
        public string Color { get; set; } = string.Empty;
        public List<int>? PossibleNumbers { get; set; }
        public string[]? Masks { get; set; }

        public string SequenceId { get; set; } = string.Empty;
    }
}
