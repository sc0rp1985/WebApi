using AutoMapper;
using BLL;
using Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web.Models;
using WebApp.CustomMiddlewares;
using WebApp.Utils;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        async public Task<IActionResult> Post([FromBody] TodoWM todo)
        {
            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<TodoDto>(todo);
                var operResult = await _gettodoService.AddTodoAsync(dto);
                return this.OperationResultToActionResult(operResult);
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
            var operResult = await _gettodoService.UpdateTodoAsync(todo);
            return this.OperationResultToActionResult(operResult);            
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
            var operResult =  await _gettodoService.DeleteTodoAsync(id);
            return this.OperationResultToActionResult(operResult);
            //return Ok();
        }
    }
}
