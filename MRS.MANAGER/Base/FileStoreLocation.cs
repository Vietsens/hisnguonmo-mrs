
using System.Configuration;
namespace MRS.MANAGER.Base
{
    /// <summary>
    /// Quy dinh thu muc luu tru file tren he thong FSS
    /// </summary>
    internal class FileStoreLocation
    {
        internal static string DOWNLOAD_FOLDER = ConfigurationManager.AppSettings["MRS.MANAGER.Base.FileStoreLocation.DOWNLOAD_FOLDER"];
    }
}
