using api.Entities;
using Microsoft.AspNetCore.Identity;

namespace api.Helpers 
{

  public interface IUsers
    {
        public void HardResetPassword(string email, string newPassword);
    }

    public class Users : IUsers
    {
        #region Constructor
        private readonly UserManager<AppUser> _userManager;
        public Users(UserManager<AppUser> userMgr)
        {
            _userManager = userMgr;
        }
        #endregion

        #region password reset
        public void HardResetPassword(string email, string newPassword)
        {
            var userTask = _userManager.FindByEmailAsync(email);
            userTask.Wait();
            var user = userTask.Result;
            ChangeUserPassword(user, newPassword);
        }

        private string GeneratePasswordResetToken(AppUser user)
        {
            var task = _userManager.GeneratePasswordResetTokenAsync(user);
            task.Wait();
            var token = task.Result;
            return token;
        }

        private void ChangeUserPassword(AppUser user, string newPassword)
        {
            var token = GeneratePasswordResetToken(user);
            var task = _userManager.ResetPasswordAsync(user, token, newPassword);
            task.Wait();
            var result = task.Result;
        }
        #endregion
    }  
}