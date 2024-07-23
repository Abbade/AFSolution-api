using AFSolution.Application.DTOs;
using AFSolution.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFSolution.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<UserDTO>
    {
        public UserController(IBaseService<UserDTO> service)
            : base(service) { }

        // Você pode adicionar métodos específicos para a entidade User aqui, se necessário
    }
}
