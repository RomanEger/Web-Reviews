using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record ExtentedReferenceDTO
    {
        public Guid id { get; init; }
        public string title { get; init; }
        public string description { get; init; }
    }
}
