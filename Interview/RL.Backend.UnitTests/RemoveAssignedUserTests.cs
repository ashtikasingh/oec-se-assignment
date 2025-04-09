using MediatR;
using RL.Backend.Commands.Handlers.Users;
using RL.Backend.Commands;
using RL.Backend.Exceptions;
using FluentAssertions;

namespace RL.Backend.UnitTests
{
    [TestClass]
    public class RemoveAssignedUserTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task RemoveAssignedUserTests_InvalidPlanId_ReturnsBadRequest(int planId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new RemoveAssignedUser(context);
            var request = new RemoveAssignedUserCommand
            {
                PlanId = planId,
                ProcedureId = 1,
                UserId = 1
            };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task RemoveAssignedUserTests_InvalidUserId_ReturnsBadRequest(int userId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new RemoveAssignedUser(context);
            var request = new RemoveAssignedUserCommand
            {
                PlanId = 1,
                ProcedureId = 1,
                UserId = userId
            };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task RemoveAssignedUserTests_InvalidProcedureId_ReturnsBadRequest(int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new RemoveAssignedUser(context);
            var request = new RemoveAssignedUserCommand
            {
                PlanId = 1,
                ProcedureId = procedureId,
                UserId = 1
            };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(2, 1, 2)]
        [DataRow(3, 2, 1)]
        public async Task RemoveAssignedUserTests_RecordNotFound_ReturnsNotFound(int userId, int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            context.Plans.Add(new Data.DataModels.Plan { PlanId = 1 });
            context.Plans.Add(new Data.DataModels.Plan { PlanId = 2 });
            context.Procedures.Add(new Data.DataModels.Procedure { ProcedureId = 1 });
            context.Procedures.Add(new Data.DataModels.Procedure { ProcedureId = 2 });
            await context.SaveChangesAsync();

            var handler = new RemoveAssignedUser(context);
            var request = new RemoveAssignedUserCommand
            {
                PlanId = planId,
                ProcedureId = procedureId,
                UserId = userId
            };

            var result = await handler.Handle(request, new CancellationToken());

            result.Exception.Should().BeOfType<NotFoundException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(1, 1, 1)]
        [DataRow(1, 1, 2)]
        [DataRow(1, 2, 1)]
        public async Task RemoveAssignedUserTests_RemoveUserAssigned_ReturnsSuccess(int userId, int planId, int procedureId)
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

            var handler = new RemoveAssignedUser(context);
            var request = new RemoveAssignedUserCommand
            {
                PlanId = planId,
                ProcedureId = procedureId,
                UserId = userId
            };

            var result = await handler.Handle(request, new CancellationToken());

            result.Value.Should().BeOfType<Unit>();
            result.Succeeded.Should().BeTrue();
        }
    }
}
