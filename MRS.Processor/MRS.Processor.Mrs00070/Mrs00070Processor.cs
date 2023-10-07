using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00070
{
    public class Mrs00070Processor : AbstractProcessor
    {
        Mrs00070Filter castFilter = null; 
        List<Mrs00070RDO> ListRdo = new List<Mrs00070RDO>(); 
        string DEPARTMENT_NAME; 
        List<V_HIS_TREATMENT> ListTreatment; 

        public Mrs00070Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00070Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00070Filter)this.reportFilter); 

                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("bat dau lay du lieu V_HIS_BILL. MRS00070 filter:. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTreatmentViewFilterQuery feeFilter = new HisTreatmentViewFilterQuery(); 
                feeFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                feeFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(feeFilter); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("Xay ra exception tai DAOGET trong qua trinh lay du lieu V_HIS_BILL MRS00070. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
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

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ProcessListTreatment(ListTreatment); 
                    result = true; 
                }
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
                    ListTreatment = ListTreatment.Where(o => (o.IS_ACTIVE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) && o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList(); 
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = ListTreatment.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_TREATMENT> hisTreatment = ListTreatment.Skip(start).Take(limit).ToList(); 
                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery(); 
                        feeFilter.IDs = hisTreatment.Select(s => s.ID).ToList();
                        List<V_HIS_TREATMENT_FEE> ListTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(feeFilter); 
                        ProcessListTreatmentFee(paramGet, ListTreatmentFee); 
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00070."); 
                    }
                    ListRdo = ListRdo.GroupBy(g => g.PATIENT_TYPE_ID).Select(s => new Mrs00070RDO { PATIENT_TYPE_ID = s.First().PATIENT_TYPE_ID, PATIENT_TYPE_CODE = s.First().PATIENT_TYPE_CODE, PATIENT_TYPE_NAME = s.First().PATIENT_TYPE_NAME, IN_VIR_TOTAL_HEIN_PRICE = s.Sum(s1 => s1.IN_VIR_TOTAL_HEIN_PRICE), IN_VIR_TOTAL_PATIENT_PRICE = s.Sum(s2 => s2.IN_VIR_TOTAL_PATIENT_PRICE), OUT_VIR_TOTAL_HEIN_PRICE = s.Sum(s3 => s3.OUT_VIR_TOTAL_HEIN_PRICE), OUT_VIR_TOTAL_PATIENT_PRICE = s.Sum(s4 => s4.OUT_VIR_TOTAL_PATIENT_PRICE) }).ToList(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessListTreatmentFee(CommonParam paramGet, List<V_HIS_TREATMENT_FEE> ListTreatmentFee)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatmentFee))
                {
                    foreach (var TreatmentFee in ListTreatmentFee)
                    {
                        Mrs00070RDO rdo = new Mrs00070RDO(); 
                        HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                        appFilter.TREATMENT_ID = TreatmentFee.ID;
                        var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last(); 
                        if (IsNotNull(currentPatientTypeAlter))
                        {
                            rdo.PATIENT_TYPE_ID = currentPatientTypeAlter.PATIENT_TYPE_ID; 
                            rdo.PATIENT_TYPE_CODE = currentPatientTypeAlter.PATIENT_TYPE_CODE; 
                            rdo.PATIENT_TYPE_NAME = currentPatientTypeAlter.PATIENT_TYPE_NAME; 
                            if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.IN_VIR_TOTAL_HEIN_PRICE = TreatmentFee.TOTAL_HEIN_PRICE ?? 0; 
                                rdo.IN_VIR_TOTAL_PATIENT_PRICE = TreatmentFee.TOTAL_PATIENT_PRICE ?? 0; 
                            }
                            else
                            {
                                rdo.OUT_VIR_TOTAL_HEIN_PRICE = TreatmentFee.TOTAL_HEIN_PRICE ?? 0; 
                                rdo.OUT_VIR_TOTAL_PATIENT_PRICE = TreatmentFee.TOTAL_PATIENT_PRICE ?? 0; 
                            }
                            ListRdo.Add(rdo); 
                        }
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
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID); 
                if (IsNotNull(department))
                {
                    DEPARTMENT_NAME = department.DEPARTMENT_NAME; 
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
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                GetDepartment(); 
                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME); 
                ListRdo = ListRdo.OrderBy(o => o.PATIENT_TYPE_ID).ToList(); 

                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
