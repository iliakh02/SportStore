using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Interfaces
{
    public interface IUrlService
    {
        string ReditectUrlForDelete(int id, int pageSize, string queries);
    }
}
