using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDeathWithin;
using MOS.MANAGER.HisDebate;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisPtttCatastrophe;
using MOS.MANAGER.HisPtttCondition;
using MOS.MANAGER.HisPtttMethod;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00662
{
    class Mrs00662Processor : AbstractProcessor
    {
        Mrs00662Filter castFilter = null;
        List<Mrs00662RDOPTTM> ListRdoOutside = new List<Mrs00662RDOPTTM>();
        List<Mrs00662RDO> ListRdo = new List<Mrs00662RDO>();
        List<Mrs00662RDO> ListRdoDepartment = new List<Mrs00662RDO>();
        List<Mrs00662RDO> ListRdoDate = new List<Mrs00662RDO>();
        List<Mrs00662RDO> ListSereServ = new List<Mrs00662RDO>();
        List<Mrs00662RDOPTTM> ListRdoPTTM = new List<Mrs00662RDOPTTM>();
        List<V_HIS_EKIP_USER> ListEkipUser = new List<V_HIS_EKIP_USER>();
        List<HIS_PTTT_METHOD> ListHisPtttMethod = new List<HIS_PTTT_METHOD>();
        List<V_HIS_PATIENT> ListPatient = new List<V_HIS_PATIENT>();
        List<HIS_EXECUTE_ROLE> LisExecuteRole = new List<HIS_EXECUTE_ROLE>();
        List<HIS_PTTT_CATASTROPHE> ListPtttCatastrophe = new List<HIS_PTTT_CATASTROPHE>();
        List<HIS_PTTT_CONDITION> ListPtttCondition = new List<HIS_PTTT_CONDITION>();
        List<HIS_DEATH_WITHIN> ListDeathWithin = new List<HIS_DEATH_WITHIN>();
        List<Mrs00662RDO> ListTypeDepa = new List<Mrs00662RDO>();
        List<HIS_PATIENT_CLASSIFY> listPatientClassify = new List<HIS_PATIENT_CLASSIFY>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMedi = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMate = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<Mrs00662RDO> listChild = new List<Mrs00662RDO>(); //dv dinh kem
        Dictionary<long, List<Mrs00662RDO>> dicChild = new Dictionary<long, List<Mrs00662RDO>>(); //dv dinh kem

        List<HIS_SERE_SERV> listHP = new List<HIS_SERE_SERV>(); //dv hao phi
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_SERVICE> listServiceHC = new List<HIS_SERVICE>();

        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq = new Dictionary<long, List<HIS_SERVICE_REQ>>();

        List<HIS_HEIN_APPROVAL> ListHeinApproval = new List<HIS_HEIN_APPROVAL>();
        //thông tin hội chẩn

        //List<HIS_DEBATE> ListDebate = new List<HIS_DEBATE>();
        List<HIS_DEBATE> ListDebateNew = new List<HIS_DEBATE>();
        Dictionary<long, List<HIS_DEBATE>> dicDebateNew = new Dictionary<long, List<HIS_DEBATE>>();

        List<HIS_DEBATE_TYPE> ListDebateType = new List<HIS_DEBATE_TYPE>();
        public Mrs00662Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00662Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00662Filter)reportFilter);

                //get dữ liệu đối tượng chi tiết
                GetPatientClassify();

                //nếu lấy thẩm mỹ thì thôi mổ khác
                if (castFilter.IS_PTTM == true)
                {
                    ListRdoPTTM = new ManagerSql().GetPTTM(castFilter);
                }
                else
                {
                    ListSereServ = new ManagerSql().GetSS(castFilter);
                }
                
                //get hội chẩn
                GetDebate();

                //get loại hội chẩn
                GetDebateType();
                
                //get dịch vụ kèm theo
                GetChild();
                
                //Get dịch vụ
                GetService();
                
                //get dịch vụ hội chẩn ca bệnh khó
                GetServiceHC();
                
                //lọc theo khoa phòng và có kip
                FilterByDepaRoomAndHasEkip();

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    //get thông tin ekip
                    GetEkip();

                    //get thuốc vật tư kèm theo
                    GetMediMateReq();
                }

                //get phương pháp PTTT
                GetPTTTMethod();

                //get bệnh nhân
                GetPatient();

                //get tai biến
                ListPtttCatastrophe = new HisPtttCatastropheManager().Get(new HisPtttCatastropheFilterQuery());

                //get tình trạng phẫu thuật
                ListPtttCondition = new HisPtttConditionManager().Get(new HisPtttConditionFilterQuery());

                //get thời gian tử vong
                ListDeathWithin = new HisDeathWithinManager().Get(new HisDeathWithinFilterQuery());
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GetPatient()
        {
            var listPatientIds = ListSereServ.Select(s => s.TDL_PATIENT_ID ?? 0).Distinct().ToList();
            var skip2 = 0;
            while (listPatientIds.Count - skip2 > 0)
            {
                var listIDs = listPatientIds.Skip(skip2).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip2 += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisPatientViewFilterQuery paFilter = new HisPatientViewFilterQuery();
                paFilter.IDs = listIDs;
                var patient = new HisPatientManager().GetView(paFilter);
                if (IsNotNullOrEmpty(patient))
                {
                    ListPatient.AddRange(patient);
                }
            }
        }

        private void GetPTTTMethod()
        {
            var PtttMethodIds = ListSereServ.Select(o => o.PTTT_METHOD_ID ?? 0).Distinct().ToList();
            if (IsNotNullOrEmpty(PtttMethodIds))
            {
                var RealPtttMethodIds = ListSereServ.Select(o => o.REAL_PTTT_METHOD_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(RealPtttMethodIds))
                    PtttMethodIds.AddRange(RealPtttMethodIds);
                PtttMethodIds = PtttMethodIds.Distinct().ToList();
                var skip = 0;
                while (PtttMethodIds.Count - skip > 0)
                {
                    var listIDs = PtttMethodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPtttMethodFilterQuery HisPtttMethodfilter = new HisPtttMethodFilterQuery();
                    HisPtttMethodfilter.IDs = listIDs;
                    var listHisPtttMethodSub = new HisPtttMethodManager().Get(HisPtttMethodfilter);
                    if (listHisPtttMethodSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisPtttMethodSub" + listHisPtttMethodSub.Count);
                    else
                    {
                        ListHisPtttMethod.AddRange(listHisPtttMethodSub);
                    }
                }
            }
        }

        private void GetMediMateReq()
        {
            var listServiceReqIds = ListSereServ.Select(p => p.SERVICE_REQ_ID ?? 0).Distinct().ToList();
            int skip1 = 0;


            while (listServiceReqIds.Count - skip1 > 0)
            {
                var listIds = listServiceReqIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                mediFilter.TDL_SERVICE_REQ_IDs = listIds;
                var medi = new HisExpMestMedicineManager().GetView(mediFilter);
                if (medi != null)
                {
                    listExpMedi.AddRange(medi);
                }

                string query = string.Format("select * from v_his_exp_mest_material where tdl_service_req_id in ({0})", string.Join(",", listIds));
                var mate = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_EXP_MEST_MATERIAL>(query);
                if (mate != null)
                {
                    listExpMate.AddRange(mate);
                }

                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.IDs = listIds;
                var serviceReqSub = new HisServiceReqManager().Get(serviceReqFilter);
                if (serviceReqSub != null)
                {
                    ListServiceReq.AddRange(serviceReqSub);
                }
                skip1 += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
            }
            dicServiceReq = ListServiceReq.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
        }

        private void GetEkip()
        {
            var listEkipIds = ListSereServ.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
            int skip = 0;

            while (listEkipIds.Count - skip > 0)
            {
                var listIds = listEkipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisEkipUserViewFilterQuery ekipFilter = new HisEkipUserViewFilterQuery();
                ekipFilter.EKIP_IDs = listIds;

                var ekips = new HisEkipUserManager().GetView(ekipFilter);
                if (IsNotNullOrEmpty(ekips))
                {
                    ListEkipUser.AddRange(ekips);
                }
            }
        }

        private void GetService()
        {
            listService = new ManagerSql().GetService();
        }

        private void GetServiceHC()
        {
            listServiceHC = listService.Where(o => !string.IsNullOrWhiteSpace(o.SERVICE_NAME) && o.SERVICE_NAME.ToLower().Contains("hội chẩn ca bệnh khó")).ToList();
        }

        private void GetChild()
        {
            listChild = new ManagerSql().GetChild(castFilter) ?? new List<Mrs00662RDO>();
            if (listChild.Count > 0)
            {
                dicChild = listChild.GroupBy(g=>g.CHILD_TREATMENT_ID??0).ToDictionary(p=>p.Key,q=>q.ToList());
            }
        }

        private void GetDebateType()
        {
            ListDebateType = new ManagerSql().GetDebateType() ?? new List<HIS_DEBATE_TYPE>();
        }

        private void GetDebate()
        {
            var listTreatmentIds = ListSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
            int skip = 0;

            while (listTreatmentIds.Count - skip > 0)
            {
                var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisDebateFilterQuery debateFilter = new HisDebateFilterQuery();
                debateFilter.TREATMENT_IDs = listIds;

                var debates = new HisDebateManager().Get(debateFilter);
                if (IsNotNullOrEmpty(debates))
                {
                    ListDebateNew.AddRange(debates);
                }
            }
            if (ListDebateNew.Count > 0)
            {
                dicDebateNew = ListDebateNew.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }


        private void FilterByDepaRoomAndHasEkip()
        {
            if (IsNotNullOrEmpty(castFilter.EXECUTE_DEPARTMENT_IDs))
            {
                ListSereServ = ListSereServ.Where(o => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
            }

            if (IsNotNullOrEmpty(castFilter.REQUEST_DEPARTMENT_IDs))
            {
                ListSereServ = ListSereServ.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
            }

            if (IsNotNullOrEmpty(castFilter.EXECUTE_ROOM_IDs))
            {
                ListSereServ = ListSereServ.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.TDL_EXECUTE_ROOM_ID)).ToList();
            }

            if (IsNotNullOrEmpty(castFilter.REQUEST_ROOM_IDs))
            {
                ListSereServ = ListSereServ.Where(o => castFilter.REQUEST_ROOM_IDs.Contains(o.TDL_REQUEST_ROOM_ID)).ToList();
            }

            if (castFilter.HAS_EKIP == true)
            {
                ListSereServ = ListSereServ.Where(o => o.EKIP_ID > 0).ToList();
            }
          
        }

        private void GetPatientClassify()
        {
            listPatientClassify = new ManagerSql().GetPatientClassify();
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

                if (castFilter.IS_PTTM == true)
                {
                    GroupPTTM(ListRdoPTTM);
                }
                else
                {
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        Dictionary<long, List<V_HIS_EKIP_USER>> dicEkip = new Dictionary<long, List<V_HIS_EKIP_USER>>();
                        Dictionary<long, V_HIS_PATIENT> dicPatient = ListPatient.ToDictionary(o => o.ID, o => o);


                        if (IsNotNullOrEmpty(ListEkipUser))
                        {
                            foreach (var item in ListEkipUser)
                            {
                                if (!dicEkip.ContainsKey(item.EKIP_ID))
                                    dicEkip[item.EKIP_ID] = new List<V_HIS_EKIP_USER>();

                                dicEkip[item.EKIP_ID].Add(item);
                            }

                            var listGroup = ListEkipUser.GroupBy(o => o.EXECUTE_ROLE_ID).ToList();
                            foreach (var item in listGroup)
                            {
                                LisExecuteRole.Add(new HIS_EXECUTE_ROLE()
                                {
                                    ID = item.First().EXECUTE_ROLE_ID,
                                    EXECUTE_ROLE_CODE = item.First().EXECUTE_ROLE_CODE,
                                    EXECUTE_ROLE_NAME = item.First().EXECUTE_ROLE_NAME
                                });
                            }
                        }


                        ListRdo = new List<Mrs00662RDO>();
                        foreach (var sereServ in ListSereServ)
                        {
                            var child = dicChild.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0) ? dicChild[sereServ.TDL_TREATMENT_ID ?? 0].FirstOrDefault(p => p.TDL_SERVICE_REQ_ID == sereServ.SERVICE_REQ_ID && p.CHILD_TREATMENT_ID == sereServ.TDL_TREATMENT_ID) : null;
                            var debate = dicDebateNew.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0) ? dicDebateNew[sereServ.TDL_TREATMENT_ID ?? 0].FirstOrDefault(p => p.DEPARTMENT_ID == sereServ.TDL_EXECUTE_DEPARTMENT_ID) : null;
                            var serviceReq = dicServiceReq.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0) ? dicServiceReq[sereServ.TDL_TREATMENT_ID ?? 0].FirstOrDefault(p => p.ID == sereServ.TDL_SERVICE_REQ_ID) : null;

                            Mrs00662RDO rdo = new Mrs00662RDO();
                            rdo.PATIENT = new V_HIS_PATIENT();

                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00662RDO>(rdo, sereServ);

                            rdo.PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

                            rdo.TDL_PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                            rdo.TDL_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

                            rdo.TDL_TREATMENT_TYPE_CODE = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == sereServ.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_CODE;
                            rdo.TDL_TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == sereServ.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;

                            
                            if (debate != null && debate.DEBATE_TYPE_ID != null && debate.DEBATE_TYPE_ID > 0)
                            {
                                var debateType = ListDebateType.FirstOrDefault(p => p.ID == debate.DEBATE_TYPE_ID);
                                if (debateType != null)
                                {
                                    rdo.DEBATE_TYPE_CODE = debateType.DEBATE_TYPE_CODE;
                                    rdo.DEBATE_TYPE_NAME = debateType.DEBATE_TYPE_NAME;
                                }
                            }

                            if (serviceReq != null)
                            {
                                rdo.SERVICE_REQ_STT_ID = serviceReq.SERVICE_REQ_STT_ID;
                                rdo.EXECUTE_LOGINNAME = serviceReq.EXECUTE_LOGINNAME;
                                rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                            }
                            rdo.NOTE = !string.IsNullOrEmpty(sereServ.NOTE) ? sereServ.NOTE : "NONE";
                            rdo.NOTICE = !string.IsNullOrEmpty(sereServ.NOTICE) ? sereServ.NOTICE : "NONE";
                            rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.TDL_INTRUCTION_TIME);
                            rdo.PTTT_PRIORITY_NAME = (HisPtttPriorityCFG.PTTT_PRIORITYs.FirstOrDefault(o => o.ID == sereServ.PTTT_PRIORITY_ID) ?? new HIS_PTTT_PRIORITY()).PTTT_PRIORITY_NAME;
                            rdo.PTTT_PRIORITY_CODE = (HisPtttPriorityCFG.PTTT_PRIORITYs.FirstOrDefault(o => o.ID == sereServ.PTTT_PRIORITY_ID) ?? new HIS_PTTT_PRIORITY()).PTTT_PRIORITY_CODE;
                            rdo.IS_CC = sereServ.PTTT_PRIORITY_ID == HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC ? (short)1 : (short)0;
                            rdo.PTTT_TABLE_NAME = (HisPtttTableCFG.PTTT_TABLEs.FirstOrDefault(o => o.ID == sereServ.PTTT_TABLE_ID) ?? new HIS_PTTT_TABLE()).PTTT_TABLE_NAME;
                            rdo.EMOTIONLESS_RESULT_NAME = (HisEmotionlessResultCFG.EMOTIONLESS_RESULTs.FirstOrDefault(o => o.ID == sereServ.EMOTIONLESS_RESULT_ID) ?? new HIS_EMOTIONLESS_RESULT()).EMOTIONLESS_RESULT_NAME;
                            rdo.SURG_PPVC_2 = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServ.EMOTIONLESS_METHOD_SECOND_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;
                            rdo.MISU_PPPT = (ListHisPtttMethod.FirstOrDefault(o => o.ID == sereServ.PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                            rdo.REAL_MISU_PPPT = (ListHisPtttMethod.FirstOrDefault(o => o.ID == sereServ.REAL_PTTT_METHOD_ID) ?? new HIS_PTTT_METHOD()).PTTT_METHOD_NAME;
                            rdo.MISU_PPVC = (HisEmotionlessMethodCFG.EMOTIONLESS_METHODs.FirstOrDefault(o => o.ID == sereServ.EMOTIONLESS_METHOD_ID) ?? new HIS_EMOTIONLESS_METHOD()).EMOTIONLESS_METHOD_NAME;
                            rdo.BEFORE_MISU = sereServ.BEFORE_PTTT_ICD_NAME;
                            rdo.AFTER_MISU = sereServ.AFTER_PTTT_ICD_NAME;
                            rdo.MANNER = sereServ.MANNER;
                            rdo.MISU_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServ.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                            string code = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServ.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_CODE;
                            if (!string.IsNullOrEmpty(code))
                            {
                                if (code.Contains("1"))
                                {
                                    rdo.TYPE_I = "X";
                                }
                                else if (code.Contains("2"))
                                {
                                    rdo.TYPE_II = "X";
                                }
                                else if (code.Contains("3"))
                                {
                                    rdo.TYPE_III = "X";
                                }
                                else
                                {
                                    rdo.TYPE_DB = "X";
                                }
                            }
                            if (castFilter.INPUT_DATA_ID_FEE_TYPE != null)
                            {
                                FilterFeeType(rdo, castFilter.INPUT_DATA_ID_FEE_TYPE ?? 0);
                            }
                            rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                            rdo.DEFAULT_MISU_TYPE_NAME = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServ.SV_PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                            rdo.MISU_TYPE_CODE = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServ.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_CODE;
                            rdo.DEFAULT_MISU_TYPE_CODE = (HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == sereServ.SV_PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_CODE;

                            rdo.TDL_EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.TDL_EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                            rdo.TDL_EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                            rdo.TDL_EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                            rdo.TDL_REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            rdo.TDL_REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                            rdo.TDL_REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                            rdo.TDL_REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                            var patientClassify = listPatientClassify.Where(x => x.ID == sereServ.TDL_PATIENT_CLASSIFY_ID).FirstOrDefault();
                            if (patientClassify != null)
                            {
                                rdo.PATIENT_CLASSIFY_NAME = patientClassify.PATIENT_CLASSIFY_NAME;
                            }
                            IsBhyt(rdo);

                            //int? tuoi = RDOCommon.CalculateAge(sereServ.TDL_PATIENT_DOB);
                            int? tuoi = Age(sereServ.TDL_PATIENT_DOB);
                            if (tuoi >= 0)
                            {
                                if (sereServ.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                                {
                                    rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                                    rdo.MALE_BIRTH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServ.TDL_PATIENT_DOB);
                                }
                                else
                                {
                                    rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                                    rdo.FEMALE_BIRTH = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServ.TDL_PATIENT_DOB);
                                }
                            }

                            if (sereServ.END_TIME.HasValue)
                            {
                                rdo.END_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.END_TIME.Value);
                                rdo.END_DATE = (sereServ.END_TIME - (sereServ.END_TIME % 1000000));
                                rdo.END_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.END_DATE ?? 0);
                            }
                            rdo.VIR_TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE;
                            if (sereServ.BEGIN_TIME.HasValue)
                            {
                                rdo.BEGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.BEGIN_TIME.Value);
                            }

                            if (sereServ.EKIP_ID.HasValue)
                            {
                                rdo.MAIN_EXECUTE = string.Join(",", ListEkipUser.Where(p => p.EKIP_ID == sereServ.EKIP_ID && (p.EXECUTE_ROLE_ID == HisExecuteRoleId__Main || p.EXECUTE_ROLE_ID == HisExecuteRoleId__Anesthetist)).Select(p => p.USERNAME));
                                rdo.EXTRA_EXECUTE = string.Join(",", ListEkipUser.Where(p => p.EKIP_ID == sereServ.EKIP_ID && (p.EXECUTE_ROLE_ID == HisExecuteRoleId__PM1 || p.EXECUTE_ROLE_ID == HisExecuteRoleId__PM2 || p.EXECUTE_ROLE_ID == HisExecuteRoleId__PMe1 || p.EXECUTE_ROLE_ID == HisExecuteRoleId__PMe2)).Select(p => p.USERNAME));
                                rdo.HELPING = string.Join(",", ListEkipUser.Where(p => p.EKIP_ID == sereServ.EKIP_ID && (p.EXECUTE_ROLE_ID == HisExecuteRoleId__YTDD || p.EXECUTE_ROLE_ID == HisExecuteRoleId__DCVPTTT)).Select(p => p.USERNAME));
                            }

                            if (dicEkip.ContainsKey(sereServ.EKIP_ID ?? 0))
                            {
                                rdo.DICR_EXECUTE_USERNAME = dicEkip[sereServ.EKIP_ID ?? 0].GroupBy(o => o.EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join(" ", q.Select(o => o.USERNAME).ToList()));
                                rdo.DICR_EXECUTE_FIRSTNAME = dicEkip[sereServ.EKIP_ID ?? 0].GroupBy(o => o.EXECUTE_ROLE_CODE).ToDictionary(q => q.Key, q => string.Join(" ", q.Select(o => o.USERNAME.Split(' ').Last()).ToList()));

                                var ekipMain = dicEkip[sereServ.EKIP_ID ?? 0].FirstOrDefault(o => o.EXECUTE_ROLE_ID == HisExecuteRoleId__Main);
                                if (IsNotNull(ekipMain))
                                {
                                    rdo.EXECUTE_DOCTOR = ekipMain.USERNAME;
                                }

                                var ekipAnes = dicEkip[sereServ.EKIP_ID ?? 0].FirstOrDefault(o => o.EXECUTE_ROLE_ID == HisExecuteRoleId__Anesthetist);
                                if (IsNotNull(ekipAnes))
                                {
                                    rdo.ANESTHESIA_DOCTOR = ekipAnes.USERNAME;
                                }
                            }

                            if (dicPatient != null && dicPatient.ContainsKey(sereServ.TDL_PATIENT_ID ?? 0))
                            {
                                rdo.PATIENT = dicPatient[sereServ.TDL_PATIENT_ID ?? 0];
                            }

                            rdo.PTTT_CATASTROPHE_NAME = (ListPtttCatastrophe.FirstOrDefault(o => o.ID == sereServ.PTTT_CATASTROPHE_ID) ?? new HIS_PTTT_CATASTROPHE()).PTTT_CATASTROPHE_NAME;
                            rdo.PTTT_CONDITION_NAME = (ListPtttCondition.FirstOrDefault(o => o.ID == sereServ.PTTT_CONDITION_ID) ?? new HIS_PTTT_CONDITION()).PTTT_CONDITION_NAME;
                            rdo.DEATH_WITHIN_NAME = (ListDeathWithin.FirstOrDefault(o => o.ID == sereServ.DEATH_WITHIN_ID) ?? new HIS_DEATH_WITHIN()).DEATH_WITHIN_NAME;

                            rdo.PTTT_CATASTROPHE_ID =sereServ.PTTT_CATASTROPHE_ID;
                            rdo.PTTT_CONDITION_ID = sereServ.PTTT_CONDITION_ID;
                            rdo.DEATH_WITHIN_ID = sereServ.DEATH_WITHIN_ID;

                            if (!rdo.EKIP_ID.HasValue)
                            {
                                rdo.EKIP_ID = 0;
                            }
                            rdo.PRIMARY_PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PRIMARY_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                            rdo.PRIMARY_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PRIMARY_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                            if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                            {
                                rdo.IS_PT = "X";
                                rdo.PT_TT_NAME = "Phẫu thuật";
                            }
                            if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                            {
                                rdo.PT_TT_NAME = "Thủ thuật";
                                rdo.IS_TT = "X";
                            }

                            rdo.ORIGINAL_PRICE = sereServ.ORIGINAL_PRICE;

                            if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                            {
                                if (sereServ.PARENT_ID != null)
                                {
                                    rdo.IS_MAIN_PTTT = "PTTT phụ";
                                }
                                else
                                {
                                    rdo.IS_MAIN_PTTT = "PTTT chính";
                                }
                            }

                            if (child != null)
                            {
                                rdo.MEDICINE_CLIPPING = child.MEDICINE_DK_PRICE;
                                rdo.MEDICINE_HPKP = child.MEDICINE_HP_PRICE;

                                rdo.MATERIAL_CLIPPING = child.MATERIAL_DK_PRICE;
                                rdo.MATERIAL_HPKP = child.MATERIAL_HP_PRICE;
                            }
                            //rdo.ICD_CODE = sereServ.ICD_CODE;
                            //rdo.ICD_SUB_CODE = sereServ.ICD_SUB_CODE;
                            //rdo.ICD_NAME = sereServ.ICD_NAME;
                            //rdo.ICD_TEXT = sereServ.ICD_TEXT;
                            ListRdo.Add(rdo);
                        }

                        ListRdoDepartment = ListRdo.GroupBy(o => o.TDL_EXECUTE_DEPARTMENT_ID).Select(s => s.First()).ToList();
                        ListRdoDate = ListRdo.GroupBy(o => o.END_DATE).Select(s => s.First()).ToList();
                        ListTypeDepa = ListRdo.GroupBy(o => new { o.TDL_SERVICE_TYPE_ID, o.TDL_REQUEST_DEPARTMENT_ID }).Select(p => new Mrs00662RDO()
                        {
                            TDL_SERVICE_TYPE_ID = p.First().TDL_SERVICE_TYPE_ID,
                            SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME,
                            TDL_REQUEST_DEPARTMENT_ID = p.First().TDL_REQUEST_DEPARTMENT_ID,
                            TDL_REQUEST_DEPARTMENT_NAME = p.First().TDL_REQUEST_DEPARTMENT_NAME,
                            JSON_PTTT_PRIORITY_AMOUNT = JsonConvert.SerializeObject(p.GroupBy(g => g.PTTT_PRIORITY_ID).ToDictionary(q => q.Key ?? 0, q => q.Sum(s => s.AMOUNT))),

                            JSON_PTTT_CATASTROPHE_AMOUNT = JsonConvert.SerializeObject(p.GroupBy(g => g.PTTT_CATASTROPHE_ID).ToDictionary(q => q.Key ?? 0, q => q.Sum(s => s.AMOUNT))),

                            JSON_DEATH_WITHIN_AMOUNT = JsonConvert.SerializeObject(p.GroupBy(g => g.DEATH_WITHIN_ID).ToDictionary(q => q.Key ?? 0, q => q.Sum(s => s.AMOUNT))),
                            AMOUNT = p.Sum(s => s.AMOUNT)
                        }).ToList();
                    }
                }

                if (castFilter.INPUT_DATA_ID_FEE_TYPE != null)
                {
                    ListRdo = ListRdo.Where(o => o.FEE_TYPE == castFilter.INPUT_DATA_ID_FEE_TYPE).ToList();
                }
            }
            
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private void IsBhyt(Mrs00662RDO rdo)
        {
            try
            {
                if (rdo.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private List<Mrs00662RDOPTTM> GroupPTTM(List<Mrs00662RDOPTTM> listRdo)
        {
            List<Mrs00662RDOPTTM> result = new List<Mrs00662RDOPTTM>();
            try
            {
                foreach (var sereServ in listRdo)
                {
                    Mrs00662RDOPTTM rdo = new Mrs00662RDOPTTM();
                    var service = listService.FirstOrDefault(p => p.ID == sereServ.SERVICE_ID) ?? new HIS_SERVICE();
                    var serviceReq = dicServiceReq.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0) ? dicServiceReq[sereServ.TDL_TREATMENT_ID ?? 0].FirstOrDefault(p => p.ID == sereServ.SERVICE_REQ_ID) : null;

                    rdo.TDL_SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                    rdo.SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID == sereServ.TDL_SERVICE_TYPE_ID)?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                    rdo.TDL_EXECUTE_DEPARTMENT_ID = sereServ.TDL_EXECUTE_DEPARTMENT_ID;
                    var serviceHC = listServiceHC.FirstOrDefault(p => p.ID == sereServ.SERVICE_ID) ?? new HIS_SERVICE();
                    rdo.TDL_PATIENT_NAME = sereServ.TDL_PATIENT_NAME;
                    rdo.TDL_TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                    rdo.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                    rdo.EXECUTE_LOGINNAME = serviceReq.EXECUTE_LOGINNAME;
                                
                    
                    rdo.TDL_PATIENT_WORK_PLACE = sereServ.TDL_PATIENT_WORK_PLACE;
                    if (!string.IsNullOrEmpty(sereServ.NOTE))
                    {
                        
                        rdo.NOTE = sereServ.NOTE;
                    }
                    ProcessRdoByServiceType(rdo, sereServ);
                    ProcessRdoByHeinServiceType(rdo, sereServ);
                    rdo.HOICHAN_PRICE = rdo.HOICHAN_PRICE_KH + rdo.HOICHAN_PRICE_KHAC;
                    if (sereServ.IS_EXPEND != null)
                    {
                        rdo.DEPARTMENT_EXPEND_PRICE = sereServ.VIR_TOTAL_PRICE - sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
                        {
                            rdo.CDHA_EXPEND_PRICE = sereServ.VIR_TOTAL_PRICE - sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        }
                        if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                        {
                            rdo.XN_EXPEND_PRICE = sereServ.VIR_TOTAL_PRICE - sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        }
                        else
                        {
                            rdo.KHAC_EXPEND_PRICE = sereServ.VIR_TOTAL_PRICE - sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        }
                    }
                    rdo.VIR_TOTAL_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) , 4) * sereServ.AMOUNT;
                    ListRdoOutside.Add(rdo);
                    result.Add(rdo);
                }
                var group = result.GroupBy(x => x.TDL_TREATMENT_CODE).ToList();
                ListRdoPTTM.Clear();
                //var group = ListRdo.GroupBy(x => x.TDL_TREATMENT_CODE).ToList();
                foreach (var item in group)
                {
                    if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                    {
                        if (item.FirstOrDefault(o => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)) == null)
                        {
                            continue;
                        };
                    }
                    Mrs00662RDOPTTM rdo = new Mrs00662RDOPTTM();
                    rdo.TDL_TREATMENT_CODE = item.First().TDL_TREATMENT_CODE;
                    rdo.TDL_PATIENT_NAME = item.First().TDL_PATIENT_NAME;
                    rdo.EXECUTE_LOGINNAME = item.First().EXECUTE_LOGINNAME;
                    rdo.REQUEST_LOGINNAME = item.First().REQUEST_LOGINNAME;         
                                
                    
                    rdo.NOTE = item.First().NOTE;
                    rdo.EXAM_PRICE = item.Sum(x => x.EXAM_PRICE);
                    rdo.BED_PRICE = item.Sum(x => x.BED_PRICE);
                    rdo.MEDICINE_PRICE = item.Sum(x => x.MEDICINE_PRICE);
                    rdo.MATERIAL_PRICE = item.Sum(x => x.MATERIAL_PRICE);
                    rdo.TEST_PRICE = item.Sum(x => x.TEST_PRICE);
                    rdo.TEST_CV_PRICE = item.Sum(x => x.TEST_CV_PRICE);
                    rdo.MAIN_PT_PRICE = item.Sum(x => x.MAIN_PT_PRICE);
                    rdo.PTYC_PRICE = item.Sum(x => x.PTYC_PRICE);
                    rdo.DIIM_PRICE = item.Sum(x => x.DIIM_PRICE);

                    rdo.EXAM_HEIN_PRICE = item.Sum(x => x.EXAM_HEIN_PRICE);
                    rdo.BED_HEIN_PRICE = item.Sum(x => x.BED_HEIN_PRICE);
                    rdo.MEDICINE_HEIN_PRICE = item.Sum(x => x.MEDICINE_HEIN_PRICE);
                    rdo.MATERIAL_HEIN_PRICE = item.Sum(x => x.MATERIAL_HEIN_PRICE);
                    rdo.TEST_HEIN_PRICE = item.Sum(x => x.TEST_HEIN_PRICE);
                    rdo.TEST_CV_HEIN_PRICE = item.Sum(x => x.TEST_CV_HEIN_PRICE);
                    rdo.MAIN_PT_HEIN_PRICE = item.Sum(x => x.MAIN_PT_HEIN_PRICE);
                    rdo.PTYC_HEIN_PRICE = item.Sum(x => x.PTYC_HEIN_PRICE);
                    rdo.DIIM_HEIN_PRICE = item.Sum(x => x.DIIM_HEIN_PRICE);

                    rdo.DEPARTMENT_EXPEND_PRICE = item.Sum(x => x.DEPARTMENT_EXPEND_PRICE);
                    rdo.CDHA_EXPEND_PRICE = item.Sum(x => x.CDHA_EXPEND_PRICE);
                    rdo.XN_EXPEND_PRICE = item.Sum(x => x.XN_EXPEND_PRICE);
                    rdo.KHAC_EXPEND_PRICE = item.Sum(x => x.KHAC_EXPEND_PRICE);
                    rdo.HOICHAN_PRICE_KH = item.Sum(x => x.HOICHAN_PRICE_KH);
                    rdo.HOICHAN_PRICE_KHAC = item.Sum(x => x.HOICHAN_PRICE_KHAC);
                    rdo.HOICHAN_PRICE = item.Sum(x => x.HOICHAN_PRICE);
                    rdo.OTHER_PRICE = item.Sum(x => x.OTHER_PRICE);
                    rdo.DEPARTMENT_EXPEND_PRICE = item.Sum(x => x.DEPARTMENT_EXPEND_PRICE);
                    rdo.EXAM_PRICE1 = item.Sum(x => x.EXAM_PRICE1);
                    rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                    rdo.PTYC_PRICE1 = item.Sum(x => x.PTYC_PRICE1);

                    rdo.EXAM_HEIN_PRICE1 = item.Sum(x => x.EXAM_HEIN_PRICE1);
                    
                    rdo.PTYC_HEIN_PRICE1 = item.Sum(x => x.PTYC_HEIN_PRICE1);
                    rdo.DIC_SERVICE_CODE = item.GroupBy(o => o.TDL_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.VIR_TOTAL_PRICE ?? 0));
                    
                    rdo.DIC_SVT_CODE = item.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.VIR_TOTAL_PRICE ?? 0));
                    rdo.DIC_HEIN_SVT_CODE = item.GroupBy(o => o.TDL_HEIN_SERVICE_BHYT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.VIR_TOTAL_PRICE ?? 0));
                    rdo.VIR_TOTAL_PRICE = item.Sum(x => x.VIR_TOTAL_PRICE ?? 0);
                    ListRdoPTTM.Add(rdo);
                }

                //var group1 = result.GroupBy(x => new {x.TDL_TREATMENT_CODE, x.})
            }
            catch (Exception ex)
            {
                ListRdoPTTM.Clear();
                LogSystem.Error(ex);
            }
            return ListRdoPTTM;

        }

        private void ProcessRdoByServiceType(Mrs00662RDOPTTM rdo, Mrs00662RDOPTTM sereServ)
        {
            if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
            {
                if (castFilter.SERVICE_CODE__HCs.Contains(sereServ.TDL_SERVICE_CODE))
                {
                    rdo.HOICHAN_PRICE_KH = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                else
                {
                    rdo.EXAM_PRICE1 = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                rdo.EXAM_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
            {
                rdo.BED_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
            {
                rdo.MEDICINE_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
            {
                rdo.MATERIAL_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && sereServ.TEST_COVID_TYPE == null)
            {
                rdo.TEST_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && sereServ.TEST_COVID_TYPE != null)
            {
                rdo.TEST_CV_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
            {
                rdo.DIIM_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
            {
                rdo.BLOOD_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.SERVICE_REQ_ID == null && (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT))
            {
                rdo.MAIN_PT_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            if (sereServ.SERVICE_REQ_ID != null && (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT))
            {
                if (castFilter.SERVICE_CODE__PTYCs.Contains(sereServ.TDL_SERVICE_CODE))
                {
                    rdo.PTYC_PRICE1 = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                rdo.PTYC_PRICE = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
        }

        private void ProcessRdoByHeinServiceType(Mrs00662RDOPTTM rdo, Mrs00662RDOPTTM sereServ)
        {
            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
            {
                if (castFilter.SERVICE_CODE__HCs.Contains(sereServ.TDL_SERVICE_CODE))
                {
                    rdo.HOICHAN_HEIN_PRICE_KH = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                else
                {
                    rdo.EXAM_HEIN_PRICE1 = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                rdo.EXAM_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT)
            {
                rdo.BED_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
            {
                rdo.MEDICINE_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
            {
                rdo.MATERIAL_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN && sereServ.TEST_COVID_TYPE == null)
            {
                rdo.TEST_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN && sereServ.TEST_COVID_TYPE != null)
            {
                rdo.TEST_CV_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
            {
                rdo.DIIM_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            
            else if (sereServ.SERVICE_REQ_ID == null && (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT))
            {
                rdo.MAIN_PT_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                if (castFilter.SERVICE_CODE__PTYCs.Contains(sereServ.TDL_SERVICE_CODE))
                {
                    rdo.PTYC_HEIN_PRICE1 = Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
                }
                rdo.PTYC_HEIN_PRICE =Math.Round((sereServ.VIR_PRICE ?? 0) * (1 + sereServ.VAT_RATIO), 4) * sereServ.AMOUNT;
            }
            
        }
        private void FilterFeeType(Mrs00662RDO rdo, short FeeType)
        {
            if (FeeType == 1)//chọn loại chi phí bảo hiểm
            {
                if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.FEE_TYPE = FeeType;
                    if (rdo.PRIMARY_PATIENT_TYPE_ID != null)
                    {
                        rdo.VIR_TOTAL_PATIENT_PRICE = (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.VIR_TOTAL_PRICE = (rdo.VIR_TOTAL_HEIN_PRICE ?? 0) + (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    }
                }
            }
            if (FeeType == 2)//chọn loại chi phí tự trả
            {
                rdo.FEE_TYPE = FeeType;
                if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    if (rdo.PRIMARY_PATIENT_TYPE_ID != null)
                    {
                        rdo.VIR_TOTAL_PATIENT_PRICE = (rdo.VIR_TOTAL_PATIENT_PRICE ?? 0) - (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.VIR_TOTAL_PRICE = (rdo.VIR_TOTAL_PRICE ?? 0) - (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) - (rdo.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.VIR_TOTAL_HEIN_PRICE = 0;
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = 0;
                    }
                    else
                    {
                        rdo.FEE_TYPE = null;
                    }
                }
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                dicSingleTag.Add("MONTH", castFilter.TIME_TO > 0 ? (castFilter.TIME_TO ?? 0).ToString().Substring(4, 2) : "");
                if (castFilter.IS_PTTM != null)
                {
                    if (castFilter.IS_PTTM == true)
                    {
                        ListRdo.Distinct().ToList();
                        ListRdo = ListRdo.Where(x => x.PATIENT_CLASSIFY_NAME != "BN phẫu thuật thẩm mỹ").ToList();
                    }
                }

                if (castFilter.IS_PT_TT.HasValue)
                {
                    if (castFilter.IS_PT_TT.Value == 0)
                    {

                        dicSingleTag.Add("TITLE_PT_TT", "THỦ THUẬT");
                    }
                    else if (castFilter.IS_PT_TT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_PT_TT", "PHẪU THUẬT");
                    }
                }
                else
                {
                    dicSingleTag.Add("TITLE_PT_TT", "PHẪU THUẬT THỦ THUẬT");
                }

                if (castFilter.IS_NT_NGT.HasValue)
                {
                    if (castFilter.IS_NT_NGT.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NGOẠI TRÚ");
                    }
                    else if (castFilter.IS_NT_NGT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NỘI TRÚ");
                    }
                }

                if (castFilter.IS_NT_NGT_0.HasValue)
                {
                    if (castFilter.IS_NT_NGT_0.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT_0", "NGOẠI TRÚ");
                    }
                    else if (castFilter.IS_NT_NGT_0.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT_0", "NỘI TRÚ");
                    }
                }

                ListRdo = ListRdo.OrderBy(o => o.TDL_TREATMENT_CODE).ToList();
                ListRdoDate = ListRdoDate.OrderBy(o => o.END_DATE).ToList();
                objectTag.AddObjectData(store, "ReportPTTM", ListRdoPTTM);
                objectTag.AddObjectData(store, "Debates", ListDebateNew);
                objectTag.AddObjectData(store, "ReportPT", ListRdo.Where(x => x.PT_TT_NAME == "Phẫu thuật").ToList());
                objectTag.AddObjectData(store, "ReportTT", ListRdo.Where(x => x.PT_TT_NAME == "Thủ thuật").ToList());
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Department", ListRdoDepartment);
                objectTag.AddRelationship(store, "Department", "Report", "TDL_EXECUTE_DEPARTMENT_ID", "TDL_EXECUTE_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "DateGroup", ListRdoDate);
                objectTag.AddRelationship(store, "DateGroup", "Report", "END_DATE", "END_DATE");
                objectTag.SetUserFunction(store, "Element", new RDOElement());
                objectTag.SetUserFunction(store, "FuncSameTitleColTreatmentCode", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColPatientCode", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColPatientName", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColAddress", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColAgeMale", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColAgeFermale", new RDOCustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleColHeinCard", new RDOCustomerFuncMergeSameData());

                objectTag.AddObjectData(store, "ExecuteRole", LisExecuteRole);
                objectTag.AddObjectData(store, "EkipUser", ListEkipUser);
                objectTag.AddObjectData(store, "Pttt", ListRdo.OrderBy(q => q.DEFAULT_MISU_TYPE_NAME ?? "z").ToList());
                objectTag.AddObjectData(store, "PtttGroup", ListRdo.GroupBy(o => o.SV_PTTT_GROUP_ID).Select(p => p.First()).OrderBy(q => q.DEFAULT_MISU_TYPE_NAME ?? "z").ToList());
                objectTag.AddRelationship(store, "PtttGroup", "Report", "SV_PTTT_GROUP_ID", "SV_PTTT_GROUP_ID");
                objectTag.AddObjectData(store, "PtttGroupInDepartment", ListRdo.GroupBy(o => new { o.TDL_EXECUTE_DEPARTMENT_ID, o.SV_PTTT_GROUP_ID }).Select(p => p.First()).OrderBy(r => r.TDL_EXECUTE_DEPARTMENT_NAME).ThenBy(q => q.DEFAULT_MISU_TYPE_NAME ?? "z").ToList());
                objectTag.AddRelationship(store, "PtttGroupInDepartment", "Report", new string[] { "TDL_EXECUTE_DEPARTMENT_ID", "SV_PTTT_GROUP_ID" }, new string[] { "TDL_EXECUTE_DEPARTMENT_ID", "SV_PTTT_GROUP_ID" });
                objectTag.AddRelationship(store, "Department", "PtttGroupInDepartment", "TDL_EXECUTE_DEPARTMENT_ID", "TDL_EXECUTE_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "Catastrophe", ListPtttCatastrophe);
                objectTag.AddObjectData(store, "DeathWithin", ListDeathWithin);
                objectTag.AddObjectData(store, "Priority", HisPtttPriorityCFG.PTTT_PRIORITYs);
                objectTag.AddObjectData(store, "TypeDepa", ListTypeDepa);

                objectTag.AddObjectData(store, "Outside", ListRdoOutside.Where(p => !string.IsNullOrEmpty(p.NOTE)).ToList());

                objectTag.AddObjectData(store, "HoiChan", ListRdo.Where(p => !string.IsNullOrEmpty(p.NOTE) && !string.IsNullOrEmpty(p.NOTICE) && p.NOTICE.Contains("Bác sỹ:")).ToList());

                if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    List<HIS_DEPARTMENT> department = new List<HIS_DEPARTMENT>();
                    HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                    departmentFilter.IDs = castFilter.EXECUTE_DEPARTMENT_IDs;
                    department = new HisDepartmentManager().Get(departmentFilter);
                    dicSingleTag.Add("EXECUTE_DEPARTMET_NAME", string.Join(",", department.Select(x => x.DEPARTMENT_NAME).ToList()));
                }
                else
                {
                    dicSingleTag.Add("EXECUTE_DEPARTMET_NAME", "Tất cả");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public int Age(long dob)
        {
            int result = -1;
            try
            {
                System.DateTime? sysDob = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(dob);
                if (sysDob != null)
                {
                    System.DateTime today = System.DateTime.Today;
                    result = today.Year - sysDob.Value.Year;
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return result;
        }

        public class RDOElement : FlexCel.Report.TFlexCelUserFunction
        {
            object result = null;
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length < 2)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                try
                {
                    string listKey = Convert.ToString(parameters[1]);
                    if (string.IsNullOrWhiteSpace(listKey))
                    {
                        listKey = "";
                    }
                    string[] arrayKey = listKey.Split(',');
                    if (parameters[0] is Dictionary<string, int>)
                    {
                        Dictionary<string, int> DicGet = parameters[0] as Dictionary<string, int>;
                        result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                    }
                    else if (parameters[0] is Dictionary<string, long>)
                    {
                        Dictionary<string, long> DicGet = parameters[0] as Dictionary<string, long>;
                        result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                    }
                    else if (parameters[0] is Dictionary<string, decimal>)
                    {
                        Dictionary<string, decimal> DicGet = parameters[0] as Dictionary<string, decimal>;
                        result = DicGet.Where(o => arrayKey.Contains(o.Key)).Sum(p => p.Value);
                    }
                    else if (parameters[0] is Dictionary<string, string>)
                    {
                        Dictionary<string, string> DicGet = parameters[0] as Dictionary<string, string>;
                        result = string.Join(";", DicGet.Where(o => arrayKey.Contains(o.Key)).Select(p => p.Value).ToList());
                    }
                    else
                    {
                        result = null;
                    }

                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                    return null;
                }

                return result;
            }
        }

        public long HisExecuteRoleId__Main = HisExecuteRoleCFG.HisExecuteRoleId__Main;

        public long HisExecuteRoleId__PM1 = HisExecuteRoleCFG.HisExecuteRoleId__PM1;

        public long HisExecuteRoleId__PM2 = HisExecuteRoleCFG.HisExecuteRoleId__PM2;

        public long HisExecuteRoleId__Anesthetist = HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist;

        public long HisExecuteRoleId__PMe1 = HisExecuteRoleCFG.HisExecuteRoleId__PMe1;

        public long HisExecuteRoleId__PMe2 = HisExecuteRoleCFG.HisExecuteRoleId__PMe2;

        public long HisExecuteRoleId__YTDD = HisExecuteRoleCFG.HisExecuteRoleId__YTDD;

        public long HisExecuteRoleId__DCVPTTT = HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT;
    }
}
