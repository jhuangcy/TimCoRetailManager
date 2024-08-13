using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace TimCoRetailManager_WASM.Auth
{
    public static class JwtHelper
    {
        public static IEnumerable<Claim> ParseClaims(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var kvps = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            ExtractRoles(claims, kvps);
            claims.AddRange(kvps.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }

        static void ExtractRoles(List<Claim> claims, Dictionary<string, object> kvps)
        {
            kvps.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                var parsed = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');
                if (parsed.Length > 1)
                {
                    foreach (var role in parsed)
                        claims.Add(new Claim(ClaimTypes.Role, role.Trim('"')));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsed[0]));
                }
                kvps.Remove(ClaimTypes.Role);
            }
        }
    }
}
