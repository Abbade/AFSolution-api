﻿using AFSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Application.DTOs
{
    public class UserDTO : BaseDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Guid RoleId { get; set; }
      //  public Role Profile { get; set; }
    }
}
