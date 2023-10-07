using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisPatient;
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
using HIS.Treatment.DateTime; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServBill; 
using MOS.MANAGER.HisService; 

namespace MRS.Processor.Mrs00454
{
    class Mrs00454Processor : AbstractProcessor
    {
        Mrs00454Filter castFilter = null; 
        List<Mrs00454RDO> listRdo = new List<Mrs00454RDO>(); 
        List<Mrs00454RDO> Service = new List<Mrs00454RDO>(); 
        List<Mrs00454RDO> GroupDepartment = new List<Mrs00454RDO>(); 
        List<Mrs00454RDO> GroupService = new List<Mrs00454RDO>(); 

        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        Dictionary<long,V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long,V_HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>(); 
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>(); 
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>(); 
        List<HIS_ICD> listIcds = new List<HIS_ICD>(); 
        public List<long> listServiceVC = null; 
        Dictionary<long, V_HIS_SERVICE_1> dicService = new Dictionary<long,V_HIS_SERVICE_1>(); 
        string thisReportTypeCode = ""; 
        public Mrs00454Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00454Filter); 
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
                objectTag.AddObjectData(store, "Service", Service); 
                objectTag.AddObjectData(store, "GroupDepartment", GroupDepartment); 
                objectTag.AddObjectData(store, "GroupService", GroupService); 

                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupService", "GroupDepartment", "GROUP_ID", "GROUP_ID"); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupDepartment", "Service", new string[] { "DEPARTMENT_ID", "GROUP_ID" }, new string[] { "DEPARTMENT_ID", "GROUP_ID" }); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Service", "Report", new string[] { "DEPARTMENT_ID", "GROUP_ID", "SERVICE_ID" }, new string[] { "DEPARTMENT_ID", "GROUP_ID", "SERVICE_ID" }); 
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
                this.castFilter = (Mrs00454Filter)this.reportFilter; 

                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00454"; 
                listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter); 
                //var listServiceIds = listServices.Select(s => s.SERVICE_ID).ToList(); 
                listServiceVC = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "454_VC").Select(s => s.SERVICE_ID).ToList(); 

                //HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery(); 
                //transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM; 
                //transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO; 
                //listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactionFilter); 

                //var skip = 0; 
                //while (listTransactions.Count - skip > 0)
                //{
                //    var ListId = listTransactions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                //    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                //    HisBillViewFilterQuery billFilter = new HisBillViewFilterQuery(); 
                //    billFilter.TRANSACTION_IDs = ListId.Select(s => s.ID).ToList(); 
                //    listBills.AddRange(new MOS.MANAGER.HisBill.HisBillManager(param).GetView(billFilter); 
                //}

                //skip = 0; 
                //while (listBills.Count - skip > 0)
                //{
                //    var ListId = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                //    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                //    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery(); 
                //    sereServBillFilter.BILL_IDs = ListId.Select(s => s.ID).ToList(); 
                //    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillFilter); 
                //}

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentFilter); 

                //Yeu cau
                var skip = 0; 
                List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>(); 
                while (listTreatments.Count - skip > 0)
                {
                    var ListId = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery(); 
                    serviceReqFilter.TREATMENT_IDs = ListId.Select(s => s.ID).ToList(); 
                    listServiceReq.AddRange(new HisServiceReqManager(param).GetView(serviceReqFilter)); 
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 

                //YC-DV
                skip = 0; 
                while (listTreatments.Count - skip > 0)
                {
                    var ListId = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                    sereServFilter.TREATMENT_IDs = ListId.Select(s => s.ID).ToList(); 
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServFilter)); 
                }

                skip = 0; 
                while (listSereServs.Count - skip > 0)
                {
                    var ListId = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery(); 
                    //departmentTranFilter.DEPARTMENT_IDs = ListId.Select(s => s.REQUEST_DEPARTMENT_ID).ToList(); 
                    departmentTranFilter.TREATMENT_IDs = ListId.Select(s => s.TDL_TREATMENT_ID??0).ToList(); 
                    listDepartmentTrans.AddRange(new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).GetView(departmentTranFilter)); 
                } //DV - thanh toan
                var listSereServId = listSereServs.Select(s => s.ID).ToList(); 

                if (IsNotNullOrEmpty(listSereServId))
                {
                    skip = 0; 
                    while (listSereServId.Count - skip > 0)
                    {
                        var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery(); 
                        filterSereServBill.SERE_SERV_IDs = listIDs; 
                        var listSereServBillSub = new HisSereServBillManager().GetView(filterSereServBill); 
                        listSereServBills.AddRange(listSereServBillSub); 
                    }
                }
                //dich vu
                HisServiceView1FilterQuery listServiceFilter = new HisServiceView1FilterQuery()
                {
                    SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
                };
                dicService = new HisServiceManager().GetView1(listServiceFilter).ToDictionary(o => o.ID); 
               

                listIcds = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(new HisIcdFilterQuery()); 
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
                V_HIS_SERVICE_REQ req = null; 
                var listSereServ = listSereServs.Where(s => s.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 
                foreach (var ss in listSereServ)
                {
                    if (!dicServiceReq.ContainsKey(ss.SERVICE_REQ_ID ?? 0)) continue; 
                    req = dicServiceReq[ss.SERVICE_REQ_ID ?? 0]; 
                    Mrs00454RDO rdo = new Mrs00454RDO(); 
                    rdo.PATIENT_CODE = req.TDL_PATIENT_CODE; 
                    rdo.PATIENT_NAME = req.TDL_PATIENT_NAME; 
                    rdo.TREATMENT_CODE = ss.TDL_TREATMENT_CODE; 
                    rdo.DEPARTMENT_NAME = req.REQUEST_DEPARTMENT_NAME; 
                    rdo.BIRTH_DAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(req.TDL_PATIENT_DOB); 
                    rdo.GENDER_NAME = req.TDL_PATIENT_GENDER_NAME; 
                    //rdo.ICD_10_IN = sereServs.; 
                    rdo.AMOUNT = ss.AMOUNT; 
                    rdo.PRICE = ss.PRICE; 
                    rdo.SERVICE_NAME = ss.TDL_SERVICE_NAME; 
                    rdo.SERVICE_ID = ss.SERVICE_ID; 
                    rdo.DEPARTMENT_ID = ss.TDL_REQUEST_DEPARTMENT_ID; 
                    rdo.PATIENT_TYPE_NAME = ss.PATIENT_TYPE_NAME; 
                    //rdo.GROUP_NAME = sereServs.SERVICE_TYPE_NAME; 
                    //congkham
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        rdo.GROUP_ID = 1; 
                        rdo.GROUP_NAME = "Khám bệnh/ Examinatio"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    //Xetngiem
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                    {
                        rdo.GROUP_ID = 2; 
                        rdo.GROUP_NAME = "Xét nghiệm/ Test"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    //CDHA
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA || ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS || ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                    {
                        rdo.GROUP_ID = 3; 
                        rdo.GROUP_NAME = "Chẩn đoán hình ảnh/ Imagery diagnostic"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    //tham do chuc nang
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN)
                    {
                        rdo.GROUP_ID = 4; 
                        rdo.GROUP_NAME = "Thăm dò chức năng/ Functional exploration"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    //Phau thuat thu thuat
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                    {
                        rdo.GROUP_ID = 5; 
                        rdo.GROUP_NAME = "Phẫu thuật - Thủ Thuật/ Surgery"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    // thuoc
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                    {
                        rdo.GROUP_ID = 6; 
                        rdo.GROUP_NAME = "Thuốc/ Medicine"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    // vat tu
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                    {
                        rdo.GROUP_ID = 7; 
                        rdo.GROUP_NAME = "Vật tư/ Material"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    // van chuyen
                    if (listServiceVC.Contains(ss.SERVICE_ID))
                    {
                        rdo.GROUP_ID = 8; 
                        rdo.GROUP_NAME = "Vận chuyển/ Transport"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    // Ngay giuong
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                    {
                        rdo.GROUP_ID = 9; 
                        rdo.GROUP_NAME = "Ngày giường/ Bed"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    // khác
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC)
                    {
                        rdo.GROUP_ID = 10; 
                        rdo.GROUP_NAME = "Khác/ Other"; 
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE_BHYT.Value; 
                        }
                        if (ss.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE = ss.VIR_TOTAL_PATIENT_PRICE.Value; 
                        }
                    }

                    var departmentTrans = listDepartmentTrans.Where(w => w.DEPARTMENT_ID == ss.TDL_REQUEST_DEPARTMENT_ID && w.TREATMENT_ID == ss.TDL_TREATMENT_ID).OrderBy(o => o.DEPARTMENT_IN_TIME).ToList(); 
                    if (IsNotNullOrEmpty(departmentTrans))
                    {
                        var departmentTranIn = departmentTrans.Where(w => w.DEPARTMENT_IN_TIME < req.INTRUCTION_TIME).ToList();  // lay Last()
                        if (IsNotNullOrEmpty(departmentTranIn))
                        {
                            if (departmentTranIn.First().DEPARTMENT_IN_TIME != 0)
                            {
                                rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTranIn.First().DEPARTMENT_IN_TIME??0); 
                            }
                            if (!String.IsNullOrEmpty(departmentTranIn.First().ICD_CODE))
                            {
                                var listIcdIn = listIcds.Where(s => s.ICD_CODE == departmentTranIn.First().ICD_CODE).ToList(); 
                                rdo.ICD_10_IN = listIcdIn.First().ICD_NAME; 
                            }
                        }
                        var departmentTranOut = listDepartmentTrans.Where(w => w.DEPARTMENT_IN_TIME > req.INTRUCTION_TIME).ToList();  //lay First()

                        if (IsNotNullOrEmpty(departmentTranOut))
                        {
                            if (departmentTranOut.First().DEPARTMENT_IN_TIME != 0)
                            {
                                rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTranOut.First().DEPARTMENT_IN_TIME??0); 
                            }
                            if (!String.IsNullOrEmpty(departmentTranOut.First().ICD_CODE))
                            {
                                var listIcdOut = listIcds.Where(s => s.ICD_CODE == departmentTranOut.First().ICD_CODE).ToList(); 
                                rdo.ICD_10_OUT = listIcdOut.First().ICD_NAME; 
                            }
                        }
                        if (!IsNotNullOrEmpty(departmentTranOut))
                        {
                            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO); 
                        }
                    }

                    listRdo.Add(rdo); 
                }

                listRdo = listRdo.GroupBy(s => new { s.GROUP_ID, s.DEPARTMENT_ID, s.SERVICE_ID, s.PATIENT_CODE, s.PRICE, s.TREATMENT_CODE, s.PATIENT_TYPE_NAME }).Select(s => new Mrs00454RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_ID = s.First().SERVICE_ID,
                    PATIENT_CODE = s.First().PATIENT_CODE,
                    PATIENT_NAME = s.First().PATIENT_NAME,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,
                    BIRTH_DAY = s.First().BIRTH_DAY,
                    GENDER_NAME = s.First().GENDER_NAME,
                    ICD_10_IN = s.First().ICD_10_IN,
                    ICD_10_OUT = s.First().ICD_10_OUT,
                    IN_TIME = s.First().IN_TIME,
                    OUT_TIME = s.First().OUT_TIME,
                    PATIENT_TYPE_NAME = s.First().PATIENT_TYPE_NAME,
                    AMOUNT = s.Sum(o => o.AMOUNT),
                    PRICE = s.First().PRICE,
                    //AMOUNT_TT s.Sum(o=>o.AMOUNT_TT),
                    TOTAL_RICE = s.Sum(o => o.TOTAL_RICE),
                    EXEM = s.Sum(o => o.EXEM),
                }).ToList(); 

                Service = listRdo.GroupBy(s => new { s.GROUP_ID, s.DEPARTMENT_ID, s.SERVICE_ID }).Select(s => new Mrs00454RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_ID = s.First().SERVICE_ID
                }).ToList(); 

                GroupDepartment = listRdo.GroupBy(s => new { s.GROUP_ID, s.DEPARTMENT_ID }).Select(s => new Mrs00454RDO
                {
                    DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                }).ToList(); 

                GroupService = listRdo.GroupBy(s => new { s.GROUP_ID }).Select(s => new Mrs00454RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                }).ToList(); 
                GroupService = GroupService.OrderBy(s => s.GROUP_ID).ToList(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }


    }
}
