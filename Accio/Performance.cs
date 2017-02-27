using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accio
{
    public class Performance : IEquatable<Performance>
    {
        public Performance(DateTime dateAndHour, int part, PerformanceAvailability availability)
        {
            DateAndHour = dateAndHour;
            Part = part;
            Availability = availability;
        }

        public DateTime DateAndHour { get; }

        public int Part { get; }

        public string PartLabel => $"Part {Part}";

        public PerformanceAvailability Availability { get; }

        public static IEnumerable<Performance> ParsePerformances(string input)
            => input.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(ParsePerformance);

        public static Performance ParsePerformance(string input)
        {
            // 01/02/2017 14:00:00;Part One\n14:00 ;206622;0;1;FL1
            var splittedInput = input.Split(';');
            var dateAndHour = DateTime.Parse(splittedInput[0]);
            var part = int.Parse(splittedInput.Last().Substring(2));
            var availability = ParseAvailability(splittedInput[3], splittedInput[4]);

            return new Performance(dateAndHour, part, availability);
        }

        private static PerformanceAvailability ParseAvailability(string field1, string field2)
        {
            switch (field1 + field2)
            {
                case "11":
                    return PerformanceAvailability.Available;
                case "10":
                    return PerformanceAvailability.Limited;
                default:
                    return PerformanceAvailability.Full;
            }
        }

        public string ToListItem()
            => string.Join("\t", DateAndHour.ToString("ddd"), DateAndHour.ToShortDateString(), DateAndHour.ToShortTimeString(), PartLabel, Availability);

        public bool Equals(Performance other) => DateAndHour == other?.DateAndHour && Part == other?.Part;

        public override bool Equals(object obj) => Equals(obj as Performance);

        public override int GetHashCode() => DateAndHour.GetHashCode() + Part.GetHashCode();

        public static bool operator ==(Performance left, Performance right) => Equals(left, right);

        public static bool operator !=(Performance left, Performance right) => !Equals(left, right);
        
        private static bool Equals(Performance left, Performance right) => left?.Equals(right) ?? (right == null);
    }

    public enum PerformanceAvailability
    {
        Full,
        Limited,
        Available
    }
}
