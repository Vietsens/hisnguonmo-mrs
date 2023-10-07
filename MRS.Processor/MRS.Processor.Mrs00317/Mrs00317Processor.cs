using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
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

namespace MRS.Processor.Mrs00317
{
    class Mrs00317Processor : AbstractProcessor
    {
        Mrs00317Filter castFilter = null; 
        List<Mrs00317RDO> ListRdo = new List<Mrs00317RDO>(); 
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>(); 
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>(); 

        public Mrs00317Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00317Filter); 
        }

        protected override bool GetData()///
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00317Filter)this.reportFilter; 
                // V_HIS_TREATMENT
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.IS_OUT = false;  //?
                var Treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentFilter); 

                // V_HIS_PATIENT_TYPE_ALTER
                var skip = 0; 
                while (Treatments.Count - skip > 0)
                {
                    var listIds = Treatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery
                    {
                        TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU,
                        TREATMENT_IDs = listIds.Select(s => s.ID).ToList()
                    }; 
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter); 
                    listPatientTypeAlters.AddRange(listPatientTypeAlter); 
                }

                // V_HIS_TREATMENT
                if (IsNotNullOrEmpty(listPatientTypeAlters))
                {
                    listTreatments = new List<V_HIS_TREATMENT>(); 
                    var treatmentIds = listPatientTypeAlters.Select(s => s.TREATMENT_ID).Distinct().ToList(); 
                    listTreatments = Treatments.Where(o => treatmentIds.Contains(o.ID)).ToList(); 
                }

                // V_HIS_TREATMENT_FEE
                if (IsNotNullOrEmpty(listTreatments))
                {
                    skip = 0; 
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var treatmentFeeFilter = new HisTreatmentFeeViewFilterQuery
                        {
                            IDs = listIds.Select(s => s.ID).Distinct().ToList()
                        };
                        var listTreatmentFeesOut = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetFeeView(treatmentFeeFilter); 
                        listTreatmentFees.AddRange(listTreatmentFeesOut); 

                        // V_HIS_DEPARTMENT_TRAN
                        HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery(); 
                        departmentTranFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                        listDepartmentTrans.AddRange(new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).GetView(departmentTranFilter)); 
                    }

                    listTreatmentFees = listTreatmentFees.Where(s => !s.OUT_TIME.HasValue).ToList(); 
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
                int number = 0; 
                foreach (var treatment in listTreatments)
                {
                    number++; 
                    decimal? residual = 0; 
                    decimal? owe = 0; 
                    decimal? patient_price = 0; 
                    //var patientTypeAlter = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatment.ID).OrderByDescending(s => s.ID).FirstOrDefault(); 
                    var treatmentFee = listTreatmentFees.OrderByDescending(s => s.ID).FirstOrDefault(s => s.ID == treatment.ID); 
                    var departmentTran = listDepartmentTrans.OrderByDescending(s => s.ID).ThenByDescending(p=>p.DEPARTMENT_IN_TIME).FirstOrDefault(s => s.TREATMENT_ID == treatment.ID); 
                    patient_price = treatmentFee != null ? treatmentFee.TOTAL_PATIENT_PRICE - (treatmentFee.TOTAL_DEPOSIT_AMOUNT + treatmentFee.TOTAL_BILL_AMOUNT - treatmentFee.TOTAL_REPAY_AMOUNT - treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT) : 0; 
                    if (patient_price > 0)
                    {
                        residual = patient_price; //Thiếu
                    }
                    else if (patient_price < 0)
                    {
                        owe = -patient_price; //Thừa
                    }
                    var rdo = new Mrs00317RDO
                    {
                        NUMBER = number,
                        PATIENT_CODE = treatment.TDL_PATIENT_CODE,
                        TREATMENT_CODE = treatment.TREATMENT_CODE,
                        PATIENT_NAME = treatment.TDL_PATIENT_NAME,
                        RESIDUAL = residual,
                        OWE = owe,
                        DEPARTMENT_NAME = departmentTran != null ? departmentTran.DEPARTMENT_NAME : ""
                    }; 
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
            try
            {
                //if (castFilter.TIME_FROM > 0)
                //{
                //    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                //}
                //if (castFilter.TIME_TO > 0)
                //{
                //    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                //}
                //dicSingleTag.Add("DATE", DateTime.Now.ToString()); 
                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PATIENT", ListRdo.OrderBy(s => s.DEPARTMENT_NAME).ToList()); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
