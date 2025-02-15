using Azure.Core;
using DatabaseImplements.Contracts;
using DatabaseImplements.ViewModels;
using Microsoft.OpenApi.Any;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TrafficLightAPI.Contracts;
using TrafficLightAPI.Models;

namespace TrafficLightAPI.BusinessLogic
{
    public class SequenceLogic : ISequenceLogic
    {
        private readonly Dictionary<string, int> _segmentDictionary = new Dictionary<string, int>
    {
        { "1110111", 0 }, { "0010010", 1 }, { "1011101", 2 },
        { "1011011", 3 }, { "0111010", 4 }, { "1101011", 5 },
        { "1101111", 6 }, { "1010010", 7 }, { "1111111", 8 },
        { "1111011", 9 }
    };
        private readonly ISequenceStorage _sequenceStorage;
        public SequenceLogic(ISequenceStorage sequenceStorage)
        {
            _sequenceStorage = sequenceStorage;
        }
        public async Task<ObservationResponse> AddObservation(ObservationRequest request)
        {
            if (request == null)
                throw new Exception("Invalid request");

            if (request.Observation.Segments != null && request.Observation.Segments.Any(s => s.Length != 7))
                throw new Exception("Invalid request");

            if (request.Observation.Color != "red" && request.Observation.Color != "green")
                throw new Exception("Invalid request");

            if (request.Observation.Segments == null && request.Observation.Color == "green")
                throw new Exception("Invalid request");

            var sequence = await _sequenceStorage.GetElement(request.SequenceId.ToString());

            if (sequence.Observations.Count == 0 && request.Observation.Color == "red")
                throw new Exception("There isn't enough data");

            if (sequence.Observations.Any(ob => ob.Color == "red"))
                throw new Exception("The red observation should be the last");

            if (sequence.Observations.Any(ob => ob.Segments == request.Observation.Segments))
                throw new Exception("No solutions found");

            var observation = new ObservationViewModel
            {
                Color = request.Observation.Color,
                Segments = request.Observation.Segments,
                SequenceId = request.SequenceId.ToString()
            };

            if (observation.Color == "green")
            {
                var (possibleNumbers, masks) = ProcessGreenSegments(observation.Segments);
                observation.PossibleNumbers = possibleNumbers;
                observation.Masks = masks;

                sequence.Observations.Add(observation);

                if (sequence.Observations.Count == 1)
                {
                    sequence.Start = sequence.Observations[0].PossibleNumbers.Select(n => n.ToString()).ToList();
                    sequence.Missing = sequence.Observations[0].Masks;
                }
                else
                {
                    var copy = new List<string>(sequence.Start);

                    foreach (var number in copy)
                    {
                        if (!observation.PossibleNumbers.Any(n => n + (sequence.Observations.Count() - 1) == int.Parse(number)))
                        {
                            sequence.Start?.Remove(number.ToString());
                        }
                    }

                    for (int i = 0; i < sequence.Missing.Count(); i++)
                    {
                        char[] missingMask = sequence.Missing[i].ToCharArray();

                        for (int j = 0; j < sequence.Missing.Length; j++)
                        {
                            if (sequence.Missing[i][j] == '1' && observation.Masks[i][j] == '0')
                            {
                                missingMask[j] = '0';
                            }
                        }

                        sequence.Missing[i] = new string(missingMask);
                    }
                }
            }
            else
            {
                observation.PossibleNumbers = null;
                observation.Masks = null;
                sequence.Observations.Add(observation);

                sequence = LastNumber(sequence);
            }

            await _sequenceStorage.Update(sequence);

            if (sequence.Start.Count == 0)
                throw new Exception("No solutions found");

            if (sequence.Start.Count == 1)
            {
                sequence = LastNumber(sequence);
            }

            return new ObservationResponse { Missing = sequence.Missing, Start = sequence.Start != null ? sequence.Start.Select(x => int.Parse(x)).ToList() : null };
        }

        public async Task ClearData() =>
            await _sequenceStorage.Delete();


