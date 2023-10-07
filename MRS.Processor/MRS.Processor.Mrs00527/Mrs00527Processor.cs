using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00527;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using System.Reflection;
using Inventec.Common.Repository;
using FlexCel.Core;

namespace MRS.Processor.Mrs00527
{
    public class Mrs00527Processor : AbstractProcessor
    {
        private List<Mrs00527RDO> ListRdo = new List<Mrs00527RDO>();
        
        Mrs00527Filter filter = null;

        string thisReportTypeCode = "";
		
		List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
		List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, string> dicBedServiceName = new Dictionary<long, string>();
        public Mrs00527Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00527Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00527Filter)this.reportFilter;
            try
            {
               HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery()
				   {
					   OUT_TIME_FROM = this.filter.TIME_FROM,
                       OUT_TIME_TO = this.filter.TIME_TO
                       
				   };
				listHisTreatment = new HisTreatmentManager(new CommonParam()).Get(HisTreatmentfilter);
                if (this.filter.DEPARTMENT_ID != null)
                    listHisTreatment = listHisTreatment.Where(o => o.END_DEPARTMENT_ID == this.filter.DEPARTMENT_ID).ToList();

				 if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var listHisTreatmentId = listHisTreatment.Select(o => o.ID).Distinct().ToList();
                    var skip = 0;
                    while (listHisTreatmentId.Count - skip > 0)
                    {
                        var limit = listHisTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = limit;
                        HisSereServfilter.ORDER_FIELD="ID";
                        HisSereServfilter.ORDER_DIRECTION="DESC";
                        HisSereServfilter.HAS_EXECUTE = true;
                        HisSereServfilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G;
                        var listHisSereServSub = new HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listHisSereServSub);
                    }
				}
                 dicBedServiceName = listHisSereServ.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, p => p.First().TDL_SERVICE_NAME);

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
            bool result = false;
            try
            {
                ListRdo = (from r in listHisSereServ select new Mrs00527RDO(r, listHisTreatment)).ToList();
                GroupByTreatment();
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00527RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByTreatment()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TREATMENT_CODE }).ToList();
                ListRdo.Clear();
                decimal sum = 0;
                Mrs00527RDO rdo;
                List<Mrs00527RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00527RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00527RDO();
                    listSub = item.ToList<Mrs00527RDO>();
                    rdo = listSub.First();
                    rdo.AMOUNT_STR="";
                    foreach (var column in dicBedServiceName)
                    {
                        if (listSub.Exists(o => o.SERVICE_ID == column.Key))
                        {
                            rdo.AMOUNT_STR += listSub.Where(o => o.SERVICE_ID == column.Key).Sum(o=>o.AMOUNT)+"\t";
                            
                        }
                        else rdo.AMOUNT_STR += "\t";
                    }
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00527RDO IsMeaningful(List<Mrs00527RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00527RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("DATE_STR", string.Join("\t", dicBedServiceName.Values));
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SplitColumn;
            objectTag.AddObjectData(store, "Report", ListRdo);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

        private void SplitColumn(ref Store store, ref System.IO.MemoryStream resultStream)
        {
            try
            {
                //MemoryStream result = new MemoryStream();
                //resultStream.Position = 0;
                //store.flexCel.Run(resultStream, result);
                //if (result != null)
                //{
                //    result.Position = 0;
                //}
                resultStream.Position = 0;
                FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
                xls.Open(resultStream);
                var rountCount = xls.RowCount;
                TFlxFormat fmt = xls.GetCellVisibleFormatDef(7, 9);
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                for (int i = 7; i <= rountCount; i++)
                {
                    xls.PasteFromTextClipboardFormat(i, 9, TFlxInsertMode.NoneRight, (string)xls.GetCellValue(i, 9));
                    for (int j = 0; j < dicBedServiceName.Count; j++)
                    {
                        xls.SetCellFormat(i, j + 9, xls.AddFormat(fmt));
                    }
                }


                xls.Save(resultStream);
                //resultStream = result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
