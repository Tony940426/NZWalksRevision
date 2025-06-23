using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace NZWalks.API.Repositories
{
    public class TokenRepository: ITokenRepository
    {
        public string CreateJWTToken(IdentityUser user, List<string> Roles)
        {
            //Create Claims from Roles
            var claims = new List<Claim>();
            Claim.Add(new Claim(ClaimTypes.Email, user.email));
        }
    }
}
