namespace ChafetzChesed.DAL.Entities
{
    public class AuditLog
    {
        public int ID { get; set; }
        public int InstitutionId { get; set; }
        public string Entity { get; set; } = "";
        public string EntityId { get; set; } = "";
        public DateTime ChangedAt { get; set; }
        public string? ChangedBy { get; set; }
        public string ChangesJson { get; set; } = "";
    }
}