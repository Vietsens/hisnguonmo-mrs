using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00292
{
    class Mrs00292Processor : AbstractProcessor
    {
        Mrs00292Filter castFilter = null;
        List<Mrs00292RDO> listRdo = new List<Mrs00292RDO>();
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_EXECUTE_ROOM_1> ListExecuteRoom = null;

        Dictionary<long, Mrs00292RDO> dicRdo = new Dictionary<long, Mrs00292RDO>();

        string branch_name = "";

        public Mrs00292Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00292Filter);
        }

        protected override bool GetData()///
        {
            bool valid = true;
            try
            {
                this.castFilter = (Mrs00292Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                //yeu cau
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                reqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                var ListServiceReqId = new HisServiceReqManager(paramGet).Get(reqFilter).Select(o => o.ID).ToList();


                //YC-DV
                if (IsNotNullOrEmpty(ListServiceReqId))
                {
                    int skip = 0;
                    while (ListServiceReqId.Count - skip > 0)
                    {
                        var limit = ListServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.SERVICE_REQ_IDs = limit;
                        var ListSereServSub = new HisSereServManager(paramGet).GetView(ssFilter);
                        if (IsNotNullOrEmpty(ListSereServSub))
                        {
                            ListSereServ.AddRange(ListSereServSub);
                        }
                    }
                }

                //phong thuc hien
                HisExecuteRoomView1FilterQuery exeRoomFilter = new HisExecuteRoomView1FilterQuery();
                exeRoomFilter.BRANCH_ID = castFilter.BRANCH_ID;
                ListExecuteRoom = new HisExecuteRoomManager(paramGet).GetView1(exeRoomFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                if (!IsNotNullOrEmpty(ListExecuteRoom))
                {
                    throw new Exception("Khong lay duoc danh sach phong kham");
                }

                Dictionary<long, V_HIS_EXECUTE_ROOM_1> dicEmergencyRoom = new Dictionary<long, V_HIS_EXECUTE_ROOM_1>();

                foreach (var item in ListExecuteRoom)
                {
                    if (item.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        Mrs00292RDO rdo = new Mrs00292RDO();
                        rdo.EXECUTE_ROOM_CODE = item.EXECUTE_ROOM_CODE;
                        rdo.EXECUTE_ROOM_NAME = item.EXECUTE_ROOM_NAME;
                        dicRdo[item.ROOM_ID] = rdo;
                    }
                    if (item.IS_EMERGENCY == 1)
                    {
                        dicEmergencyRoom[item.ROOM_ID] = item;
                    }
                }

                Dictionary<long, V_HIS_SERE_SERV> dicSereServEmergency = new Dictionary<long, V_HIS_SERE_SERV>();

                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    var listTreatmentId = ListSereServ.Where(o => o.TDL_TREATMENT_ID.HasValue).Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    if (!IsNotNullOrEmpty(listTreatmentId))
                    {
                        throw new Exception("Danh sach sereServ khong chua treatmentId" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ListSereServ), ListSereServ));
                    }

                    int start = 0;
                    int count = listTreatmentId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listId = listTreatmentId.Skip(start).Take(limit).ToList();
                        //chuyen doi tuong
                        HisPatientTypeAlterViewFilterQuery ptAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        ptAlterFilter.TREATMENT_IDs = listId;
                        var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetView(ptAlterFilter);
                        //?YC-DV
                        var listSereServ = ListSereServ.Where(o => listId.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co loi xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_PATIENT_TYPE_ALTER va V_HIS_SERE_SERV: MRS00292");
                        }
                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            listPatientTypeAlter = listPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                            foreach (var item in listPatientTypeAlter)
                            {
                                if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                    dicPatientTypeAlter[item.TREATMENT_ID] = item;
                            }
                        }

                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var item in listSereServ)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    continue;
                                if (!dicEmergencyRoom.ContainsKey(item.TDL_EXECUTE_ROOM_ID))
                                    continue;
                               
                                if (!dicSereServEmergency.ContainsKey(item.TDL_TREATMENT_ID ?? 0)) dicSereServEmergency[item.TDL_TREATMENT_ID ?? 0] = item;
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ProcessListSereServ(dicPatientTypeAlter, dicSereServEmergency);
                    ProcessBranchById();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void ProcessListSereServ(Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, Dictionary<long, V_HIS_SERE_SERV> dicSereServEmergency)
        {
            if (IsNotNullOrEmpty(ListSereServ))
            {
                foreach (var item in ListSereServ)
                {
                    if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.PRICE == 0)
                        continue;
                    if (!dicRdo.ContainsKey(item.TDL_EXECUTE_ROOM_ID))
                        continue;
                    var rdo = dicRdo[item.TDL_EXECUTE_ROOM_ID];
                    rdo.TOTAL_AMOUNT += 1;
                    if (dicPatientTypeAlter.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                    {
                        var paty = dicPatientTypeAlter[item.TDL_TREATMENT_ID ?? 0];
                        if (paty.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.TOTAL_BHYT_AMOUNT += 1;
                        }
                        else if (paty.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            rdo.TOTAL_FEE_AMOUNT += 1;
                        }
                        else
                        {
                            rdo.TOTAL_OTHER_AMOUNT += 1;
                        }
                    }
                    else
                    {
                        rdo.TOTAL_OTHER_AMOUNT += 1;
                    }

                    if (dicSereServEmergency.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                    {
                        rdo.TOTAL_CC_AMOUNT += 1;
                    }
                }
            }
        }

        private void ProcessBranchById()
        {
            try
            {
                if (castFilter.BRANCH_ID.HasValue)
                {
                    var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID.Value);
                    if (branch != null)
                    {
                        this.branch_name = branch.BRANCH_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                dicSingleTag.Add("BRANCH_NAME", this.branch_name);
                objectTag.AddObjectData(store, "Report", dicRdo.Select(s => s.Value).OrderBy(o => o.EXECUTE_ROOM_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
