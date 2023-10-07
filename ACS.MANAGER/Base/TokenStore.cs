
using Inventec.Common.Logging;
using System;
namespace ACS.MANAGER.Base
{
    class TokenStore
    {
        private static string tokenCode;
        internal static string GetTokenCode
        {
            get
            {
                if (String.IsNullOrEmpty(tokenCode))
                {
                    tokenCode = Inventec.Token.ResourceSystem.ResourceTokenManager.GetTokenCode();
                }
                return tokenCode;
            }
        }
    }
}
