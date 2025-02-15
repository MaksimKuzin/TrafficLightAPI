using DatabaseImplements.Models;
using DatabaseImplements.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.Contracts
{
    public interface ISequenceStorage
    {
        public Task<SequenceViewModel> GetElement(string id);
        public Task Create(SequenceViewModel sequence);
        public Task Update(SequenceViewModel sequence);
        public Task Delete();

    }
}
