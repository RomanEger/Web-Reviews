using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserCommentDTO
    {
        public Guid UserCommentId { get; init; }

        public string Text { get; init; }

        public Guid UserId { get; init; }

        public Guid VideoId { get; init; }

        public string? Disadvantages { get; init; }

        public string? Advantages { get; init; }

        public DateTime CommentDate { get; init; }
    }
}
