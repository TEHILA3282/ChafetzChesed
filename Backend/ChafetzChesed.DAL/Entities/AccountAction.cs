using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChafetzChesed.DAL.Entities
{
    public class AccountAction
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public long Zeout { get; set; }
        public int Seder { get; set; }
        public string Perut { get; set; } = string.Empty;
        public int Important { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
