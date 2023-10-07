using MOS.MANAGER.HisService;
using MOS.MANAGER.HisExecuteRoom;
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
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00473
{
    //báo cáo hoạt động điều trị

    class Mrs00473Processor : AbstractProcessor
    {
        Mrs00473Filter castFilter = new Mrs00473Filter();
        List<Mrs00473RDO> listRdo = new List<Mrs00473RDO>();
        List<EXECUTE_ROOM_473> listRooms = new List<EXECUTE_ROOM_473>();

        List<HIS_SERVICE_REQ> listServcieReqs = new List<HIS_SERVICE_REQ>();
        List<V_HIS_SERE_SERV> listSereServMediMates = new List<V_HIS_SERE_SERV>();
        //List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        List<HIS_SERVICE_REQ> listPrescriptions = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_EXECUTE_ROOM> listExamRooms = new List<V_HIS_EXECUTE_ROOM>();
        List<V_HIS_BED_ROOM> listBedRooms = new List<V_HIS_BED_ROOM>();
        string EXECUTE_USERNAME = "";

        long examEndTypePause = 3;
        long examEndTypeInTreat = 2;

        public Mrs00473Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00473Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00473Filter)this.reportFilter;

                HisBedRoomViewFilterQuery bedRoomFilter = new HisBedRoomViewFilterQuery();
                bedRoomFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                listBedRooms = new HisBedRoomManager(param).GetView(bedRoomFilter);

                var skip = 0;
                List<V_HIS_EXECUTE_ROOM> listExecuteRooms = new List<V_HIS_EXECUTE_ROOM>();
                HisExecuteRoomViewFilterQuery executeRoomFilter = new HisExecuteRoomViewFilterQuery();
                executeRoomFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                executeRoomFilter.IS_EXAM = true;
                listExamRooms = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(param).GetView(executeRoomFilter);

                listExecuteRooms = listExamRooms;
                if (IsNotNullOrEmpty(castFilter.EXECUTE_ROOM_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.EXE_ROOM_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.EXE_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }

                if (IsNotNullOrEmpty(listExecuteRooms))
                {
                    HisServiceReqFilterQuery serviceReqViewFilter = new HisServiceReqFilterQuery();
                    serviceReqViewFilter.EXECUTE_ROOM_IDs = listExecuteRooms.Select(o => o.ROOM_ID).ToList(); ;
                    serviceReqViewFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                    serviceReqViewFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                    serviceReqViewFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    serviceReqViewFilter.HAS_EXECUTE = true;
                    listServcieReqs = new HisServiceReqManager(param).Get(serviceReqViewFilter);

                    listServcieReqs = listServcieReqs.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && ((!IsNotNullOrEmpty(castFilter.EXECUTE_LOGINNAME)) || castFilter.EXECUTE_LOGINNAME == w.EXECUTE_LOGINNAME)).ToList();

                    if (IsNotNullOrEmpty(listServcieReqs))
                    {


                        skip = 0;
                        while (listServcieReqs.Count - skip > 0)
                        {
                            var listIds = listServcieReqs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisServiceReqFilterQuery presFilter = new HisServiceReqFilterQuery();
                            presFilter.REQUEST_ROOM_IDs = listExecuteRooms.Select(s => s.ROOM_ID).ToList();
                            presFilter.TREATMENT_IDs = listIds.Select(s => s.TREATMENT_ID).ToList();
                            presFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT };
                            listPrescriptions.AddRange(new HisServiceReqManager(param).Get(presFilter));
                            HisTreatmentFilterQuery tmFilter = new HisTreatmentFilterQuery();
                            tmFilter.IDs = listIds.Select(s => s.TREATMENT_ID).ToList();
                            tmFilter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                            listTreatment.AddRange(new HisTreatmentManager(param).Get(tmFilter) ?? new List<HIS_TREATMENT>());
                        }
                    }

                    listExecuteRooms = listExecuteRooms.OrderBy(o => o.EXECUTE_ROOM_NAME).ToList();
                    int i = 1;
                    foreach (var room in listExecuteRooms)
                    {
                        var rdoRoom = new EXECUTE_ROOM_473();
                        rdoRoom.NUMBER = i;
                        rdoRoom.ROOM_ID = room.ROOM_ID;
                        rdoRoom.ROOM_NAME = room.EXECUTE_ROOM_NAME;
                        rdoRoom.ROOM_TAG = "ROOM_" + i;
                        i++;
                        listRooms.Add(rdoRoom);
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(listServcieReqs))
                {
                    EXECUTE_USERNAME = listServcieReqs.First().EXECUTE_USERNAME;

                    var listSereServGroupByTreatments = listServcieReqs.GroupBy(g => g.TREATMENT_ID);

                    var pres = listPrescriptions.Where(w => !IsNotNullOrEmpty(castFilter.EXECUTE_LOGINNAME) || castFilter.EXECUTE_LOGINNAME == w.REQUEST_LOGINNAME).ToList();
                    var tranpati = listTreatment.Where(o => !IsNotNullOrEmpty(castFilter.EXECUTE_LOGINNAME) || o.DOCTOR_LOGINNAME == castFilter.EXECUTE_LOGINNAME).ToList();

                    foreach (var treatment in listSereServGroupByTreatments)
                    {
                        var rdo = new Mrs00473RDO();
                        rdo.PATIENT_CODE = treatment.First().TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = treatment.First().TDL_PATIENT_NAME;
                        rdo.TREATMENT_CODE = treatment.First().TDL_TREATMENT_CODE;

                        bool notFinish = true;
                        bool TrueMrs476 = false;
                        foreach (var exam in treatment)
                        {
                            TrueMrs476 = TrueMrs476 || pres.Exists(p => p.PARENT_ID == exam.ID) || listExamRooms.Exists(p => p.ROOM_ID == exam.REQUEST_ROOM_ID)
                                || listBedRooms.Exists(p => p.ROOM_ID == exam.REQUEST_ROOM_ID)
                                || tranpati.Exists(p => p.ID == exam.TREATMENT_ID && exam.EXECUTE_ROOM_ID == p.END_ROOM_ID);


                            if (this.castFilter.EXAM_END_TYPE__PAUSE == true)
                            {
                                //kham ket thuc dieu tri: phong kham nay ket thuc dieu tri cho benh nhan va bac si kham nay la bac si ket luan cho benh nhan
                                TrueMrs476 = TrueMrs476 || exam.EXAM_END_TYPE == examEndTypePause;
                            }
                            if (this.castFilter.EXAM_END_TYPE__INTREAT == true)
                            {
                                //kham nhap vien: phong kham nay nhap vien cho benh nhan
                                TrueMrs476 = TrueMrs476 || exam.EXAM_END_TYPE == examEndTypeInTreat;
                            }
                            //if (IsNotNullOrEmpty(pres))
                            {
                                var roomtag = listRooms.Where(w => w.ROOM_ID == exam.EXECUTE_ROOM_ID).ToList();
                                if (IsNotNullOrEmpty(roomtag))
                                {
                                    if (roomtag.First().NUMBER == 1) rdo.ROOM_1 = "X";
                                    else if (roomtag.First().NUMBER == 2) rdo.ROOM_2 = "X";
                                    else if (roomtag.First().NUMBER == 3) rdo.ROOM_3 = "X";
                                    else if (roomtag.First().NUMBER == 4) rdo.ROOM_4 = "X";
                                    else if (roomtag.First().NUMBER == 5) rdo.ROOM_5 = "X";
                                    else if (roomtag.First().NUMBER == 6) rdo.ROOM_6 = "X";
                                    else if (roomtag.First().NUMBER == 7) rdo.ROOM_7 = "X";
                                    else if (roomtag.First().NUMBER == 8) rdo.ROOM_8 = "X";
                                    else if (roomtag.First().NUMBER == 9) rdo.ROOM_9 = "X";
                                    else if (roomtag.First().NUMBER == 10) rdo.ROOM_10 = "X";
                                    else if (roomtag.First().NUMBER == 11) rdo.ROOM_11 = "X";
                                    else if (roomtag.First().NUMBER == 12) rdo.ROOM_12 = "X";
                                    else if (roomtag.First().NUMBER == 13) rdo.ROOM_13 = "X";
                                    else if (roomtag.First().NUMBER == 14) rdo.ROOM_14 = "X";
                                    else if (roomtag.First().NUMBER == 15) rdo.ROOM_15 = "X";
                                }
                                if (exam.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                                {
                                    notFinish = false;
                                }
                            }
                        }

                        if (!notFinish)
                            rdo.ROOM_OTHER = "X";
                        if (TrueMrs476)
                        listRdo.Add(rdo);
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
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("EXECUTE_USERNAME", this.EXECUTE_USERNAME);
                foreach (var roomTag in listRooms)
                {
                    dicSingleTag.Add(roomTag.ROOM_TAG, roomTag.ROOM_NAME);
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.TREATMENT_CODE).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
