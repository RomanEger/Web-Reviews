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
        [Required(ErrorMessage= "Title поле обязательно")]
        [Length(minimumLength: 1, maximumLength: 50, ErrorMessage ="Title длина должна быть между 1 и 50")]
        public string? title {  get; init; }
    }
}
