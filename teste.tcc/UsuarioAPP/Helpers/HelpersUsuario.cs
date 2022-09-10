using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Helpers
{
    public class HelpersUsuario
    {
        public int RetornarIdUsuario (string token)
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            var reivindicações = jwtSecurityToken.Claims.ToList();

            var id = int.Parse(reivindicações[1].Value);
            return id;
        }
    }
}
