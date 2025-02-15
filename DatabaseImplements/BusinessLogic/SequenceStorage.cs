using DatabaseImplements.Contracts;
using DatabaseImplements.Models;
using DatabaseImplements.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.BusinessLogic
{
    public class SequenceStorage : ISequenceStorage
    {
        public async Task Create(SequenceViewModel sequence)
        {
            if (sequence == null)
                throw new Exception("An exception while attemping to create the sequence");
            using (TLDbContext _db = new TLDbContext())
            {
                await _db.Sequences.AddAsync(CreateModel(sequence));
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete()
        {
            using (TLDbContext _db = new TLDbContext())
            {
                _db.Sequences.RemoveRange(_db.Sequences);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<SequenceViewModel> GetElement(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("An exception while attemping to read the sequence");

            using (TLDbContext _db = new TLDbContext())
            {
                var element = await _db.Sequences.FirstOrDefaultAsync(s => s.Id == id);

                if (element == null)
                    throw new Exception("The sequence isn't found");

                return CreateModel(element);
            }
        }

        public async Task Update(SequenceViewModel sequence)
        {
            if(sequence== null)
                throw new Exception("An exception while attemping to update the sequence");

            using(TLDbContext _db = new TLDbContext())
            {
                var currentSequence = _db.Sequences.FirstOrDefault(x=>x.Id == sequence.Id);

                if(currentSequence == null)
                    throw new Exception("An exception while attemping to update the sequence");

                var model = CreateModel(sequence);
                currentSequence.Start = model.Start;
                currentSequence.Missing = model.Missing;
                currentSequence.Observations = model.Observations;

                await _db.SaveChangesAsync();
            }
        }
        private Sequence CreateModel(SequenceViewModel sequence)
        {
            var model = new Sequence();

            model.Id = sequence.Id;
            model.Start = sequence.Start != null ? string.Join(", ", sequence.Start) : null;
            model.Missing = sequence.Missing != null ? string.Join(", ", sequence.Missing) : null;
            model.Observations = CreateModel(sequence.Observations);

            return model;
        }
        private SequenceViewModel CreateModel(Sequence model)
        {
            var sequence = new SequenceViewModel();

            sequence.Id = model.Id;
            sequence.Start = model.Start != null ? model.Start.Split(", ").ToList() : null;
            sequence.Missing = model.Missing != null ? model.Missing.Split(", ") : null;
            sequence.Observations = CreateModel(model.Observations).OrderBy(ob =>ob.Id).ToList();

            return sequence;
        }

        private List<Observation> CreateModel(List<ObservationViewModel> observations)
        {
            List<Observation> models = new List<Observation>();

            foreach (var observation in observations)
            {
                models.Add(new Observation
                {
                    Id = observation.Id,
                    Color = observation.Color,
                    Masks = observation.Masks != null ? string.Join(", ", observation.Masks) : null,
                    Segments = observation.Segments != null ? string.Join(", ", observation.Segments) : null,
                    PossibleNumbers = observation.PossibleNumbers != null ?  string.Join(", ", observation.PossibleNumbers) : null,
                    SequenceId = observation.SequenceId
                });
            }

            return models;
        }
        private List<ObservationViewModel> CreateModel(List<Observation> models)
        {
            List<ObservationViewModel> observations = new List<ObservationViewModel>();

            foreach (var model in models)
            {
                observations.Add(new ObservationViewModel
                {
                    Id = model.Id,
                    Color = model.Color,
                    Masks = model.Masks != null ? model.Masks.Split(", ") : null,
                    Segments = model.Segments != null ? model.Segments.Split(", ") : null,
                    PossibleNumbers = model.PossibleNumbers != null ? model.PossibleNumbers.Split(", ").Select(n => int.Parse(n)).ToList() : null,
                    SequenceId = model.SequenceId
                }); 
            }

            return observations;
        }
    }
}
