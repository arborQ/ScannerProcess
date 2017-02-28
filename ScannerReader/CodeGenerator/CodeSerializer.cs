using System.Collections.Generic;
using JWT;
using Logger.Interfaces;

namespace CodeGenerator
{
    public class CodeSerializer : ICodeSerializer
    {
        private readonly ILogService _logService;
        const string SecretKey = "RVCsIl171I5Vd6Eb631cBGm9Lg8z9N7V";
        const string LoginKey = "login";

        public CodeSerializer(ILogService logService)
        {
            _logService = logService;
        }

        public string SerializeCode(string message)
        {
            var payload = new Dictionary<string, string>
            {
                { LoginKey, message }
            };

            return JsonWebToken.Encode(payload, SecretKey, JwtHashAlgorithm.HS256);
        }

        public string DeserializeCode(string token)
        {
            try
            {
                var payload = JsonWebToken.DecodeToObject<Dictionary<string, string>>(token, SecretKey);

                if (payload != null && payload.ContainsKey(LoginKey))
                {
                    return payload[LoginKey];
                }
            }
            catch (SignatureVerificationException ex)
            {
                _logService.Exception(ex);
            }

            return null;
        }
    }

    public interface ICodeSerializer
    {
        string SerializeCode(string message);

        string DeserializeCode(string token);
    }
}