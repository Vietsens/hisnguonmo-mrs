using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisServiceReq;
using SDA.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;

namespace MRS.Processor.Mrs00474
{
    public class Mrs00474Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00474RDO> listRdo = new List<Mrs00474RDO>();
        private List<MOS.EFMODEL.DataModels.V_HIS_TREATMENT_4> Treatments = new List<V_HIS_TREATMENT_4>();
        Mrs00474Filter filter = null;
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_7> listSereServ = new List<V_HIS_SERE_SERV_7>();
        Dictionary<long, V_HIS_SERVICE> dicService = new Dictionary<long, V_HIS_SERVICE>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety = new Dictionary<long, List<HIS_SERVICE_METY>>();
        Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty = new Dictionary<long, List<HIS_SERVICE_MATY>>();
        Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceChem = new Dictionary<long, List<HIS_SERVICE_MATY>>();
        Dictionary<long, HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_TREATMENT_4> dicTreatment = new Dictionary<long, V_HIS_TREATMENT_4>();
        Dictionary<long,
            HIS_DEPARTMENT> dicDepartment = new Dictionary<long, HIS_DEPARTMENT>();
        Dictionary<long, V_HIS_EXECUTE_ROOM> dicExamRoom = new Dictionary<long, V_HIS_EXECUTE_ROOM>();
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();

