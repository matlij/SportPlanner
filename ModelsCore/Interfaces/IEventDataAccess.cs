using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelsCore.Interfaces
{
    public interface IEventDataAccess
    {
        Task<bool> Delete(string identifier);
        EventDto Get(string identifier);
        ICollection<EventDto> Get();
        ICollection<EventDto> GetByUser(string userIdentifier);
        Task<EventDto> Store(EventDto input);
        Task<EventDto> Update(EventDto input);
    }
}