﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class UserDTO
    {
        public Guid UserId { get; init; }

        public string Nickname { get; init; } 

        public string Email { get; init; } 

        public string Password { get; init; } 

        public Guid UserRankId { get; init; }

        public byte[]? Photo { get; init; }

        public Userrank UserRank { get; init; }
    }
}
