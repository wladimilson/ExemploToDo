using System;
using System.Linq;
using System.Collections.Generic;
using ToDo.Data;
using ToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Repositories
{
    public class TaskRepository : ITaskRepository, IDisposable
    {
        private readonly ToDoContext _context;
        public TaskRepository(ToDoContext context)
        {
            this._context = context;
        }

        public List<Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Task GetById(long id)
        {
            return _context.Tasks.Find(id);
        }

        public bool Remove(long id)
        {
            var task = _context.Tasks.Find(id);
            if(task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Task Save(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return task;
        }

        public Task Update(Task task)
        {
            var entity = _context.Tasks.Find(task.Id);
            entity.Name = task.Name;
            entity.IsComplete = task.IsComplete;
            _context.SaveChanges();

            return task;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}