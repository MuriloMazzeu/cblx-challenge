using System;

namespace CblxChallenge.Domain.ViewModels
{
    public class ReceivedMineralsQuery
    {
        // para uso de log e auditoria
        public string RequestId { get; set; }
        public DateTime Period { get; set; }
    }
}
