using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IGenericRepository<Videostatus> VideoStatuses { get; }
        IGenericRepository<Videotype> VideoType { get; }
        IGenericRepository<Videorestriction> VideoRestriction { get; }
        IGenericRepository<Videogenre> VideoGenre { get; }
        IGenericRepository<Userrank> UserRank { get; }
        IUserRepository User {  get; }
        IVideoRepository Video { get; }
        IUserCommentsRepository UserComments { get; }
        Task SaveAsync();
        IGenericRepository<T> Set<T>() where T : class;
    }
}
