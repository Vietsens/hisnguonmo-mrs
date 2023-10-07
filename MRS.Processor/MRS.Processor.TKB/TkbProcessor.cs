using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using Newtonsoft.Json;
using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportType.Get;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.TKB
{
    class TkbProcessor : AbstractProcessor
    {
        TKBFilter castFilter = null;

        private DataTable listRdo = new DataTable();
        private Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();
        Dictionary<string, object> dicMini = new Dictionary<string, object>();
        private string querry;
        List<List<DataTable>> dataObject = new List<List<DataTable>>();

        public TkbProcessor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(TKBFilter);
        }

        //xử lý filter json đưa vào dicSingleKey
        protected override bool GetData()
        {
            bool result = false;
            try
            {

                this.castFilter = (TKBFilter)reportFilter;
                if (this.templateClientData != null)
                {
                    new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell(ref dicSingleKey, ref dataObject, this.dicDataFilter, this.templateClientData, 15);
                    return true;
                }


                if (new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell<TKBFilter>(ref dicSingleKey, ref dataObject, this.castFilter, this.dicDataFilter, this.reportTemplate.REPORT_TEMPLATE_URL, 15))
                {
                    return true;
                }

                //get lại reportType tránh th sửa mà mrs chưa update lại dữ liệu
                SarReportTypeFilterQuery filter = new SarReportTypeFilterQuery();
                filter.REPORT_TYPE_CODE = this.reportType.REPORT_TYPE_CODE;
                var reports = new SAR.MANAGER.Manager.SarReportTypeManager(new CommonParam()).Get<List<SAR_REPORT_TYPE>>(filter);
                if (reports != null && reports.Count == 1)
                {
                    querry = ProcessSql(System.Text.Encoding.UTF8.GetString(reports.First().SQL));
                }

                if (string.IsNullOrWhiteSpace(querry))
                {
                    return false;
                }

                if (castFilter != null && castFilter.FILTER != null)
                {

                    //dua vao dic de tranh trung key
                    foreach (var item in castFilter.FILTER)
                    {
                        var filters = item.Split(':');
                        if (filters.Length == 2)
                        {
                            dicSingleKey[filters[0]] = filters[1];
                        }
                    }

                    if (dicSingleKey != null && dicSingleKey.Count > 0)
                    {
                        foreach (var item in dicSingleKey)
                        {
                            string value = ProcessValueType(item.Value);
                            querry = querry.Replace(":" + item.Key, value);
                            querry = querry.Replace("&" + item.Key, value);
                        }
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// dữ liệu kiểu số sẽ giữ nguyên
        /// dữ liệu kiểu chuỗi sẽ thêm dấu nháy
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string ProcessValueType(string p)
        {
            string result = "";
            try
            {
                bool isDigit = true;
                foreach (char item in p)
                {
                    isDigit = isDigit && char.IsDigit(item);
                }

                if (isDigit)
                {
                    result = p;
                }
                else
                {
                    result = string.Format("'{0}'", p);
                }
            }
            catch (Exception ex)
            {
                result = p;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ProcessSql(string sql)
        {
            string result = "";
            try
            {
                if (IsNotNull(sql))
                {
                    result = sql;
                    //nếu có ; sẽ chỉ lấy câu truy vấn từ đầu đến dấu ;
                    if (sql.Contains(";"))
                    {
                        //nếu có dấu nháy đơn kiểm tra xem dấu ; có nằm giữ 2 dấu nháy đơn không.
                        //nếu có thì vẫn lấy cả câu. nếu không thì cắt đến vị trí dấu ;
                        if (sql.Contains("'"))
                        {
                            int lastI = sql.LastIndexOf(';');
                            int i = 0;
                            while (i >= 0 && i < lastI)
                            {
                                int before = sql.IndexOf('\'', i);
                                int after = sql.IndexOf('\'', before + 1);
                                i = sql.IndexOf(';', i);

                                if ((before == -1 || after == -1) && (before > i || after < i))
                                {
                                    break;
                                }

                                //vòng lặp tiếp theo sẽ bắt đầu từ dấu ngoặc cuối để không lấy nhầm bộ ngoặc đơn
                                if (after > 0)
                                {
                                    i = sql.IndexOf('\'', after) + 1;
                                }
                                else
                                    i = sql.IndexOf(';', i);
                            }

                            if (i > 0 && i <= lastI)
                            {
                                result = sql.Substring(0, i);
                            }
                        }
                        else
                        {
                            result = sql.Substring(0, sql.IndexOf(';', 0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //chạy sql 
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (dataObject.Count > 0)
                {
                    return true;
                }

                if (!string.IsNullOrWhiteSpace(querry))
                {
                    List<string> error = new List<string>();
                    listRdo = new Db.AnalysisDb(querry).GetDataSql(ref error);

                    if (error != null && error.Count > 0)
                    {
                        result = false;
                        Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => error), error));
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

        protected override void SetTag(Dictionary<string, object> dicSingleData, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if (dicSingleKey != null && dicSingleKey.Count > 0)
            {
                foreach (var item in dicSingleKey)
                {
                    if (!dicSingleData.ContainsKey(item.Key))
                    {
                        dicSingleData.Add(item.Key, item.Value);
                    }
                    else
                    {
                        dicSingleData[item.Key] = item.Value;
                    }
                }
            }

            this.GetDicMini(ref dicMini);

            List<string> lstSingerObject = new List<string>();
            this.GetSingerObject(ref lstSingerObject);
            if (dataObject.Count > 0)
            {

                if (dataObject[0].Count > 0 && dataObject[0][0] != null && dataObject[0][0].Rows != null && dataObject[0][0].Rows.Count > 0 && dataObject[0][0].Rows.Count > 1048576)
                {
                    var totalRows = dataObject[0][0].Rows.Count;
                    var halfway = totalRows / 2;
                    var firstHalf = dataObject[0][0].AsEnumerable().Take(halfway).CopyToDataTable();
                    var secondHalf = dataObject[0][0].AsEnumerable().Skip(halfway).Take(totalRows - halfway).CopyToDataTable();
                    objectTag.AddObjectData(store, "DataSheet2", firstHalf);
                    dataObject[0][0] = secondHalf;
                }
                else
                {

                    objectTag.AddObjectData(store, "DataSheet2", new DataTable());
                }
                for (int i = 0; i < 15; i++)
                {
                    string Reporti = "Report" + i;
                    if (dicMini != null && dicMini.ContainsKey(Reporti))
                    {
                        var value = dicMini[Reporti];
                        if (value != null)
                        {
                            Reporti = value.ToString();
                        }
                        dicMini.Remove(Reporti);
                    }
                    objectTag.AddObjectData(store, Reporti, dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                    objectTag.AddObjectData(store, "Parent" + i, dataObject[i].Count > 1 ? dataObject[i][1] : new DataTable());
                    objectTag.AddObjectData(store, "GrandParent" + i, dataObject[i].Count > 2 ? dataObject[i][2] : new DataTable());
                    objectTag.AddRelationship(store, "Parent" + i, Reporti, "PARENT_KEY", "PARENT_KEY");
                    objectTag.AddRelationship(store, "GrandParent" + i, "Parent" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");

                    //Dữ liệu Dasboard có các đối tượng đơn. không phải là danh sách
                    //Khai báo tên đối tượng trả về ở dạng đơn trên mẫu.
                    //Khi có khai báo và có 1 dòng dữ liệu
                    //ghi đè đối tượng dạng danh sách thành 1 đối tượng
                    if (lstSingerObject.Contains(Reporti))
                    {
                        Dictionary<string, object> rowData = new Dictionary<string, object>();
                        if (dataObject[i].Count == 1 && dataObject[i][0].Rows.Count == 1)
                        {
                            foreach (DataColumn item in dataObject[i][0].Columns)
                            {
                                rowData[item.ColumnName] = dataObject[i][0].Rows[0][item.ColumnName];
                            }
                        }

                        objectTag.OverWriteObjectData(Reporti, rowData);
                    }

                    DataTable iFather = new DataTable();
                    try
                    {
                        iFather = (dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable()).AsEnumerable()
                                        .GroupBy(d => d.Field<string>("PARENT_KEY"))
                                        .Select(g => g.FirstOrDefault())
                                        .CopyToDataTable() ?? new DataTable();
                    }
                    catch (Exception)
                    {
                        iFather = new DataTable();
                    }
                    objectTag.AddObjectData(store, "Father" + i, iFather);
                    DataTable iGrandFather = new DataTable();
                    try
                    {
                        iGrandFather = iFather.AsEnumerable()
                                        .GroupBy(d => d.Field<string>("GRAND_PARENT_KEY"))
                                        .Select(g => g.FirstOrDefault())
                                        .CopyToDataTable() ?? new DataTable();
                    }
                    catch (Exception)
                    {
                        iGrandFather = new DataTable();
                    }
                    objectTag.AddObjectData(store, "GrandFather" + i, iGrandFather);
                    if (iFather.AsEnumerable().FirstOrDefault() != null)
                    {
                        objectTag.AddRelationship(store, "Father" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
                        if (iGrandFather.AsEnumerable().FirstOrDefault() != null)
                        {
                            objectTag.AddRelationship(store, "GrandFather" + i, "Father" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");
                        }
                    }
                }
            }

            objectTag.AddObjectData(store, "Report", listRdo);
            foreach (var item in dicMini)
            {
                if (item.Value is DataTable)
                {
                    objectTag.AddObjectData(store, item.Key, item.Value as DataTable);
                }
            }
        }

        private void GetSingerObject(ref List<string> lstSingerObject)
        {
            try
            {
                string jsonNewObjectKey = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 2, 2, 1);
                if (!string.IsNullOrWhiteSpace(jsonNewObjectKey))
                {
                    lstSingerObject = jsonNewObjectKey.Trim().Split(';').ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetDicMini(ref Dictionary<string, object> dicMini)
        {
            try
            {
                string jsonNewObjectKey = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 2, 1, 1);
                if (!string.IsNullOrWhiteSpace(jsonNewObjectKey))
                {
                    if (jsonNewObjectKey.StartsWith("{") && jsonNewObjectKey.EndsWith("}") || jsonNewObjectKey.StartsWith("[") && jsonNewObjectKey.EndsWith("]"))
                    {
                        dicMini = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonNewObjectKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
