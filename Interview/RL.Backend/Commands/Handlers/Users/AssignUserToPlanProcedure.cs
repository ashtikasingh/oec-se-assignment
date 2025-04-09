using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Commands.Handlers.Users
{
    public class AssignUserToPlanProcedure : IRequestHandler<AssignUserCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public AssignUserToPlanProcedure(RLContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(AssignUserCommand request, CancellationToken cancellationToken)
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

                var user = await _context.Users.FindAsync(request.UserId);

                if (user is null)
                {
                    return ApiResponse<Unit>.Fail(new NotFoundException("User not found"));
                }

                var planProcedure = await _context.PlanProcedures.FirstOrDefaultAsync(p => p.PlanId == request.PlanId && p.ProcedureId == request.ProcedureId);

                if (planProcedure is null)
                {
                    return ApiResponse<Unit>.Fail(new NotFoundException("PlanProcedure not found"));
                }
                var assignedData = await _context.AssignedUsers.FirstOrDefaultAsync(user => user.UserId == request.UserId && user.PlanId == request.PlanId && user.ProcedureId == request.ProcedureId);

                if (assignedData is null)
                {
                    AssignedUser assigned = new AssignedUser() { UserId = request.UserId, PlanId = request.PlanId, ProcedureId = request.ProcedureId };
                    var data = await _context.AssignedUsers.AddAsync(assigned);
                    await _context.SaveChangesAsync();
                }
                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Fail(ex);
            }
        }
    }
}
