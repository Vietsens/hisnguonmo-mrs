using Inventec.Common.Logging;
using System;
using System.Threading;

namespace MRS.MANAGER.Base
{
    class TokenCodeStore
    {
        internal static LocalDataStoreSlot SlotTokenCode = Thread.AllocateNamedDataSlot("Mrs.Webserver.TokenCode");
        internal static string TokenCode
        {
            get
            {
                return (Thread.GetData(SlotTokenCode) ?? "").ToString();
            }
        }
    }
}
