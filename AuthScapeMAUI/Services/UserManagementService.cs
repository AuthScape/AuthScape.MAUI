using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthScapeMAUI.Services
{
    public class UserManagementService
    {
        public async Task GetSignedInUser()
        {
            var user = await SecureStorage.Default.GetAsync("user");
        }
    }
}
