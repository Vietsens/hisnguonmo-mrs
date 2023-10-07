using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisExecuteRoom;
using System.Threading;
using MOS.MANAGER.HisExpMestMedicine;
using System.Data;
using MOS.MANAGER.HisTreatmentEndType;
using IcdVn;
using SAR.EFMODEL.DataModels;
using Inventec.Common.FileFolder;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;
using System.IO;
using Inventec.Fss.Utility;
using System.Web;
using SAR.DAO.Sql;

namespace MRS.Processor.Mrs00715
{
    public class Mrs00715Processor : AbstractProcessor
    {
        private Mrs00715Filter filter;
        List<SAR.EFMODEL.DataModels.SAR_REPORT_TEMPLATE> listRdo = new List<SAR_REPORT_TEMPLATE>();

        CommonParam paramGet = new CommonParam();

        public Mrs00715Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00715Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                listRdo = new SqlDAO().GetSql<SAR_REPORT_TEMPLATE>("select * from sar_report_template", new object[0]);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                string AppPath = HttpRuntime.AppDomainAppPath;
                string strFolderName = string.Format(@"{0}\\bin\\BeautifulTempReport", AppPath);
                DirectoryInfo Dir = new DirectoryInfo(strFolderName);

                List<FileInfo> files = Dir.GetFiles().ToList();
                foreach (var item in listRdo)
                {
                    bool IsExists = false;
                    ResultFile templateFile = null;
                    try
                    {
                        templateFile = JsonConvert.DeserializeObject<ResultFile>(item.REPORT_TEMPLATE_URL ?? "");

                        WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["fss.uri.base"] + templateFile.URL);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusDescription == "OK")
                        {
                            IsExists = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (!IsExists)
                    {

                        LogSystem.Info(item.REPORT_TEMPLATE_CODE+" start update");
                        try
                        {
                            FileInfo file = files.FirstOrDefault(o => o.Name.ToLower().StartsWith(item.REPORT_TEMPLATE_CODE.ToLower().Substring(0, 8)));
                            
                            using (MemoryStream TemplateStream = new MemoryStream())
                            {
                                byte[] byteArray = ReadAllBytes2(file.FullName);
                                if (byteArray.Length > 0)
                                {
                                    TemplateStream.Write(byteArray, 0, (int)byteArray.Length);
                                    TemplateStream.Position = 0;
                                }

                                FileUploadInfo fileupload = Inventec.Fss.Client.FileUpload.UploadFile("SAR", "ReportTemplate", TemplateStream, file.Name, true);

                                if (fileupload != null)
                                {
                                    TemplateADO url = new TemplateADO();
                                    url.FILE_NAME = fileupload.OriginalName;
                                    url.URL = fileupload.Url;
                                    url.EXTENSION = fileupload.OriginalName.Split('.').Last();
                                    //string test = fileupload.Url.Split('\\').LastOrDefault();
                                    //string Url = "{\"FILE_NAME\":\"" + h + "\",\"URL\":\"\\\\\\\\Upload\\\\\\\\SAR\\\\\\\\ReportTemplate\\\\\\\\" + h + "\",\"EXTENSION\":\"\"}";
                                    var temUrl = Newtonsoft.Json.JsonConvert.SerializeObject(url);
                                    item.REPORT_TEMPLATE_URL = temUrl;
                                    new SAR.DAO.Sql.SqlDAO().Execute(string.Format("update sar_report_template set report_template_url='{0}' where id={1} ", item.REPORT_TEMPLATE_URL, item.ID));

                                    LogSystem.Info(item.REPORT_TEMPLATE_CODE+ " update successful");
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            LogSystem.Debug(item.REPORT_TEMPLATE_CODE + " cannot update");
                        }

                    }



                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private byte[] ReadAllBytes2(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
           

        }
    }
    internal class TemplateADO
    {
        public string FILE_NAME { get; set; }
        public string URL { get; set; }
        public string EXTENSION { get; set; }
    }
}
