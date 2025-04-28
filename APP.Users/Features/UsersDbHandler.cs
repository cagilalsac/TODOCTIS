using APP.Users.Domain;
using CORE.APP.Features;
using System.Globalization;
using System.Security.Claims;

namespace APP.Users.Features
{
    public class UsersDbHandler : Handler
    {
        protected readonly UsersDb _db;

        public UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }

        protected virtual List<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString())
            };
        }
    }
}
