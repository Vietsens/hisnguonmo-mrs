using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
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
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatmentLogging;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00587
{
    class Mrs00587Processor : AbstractProcessor
    {
        Mrs00587Filter castFilter = null;
        List<Mrs00587RDO> ListRdo = new List<Mrs00587RDO>();
        List<HIS_TREATMENT_LOGGING> lastTreatmentLoggings = new List<HIS_TREATMENT_LOGGING>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>();
        List<ACS_USER> listAcsUser = new List<ACS_USER>();
        public Mrs00587Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00587Filter);
        }

        protected override bool GetData()///
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00587Filter)this.reportFilter;
                // HIS_TREATMENT
                HisTreatmentFeeViewFilterQuery treatmentFeeFilter = new HisTreatmentFeeViewFilterQuery();
                treatmentFeeFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                treatmentFeeFilter.FEE_LOCK_TIME_FROM = castFilter.FEE_LOCK_TIME_FROM;
                treatmentFeeFilter.FEE_LOCK_TIME_TO = castFilter.FEE_LOCK_TIME_TO;
                listTreatmentFees = new HisTreatmentManager(param).GetFeeView(treatmentFeeFilter);
                var treatmentId = listTreatmentFees.Select(o => o.ID).ToList();

                var skip = 0;

                List<HIS_TREATMENT_LOGGING> listTreatmentLoggings = new List<HIS_TREATMENT_LOGGING>();
                List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
                while (treatmentId.Count - skip > 0)
                {


                    var listIds = treatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    var listPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(patientTypeAlterFilter);
                    listPatientTypeAlters.AddRange(listPatientTypeAlterSub);

                    var TreatmentLoggingFilter = new HisTreatmentLoggingFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        TREATMENT_LOG_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_LOG_TYPE.ID__KVP
                    };
                    var listTreatmentLoggingSub = new HisTreatmentLoggingManager(param).Get(TreatmentLoggingFilter);
                    listTreatmentLoggings.AddRange(listTreatmentLoggingSub);
                }
                lastTreatmentLoggings = listTreatmentLoggings.OrderBy(o => o.CREATE_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                lastPatientTypeAlters = listPatientTypeAlters.OrderBy(o => o.LOG_TIME).ThenBy(r => r.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                var AcsUserFilter = new AcsUserFilterQuery
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listAcsUser = new AcsUserManager(param).Get<List<ACS_USER>>(AcsUserFilter);

                if (castFilter.TREATMENT_TYPE_ID != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => lastPatientTypeAlters.Exists(p => p.TREATMENT_ID == o.ID && p.TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)).ToList();
                }
                if (castFilter.PATIENT_TYPE_ID != null)
                {
                    listTreatmentFees = listTreatmentFees.Where(o => lastPatientTypeAlters.Exists(p => p.TREATMENT_ID == o.ID && p.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID)).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                foreach (var item in listTreatmentFees)
                {
                    var Residual = ((item.TOTAL_DEPOSIT_AMOUNT ?? 0) - (item.TOTAL_REPAY_AMOUNT ?? 0) + ((item.TOTAL_BILL_AMOUNT ?? 0) - (item.TOTAL_BILL_TRANSFER_AMOUNT ?? 0))) - ((item.TOTAL_PATIENT_PRICE ?? 0) - (item.TOTAL_BILL_EXEMPTION ?? 0) - (item.TOTAL_BILL_FUND ?? 0));
                    if (Residual == 0) continue;
                    var treatmentLogging = lastTreatmentLoggings.FirstOrDefault(o => o.TREATMENT_ID == item.ID);
                    Mrs00587RDO rdo = new Mrs00587RDO();
                    rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                    rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                    rdo.DOB = item.TDL_PATIENT_DOB;
                    rdo.GENDER_NAME = item.TDL_PATIENT_GENDER_NAME;
                    rdo.ADDRESS = item.TDL_PATIENT_ADDRESS;
                    rdo.HEIN_CARD_NUMBER = item.TDL_HEIN_CARD_NUMBER;
                    rdo.FEE_LOCK_TIME = item.FEE_LOCK_TIME ?? 0;
                    if (treatmentLogging != null)
                    {
                        rdo.EXECUTE_USERNAME = (listAcsUser.FirstOrDefault(o => o.LOGINNAME == treatmentLogging.LOGINNAME) ?? new ACS_USER()).USERNAME;
                    }
                    rdo.DIFF_TOTAL_PRICE = Residual;
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(s => s.TREATMENT_CODE).ToList());
        }
    }
}
