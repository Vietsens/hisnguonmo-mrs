using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00420
{
    class Mrs00420Processor : AbstractProcessor
    {
        Mrs00420Filter castFilter = null; 
        List<Mrs00420RDO> listRdo = new List<Mrs00420RDO>(); 
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        public Mrs00420Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00420Filter); 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "Report", listRdo); 

                string TREATMENT_TYPE_NAME = ""; 
                var listPatientTypeAlter = listPatientTypeAlters.GroupBy(s=>s.TREATMENT_TYPE_ID).ToList(); 
                foreach (var PatientTypeAlter in listPatientTypeAlter)
                {
                    TREATMENT_TYPE_NAME = TREATMENT_TYPE_NAME + " - " + PatientTypeAlter.First().TREATMENT_TYPE_NAME; 
                }
                dicSingleTag.Add("TREATMENT_TYPE_NAME", TREATMENT_TYPE_NAME); 

                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00420Filter)this.reportFilter; 
                //nhom loai bao cao
                HisServiceRetyCatViewFilterQuery serviceRetyCatFillter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyCatFillter.REPORT_TYPE_CODE__EXACT = "MRS00420"; 
                listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFillter); 
                //yeu cau
                HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery(); 
                reqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                reqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                var serviceReqs = new HisServiceReqManager().GetView(reqFilter); 
                var treatmentIds = serviceReqs.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                dicServiceReq = serviceReqs.ToDictionary(o => o.ID); 
                //YC-DV
                var skip = 0; 
                while (listServiceRetyCats.Count - skip > 0)
                {
                    var listIds = listServiceRetyCats.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    
                    int sk = 0; 
                    while (treatmentIds.Count - sk > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        sk = sk + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery sereServFillter = new HisSereServViewFilterQuery(); 
                        sereServFillter.TREATMENT_IDs = limit; 
                        sereServFillter.SERVICE_IDs = listIds.Select(s => s.SERVICE_ID).ToList(); 
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServFillter); 
                        listSereServs.AddRange(listSereServ); 
                    }
                }
                listSereServs=listSereServs.GroupBy(p=>p.ID).Select(o=>o.First()).ToList(); 

                skip = 0; 
                while (listSereServs.Count - skip > 0)
                {
                    var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFillter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterFillter.TREATMENT_TYPE_IDs = castFilter.TREATMENT_TYPE_IDs; 
                    patientTypeAlterFillter.TREATMENT_IDs = listIds.Select(s => s.TDL_TREATMENT_ID??0).ToList(); 
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFillter); 
                    listPatientTypeAlters.AddRange(listPatientTypeAlter); 
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
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 

                var listTreatmentIdinPatientTypeAlter = listPatientTypeAlters.Select(s => s.TREATMENT_ID).Distinct().ToList(); 
                var listSereServ = listSereServs.Where(s => listTreatmentIdinPatientTypeAlter.Contains(s.TDL_TREATMENT_ID??0)).ToList(); 


                foreach (var serviceRetyCats in listServiceRetyCats)
                {
                    var listSereServss = listSereServ.Where(s => s.SERVICE_ID == serviceRetyCats.SERVICE_ID).ToList(); 
                    foreach (var sereServ in listSereServss)
                    {
                        Mrs00420RDO rdo = new Mrs00420RDO(); 

                        var patientTypeAlter = listPatientTypeAlters.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).Distinct().OrderByDescending(o => o.LOG_TIME).ToList(); 

                        List<MY_PATIENT_TYPE_ALTER> mptas = new List<MY_PATIENT_TYPE_ALTER>(); 
                        var outtime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now); 
                        foreach (var pta in patientTypeAlter)
                        {
                            MY_PATIENT_TYPE_ALTER mpta = new MY_PATIENT_TYPE_ALTER(); 
                            mpta.ID = pta.ID; 
                            mpta.TREATMENT_TYPE_ID = pta.TREATMENT_TYPE_ID; 
                            mpta.LOG_IN_TIME = pta.LOG_TIME; 
                            mpta.LOG_OUT_TIME = outtime.Value; 
                            mptas.Add(mpta); 

                            //Gán thời gian bắt đầu của dtg điều trị sau bằng thời gian kết thúc của đtg điều trị trước. (sắp xếp thời gian giản dần theo LOG_TIME)
                            outtime = pta.LOG_TIME; 
                        }

                        mptas = mptas.Where(w => castFilter.TREATMENT_TYPE_IDs.Contains(w.TREATMENT_TYPE_ID)).ToList(); 
                        bool X = false; 
                        foreach (var i in mptas)
                        {
                            if (req(sereServ).INTRUCTION_TIME <= i.LOG_OUT_TIME && req(sereServ).INTRUCTION_TIME >= i.LOG_IN_TIME)
                            {
                                X = true; 
                                break; 
                            }
                        }

                        if (X == true)
                        {
                            rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME; 
                            if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420ECG")
                            {
                                rdo.TOTAL_ECG = sereServ.AMOUNT; 
                            }

                            if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420SUIM")
                            {
                                rdo.TOTAL_SUIM = sereServ.AMOUNT; 
                            }

                            if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420CST")
                            {
                                rdo.TOTAL_CST = sereServ.AMOUNT; 
                            }
                            listRdo.Add(rdo); 
                        }
                    }
                }

                listRdo = listRdo.GroupBy(g => g.REQUEST_DEPARTMENT_NAME).Select(ss => new Mrs00420RDO
                {
                    REQUEST_DEPARTMENT_NAME = ss.First().REQUEST_DEPARTMENT_NAME,
                    TOTAL_ECG = ss.Sum(s => s.TOTAL_ECG),
                    TOTAL_SUIM = ss.Sum(s => s.TOTAL_SUIM),
                    TOTAL_CST = ss.Sum(s => s.TOTAL_CST)
                }).ToList(); 

                #region
                //foreach (var serviceRetyCats in listServiceRetyCats)
                //{
                //    var listSereServ = listSereServs.Where(s => s.SERVICE_ID == serviceRetyCats.SERVICE_ID).ToList(); 
                //    foreach (var sereServ in listSereServ)
                //    {
                //        Mrs00420RDO rdo = new Mrs00420RDO(); 
                //        rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME; 
                //        if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420ECG")
                //        {
                //            rdo.TOTAL_ECG = sereServ.AMOUNT; 
                //        }

                //        if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420SUIM")
                //        {
                //            rdo.TOTAL_SUIM = sereServ.AMOUNT; 
                //        }

                //        if (sereServ.SERVICE_ID == serviceRetyCats.SERVICE_ID && serviceRetyCats.CATEGORY_CODE == "420CST")
                //        {
                //            rdo.TOTAL_CST = sereServ.AMOUNT; 
                //        }
                //        listRdo.Add(rdo); 
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private V_HIS_SERVICE_REQ req(V_HIS_SERE_SERV sereServ)
        {
            return dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0) ? dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0] : new V_HIS_SERVICE_REQ(); 
        }


    }
}
