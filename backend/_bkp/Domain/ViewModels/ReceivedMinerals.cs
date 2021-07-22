using System;
using System.Collections.Generic;

namespace CblxChallenge.Domain.ViewModels
{
    public class ReceivedMinerals
    {
        public IEnumerable<ReceivedMineralsData> Items { get; set; }
        public IDictionary<string, double> TotalCost { get; set; }
        public IDictionary<string, int> IdlenessIndex { get; set; }
    }

    public class ReceivedMineralsData
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Mineral { get; set; }
        public double Cost { get; set; }
    }
}
