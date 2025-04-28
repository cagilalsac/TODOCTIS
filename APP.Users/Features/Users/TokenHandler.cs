using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Users
{
    public class TokenRequest : Request, IRequest<TokenResponse>
    {
        [Required, StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required, StringLength(10, MinimumLength = 3)]
        public string Password { get; set; }
    }

    public class TokenResponse : CommandResponse
    {
        public string Token { get; set; }

        public TokenResponse(bool isSuccessful, string message = "", int id = 0) : base(isSuccessful, message, id)
        {
        }
    }

    public class TokenHandler : UsersDbHandler, IRequestHandler<TokenRequest, TokenResponse>
    {
        public TokenHandler(UsersDb db) : base(db)
        {
        }

        public async Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.Include(u => u.Role).SingleOrDefaultAsync(u => 
                u.UserName == request.UserName && u.Password == request.Password && u.IsActive);
            if (user is null)
                return new TokenResponse(false, "Active user with the user name and password not found!");
            var claims = GetClaims(user);
            var expiration = DateTime.Now.AddMinutes(AppSettings.ExpirationInMinutes);
        }
    }
}
