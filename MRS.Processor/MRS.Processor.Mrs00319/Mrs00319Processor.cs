using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Base; 

namespace MRS.Processor.Mrs00319
{
    class Mrs00319Processor : AbstractProcessor
    {
        Mrs00319Filter mrs00319Filter = new Mrs00319Filter(); 
        List<V_HIS_TREATMENT_2> listTreatment = new List<V_HIS_TREATMENT_2>(); 
        List<Mrs00319RDO> listMrs00319RDO = new List<Mrs00319RDO>(); 

        public Mrs00319Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00319Filter); 
        }

        protected override bool GetData()///
        {
            bool result = true; 
            try
            {
                this.mrs00319Filter = (Mrs00319Filter)this.reportFilter; 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT_2, V_HIS_PATIENT_TYPE_ALTER, HIS_DEPARTMENT, MRS00319: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mrs00319Filter), mrs00319Filter)); 
                CommonParam paramGet = new CommonParam(); 
                HisTreatmentView2FilterQuery treatment2Filter = new HisTreatmentView2FilterQuery(); 
                treatment2Filter.FEE_LOCK_TIME_FROM = this.mrs00319Filter.TIME_FROM; 
                treatment2Filter.FEE_LOCK_TIME_TO = this.mrs00319Filter.TIME_TO; 
                treatment2Filter.IS_PAUSE = true; 
                var rs = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView2(treatment2Filter); 
                rs = rs.Where(p => p.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 
                if (rs != null)
                {
                    foreach (var item in rs)
                    {
                        if (!CheckTreatmentInBranch(item))
                            continue; 

                        decimal totalReceive = ((item.TOTAL_DEPOSIT_AMOUNT ?? 0) + (item.TOTAL_BILL_AMOUNT ?? 0) - (item.TOTAL_BILL_TRANSFER_AMOUNT ?? 0) - (item.TOTAL_BILL_FUND ?? 0) - (item.TOTAL_REPAY_AMOUNT ?? 0)); 

                        decimal totalReceiveMore = (item.TOTAL_PATIENT_PRICE ?? 0) - totalReceive; 
                        // adoPrice.TotalReceiveMorePrice = Inventec.Common.Number.Convert.NumberToStringMoneyAfterRound(totalReceiveMore); 
                        if (totalReceiveMore != 0)
                            continue; 
                        listTreatment.Add(item); 
                    }
                }

                //Diện điều trị là nội trú
                var listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                if (listTreatment != null)
                {
                    var skip = 0;
                    while (listTreatment.Count - skip > 0)
                    {
                        var listIds = listTreatment.Select(o=>o.ID).Distinct().Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                        {
                            TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU,
                            TREATMENT_IDs = listIds
                        };
                        var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).Get(patientTypeAlterFilter);
                        listPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                    }
                }
                List<long> treatemtIdsByTreaIn = new List<long>(); 
                if (listPatientTypeAlter != null && listPatientTypeAlter.Count > 0)
                {
                    treatemtIdsByTreaIn = listPatientTypeAlter.Select(p => p.TREATMENT_ID).ToList(); 
                }
                listTreatment = listTreatment.Where(p => treatemtIdsByTreaIn.Contains(p.ID)).ToList(); 
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
                if (listTreatment != null && listTreatment.Count > 0)
                {
                    foreach (var item in listTreatment)
                    {
                        Mrs00319RDO ado = new Mrs00319RDO(); 
                        ado.TREATMENT_CODE = item.TREATMENT_CODE; 
                        ado.PATIENT_CODE = item.TDL_PATIENT_CODE; 
                        ado.HEIN_CARD_NUMBER = item.TDL_HEIN_CARD_NUMBER; 
                        ado.AGE = Convert.ToInt64(Age.CalculateAge(item.TDL_PATIENT_DOB)); 
                        ado.END_DEPARTMENT_NAME = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.END_DEPARTMENT_ID).DEPARTMENT_NAME; 
                        ado.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.FEE_LOCK_TIME ?? 0); 
                        ado.GENDER_NAME = item.TDL_PATIENT_GENDER_NAME; 
                        ado.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.OUT_TIME ?? 0); 
                        ado.STORE_CODE = item.STORE_CODE; 
                        ado.VIR_ADDRESS = item.TDL_PATIENT_ADDRESS; 
                        ado.VIR_PATIENT_NAME = item.TDL_PATIENT_NAME; 
                        listMrs00319RDO.Add(ado); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private bool CheckTreatmentInBranch(V_HIS_TREATMENT_2 treat)
        {
            try
            {
                if (!this.mrs00319Filter.BRANCH_ID.HasValue)
                {
                    return true; 
                }
                var depart = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treat.END_DEPARTMENT_ID); 
                if (depart != null && depart.BRANCH_ID == this.mrs00319Filter.BRANCH_ID.Value)
                {
                    return true; 
                }
                return false; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return true; 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (mrs00319Filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00319Filter.TIME_FROM)); 
                }
                if (mrs00319Filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00319Filter.TIME_TO)); 
                }
                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", listMrs00319RDO); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
