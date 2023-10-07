using Inventec.Common.Logging;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    //Luu thong tin dia chi cua roche tuong ung voi tung phong xet nghiem
    public class RocheAddress
    {
        public string RoomCode { get; set; }
        public string Url { get; set; }
    }

    public class RocheIp
    {
        public string RoomCode { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    //Luu cac cau hinh de tich hop voi Roche
    public class RocheCFG
    {
        private const string ROCHE_SENDING_APP_CODE_CFG = "MOS.ROCHE_SENDING_APP_CODE";
        private const string ROCHE_RECEIVING_APP_CODE_CFG = "MOS.ROCHE_RECEIVING_APP_CODE";
        private const string ROCHE_USE_PATIENT_TYPE_INSTEAD_OF_DEPARTMENT_CODE_CFG = "MOS.ROCHE_USE_PATIENT_TYPE_INSTEAD_OF_DEPARTMENT_CODE";
        private const string ROCHE_IS_INTEGRATE_CFG = "MOS.ROCHE_IS_INTEGRATE";
        private const string ROCHE_ADDRESS_CFG = "MOS.ROCHE_ADDRESS";
        private const string ROCHE_IP_CFG = "MOS.ROCHE_IP";
        private const string ROCHE_FOLDER_SAVE = "MOS.ROCHE_FOLDER_SAVE";
        private const string ROCHE_FOLDER_RESULT = "MOS.ROCHE_FOLDER_RESULT";
        private const string ROCHE_CONNECT_BY_FOLDER = "MOS.ROCHE_CONNECT_BY_FOLDER";

        public const long LIS_STT_ID_NEW = 1;
        public const long LIS_STT_ID_PROCESS = 2;
        public const long LIS_STT_ID_SUCCESS = 3;
        public const long LIS_STT_ID_ERROR = 4;

        private static string rocheSendingAppCode;
        public static string ROCHE_SENDING_APP_CODE
        {
            get
            {
                if (rocheSendingAppCode == null)
                {
                    rocheSendingAppCode = ConfigUtil.GetStrConfig(ROCHE_SENDING_APP_CODE_CFG);
                }
                return rocheSendingAppCode;
            }
            set
            {
                rocheSendingAppCode = value;
            }
        }

        private static string rocheReceivingAppCode;
        public static string ROCHE_RECEIVING_APP_CODE
        {
            get
            {
                if (rocheReceivingAppCode == null)
                {
                    rocheReceivingAppCode = ConfigUtil.GetStrConfig(ROCHE_RECEIVING_APP_CODE_CFG);
                }
                return rocheReceivingAppCode;
            }
            set
            {
                rocheSendingAppCode = value;
            }
        }

        private static bool? rocheIsIntegrate;
        public static bool ROCHE_IS_INTEGRATE
        {
            get
            {
                if (!rocheIsIntegrate.HasValue)
                {
                    rocheIsIntegrate = ConfigUtil.GetIntConfig(ROCHE_IS_INTEGRATE_CFG) == 1;
                }
                return rocheIsIntegrate.Value;
            }
            set
            {
                rocheIsIntegrate = value;
            }
        }

        private static bool? rocheUsePatientTypeInsteadOfDepartmentCode;
        public static bool ROCHE_USE_PATIENT_TYPE_INSTEAD_OF_DEPARTMENT_CODE
        {
            get
            {
                if (!rocheUsePatientTypeInsteadOfDepartmentCode.HasValue)
                {
                    rocheUsePatientTypeInsteadOfDepartmentCode = ConfigUtil.GetIntConfig(ROCHE_USE_PATIENT_TYPE_INSTEAD_OF_DEPARTMENT_CODE_CFG) == 1;
                }
                return rocheUsePatientTypeInsteadOfDepartmentCode.Value;
            }
            set
            {
                rocheUsePatientTypeInsteadOfDepartmentCode = value;
            }
        }

        private static List<RocheAddress> rocheAddresses;
        public static List<RocheAddress> ROCHE_ADDRESSES
        {
            get
            {
                if (rocheAddresses == null)
                {
                    rocheAddresses = RocheCFG.ParseRocheAddress(ROCHE_ADDRESS_CFG);
                }
                return rocheAddresses;
            }
            set
            {
                rocheAddresses = value;
            }
        }

        private static List<RocheIp> rocheIp;
        public static List<RocheIp> ROCHE_IP
        {
            get
            {
                if (rocheIp == null)
                {
                    rocheIp = RocheCFG.ParseRocheIp(ROCHE_IP_CFG);
                }
                return rocheIp;
            }
            set
            {
                rocheIp = value;
            }
        }

        private static string rocheFolderSave;
        public static string ROCHE_FOLDER_SAVE_NAME
        {
            get
            {
                if (rocheFolderSave == null)
                {
                    rocheFolderSave = ConfigUtil.GetStrConfig(ROCHE_FOLDER_SAVE);
                }
                return rocheFolderSave;
            }
            set
            {
                rocheFolderSave = value;
            }
        }

        private static string rocheFolderResult;
        public static string ROCHE_FOLDER_RESULT_NAME
        {
            get
            {
                if (rocheFolderResult == null)
                {
                    rocheFolderResult = ConfigUtil.GetStrConfig(ROCHE_FOLDER_RESULT);
                }
                return rocheFolderResult;
            }
            set
            {
                rocheFolderResult = value;
            }
        }

        private static string rocheConnectFolder;
        public static string ROCHE_CONNECT_FOLDER
        {
            get
            {
                if (rocheConnectFolder == null)
                {
                    rocheConnectFolder = ConfigUtil.GetStrConfig(ROCHE_CONNECT_BY_FOLDER);
                }
                return rocheConnectFolder;
            }
            set
            {
                rocheConnectFolder = value;
            }
        }

        private static List<RocheAddress> ParseRocheAddress(string code)
        {
            try
            {
                List<RocheAddress> result = new List<RocheAddress>();
                string data = ConfigUtil.GetStrConfig(code);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RocheAddress>>(data);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

        private static List<RocheIp> ParseRocheIp(string code)
        {
            try
            {
                List<RocheIp> result = new List<RocheIp>();
                string data = ConfigUtil.GetStrConfig(code);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RocheIp>>(data);
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
            var sendingAppCode = ConfigUtil.GetStrConfig(ROCHE_SENDING_APP_CODE_CFG);
            var receivingAppCode = ConfigUtil.GetStrConfig(ROCHE_RECEIVING_APP_CODE_CFG);
            var isIntegrate = ConfigUtil.GetIntConfig(ROCHE_IS_INTEGRATE_CFG) == 1;
            var usePatientTypeInsteadOfDepartmentCode = ConfigUtil.GetIntConfig(ROCHE_USE_PATIENT_TYPE_INSTEAD_OF_DEPARTMENT_CODE_CFG) == 1;
            var addresses = RocheCFG.ParseRocheAddress(ROCHE_ADDRESS_CFG);
            rocheReceivingAppCode = receivingAppCode;
            rocheSendingAppCode = sendingAppCode;
            rocheAddresses = addresses;
            rocheIsIntegrate = isIntegrate;
            rocheUsePatientTypeInsteadOfDepartmentCode = usePatientTypeInsteadOfDepartmentCode;

            var ips = RocheCFG.ParseRocheIp(ROCHE_IP_CFG);
            var folderSave = ConfigUtil.GetStrConfig(ROCHE_FOLDER_SAVE);
            var folderResult = ConfigUtil.GetStrConfig(ROCHE_FOLDER_RESULT);
            var folderConnect = ConfigUtil.GetStrConfig(ROCHE_CONNECT_BY_FOLDER);
            rocheIp = ips;
            rocheFolderSave = folderSave;
            rocheFolderResult = folderResult;
            rocheConnectFolder = folderConnect;
        }
    }
}
