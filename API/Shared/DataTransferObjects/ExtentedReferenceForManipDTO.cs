using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ExtentedReferenceForManipDTO
    {
        [Required(ErrorMessage = "Title поле обязательно")]
        [Length(minimumLength: 1, maximumLength: 30, ErrorMessage = "Title length could be between 1 and 30 chars")]
        public string? title { get; init; }

        [Required(ErrorMessage = "Description поле обязательно")]
        [Length(minimumLength: 1, maximumLength: 100, ErrorMessage = "Description length could be between 1 and 100 chars")]
        public string? description { get; init; }
    }
}
