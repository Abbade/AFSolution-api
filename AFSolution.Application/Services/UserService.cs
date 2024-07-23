using AFSolution.Application.DTOs;
using AFSolution.Application.Interfaces;
using AFSolution.Domain.Entities;
using AFSolution.Domain.Interfaces;
using AutoMapper;

namespace AFSolution.Application.Services
{
    public class UserService : BaseService<User, UserDTO>, IUserService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public UserService(IBaseRepository<User> repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(repository, mapper) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<UserDTO> CreateAsync(UserDTO dto)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var entity = _mapper.Map<User>(dto);

            entity.PasswordHash = hashedPassword;

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<UserDTO>(entity);
        }


    }
}
