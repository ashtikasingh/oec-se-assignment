using FluentAssertions;
using MediatR;
using RL.Backend.Commands;
using RL.Backend.Commands.Handlers.Users;
using RL.Backend.Exceptions;
using RL.Backend.Models;

namespace RL.Backend.UnitTests
{
    [TestClass]
    public class RemoveAllAssignedUserTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task RemoveAllAssignedUserTests_InvalidPlanId_ReturnsBadRequest(int planId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new RemoveAllAssignedUser(context);
            var request = new RemoveAllAssignedUserCommand { PlanId = planId, ProcedureId = 1 };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task RemoveAllAssignedUserTests_InvalidProcedureId_ReturnsBadRequest(int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new RemoveAllAssignedUser(context);
            var request = new RemoveAllAssignedUserCommand { PlanId = 1, ProcedureId = procedureId };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(2, 1)]
        public async Task RemoveAllAssignedUserTests_RemoveAllUserAssigned_ReturnsSuccess(int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();

            context.Users.Add(new Data.DataModels.User { UserId = 1 });
            context.Plans.Add(new Data.DataModels.Plan { PlanId = 1 });
            context.Plans.Add(new Data.DataModels.Plan { PlanId = 2 });
            context.Procedures.Add(new Data.DataModels.Procedure { ProcedureId = 1 });
            context.Procedures.Add(new Data.DataModels.Procedure { ProcedureId = 2 });
            context.PlanProcedures.Add(new Data.DataModels.PlanProcedure { PlanId = 1, ProcedureId = 1 });
            context.PlanProcedures.Add(new Data.DataModels.PlanProcedure { PlanId = 1, ProcedureId = 2 });
            context.PlanProcedures.Add(new Data.DataModels.PlanProcedure { PlanId = 2, ProcedureId = 1 });
            context.AssignedUsers.Add(new Data.DataModels.AssignedUser { UserId = 1, PlanId = 1, ProcedureId = 1 });
            context.AssignedUsers.Add(new Data.DataModels.AssignedUser { UserId = 1, PlanId = 1, ProcedureId = 2 });
            context.AssignedUsers.Add(new Data.DataModels.AssignedUser { UserId = 1, PlanId = 2, ProcedureId = 1 });

            await context.SaveChangesAsync();

            var handler = new RemoveAllAssignedUser(context);
            var request = new RemoveAllAssignedUserCommand { PlanId = planId, ProcedureId = procedureId };

            var result = await handler.Handle(request, new CancellationToken());
            var assignedUsers = context.AssignedUsers.Where(user => user.PlanId == planId && user.ProcedureId == procedureId);

            if (assignedUsers.Any())
                result = ApiResponse<Unit>.Fail(new Exception("Something went wrong"));

            result.Value.Should().BeOfType<Unit>();
            result.Succeeded.Should().BeTrue();
        }
    }
}
