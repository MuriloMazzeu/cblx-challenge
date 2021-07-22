using System;

namespace CblxChallenge.Domain.ViewModels
{
    public class FreighterCheckinCommand
    {
        // para uso de idempotência, log e auditoria
        public string RequestId { get; set; }
        public DateTime EndAt { get; set; }
        public string Mineral { get; set; }
        public int Amount { get; set; }
        public string Id { get; set; }
    }
}
