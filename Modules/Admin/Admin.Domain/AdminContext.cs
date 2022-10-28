using Admin.Domain.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Domain
{
    public class AdminContext
    {
        public virtual IAccessManager AccessManager { get; set; }
    }
}
