namespace ChafetzChesed.DAL.Entities
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
