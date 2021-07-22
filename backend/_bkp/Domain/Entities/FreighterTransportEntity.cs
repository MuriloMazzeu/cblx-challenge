using Google.Cloud.Firestore;
using System;

namespace CblxChallenge.Domain.Entities
{
    [FirestoreData]
    public class FreighterTransportEntity
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Type { get; set; }

        [FirestoreProperty]
        public Timestamp StartAt { get; set; }

        [FirestoreProperty]
        public Timestamp? EndAt { get; set; }

        [FirestoreProperty]
        public string Mineral { get; set; }

        [FirestoreProperty]
        public string RecommendedMineral { get; set; }

        [FirestoreProperty]
        public double? Cost { get; set; }

        [FirestoreProperty]
        public int? Amount { get; set; }

        [FirestoreProperty]
        public string Period { get; set; }

        [FirestoreProperty]
        public string WeekPeriod { get; set; }

        public void CalculateCost(double price)
        {
            if (!Amount.HasValue) throw new InvalidOperationException();
            Cost = Amount.Value * price;
        }

        public void SetWeeklyPeriod(DateTime reference)
        {
            int week = 4, day = reference.Day;
            if (day >= 1 && day <= 7) week = 1;
            if (day >= 8 && day <= 14) week = 2;
            if (day >= 15 && day <= 21) week = 3;
            WeekPeriod = $"{reference.Year}-{reference.Month}_{week}";
        }
    }
}
