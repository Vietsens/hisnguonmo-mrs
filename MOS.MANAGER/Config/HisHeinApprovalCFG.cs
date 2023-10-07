using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    public class HisHeinApprovalCFG
    {
        private const string XML_FOLDER_PATH_CFG = "MOS.HIS_HEIN_APPROVAL.XML_FOLDER_PATH";
        private const string IS_AUTO_EXPORT_XML_CFG = "MOS.HIS_HEIN_APPROVAL.IS_AUTO_EXPORT_XML";

        private const string BHYT_NDS_ICD_CODE__OTHER_CFG = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__OTHER";
        private const string BHYT_NDS_ICD_CODE__TE_CFG = "MRS.HIS_REPORT_BHYT_NDS_ICD_CODE__TE";

        private static bool? isAutoExportXml;
        public static bool IS_AUTO_EXPORT_XML
        {
            get
            {
                if (!isAutoExportXml.HasValue)
                {
                    isAutoExportXml = ConfigUtil.GetIntConfig(IS_AUTO_EXPORT_XML_CFG) == 1;
                }
                return isAutoExportXml.Value;
            }
            set
            {
                isAutoExportXml = value;
            }
        }

        private static string xmlFolderPath;
        public static string XML_FOLDER_PATH
        {
            get
            {
                if (xmlFolderPath == null)
                {
                    xmlFolderPath = ConfigUtil.GetStrConfig(XML_FOLDER_PATH_CFG);
                }
                return xmlFolderPath;
            }
            set
            {
                xmlFolderPath = value;
            }
        }

        private static List<string> bhytNdsIcdCodeTe;
        public static List<string> BHYT_NDS_ICD_CODE__TE
        {
            get
            {
                if (bhytNdsIcdCodeTe != null)
                {
                    bhytNdsIcdCodeTe = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__TE_CFG);
                }
                return bhytNdsIcdCodeTe;
            }
            set
            {
                bhytNdsIcdCodeTe = value;
            }
        }

        private static List<string> bhytNdsIcdCodeOther;
        public static List<string> BHYT_NDS_ICD_CODE__OTHER
        {
            get
            {
                if (bhytNdsIcdCodeOther != null)
                {
                    bhytNdsIcdCodeOther = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__OTHER_CFG);
                }
                return bhytNdsIcdCodeOther;
            }
            set
            {
                bhytNdsIcdCodeOther = value;
            }
        }

        public static void Reload()
        {
            var autoExportXml = ConfigUtil.GetIntConfig(IS_AUTO_EXPORT_XML_CFG) == 1;
            var folderPath = ConfigUtil.GetStrConfig(XML_FOLDER_PATH_CFG);
            var ndsIcdCodeTe = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__TE_CFG);
            var ndsIcdCodeOther = ConfigUtil.GetStrConfigs(BHYT_NDS_ICD_CODE__OTHER_CFG);
            bhytNdsIcdCodeTe = ndsIcdCodeTe;
            bhytNdsIcdCodeOther = ndsIcdCodeOther;
            isAutoExportXml = autoExportXml;
            xmlFolderPath = folderPath;
        }
    }
}
