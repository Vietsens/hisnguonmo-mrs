using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExecuteRole;
using MOS.MANAGER.HisEkipUser;
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MRS.Processor.Mrs00225; 
using MOS.MANAGER.HisSereServ; 
 
using MOS.MANAGER.HisSereServPttt; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisTreatmentType; 
using MOS.MANAGER.HisPtttGroup; 
using MOS.MANAGER.HisServiceReq; 

namespace MRS.Processor.Mrs00225
{
    public class Mrs00225Processor : AbstractProcessor
    {
        private List<Mrs00225RDO> listMrs00225Rdos = new List<Mrs00225RDO>();
        private Mrs00225Filter FilterMrs00225;

        private List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        private List<V_HIS_EKIP_USER> listEkipUsers; 
        private List<V_HIS_SERE_SERV_PTTT> listSereServPttts; 
        private List<HIS_TREATMENT_TYPE> listTreatmentTypes; 
        private List<HIS_PTTT_GROUP> listPtttGroups; 
        private List<V_HIS_TREATMENT> listTreatments;
        private List<HIS_SERVICE> listService;


        public Mrs00225Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            FilterMrs00225 = ((Mrs00225Filter)this.reportFilter); 
            return typeof(Mrs00225Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            var paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
            try
            {
                //Yeu cau
                string query = "select ss.* from v_his_sere_serv ss \n";
                query += " join v_his_service_req sr on ss.service_req_id = sr.id \n";
                query += " join his_treatment trea on trea.id = ss.tdl_treatment_id \n";
                query += "where 1=1 and ss.is_delete=0 and ss.is_no_execute is null\n";
                //chon thoi gian
                if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND ss.id in (select sse.sere_serv_id from his_sere_serv_ext sse where SSE.BEGIN_TIME BETWEEN {0} and {1}) \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND ss.id in (select ssb.sere_serv_id from his_sere_serv_bill ssb join his_transaction tran on tran.id=ssb.bill_id where tran.is_cancel is null and TRAN.TRANSACTION_TIME BETWEEN {0} and {1}) \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);
                }
                else if (((Mrs00225Filter)this.reportFilter).INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);
                }
                else
                    query += string.Format("and sr.finish_time between {0} and {1} \n", ((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM, ((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO);

                query += string.Format("and sr.service_req_type_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT);
                if (((Mrs00225Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null) //trường lọc Khoa thực hiện (chọn 1)
                {
                    query += string.Format("and sr.execute_department_id = {0} \n", ((Mrs00225Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID);
                }
                if (((Mrs00225Filter)this.reportFilter).EXECUTE_DEPARTMENT_IDs != null) //trường lọc Khoa thực hiện (chọn nhiều)
                {
                    query += string.Format("and sr.execute_department_id in ({0}) \n", string.Join(",", ((Mrs00225Filter)this.reportFilter).EXECUTE_DEPARTMENT_IDs));
                }
                if (((Mrs00225Filter)this.reportFilter).EXECUTE_ROOM_IDs != null) //trường lọc Khoa thực hiện (chọn nhiều)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", ((Mrs00225Filter)this.reportFilter).EXECUTE_ROOM_IDs));
                }
                if (((Mrs00225Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs != null) //trường lọc Khoa chỉ định (chọn nhiều)
                {
                    query += string.Format("and sr.request_department_id in ({0}) \n", string.Join(",", ((Mrs00225Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs));
                }
                if (((Mrs00225Filter)this.reportFilter).PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.patient_type_id in ({0}) \n", string.Join(",", ((Mrs00225Filter)this.reportFilter).PATIENT_TYPE_IDs));
                }
                if (((Mrs00225Filter)this.reportFilter).TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id = {0} \n", ((Mrs00225Filter)this.reportFilter).TREATMENT_TYPE_ID);
                }
                if (((Mrs00225Filter)this.reportFilter).PTTT_GROUP_IDs != null)
                {
                    query += string.Format("and ss.service_id in (select id from his_service where pttt_group_id in ({0})) \n", string.Join(",", ((Mrs00225Filter)this.reportFilter).PTTT_GROUP_IDs));
                }
                LogSystem.Info("START QUERY: " + query);
                listSereServs = new List<V_HIS_SERE_SERV>();
                listSereServs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_SERE_SERV>(query) ?? new List<V_HIS_SERE_SERV>();
                
                LogSystem.Info("a");
                if (listSereServs != null)
                {
                    //DV
                    var listServiceId = listSereServs.Select(p => p.SERVICE_ID).Distinct().ToList();
                    listService = new List<HIS_SERVICE>();
                    if (IsNotNullOrEmpty(listServiceId))
                    {
                        int skip = 0;
                        while (listServiceId.Count - skip > 0)
                        {
                            var listId = listServiceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var serviceFilter = new HisServiceFilterQuery()
                            {
                                IDs = listId
                            };
                            var listServiceSub = new HisServiceManager(paramGet).Get(serviceFilter);
                            listService.AddRange(listServiceSub);
                        }
                    }
                    LogSystem.Info("1");
                    //Thong tin phau thuat
                    var listSereServId = listSereServs.Select(s => s.ID).Distinct().ToList();
                    listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>();
                    if (IsNotNullOrEmpty(listSereServId))
                    {
                        var skip = 0;
                        while (listSereServId.Count() - skip > 0)
                        {
                            var ListDSs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var sereServPtttFilter = new HisSereServPtttViewFilterQuery()
                            {
                                SERE_SERV_IDs = ListDSs,
                            };
                            var sereServPttts = new HisSereServPtttManager(paramGet).GetView(sereServPtttFilter);
                            listSereServPttts.AddRange(sereServPttts);
                        }
                    }

                    LogSystem.Info("2");
                    //thong tin kip mo

                    var listEkipId = listSereServs.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
                    listEkipUsers = new List<V_HIS_EKIP_USER>();
                    if (IsNotNullOrEmpty(listEkipId))
                    {
                        var skip = 0;
                        while (listEkipId.Count() - skip > 0)
                        {
                            var ListDSs = listEkipId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var ekipUserFilter = new HisEkipUserViewFilterQuery()
                            {
                                EKIP_IDs = ListDSs
                            };
                            var ekipUsers = new HisEkipUserManager(paramGet).GetView(ekipUserFilter);
                            listEkipUsers.AddRange(ekipUsers);
                        }
                    }
                    LogSystem.Info("3");
                   
                    listTreatments = new List<V_HIS_TREATMENT>();
                    var listTreaId = listSereServs.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList();
                    if (IsNotNullOrEmpty(listTreaId))
                    {
                        var skip = 0;
                        while (listTreaId.Count() - skip > 0)
                        {
                            var ListDSs = listTreaId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var treatmentFilter = new HisTreatmentViewFilterQuery()
                            {
                                IDs = ListDSs,
                            };
                            var treatments = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                            listTreatments.AddRange(treatments);
                        }
                    }
                    LogSystem.Info("5");
                    //nhom phau thuat
                    var listPtttGroupId = listSereServPttts.Select(s => s.PTTT_GROUP_ID ?? 0).Distinct().ToList();
                    listPtttGroups = new List<HIS_PTTT_GROUP>();
                    if (IsNotNullOrEmpty(listPtttGroupId))
                    {
                        var skip = 0;
                        while (listPtttGroupId.Count() - skip > 0)
                        {
                            var ListDSs = listPtttGroupId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var ptttGroupFilter = new HisPtttGroupFilterQuery()
                            {
                                IDs = ListDSs,
                            };
                            var ptttGroups = new HisPtttGroupManager(paramGet).Get(ptttGroupFilter);
                            listPtttGroups.AddRange(ptttGroups);
                        }
                    }
                }
                LogSystem.Info("b");
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
            bool result = true; 
            try
            {
                long STT = 0;
                foreach (var treatment in listTreatments)
                {
                    var sereServs = listSereServs.Where(s => s.TDL_TREATMENT_ID == treatment.ID).ToList();
                    ProcessDetail(sereServs, treatment, ref STT);
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private void ProcessDetail(List<V_HIS_SERE_SERV> listSS, V_HIS_TREATMENT treatment, ref long STT)
        {
            try
            {
                if (listSS != null)
                {
                    foreach (var sereServ in listSS)
                    {
                        var service = listService.FirstOrDefault(p => p.ID == sereServ.SERVICE_ID);

                        var ekipUsers = listEkipUsers.Where(s => s.EKIP_ID == sereServ.EKIP_ID).ToList();

                        var sereServPttts = listSereServPttts.Where(s => s.SERE_SERV_ID == sereServ.ID).ToList();
                        if (IsNotNullOrEmpty(sereServPttts))
                        {
                            foreach (var sereServPttt in sereServPttts)
                            {
                                STT++;
                                var rdo = new Mrs00225RDO(treatment,sereServ,service, ekipUsers);
                                rdo.STT = STT;
                                rdo.BEFORE_PTTT_ICD_NAME = sereServPttt.BEFORE_PTTT_ICD_NAME;
                                rdo.AFTER_PTTT_ICD_NAME = sereServPttt.AFTER_PTTT_ICD_NAME;
                                rdo.PTTT_METHOD_NAME = sereServPttt.PTTT_METHOD_NAME;
                                rdo.EMOTIONLESS_METHOD_NAME = sereServPttt.EMOTIONLESS_METHOD_NAME;
                                listMrs00225Rdos.Add(rdo);
                            }
                        }    
                        else
                        {
                            var rdo = new Mrs00225RDO(treatment, sereServ, service, ekipUsers);
                            STT++;
                            rdo.STT = STT;
                            listMrs00225Rdos.Add(rdo);
                        }    
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM != null) dicSingleTag.Add("FINISH_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00225Filter)this.reportFilter).FINISH_TIME_FROM)); 
            if (((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO != null) dicSingleTag.Add("FINISH_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00225Filter)this.reportFilter).FINISH_TIME_TO)); 

            string PTTT_GROUP_NAME = ""; 
            if (IsNotNullOrEmpty(listPtttGroups))
                foreach (var listPtttGroup in listPtttGroups)
                {
                    PTTT_GROUP_NAME = PTTT_GROUP_NAME + " - " + listPtttGroup.PTTT_GROUP_NAME; 
                }
            dicSingleTag.Add("PTTT_GROUP_IDs", PTTT_GROUP_NAME); 
            if (listTreatmentTypes != null && listTreatmentTypes.Count > 0) dicSingleTag.Add("TREATMENT_TYPE_ID", listTreatmentTypes.First().TREATMENT_TYPE_NAME); 
            LogSystem.Info(listMrs00225Rdos.Count.ToString());
            objectTag.AddObjectData(store, "Report", listMrs00225Rdos);
            objectTag.AddObjectData(store, "Detail", listMrs00225Rdos);
            objectTag.AddObjectData(store, "PtttGroups", HisPtttGroupCFG.PTTT_GROUPs); 
        }
    }
}
