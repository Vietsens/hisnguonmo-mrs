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
using MRS.Proccessor.Mrs00532;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00532
{
    public class Mrs00532Processor : AbstractProcessor
    {
        private List<Mrs00532RDO> ListRdoDetail = new List<Mrs00532RDO>();
        private List<Mrs00532RDO> ListRdo = new List<Mrs00532RDO>();
        
        Mrs00532Filter filter = null;

        string thisReportTypeCode = "";

        List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatienttypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        public Mrs00532Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00532Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00532Filter)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00532: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter)); 
            
            try
            {
              //Cac ho so dieu tri dang dieu tri
                HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IS_OUT = false;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatment);
                filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IS_OUT = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;// Cac BN ra ngay time_from thi khong tinh nhung ko sao.
                var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatmentOut);
                //Inventec.Common.Logging.LogSystem.Info("listHisTreatment" + listHisTreatment.Count);

                //Cac ho so dieu tri da duyet khoa vien phi
                HisTreatmentView4FilterQuery HisTreatmentFeeLock = new HisTreatmentView4FilterQuery();
                HisTreatmentFeeLock.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                HisTreatmentFeeLock.FEE_LOCK_TIME_TO = filter.TIME_TO;
                HisTreatmentFeeLock.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                var listTreatmentSub = new HisTreatmentManager().GetView4(HisTreatmentFeeLock);
                listHisTreatment.AddRange(listTreatmentSub);
                listHisTreatment = listHisTreatment.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                List<long> treatmentIds = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                //
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
                        sereServFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                        sereServFilter.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                        if (listSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listSereServSub);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listHisSereServ" + listHisSereServ.Count);
                }
				listHisTreatment = listHisTreatment.Where(o=>listHisSereServ.Exists(p=>p.TDL_TREATMENT_ID == o.ID)).ToList();
				treatmentIds = treatmentIds.Where(o=>listHisSereServ.Exists(p=>p.TDL_TREATMENT_ID == o)).ToList();
                //
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
                
                //Cac BN da khoa vien phi
              
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
                
                ListRdoDetail = (from r in listHisTreatment select new Mrs00532RDO(r, listHisSereServ, lastPatienttypeAlter)).ToList();
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
                ListRdoDetail = new List<Mrs00532RDO>();
                result = false;
            }
            return result;
        }

		void ProcessInDate(DateTime IndexTime)
		{
			Mrs00532RDO rdo = new Mrs00532RDO();
            rdo.COUNT_TREATTING = ListRdoDetail.Where(o => o.OUT_TIME==null||this.ToDate(o.OUT_TIME ?? 0) > IndexTime).ToList().Count;

            var ListRdoDetailFeeLock = ListRdoDetail.Where(o => o.IS_ACTIVE == 0 && this.ToDate(o.FEE_LOCK_TIME ?? 0) == IndexTime).ToList();
			
			var listSub = ListRdoDetailFeeLock.Where(o=>o.TREATMENT_TYPE_ID ==1).Select(p=>p.AllTreatmentType).ToList();
			rdo.Exam = GroupSum(listSub);

            listSub = ListRdoDetailFeeLock.Where(o => o.TREATMENT_TYPE_ID == 2).Select(p => p.AllTreatmentType).ToList();
			rdo.TreatOut = GroupSum(listSub);

            listSub = ListRdoDetailFeeLock.Where(o => o.TREATMENT_TYPE_ID == 3).Select(p => p.AllTreatmentType).ToList();
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
					rdo.INFO_AVG = rdo.SUM_TOTAL_PRICE/rdo.COUNT_FEE_LOCK;
                    if (rdo.SUM_TOTAL_PRICE > 0)
                    {
                        rdo.INFO_PERCENT_MEDI = rdo.SUM_MEDI_PRICE / rdo.SUM_TOTAL_PRICE * 100;
                        rdo.INFO_PERCENT_CLS = rdo.SUM_CLS_PRICE / rdo.SUM_TOTAL_PRICE * 100;
                    }
                    
                
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
