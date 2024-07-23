using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Application.DTOs
{
    public abstract class BaseDTO
    {
        public virtual Guid? Id { get; set; }
    }
}
