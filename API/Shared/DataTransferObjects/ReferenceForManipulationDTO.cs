using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ReferenceForManipulationDTO
    {
        [Required(ErrorMessage="Field title is required")]
        [Length(minimumLength: 1, maximumLength: 50, ErrorMessage ="Title length could be between 1 and 50 chars")]
        public string? title {  get; init; }
    }
}
