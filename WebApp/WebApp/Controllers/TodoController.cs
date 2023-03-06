using AutoMapper;
using BLL;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web.Models;
using WebApp.CustomMiddlewares;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        readonly IGetTodoService _gettodoService;
        readonly IMapper _mapper;
        public TodoController(IGetTodoService getTodoService, IMapper mapper)
        {            
            _gettodoService = getTodoService;
            _mapper = mapper;
        }      

        // GET: api/<TodoController>
        [HttpGet]
        [Route("List")]
        //[Route("List/Filter/{query?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoWM>))]
        async public Task<IActionResult> Get([FromQuery]TodoQueryWM query)
        {            
            var dtoList = await _gettodoService.ListAsync(_mapper.Map<TodoQueryDto>(query));
            var result = dtoList.Select(x => _mapper.Map<TodoWM>(x)).ToList();            
            return Ok(result);
        }       
        

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoWM))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        async public Task<IActionResult> Get(int id)
        {
            var todo = await _gettodoService.GetAsync(id);
            return todo != null ? Ok(_mapper.Map<TodoWM>(todo)) : NotFound();
        }

        // POST api/<TodoController>
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<IActionResult> Post([FromBody] TodoWM todo)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<TodoDto>(todo);
                await _gettodoService.AddTodoAsync(dto);
                return Ok();
            }
            return BadRequest();
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> Put(int id, [FromBody] string title)
        {
            var todo = await _gettodoService.GetAsync(id);
            if (todo == null)
                return NotFound();
            todo.Title = title;
            await _gettodoService.UpdateTodo(todo);
            return Ok();
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> Delete(int id)
        {
            var todo = await _gettodoService.GetAsync(id);
            if (todo == null)
                return NotFound();
            await _gettodoService.DeleteTodoAsync(id);
            return Ok();
        }
    }
}
