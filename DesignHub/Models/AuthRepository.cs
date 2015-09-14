using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DesignHub.Models
{

    public class AuthRepository : IDisposable
    {
        private readonly AuthContext _db;

        private readonly UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _db = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_db));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {

       
            var user = new MyUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _db.Dispose();
            _userManager.Dispose();

        }
    }

}