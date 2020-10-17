﻿using ModelsCore.TaskModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelsCore.Interfaces
{
    public interface IDataLayer
    {
        Task<bool> Delete(string identifier);
        EventDto Get(string identifier);
        ICollection<EventDto> GetByUser(string userIdentifier);
        Task<EventDto> Store(TaskCreateEvent input);
        Task<EventDto> Update(TaskUpdateEvent input);
    }
}