using System;
using System.Text.Json.Serialization;

namespace ChafetzChesed.Common.DTOs
{
    public class CreateDepositDto
    {
        [JsonPropertyName("depositTypeId")]
        public int DepositTypeID { get; set; }

        public decimal? Amount { get; set; }
        public string? PurposeDetails { get; set; }
        public bool IsDirectDeposit { get; set; }

        public DateTime? DepositDate { get; set; }
        public DateTime? DepositReceivedDate { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
