using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        IVideoStatusesService VideoStatuses { get; }
        IUserService User { get; }
        IAuthenticationService Authentication { get; }
    }
}
