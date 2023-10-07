using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00068
{
    public class Mrs00068Processor : AbstractProcessor
    {
        Mrs00068Filter castFilter = null; 
        List<Mrs00068RDO> ListRdo = new List<Mrs00068RDO>(); 
        Dictionary<V_HIS_TREATMENT_FEE, string> dicIsBhyt = new Dictionary<V_HIS_TREATMENT_FEE, string>(); 
        Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran = new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>(); 
        List<V_HIS_TREATMENT_FEE> ListTreatmentFee; 

        public Mrs00068Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00068Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00068Filter)this.reportFilter); 

                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT_FEE. MRS00068. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                //lay du lieu vien phi cua HSDT
                ListTreatmentFee = getTreatmentFee(castFilter.TIME_FROM, castFilter.TIME_TO); 
                var ListTreatmentId = ListTreatmentFee.Select(o => o.ID).ToList(); 
                //lay du lieu chuyen khoa cua hsdt
                dicDepartmentTran = IsNotNullOrEmpty(ListTreatmentId) ? getDepartmentTran(ListTreatmentId) : new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>(); 

                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT_FREE. MRS00068. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
                    throw new DataMisalignedException(); 
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private List<V_HIS_TREATMENT_FEE> getTreatmentFee(long p1, long p2)
        {
            List<V_HIS_TREATMENT_FEE> result = new List<V_HIS_TREATMENT_FEE>(); 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                HisTreatmentFeeViewFilterQuery treatmentFeeFilter = new HisTreatmentFeeViewFilterQuery(); 
                treatmentFeeFilter.CREATE_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFeeFilter.CREATE_TIME_TO = castFilter.TIME_TO;
                result = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(treatmentFeeFilter); 
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new List<V_HIS_TREATMENT_FEE>(); 
            }
            return result; 
        }

        private Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> getDepartmentTran(List<long> listTreatmentId)
        {
            Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> result = new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>(); 
            try
            {
                List<V_HIS_DEPARTMENT_TRAN> listD = new List<V_HIS_DEPARTMENT_TRAN>(); 
                var skip = 0; 
                while (listTreatmentId.Count - skip > 0)
                {
                    var Ids = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery(); 
                    departmentTranFilter.TREATMENT_IDs = Ids; 
                    var listDepartmentTranSub = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).GetView(departmentTranFilter); 
                    listD.AddRange(listDepartmentTranSub); 
                }
                result = listD.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList()); 

            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>(); 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(ListTreatmentFee))
                {
                    ProcessListTreatmentFee(ListTreatmentFee, dicDepartmentTran); 
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

        private void ProcessListTreatmentFee(List<V_HIS_TREATMENT_FEE> ListTreatmentFee, Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                if (IsNotNullOrEmpty(ListTreatmentFee))
                {
                    //Lây tất cả hay còn thiếu hoặc còn thừa
                    if (castFilter.OWED_OR_RESIDUAL.HasValue)
                    {
                        // Hồ sơ điều trị còn thiếu viện phí
                        if (castFilter.OWED_OR_RESIDUAL.Value == 0)
                        {
                            ListTreatmentFee = ListTreatmentFee.Where(o => ((o.TOTAL_PATIENT_PRICE ?? 0) + (o.TOTAL_REPAY_AMOUNT ?? 0)) - ((o.TOTAL_BILL_AMOUNT ?? 0) + (o.TOTAL_DEPOSIT_AMOUNT ?? 0) - (o.TOTAL_BILL_TRANSFER_AMOUNT ?? 0)) > 0).ToList(); 
                        }
                        else if (castFilter.OWED_OR_RESIDUAL == 1)
                        {
                            // Hô sơ điêu trị thừa hoặc đủ viện phí
                            ListTreatmentFee = ListTreatmentFee.Where(o => ((o.TOTAL_PATIENT_PRICE ?? 0) + (o.TOTAL_REPAY_AMOUNT ?? 0)) - ((o.TOTAL_BILL_AMOUNT ?? 0) + (o.TOTAL_DEPOSIT_AMOUNT ?? 0) - (o.TOTAL_BILL_TRANSFER_AMOUNT ?? 0)) <= 0).ToList(); 
                        }
                    }
                    ListTreatmentFee = ListTreatmentFee.Where(o => CheckTreatmentTypeAndPatientType(paramGet, o)).ToList(); 
                }
                ProcessDicTreatmentAndIsBhyt(paramGet, dicDepartmentTran); 
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu"); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessDicTreatmentAndIsBhyt(CommonParam paramGet, Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran)
        {
            try
            {
                if (IsNotNullOrEmpty(dicIsBhyt))
                {
                    ListRdo = (from r in dicIsBhyt select new Mrs00068RDO(r.Key, ref paramGet, r.Value, dicDepartmentTran)).ToList(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        //Kiểm trả treatment có phải là đối tượng bệnh nhân castFilter.PATIENT_TYPE_ID
        private bool CheckTreatmentTypeAndPatientType(CommonParam paramGet, V_HIS_TREATMENT_FEE treatmentFee)
        {
            bool result = false; 
            try
            {
                HisPatientTypeAlterViewFilterQuery hisPTAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                hisPTAlterFilter.TREATMENT_ID = treatmentFee.ID; 
                V_HIS_PATIENT_TYPE_ALTER currentHisPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(hisPTAlterFilter).OrderBy(o=>o.LOG_TIME).ThenBy(p=>p.ID).Last(); 
                if (IsNotNull(currentHisPatientTypeAlter))
                {
                    if (currentHisPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        if (castFilter.PATIENT_TYPE_ID.HasValue)
                        {
                            result = true; 
                            if (currentHisPatientTypeAlter.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID.Value)
                            {
                                if (currentHisPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    dicIsBhyt.Add(treatmentFee, "X"); 
                                }
                                else
                                {
                                    dicIsBhyt.Add(treatmentFee, ""); 
                                }
                            }
                        }
                        else
                        {
                            result = true; 
                            if (currentHisPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                dicIsBhyt.Add(treatmentFee, "X"); 
                            }
                            else
                            {
                                dicIsBhyt.Add(treatmentFee, ""); 
                            }
                        }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
