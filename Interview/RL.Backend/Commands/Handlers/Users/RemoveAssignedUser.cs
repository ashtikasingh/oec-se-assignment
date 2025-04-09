using MediatR;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;

namespace RL.Backend.Commands.Handlers.Users
{
    public class RemoveAssignedUser : IRequestHandler<RemoveAssignedUserCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public RemoveAssignedUser(RLContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(RemoveAssignedUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UserId < 1)
                {
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid UserId"));
                }
                if (request.PlanId < 1)
                {
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                }

                if (request.ProcedureId < 1)
                {
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
                }

                var assignedData = _context.AssignedUsers.FirstOrDefault(user => user.UserId == request.UserId && user.PlanId == request.PlanId && user.ProcedureId == request.ProcedureId);

                if (assignedData is null)
                {
                    return ApiResponse<Unit>.Fail(new NotFoundException("record not found")); ;
                }

                var data = _context.AssignedUsers.Remove(assignedData);
                await _context.SaveChangesAsync();

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Fail(ex);
            }
        }
    }
}