        public async Task<string> CreateSequence()
        {
            string sequenceId = Guid.NewGuid().ToString();
            await _sequenceStorage.Create(new SequenceViewModel { Id = sequenceId });

            return sequenceId;
        }

        private (List<int>, string[]) ProcessGreenSegments(string[] inputs)
        {
            List<List<int>> allPossibleNumbers = new List<List<int>>();
            List<string> masks = new List<string>();

            foreach (string input in inputs)
            {
                var (numbers, mask) = GetPossibleNumbersWithMask(input);
                allPossibleNumbers.Add(numbers);
                masks.Add(mask);
            }

            return (GenerateCombinations(allPossibleNumbers), masks.ToArray());
        }
        private (List<int> numbers, string mask) GetPossibleNumbersWithMask(string input)
        {
            List<int> result = new List<int>();
            bool[] maskArray = new bool[input.Length];

            if (_segmentDictionary.TryGetValue(input, out int foundNumber))
            {
                result.Add(foundNumber);
            }

            foreach (var pair in _segmentDictionary)
            {
                if (CanTransform(input, pair.Key, out bool[] currentMask))
                {
                    result.Add(pair.Value);
                    for (int i = 0; i < maskArray.Length; i++)
                    {
                        maskArray[i] |= currentMask[i];
                    }
                }
            }

            string mask = new string(maskArray.Select(b => b ? '1' : '0').ToArray());

            return (result.Distinct().ToList(), mask);
        }

        private bool CanTransform(string input, string key, out bool[] mask)
        {
            if (input.Length != key.Length)
            {
                mask = new bool[input.Length];
                return false;
            }

            bool isValid = true;
            mask = new bool[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '1' && key[i] == '0')
                {
                    isValid = false;
                    break;
                }
                if (input[i] == '0' && key[i] == '1')
                {
                    mask[i] = true;
                }
            }

            return isValid;
        }

        private List<int> GenerateCombinations(List<List<int>> allNumbers)
        {
            if (allNumbers.Count == 0) return new List<int>();

            List<int> combinations = new List<int>();
            GenerateCombinationsRecursive(allNumbers, 0, "", combinations);
            return combinations.Distinct().ToList();
        }

        private void GenerateCombinationsRecursive(List<List<int>> allNumbers, int index, string current, List<int> results)
        {
            if (index == allNumbers.Count)
            {
                results.Add(int.Parse(current));
                return;
            }

            foreach (int num in allNumbers[index])
            {
                GenerateCombinationsRecursive(allNumbers, index + 1, current + num, results);
            }
        }
        private SequenceViewModel LastNumber(SequenceViewModel sequence)
        {
            int startValue = int.Parse(sequence.Start[0]);

            sequence.Missing = new string[] { "0000000", "0000000" };

            for (int i = 0; i < sequence.Observations.Count; i++)
            {
                if (i == sequence.Observations.Count - 1)
                    break;

                var obs = sequence.Observations[i];

                int correctValue = startValue - i;
                string[] correctSegments = new string[2];

                if (correctValue < 10)
                {
                    correctSegments[0] = _segmentDictionary.FirstOrDefault(s => s.Value == 0).Key;
                    correctSegments[1] = _segmentDictionary.FirstOrDefault(s => s.Value == correctValue).Key;
                }
                else
                {
                    correctSegments[0] = _segmentDictionary.FirstOrDefault(s => s.Value == int.Parse(correctValue.ToString().First().ToString())).Key;
                    correctSegments[1] = _segmentDictionary.FirstOrDefault(s => s.Value == int.Parse(correctValue.ToString().Last().ToString())).Key;
                }

                for (int j = 0; j < obs.Segments.Length; j++)
                {
                    var segment = obs.Segments[j];
                    char[] missingMask = sequence.Missing[j].ToCharArray();

                    for (int k = 0; k < segment.Length; k++)
                    {
                        var digit = segment[k];

                        if (digit == '0' && correctSegments[j][k] == '1')
                            missingMask[k] = '1';
                    }

                    sequence.Missing[j] = new string(missingMask);
                }
            }
            return sequence;
        } 
    }
}
