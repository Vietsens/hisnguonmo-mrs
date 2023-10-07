using Inventec.Core;
using Inventec.Token.AuthSystem;
using SDA.MANAGER.Base;
using System;
using System.Web.Http.Controllers;

namespace SDA.MANAGER.Token
{
    /// <summary>
    /// Khong cho phep thua ke
    /// </summary>
    public sealed partial class TokenManager : BusinessBase
    {
        AuthTokenManager authManager;

        public TokenManager()
            : base()
        {

        }

        public TokenManager(CommonParam param)
            : base(param)
        {

        }

        public ApiResultObject<Inventec.Token.Core.TokenData> Login(HttpActionContext httpActionContext)
        {
            ApiResultObject<Inventec.Token.Core.TokenData> result = new ApiResultObject<Inventec.Token.Core.TokenData>(null, false);
            try
            {
                Inventec.Token.Core.TokenData token = Inventec.Token.Manager.Login(httpActionContext);
                if (token != null)
                {
                    result = new ApiResultObject<Inventec.Token.Core.TokenData>(token, true);
                }
                else
                    result.SetValue(null, false, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ApiResultObject<Inventec.Token.Core.TokenData>(null, false);
            }
            return result;
        }

        public ApiResultObject<bool> ChangePassword(string oldPassword, string newPassword)
        {
            ApiResultObject<bool> result = new ApiResultObject<bool>(false);
            try
            {
                bool success = Inventec.Token.Manager.ChangePassword(oldPassword, newPassword);
                result = new ApiResultObject<bool>(success, success);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ApiResultObject<bool>(false);
            }
            return result;
        }

        public ApiResultObject<bool> Logout()
        {
            ApiResultObject<bool> result = new ApiResultObject<bool>(false);
            try
            {
                bool success = Inventec.Token.Manager.Logout();
                result = new ApiResultObject<bool>(success, success);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ApiResultObject<bool>(false);
            }
            return result;
        }
    }
}