        string outPatientDepartmentCode = "KKB";
        public Mrs00474Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00474Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00474Filter)reportFilter);
            listSereServ = new List<V_HIS_SERE_SERV_7>();
            var result = true;
            CommonParam param = new CommonParam();
            try
            {
                //Cau hinh khoa kham benh
                var config = Loader.dictionaryConfig["MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__EXAM"];
                if (config == null) throw new ArgumentNullException("MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__EXAM");
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (value != null) outPatientDepartmentCode = value;

                // Ho so dieu tri
                HisTreatmentView4FilterQuery filtermain = new HisTreatmentView4FilterQuery();
                filtermain.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                filtermain.FEE_LOCK_TIME_TO = filter.TIME_TO;
                filtermain.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                Treatments = new HisTreatmentManager(paramGet).GetView4(filtermain);
                dicTreatment = Treatments.ToDictionary(o => o.ID);
                // sere_serv tuong ung
                if (Treatments != null && Treatments.Count > 0)
                {
                    var skip = 0;
                    while (Treatments.Count - skip > 0)
                    {
                        var Ids = Treatments.Select(o => o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServView7FilterQuery sereServFilter = new HisSereServView7FilterQuery();
                        sereServFilter.TDL_TREATMENT_IDs = Ids;
                        var listSereServSub = new HisSereServManager(param).GetView7(sereServFilter);
                        listSereServ.AddRange(listSereServSub);
                    }
                }
                listSereServ = listSereServ.Where(o => o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var listTreatmentId = listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                //service_req tuong ung
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var serviceReqfilter = new HisServiceReqFilterQuery()
                        {
                            TREATMENT_IDs = limit
                        };
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(serviceReqfilter);
                        listServiceReq.AddRange(listServiceReqSub);
                    }
                    dicServiceReq = listServiceReq.ToDictionary(o => o.ID);
                }
                #region cac danh sach phu tro
                //phong kham
                HisExecuteRoomViewFilterQuery exroom = new HisExecuteRoomViewFilterQuery();
                exroom.IS_EXAM = true;
                var ExamRooms = new HisExecuteRoomManager(param).GetView(exroom);
                foreach (var item in ExamRooms)
                {
                    if (!dicExamRoom.ContainsKey(item.ROOM_ID)) dicExamRoom[item.ROOM_ID] = item;
                }
                // Khoa lam sang
                HisDepartmentFilterQuery clndepartment = new HisDepartmentFilterQuery();
                clndepartment.IS_CLINICAL = true;
                var ClinicalDepartments = new HisDepartmentManager(param).Get(clndepartment);
                foreach (var item in ClinicalDepartments)
                {
                    if (!dicDepartment.ContainsKey(item.ID)) dicDepartment[item.ID] = item;
                }

                //Doi tuong dieu tri
                dicCurrentPatyAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatmentId).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).GroupBy(q => q.TREATMENT_ID).ToDictionary(r => r.Key, r => r.Last());

                //Dich vu
                List<V_HIS_SERVICE> listService = new List<V_HIS_SERVICE>();
                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                listService = new HisServiceManager().GetView(serviceFilter);
                foreach (var item in listService)
                {
                    if (!dicService.ContainsKey(item.ID)) dicService[item.ID] = item;
                }

                //Dinh muc thuoc hao phi
                List<HIS_SERVICE_METY> listServiceMety = new List<HIS_SERVICE_METY>();
                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery();
                listServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);
                foreach (var item in listServiceMety)
                {
                    if (!dicServiceMety.ContainsKey(item.SERVICE_ID)) dicServiceMety[item.SERVICE_ID] = new List<HIS_SERVICE_METY>();
                    dicServiceMety[item.SERVICE_ID].Add(item);
                }

                //Dinh muc vat tu hao phi
                List<HIS_SERVICE_MATY> listServiceMaty = new List<HIS_SERVICE_MATY>();
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery();
                listServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);

                //Hoa chat
                List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
                if (IsNotNullOrEmpty(listServiceMaty))
                {
                    var skip = 0;
                    while (listServiceMaty.Count - skip > 0)
                    {
                        var listIDs = listServiceMaty.Select(o => o.MATERIAL_TYPE_ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialTypeFilterQuery materialTypeFilter = new HisMaterialTypeFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var materialType = new HisMaterialTypeManager(paramGet).Get(materialTypeFilter);
                        listMaterialType.AddRange(materialType);
                    }
                    foreach (var item in listMaterialType)
                    {
                        if (!dicMaterialType.ContainsKey(item.ID)) dicMaterialType[item.ID] = item;
                    }
                    foreach (var item in listServiceMaty)
                    {
                        if (!dicServiceMaty.ContainsKey(item.SERVICE_ID)) dicServiceMaty[item.SERVICE_ID] = new List<HIS_SERVICE_MATY>();
                        dicServiceMaty[item.SERVICE_ID].Add(item);
                        if (dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) && dicMaterialType[item.MATERIAL_TYPE_ID].IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            if (!dicServiceChem.ContainsKey(item.SERVICE_ID)) dicServiceChem[item.SERVICE_ID] = new List<HIS_SERVICE_MATY>();
                            dicServiceChem[item.SERVICE_ID].Add(item);
                        }
                    }
                }
                #endregion
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
            listRdo.Clear();
            try
            {
                listRdo.Clear();
                if (IsNotNullOrEmpty(listSereServ))
                {
                    //Loc theo dien dieu tri da chon
                    if (listSereServ != null)
                        listSereServ = listSereServ.Where(o => (!IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs)) || filter.TREATMENT_TYPE_IDs.Contains(treatmentType(o.TDL_TREATMENT_ID ?? 0))).ToList();

                    //neu la kham thi lay phong chi dinh la phong thuc hien
                    foreach (var item in listSereServ)
                    {
                        if (dicServiceReq.ContainsKey(item.SERVICE_REQ_ID ?? 0) && dicServiceReq[item.SERVICE_REQ_ID ?? 0].SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH)
                        {
                            item.TDL_REQUEST_DEPARTMENT_ID = dicServiceReq[item.SERVICE_REQ_ID ?? 0].EXECUTE_DEPARTMENT_ID;
                            item.TDL_REQUEST_ROOM_ID = dicServiceReq[item.SERVICE_REQ_ID ?? 0].EXECUTE_ROOM_ID;
                        }
                    }
                    listSereServ = listSereServ.Where(o => (filter.DEPARTMENT_ID ?? 0) == 0 || o.TDL_REQUEST_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                    List<V_HIS_SERE_SERV_7> sub = new List<V_HIS_SERE_SERV_7>();

                    if (IsNotNullOrEmpty(listSereServ))
                    {
                        var goupbyReq = listSereServ.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList();

                        //Phong chi dinh
                        GetRequestRoomInfo(listSereServ);

                        foreach (var item in goupbyReq)
                        {
                            List<V_HIS_SERE_SERV_7> listSub = item.ToList<V_HIS_SERE_SERV_7>();
                            Mrs00474RDO rdo = new Mrs00474RDO();

                            if (dicDepartment.ContainsKey(item.Key) && dicDepartment[item.Key].DEPARTMENT_CODE == this.outPatientDepartmentCode)
                            {
                                #region du lieu khoa kham benh
                                var groupByRoom = listSub.GroupBy(o => o.TDL_REQUEST_ROOM_ID).ToList();
                                foreach (var itemm in groupByRoom)
                                {
                                    rdo = new Mrs00474RDO();
                                    Inventec.Common.Logging.LogSystem.Info("1:So BN khoa Kham benh:" + string.Join(",", listSub.Select(o => o.TDL_TREATMENT_CODE).ToList()));

                                    List<V_HIS_SERE_SERV_7> Sub = itemm.ToList<V_HIS_SERE_SERV_7>();
                                    rdo.DEPARTMENT_NAME = dicRoom.ContainsKey(Sub.First().TDL_REQUEST_ROOM_ID) ? dicRoom[Sub.First().TDL_REQUEST_ROOM_ID].ROOM_NAME : "";

                                    rdo.TREATMENT_COUNT = Sub.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;
                                    rdo.TOTAL_PRICE = Sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                    rdo.HEIN_TOTAL_PRICE = Sub.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                                    rdo.PATIENT_TOTAL_PRICE = Sub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                    rdo.PATIENT_TOTAL_PRICE_BHYT = Sub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                    rdo.FEE_TOTAL_PRICE = rdo.PATIENT_TOTAL_PRICE - rdo.PATIENT_TOTAL_PRICE_BHYT;
                                    rdo.revenue = loadField(Sub);
                                    rdo.revenuePk = new REVENUE();
                                    listRdo.Add(rdo);
                                }
                                #endregion
                            }
                            else
                            {
                                //voi khoa khac
                                #region du lieu khoa khac
                                rdo.DEPARTMENT_NAME = dicDepartment.ContainsKey(listSub.First().TDL_REQUEST_DEPARTMENT_ID) ? dicDepartment[listSub.First().TDL_REQUEST_DEPARTMENT_ID].DEPARTMENT_NAME : "";
                                rdo.TREATMENT_COUNT = listSub.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;
                                var listSub1 = listSub.Where(o => !(dicExamRoom.ContainsKey(o.TDL_REQUEST_ROOM_ID))).ToList();
                                if (IsNotNullOrEmpty(listSub1))
                                {
                                    rdo.TOTAL_PRICE = listSub1.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                                    rdo.HEIN_TOTAL_PRICE = listSub1.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                                    rdo.PATIENT_TOTAL_PRICE = listSub1.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                    rdo.PATIENT_TOTAL_PRICE_BHYT = listSub1.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                    rdo.FEE_TOTAL_PRICE = rdo.PATIENT_TOTAL_PRICE - rdo.PATIENT_TOTAL_PRICE_BHYT;
                                    rdo.revenue = loadField(listSub1);
                                }
                                else rdo.revenue = new REVENUE();

                                var listSub2 = listSub.Where(o => dicExamRoom.ContainsKey(o.TDL_REQUEST_ROOM_ID)).ToList();
                                if (IsNotNullOrEmpty(listSub2))
                                {
                                    rdo.revenuePk = loadField(listSub2);
                                }
                                else rdo.revenuePk = new REVENUE();
                                listRdo.Add(rdo);
                                #endregion
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void GetRequestRoomInfo(List<V_HIS_SERE_SERV_7> listSereServ)
        {
            var listReqRoomId = listSereServ.Select(o => o.TDL_REQUEST_ROOM_ID).Distinct().ToList();
            if (IsNotNullOrEmpty(listReqRoomId))
            {
                List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
                var skip = 0;
                while (listReqRoomId.Count - skip > 0)
                {
                    var Ids = listReqRoomId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var roomfilter = new HisRoomViewFilterQuery()
                    {
                        IDs = Ids
                    };
                    var listRoomSub = new HisRoomManager(paramGet).GetView(roomfilter);
                    listRoom.AddRange(listRoomSub);
                }
                dicRoom = listRoom.ToDictionary(o => o.ID);
            }
        }

        private long treatmentType(long treatmentId)
        {
            return dicCurrentPatyAlter.ContainsKey(treatmentId) ? dicCurrentPatyAlter[treatmentId].TREATMENT_TYPE_ID : 0;
        }

        private REVENUE loadField(List<V_HIS_SERE_SERV_7> listSub1)
        {
            REVENUE result = new REVENUE();
            try
            {
                List<V_HIS_SERE_SERV_7> sub = null;
                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();
                result.EXAM_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList();
                result.BED_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).ToList();
                result.FUEX_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList();
                result.SUIM_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && dicService[o.SERVICE_ID].SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                result.OTHER_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).ToList();
                result.ENDO_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).ToList();
                result.BLOOD_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList();
                result.TEIN_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).ToList();
                result.DIIM_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
                result.MISU_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
                result.SURG_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList();
                result.MEDI_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => dicService.ContainsKey(o.SERVICE_ID) && dicService[o.SERVICE_ID].SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                result.MATE_PRICE = sub.Count > 0 ? sub.Sum(s => s.VIR_TOTAL_PRICE ?? 0) : 0;

                sub = listSub1.Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                result.EXPEND_TICK = sub.Count > 0 ? sub.Sum(s => s.AMOUNT * s.PRICE) : 0;

                sub = listSub1.Where(o => dicServiceMaty.ContainsKey(o.SERVICE_ID) && (!dicServiceChem.ContainsKey(o.SERVICE_ID))).ToList();
                result.MATE_EXPEND = sub.Count > 0 ? sub.Select(s => dicServiceMaty[s.SERVICE_ID].Select(su => su.EXPEND_AMOUNT * su.EXPEND_PRICE ?? 0).Sum()).Sum() : 0;

                sub = listSub1.Where(o => dicServiceMety.ContainsKey(o.SERVICE_ID)).ToList();
                result.MEDI_EXPEND = sub.Count > 0 ? sub.Select(s => dicServiceMety[s.SERVICE_ID].Select(su => su.EXPEND_AMOUNT * su.EXPEND_PRICE ?? 0).Sum()).Sum() : 0;

                sub = listSub1.Where(o => dicServiceChem.ContainsKey(o.SERVICE_ID)).ToList();
                result.CHEMICAL_EXPEND = sub.Count > 0 ? sub.Select(s => dicServiceChem[s.SERVICE_ID].Select(su => su.EXPEND_AMOUNT * su.EXPEND_PRICE ?? 0).Sum()).Sum() : 0;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new REVENUE();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00474Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00474Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00474Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00474Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "SereServ", listSereServ.OrderBy(o=>o.TDL_REQUEST_DEPARTMENT_ID).ThenBy(p=>p.TDL_REQUEST_ROOM_ID).ThenBy(q=>q.TDL_TREATMENT_ID).ToList());
        }
    }
}
