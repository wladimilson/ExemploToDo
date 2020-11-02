using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;
using ToDo.Repositories;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TodoController(ITaskRepository repository) => _repository = repository;

        [HttpGet]
        public ActionResult<List<Task>> Get() => _repository.GetAll();

        [HttpPost]
        public ActionResult<Task> Post([FromBody] Task value)
        {
            Console.WriteLine(value);
            if(value == null)
                return BadRequest();


            _repository.Save(value);

            return value;
        }

        [HttpPut("{id}")]
        public ActionResult<Task> Put(long id, Task value)
        {
            var item = _repository.GetById(id);
            if(item != null)
            {
                value.Id = id;
                value.Name = item.Name;
                _repository.Update(value);

                return value;
            }
            
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            if(_repository.Remove(id))
            {
                
                return Ok(new { Description = "Item removed" });
            }

            return BadRequest();
        }
    }
}