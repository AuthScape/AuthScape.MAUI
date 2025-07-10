using AuthScape.MAUI.Helpers;
using AuthScapeMAUI.Models;
using System.Text.Json;

namespace AuthScapeMAUI.Services
{
    public class UserManagementService
    {
        public async Task<SignedInUser?> GetSignedInUser()
        {
            var user = await SecureStorage.Default.GetAsync("user");
            if (String.IsNullOrEmpty(user))
            {
                // If no user is found, return an empty string or handle accordingly
                return null;
            }

            return JsonHelper.Deserialize<SignedInUser>(user);
        }
    }
}