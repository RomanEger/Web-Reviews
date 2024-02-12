using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync(bool trackChanges);
        Task<UserDTO> GetUserByIdAsync(Guid userId, bool trackChanges);
        Task<UserDTO> UpdateUserAsync(Guid userId, UserForUpdateDTO userForUpdate, bool trackChanges);
        Task DeleteUserAsync(Guid userId, bool trackChanges);
    }
}
