﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accio
{
    public class Performance
    {
        public Performance(DateTime dateAndHour, int part, PerformanceAvailability availability)
        {
            DateAndHour = dateAndHour;
            Part = part;
            Availability = availability;
        }

        public DateTime DateAndHour { get; }

        public int Part { get; }

        public PerformanceAvailability Availability { get; }

        public static IEnumerable<Performance> ParsePerformances(string input) => input.Split('|').Select(ParsePerformance);

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
    }

    // https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/EntaWebShow/ShowLanding.aspx

    public enum PerformanceAvailability
    {
        Full,
        Limited,
        Available
    }
}
