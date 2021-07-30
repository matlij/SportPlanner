using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelsCore.Interfaces
{
    public interface IEventDataAccess
    {
        Task<bool> Delete(string identifier);

        EventDto GetById(string identifier);

        ICollection<EventDto> GetAll(string userId = null);

        Task<EventDto> Store(EventDto input);

        Task<EventDto> Update(EventDto input);
    }
}