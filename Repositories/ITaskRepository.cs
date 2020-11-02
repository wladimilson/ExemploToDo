using System.Collections.Generic;
using ToDo.Models;

namespace ToDo.Repositories
{
    public interface ITaskRepository
    {
        List<Task> GetAll();
        Task GetById(long id);
        Task Save(Task item);
        Task Update(Task item);
        bool Remove(long id);
    }
}