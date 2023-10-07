using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;

namespace MRS.Processor.Mrs00586
{
    public class Mrs00586Processor : AbstractProcessor
    {
        Mrs00586Filter filter = null;
        List<HIS_TREATMENT> lastHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    List<HIS_DEPARTMENT_TRAN> lastHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        string thisReportTypeCode = "";

        public Mrs00586Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00586Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00586Filter)this.reportFilter;
            try
            {
                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter = this.MapFilter<Mrs00586Filter, HisTreatmentFilterQuery>(filter, listHisTreatmentfilter);
                lastHisTreatment = new HisTreatmentManager(new CommonParam()).Get(listHisTreatmentfilter);

                var treatmentIds = lastHisTreatment.Select(o => o.ID).ToList();
                // dien dieu tri cuoi cung
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    
                    List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);
                    }
                    lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                    lastHisDepartmentTran = listHisDepartmentTran.Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME.Value).ThenBy(q=>q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }
                
                //Lay danh sach BN theo doi tuong bn
                if (filter.PATIENT_TYPE_ID != null)
                    lastHisTreatment = lastHisTreatment.Where(o => lastHisPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.ID && p.PATIENT_TYPE_ID == filter.PATIENT_TYPE_ID)).ToList();
                //Lay danh sach BN theo khoa
                if (filter.DEPARTMENT_ID != null)
                    lastHisTreatment = lastHisTreatment.Where(o => lastHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == filter.DEPARTMENT_ID)).ToList();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

       
    
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM??0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == filter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            dicSingleTag.Add("COUNT_EX_LESS15", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));
            dicSingleTag.Add("COUNT_IN_LESS15", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));
            dicSingleTag.Add("COUNT_OUT_LESS15", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15));
            dicSingleTag.Add("COUNT_EX_OVER60", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60));
            dicSingleTag.Add("COUNT_IN_OVER60", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60));
            dicSingleTag.Add("COUNT_OUT_OVER60", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60));
            dicSingleTag.Add("COUNT_EX_OVER80", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 80));
            dicSingleTag.Add("COUNT_IN_OVER80", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).Count(o =>Age(o.IN_TIME,o.TDL_PATIENT_DOB) >= 80));
            dicSingleTag.Add("COUNT_OUT_OVER80", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).Count(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 80));
            dicSingleTag.Add("COUNT_EX", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).Count());
            dicSingleTag.Add("COUNT_IN", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).Count());
            dicSingleTag.Add("COUNT_OUT", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).Count());
            dicSingleTag.Add("COUNT_EX_FEMALE", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).Count(o=>o.TDL_PATIENT_GENDER_ID==IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("COUNT_IN_FEMALE", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            dicSingleTag.Add("COUNT_OUT_FEMALE", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE));
            objectTag.AddObjectData(store, "ReportIN",lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 3)).ToList());
            objectTag.AddObjectData(store, "ReportOUT", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 2)).ToList());
            objectTag.AddObjectData(store, "ReportEX", lastHisTreatment.Where(p => lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == p.ID && q.TREATMENT_TYPE_ID == 1)).ToList());
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

    }

}
