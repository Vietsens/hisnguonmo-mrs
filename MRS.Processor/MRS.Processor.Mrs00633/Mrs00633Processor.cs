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
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisAccidentHurt;
using MOS.MANAGER.HisAccidentHurtType;
using MOS.MANAGER.HisAccidentResult;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatmentResult;

namespace MRS.Processor.Mrs00633
{
    public class Mrs00633Processor : AbstractProcessor
    {
        Mrs00633Filter filter = null;
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_TREATMENT_RESULT> listHisTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<Mrs00633RDO> listRdo = new List<Mrs00633RDO>();
        List<DepartmentInOut> DepartmentInOuts = new List<DepartmentInOut>();
        List<V_HIS_ACCIDENT_HURT> listAccidentHurts = new List<V_HIS_ACCIDENT_HURT>();
        List<HIS_ACCIDENT_HURT_TYPE> listAccidentHurtTypes = new List<HIS_ACCIDENT_HURT_TYPE>();
        List<HIS_ACCIDENT_RESULT> listAccidentResults = new List<HIS_ACCIDENT_RESULT>();

        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        string thisReportTypeCode = "";

        public Mrs00633Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00633Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00633Filter)this.reportFilter;
            try
            {
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00633"
                };
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
                HisTreatmentEndTypeFilterQuery HisTreatmentEndTypefilter = new HisTreatmentEndTypeFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listHisTreatmentEndType = new HisTreatmentEndTypeManager().Get(HisTreatmentEndTypefilter);
                HisTreatmentResultFilterQuery HisTreatmentResultfilter = new HisTreatmentResultFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listHisTreatmentResult = new HisTreatmentResultManager().Get(HisTreatmentResultfilter);
                HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                HisSereServfilter.TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM;
                HisSereServfilter.TDL_INTRUCTION_TIME_TO = filter.TIME_TO;
                HisSereServfilter.IS_EXPEND = false;
                HisSereServfilter.HAS_EXECUTE = true;
                listHisSereServ = new HisSereServManager(param).Get(HisSereServfilter) ?? new List<HIS_SERE_SERV>();
               
