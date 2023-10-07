using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00431
{
    public class Mrs00431Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 

        private List<Mrs00431RDO> ListRdo = new List<Mrs00431RDO>(); 
        private List<Mrs00431RDO> ListOutTime = new List<Mrs00431RDO>(); 
        private List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>(); 
        private List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>(); 
        List<MOS.EFMODEL.DataModels.HIS_ICD> icds = new List<HIS_ICD>(); 
        private int CountTreatment = 0; 

        public Mrs00431Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00431Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00431Filter)reportFilter); 
            var result = true; 
            try
            {
                HisTreatmentViewFilterQuery filtermain = new HisTreatmentViewFilterQuery(); 
                filtermain.OUT_TIME_FROM = filter.TIME_FROM; 
                filtermain.OUT_TIME_TO = filter.TIME_TO; 
                ListTreatment = new HisTreatmentManager(paramGet).GetView(filtermain); 
                //var ListTreatmentId = ListTreatment.Select(o => o.ID).ToList(); 
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    int start = 0; 
                    int count = ListTreatment.Count; 
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => count), count)); 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = ListTreatment.Skip(start).Take(limit).ToList(); 


                        HisIcdManager hisIcdGet = new HisIcdManager(); 
                        HisIcdFilterQuery icdFilter = new HisIcdFilterQuery();
                        icdFilter.ICD_CODEs = listSub.Where(p=>!String.IsNullOrEmpty(p.ICD_CODE)).Select(o => o.ICD_CODE).Distinct().ToList(); 
                        var rsIcd = hisIcdGet.Get(icdFilter); 
                        icds.AddRange(rsIcd); 


                        ListPatientTypeAlter.AddRange(new HisPatientTypeAlterManager().GetViewByTreatmentIds(ListTreatment.Select(o => o.ID).ToList())); 

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
                dicPatientTypeAlter = ListPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList()); 
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
                ListRdo.Clear(); 
                ListOutTime.Clear(); 
                foreach (var treatment in ListTreatment)
                {
                    Mrs00431RDO rdo = new Mrs00431RDO(); 
                    rdo.TREATMENT_CODE = treatment.TREATMENT_CODE; 

                    if (dicPatientTypeAlter.ContainsKey(treatment.ID))// && ListPatientTypeAlterCurrent.Count() > 0)
                    {
                        if (dicPatientTypeAlter[treatment.ID].Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList().Count > 0)
                        {
                            rdo.TREATMENT_TYPE_NOI_TRU = "X"; 
                        }
                        else
                            rdo.TREATMENT_TYPE_NGOAI_TRU = "X"; 

                        if (dicPatientTypeAlter[treatment.ID].Where(o => IsNotNullOrEmpty(o.HEIN_CARD_NUMBER)).ToList().Count > 0)
                        {
                            rdo.HEIN_CARD_NUMBER = dicPatientTypeAlter[treatment.ID].First().HEIN_CARD_NUMBER; 
                        }
                    }
                    rdo.MEDI_ORG_NAME = treatment.MEDI_ORG_NAME; 
                    rdo.MEDI_ORG_CODE = treatment.MEDI_ORG_CODE; 
                    CalcuatorAge(rdo, treatment); 
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                    rdo.OUT_TIME = treatment.OUT_TIME; 
                    rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS; 
                    rdo.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME; 
                    rdo.END_USERNAME = treatment.END_USERNAME; 
                    rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME; 
                    rdo.OUT_DATE = Inventec.Common.TypeConvert.Parse.ToInt64(((rdo.OUT_TIME ?? 0).ToString().Substring(0, 8))); 
                    rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.OUT_TIME ?? 0);
                    if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                    {
                        var checkIcd = icds.FirstOrDefault(o => o.ICD_CODE == treatment.ICD_CODE); 
                        if (checkIcd != null)
                        {

                            if (!string.IsNullOrEmpty(treatment.ICD_NAME))
                            {
                                rdo.ICD_NAME = treatment.ICD_NAME; 
                            }
                            else
                            {
                                rdo.ICD_NAME = checkIcd.ICD_NAME; 
                            }
                        }

                        rdo.ICD_CODE = checkIcd.ICD_CODE; 
                    }

                    CountTreatment++; 
                    rdo.NUM_ORDER = CountTreatment; 
                    ListRdo.Add(rdo); 
                    ListOutTime.Add(rdo); 
                }
                ListOutTime = ListOutTime.GroupBy(p => new { p.OUT_DATE }).Select(g => g.First()).OrderBy(o => o.OUT_DATE).ToList(); 
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private void CalcuatorAge(Mrs00431RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = Inventec.Common.DateTime.Calculation.Age(treatment.TDL_PATIENT_DOB); 
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.AGE = (tuoi >= 1) ? tuoi : 1; 
                    }
                    else
                    {
                        rdo.AGE = (tuoi >= 1) ? tuoi : 1; 
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00431Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00431Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00431Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00431Filter)reportFilter).TIME_TO)); 
            }
            bool exportSuccess = true; 
            dicSingleTag.Add("SUM_TREATMENT", CountTreatment); 
            objectTag.AddObjectData(store, "ListOutTime", ListOutTime); 
            objectTag.AddObjectData(store, "Report", ListRdo); 
            objectTag.AddRelationship(store, "ListOutTime", "Report", "OUT_DATE", "OUT_DATE"); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
