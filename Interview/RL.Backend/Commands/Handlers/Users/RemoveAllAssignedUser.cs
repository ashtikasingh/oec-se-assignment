using MediatR;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;

namespace RL.Backend.Commands.Handlers.Users
{
    public class RemoveAllAssignedUser : IRequestHandler<RemoveAllAssignedUserCommand, ApiResponse<Unit>>
    {
        private readonly RLContext context;

        public RemoveAllAssignedUser(RLContext context)
        {
            this.context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(RemoveAllAssignedUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PlanId < 1)
                {
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                }
                if (request.ProcedureId < 1)
                {
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
                }

                var assignedUsers = context.AssignedUsers.Where(user => user.PlanId == request.PlanId && user.ProcedureId == request.ProcedureId);

                if (assignedUsers.Any())
                {
                    context.AssignedUsers.RemoveRange(assignedUsers);
                    await context.SaveChangesAsync();
                }

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Fail(new BadRequestException(ex.Message));
            }
        }
    }
}
