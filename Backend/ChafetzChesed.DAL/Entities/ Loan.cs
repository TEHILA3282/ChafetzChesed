namespace ChafetzChesed.DAL.Entities
{
    public class Loan
    {
        public int ID { get; set; }
        public string ClientID { get; set; } = string.Empty; 
        public int LoanTypeID { get; set; }
        public DateTime LoanDate { get; set; }
        public decimal Amount { get; set; }
        public int InstallmentsCount { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string? PurposeDetails { get; set; }
        public Registration? Client { get; set; }
        public LoanType? LoanType { get; set; }
    }
}
