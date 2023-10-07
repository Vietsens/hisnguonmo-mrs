using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00407
{
    class Mrs00407Processor : AbstractProcessor
    {
        Mrs00407Filter castFilter = null; 

        List<Mrs00407RDO> ListRdo = new List<Mrs00407RDO>(); 

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        public Mrs00407Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00407Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00407Filter)this.reportFilter; 

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                treatmentViewFilter.IS_PAUSE = true; 
                treatmentViewFilter.OUT_TIME_FROM = castFilter.TIME_FROM; 
                treatmentViewFilter.OUT_TIME_TO = castFilter.TIME_TO; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter); 

                listTreatments = listTreatments.Where(s => s.IS_ACTIVE== IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                if (this.castFilter.BRANCH_ID != null)
                {
                    listTreatments = listTreatments.Where(o => o.BRANCH_ID == this.castFilter.BRANCH_ID).ToList();
                }

                var treatmentIds = listTreatments.Select(o => o.ID).Distinct().ToList(); 
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0; 
                    while (treatmentIds.Count - skip > 0)
                    {
                        var IdSubs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var sereservfilter = new HisSereServFilterQuery()
                        {
                            TREATMENT_IDs = IdSubs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false
                        }; 
                        var sereServSub = new HisSereServManager(param).Get(sereservfilter);
                        if (sereServSub != null)
                        {
                            listSereServ.AddRange(sereServSub);
                        }
                    }
                }
                //listTreatments = listTreatments.Where(o => dicSereServ.ContainsKey(o.ID)).ToList(); 
                if (IsNotNullOrEmpty(listTreatments))
                {
                    var skip = 0; 
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                        listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter)); 
                    }
                    GetPatientTypeAlter(); 

                  
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
                
                foreach (var treatment in listTreatments)
                {
                    List<HIS_SERE_SERV> sereServSub = listSereServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList(); 
                    if (this.castFilter.HAS_ZERO_TOTAL_PRICE != true)
                    {
                        if (sereServSub.Sum(s => s.VIR_TOTAL_PRICE) == 0)
                        {
                            continue;
                        }
                    }
                    if (!dicCurrentPatyAlter.ContainsKey(treatment.ID))
                    {
                        continue;
                    }
                    if (IsNotNullOrEmpty(this.castFilter.TREATMENT_TYPE_IDs))
                    {
                        if (!this.castFilter.TREATMENT_TYPE_IDs.Contains(dicCurrentPatyAlter[treatment.ID].TREATMENT_TYPE_ID))
                        {
                            continue;
                        }
                    }
                    if (IsNotNullOrEmpty(this.castFilter.PATIENT_TYPE_ID_WITH_CARDs))
                    {
                        if (!this.castFilter.PATIENT_TYPE_ID_WITH_CARDs.Contains(dicCurrentPatyAlter[treatment.ID].PATIENT_TYPE_ID))
                        {
                            continue;
                        }
                    }
                    if (IsNotNullOrEmpty(this.castFilter.PATIENT_TYPE_ID_WITH_SERVICEs))
                    {
                        if (!sereServSub.Exists(o => this.castFilter.PATIENT_TYPE_ID_WITH_SERVICEs.Contains(o.PATIENT_TYPE_ID)))
                        {
                            continue;
                        }
                    }
                    var rdo = new Mrs00407RDO(); 
                    rdo.TREATMENT_CODE = treatment.TREATMENT_CODE; 
                    rdo.TREATMENT_ID = treatment.ID; 
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                    rdo.DOB = treatment.TDL_PATIENT_DOB; 
                    rdo.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME; 
                    rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS; 
                    var patientTypeAlter = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatment.ID).OrderByDescending(o => o.LOG_TIME).ThenByDescending(p=>p.ID).ToList(); 
                    rdo.HEIN_CARD_NUMBER = patientTypeAlter.Count > 0 ? patientTypeAlter.First().HEIN_CARD_NUMBER : ""; 
                        
                    rdo.IN_TIME = treatment.IN_TIME; 
                    rdo.OUT_TIME = treatment.OUT_TIME; 
                    if (dicCurrentPatyAlter[treatment.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        rdo.EXECUTE_ROOM_NAME = treatment.END_ROOM_NAME; 
                    else rdo.EXECUTE_ROOM_NAME = treatment.END_DEPARTMENT_NAME; 

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
        private void GetPatientTypeAlter()
        {
            try
            {
                if (IsNotNullOrEmpty(this.listPatientTypeAlters))
                {
                    var Groups = listPatientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToList(); 
                    foreach (var group in Groups)
                    {
                        var listGroup = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().OrderBy(o => o.LOG_TIME).ThenBy(p=>p.ID).ToList(); 
                        if (!dicCurrentPatyAlter.ContainsKey(listGroup.First().TREATMENT_ID)) dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = new V_HIS_PATIENT_TYPE_ALTER(); 
                        dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = listGroup.Last(); 
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.PATIENT_NAME).ToList());
                objectTag.AddObjectData(store, "RdoNoServiceReq", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => !listSereServ.Exists(p => p.SERVICE_REQ_ID.HasValue && p.TDL_TREATMENT_ID == o.TREATMENT_ID && p.VIR_TOTAL_PRICE > 0)).ToList());
                objectTag.AddObjectData(store, "RdoServiceReq", ListRdo.OrderBy(s => s.PATIENT_NAME).Where(o => listSereServ.Exists(p => p.SERVICE_REQ_ID.HasValue && p.TDL_TREATMENT_ID == o.TREATMENT_ID && p.VIR_TOTAL_PRICE > 0)).ToList()); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
