using Inventec.Common.WebApiClient;
using Inventec.Token.ResourceSystem;
using MOS.MANAGER.Base;
using System.Configuration;

namespace MOS.MANAGER.ConsumerManager
{
    class ApiConsumerStore
    {
        private static ApiConsumer sdaConsumer;
        internal static ApiConsumer SdaConsumer
        {
            get
            {
                if (sdaConsumer == null)
                {
                    sdaConsumer = new ApiConsumer(ConfigurationManager.AppSettings["Inventec.SdaConsumer.Base.Uri"], ManagerConstant.APPLICATION_CODE);
                }
                return sdaConsumer;
            }
        }

        internal static ApiConsumer AcsConsumer
        {
            get
            {
                string tokenCode = ResourceTokenManager.GetTokenCode();
                return new ApiConsumer(ConfigurationManager.AppSettings["Inventec.AcsConsumer.Base.Uri"], tokenCode, ManagerConstant.APPLICATION_CODE);
            }
        }

        private static ApiConsumer rocheIntegrateServiceConsumer;
        internal static ApiConsumer RocheIntegrateConsumer
        {
            get
            {
                if (rocheIntegrateServiceConsumer == null)
                {
                    rocheIntegrateServiceConsumer = new ApiConsumer(ConfigurationManager.AppSettings["Inventec.RocheIntegrateService.Base.Uri"], ManagerConstant.APPLICATION_CODE);
                }
                return rocheIntegrateServiceConsumer;
            }
        }
    }
}
