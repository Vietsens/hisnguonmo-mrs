using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00086
{
    public class Mrs00086Processor : AbstractProcessor
    {
        Mrs00086Filter castFilter = null; 
        List<Mrs00086RDO> ListRdo = new List<Mrs00086RDO>(); 
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>(); 
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>(); 

        public Mrs00086Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00086Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00086Filter)this.reportFilter); 

                LoadDataToRam(); 
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
                ProcessListTreatment(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListTreatment()
        {
            try
            {
                if (ListTreatment.Count > 0)
                {
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = ListTreatment.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<V_HIS_TREATMENT> hisTreatment = ListTreatment.Skip(start).Take(limit).ToList(); 
                        HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery(); 
                        filter.TREATMENT_IDs = hisTreatment.Select(s => s.ID).ToList(); 
                        List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter); 

                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                        ssFilter.TREATMENT_IDs = hisTreatment.Select(s => s.ID).ToList(); 
                        List<V_HIS_SERE_SERV> hisSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(ssFilter); 
                        HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery(); 
                        reqFilter.TREATMENT_IDs = hisTreatment.Select(s => s.ID).ToList(); 
                        dicServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(param).Get(reqFilter).ToDictionary(o => o.ID); 

                        ProcessDetailListTreatment(hisTreatment, hisPatientTypeAlter, hisSereServ); 
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co Exception xay ra tai DAOGET trong qua trinh tong hop du lieu."); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessDetailListTreatment(List<V_HIS_TREATMENT> treatments, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, List<V_HIS_SERE_SERV> hisSereServ)
        {
            try
            {
                Dictionary<V_HIS_TREATMENT, string> dicTreatment = new Dictionary<V_HIS_TREATMENT, string>(); 
                foreach (var treatment in treatments)
                {
                    var dataPTAs = PatientTypeAlters.Where(o => o.TREATMENT_ID == treatment.ID).ToList(); 
                    if (dataPTAs != null && dataPTAs.Count > 0)
                    {
                        dataPTAs = dataPTAs.OrderByDescending(o => o.LOG_TIME).ToList(); 
                        if (dataPTAs[0].TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)
                        {
                            if (dataPTAs[0].PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                dicTreatment.Add(treatment, "X"); 
                            }
                            else
                            {
                                dicTreatment.Add(treatment, ""); 
                            }
                        }
                    }
                }
                if (dicTreatment.Count > 0)
                {
                    ProcessDicTreatment(dicTreatment, hisSereServ); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ(); 
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0]; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new HIS_SERVICE_REQ(); 
            }
            return result; 
        }

        private void ProcessDicTreatment(Dictionary<V_HIS_TREATMENT, string> dicTreatment, List<V_HIS_SERE_SERV> hisSereServ)
        {
            try
            {
                foreach (var dic in dicTreatment)
                {
                    var listSereServ = hisSereServ.Where(o => o.TDL_TREATMENT_ID == dic.Key.ID).ToList(); 
                    foreach (var sereServ in listSereServ)
                    {
                        Mrs00086RDO rdo = new Mrs00086RDO(); 
                        rdo.TREATMENT_CODE = dic.Key.TREATMENT_CODE; 
                        rdo.PATIENT_CODE = dic.Key.TDL_PATIENT_CODE; 
                        rdo.PATIENT_NAME = dic.Key.TDL_PATIENT_NAME; 
                        rdo.VIR_ADDRESS = dic.Key.TDL_PATIENT_ADDRESS; 
                        rdo.IS_BHYT = dic.Value; 
                        CalcuatorAge(rdo, dic.Key); 
                        rdo.SERVICE_CODE = sereServ.TDL_SERVICE_CODE; 
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                        rdo.AMOUNT = sereServ.AMOUNT; 
                        rdo.EXECUTE_ROOM_NAME = sereServ.EXECUTE_ROOM_NAME; 
                        rdo.EXECUTE_USERNAME = req(sereServ).EXECUTE_USERNAME; 
                        ListRdo.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void CalcuatorAge(Mrs00086RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB); 
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB); 
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4); 
                }
                return null; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return null; 
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery(); 
                filter.CREATE_TIME_FROM = castFilter.TIME_FROM; 
                filter.CREATE_TIME_TO = castFilter.TIME_TO; 
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetView(filter); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListTreatment.Clear(); 
            }
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

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ThenBy(t => t.SERVICE_CODE).ToList(); 
                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
