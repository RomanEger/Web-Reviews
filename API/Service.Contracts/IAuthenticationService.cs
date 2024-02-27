using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task CreateUserAsync(UserForRegistrationDTO userForRegistration);
        Task<TokenDTO> CreateToken(bool populateExp);
        Task<TokenDTO> RefreshToken(TokenDTO tokenDTO);
        Task<bool> ValidateUser(UserForAuthenticationDTO userForAuthentication);
        Task<UserDTO> GetUserByTokenAsync(TokenDTO tokenDTO);
    }
}
