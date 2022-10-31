using Admin.Domain;
using Admin.Domain.UsersManagement;
using HARS.Shared.Architecture.Commands;
using HARS.Shared.DataBases;
using HARS.Shared.Utilities;

namespace Admin.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, CommandAccessor<AdminContext>, Guid>
    {
        public override async Task<ActionResult<Guid>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            var fetchUser = await DbContext.Store
                .AccessManager
                .UserRepository.FindUserByEmail(command.Email);

            //if (fetchUser.WasSuccessful && !fetchUser.Data.IsDeleted) return FailedOperation(ErrorCode.BadRequest, $"{command.Email} already exists.");

            var user = new HARSUser().CreateHARSUser(command.FirstName, command.LastName, command.Email, command.IsAdmin ? AccountTypes.Admin : AccountTypes.Regular);

            var passwordHash = DbContext.Store
                .AccessManager
                .HashPassword(command.Password);

            user.PasswordHash = passwordHash;

            DbContext.Store
                .AccessManager
                .UserRepository.Add(user);


            return SuccessfulOperation(user.Id);
        }
    }
}
