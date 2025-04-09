using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RL.Backend.Commands;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;
using System.Data.Entity;

namespace RL.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssignUserController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly RLContext _context;
        private readonly IMediator _mediator;

        public AssignUserController(ILogger<UsersController> logger, RLContext context, IMediator mediator)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [EnableQuery]
        public IEnumerable<AssignedUser> Get()
        {
            return _context.AssignedUsers;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AssignUserCommand user, CancellationToken token)
        {
            var resonse = await _mediator.Send(user, token);
            return resonse.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(RemoveAssignedUserCommand user, CancellationToken token)
        {
            var resonse = await _mediator.Send(user, token);
            return resonse.ToActionResult();
        }


        [Route("all")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll(RemoveAllAssignedUserCommand user, CancellationToken token)
        {
            var resonse = await _mediator.Send(user, token);
            return resonse.ToActionResult();
        }

    }
}
