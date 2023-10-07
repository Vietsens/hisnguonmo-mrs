using Inventec.Common.FlexCellExport;
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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00622
{
    public class Mrs00622Processor : AbstractProcessor
    {
        private Mrs00622Filter filter;
        private CommonParam paramGet = new CommonParam();
        private List<Mrs00622RDO> listRdo = new List<Mrs00622RDO>();
        private List<Mrs00622RDO> listMonthRdo = new List<Mrs00622RDO>();

        private List<HIS_TREATMENT> lisTreatments = new List<HIS_TREATMENT>();
        private List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_ICD> listICD = new List<HIS_ICD>();

        public Mrs00622Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00622Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            filter = (Mrs00622Filter)reportFilter;
            try
            {
                var treatmentFilter = new HisTreatmentFilterQuery();
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    treatmentFilter = new HisTreatmentFilterQuery
                    {
                        IN_TIME_FROM = filter.TIME_FROM,
                        IN_TIME_TO = filter.TIME_TO,

                    };
                }
                else
                {
                    treatmentFilter = new HisTreatmentFilterQuery
                    {
                        IS_PAUSE = true,
                        OUT_TIME_FROM = filter.TIME_FROM,
                        OUT_TIME_TO = filter.TIME_TO,
                        END_DEPARTMENT_IDs =filter.DEPARTMENT_IDs

                    };
                }
                lisTreatments = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                if (filter.DEPARTMENT_ID != null)
                {
                    lisTreatments = lisTreatments.Where(o => o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    lisTreatments = lisTreatments.Where(o => filter.EXAM_ROOM_IDs.Contains(o.END_ROOM_ID ?? 0)).ToList();
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    lisTreatments = lisTreatments.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }

                if (filter.IS_TREAT.HasValue)
                {
                    var treatmentTypes = HisTreatmentTypeCFG.HisTreatmentTypes;
                    if (filter.IS_TREAT.Value && treatmentTypes !=null)// heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                    {
                        var treatmentTypeIds = treatmentTypes.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT).Select(p => p.ID).ToList();
                        lisTreatments = lisTreatments.Where(o => treatmentTypeIds.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }
                    else if (!filter.IS_TREAT.Value && treatmentTypes !=null)// heinAproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                    {
                        var treatmentTypeIds = treatmentTypes.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM).Select(p => p.ID).ToList();
                        lisTreatments = lisTreatments.Where(o => treatmentTypeIds.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    lisTreatments = lisTreatments.Where(o => filter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (filter.ICD_IDs != null)
                {
                    string query = string.Format("select * from his_icd where id in ({0})", string.Join(",", filter.ICD_IDs));
                    listICD = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_ICD>(query);
                    if (listICD != null)
                    {
                        lisTreatments = lisTreatments.Where(o => listICD.Exists(p => p.ICD_CODE == o.ICD_CODE)).ToList();
                    }
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
            var result = true;
            try
            {
                var group = lisTreatments.GroupBy(o => o.ICD_CODE).ToList();

                foreach (var item in group)
                {
                    List<HIS_TREATMENT> listSub = item.ToList<HIS_TREATMENT>();
                    Mrs00622RDO rdo = new Mrs00622RDO();
                    rdo.ICD_NAME = listSub.First().ICD_NAME;
                    rdo.ICD_CODE = listSub.First().ICD_CODE;
                    rdo.COUNT_TREAT = listSub.Count;
                    rdo.COUNT_KHOI = listSub.Count(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI);
                    rdo.COUNT_DO = listSub.Count(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO);
                    rdo.COUNT_KTD = listSub.Count(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD);
                    rdo.COUNT_NANG = listSub.Count(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG);
                    rdo.COUNT_CV = listSub.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                    rdo.COUNT_CHET = listSub.Count(o => o.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET);
                    rdo.DAY = listSub.Sum(o => HIS.Common.Treatment.Calculation.DayOfTreatment(o.CLINICAL_IN_TIME, o.OUT_TIME, o.TREATMENT_END_TYPE_ID, o.TREATMENT_RESULT_ID, o.TDL_HEIN_CARD_NUMBER != null ? HIS.Common.Treatment.PatientTypeEnum.TYPE.BHYT : HIS.Common.Treatment.PatientTypeEnum.TYPE.THU_PHI) ?? 0);
                    rdo.COUNT_BH_NT = listSub.Count(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_BH_NGT = listSub.Count(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_VP_NT = listSub.Count(o => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_VP_NGT = listSub.Count(o => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_CV_NT = listSub.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_CV_NGT = listSub.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    rdo.COUNT_VV = listSub.Count(x => x.IN_TIME >= filter.TIME_FROM && x.IN_TIME <= filter.TIME_TO);
                    rdo.COUNT_NAM = listSub.Count(x => x.TDL_PATIENT_GENDER_NAME == "Nam" && (x.IN_TIME - x.TDL_PATIENT_DOB) > 150000000000);
                    rdo.COUNT_NU = listSub.Count(x => x.TDL_PATIENT_GENDER_NAME == "Nữ" && (x.IN_TIME - x.TDL_PATIENT_DOB) > 150000000000);
                    rdo.COUNT_TE_DUOI15 = listSub.Count(x =>(x.IN_TIME - x.TDL_PATIENT_DOB) < 150000000000);
                    listRdo.Add(rdo);
                }

                // tổng hợp theo tháng
                GroupByMonth();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GroupByMonth()
        {
            var group = lisTreatments.GroupBy(o => string.Format("{0}_{1}", o.ICD_CODE,o.IN_DATE.ToString().Substring(4,2))).ToList();

            foreach (var item in group)
            {
                List<HIS_TREATMENT> listSub = item.ToList<HIS_TREATMENT>();
                Mrs00622RDO rdo = new Mrs00622RDO();
                rdo.IN_MONTH =listSub.First().IN_DATE.ToString().Substring(4, 2);
                rdo.ICD_NAME = listSub.First().ICD_NAME;
                rdo.ICD_CODE = listSub.First().ICD_CODE;
                rdo.COUNT_TREAT = listSub.Count;
               
                listMonthRdo.Add(rdo);
            }
        }

        private bool Death(long TREATMENT_ID)
        {
            var treatment = lisTreatments.FirstOrDefault(o => o.ID == TREATMENT_ID);
            if (treatment != null && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int Age(long INTRUCTION_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(INTRUCTION_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));

            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ReportMonth", listMonthRdo);
        }

    }
}
