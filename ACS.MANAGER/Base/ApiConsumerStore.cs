
using System;
namespace ACS.MANAGER.Base
{
    class ApiConsumerStore
    {
        private static Inventec.Common.WebApiClient.ApiConsumer sdaConsumer;
        internal static Inventec.Common.WebApiClient.ApiConsumer SdaConsumer
        {
            get
            {
                if (sdaConsumer == null)
                {
                    if (string.IsNullOrEmpty(ACS.LibraryConfig.WebConfig.URI_API_SDA))
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Khong doc duoc cau hinh dia chi ip api sda");
                    }
                    sdaConsumer = new Inventec.Common.WebApiClient.ApiConsumer(ACS.LibraryConfig.WebConfig.URI_API_SDA, TokenStore.GetTokenCode);
                }
                return sdaConsumer;
            }
        }

        internal static Inventec.Common.WebApiClient.ApiConsumer AcsConsumer
        {
            get
            {
                string tokenCode = TokenStore.GetTokenCode;
                if (String.IsNullOrEmpty(tokenCode))
                {
                    Inventec.Common.Logging.LogSystem.Debug("Khong lay duoc token code de cap nhat vao cac SarConsumer");
                }
                return new Inventec.Common.WebApiClient.ApiConsumer(ACS.LibraryConfig.WebConfig.URI_API_ACS, tokenCode);
            }
        }
    }
}
