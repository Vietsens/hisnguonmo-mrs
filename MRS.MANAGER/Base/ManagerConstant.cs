
namespace MRS.MANAGER.Base
{
    public class ManagerConstant
    {
        //So chu so sau phan thap phan
        public const int DECIMAL_PRECISION = 4;
        //Max time
        public const long MAX_TIME = 99999999999999;
        //Client code khi goi upload file sang he thong FSS
        public const string FSS_CLIENT_CODE = "MRS";
        public const int MAX_REQUEST_LENGTH_PARAM = 500;

        public static string GetUpdateToError = System.Configuration.ConfigurationManager.AppSettings["MRS.MANAGER.Base.UpdateToError"];
        public static string GetExportPdf = System.Configuration.ConfigurationManager.AppSettings["MRS.Processor.Export.PDF"];
    }
}
