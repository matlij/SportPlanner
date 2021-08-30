using AutoMapper;
using ModelsCore;
using ModelsCore.Interfaces;
using SportPlannerIngestion.DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportPlannerIngestion.DataLayer.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly SportPlannerContext _context;
        private readonly IMapper _mapper;

        public UserDataAccess(SportPlannerContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _context.Users.ToList();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
