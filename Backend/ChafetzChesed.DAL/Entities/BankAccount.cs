using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChafetzChesed.DAL.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string RegistrationId { get; set; } 
        public string BankNumber { get; set; }
        public string BranchNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountOwnerName { get; set; }
        public bool HasDirectDebit { get; set; }
    }
}
