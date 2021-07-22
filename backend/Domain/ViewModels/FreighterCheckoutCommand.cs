using System;

namespace CblxChallenge.Domain.ViewModels
{
    public class FreighterCheckoutCommand
    {
        // para uso de idempotência, log e auditoria
        public string RequestId { get; set; }
        public string Type { get; set; }
        public DateTime StartAt { get; set; }
        
    }
}
