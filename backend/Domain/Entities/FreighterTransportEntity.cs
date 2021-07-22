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
        public double? Cost { get; set; }

        [FirestoreProperty]
        public int? Amount { get; set; }

        [FirestoreProperty]
        public string Period { get; set; }

        public void CalculateCost(double price)
        {
            if (!Amount.HasValue) throw new InvalidOperationException();
            Cost = Amount.Value * price;
        }
    }
}
