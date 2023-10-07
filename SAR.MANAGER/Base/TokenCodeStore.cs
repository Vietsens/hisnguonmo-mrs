using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAR.MANAGER.Base
{
    class TokenCodeStore
    {
        private static string tokenCode;
        internal static string TokenCode
        {
            get
            {
                string tempCode = "";
                try
                {
                    tempCode = Inventec.Token.ResourceSystem.ResourceTokenManager.GetTokenCode();
                }
                catch { }

                tokenCode = (String.IsNullOrEmpty(tempCode) ? tokenCode : tempCode);
                return tokenCode;
            }
            set
            {
                tokenCode = value;
            }
        }
    }
}
