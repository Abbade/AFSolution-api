using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public ICollection<User> Users { get; set;}
    }
}
