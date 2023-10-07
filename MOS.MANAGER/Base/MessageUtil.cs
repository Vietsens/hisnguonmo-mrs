using Inventec.Common.Logging;
using Inventec.Core;
using MOS.LibraryMessage;
using MOS.MANAGER.Token;
using System;

namespace MOS.MANAGER.Base
{
    public class MessageUtil
    {
        public static void SetMessage(CommonParam param, MOS.LibraryMessage.Message.Enum en, params string[] extraMessage)
        {
            try
            {
                string message = GetMessage(en);
                if (extraMessage != null && extraMessage.Length > 0)
                {
                    message = String.Format(message, extraMessage);
                }
                AddMessage(param, message);
            }
            catch (Exception ex)
            {
                LogSystem.Error("Co exception khi SetParam co tham so phu.", ex);
            }
        }

        private static string GetMessage(MOS.LibraryMessage.Message.Enum en)
        {
            string languageKey = Message.LanguageName.VI;
            try
            {
                //languageKey = TokenManager.GetLanguage(); //review
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }

            Message message = MOS.LibraryMessage.DatabaseMessage.Get(languageKey, en);
            return message != null ? message.message : null;
        }

        private static void AddMessage(CommonParam param, string message)
        {
            if (message != null)
            {
                if (!param.Messages.Contains(message))
                {
                    param.Messages.Add(message);
                }
            }
        }
    }
}
