using FlexCel.Report;
using Inventec.Common.FileFolder;
using Inventec.Common.Logging;
using Inventec.Fss.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.Lib
{
    public class ProcessExcel
    {
        public bool GetByCell<TFilter>(ref Dictionary<string, string> dicSingleKey, ref List<List<DataTable>> dataObject, TFilter filter, string ReportTemplateUrl, int Amount)
        {
            if (filter == null)
            {
                Inventec.Common.Logging.LogSystem.Debug("TFilter null");
                return false;
            }
            string jsonFilter = Newtonsoft.Json.JsonConvert.SerializeObject(filter, Newtonsoft.Json.Formatting.None);
            Dictionary<string, object> dicFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFilter);
            return GetByCell(ref dicSingleKey, ref dataObject, dicFilter, ReportTemplateUrl, Amount);
        }
        public bool GetByCell<TFilter>(ref Dictionary<string, string> dicSingleKey, ref List<List<DataTable>> dataObject, TFilter filter, Dictionary<string, object> dicFilterInput, string ReportTemplateUrl, int Amount)
        {
            if (filter == null)
            {
                Inventec.Common.Logging.LogSystem.Debug("TFilter null");
                return false;
            }
            string jsonFilter = Newtonsoft.Json.JsonConvert.SerializeObject(filter, Newtonsoft.Json.Formatting.None);
            Dictionary<string, object> dicFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFilter);
            if (dicFilterInput == null)
            {
                Inventec.Common.Logging.LogSystem.Debug("dicFilterInput null");
                return false;
            }
            foreach (var item in dicFilterInput)
            {
                try
                {
                    var value = item.Value;
                    if (!dicFilter.ContainsKey(item.Key))
                    {
                        dicFilter[item.Key] = value;
                    }

                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }
            }
            return GetByCell(ref dicSingleKey, ref dataObject, dicFilter, ReportTemplateUrl, Amount);
        }

        private bool GetByCell(ref Dictionary<string, string> dicSingleKey, ref List<List<DataTable>> dataObject,Dictionary<string,object> dicFilter, string ReportTemplateUrl, int Amount)
        {
            bool result = false;
            try
            {
                List<string> queryValueCells = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(ReportTemplateUrl, 1, 1, 1, Amount, 1);
                if (queryValueCells != null && queryValueCells.Exists(o => o.ToUpper().Contains("SELECT")))
                {
                    

                    for (int i = 0; i < 15; i++)
                    {
                        var data = new ManagerSql().GetSum(dicFilter, queryValueCells[i]);
                        dataObject.Add(data ?? new List<DataTable>());
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }
            return result;
        }

        public bool GetByCell(ref Dictionary<string, string> dicSingleKey, ref List<List<DataTable>> dataObject, Dictionary<string, object> dicFilter, byte[] ReportTemplateData, int Amount)
        {
            bool result = false;
            try
            {
                List<string> queryValueCells = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(ReportTemplateData, 1, 1, 1, Amount, 1);
                if (queryValueCells != null && queryValueCells.Exists(o => o.ToUpper().Contains("SELECT")))
                {


                    for (int i = 0; i < 15; i++)
                    {
                        var data = new ManagerSql().GetSum(dicFilter, queryValueCells[i]);
                        dataObject.Add(data ?? new List<DataTable>());
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }
            return result;
        }

    }
}
