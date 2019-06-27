using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DomainCore.Core.Interfaces;
using DomainCore.Core.EntitiesDTO.App.Example;

namespace ApiCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//   default JWT auth
    public class ExamplesController : Controller
    {
        #region Propeties

        private readonly IExampleRep _exampleRep;
        private readonly ILogger _logger;

        #endregion

        #region Construct

        public ExamplesController(
            IExampleRep exampleRep,
            ILogger<ExamplesController> logger)
        {
            _logger = logger;
            _exampleRep = exampleRep;
        }

        #endregion

        #region Methods

        #region Get's methods

        [HttpGet]
        public async Task<ActionResult<ExampleDTO>> GetAll()
        {
            _logger.LogInformation("Try acces to all examples");
            var response = await _exampleRep.GetAll();
            if (response == null)
            {
                _logger.LogWarning("Some error ocurred");
                return NotFound();
            }

            _logger.LogInformation("All examples showed");
            return Ok(response);
        }

        [HttpGet("id={id}")]
        public async Task<ActionResult<ExampleDTO>> GetById([FromRoute] int id)
        {
            _logger.LogInformation($"Try acces to example with Id=> {id}");
            var response = await _exampleRep.GetById(id);
            if (response == null)
            {
                _logger.LogWarning("Some error ocurred");
                return NotFound();
            }

            _logger.LogInformation($"Example with ID=> {id} is finded");
            return Ok(response);
        }

        #endregion

        #region CRUD

        [HttpPost]
        public async Task<ActionResult<ExampleDTO>> CreateUserInfo([FromBody] CreateExampleDTO create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var response = await _exampleRep.CreateAsync(create);
            if (response == null)
                return BadRequest("Some error ocurred");

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateExampleDTO update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var response = await _exampleRep.UpdateAsync(update);
            if (!response)
                return NotFound("Some error ocurred");

            
            return Ok();
        }

        [HttpDelete("delete_id={deleteId}")]
        public async Task<IActionResult> DeleteUserInfo([FromRoute] int deleteId)
        {
            var response = await _exampleRep.DeleteAsync(deleteId);
            if (!response)
                return NotFound();

            return Ok();
        }

        #endregion

        #endregion
    }
}
