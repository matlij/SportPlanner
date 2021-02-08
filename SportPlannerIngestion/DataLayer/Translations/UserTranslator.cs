using ModelsCore;
using SportPlannerIngestion.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportPlannerIngestion.DataLayer.Translations
{
    public static class UserTranslator
    {
        public static UserDto AsUserDto(this User user)
        {
            return new UserDto
            {
                UserId = user.Identifier,
                UserName = user.Name
            };
        }
    }
}
