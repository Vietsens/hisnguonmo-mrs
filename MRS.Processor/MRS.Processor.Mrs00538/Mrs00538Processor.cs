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
using MRS.Proccessor.Mrs00538;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00538
{
    public class Mrs00538Processor : AbstractProcessor
    {
        private List<Mrs00538RDO> ListRdoDetail = new List<Mrs00538RDO>();
        private List<Mrs00538RDO> ListRdo = new List<Mrs00538RDO>();

        Mrs00538Filter filter = null;

        string thisReportTypeCode = "";

        List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatienttypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        public Mrs00538Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00538Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00538Filter)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00538: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));

            try
            {
                //Cac ho so chua ra vien, thoi gian vao truoc time to
                HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.IS_OUT = false;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatment);

                //Cac ho so da ra vien, thoi gian vao truoc time to ra sau time from
                filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IS_OUT = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatmentOut);

                //Inventec.Common.Logging.LogSystem.Info("listHisTreatment" + listHisTreatment.Count);

                List<long> treatmentIds = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                //SereServ tuong ung
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.TREATMENT_IDs = limit;
                        sereServFilter.ORDER_FIELD = "ID";
                        sereServFilter.ORDER_DIRECTION = "ASC";
                        sereServFilter.IS_EXPEND = false;
                        if ((this.filter.PATIENT_TYPE_ID_WITH_SERVICE ?? 0) != 0)
                        {
                            sereServFilter.PATIENT_TYPE_ID = this.filter.PATIENT_TYPE_ID_WITH_SERVICE;
                        }
                        sereServFilter.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                        if (listSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listSereServSub);
                    }

                    //Inventec.Common.Logging.LogSystem.Info("listHisSereServ" + listHisSereServ.Count);
                }

                listHisTreatment = listHisTreatment.Where(o => listHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o.ID)).ToList();

                treatmentIds = treatmentIds.Where(o => listHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o)).ToList();

                //Chuyen doi tuong
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery patientTyeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patientTyeAlterFilter.TREATMENT_IDs = limit;
                        patientTyeAlterFilter.ORDER_FIELD = "ID";
                        patientTyeAlterFilter.ORDER_DIRECTION = "ASC";

                        var listPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(patientTyeAlterFilter);
                        if (listPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientTypeAlterSub Get null");
                        else
                            lastPatienttypeAlter.AddRange(listPatientTypeAlterSub);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("lastPatienttypeAlter" + lastPatienttypeAlter.Count);
                }

                lastPatienttypeAlter = lastPatienttypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                if ((this.filter.PATIENT_TYPE_ID_WITH_CARD??0) != 0)
                {
                    listHisTreatment = listHisTreatment.Where(o => lastPatienttypeAlter.Exists(p => p.TREATMENT_ID == o.ID && p.PATIENT_TYPE_ID == this.filter.PATIENT_TYPE_ID_WITH_CARD)).ToList();
                }

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

                ListRdoDetail = (from r in listHisTreatment select new Mrs00538RDO(r, listHisSereServ, lastPatienttypeAlter)).ToList();
                //Inventec.Common.Logging.LogSystem.Info("ListRdoDetail" + ListRdoDetail.Count);

                DateTime StartTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_FROM);
                DateTime FinishTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_TO);
                DateTime IndexTime = StartTime.Date;
                while (IndexTime < FinishTime)
                {
                    ProcessInDate(IndexTime);
                    var daysInMonth = DateTime.DaysInMonth(IndexTime.Year, IndexTime.Month);
                    IndexTime = IndexTime.AddDays(1);
                }


                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00538RDO>();
                result = false;
            }
            return result;
        }

        void ProcessInDate(DateTime IndexTime)
        {
            Mrs00538RDO rdo = new Mrs00538RDO();
            rdo.COUNT_TREATTING = ListRdoDetail.Where(o =>o.TREATMENT_TYPE_ID==3&&o.CLINICAL_IN_TIME!=null&& o.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
            || o.OUT_TIME == null || this.ToDate(o.OUT_TIME ?? 0) > IndexTime).ToList().Count;

            var ListRdoDetailOut = ListRdoDetail.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
            && this.ToDate(o.OUT_TIME ?? 0) == IndexTime).ToList();

            var listSub = ListRdoDetailOut.Where(o => o.TREATMENT_TYPE_ID == 1 || o.TREATMENT_TYPE_ID == 2).Select(p => p.AllTreatmentType).ToList();
            rdo.TreatOut = GroupSum(listSub);

            listSub = ListRdoDetailOut.Where(o => o.TREATMENT_TYPE_ID == 3).Select(p => p.AllTreatmentType).ToList();
            rdo.TreatIn = GroupSum(listSub);
            rdo.DATE = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(IndexTime);
            ListRdo.Add(rdo);
        }

        private DateTime ToDate(long p)
        {
            DateTime date = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(p);
            return date.Date;
        }

        private GROUP_PRICE GroupSum(List<GROUP_PRICE> list)
        {
            string errorField = "";
            GROUP_PRICE rdo = new GROUP_PRICE();
            try
            {
                if (list.Count == 0) return new GROUP_PRICE();
                Decimal sum = 0;
                PropertyInfo[] pi = Properties.Get<GROUP_PRICE>();

                foreach (var field in pi)
                {
                    errorField = field.Name;
                    if (field.Name.Contains("SUM") || field.Name.Contains("COUNT"))
                    {
                        sum = list.Sum(s => (Decimal)field.GetValue(s));

                        field.SetValue(rdo, sum);
                    }

                }
                rdo.INFO_AVG = rdo.SUM_TOTAL_PRICE / rdo.COUNT_OUT;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new GROUP_PRICE();
            }
            return rdo;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }


    }

}