                DepartmentInOuts = new ManagerSql().GetDepartmentTran(filter);
                var treatmentIds = DepartmentInOuts.Select(o => o.TREATMENT_ID).Distinct().ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
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
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);

                        HisAccidentHurtViewFilterQuery accidentHurtFilter = new HisAccidentHurtViewFilterQuery();
                        accidentHurtFilter.TREATMENT_IDs = Ids;
                        accidentHurtFilter.ORDER_DIRECTION = "ID";
                        accidentHurtFilter.ORDER_FIELD = "ASC";
                        var listAccidentHurtSub = new HisAccidentHurtManager(param).GetView(accidentHurtFilter);
                        if (listAccidentHurtSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listAccidentHurtSub Get null");
                        else
                            listAccidentHurts.AddRange(listAccidentHurtSub);
                    }
                }

                listAccidentHurtTypes = new MOS.MANAGER.HisAccidentHurtType.HisAccidentHurtTypeManager(param).Get(new HisAccidentHurtTypeFilterQuery());
                listAccidentResults = new MOS.MANAGER.HisAccidentResult.HisAccidentResultManager(param).Get(new HisAccidentResultFilterQuery());
                listHisService = new HisServiceManager().Get(new HisServiceFilterQuery() { SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT });
                
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
                DateTime Itime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_FROM ?? 0);
                while (Itime < (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.TIME_TO ?? 0))
                {
                    DateTime IDate = new System.DateTime(Itime.Year, Itime.Month, Itime.Day);
                    Mrs00633RDO rdo = new Mrs00633RDO();
                    rdo.DATE = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(IDate) ?? 0;

                    var sereServSub = listHisSereServ.Where(o => o.TDL_INTRUCTION_DATE == rdo.DATE).ToList();

                    rdo.DIC_CATEGORY = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, o.TDL_REQUEST_DEPARTMENT_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_CATEGORY_COUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, o.TDL_REQUEST_DEPARTMENT_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Count());
                    rdo.DIC_MISU_GROUP = sereServSub.Where(r => r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).GroupBy(o => DepartmentPtttGroupCode(o.TDL_REQUEST_DEPARTMENT_ID, o.SERVICE_ID)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_EXP_DIE_BEFORE_IN = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET && o.DEATH_DOCUMENT_DATE < o.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());
                    rdo.DIC_EXP_DIE_AFTER_IN = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET && o.DEATH_DOCUMENT_DATE >= o.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    Inventec.Common.Logging.LogSystem.Info("rdo.DIC_EXP_DIE_BEFORE_IN" + string.Join(",", rdo.DIC_EXP_DIE_BEFORE_IN.Keys));
                    Inventec.Common.Logging.LogSystem.Info("rdo.DIC_EXP_DIE_AFTER_IN" + string.Join(",", rdo.DIC_EXP_DIE_AFTER_IN.Keys));
                    rdo.DIC_EXP = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_EXP_RESULT_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && o.NEXT_ID == null).GroupBy(r => DepartmentTreatmentResultCodeTreatmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_RESULT_ID ?? 0), (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count());
                    rdo.DIC_EXP_CTCV = DepartmentInOuts.Where(o => o.OUT_DATE == rdo.DATE && (o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV||Inventec.Common.DateTime.Calculation.DifferenceTime(o.CLINICAL_IN_TIME??0,o.OUT_TIME??0,Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR)<4) && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_MOVE_TREAT_IN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && listHisPatientTypeAlter.Exists(t => t.DEPARTMENT_TRAN_ID == o.PREVIOUS_ID && t.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_MOVE_EXAM = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && listHisPatientTypeAlter.Exists(t => t.DEPARTMENT_TRAN_ID == o.PREVIOUS_ID && t.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_TRANSFER_IN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && o.PREVIOUS_ID==null&&o.TRANSFER_IN_MEDI_ORG_CODE !=null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_LESS15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) <= 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_IMP_MORE15 = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && Age(o.DEPARTMENT_IN_TIME ?? 0, o.TDL_PATIENT_DOB) > 15).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_CHMS = DepartmentInOuts.Where(o => (long)(o.NEXT_DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)rdo.DATE / 1000000 && o.NEXT_ID != null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_BEGIN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)rdo.DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= rdo.DATE || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_END = DepartmentInOuts.Where(o => o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE + 235959 || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= rdo.DATE + 235959 || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_ACCIDENT_TYPE_RESULT = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)rdo.DATE / 1000000 && listAccidentHurts.Exists(t => t.TREATMENT_ID == o.TREATMENT_ID)).GroupBy(r => DepartmentAccidentTypeResultCode(r.DEPARTMENT_ID,r.TREATMENT_ID, listAccidentHurts)).ToDictionary(p => p.Key, q => q.Count());

                    rdo.DIC_ICD = DepartmentInOuts.Where(o => o.DEPARTMENT_IN_TIME < rdo.DATE + 235959 && (o.NEXT_DEPARTMENT_IN_TIME >= rdo.DATE || (o.NEXT_ID == null && (o.IS_PAUSE == 1 || (o.IS_PAUSE == null && o.OUT_TIME >= rdo.DATE))))).GroupBy(r => IcdCode(r.TREATMENT_ICD_CODE, r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());
                  
                    listRdo.Add(rdo);
                    Itime = Itime.AddDays(1);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string DepartmentTreatmentResultCodeTreatmentEndTypeCode(long DepartmentId, long TreatmentResultId, long TreatmentEndTypeId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == DepartmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + ((listHisTreatmentResult.FirstOrDefault(o => o.ID == TreatmentResultId) ?? new HIS_TREATMENT_RESULT()).TREATMENT_RESULT_CODE ?? "")
                     + "_" + ((listHisTreatmentEndType.FirstOrDefault(o => o.ID == TreatmentEndTypeId) ?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentAccidentTypeResultCode(long departmentId, long treatmentId, List<V_HIS_ACCIDENT_HURT> listAccidentHurts)
        {
            string result = "";
            try
            {
                var accident = listAccidentHurts.FirstOrDefault(o => o.TREATMENT_ID == treatmentId);
                if (accident != null)
                {
                    result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                         + "_" + (accident.ACCIDENT_HURT_TYPE_CODE ?? "") + "_" + (accident.ACCIDENT_RESULT_CODE ?? "");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentPtttGroupCode(long departmentId, long serviceId)
        {
            string result = "";
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId) ?? new HIS_SERVICE();
                if (service != null)
                {
                    result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                         + "_" + ((HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_CODE ?? "");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }



        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        private string TreatmentPatientTypeCode(long TreatmentTypeId,long PatientTypeId)
        {
            string result = "";
            try
            {
                result = ((HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == TreatmentTypeId) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE ?? "") + "_" + ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string PatientTypeCode(long PatientTypeId)
        {
            string result = "";
            try
            {
                result = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == PatientTypeId) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentEndTypeCode(long departmentId,long endTypeId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + ((listHisTreatmentEndType.FirstOrDefault(o=>o.ID==endTypeId)?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string IcdCode(string IcdCode)
        {
            string result = "";
            try
            {
                result = (IcdCode ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string IcdCode(string IcdCode,long departmentId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE??"")
                     + "_" + (IcdCode??"");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
        private string ServiceTypeCode(long serviceTypeId, long departmentId)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId, long departmentId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string CategoryCode(long serviceId,List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return ((listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string ServiceCode(string serviceCode, long departmentId)
        {
            try
            {
                return ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                    + "_" + (serviceCode??"");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.SetUserFunction(store, "SumKeys", new RDOSumKeys());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
