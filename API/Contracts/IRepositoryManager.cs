﻿using Entities.Models;
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
        Task SaveAsync();
        IGenericRepository<T> Set<T>() where T : class;
    }
}
