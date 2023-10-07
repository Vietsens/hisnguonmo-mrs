using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Base
{
    public class BaseUriStore
    {
        public static string GetSdaUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Sda"];
        public static string GetSarUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Sar"];
        public static string GetAcsUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Acs"];
        public static string GetMosUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Mos"];
        public static string GetFssUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Fss"];
        public static string GetHtcUri = ConfigurationManager.AppSettings["MRS.MANAGER.Base.ApiConsumerStore.Htc"];
    }
}
