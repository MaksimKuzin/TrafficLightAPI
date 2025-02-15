using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.ViewModels
{
    public class SequenceViewModel
    {
        public string Id { get; set; } = string.Empty;
        public List<string>? Start { get; set; }
        public string[]? Missing { get; set; }

        public List<ObservationViewModel> Observations { get; set; } = new List<ObservationViewModel>();
    }
}
