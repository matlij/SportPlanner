using ModelsCore.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelsCore.Interfaces
{
    public interface IEventDataAccess
    {
        EventDto GetById(string identifier);

        ICollection<EventDto> GetAll(string userId = null);

        Task<(CrudResult crudResult, EventDto eventDto)> Store(EventDto input);

        Task<CrudResult> Update(string id, EventDto input);

        Task<CrudResult> Delete(string identifier);
    }
}