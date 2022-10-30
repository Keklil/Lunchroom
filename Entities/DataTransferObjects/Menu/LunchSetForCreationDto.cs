using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class LunchSetForCreationDto
    {
        public decimal Price { get; set; }
        public List<string> LunchSetList { get; set; } = null!;
    }
}
