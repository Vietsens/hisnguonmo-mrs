using FlexCel.Report;
using Inventec.Common.DateTime;
using Inventec.Common.FileFolder;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using Inventec.Fss.Client;
using Inventec.Token.ResourceSystem;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.Lib;
using MRS.MANAGER.Core.MrsReport.RDO;
using MRS.MANAGER.Core.MrsReport.ReportException;
using MRS.MANAGER.Manager;
using MRS.SDO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportTemplate.Get;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport
{
    public abstract class AbstractProcessor : BeanObjectBase
    {
        private const string REPORT_EXTENSION_XLSX = ".xlsx";
        private const string REPORT_EXTENSION_PDF = ".pdf";
        private string currentTokenCode;
        private string userName;
        protected long branch_id;

        const string TFlexCelUFIcdVnCode = "FlFuncIcdVnCode";

        const string TFlexCelUFElement = "FlFuncElement";

        const string TFlexCelUFCompressString = "FlFuncCompressString";

        const string TFlexCelUFDecompressString = "FlFuncDecompressString";

        const string TFlexCelUFDbValueOfJson = "FlFuncDbValueOfJson";

        const string TFlexCelUFSerializeObject = "FlFuncSerializeObject";

        const string TFlexCelUFDayOfTreatment6556 = "FlFuncDayOfTreatment6556";

        const string TFlexCelUFTextInList = "FlFuncTextInList";

        const string TFlexCelUFTimeSpan = "FlFuncTimeSpan";

        const string TFlexCelUFSelectSheet = "FlFuncSelectSheet";

        const string TFlexCelUFSelectSheetChart = "FlFuncSelectSheetChart";

        const string TFlexCelUFDistinctListText = "FlFuncDistinctListText";

        private SAR_REPORT report;
        protected SAR_REPORT_TYPE reportType;
        protected SAR_REPORT_TEMPLATE reportTemplate;

        private SarReportUtil sarReportUtil;
        private MemoryStream resultStream;
        private MemoryStream templateStream;
        public byte[] templateClientData;
        protected string ReportTypeCode { get; set; }
        public string ReportTemplateCode;
        protected object reportFilter;

        protected Dictionary<string, object> dicDataFilter;

        private MemoryStream resultStreamPDF;

        public abstract Type FilterType();

        public Dictionary<string, object> dicOtherReport = new Dictionary<string, object>();
        protected abstract bool GetData();
        protected abstract bool ProcessData();
        protected abstract void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store);

        public AbstractProcessor(CommonParam param, string reportTypeCode)
            : base(param)
        {
            try
            {
                this.reportType = SarReportTypeCFG.REPORT_TYPE_ACTIVE != null ?
                SarReportTypeCFG.REPORT_TYPE_ACTIVE
                .Where(o => o.REPORT_TYPE_CODE == reportTypeCode).FirstOrDefault() : null;
                if (this.reportType == null)
                    this.reportType = new SAR.MANAGER.Manager.SarReportTypeManager(new CommonParam()).Get<SAR_REPORT_TYPE>(reportTypeCode);

                if (this.reportType == null)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                    throw new ReportTypeNotFoundException(reportTypeCode);
                }

                this.sarReportUtil = new SarReportUtil(param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        public SAR_REPORT Run(CreateReportSDO data)
        {
            try
            {
                if (data.Filter == null)
                {
                    throw new Exception("Filter null");
                }
                data.Filter = data.Filter.ToString().Replace("[]", "null");
                this.dicDataFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.Filter.ToString());
                this.dicDataFilter.Add("SENDER_LOGINNAME", data.Loginname);
                this.dicDataFilter.Add("SENDER_BRANCH_ID", data.BranchId);
                this.ReportTemplateCode = data.ReportTemplateCode;
                this.reportFilter = JsonConvert.DeserializeObject(data.Filter.ToString(), this.FilterType());

                Thread.SetData(TokenCodeStore.SlotTokenCode, ResourceTokenManager.GetTokenCode());

                if (this.CreateReport(data))
                {
                    try
                    {
                        this.ThreadInit();
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return this.report;
        }

        private void ThreadInit()
        {
            try
            {
                Thread thread = new Thread(new ParameterizedThreadStart(this.ThreadProcess));
                //thread.Priority = ThreadPriority.Highest;
                try
                {
                    thread.Start(ResourceTokenManager.GetTokenCode());
                }
                catch (Exception ex)
                {
                    LogSystem.Error("Khoi tao tien trinh xu ly yeu cau bao cao that bai", ex);
                    thread.Abort();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Khoi tao tien trinh xu ly yeu cau bao cao that bai", ex);
            }
        }

        private void ThreadProcess(object tokenCode)
        {
            bool success = true;
            try
            {
                Thread.SetData(TokenCodeStore.SlotTokenCode, tokenCode);

                LogSystem.Info(string.Format("Bat dau tien trinh xu ly bao cao {0}. TokenCode: {1}", this.reportType.REPORT_TYPE_CODE, tokenCode));

                this.currentTokenCode = (tokenCode ?? "").ToString();
                report.REPORT_STT_ID = IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__DXL;
                report.START_TIME = Inventec.Common.DateTime.Get.Now();

                this.sarReportUtil.Update(report);
                AddFilterFey(dicDataFilter);
                success = success && this.GetReportOther();
                success = success && this.GetQcsOther();
                success = success && this.GetExportData();
                success = success && this.ProcessExportData();
                RemoveEditSql();
                success = success && this.UploadFile();

                report.REPORT_STT_ID = success ? IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__HT : IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__LOI;
                report.FINISH_TIME = Inventec.Common.DateTime.Get.Now();

                this.sarReportUtil.Update(report);
                LogSystem.Info(string.Format("Ket thuc tien trinh xu ly bao cao {0}. TokenCode: {1}", this.reportType.REPORT_TYPE_CODE, tokenCode));
            }
            catch (Exception ex)
            {
                report.ERROR = ex.Message;
                report.REPORT_STT_ID = IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__LOI;
                this.sarReportUtil.Update(report);
                LogSystem.Error(ex);
            }
        }

        private bool GetReportOther()
        {
            var result = true;
            try
            {
                LogSystem.Info(string.Format("Bat dau lay du lieu bao cao bao cao khac {0}", this.reportType.REPORT_TYPE_CODE));
                List<string> InfoOtherKey = this.GetKeyOther();
                this.GenOtherKey(InfoOtherKey);
                List<string> InfoOtherReport = this.GetInfoReportOther();
                Dictionary<string, MRS.SDO.CreateReportSDO> dicReportCreateSdo = this.GenReportSDO(InfoOtherReport);
                this.EditSql();
                this.CreateOtherReport(dicReportCreateSdo);
                List<string> OtherSqls = this.GetOtherDataSql();
                this.CreateOtherData(OtherSqls);

            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                result = true;
            }
            return result;
        }

        private bool GetQcsOther()
        {
            var result = true;
            try
            {
                LogSystem.Info(string.Format("Bat dau lay du lieu bao cao server khac {0}", this.reportType.REPORT_TYPE_CODE));
                List<string> InfoOtherKey = this.GetKeyQcsOther();
                Dictionary<string, string> dicTdo = this.GenDicTdo(InfoOtherKey);
                List<string> OtherQcs = this.GetOtherDataQcs();
                this.CreateQcsData(OtherQcs, dicTdo);
                List<DataTable> ExcelData = this.GetExcelData();
                this.CreateExcelData(ExcelData);

            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                result = true;
            }
            return result;
        }

        private void CreateExcelData(List<DataTable> ExcelData)
        {
            foreach (var item in ExcelData)
            {
                if (this.dicOtherReport.ContainsKey(item.TableName)) continue;
                this.dicOtherReport[item.TableName] = item ?? new DataTable();
            }
        }

        private void CreateOtherData(List<string> queryValueCells)
        {
            if (queryValueCells != null && queryValueCells.Exists(o => o.ToUpper().Contains("SELECT")))
            {
                for (int i = 0; i < 15; i++)
                {
                    if (this.dicOtherReport.ContainsKey(string.Format("OtherData{0}", i))) continue;
                    var data = new ManagerSql().GetSum(this.dicDataFilter, queryValueCells[i]) ?? new List<DataTable>();
                    this.dicOtherReport[string.Format("OtherData{0}", i)] = data.FirstOrDefault() ?? new DataTable();
                }
            }
        }

        private void CreateQcsData(List<string> queryValueCells,Dictionary<string,string> DicTdo)
        {
            if (queryValueCells != null && queryValueCells.Exists(o => o.ToUpper().Contains("SELECT")))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var item in DicTdo)
                {
                    if(!item.Key.EndsWith("_"))
                    dic.Add(item.Key,item.Value);
                }
                for (int i = 0; i < 15; i++)
                {
                    List<string> keyi = DicTdo.Keys.Where(o => o.EndsWith(string.Format("{0}_",i))).ToList();
                    if (this.dicOtherReport.ContainsKey(string.Format("QcsData{0}", i))) continue;
                    foreach (var key in keyi)
                    {
                        if (dic.ContainsKey(key.Replace(string.Format("{0}_", i), "")))
                        {
                            dic[key.Replace(string.Format("{0}_", i), "")] = DicTdo[key];
                        }
                    }
                    var data = new ManagerQcs().GetSum(this.dicDataFilter, dic, queryValueCells[i]) ?? new List<DataTable>();
                    foreach (var key in keyi)
                    {
                        if (dic.ContainsKey(key.Replace(string.Format("{0}_", i), "")))
                        {
                            dic[key.Replace(string.Format("{0}_", i), "")] = DicTdo[key.Replace(string.Format("{0}_", i), "")];
                        }
                    }
                    this.dicOtherReport[string.Format("QcsData{0}", i)] = data.FirstOrDefault() ?? new DataTable();
                }
            }
        }

        private void CreateOtherReport(Dictionary<string, MRS.SDO.CreateReportSDO> reportCreateSdos)
        {
            foreach (var item in reportCreateSdos)
            {
                ReportResultSDO result = GetDataReport(item.Value);
                if (item.Key != null)
                {
                    string[] newKey = item.Key.Split(',');
                    string[] oldKey = item.Value.ListKeyAllow.Split(',');
                    for (int i = 0; i < newKey.Length; i++)
                    {
                        if (this.dicOtherReport.ContainsKey(newKey[i])) continue;
                        if (result != null && result.DATA_DETAIL != null && result.DATA_DETAIL.Count > i && oldKey.Length > i)
                        {
                            this.dicOtherReport[newKey[i]] = result.DATA_DETAIL[oldKey[i]];


                        }
                    }
                }
            }
        }

        private Dictionary<string, CreateReportSDO> GenReportSDO(List<string> infoOtherReport)
        {
            Dictionary<string, CreateReportSDO> result = new Dictionary<string, CreateReportSDO>();
            try
            {
                if (infoOtherReport == null || infoOtherReport.Count == 0)
                    throw new Exception("Hasn't other report");
                String[] separator = new String[] { "->" };

                for (int i = 0; i < 1000; i++)
                {
                    if (string.IsNullOrWhiteSpace(infoOtherReport[3 * i]) || string.IsNullOrWhiteSpace(infoOtherReport[3 * i + 1]))
                        break;
                    if (result.ContainsKey(infoOtherReport[3 * i]))
                        continue;
                    CreateReportSDO sdo = new CreateReportSDO();
                    string[] otherReport = infoOtherReport[3 * i + 1].Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (otherReport.Length != 3)
                    {
                        throw new Exception(string.Format("Cannot create report {0}. Because Report information is wrong({1})", infoOtherReport[3 * i], infoOtherReport[3 * i + 1]));
                    }
                    sdo.ReportTypeCode = otherReport[0];
                    sdo.ReportTemplateCode = otherReport[1];
                    sdo.ListKeyAllow = otherReport[2];
                    Dictionary<string, object> dicfilter = new Dictionary<string, object>();
                    bool IsThisReport = false;
                    if (!string.IsNullOrWhiteSpace(infoOtherReport[3 * i + 2]))
                    {
                        string[] relationShipFilter = infoOtherReport[3 * i + 2].Split(';');
                        if (relationShipFilter != null && relationShipFilter.Length > 0)
                        {
                            foreach (var item in relationShipFilter)
                            {
                                string[] cupFilter = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                if (cupFilter.Length != 2 || string.IsNullOrWhiteSpace(cupFilter[0]) || string.IsNullOrWhiteSpace(cupFilter[1]))
                                {
                                    throw new Exception(string.Format("Cannot create report {0}. Because Filter information is wrong({1})", infoOtherReport[3 * i], infoOtherReport[3 * i + 2]));
                                }
                                if (this.dicDataFilter.ContainsKey(cupFilter[1]) && !dicfilter.ContainsKey(cupFilter[0]))
                                {
                                    if (this.reportType.REPORT_TYPE_CODE == sdo.ReportTypeCode)
                                    {
                                        IsThisReport = true;
                                        this.dicDataFilter[cupFilter[1]] = cupFilter[0];

                                        //return new Dictionary<string, CreateReportSDO>();
                                    }
                                    else
                                    {
                                        dicfilter.Add(cupFilter[0], this.dicDataFilter[cupFilter[1]]);
                                    }
                                }
                            }
                        }
                    }
                    sdo.Filter = JsonConvert.SerializeObject(dicfilter);
                    if (IsThisReport == false)
                    {
                        result.Add(infoOtherReport[3 * i], sdo);
                        Inventec.Common.Logging.LogSystem.Info(string.Format("Filter{0}:{1}", infoOtherReport[3 * i], Newtonsoft.Json.JsonConvert.SerializeObject(sdo)));
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        private void GenOtherKey(List<string> infoOtherKey)
        {
            try
            {
                if (infoOtherKey == null || infoOtherKey.Count == 0)
                    throw new Exception("Hasn't other Key");

                for (int i = 0; i < 1000; i++)
                {
                    if (string.IsNullOrWhiteSpace(infoOtherKey[2 * i]) || string.IsNullOrWhiteSpace(infoOtherKey[2 * i + 1]))
                        break;
                    if (this.dicDataFilter != null && this.dicDataFilter.ContainsKey(infoOtherKey[2 * i]))
                    {
                        string stringValue = JsonConvert.SerializeObject(this.dicDataFilter[infoOtherKey[2 * i]]);
                        if (stringValue == "null" || stringValue == "\"\"" || stringValue == "[]" || stringValue == "{}" || stringValue == "0" || stringValue.ToLower() == "false")
                        {
                            this.dicDataFilter[infoOtherKey[2 * i]] = infoOtherKey[2 * i + 1];
                        }
                    }
                    else if (this.dicDataFilter != null)
                    {
                        this.dicDataFilter.Add(infoOtherKey[2 * i], infoOtherKey[2 * i + 1]);
                    }
                }
                //[\\\"ss1\\\",\\\"ss2\\\"]
                this.reportFilter = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this.dicDataFilter).Replace("\"[", "[").Replace("]\"", "]").Replace("\"[", "[").Replace("]\"", "]").Replace("\\\"", "\""), this.FilterType());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private Dictionary<string,string> GenDicTdo(List<string> infoApiKey)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (infoApiKey == null || infoApiKey.Count == 0)
                    throw new Exception("Hasn't Qcs Other Key");

                for (int i = 0; i < 1000; i++)
                {
                    if (string.IsNullOrWhiteSpace(infoApiKey[2 * i]) || string.IsNullOrWhiteSpace(infoApiKey[2 * i + 1]))
                        break;
                    if (result != null && !result.ContainsKey(infoApiKey[2 * i]))
                    {
                        result.Add(infoApiKey[2 * i], infoApiKey[2 * i + 1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        ReportResultSDO GetDataReport(CreateReportSDO createReportSDO)
        {
            ReportResultSDO result = null;
            try
            {
                var rs = new MRS.MANAGER.Manager.MrsReportManager(new CommonParam()).CreateData(createReportSDO);
                result = rs as ReportResultSDO;

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = null;
            }

            return result;
        }

        private List<string> GetInfoReportOther()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 10, 1, 1000, 3, "ReportOther");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> GetInfoReplaceSql()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 10, 1, 1000, 3, "ReplaceSql", this.dicDataFilter);

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> GetOtherDataSql()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 1, 1, 15, "ReportOther");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> GetOtherDataQcs()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 1, 1, 15, "QcsOther");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<DataTable> GetExcelData()
        {
            List<DataTable> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetDataBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 1, 1, 15, "ExcelData");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> GetKeyOther()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 10, 13, 1000, 2, "ReportOther");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> GetKeyQcsOther()
        {
            List<string> result = null;
            try
            {
                result = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 10, 13, 1000, 2, "QcsOther");

            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool CreateReport(CreateReportSDO data)
        {
            bool result = false;
            try
            {
                if (this.Validate(data))
                {
                    this.userName = data.Username;
                    this.report = new SAR_REPORT();
                    this.report.REPORT_TYPE_ID = this.reportType.ID;
                    this.report.REPORT_TEMPLATE_ID = this.reportTemplate.ID;
                    this.report.REPORT_STT_ID = IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__CXL;
                    this.report.JSON_FILTER = this.dicDataFilter != null ? JsonConvert.SerializeObject(this.dicDataFilter, Formatting.None) : null;
                    this.report.REPORT_NAME = data.ReportName;
                    this.report.DESCRIPTION = data.Description;
                    this.report.CREATOR = !String.IsNullOrEmpty(data.Loginname) ? data.Loginname : ResourceTokenManager.GetLoginName();
                    this.report.MODIFIER = !String.IsNullOrEmpty(data.Loginname) ? data.Loginname : ResourceTokenManager.GetLoginName();
                    this.branch_id = data.BranchId;
                    result = this.sarReportUtil.Create(this.report, ref this.report);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool ProcessExportData()
        {
            var result = false;
            try
            {
                LogSystem.Info(string.Format("Bat dau xu ly du lieu bao cao {0}", this.reportType.REPORT_TYPE_CODE));
                if (this.ProcessData())
                {
                    report.FINISH_PREPARE_DATA_TIME = Get.Now();
                    LogSystem.Info(string.Format("Ket thuc xu ly du lieu bao cao {0}", this.reportType.REPORT_TYPE_CODE));

                    report.START_GENERATE_FILE_TIME = Get.Now();
                    var store = new Store();
                    var singleTag = new ProcessSingleTag();
                    var gridTag = new ProcessObjectTag();

                    var dicSingleData = new Dictionary<string, object>();
                    this.SetCommonSingleKey(dicSingleData);
                    LogSystem.Info(string.Format("Bat dau xu ly bieu mau bao cao {0}", this.reportType.REPORT_TYPE_CODE));
                    this.SetTag(dicSingleData, gridTag, store);
                    this.AddFilterFey(dicSingleData);
                    this.AddFilterKeyName(dicSingleData);
                    this.AddObjectOther(gridTag, store);
                    var exportSuccess = true;
                    exportSuccess = exportSuccess && store.ReadTemplate(this.templateStream);
                    exportSuccess = exportSuccess && singleTag.ProcessData(store, dicSingleData);
                    if (exportSuccess)
                    {
                        store.SetCommonFunctions();
                        this.SetCommonFunctions(store.flexCel);
                        this.resultStream = store.OutStream();
                        LogSystem.Info(string.Format("Ket thuc xu ly bieu mau bao cao {0}", this.reportType.REPORT_TYPE_CODE));

                        if (Base.ManagerConstant.GetExportPdf == "1")
                        {
                            try
                            {
                                this.resultStreamPDF = store.OutStreamPDF();
                            }
                            catch (Exception ex)
                            {
                                Inventec.Common.Logging.LogSystem.Error(ex);
                            }
                        }

                        if (AbsProcessDelegate.ProcessMrs != null)
                        {
                            AbsProcessDelegate.ProcessMrs(ref store, ref this.resultStream);
                            AbsProcessDelegate.ProcessMrs = null;
                        }

                        result = exportSuccess && (this.resultStream != null && this.resultStream.Length > 0);

                        if (!result)
                        {
                            report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiTemplate);
                        }
                    }
                }
                else
                {
                    if (reportType.REPORT_TYPE_CODE.StartsWith("TKB"))
                    {
                        report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiTruyVan);
                    }
                    else
                    {
                        report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiXuLyDuLieu);
                    }
                }
            }
            catch (Exception ex)
            {
                report.ERROR = report.ERROR ?? "" + ex.Message;
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddObjectOther(ProcessObjectTag gridTag, Store store)
        {
            foreach (var item in this.dicOtherReport)
            {
                try
                {
                    var value = item.Value;
                    if (value != null)
                    {
                        if (value is DataTable)
                        {
                            gridTag.AddObjectData(store, item.Key, value as DataTable);
                        }
                        else
                        {
                            gridTag.AddObjectData(store, item.Key, GetDataTableFromObjects(value)); 
                        }
                    }
                    else
                    {
                        gridTag.AddObjectData(store, item.Key, new DataTable());
                    }

                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }
            }
        }
        public DataTable GetDataTableFromObjects(object reportOther)
        {
            DataTable result = null;
            try
            {
                IEnumerable<object> enumerable = reportOther as IEnumerable<object>;
                object[] objects = enumerable.Cast<object>().ToArray();
                if (objects != null && objects.Length > 0)
                {

                    Type t = objects[0].GetType();

                    result = new DataTable(t.Name);

                    foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        Type propType = pi.PropertyType; 
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            propType = new NullableConverter(propType).UnderlyingType; 
                        result.Columns.Add(new DataColumn(pi.Name));

                    }

                    foreach (var o in objects)
                    {

                        DataRow dr = result.NewRow();

                        foreach (DataColumn dc in result.Columns)
                        {

                            dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);

                        }

                        result.Rows.Add(dr);

                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }
            return result;
            
            //    return dt;

            //}

            //return null;

        }

        private void AddFilterFey(Dictionary<string, object> dicSingleData)
        {
            foreach (var item in this.dicDataFilter)
            {
                try
                {
                    var value = item.Value;
                    if (!dicSingleData.ContainsKey(item.Key))
                    {
                        if (value != null)
                        {
                            dicSingleData[item.Key] = value.GetType() == typeof(Newtonsoft.Json.Linq.JArray) ? Newtonsoft.Json.JsonConvert.SerializeObject(value) : value;
                        }
                        else
                        {
                            dicSingleData[item.Key] = "";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }
            }
        }

        private void AddFilterKeyName(Dictionary<string, object> dicSingleData)
        {
            foreach (var item in this.dicDataFilter)
            {
                try
                {
                    var value = item.Value;


                    string tableName = MRS.MANAGER.Core.MrsReport.Lib.GetTableName.HisFilterTypes(item.Key);
                    if (tableName != null && tableName.Length > 4)
                    {
                        if (value != null)
                        {
                            string query = string.Format("select LISTAGG({0}_NAME, '; ') WITHIN GROUP (ORDER BY id) name,LISTAGG({0}_CODE, '; ') WITHIN GROUP (ORDER BY id) code from {1} where id in('{2}')", tableName.Replace("V_HIS_", "").Replace("HIS_", ""), tableName, value.GetType() == typeof(Newtonsoft.Json.Linq.JArray) ? string.Join("','", ((JArray)value).Take(100).ToList()) : value);
                            DataGet result = new MOS.DAO.Sql.SqlDAO().GetSqlSingle<DataGet>(query) ?? new DataGet();
                            if (!dicSingleData.ContainsKey(string.Format("{0}_NAME", item.Key)))
                            {
                                dicSingleData[string.Format("{0}_NAME", item.Key)] = result.NAME;
                            }
                            if (!dicSingleData.ContainsKey(string.Format("{0}_CODE", item.Key)))
                            {
                                dicSingleData[string.Format("{0}_CODE", item.Key)] = result.CODE;
                            }
                        }
                        else
                        {
                            if (!dicSingleData.ContainsKey(string.Format("{0}_NAME", item.Key)))
                            {
                                dicSingleData[string.Format("{0}_NAME", item.Key)] = "";
                            }
                            if (!dicSingleData.ContainsKey(string.Format("{0}_CODE", item.Key)))
                            {
                                dicSingleData[string.Format("{0}_CODE", item.Key)] = "";
                            }
                        }
                    }




                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }
            }
        }

        public bool SetCommonFunctions(FlexCelReport flexCel)
        {
            bool result = false;
            try
            {
                if (flexCel == null) throw new ArgumentNullException("flexCel");

                flexCel.SetUserFunction(TFlexCelUFIcdVnCode, new RDOIcdVnCode());

                flexCel.SetUserFunction(TFlexCelUFElement, new RDOElement());

                flexCel.SetUserFunction(TFlexCelUFCompressString, new RDOCompressString());

                flexCel.SetUserFunction(TFlexCelUFDecompressString, new RDODecompressString());

                flexCel.SetUserFunction(TFlexCelUFDbValueOfJson, new RDODbValueOfJson());

                flexCel.SetUserFunction(TFlexCelUFSerializeObject, new RDOSerializeObject());

                flexCel.SetUserFunction(TFlexCelUFDayOfTreatment6556, new RDODayOfTreatment6556());

                flexCel.SetUserFunction(TFlexCelUFTextInList, new RDOTextInList());

                flexCel.SetUserFunction(TFlexCelUFTimeSpan, new RDOTimeSpan());

                flexCel.SetUserFunction(TFlexCelUFSelectSheet, new RDOSelectSheet());

                flexCel.SetUserFunction(TFlexCelUFSelectSheetChart, new RDOSelectSheetChart());

                flexCel.SetUserFunction(TFlexCelUFDistinctListText, new RDODistinctListText());

                result = true;
            }
            catch (ArgumentNullException ex)
            {
                LogSystem.Warn(ex);
                result = false;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool GetExportData()
        {
            var result = false;
            try
            {
                LogSystem.Info(string.Format("Bat dau lay du lieu bao cao {0}", this.reportType.REPORT_TYPE_CODE));
                report.START_PREPARE_DATA_TIME = Get.Now();
                result = this.GetData();
                report.FINISH_QUERY_TIME = Get.Now();

                if (!result)
                {
                    report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiGetData);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                report.ERROR = ex.Message;
                result = false;
            }
            return result;
        }

        private void EditSql()
        {
            try
            {
                LogSystem.Info(string.Format("Bat dau sua sql bao cao {0}", this.reportType.REPORT_TYPE_CODE));
                MOS.DAO.Sql.EditSql editSql = GetInfoReplaceSql;
                if (MOS.DAO.Sql.ProcessDelegate.EditSql == null)
                {
                    MOS.DAO.Sql.ProcessDelegate.EditSql = new Dictionary<long, MOS.DAO.Sql.EditSql>();
                }
                Thread thr;
                thr = Thread.CurrentThread;
                int threadId = thr.ManagedThreadId;
                if (MOS.DAO.Sql.ProcessDelegate.EditSql.ContainsKey(threadId))
                {
                    MOS.DAO.Sql.ProcessDelegate.EditSql[threadId] = editSql;
                }    
                else
                {
                    MOS.DAO.Sql.ProcessDelegate.EditSql.Add(threadId,editSql);
                }    
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                report.ERROR = ex.Message;
            }
        }


        private void RemoveEditSql()
        {
            try
            {
                Thread thr;
                thr = Thread.CurrentThread;
                int threadId = thr.ManagedThreadId;
                
                if (MOS.DAO.Sql.ProcessDelegate.EditSql != null && MOS.DAO.Sql.ProcessDelegate.EditSql.ContainsKey(threadId))
                {
                    MOS.DAO.Sql.ProcessDelegate.EditSql.Remove(threadId);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                report.ERROR = ex.Message;
            }
        }
        private bool UploadFile()
        {
            bool result = false;
            try
            {
                LogSystem.Info(string.Format("Bat dau xuat file bao cao {0}", this.reportType.REPORT_TYPE_CODE));

                var fileUploadInfo = FileUpload.UploadFile(ManagerConstant.FSS_CLIENT_CODE, FileStoreLocation.DOWNLOAD_FOLDER, this.resultStream, report.REPORT_NAME + REPORT_EXTENSION_XLSX);
                if (Base.ManagerConstant.GetExportPdf == "1")
                {
                    try
                    {
                        var fileUploadPdfInfo = FileUpload.UploadFile(ManagerConstant.FSS_CLIENT_CODE, FileStoreLocation.DOWNLOAD_FOLDER, resultStreamPDF, report.REPORT_NAME + REPORT_EXTENSION_PDF);

                        if (fileUploadPdfInfo != null)
                        {
                            report.REPORT_URL_PDF = fileUploadPdfInfo.Url;
                        }
                    }
                    catch (Exception ex)
                    {
                        report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiFss) + ex.Message;
                        Inventec.Common.Logging.LogSystem.Error(ex);
                    }
                }

                if (fileUploadInfo == null)
                {
                    report.IS_URL_ERROR = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                }
                else
                {
                    report.REPORT_URL = fileUploadInfo.Url;
                    result = true;
                }

                this.report.REPORT_STT_ID = IMSys.DbConfig.SAR_RS.SAR_REPORT_STT.ID__HT;
                LogSystem.Info(string.Format("Hoan thanh xuat file va day len FSS bao cao  {0}", this.reportType.REPORT_TYPE_CODE));
            }
            catch (Exception ex)
            {
                report.ERROR = MANAGER.Base.MessageUtil.GetMessage(LibraryMessage.Message.Enum.Core_MrsReport_Create__LoiFss) + ex.Message;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void SetCommonSingleKey(Dictionary<string, object> dicSingleData)
        {
            try
            {
                System.DateTime now = DateTime.Now;
                dicSingleData.Add("CURRENT_TIME_STR", now.ToString("dd/MM/yyyy HH:mm:ss"));
                dicSingleData.Add("CURRENT_DATE_STR", now.ToString("dd/MM/yyyy"));
                dicSingleData.Add("CURRENT_MONTH_STR", now.ToString("MM/yyyy"));
                dicSingleData.Add("CURRENT_DATE_SEPARATE_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateSeparateString(now));
                dicSingleData.Add("CURRENT_MONTH_SEPARATE_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToMonthSeparateString(now));
                if (this.report != null)
                {
                    dicSingleData.Add("REPORT_CODE", report.REPORT_CODE);
                    dicSingleData.Add("JSON_FILTER", report.JSON_FILTER);
                    if (String.IsNullOrEmpty(this.userName))
                    {
                        dicSingleData.Add("REPORTER", this.report.CREATOR);
                    }
                    else
                    {
                        dicSingleData.Add("REPORTER", this.report.CREATOR + " (" + this.userName + ")");
                        dicSingleData.Add("CREATOR_USERNAME", this.userName);
                    }
                }
                if (this.reportType != null)
                {
                    dicSingleData.Add("REPORT_TYPE_CODE", this.reportType.REPORT_TYPE_CODE);
                    dicSingleData.Add("REPORT_TYPE_NAME", this.reportType.REPORT_TYPE_NAME.Trim().ToUpper());
                    dicSingleData.Add("DESCRIPTION", this.reportType.DESCRIPTION);
                }

                if (this.reportTemplate != null)
                {
                    dicSingleData.Add("REPORT_TEMPLATE_CODE", this.reportTemplate.REPORT_TEMPLATE_CODE);
                    dicSingleData.Add("REPORT_TEMPLATE_URL", this.reportTemplate.REPORT_TEMPLATE_URL);
                    dicSingleData.Add("REPORT_TEMPLATE_NAME", this.reportTemplate.REPORT_TEMPLATE_NAME.Trim().ToUpper());
                }
                dicSingleData.Add("REPORT_TIME", now.ToString("dd/MM/yyyy HH:mm:ss"));


                var branch = Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.branch_id);
                if (branch != null)
                {
                    dicSingleData.Add("PARENT_ORGANIZATION_NAME", branch.PARENT_ORGANIZATION_NAME);
                    dicSingleData.Add("ORGANIZATION_NAME", branch.BRANCH_NAME);
                    dicSingleData.Add("ORGANIZATION_CODE", branch.BRANCH_CODE);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool Validate(CreateReportSDO data)
        {
            bool valid = true;
            try
            {
                this.reportTemplate = this.GetReportTemplate(data.ReportTemplateCode);
                if (this.reportTemplate == null)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                    throw new ReportTemplateNotFoundException(data.ReportTemplateCode);
                }

                if (String.IsNullOrWhiteSpace(this.reportTemplate.REPORT_TEMPLATE_URL))
                {
                    MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__BieuMauKhongCoThongTinFile);
                    throw new ReportTemplateUrlNullException(LogUtil.TraceData("reportTemplate", this.reportTemplate));
                }

                if (this.reportTemplate.REPORT_TYPE_ID != this.reportType.ID)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__LoaiBaoCaoVaBieuMauKhongCungCap);
                    throw new ReportTemplateNotMatchTypeException(LogUtil.TraceData("reportTemplate", this.reportTemplate) + LogUtil.TraceData("reportType", reportType));
                }

                ResultFile templateFile = JsonConvert.DeserializeObject<ResultFile>(this.reportTemplate.REPORT_TEMPLATE_URL);
                if (templateFile == null || String.IsNullOrEmpty(templateFile.URL))
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__KhongTimThayThongTinBieuMau);
                    throw new TemplateFileNotFoundException("Khong tim thay thong tin url bieu mau bao cao" + LogUtil.TraceData("reportTemplate", this.reportTemplate));
                }

                this.templateStream = null;
                try
                {
                    this.templateStream = FileDownload.GetFile(templateFile.URL);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }

                if (this.templateStream == null)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu);
                    MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu);
                    throw new TemplateFileNotFoundException("Khong tim duoc template tren may chu." + LogUtil.TraceData("URL", templateFile.URL));
                }

                if (CheckTimeCreate(this.reportType, param))
                {
                    throw new Exception("Goi han thoi gian bao cao");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private SAR_REPORT_TEMPLATE GetReportTemplate(string reportTemplateCode)
        {
            SAR_REPORT_TEMPLATE result = null;
            try
            {
                SarReportTemplateFilterQuery filter = new SarReportTemplateFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.SAR_RS.COMMON.IS_ACTIVE__TRUE;
                filter.REPORT_TEMPLATE_CODE = reportTemplateCode;
                var data = new SAR.MANAGER.Manager.SarReportTemplateManager(new CommonParam()).Get<List<SAR_REPORT_TEMPLATE>>(filter);
                result = IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private bool CheckTimeCreate(SAR_REPORT_TYPE data, CommonParam param)
        {
            bool result = false;
            try
            {
                long hourNow = long.Parse(DateTime.Now.ToString("HHmm"));
                if (!String.IsNullOrWhiteSpace(this.reportType.HOUR_FROM))
                {
                    if (!String.IsNullOrWhiteSpace(this.reportType.HOUR_TO))
                    {
                        if (long.Parse(this.reportType.HOUR_FROM) < hourNow && long.Parse(this.reportType.HOUR_TO) > hourNow)
                        {
                            MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__GoiHanThoiGianTuDen, ProcessHour(reportType.HOUR_FROM), ProcessHour(reportType.HOUR_TO));
                            result = true;
                        }
                    }
                    else
                    {
                        if (long.Parse(this.reportType.HOUR_FROM) < hourNow)
                        {
                            MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__GoiHanThoiGianTu, ProcessHour(reportType.HOUR_FROM));
                            result = true;
                        }
                    }
                }
                else if (!String.IsNullOrWhiteSpace(this.reportType.HOUR_TO) && long.Parse(this.reportType.HOUR_TO) > hourNow)
                {
                    MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__GoiHanThoiGianDen, ProcessHour(reportType.HOUR_TO));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ProcessHour(string p)
        {
            return string.Format("{0}:{1}", p.Substring(0, 2), p.Substring(2, 2));
        }

        internal ReportResultSDO RunData(CreateReportSDO data)
        {
            ReportResultSDO result = null;
            try
            {
                if (data.Filter == null)
                {
                    throw new Exception("Filter null");
                }

                data.Filter = data.Filter.ToString().Replace("[]", "null");
                this.dicDataFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.Filter.ToString());
                this.reportFilter = JsonConvert.DeserializeObject(data.Filter.ToString(), this.FilterType());

                if (string.IsNullOrWhiteSpace(data.ReportTemplateCode) || ValidateTemplate(data))
                {
                    Inventec.Common.Logging.LogSystem.Info("Check ReportTemplateCode. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data));
                }


                result = new ReportResultSDO();
                LogSystem.Info(string.Format("Bat dau tien trinh xu ly bao cao {0}", data.ReportTypeCode));
                bool success = true;
                LogSystem.Info(string.Format("Bat dau lay du lieu bao cao {0}", data.ReportTypeCode));
                this.GetReportOther();
                this.GetQcsOther();
                success = success && this.GetData();
                LogSystem.Info(string.Format("Bat dau xu ly du lieu bao cao {0}", data.ReportTypeCode));
                success = success && this.ProcessData();
                if (success)
                {
                    var store = new Store();
                    var gridTag = new ProcessObjectTag();
                    var singleTag = new ProcessSingleTag();
                    var dicSingleData = new Dictionary<string, object>();
                    this.SetCommonSingleKey(dicSingleData);
                    LogSystem.Info(string.Format("Bat dau xu ly bieu mau bao cao {0}", data.ReportTypeCode));
                    this.SetTag(dicSingleData, gridTag, store);

                    Dictionary<string, object> totalData = gridTag.GetTotalData;

                    result.DATA_DETAIL = new Dictionary<string, object>();
                    if (totalData != null)
                    {
                        foreach (var item in totalData)
                        {
                            if (data != null && !string.IsNullOrWhiteSpace(data.ListKeyAllow) && ("," + data.ListKeyAllow.Trim() + ",").IndexOf("," + item.Key.Trim() + ",") == -1)
                            {
                                continue;
                            }

                            result.DATA_DETAIL[item.Key] = item.Value;
                        }
                    }
                    result.SINGER_DATA = dicSingleData;

                }

                LogSystem.Info(string.Format("Ket thuc tien trinh xu ly bao cao {0}", data.ReportTypeCode));
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }


        internal ByteResultSDO RunByte(CreateByteSDO data)
        {
            ByteResultSDO result = null;
            try
            {
                if (data.Filter == null)
                {
                    throw new Exception("Filter null");
                }
                if (data.TEMPLATE_DATA != null && data.TEMPLATE_DATA.Length > 0)
                {
                    templateClientData = data.TEMPLATE_DATA;
                }
                else
                {
                    throw new Exception("data.TEMPLATE_DATA null");
                }

                data.Filter = data.Filter.ToString().Replace("[]", "null");
                this.dicDataFilter = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.Filter.ToString());
                this.reportFilter = JsonConvert.DeserializeObject(data.Filter.ToString(), this.FilterType());


                result = new ByteResultSDO();
                LogSystem.Info(string.Format("Bat dau tien trinh xu ly bao cao"));
                bool success = true;
                LogSystem.Info(string.Format("Bat dau lay du lieu bao cao"));
                this.GetReportOther();
                this.GetQcsOther();
                success = success && this.GetData();
                LogSystem.Info(string.Format("Bat dau xu ly du lieu bao cao"));
                success = success && this.ProcessData();
                if (success)
                {
                    var store = new Store();
                    var gridTag = new ProcessObjectTag();
                    var singleTag = new ProcessSingleTag();
                    var dicSingleData = new Dictionary<string, object>();
                    this.SetCommonSingleKey(dicSingleData);
                    LogSystem.Info(string.Format("Bat dau xu ly bieu mau bao cao"));
                    this.SetTag(dicSingleData, gridTag, store);
                    this.AddFilterFey(dicSingleData);
                    this.AddFilterKeyName(dicSingleData);
                    this.AddObjectOther(gridTag, store);
                    var exportSuccess = true;
                    using (var templateClientStream = new MemoryStream())
                    {
                        templateClientStream.Write(templateClientData, 0, (int)templateClientData.Length);
                        templateClientStream.Position = 0;
                        exportSuccess = exportSuccess && store.ReadTemplate(templateClientStream);
                    }
                    exportSuccess = exportSuccess && singleTag.ProcessData(store, dicSingleData);
                    if (exportSuccess)
                    {
                        store.SetCommonFunctions();
                        this.SetCommonFunctions(store.flexCel);
                        result.REPORT_DATA = ((MemoryStream)store.OutStream()).ToArray();
                    }


                }

                LogSystem.Info(string.Format("Ket thuc tien trinh xu ly bao cao"));
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool ValidateTemplate(CreateReportSDO data)
        {
            bool valid = false;
            try
            {
                this.reportTemplate = this.GetReportTemplate(data.ReportTemplateCode);
                if (this.reportTemplate == null)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                    throw new Exception(data.ReportTemplateCode);
                }

                if (String.IsNullOrWhiteSpace(this.reportTemplate.REPORT_TEMPLATE_URL))
                {
                    MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__BieuMauKhongCoThongTinFile);
                    throw new Exception(LogUtil.TraceData("reportTemplate", this.reportTemplate));
                }

                if (this.reportTemplate.REPORT_TYPE_ID != this.reportType.ID)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__LoaiBaoCaoVaBieuMauKhongCungCap);
                    throw new Exception(LogUtil.TraceData("reportTemplate", this.reportTemplate) + LogUtil.TraceData("reportType", reportType));
                }

                ResultFile templateFile = JsonConvert.DeserializeObject<ResultFile>(this.reportTemplate.REPORT_TEMPLATE_URL);
                if (templateFile == null || String.IsNullOrEmpty(templateFile.URL))
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__KhongTimThayThongTinBieuMau);
                    throw new Exception("Khong tim thay thong tin url bieu mau bao cao" + LogUtil.TraceData("reportTemplate", this.reportTemplate));
                }

                this.templateStream = null;
                try
                {
                    this.templateStream = FileDownload.GetFile(templateFile.URL);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }

                if (this.templateStream == null)
                {
                    MRS.MANAGER.Base.BugUtil.SetBugCode(param, MRS.LibraryBug.Bug.Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu);
                    MANAGER.Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu);
                    throw new Exception("Khong tim duoc template tren may chu." + LogUtil.TraceData("URL", templateFile.URL));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                valid = true;
            }
            return valid;
        }
    }
}
