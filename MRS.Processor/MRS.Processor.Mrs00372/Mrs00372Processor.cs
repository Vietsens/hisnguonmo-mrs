using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00372
{
    class Mrs00372Processor : AbstractProcessor
    {
        Mrs00372Filter castFilter = null; 

        List<Mrs00372RDO> ListRdo = new List<Mrs00372RDO>(); 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        List<PATIENT_TYPE_ALTER> listPTAs = new List<PATIENT_TYPE_ALTER>(); 

        public Mrs00372Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00372Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00372Filter)this.reportFilter; 

                //chi nhanh
                HisBranchFilterQuery branchName = new HisBranchFilterQuery(); 
                branchName.ID = castFilter.BRANCH_ID; 
                listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchName); 

                //khoa
                var departmentFilter = new HisDepartmentFilterQuery()
                {
                    BRANCH_ID = castFilter.BRANCH_ID,
                }; 
                listDepartments = new HisDepartmentManager(param).Get(departmentFilter); 

                //Dich vu
                HisServiceViewFilterQuery serviceViewFilter = new HisServiceViewFilterQuery(); 
                serviceViewFilter.ID = castFilter.SERVICE_ID; 
                listServices = new MOS.MANAGER.HisService.HisServiceManager(param).GetView(serviceViewFilter); 

                List<long> listTreatmentIds = new List<long>(); 
                //TUDV - HUDV
                HisSereServDepositFilterQuery depositFilter = new HisSereServDepositFilterQuery(); 
                depositFilter.CREATE_TIME_FROM = castFilter.TIME_FROM; 
                depositFilter.CREATE_TIME_TO = castFilter.TIME_TO; 
                var lstDeposit = new MOS.MANAGER.HisSereServDeposit.HisSereServDepositManager(param).Get(depositFilter); 

                if (IsNotNullOrEmpty(lstDeposit))
                {
                    var listDepositId = lstDeposit.Select(o => o.ID).ToList(); 
                    var skip = 0; 
                    var lstRepay = new List<HIS_SESE_DEPO_REPAY>(); 
                    while (lstDeposit.Count() - skip > 0)
                    {
                        var limit = lstDeposit.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisSeseDepoRepayFilterQuery reqFilter = new HisSeseDepoRepayFilterQuery(); 
                        reqFilter.SERE_SERV_DEPOSIT_IDs = limit.Select(o => o.ID).ToList(); 
                        lstRepay.AddRange(new MOS.MANAGER.HisSeseDepoRepay.HisSeseDepoRepayManager(param).Get(reqFilter));  
                    }
                    if (lstRepay != null && lstRepay.Count > 0)
                    {
                        lstDeposit = lstDeposit.Where(o => !(lstRepay.Select(s => s.SERE_SERV_DEPOSIT_ID).ToList()).Contains(o.ID)).ToList(); 
                    }
                    listTreatmentIds = lstDeposit.Where(o => !o.IS_CANCEL.HasValue).Select(s => s.TDL_TREATMENT_ID).ToList(); 
                }
                //List<V_HIS_DERE_DETAIL> listDereDetails = new List<V_HIS_DERE_DETAIL>(); 
                //HisDereDetailViewFilterQuery dereDetailViewFilter = new HisDereDetailViewFilterQuery(); 
                //dereDetailViewFilter.CREATE_TIME_FROM = castFilter.TIME_FROM; 
                //dereDetailViewFilter.CREATE_TIME_TO = castFilter.TIME_TO; 
                //var listDereDetails = new MOS.MANAGER.HisDereDetail.HisDereDetailManager(param).GetView(dereDetailViewFilter); 

                //listDereDetails = listDereDetails.Where(s => s.DEPOSIT_IS_CANCEL == null && s.REPAY_ID == null).ToList(); 

                //listTreatmentIds = listDereDetails.Select(o => o.TDL_TREATMENT_ID).Distinct().ToList(); 

                //YC-DV
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var skip = 0; 
                    while (listTreatmentIds.Count() - skip > 0)
                    {
                        var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                        ssFilter.TREATMENT_IDs = limit; 
                        ssFilter.REQUEST_DEPARTMENT_IDs = listDepartments.Select(o => o.ID).ToList(); 
                        ssFilter.SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID; 
                        listSereServs.AddRange(new HisSereServManager(param).GetView(ssFilter));  
                    }
                }

                //YC-DV
                var listServiceReq = new List<V_HIS_SERVICE_REQ>(); 
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var skip = 0; 
                    while (listTreatmentIds.Count() - skip > 0)
                    {
                        var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery(); 
                        reqFilter.TREATMENT_IDs = limit; 
                        reqFilter.REQUEST_DEPARTMENT_IDs = listDepartments.Select(o => o.ID).ToList(); 
                        reqFilter.SERVICE_REQ_TYPE_ID = castFilter.SERVICE_TYPE_ID;
                        listServiceReq.AddRange(new HisServiceReqManager(param).GetView(reqFilter));  
                    }
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 

                if (IsNotNullOrEmpty(listSereServs))
                {
                    listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList(); 
                    //chuyen doi tuong
                    if (IsNotNullOrEmpty(listTreatmentIds))
                    {
                        var skip = 0; 
                        while (listTreatmentIds.Count - skip > 0)
                        {
                            var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                            HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                            patientTypeAlterViewFilter.TREATMENT_IDs = listIds; 
                            patientTypeAlterViewFilter.LOG_TIME_TO = castFilter.TIME_TO; 
                            listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));  
                        }
                    }

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
                var listPatientTypeAlterGroupByTreatmentIds = listPatientTypeAlters.GroupBy(s => s.TREATMENT_ID).ToList(); 
                foreach (var listPatientTypeAlterGroupByTreatmentId in listPatientTypeAlterGroupByTreatmentIds)
                {
                    var patientTypeAlter = listPatientTypeAlterGroupByTreatmentId.OrderByDescending(s => s.LOG_TIME).ToList(); 
                    var OUT_TIME = castFilter.TIME_TO; 
                    foreach (var i in patientTypeAlter)
                    {
                        var pta = new PATIENT_TYPE_ALTER(); 
                        pta.TREATMENT_ID = i.TREATMENT_ID; 

                        pta.TREATMENT_TYPE_ID = i.TREATMENT_TYPE_ID; 
                        pta.TREATMENT_TYPE_CODE = i.TREATMENT_TYPE_CODE; 
                        pta.TREATMENT_TYPE_NAME = i.TREATMENT_TYPE_NAME; 

                        pta.PATIENT_TYPE_ID = i.PATIENT_TYPE_ID; 
                        pta.PATIENT_TYPE_CODE = i.PATIENT_TYPE_CODE; 
                        pta.PATIENT_TYPE_NAME = i.PATIENT_TYPE_NAME; 

                        pta.LOG_TIME = i.LOG_TIME; 
                        pta.OUT_TIME = OUT_TIME; 
                        listPTAs.Add(pta); 

                        OUT_TIME = i.LOG_TIME; 
                    }
                }
                // lấy ra thời gian bệnh nhân ngoại trú (khám)
                listPTAs = listPTAs.Where(s => s.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList(); 

                if (IsNotNullOrEmpty(listSereServs))
                {
                    V_HIS_SERVICE_REQ req = null; 
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue; 
                        req = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0]; 
                        var patientTypeAlter = listPTAs.Where(s => s.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).ToList(); 
                        if (IsNotNullOrEmpty(patientTypeAlter))
                        {
                            foreach (var pta in patientTypeAlter)
                            {

                                if (req.INTRUCTION_TIME > pta.LOG_TIME && req.INTRUCTION_TIME < pta.OUT_TIME)
                                {
                                    foreach (var service in listServices)
                                    {
                                        if (sereServ.PARENT_ID == service.ID)
                                        {
                                            for (int i = 1;  i <= sereServ.AMOUNT;  i++)
                                            {
                                                Mrs00372RDO rdo = new Mrs00372RDO(); 
                                                rdo.TREATMENT_CODE = req.TREATMENT_CODE; 
                                                rdo.PATIENT_NAME = req.TDL_PATIENT_NAME; 
                                                rdo.DOB = req.TDL_PATIENT_DOB; 
                                                rdo.GENDER = req.TDL_PATIENT_GENDER_NAME; 
                                                rdo.IS_HEIN = pta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT ? "X" : ""; 
                                                rdo.INTRUCTION_TIME = req.INTRUCTION_TIME; 
                                                rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                                                rdo.PRICE = sereServ.PRICE; 

                                                ListRdo.Add(rdo); 
                                            }
                                        }
                                    }
                                }
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
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                if (IsNotNullOrEmpty(listBranchs))
                {
                    dicSingleTag.Add("BRANCH_NAME", listBranchs.First().BRANCH_NAME); 
                }

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.TREATMENT_CODE).ToList()); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
