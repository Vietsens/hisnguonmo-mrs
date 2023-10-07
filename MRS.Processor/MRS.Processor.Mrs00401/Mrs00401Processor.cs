using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatientTypeAlter; 
using AutoMapper; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisServiceReq; 
using MRS.MANAGER.Core.MrsReport.RDO; 

namespace MRS.Processor.Mrs00401
{
    public class Mrs00401Processor : AbstractProcessor
    {
        Mrs00401Filter filter = null; 
        private List<Mrs00401RDO> ListRdo = new List<Mrs00401RDO>();
        private List<V_HIS_TREATMENT_FEE> Treatments = new List<V_HIS_TREATMENT_FEE>(); 
        private List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        private List<V_HIS_SERVICE_REQ> examServiceReqs = new List<V_HIS_SERVICE_REQ>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 

        public Mrs00401Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00401Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 

                this.filter = ((Mrs00401Filter)reportFilter); 
                // get treatments
                HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                treatmentFeeViewFilter.FEE_LOCK_TIME_FROM = this.filter.TIME_FROM;
                treatmentFeeViewFilter.FEE_LOCK_TIME_TO = this.filter.TIME_TO;
                this.Treatments = new HisTreatmentManager(paramGet).GetFeeView(treatmentFeeViewFilter); 

                // danh sach treatment chua khoa Bhyt
                this.Treatments = this.Treatments.Where(o => o.IS_LOCK_HEIN != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 

                // get patientTypeAlter by treatment
                int start = 0; 
                int count = this.Treatments.Count; 
                while (count > 0)
                {
                    if (this.Treatments == null || this.Treatments.Count == 0)
                        break; 
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    var listTreatmentLimit = this.Treatments.Skip(start).Take(limit).ToList(); 
                    HisPatientTypeAlterViewFilterQuery hisPatientTypeAlter = new HisPatientTypeAlterViewFilterQuery(); 
                    hisPatientTypeAlter.TREATMENT_IDs = Treatments.Select(o => o.ID).ToList(); 
                    var patientTypeALterLimits = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(hisPatientTypeAlter); 
                    this.patientTypeAlters.AddRange(patientTypeALterLimits); 
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }
              
                // get examServiceReq
                int startExamServiceReq = 0; 
                int countExamServiceReq = this.Treatments.Count; 

                while (countExamServiceReq > 0)
                {
                    int limit = countExamServiceReq <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? countExamServiceReq : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var listTreatmentLimit = this.Treatments.Skip(startExamServiceReq).Take(limit).ToList(); 
                    HisServiceReqViewFilterQuery exampServiceReqFilter = new HisServiceReqViewFilterQuery(); 
                    exampServiceReqFilter.TREATMENT_IDs = listTreatmentLimit.Select(o => o.ID).ToList(); 
                    exampServiceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH; 
                    var examServiceReqLimits = new HisServiceReqManager(paramGet).GetView(exampServiceReqFilter); 
                    examServiceReqs.AddRange(examServiceReqLimits); 
                    startExamServiceReq += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    countExamServiceReq -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }

                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao Mrs00401: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.filter), this.filter)); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00401"); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessRdos(List<V_HIS_TREATMENT_FEE> treatments)
        {
            try
            {
                GetPatyAlterBhyt(); 

                foreach (var item in treatments)
                {
                    if (!this.filter.PATIENT_TYPE_IDs.Contains(dicCurrentPatyAlter[item.ID].PATIENT_TYPE_ID)) continue; 
                    if (!this.filter.TREATMENT_TYPE_IDs.Contains(dicCurrentPatyAlter[item.ID].TREATMENT_TYPE_ID)) continue; 

                    Mrs00401RDO rdo = new Mrs00401RDO(); 
                    rdo.TREATMENT_CODE = item.TREATMENT_CODE; 
                    rdo.VIR_PATIENT_NAME = item.TDL_PATIENT_NAME; 
                    rdo.VIR_ADDRESS = item.TDL_PATIENT_ADDRESS; 
                    rdo.GENDER_NAME = item.TDL_PATIENT_GENDER_NAME; 
                    if (dicCurrentPatyAlter.ContainsKey(item.ID))
                    {
                        if (dicCurrentPatyAlter[item.ID].HEIN_CARD_NUMBER != null)
                        {
                            rdo.HEIN_CARD_NUMBER = RDOCommon.GenerateHeinCardSeparate(dicCurrentPatyAlter[item.ID].HEIN_CARD_NUMBER); 
                            rdo.BILL_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.FEE_LOCK_TIME ?? 0); 
                        }
                        else
                        {
                            rdo.BILL_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.OUT_TIME ?? 0); 
                        }
                    }
                    rdo.CREATE_TIME_TREATMENT = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(item.CREATE_TIME ?? 0); 
                    if (examServiceReqs != null && examServiceReqs.Count > 0)
                    {
                        var checkExamServiceReq = examServiceReqs.FirstOrDefault(o => o.TREATMENT_ID == item.ID); 
                        if (checkExamServiceReq != null)
                        {
                            rdo.EXECUTE_ROOM_NAME = checkExamServiceReq.EXECUTE_ROOM_NAME; 
                        }
                    }

                    CalculatorAge(rdo, item);
                    rdo.IN_TIME = item.IN_TIME;
                    rdo.OUT_TIME = item.OUT_TIME;
                    rdo.TOTAL_HEIN_PRICE = item.TOTAL_HEIN_PRICE??0;
                    rdo.TOTAL_PATIENT_PRICE = item.TOTAL_PATIENT_PRICE ?? 0;
                    rdo.TOTAL_PRICE = item.TOTAL_PRICE ?? 0;
                    this.ListRdo.Add(rdo); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void CalculatorAge(Mrs00401RDO rdo, V_HIS_TREATMENT_FEE treatment)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB); 
                if (tuoi >= 0)
                {
                    rdo.AGE_STR = (tuoi >= 1) ? tuoi : 1; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


        private void GetPatyAlterBhyt()
        {
            try
            {
                if (IsNotNullOrEmpty(this.patientTypeAlters))
                {
                    var Groups = patientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToList(); 
                    foreach (var group in Groups)
                    {
                        var listGroup = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().OrderBy(o => o.LOG_TIME).ToList(); 
                        dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = listGroup.Last(); 
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                ProcessRdos(this.Treatments); 
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00401Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToMonthString(((Mrs00401Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00401Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToMonthString(((Mrs00401Filter)reportFilter).TIME_TO)); 
            }
            bool exportSuccess = true; 
            objectTag.AddObjectData(store, "Report", ListRdo); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
