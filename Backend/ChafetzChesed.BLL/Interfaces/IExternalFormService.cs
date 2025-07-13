using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChafetzChesed.Common.Models;


namespace ChafetzChesed.BLL.Interfaces
{
    public interface IExternalFormService
    {
        Task<List<ExternalForm>> GetFormsAsync();
    }
}
