using Inventec.Core;
using SDA.MANAGER.Base;
using System;

namespace SDA.MANAGER.Token
{
    public sealed partial class TokenManager : BusinessBase
    {
        private const string LANGUAGE = "LANGUAGE";

        public ApiResultObject<bool> UpdateLanguage(string language)
        {
            return new ApiResultObject<bool>(Inventec.Token.ResourceSystem.ResourceTokenManager.SetCredentialData(LANGUAGE, language), true);
        }

        public ApiResultObject<string> GetLanguage()
        {
            string language = Inventec.Token.ResourceSystem.ResourceTokenManager.GetCredentialData<string>(LANGUAGE);
            if (string.IsNullOrWhiteSpace(language))
            {
                Inventec.Common.Logging.LogSystem.Warn("language null or white space");
            }
            return new ApiResultObject<string>(language, true);
        }
    }
}
