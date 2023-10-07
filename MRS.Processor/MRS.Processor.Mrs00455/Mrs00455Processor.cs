using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
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
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisCashierRoom;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using Inventec.Common.Logging;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisTreatmentResult;

namespace MRS.Processor.Mrs00455
{
    class Mrs00455Processor : AbstractProcessor
    {
        Mrs00455Filter castFilter = null;
        List<Mrs00455RDO> listRdo = new List<Mrs00455RDO>();
        List<Mrs00455RDO> GroupDepartment = new List<Mrs00455RDO>();
        List<Mrs00455RDO> GroupRoom = new List<Mrs00455RDO>();
        List<HIS_CASHIER_ROOM> listCashierRoom = new List<HIS_CASHIER_ROOM>();
        //List<HIS_SERE_SERV_EXT> listSSE = new List<HIS_SERE_SERV_EXT>();
        //List<HIS_SERE_SERV_TEIN> listSST = new List<HIS_SERE_SERV_TEIN>();
        List<V_HIS_TEST_INDEX> listTextIndex = new List<V_HIS_TEST_INDEX>();
        List<HIS_TREATMENT_END_TYPE> listEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_TREATMENT_RESULT> listTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        List<HIS_DHST> listDHST = new List<HIS_DHST>();
        public string listServiceVC = "455_VC";
        public string listServiceXRAY = "455_XRAY";
        public string listServiceMRI = "455_MRI";
        public string listServiceCT = "455_CT";
        public string listServiceOXY = "455_OXY";


        Dictionary<long, string> dicOtherResult = new Dictionary<long, string>();
        Dictionary<long, List<string>> dicTeinResult = new Dictionary<long, List<string>>();
        Dictionary<string, string> dicSampleResult = new Dictionary<string, string>();
        Dictionary<long, HIS_SERVICE> dicSv = new Dictionary<long, HIS_SERVICE>();
        Dictionary<long, HIS_SERVICE> dicPar = new Dictionary<long, HIS_SERVICE>();
        List<HIS_HEIN_SERVICE_TYPE> ListHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();

