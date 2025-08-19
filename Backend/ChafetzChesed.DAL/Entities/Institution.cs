namespace ChafetzChesed.DAL.Entities
{
    public class Institution
    {
        public int ID { get; set; }
        public string Subdomain { get; set; } = "";   
        public string Name { get; set; } = "";
        public string? LogoUrl { get; set; }
        public string? ThemeColor { get; set; }
        public bool IsActive { get; set; }

    }
}
