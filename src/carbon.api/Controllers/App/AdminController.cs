using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers.App
{
    public class AdminController : CarbonAuthenticatedController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public AdminController(IUserStore<IdentityUser> users, 
            IReadWriteRepository readWriteRepository, 
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
            :base(users, readOnlyRepository, mapper)
        {
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await GetUserProfile();
            if (user.CoreUserDto.Access < AccessEnum.Admin) return Unauthorized();

            var coreUsers = _readOnlyRepository.Table<CoreUser, Guid>().ToList();

            var users = new List<UserDto>();
            
            var lookUps = new List<Task<UserDto>>();
            
            foreach (var coreUser in coreUsers)
            {
                lookUps.Add(GetUserDto(coreUser));
            }

            await Task.WhenAll(lookUps);

            foreach (var lookUp in lookUps)
            {
                users.Add(lookUp.Result);
            }
            
            return Ok(users);    

        }

        private async Task<UserDto> GetUserDto(CoreUser coreUser)
        {
            var identityUser = await _users.FindByIdAsync(coreUser.Id.ToString(), new CancellationToken());
                    
            return new UserDto()
            {
                Id = coreUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                EmailConfirmed = identityUser.EmailConfirmed,
                TwoFactorEnabled = identityUser.EmailConfirmed,
                AccessFailedCount = identityUser.AccessFailedCount,
                CoreUser = _mapper.Map<CoreUserDto>(coreUser)
            };
        }
        
    }
}