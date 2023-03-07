using AutoMapper;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using WebApp.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        readonly IGetTodoService _gettodoService;
        readonly IMapper _mapper;
        public CommentController(IGetTodoService getTodoService, IMapper mapper)
        {
            _gettodoService = getTodoService;
            _mapper = mapper;
        }        

        // GET api/<CommentController>/5
        [Route("todo/{id:int}")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentWM>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> Get(int id)
        {
            var todo = await _gettodoService.GetAsync(id);
            if (todo == null)
                return NotFound();
            var comments = await _gettodoService.GetTodoCommetsAsync(id);
            var result = comments.Select(x => _mapper.Map<CommentWM>(x)).ToList();
            return Ok(result);
        }

        // POST api/<CommentController>
        [Route("todo/{id:int}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        async public Task<IActionResult> Post(int id, [FromBody] string value)
        {
            var todo = await _gettodoService.GetAsync(id);
            if (todo == null)
                return NotFound();
            var operationResult = await _gettodoService.AddCommentAsync(id, new CommentDto { Text = value });
            var result = this.OperationResultToActionResult(operationResult);
            return result;
        }
        
    }
}
