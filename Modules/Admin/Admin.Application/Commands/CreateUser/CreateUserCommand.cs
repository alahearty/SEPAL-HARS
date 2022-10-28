using HARS.Shared.Architecture.Commands;
using HARS.Shared.Builders;
using HARS.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.Commands.CreateUser
{
    public class CreateUserCommand : Command<Guid>
    {
        public override ActionResult Validate()
        {
            return new FluentValidator()
              .IsValidText(FirstName, "First Name cannot be empty")
              .IsValidText(LastName, "Last Name cannot be empty")
              .IsValidEmail(Email, $"Invalid email. {Email}")
              .Result;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool NotifyMail { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
