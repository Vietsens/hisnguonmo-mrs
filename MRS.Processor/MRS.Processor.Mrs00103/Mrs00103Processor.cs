using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisPatientTypeAlter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00103
{
    public class Mrs00103Processor : AbstractProcessor
    {
        Mrs00103Filter castFilter = null; 
        List<Mrs00103RDO> ListRdo = new List<Mrs00103RDO>(); 
        Dictionary<V_HIS_TREATMENT, Mrs00103RDO> dicTreatmentRdo = new Dictionary<V_HIS_TREATMENT, Mrs00103RDO>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        string Department_Name; 
        List<V_HIS_TREATMENT> ListTreatment; 

        public Mrs00103Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00103Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00103Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT, MRS00103 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                treatmentFilter.IS_PAUSE = true; 
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 
                dicPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(ListTreatment.Select(o => o.ID).ToList()).OrderBy(p => p.LOG_TIME).ThenBy(q => q.ID).GroupBy(r => r.TREATMENT_ID).ToDictionary(s => s.Key, s => s.Last()); 
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
            bool result = false; 
            try
            {
                ProcessListTreatment(ListTreatment); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatment = ListTreatment.Where(o => (o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) && o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList(); 
                    }
                    var TreatmentIDs = ListTreatment.Select(o => o.ID).ToList(); 
                    CommonParam paramGet = new CommonParam(); 
                    var skip = 0; 
                    var ListTransaction = new List<V_HIS_TRANSACTION>(); 
                    while (TreatmentIDs.Count - skip > 0)
                    {
                        var listIds = TreatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisTransactionViewFilterQuery tf = new HisTransactionViewFilterQuery()
                        {
                            TREATMENT_IDs = listIds
                        }; 

                        var ListTransactionSub = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(tf); 
                        ListTransaction.AddRange(ListTransactionSub ?? new List<V_HIS_TRANSACTION>()); 
                    }
                    
                    if (castFilter.CASHIER_ROOM_ID != null)
                    {
                        TreatmentIDs = ListTransaction.Where(p => p.CASHIER_ROOM_ID == castFilter.CASHIER_ROOM_ID).Select(o => o.TREATMENT_ID ?? 0).ToList(); 
                        ListTreatment = ListTreatment.Where(o => TreatmentIDs.Contains(o.ID)).ToList(); 
                    }
                    foreach (var treatment in ListTreatment)
                    {
                        Mrs00103RDO rdo = new Mrs00103RDO(); 

                        bool valid = false; 
                        var currentPatientTypeAlter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new V_HIS_PATIENT_TYPE_ALTER(); 

                        if (IsNotNull(currentPatientTypeAlter))
                        {
                            if (castFilter.TREATMENT_TYPE_ID.HasValue)
                            {
                                if (currentPatientTypeAlter.TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)
                                {
                                    valid = true; 
                                }
                            }
                            else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                            {
                                if (castFilter.TREATMENT_TYPE_IDs.Contains(currentPatientTypeAlter.TREATMENT_TYPE_ID))
                                {
                                    valid = true; 
                                }
                            }
                            else
                            {
                                valid = true; 
                            }

                            if (valid)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.IS_BHYT = "X"; 
                                }
                                else
                                {
                                    rdo.IS_NOTBHYT = "X"; 
                                }
                                rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME); 
                                if (treatment.OUT_TIME.HasValue)
                                {
                                    rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME.Value); 
                                }
                                dicTreatmentRdo.Add(treatment, rdo); 
                            }

                        }
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh ProcessListTreatment, MRS00103."); 
                    }

                    ProcessDicTreatmentRDO(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessDicTreatmentRDO()
        {
            try
            {
                if (IsNotNullOrEmpty(dicTreatmentRdo))
                {
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = dicTreatmentRdo.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        Dictionary<V_HIS_TREATMENT, Mrs00103RDO> dicSkip = dicTreatmentRdo.Skip(start).Take(limit).ToDictionary(kvp => kvp.Key, kvp => kvp.Value); 
                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery(); 
                        feeFilter.IDs = dicSkip.Select(s => s.Key.ID).ToList();
                        List<V_HIS_TREATMENT_FEE> ListTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(feeFilter); 
                        ProcessDetailDicTreatmentRDO(dicSkip, ListTreatmentFee); 
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu ProcessDicTreatmentRDO, MRS00103."); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessDetailDicTreatmentRDO(Dictionary<V_HIS_TREATMENT, Mrs00103RDO> dicSkip, List<V_HIS_TREATMENT_FEE> ListTreatmentFee)
        {
            try
            {
                if (IsNotNullOrEmpty(dicSkip) && IsNotNullOrEmpty(ListTreatmentFee))
                {
                    foreach (var dic in dicSkip)
                    {
                        var treatmentFee = ListTreatmentFee.FirstOrDefault(f => f.ID == dic.Key.ID); 
                        if (IsNotNull(treatmentFee))
                        {
                            dic.Value.ProcessHisTreatmentFee(treatmentFee); 
                        }
                        ListRdo.Add(dic.Value); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GetDepartment()
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value); 
                    if (IsNotNull(department))
                    {
                        Department_Name = department.DEPARTMENT_NAME; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("LOG_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("LOG_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                GetDepartment(); 
                dicSingleTag.Add("DEPARTMENT_NAME", Department_Name); 

                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
