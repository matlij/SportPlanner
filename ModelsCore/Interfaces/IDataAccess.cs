using ModelsCore.TaskModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelsCore.Interfaces
{
    public interface IDataAccess
    {
        Task<bool> Delete(string identifier);
        EventDto Get(string identifier);
        ICollection<EventDto> Get();
        ICollection<EventDto> GetByUser(string userIdentifier);
        Task<EventDto> Store(TaskCreateEvent input);
        Task<EventDto> Update(TaskUpdateEvent input);
    }
}