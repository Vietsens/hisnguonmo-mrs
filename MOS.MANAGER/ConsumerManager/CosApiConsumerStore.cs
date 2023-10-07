using Inventec.Common.WebApiClient;
using MOS.MANAGER.Base;
using System.Configuration;

namespace MOS.MANAGER.ConsumerManager
{
    class CosApiConsumerStore
    {
        private static ApiConsumerWrapper cosConsumerWrapper;
        internal static ApiConsumerWrapper CosConsumerWrapper
        {
            get
            {
                if (cosConsumerWrapper == null)
                {
                    cosConsumerWrapper = new ApiConsumerWrapper(
                        true,
                        ManagerConstant.APPLICATION_CODE,
                        ConfigurationManager.AppSettings["Inventec.CosConsumer.Base.Uri"],
                        ConfigurationManager.AppSettings["Inventec.CosConsumer.Acs.Uri"],
                        ConfigurationManager.AppSettings["Inventec.CosConsumer.LoginName"],
                        ConfigurationManager.AppSettings["Inventec.CosConsumer.Password"]);
                    cosConsumerWrapper.UseRegistry(false);
                }
                return cosConsumerWrapper;
            }
        }
    }
}
