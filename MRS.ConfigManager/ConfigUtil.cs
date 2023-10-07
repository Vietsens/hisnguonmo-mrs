using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.ConfigManager
{
    public class ConfigUtil
    {
        public static string DataSource = ConfigurationManager.AppSettings["MRS.ConfigManager.ConnectionStrings.DataSource"] ?? "IMSYSDB";
        public static string IdDb = ConfigurationManager.AppSettings["MRS.ConfigManager.ConnectionStrings.Id"] ?? "MRS_RS";
        public static string PassDb = ConfigurationManager.AppSettings["MRS.ConfigManager.ConnectionStrings.Pass"] ?? "MRS_RS";
    }
}
