using FluentAssertions;
using MediatR;
using RL.Backend.Commands;
using RL.Backend.Commands.Handlers.Users;
using RL.Backend.Exceptions;

namespace RL.Backend.UnitTests
{
    [TestClass]
    public class AssignUserTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        public async Task AssignUserToPlanProcedureTests_InvalidPlanId_ReturnsBadRequest(int planId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
            {
                PlanId = planId,
                ProcedureId = 2,
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
        public async Task AssignUserToPlanProcedureTests_InvalidUserId_ReturnsBadRequest(int userId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
            {
                PlanId = 1,
                ProcedureId = 2,
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
        public async Task AssignUserToPlanProcedureTests_InvalidProcedureId_ReturnsBadRequest(int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
            {
                PlanId = 1,
                ProcedureId = procedureId,
                UserId = 2
            };
            var result = await handler.Handle(request, new CancellationToken());
            result.Exception.Should().BeOfType<BadRequestException>();
            result.Succeeded.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(3, 1, 2)]
        [DataRow(2, 1, 2)]
        public async Task AssignUserToPlanProcedureTests_UserNotFound_ReturnsNotFound(int userId, int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            context.Plans.Add(new Data.DataModels.Plan() { PlanId = 1 });
            context.Procedures.Add(new Data.DataModels.Procedure() { ProcedureId = 2 });
            await context.SaveChangesAsync();

            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
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
        [DataRow(1, 2, 3)]
        public async Task AssignUserToPlanProcedureTests_PlanProcedureNotFound_ReturnsNotFound(int userId, int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            context.Users.Add(new Data.DataModels.User() { UserId = userId });
            context.Plans.Add(new Data.DataModels.Plan() { PlanId = planId });
            context.Procedures.Add(new Data.DataModels.Procedure() { ProcedureId = procedureId });
            await context.SaveChangesAsync();

            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
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
        public async Task AssignUserToPlanProcedureTests_UserAlreadyAssigned_ReturnsSuccess(int userId, int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            context.Users.Add(new Data.DataModels.User() { UserId = userId });
            context.Plans.Add(new Data.DataModels.Plan() { PlanId = planId });
            context.Procedures.Add(new Data.DataModels.Procedure() { ProcedureId = procedureId });
            context.PlanProcedures.Add(new Data.DataModels.PlanProcedure() { PlanId = planId, ProcedureId = procedureId });
            context.AssignedUsers.Add(new Data.DataModels.AssignedUser() { UserId = userId, PlanId = planId, ProcedureId = procedureId });
            await context.SaveChangesAsync();

            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
            {
                PlanId = planId,
                ProcedureId = procedureId,
                UserId = userId
            };
            var result = await handler.Handle(request, new CancellationToken());
            result.Value.Should().BeOfType<Unit>();
            result.Succeeded.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(2, 1, 3)]
        public async Task AssignUserToPlanProcedureTests_UserAssigned_ReturnsSuccess(int userId, int planId, int procedureId)
        {
            var context = DbContextHelper.CreateContext();
            context.Users.Add(new Data.DataModels.User() { UserId = userId });
            context.Plans.Add(new Data.DataModels.Plan() { PlanId = planId });
            context.Procedures.Add(new Data.DataModels.Procedure() { ProcedureId = procedureId });
            context.PlanProcedures.Add(new Data.DataModels.PlanProcedure() { PlanId = planId, ProcedureId = procedureId });
            await context.SaveChangesAsync();

            var handler = new AssignUserToPlanProcedure(context);
            var request = new AssignUserCommand()
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
