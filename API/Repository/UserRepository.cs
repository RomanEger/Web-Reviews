using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(WebReviewsContext webReviewsContext) 
            : base(webReviewsContext) { }

        public void CreateUser(User user) =>
            Create(user);

        public void DeleteUser(User user) =>
            Delete(user);

        public async Task<User> GetUserAsync(Guid userId, bool trackChanges) =>
            await FindByConditions(x => x.UserId == userId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<User> GetUserByEmailAsync(string email, bool trackChanges) =>
            await FindByConditions(x => x.Email == email, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<User> GetUserByNicknameAsync(string nickname, bool trackChanges) =>
            await FindByConditions(x => x.Nickname.ToLower() == nickname.ToLower(), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<User>> GetUsersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .ToListAsync();
    }
}
