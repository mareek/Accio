using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Accio.Test
{
    public class PerformanceTest
    {
        [Fact]
        public void WhenParseFullPartOneOnWensdayAfternooThenReturnCorrectResult()
        {
            var performance = Performance.ParsePerformance("01/02/2017 14:00:00;Part One\n14:00 ;206622;0;1;FL1");
            CheckPerformanceValues(performance, new DateTime(2017, 2, 1, 14, 0, 0), 1, PerformanceAvailability.Full);
        }

        [Fact]
        public void WhenParseMultiplePartsThenReturnCorrectResult()
        {
            var performances = Performance.ParsePerformances("01/02/2017 19:30:00;Part Two\n19:30 ;206864;1;1;FL2|02/02/2017 19:30:00;Part One\n19:30 ;207105;1;0;FX1").ToList();
            CheckPerformanceValues(performances[0], new DateTime(2017, 2, 1, 19, 30, 0), 2, PerformanceAvailability.Available);
            CheckPerformanceValues(performances[1], new DateTime(2017, 2, 2, 19, 30, 0), 1, PerformanceAvailability.Limited);
        }

        private static void CheckPerformanceValues(Performance performance, DateTime dateAndHour, int part, PerformanceAvailability availability)
        {
            Assert.Equal(dateAndHour, performance.DateAndHour);
            Assert.Equal(part, performance.Part);
            Assert.Equal(availability, performance.Availability);
        }
    }
}
