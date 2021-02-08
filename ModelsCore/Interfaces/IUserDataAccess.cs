using ModelsCore;
using System.Collections.Generic;

namespace ModelsCore.Interfaces
{
    public interface IUserDataAccess
    {
        IEnumerable<UserDto> GetAll();
    }
}