        string thisReportTypeCode = "";
        public Mrs00455Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00455Filter);
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
                var acsUsers = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery());
                dicSingleTag.Add("CASHIER_USERNAME", castFilter.CASHIER_LOGINNAMEs != null ? string.Join(",", acsUsers.Where(p => castFilter.CASHIER_LOGINNAMEs.Contains(p.LOGINNAME)).Select(o => o.USERNAME).ToList()) : "");
                dicSingleTag.Add("CASHIER_ROOM_NAME", castFilter.EXACT_CASHIER_ROOM_IDs != null ? string.Join(",", (new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery() { IDs = castFilter.EXACT_CASHIER_ROOM_IDs }) ?? new List<V_HIS_CASHIER_ROOM>()).Select(o => o.CASHIER_ROOM_NAME).ToList()) : "");
                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.PATIENT_CODE).ToList());
                objectTag.AddObjectData(store, "GroupRoom", GroupRoom);
                objectTag.AddObjectData(store, "GroupDepartment", GroupDepartment);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupDepartment", "GroupRoom", "DEPARTMENT_CODE", "DEPARTMENT_CODE");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupRoom", "Report", new string[] { "DEPARTMENT_CODE", "ROOM_CODE" }, new string[] { "DEPARTMENT_CODE", "ROOM_CODE" });
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

                string queryTextIndex = "select * from v_his_test_index";
                listTextIndex = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TEST_INDEX>(queryTextIndex);

                string queryEndType = "select * from his_rs.his_treatment_end_type";
                listEndType = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT_END_TYPE>(queryEndType);

                listTreatmentResult = new HisTreatmentResultManager().Get(new HisTreatmentResultFilterQuery());

                //string queryDHST = "select * from his_rs.his_dhst";
                //listDHST = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DHST>(queryDHST);
                this.castFilter = (Mrs00455Filter)this.reportFilter;

                listRdo = new ManagerSql().GetRdo(castFilter)??new List<Mrs00455RDO>();
                var listSSE = new ManagerSql().GetSse(castFilter) ?? new List<HIS_SERE_SERV_EXT>();
                var listSST = new ManagerSql().GetSst(castFilter)??new List<HIS_SERE_SERV_TEIN>();

                string querySample = "select sam.service_req_code, sr.service_result_name from lis_rs.lis_sample_service sa \n";
                querySample += "join lis_rs.lis_sample sam on sa.sample_id = sam.id \n";
                querySample += "join lis_rs.lis_service_result sr on sa.service_result_id = sr.id";
                var listSample = new LIS.DAO.Sql.SqlDAO().GetSql<LISMrs00455RDO>(querySample) ?? new List<LISMrs00455RDO>();

                dicOtherResult = listSSE.Where(o => o.END_TIME > 0).GroupBy(g => g.SERE_SERV_ID).ToDictionary(p => p.Key, q => (q.First().DESCRIPTION ?? "") + "_AND_" + (q.First().CONCLUDE ?? ""));
                dicTeinResult = listSST.GroupBy(g => g.SERE_SERV_ID).ToDictionary(p => p.Key, q => q.Select(o => (o.TEST_INDEX_ID ?? 0) + "_IS_" + (o.VALUE ?? "")).ToList());
                dicSampleResult = listSample.GroupBy(g => g.SERVICE_REQ_CODE??"NONE").ToDictionary(p => p.Key, q => string.Join(";",q.Select(o=>o.SERVICE_RESULT_NAME).Distinct().ToList()));
                Inventec.Common.Logging.LogSystem.Info("To Dictionary _OK");

                GetCashierRoom();

                //dịch vụ
                GetService();

                //loại dịch vụ bảo hiểm
                GetHeinServiceType();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetCashierRoom()
        {
            listCashierRoom = new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery());
        }


        private void GetService()
        {
            HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
            if (castFilter.SERVICE_IDs != null)
            {
                serviceFilter.IDs = castFilter.SERVICE_IDs;
            }
            var listService = new HisServiceManager().Get(serviceFilter);
            if (listService != null)
            {
                dicSv = listService.ToDictionary(o => o.ID, p => p);
                dicPar = listService.ToDictionary(o => o.ID, p => listService.FirstOrDefault(q => q.ID == p.PARENT_ID) ?? new HIS_SERVICE());
            }
        }

        private void GetHeinServiceType()
        {
            ListHeinServiceType = new HisHeinServiceTypeManager().Get(new HisHeinServiceTypeFilterQuery());
          
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (castFilter.EXIST_SERVICE_IDs != null)
                {
                    //lọc các bệnh nhân có dùng dịch vụ đã chọn
                    FilterTreatmentByService();
                }
                if (castFilter.TDL_OTHER_PAY_SOURCE_IDs != null)
                {
                    //lọc các bệnh nhân có dùng nguồn đã chọn
                    FilterTreatmentBySource();
                }
                foreach (var rdo in listRdo)
                {
                    #region
                    rdo.BIRTH_DAY = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.TDL_PATIENT_DOB ?? 0);
                    rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.IN_TIME_NUM);
                    rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.OUT_TIME_NUM ?? 0);
                    rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.INTRUCTION_TIME);
                    
                    if (dicSampleResult.ContainsKey(rdo.SERVICE_REQ_CODE??""))
                    {
                        rdo.PCR = dicSampleResult[rdo.SERVICE_REQ_CODE ?? ""];
                    }
                    rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    rdo.TREATMENT_END_TYPE_NAME = rdo.TREATMENT_END_TYPE_ID != null && listEndType != null ? listEndType.Where(o => o.ID == rdo.TREATMENT_END_TYPE_ID).FirstOrDefault().TREATMENT_END_TYPE_NAME : "";
                    rdo.TREATMENT_RESULT_NAME = rdo.TREATMENT_RESULT_ID != null && listTreatmentResult != null ? listTreatmentResult.Where(o => o.ID == rdo.TREATMENT_RESULT_ID).FirstOrDefault().TREATMENT_RESULT_NAME : "";
                    rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == rdo.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;

                    //thông tin dịch vụ
                    var service = dicSv.ContainsKey(rdo.SERVICE_ID) ? dicSv[rdo.SERVICE_ID] : null;
                    if (service != null)
                    {
                        rdo.SERVICE_CODE = service.SERVICE_CODE;
                        rdo.SERVICE_NAME = service.SERVICE_NAME;
                    }
                    //thông tin dịch vụ cha
                    var par = dicPar.ContainsKey(rdo.SERVICE_ID) ? dicPar[rdo.SERVICE_ID] : null;
                    if (par != null)
                    {
                        rdo.PR_SERVICE_CODE = par.SERVICE_CODE;
                        rdo.PR_SERVICE_NAME = par.SERVICE_NAME;
                    }

                    //thông tin loại dịch vụ
                    var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID==rdo.TDL_SERVICE_TYPE_ID);
                    if (serviceType != null)
                    {
                        rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        rdo.TDL_SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        rdo.TDL_SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                    }
                    var heinServiceType = ListHeinServiceType.FirstOrDefault(o=>o.ID == rdo.TDL_HEIN_SERVICE_TYPE_ID);
                    if (heinServiceType != null)
                    {
                        rdo.HEIN_SERVICE_TYPE_CODE = heinServiceType.HEIN_SERVICE_TYPE_CODE;
                    }
                    var room = rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH ? HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.EXECUTE_ROOM_ID) : HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.REQUEST_ROOM_ID);
                    if (room != null)
                    {

                        rdo.DEPARTMENT_ID = room.DEPARTMENT_ID;
                        rdo.DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                        rdo.ROOM_CODE = room.ROOM_CODE;
                        rdo.ROOM_NAME = room.ROOM_NAME;
                    }
                    var Dexe = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID == rdo.EXECUTE_DEPARTMENT_ID);
                    if (Dexe != null)
                    {

                        rdo.EXECUTE_DEPARTMENT_CODE = Dexe.DEPARTMENT_CODE;
                        rdo.EXECUTE_DEPARTMENT_NAME = Dexe.DEPARTMENT_NAME;
                    }
                    if (castFilter.INPUT_DATA_ID_PRICE_TYPEs != null)
                    {
                        decimal totalPrice = 0;
                        if (castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)1))
                        {
                            if (string.IsNullOrWhiteSpace(rdo.HEIN_CARD_NUMBER))
                            {
                                totalPrice += rdo.VIR_TOTAL_PRICE ?? 0;
                            }
                        }

                        if (castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)2))
                        {
                            if (!string.IsNullOrWhiteSpace(rdo.HEIN_CARD_NUMBER))
                            {
                                totalPrice += rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                            }
                        }

                        if (castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)3))
                        {
                            if (!string.IsNullOrWhiteSpace(rdo.HEIN_CARD_NUMBER))
                            {
                                totalPrice += rdo.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                        }
                        rdo.VIR_TOTAL_PRICE = totalPrice;
                    }
                    //congkham
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_EXAM = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_EXAM = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //Xetngiem
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_TRSP = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_TEST = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //XRAY
                    if (listServiceXRAY == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_XRAY = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_XRAY = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //MRI
                    if (listServiceXRAY == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_MRI = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_MRI = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // CT
                    if (listServiceCT == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_CT = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_CT = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //tham do chuc nang
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_FUEX = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_FUEX = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //Phau thuat thu thuat
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_SURG = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_SURG = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // thuoc
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM && listServiceOXY == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_MEDI = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_MEDI = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // vat tu
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM && listServiceOXY == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_MATE = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_MATE = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // van chuyen
                    if (listServiceVC == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_TRSP = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_TRSP = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // Ngay giuong
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_BED = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_BED = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // mau
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_BLOOD = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_BLOOD = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // oxy
                    if (listServiceOXY == rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_OXY = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_OXY = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // noi soi
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_ENDO = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_ENDO = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // sieu am
                    if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_SUIM = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_SUIM = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    // khác
                    if ((rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC
                        || rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
                        && listServiceXRAY != rdo.CATEGORY_CODE
                        && listServiceVC != rdo.CATEGORY_CODE
                        && listServiceMRI != rdo.CATEGORY_CODE
                        && listServiceCT != rdo.CATEGORY_CODE
                        && listServiceOXY != rdo.CATEGORY_CODE
                        && listServiceVC != rdo.CATEGORY_CODE)
                    {
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_RICE_OTHER = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        if (rdo.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_RICE_OTHER = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        }
                    }

                    //var result_xn = listSST.Where(o => o.SERE_SERV_ID == rdo.SERE_SERV_ID).ToList();
                    //var result_other = listSSE.FirstOrDefault(o => o.SERE_SERV_ID == rdo.SERE_SERV_ID);
                    if (dicTeinResult.ContainsKey(rdo.SERE_SERV_ID))
                    {
                        List<string> strResultIndex = dicTeinResult[rdo.SERE_SERV_ID];
                        string value = "";
                        if (strResultIndex != null && strResultIndex.Count > 0)
                        {
                           
                            foreach (var item in strResultIndex)
                            {
                                string[] pairIndexValue = item.Split(new string[] { "_IS_" },StringSplitOptions.None);
                                if (pairIndexValue.Length < 2 || string.IsNullOrWhiteSpace(pairIndexValue[0])|| string.IsNullOrWhiteSpace(pairIndexValue[1]))
                                    continue;
                                long testIndexId = 0;
                                long.TryParse(pairIndexValue[0], out testIndexId);
                               var testIndex = listTextIndex.FirstOrDefault(o => o.ID == testIndexId);
                                if (testIndex != null)
                                {
                                    value += string.Format(" {0}: {1} {2},", testIndex.TEST_INDEX_NAME, pairIndexValue[1], testIndex.TEST_INDEX_UNIT_NAME);
                                }
                            }

                        }
                        rdo.TEST_RESULT = value;
                    }

                    if (dicOtherResult.ContainsKey(rdo.SERE_SERV_ID))
                    {
                        string strResult = dicOtherResult[rdo.SERE_SERV_ID];
                        string[] pairResult = strResult.Split(new string[] { "_AND_" }, StringSplitOptions.None);
                        if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                        {
                            if (pairResult.Length >0)
                            rdo.OTHER_RESULT = pairResult[0];
                        }
                        else
                        {
                            if (pairResult.Length > 1)
                                rdo.OTHER_RESULT = pairResult[1];
                        }
                    }

                    #endregion
                }


                if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                {
                    listRdo = listRdo.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                }

                string KeyGroupSS = "{0}_{1}_{2}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null)
                {
                    KeyGroupSS = this.dicDataFilter["KEY_GROUP_SS"].ToString();
                }
                listRdo = listRdo.GroupBy(s => string.Format(KeyGroupSS, s.PATIENT_CODE, s.DEPARTMENT_CODE, s.ROOM_CODE, s.TREATMENT_CODE, s.TRANSACTION_CODE, s.CASHIER_ROOM_ID, s.TDL_TREATMENT_TYPE_ID, s.FEE_LOCK_DATE ?? 0, s.BILL_NUM_ORDER ?? 0, s.CASHIER_LOGINNAME, s.INTRUCTION_TIME, s.SERVICE_REQ_CODE)).Select(s => new Mrs00455RDO
                {
                    PATIENT_CODE = s.First().PATIENT_CODE,
                    PATIENT_NAME = s.First().PATIENT_NAME,
                    TDL_PATIENT_DOB = s.First().TDL_PATIENT_DOB,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,
                    TRANSACTION_CODE = s.First().TRANSACTION_CODE,
                    CASHIER_ROOM_ID = s.First().CASHIER_ROOM_ID,
                    TDL_TREATMENT_TYPE_ID = s.First().TDL_TREATMENT_TYPE_ID,
                    FEE_LOCK_DATE = s.First().FEE_LOCK_DATE ?? 0,
                    CASHIER_ROOM_CODE = s.First().CASHIER_ROOM_ID != null ? (listCashierRoom.FirstOrDefault(o => o.ID == s.First().CASHIER_ROOM_ID) ?? new HIS_CASHIER_ROOM()).CASHIER_ROOM_CODE : "",
                    BIRTH_DAY = s.First().BIRTH_DAY,
                    TDL_PATIENT_GENDER_ID = s.First().TDL_PATIENT_GENDER_ID,
                    GENDER_NAME = s.First().GENDER_NAME,
                    PR_SERVICE_CODE = s.First().PR_SERVICE_CODE,
                    INTRUCTION_TIME_STR = s.First().INTRUCTION_TIME_STR,
                    HEIN_CARD_NUMBER = s.First().HEIN_CARD_NUMBER,
                    TDL_HEIN_CARD_NUMBER = s.First().TDL_HEIN_CARD_NUMBER,
                    PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == s.First().PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME ,
                    TDL_SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == s.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ,
                    TDL_SERVICE_TYPE_NAME =  (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == s.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME ,
                    PR_SERVICE_NAME = s.First().PR_SERVICE_NAME,
                    TDL_SERVICE_TYPE_ID = s.First().TDL_SERVICE_TYPE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    TREATMENT_END_TYPE_NAME = s.First().TREATMENT_END_TYPE_NAME,
                    TREATMENT_TYPE_NAME = s.First().TREATMENT_TYPE_NAME,
                    ICD_SUB_CODE = s.First().ICD_SUB_CODE,
                    ICD_10 = s.First().ICD_10,
                    MEDI_ORG_CODE = s.First().MEDI_ORG_CODE,
                    MEDI_ORG_NAME = s.First().MEDI_ORG_NAME,
                    IN_TIME = s.First().IN_TIME,
                    OUT_TIME = s.First().OUT_TIME,
                    IN_TIME_NUM = s.First().IN_TIME_NUM,
                    OUT_TIME_NUM = s.First().OUT_TIME_NUM,
                    INTRUCTION_TIME = s.First().INTRUCTION_TIME,
                    INTRUCTION_DATE = s.First().INTRUCTION_DATE,
                    START_TIME = s.First().START_TIME,
                    FINISH_TIME = s.First().FINISH_TIME,
                    TREATMENT_RESULT_NAME = s.First().TREATMENT_RESULT_NAME,
                    IN_CODE = s.First().IN_CODE,
                    OUT_CODE = s.First().OUT_CODE,
                    TREATMENT_DAY_COUNT = s.First().TREATMENT_DAY_COUNT,
                    DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    EXECUTE_DEPARTMENT_CODE = s.First().EXECUTE_DEPARTMENT_CODE,
                    EXECUTE_DEPARTMENT_NAME = s.First().EXECUTE_DEPARTMENT_NAME,
                    ROOM_NAME = s.First().ROOM_NAME,
                    BILL_NUM_ORDER = s.First().BILL_NUM_ORDER,
                    CASHIER_LOGINNAME = s.First().CASHIER_LOGINNAME,
                    CASHIER_USERNAME = s.First().CASHIER_USERNAME,
                    COUNT_TREATMENT = s.Select(p => p.TREATMENT_CODE).Distinct().Count(),// mien giam
                    TOTAL_RICE_TEST = s.Sum(o => o.TOTAL_RICE_TEST ?? 0), //xet nghiem
                    TOTAL_RICE_XRAY = s.Sum(o => o.TOTAL_RICE_XRAY ?? 0),
                    TOTAL_RICE_MRI = s.Sum(o => o.TOTAL_RICE_MRI ?? 0),
                    TOTAL_RICE_CT = s.Sum(o => o.TOTAL_RICE_CT ?? 0),
                    TOTAL_RICE_FUEX = s.Sum(o => o.TOTAL_RICE_FUEX ?? 0), // tham do chuc nang
                    TOTAL_RICE_ENDO = s.Sum(o => o.TOTAL_RICE_ENDO ?? 0), // noi soi
                    TOTAL_RICE_SUIM = s.Sum(o => o.TOTAL_RICE_SUIM ?? 0),  // sieu am
                    TOTAL_RICE_MEDI = s.Sum(o => o.TOTAL_RICE_MEDI ?? 0),  // thuoc
                    TOTAL_RICE_BLOOD = s.Sum(o => o.TOTAL_RICE_BLOOD ?? 0), // máu
                    TOTAL_RICE_SURG = s.Sum(o => o.TOTAL_RICE_SURG ?? 0), // PTTT
                    TOTAL_RICE_EXAM = s.Sum(o => o.TOTAL_RICE_EXAM ?? 0), // khám
                    TOTAL_RICE_MATE = s.Sum(o => o.TOTAL_RICE_MATE ?? 0), // vat tu
                    TOTAL_RICE_BED = s.Sum(o => o.TOTAL_RICE_BED ?? 0), // giuong
                    TOTAL_RICE_OXY = s.Sum(o => o.TOTAL_RICE_OXY ?? 0), // oxy 
                    TOTAL_RICE_TRSP = s.Sum(o => o.TOTAL_RICE_TRSP ?? 0), // van chuyen
                    TOTAL_RICE_OTHER = s.Sum(o => o.TOTAL_RICE_OTHER ?? 0),  //khac
                    TOTAL_EXPEND_PRICE = s.Sum(o => o.TOTAL_EXPEND_PRICE ?? 0),  //khac
                    VIR_TOTAL_PRICE = s.Sum(o => o.VIR_TOTAL_PRICE ?? 0),  //tong chi phí
                    VIR_TOTAL_HEIN_PRICE = s.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0),  //bao hiem
                    VIR_TOTAL_PATIENT_PRICE = s.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0),  //benh nhan
                    VIR_TOTAL_PATIENT_PRICE_BHYT = s.Sum(o => o.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0),  //dong chi tra
                    TOTAL_OTHER_SOURCE_PRICE = s.Sum(o => o.TOTAL_OTHER_SOURCE_PRICE ?? 0),  //nguon khac
                    TOTAL_PRICE_EXEM = s.GroupBy(o => o.TRANSACTION_CODE).Select(p => p.First()).Sum(su => su.TOTAL_PRICE_EXEM ?? 0),// mien giam
                    DIC_CATE_TOTAL_PRICE = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE ?? 0)),
                    DIC_CATE_TOTAL_HEIN_PRICE = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0)),
                    DIC_CATE_TOTAL_PATIENT_PRICE = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0)),
                    DIC_CATE_TOTAL_PATIENT_PRICE_BHYT = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)),
                    DIC_CATE_TOTAL_PATIENT_PRICE_SELF = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE ?? 0) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0))),
                    DIC_CATE_TOTAL_OTHER_SOURCE_PRICE = s.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE ?? 0)),
                    DIC_SVT_TOTAL_PRICE = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE ?? 0)),
                    DIC_SVT_TOTAL_HEIN_PRICE = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0)),
                    DIC_SVT_TOTAL_PATIENT_PRICE = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0)),
                    DIC_SVT_TOTAL_PATIENT_PRICE_BHYT = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)),
                    DIC_SVT_TOTAL_PATIENT_PRICE_SELF = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE ?? 0) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0))),
                    DIC_SVT_TOTAL_OTHER_SOURCE_PRICE = s.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE ?? 0)),
                    DIC_PAR_TOTAL_PRICE = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE ?? 0)),
                    DIC_PAR_TOTAL_HEIN_PRICE = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0)),
                    DIC_PAR_TOTAL_PATIENT_PRICE = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0)),
                    DIC_PAR_TOTAL_PATIENT_PRICE_BHYT = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)),
                    DIC_PAR_TOTAL_PATIENT_PRICE_SELF = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE ?? 0) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0))),
                    DIC_PAR_TOTAL_OTHER_SOURCE_PRICE = s.GroupBy(o => o.PR_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE ?? 0)),
                    DIC_HSVT_TOTAL_PRICE = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE ?? 0)),
                    DIC_HSVT_TOTAL_HEIN_PRICE = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0)),
                    DIC_HSVT_TOTAL_PATIENT_PRICE = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0)),
                    DIC_HSVT_TOTAL_PATIENT_PRICE_BHYT = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)),
                    DIC_HSVT_TOTAL_PATIENT_PRICE_SELF = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE ?? 0) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0))),
                    DIC_HSVT_TOTAL_OTHER_SOURCE_PRICE = s.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE ?? 0)),
                    DIC_DEXE_TOTAL_PRICE = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PRICE ?? 0)),
                    DIC_DEXE_TOTAL_HEIN_PRICE = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0)),
                    DIC_DEXE_TOTAL_PATIENT_PRICE = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0)),
                    DIC_DEXE_TOTAL_PATIENT_PRICE_BHYT = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)),
                    DIC_DEXE_TOTAL_PATIENT_PRICE_SELF = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => (su.VIR_TOTAL_PATIENT_PRICE ?? 0) - (su.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0))),
                    DIC_DEXE_TOTAL_OTHER_SOURCE_PRICE = s.GroupBy(o => o.EXECUTE_DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(su => su.TOTAL_OTHER_SOURCE_PRICE ?? 0)),
                    BELLY = s.First().BELLY,
                    BLOOD_PRESSURE_MAX = s.First().BLOOD_PRESSURE_MAX,
                    BLOOD_PRESSURE_MIN = s.First().BLOOD_PRESSURE_MIN,
                    BREATH_RATE = s.First().BREATH_RATE,
                    CHEST = s.First().CHEST,
                    HEIGHT = s.First().HEIGHT,
                    PULSE = s.First().PULSE,
                    TEMPERATURE = s.First().TEMPERATURE,
                    SP02 = s.First().SP02,
                    WEIGHT = s.First().WEIGHT,
                    TEST_RESULT = s.First().TEST_RESULT,
                    OTHER_RESULT = s.First().OTHER_RESULT,
                    PCR = s.First().PCR
                }).ToList();

                GroupRoom = listRdo.GroupBy(s => new { s.DEPARTMENT_CODE, s.ROOM_CODE }).Select(s => new Mrs00455RDO
                {
                    DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    ROOM_CODE = s.First().ROOM_CODE,
                    ROOM_NAME = s.First().ROOM_NAME,
                }).ToList();

                GroupDepartment = listRdo.GroupBy(s => new { s.DEPARTMENT_CODE }).Select(s => new Mrs00455RDO
                {
                    DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                    DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterTreatmentByService()
        {
            var treatmentIds = listRdo.Where(o => castFilter.EXIST_SERVICE_IDs.Contains(o.SERVICE_ID)).Select(p => p.TREATMENT_ID).Distinct().ToList();
            listRdo = listRdo.Where(o => treatmentIds.Contains(o.TREATMENT_ID)).ToList();
        }

        private void FilterTreatmentBySource()
        {
            var treatmentIds = listRdo.Where(o => castFilter.TDL_OTHER_PAY_SOURCE_IDs.Contains(o.OTHER_PAY_SOURCE_ID??0)).Select(p => p.TREATMENT_ID).Distinct().ToList();
            listRdo = listRdo.Where(o => treatmentIds.Contains(o.TREATMENT_ID)).ToList();
        }


    }
}
