using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    //Luu cac cau hinh de tich hop voi Lis
    class LisCFG
    {
        public enum LisIntegrateOption
        {
            LIS = 1,//dung he thong LIS do Inventec phat trien
        }

        //Luu thong tin dia chi cua LIS tuong ung voi tung phong xet nghiem
        public class LisAddress
        {
            public string RoomCode { get; set; }
            public string Url { get; set; }
        }

        private const string LIS_INTEGRATE_OPTION_CFG = "MOS.LIS.INTEGRATE_OPTION";
        private const string LIS_ADDRESS_CFG = "MOS.LIS.ADDRESS";

        private static int lisIntegrateOption;
        public static int LIS_INTEGRATE_OPTION
        {
            get
            {
                if (lisIntegrateOption == 0)
                {
                    lisIntegrateOption = ConfigUtil.GetIntConfig(LIS_INTEGRATE_OPTION_CFG);
                }
                return lisIntegrateOption;
            }
            set
            {
                lisIntegrateOption = value;
            }
        }

        private static List<LisAddress> lisAddresses;
        public static List<LisAddress> LIS_ADDRESSES
        {
            get
            {
                if (lisAddresses == null)
                {
                    lisAddresses = LisCFG.ParseLisAddress(LIS_ADDRESS_CFG);
                }
                return lisAddresses;
            }
            set
            {
                lisAddresses = value;
            }
        }

        private static List<LisAddress> ParseLisAddress(string code)
        {
            try
            {
                List<LisAddress> result = new List<LisAddress>();
                string data = ConfigUtil.GetStrConfig(code);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LisAddress>>(data);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

        public static void Reload()
        {
            var address = LisCFG.ParseLisAddress(LIS_ADDRESS_CFG);
            var integrateOption = ConfigUtil.GetIntConfig(LIS_INTEGRATE_OPTION_CFG);
            lisAddresses = address;
            lisIntegrateOption = integrateOption;
        }
    }
}
