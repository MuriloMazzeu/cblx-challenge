using CblxChallenge.Domain.Entities;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Infrastructure
{
    public class FirestoreService : IFreighterRepository, IReceivedMineralsRepository
    {
        public FirestoreService()
        {
            Firestore = FirestoreDb.Create("cblx-challenge");
        }

        private FirestoreDb Firestore { get; }

        public async Task AddCheckoutAsync(FreighterTransportEntity entity)
        {
            await Firestore.Collection("transports").Document().CreateAsync(entity);
        }

        public async Task AddCheckinAsync(FreighterTransportEntity entity)
        {
            var data = new Dictionary<string, object>()
            {
                { "Mineral", entity.Mineral },
                { "Amount", entity.Amount },
                { "EndAt", entity.EndAt },
                { "Cost", entity.Cost },
            };

            await Firestore.Collection("transports").Document(entity.Id).UpdateAsync(data);
        }

        public async Task<IEnumerable<FreighterTransportEntity>> GetByPeriodAsync(string period)
        {
            var snap = await Firestore.Collection("transports")
                .WhereNotEqualTo("EndAt", null)
                .WhereEqualTo("Period", period)
                .GetSnapshotAsync();

            return snap.Count == 0
                ? Enumerable.Empty<FreighterTransportEntity>()
                : snap.Documents.Select(i => i.ConvertTo<FreighterTransportEntity>());
        }
    }
}
