using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MRS.Processor.Mrs00250;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
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

using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using ACS.EFMODEL.DataModels;
using ACS.Filter;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00250
{
    public class Mrs00250Processor : AbstractProcessor
    {
        Mrs00250Filter castFilter = null;
        private List<Mrs00250RDO> ListRdo = new List<Mrs00250RDO>();
        private List<Mrs00250RDO> ListRdoDetail = new List<Mrs00250RDO>();
        private Dictionary<long, List<HIS_SERVICE_REQ>> dicPrescription = new Dictionary<long, List<HIS_SERVICE_REQ>>();
        private Dictionary<long, List<HIS_EXP_MEST>> dicExpMest = new Dictionary<long, List<HIS_EXP_MEST>>();
        private Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        Dictionary<long, List<HIS_EXP_MEST_MATERIAL>> dicExpMestMaterial = new Dictionary<long, List<HIS_EXP_MEST_MATERIAL>>();
        Dictionary<long, List<HIS_EXP_MEST_MEDICINE>> dicExpMestMedicine = new Dictionary<long, List<HIS_EXP_MEST_MEDICINE>>();
        CommonParam paramGet = new CommonParam();
        public Mrs00250Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00250Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            try
            {
                this.castFilter = (Mrs00250Filter)this.reportFilter;

                //get danh sach kham
                if (castFilter.IS_THROW_KSK_OTHER_SOURCE == true)
                {
                    GetServiceReqSql();
                }
                else
                {
                    GetServiceReq();
                }

                if (castFilter.LOGINNAME != null && castFilter.LOGINNAME.Length > 0)
                {
                    ListServiceReq = ListServiceReq.Where(o => o.EXECUTE_LOGINNAME == castFilter.LOGINNAME).ToList();
                }
                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    ListServiceReq = ListServiceReq.Where(o => o.EXECUTE_DEPARTMENT_ID == castFilter.REQUEST_DEPARTMENT_ID).ToList();
                }
                if (castFilter.REQUEST_ROOM_ID != null)
                {
                    ListServiceReq = ListServiceReq.Where(o => o.EXECUTE_ROOM_ID == castFilter.REQUEST_ROOM_ID).ToList();
                }
                if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                {
                    ListServiceReq = ListServiceReq.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.EXECUTE_DEPARTMENT_ID)).ToList();
                }
                if (castFilter.REQUEST_ROOM_IDs != null)
                {
                    ListServiceReq = ListServiceReq.Where(o => castFilter.REQUEST_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();
                }
                if (this.castFilter.BRANCH_ID.HasValue)
                {
                    var departments = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.Where(o => o.BRANCH_ID == this.castFilter.BRANCH_ID.Value).ToList();

                    var departmentIds = departments.Select(o => o.ID).ToList();

                    ListServiceReq = ListServiceReq.Where(o => departmentIds.Contains(o.EXECUTE_DEPARTMENT_ID)).ToList();
                }

                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    var treatmentIds = ListServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    var skip = 0;
                    var listTreatment = new List<HIS_TREATMENT>();
                    var listPrescription = new List<HIS_SERVICE_REQ>();
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var treatmentfilter = new HisTreatmentFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var treatments = new HisTreatmentManager().Get(treatmentfilter);
                        listTreatment.AddRange(treatments);
                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            ORDER_FIELD = "ID",
                            ORDER_DIRECTION = "ASC",
                            HAS_EXECUTE = true,
                            SERVICE_REQ_TYPE_IDs = new List<long>()
                    {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK
                    },

                        };
                        var ListServiceReqSub = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);
                        listPrescription.AddRange(ListServiceReqSub);
                    }
                    //Các chỉ định đơn thuốc
                    var treatmentId = listTreatment.Where(q => q.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(o => o.ID).ToList();
                    var dicParentExe = ListServiceReq.Where(p => treatmentId.Contains(p.TREATMENT_ID)).GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First().EXECUTE_LOGINNAME);
                    listPrescription = listPrescription.Where(o => dicParentExe.ContainsKey(o.PARENT_ID ?? 0) && dicParentExe[o.PARENT_ID ?? 0] == o.REQUEST_LOGINNAME).ToList();
                    if (castFilter.IS_REMOVE_DUPLICATE_PRES == true)
                    {
                        listPrescription = listPrescription.GroupBy(g => g.PARENT_ID ?? 0).Select(o => o.OrderBy(p => p.INTRUCTION_TIME).Last()).ToList();
                    }

                    dicTreatment = listTreatment.GroupBy(g => g.ID).ToDictionary(p => p.Key, q => q.First());
                    dicPrescription = listPrescription.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                }

                var treatmentMediIds = dicPrescription.Keys;
                if (IsNotNullOrEmpty(treatmentMediIds))
                {
                    var skip = 0;
                    var listExpMest = new List<HIS_EXP_MEST>();
                    while (treatmentMediIds.Count - skip > 0)
                    {
                        var listIDs = treatmentMediIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestFilterQuery ExpMestfilter = new HisExpMestFilterQuery()
                        {
                            TDL_TREATMENT_IDs = listIDs,
                            ORDER_FIELD = "ID",
                            ORDER_DIRECTION = "ASC"
                        };
                        var ListExpMestSub = new HisExpMestManager(paramGet).Get(ExpMestfilter);
                        listExpMest.AddRange(ListExpMestSub);
                    }
                    listExpMest = listExpMest.Where(o => dicPrescription.ContainsKey(o.TDL_TREATMENT_ID??0) &&dicPrescription[o.TDL_TREATMENT_ID??0].Exists(p => p.ID == o.SERVICE_REQ_ID || p.ID == o.PRESCRIPTION_ID)).ToList();
                    //danh sach thuoc vat tu tinh tong tien
                    if (IsNotNullOrEmpty(listExpMest))
                    {
                        skip = 0;
                        var listExpMestMaterial = new List<HIS_EXP_MEST_MATERIAL>();
                        var listExpMestMedicine = new List<HIS_EXP_MEST_MEDICINE>();
                        while (listExpMest.Count - skip > 0)
                        {
                            var listIDs = listExpMest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var HisMaterialfilter = new HisExpMestMaterialFilterQuery()
                            {
                                EXP_MEST_IDs = listIDs.Select(s => s.ID).ToList(),
                                IS_EXPORT = true
                            };
                            var listMaterialSub = new HisExpMestMaterialManager(paramGet).Get(HisMaterialfilter);
                            if (IsNotNullOrEmpty(listMaterialSub))
                            {
                                listExpMestMaterial.AddRange(listMaterialSub);
                            }

                            var HisMedicinefilter = new HisExpMestMedicineFilterQuery()
                            {
                                EXP_MEST_IDs = listIDs.Select(s => s.ID).ToList(),
                                IS_EXPORT = true
                            };
                            var listMedicineSub = new HisExpMestMedicineManager(paramGet).Get(HisMedicinefilter);
                            if (IsNotNullOrEmpty(listMedicineSub))
                            {
                                listExpMestMedicine.AddRange(listMedicineSub);
                            }
                        }
                        dicExpMestMedicine = listExpMestMedicine.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());
                        dicExpMestMaterial = listExpMestMaterial.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());

                    }
                    listExpMest = listExpMest.Where(o => dicExpMestMaterial.ContainsKey(o.TDL_TREATMENT_ID ?? 0) && dicExpMestMaterial[o.TDL_TREATMENT_ID ?? 0].Exists(p => p.EXP_MEST_ID == o.ID) || dicExpMestMedicine.ContainsKey(o.TDL_TREATMENT_ID ?? 0) && dicExpMestMedicine[o.TDL_TREATMENT_ID ?? 0].Exists(p => p.EXP_MEST_ID == o.ID)).ToList();
                    dicExpMest = listExpMest.GroupBy(g => g.TDL_TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());
                }

               

            }
            catch (Exception ex)
            {
                LogSystem.Info("????????????????????????????????????????3");
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetServiceReqSql()
        {
            string query = " -- danh sách khám\n";
            query += "select\n";
            query += "sr.*\n";
            query += "from his_service_req sr\n";
            query += "join his_sere_serv ss on ss.service_req_id=sr.id\n";
            query += "left join his_bed_room br on br.room_id=sr.request_room_id\n";
            query += "where 1=1\n";
            query += "and sr.is_delete=0\n";
            query += "and sr.is_no_execute is null\n";
            query += "and sr.service_req_type_id=1\n";
            query += "and sr.service_req_stt_id=3\n";
            query += string.Format("and sr.finish_time between {0} and {1}\n", castFilter.TIME_FROM, castFilter.TIME_TO);
            query += "and br.id is null\n";
            //if (castFilter.LOGINNAME != null && castFilter.LOGINNAME.Length > 0)
            //{
            //    query += string.Format("and sr.execute_loginname = '{0}'\n", castFilter.LOGINNAME);
            //}
            //if (castFilter.REQUEST_DEPARTMENT_ID != null)
            //{
            //    query += string.Format("and sr.execute_department_id = {0}\n", castFilter.REQUEST_DEPARTMENT_ID);
            //}
            //if (castFilter.REQUEST_ROOM_ID != null)
            //{
            //    query += string.Format("and sr.execute_room_id = {0}\n", castFilter.REQUEST_ROOM_ID);
            //}
            //if (this.castFilter.BRANCH_ID.HasValue)
            //{
            //    var departments = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.Where(o => o.BRANCH_ID == this.castFilter.BRANCH_ID.Value).ToList();

            //    var departmentIds = departments.Select(o => o.ID).ToList();

            //    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", departmentIds));
            //}
            query += string.Format("and ss.patient_type_id not in ({0},{1})\n", HisPatientTypeCFG.PATIENT_TYPE_ID__KSK, HisPatientTypeCFG.PATIENT_TYPE_ID__KSKHD);
            query += "and ss.other_pay_source_id is null\n";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            ListServiceReq = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_REQ>(query);


            ListServiceReq = ListServiceReq.OrderBy(o => o.FINISH_TIME).ThenBy(o => o.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
        }
        private void GetServiceReq()
        {
            HisServiceReqFilterQuery ServiceReqfilter = new HisServiceReqFilterQuery()
            {
                FINISH_TIME_FROM = this.castFilter.TIME_FROM,
                FINISH_TIME_TO = this.castFilter.TIME_TO,
                ORDER_FIELD = "ID",
                ORDER_DIRECTION = "ASC",
                HAS_EXECUTE = true,
                SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT
            };
            ListServiceReq = new HisServiceReqManager(paramGet).Get(ServiceReqfilter);


            //Các yêu cầu khám kết thúc trong thời gian báo cáo
            HisBedRoomViewFilterQuery bedRoomFilter = new HisBedRoomViewFilterQuery();
            bedRoomFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            var listBedRooms = new HisBedRoomManager(param).GetView(bedRoomFilter);

            ListServiceReq = ListServiceReq.Where(o => !listBedRooms.Exists(p => p.ROOM_ID == o.REQUEST_ROOM_ID)).OrderBy(o => o.FINISH_TIME).ThenBy(o => o.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

           
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {

                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    foreach (var item in ListServiceReq)
                    {
                        List<HIS_SERVICE_REQ> prescription = dicPrescription.ContainsKey(item.TREATMENT_ID) ? dicPrescription[item.TREATMENT_ID] : new List<HIS_SERVICE_REQ>();
                        prescription = prescription.Where(o => o.PARENT_ID == item.ID && o.REQUEST_LOGINNAME == item.EXECUTE_LOGINNAME).ToList();

                        List<HIS_EXP_MEST> expMestsub = dicExpMest.ContainsKey(item.TREATMENT_ID)?dicExpMest[item.TREATMENT_ID] : new List<HIS_EXP_MEST>();
                        expMestsub = expMestsub.Where(o => prescription.Exists(p => p.ID == o.SERVICE_REQ_ID || p.ID == o.PRESCRIPTION_ID)).ToList();

                        List<HIS_EXP_MEST_MATERIAL> expMestMaterials = dicExpMestMaterial.ContainsKey(item.TREATMENT_ID)?dicExpMestMaterial[item.TREATMENT_ID] : new List<HIS_EXP_MEST_MATERIAL>();
                        expMestMaterials = expMestMaterials.Where(o => expMestsub.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();

                        List<HIS_EXP_MEST_MEDICINE> expMestMedicines = dicExpMestMedicine.ContainsKey(item.TREATMENT_ID) ? dicExpMestMedicine[item.TREATMENT_ID] : new List<HIS_EXP_MEST_MEDICINE>();
                        expMestMedicines = expMestMedicines.Where(o => expMestsub.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();

                        Mrs00250RDO rdo = new Mrs00250RDO();
                        rdo.LOGINNAME = item.EXECUTE_LOGINNAME;
                        rdo.USERNAME = item.EXECUTE_USERNAME;
                        rdo.AMOUNT = prescription.Count;
                        rdo.AMOUNT_BH = prescription.Count(o => !string.IsNullOrWhiteSpace((dicTreatment.ContainsKey(item.TREATMENT_ID)?dicTreatment[item.TREATMENT_ID] : new HIS_TREATMENT()).TDL_HEIN_CARD_NUMBER));
                        rdo.AMOUNT_VP = prescription.Count(o => string.IsNullOrWhiteSpace((dicTreatment.ContainsKey(item.TREATMENT_ID)?dicTreatment[item.TREATMENT_ID] : new HIS_TREATMENT()).TDL_HEIN_CARD_NUMBER));
                        rdo.TOTAL_PRICE = expMestMaterials.Sum(o => (o.AMOUNT * (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0))))
                            + expMestMedicines.Sum(o => (o.AMOUNT * (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0))));
                        rdo.AMOUNT_EXAM_SERVICE = 1;
                        rdo.AMOUNT_HAS_PRES = prescription.Count > 0 ? 1 : 0;
                        rdo.AMOUNT_HAS_PRES_BH = prescription.Count(o => !string.IsNullOrWhiteSpace((dicTreatment.ContainsKey(o.TREATMENT_ID)?dicTreatment[o.TREATMENT_ID] : new HIS_TREATMENT()).TDL_HEIN_CARD_NUMBER)) > 0 ? 1 : 0;
                        rdo.AMOUNT_IN_TREAT = dicTreatment.ContainsKey(item.TREATMENT_ID)
                            && (dicTreatment[item.TREATMENT_ID].TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM) ? 1 : 0;
                        
                        rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                        rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.PARENT_ID = item.ID;
                        rdo.IS_BHYT = (!string.IsNullOrWhiteSpace((dicTreatment.ContainsKey(item.TREATMENT_ID)?dicTreatment[item.TREATMENT_ID] : new HIS_TREATMENT()).TDL_HEIN_CARD_NUMBER)) ? "x" : "";
                        rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FINISH_TIME??0);

                        ListRdoDetail.Add(rdo);
                    }
                    var groupByCreater = ListRdoDetail.GroupBy(o => o.LOGINNAME).ToList();

                    foreach (var group in groupByCreater)
                    {
                        List<Mrs00250RDO> listSub = group.ToList<Mrs00250RDO>();

                        Mrs00250RDO rdo = new Mrs00250RDO()
                        {
                            LOGINNAME = listSub.First().LOGINNAME,
                            USERNAME = listSub.First().USERNAME,
                            AMOUNT = listSub.Sum(s=>s.AMOUNT),
                            AMOUNT_BH = listSub.Sum(s => s.AMOUNT_BH),
                            AMOUNT_VP = listSub.Sum(s => s.AMOUNT_VP),
                            TOTAL_PRICE = listSub.Sum(s => s.TOTAL_PRICE),
                            AMOUNT_EXAM_SERVICE = listSub.Sum(s => s.AMOUNT_EXAM_SERVICE),
                            AMOUNT_HAS_PRES = listSub.Sum(s => s.AMOUNT_HAS_PRES),
                            AMOUNT_HAS_PRES_BH = listSub.Sum(s => s.AMOUNT_HAS_PRES_BH),
                            AMOUNT_IN_TREAT = listSub.Sum(s => s.AMOUNT_IN_TREAT)
                        };

                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.castFilter.TIME_TO));
            var department = new HIS_DEPARTMENT();
            if ((castFilter.REQUEST_DEPARTMENT_ID ?? 0) != 0)
            {
                department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
            }
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportPres", ListRdoDetail.Where(p => p.AMOUNT > 0).OrderBy(o => o.INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail.OrderBy(o => o.INTRUCTION_TIME).ToList());
            objectTag.AddRelationship(store, "Report", "ReportPres", "LOGINNAME", "LOGINNAME");
            objectTag.AddObjectData(store, "ServiceReqs", ListServiceReq);
            objectTag.AddObjectData(store, "Treatments", dicTreatment.Values.ToList());
            objectTag.AddObjectData(store, "Departments", HisDepartmentCFG.DEPARTMENTs);
        }
    }
}
//cac HSDT dc kham boi bs
/*select * from his_treatment where ((out_time between 20180723000000 and 20180723235959 and tdl_treatment_type_id =1 and is_pause =1) 
or (clinical_in_time between 20180723000000 and 20180723235959 and tdl_treatment_type_id >1))
and exists(select 1 from his_service_req where service_req_type_id = 1 and execute_username = 'ThS.Bs.Trần Thị Thanh Hà' and( is_no_execute is null or is_no_execute <>1) and treatment_id = his_treatment.id);*/