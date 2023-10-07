using MOS.MANAGER.HisService;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00476
{
    //báo cáo hoạt động điều trị

    class Mrs00476Processor : AbstractProcessor
    {
        Mrs00476Filter castFilter = null;
        List<Mrs00476RDO> listRdo = new List<Mrs00476RDO>();

        List<HIS_SERVICE_REQ> listServiceReqs = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listDetail = new List<HIS_SERVICE_REQ>();
        List<EXECUTE_ROOM_476> listRooms = new List<EXECUTE_ROOM_476>();
        List<HIS_SERVICE_REQ> listPresServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_EXECUTE_ROOM> listExamRooms = new List<V_HIS_EXECUTE_ROOM>();
        List<V_HIS_BED_ROOM> listBedRooms = new List<V_HIS_BED_ROOM>();
        long presExam = 1;
        long addExam = 2;
        long specExam = 3;
        long tranExam = 4;

        long examEndTypePause = 3;
        long examEndTypeInTreat = 2;

        public Mrs00476Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00476Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00476Filter)this.reportFilter;
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
                if (IsNotNullOrEmpty(castFilter.AREA_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.AREA_IDs.Contains(o.AREA_ID??0)).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.EXAM_ROOM_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.EXAM_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.EXECUTE_ROOM_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.ROOM_IDs))
                {
                    listExecuteRooms = listExamRooms.Where(o => castFilter.ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                long i = 1;
                foreach (var executeRoom in listExecuteRooms)
                {
                    var room = new EXECUTE_ROOM_476();
                    room.NUMBER = i;
                    room.ROOM_ID = executeRoom.ROOM_ID;
                    room.ROOM_NAME = executeRoom.EXECUTE_ROOM_NAME;
                    room.ROOM_TAG = "ROOM_" + i;
                    i++;
                    listRooms.Add(room);
                }

                if (IsNotNullOrEmpty(listRooms))
                {
                    skip = 0;
                    while (listRooms.Count - skip > 0)
                    {
                        var listIds = listRooms.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery serviecReqFilter = new HisServiceReqFilterQuery();
                        serviecReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                        serviecReqFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                        serviecReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        serviecReqFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                        serviecReqFilter.EXECUTE_ROOM_IDs = listIds.Select(s => s.ROOM_ID).ToList();
                        listServiceReqs.AddRange(new HisServiceReqManager(param).Get(serviecReqFilter));
                        if (castFilter.EXAM_EXECUTING == true)
                        {
                            HisServiceReqFilterQuery serviecReqExecutingFilter = new HisServiceReqFilterQuery();
                            serviecReqExecutingFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                            serviecReqExecutingFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                            serviecReqExecutingFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                            serviecReqExecutingFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL;
                            serviecReqExecutingFilter.EXECUTE_ROOM_IDs = listIds.Select(s => s.ROOM_ID).ToList();
                            listServiceReqs.AddRange(new HisServiceReqManager(param).Get(serviecReqExecutingFilter));
                        }
                        if (castFilter.EXAM_YET == true)
                        {
                            HisServiceReqFilterQuery serviecReqExecutingFilter = new HisServiceReqFilterQuery();
                            serviecReqExecutingFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                            serviecReqExecutingFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                            serviecReqExecutingFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                            serviecReqExecutingFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL;
                            serviecReqExecutingFilter.EXECUTE_ROOM_IDs = listIds.Select(s => s.ROOM_ID).ToList();
                            listServiceReqs.AddRange(new HisServiceReqManager(param).Get(serviecReqExecutingFilter));
                        }
                    }
                }
                if (castFilter.LOGINNAME_DOCTOR != null)
                {
                    listServiceReqs = listServiceReqs.Where(o => o.EXECUTE_LOGINNAME == castFilter.LOGINNAME_DOCTOR).ToList();
                }
                listServiceReqs = listServiceReqs.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    skip = 0;
                    var listTreatmentIds = listServiceReqs.Select(s => s.TREATMENT_ID).Distinct().ToList();
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery presViewFilter = new HisServiceReqFilterQuery();
                        presViewFilter.REQUEST_ROOM_IDs = listRooms.Select(s => s.ROOM_ID).ToList();
                        presViewFilter.TREATMENT_IDs = listIds;
                        presViewFilter.HAS_EXECUTE = true;
                        presViewFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT };
                        listPresServiceReq.AddRange(new HisServiceReqManager(param).Get(presViewFilter) ?? new List<HIS_SERVICE_REQ>());

                        HisTreatmentFilterQuery tmFilter = new HisTreatmentFilterQuery();
                        tmFilter.IDs = listIds;
                        tmFilter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                        listTreatment.AddRange(new HisTreatmentManager(param).Get(tmFilter) ?? new List<HIS_TREATMENT>());
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

                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    var listSereServGroupByExecuteLoginnames = listServiceReqs.GroupBy(g => g.EXECUTE_LOGINNAME);
                    foreach (var loginname in listSereServGroupByExecuteLoginnames)
                    {
                        var rdo = new Mrs00476RDO();
                        rdo.LOGINNNAME = loginname.First().EXECUTE_LOGINNAME;
                        rdo.USERNAME = loginname.First().EXECUTE_USERNAME;

                        var pres = listPresServiceReq.Where(w => w.REQUEST_LOGINNAME == loginname.First().EXECUTE_LOGINNAME).ToList();
                        var tranpati = listTreatment.Where(o => o.DOCTOR_LOGINNAME == loginname.First().EXECUTE_LOGINNAME).ToList();
                        foreach (var room in listRooms)
                        {
                            var examServiceReq = new List<HIS_SERVICE_REQ>();
                            //kham tai phong: phong kham = phong
                            var exam = loginname.Where(o => o.EXECUTE_ROOM_ID == room.ROOM_ID).ToList();

                            if (this.castFilter.INPUT_DATA_IDs == null || this.castFilter.INPUT_DATA_IDs.Contains(presExam))
                            {
                                // kham ke don: (y lenh cua don la y lenh con cua kham) va bac si kham nay la bac si ke don cho benh nhan.
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && pres.Exists(p => p.PARENT_ID == o.ID)).ToList());
                            }
                            if (this.castFilter.INPUT_DATA_IDs == null || this.castFilter.INPUT_DATA_IDs.Contains(addExam))
                            {
                                //kham la kham them: co phong yeu cau la phong kham
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && listExamRooms.Exists(p => p.ROOM_ID == o.REQUEST_ROOM_ID)).ToList());
                            }
                            if (this.castFilter.INPUT_DATA_IDs == null || this.castFilter.INPUT_DATA_IDs.Contains(specExam))
                            {
                                //kham la noi tru kham chuyen khoa: co phong yeu cau la buong benh
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && listBedRooms.Exists(p => p.ROOM_ID == o.REQUEST_ROOM_ID)).ToList());
                            }
                            if (this.castFilter.INPUT_DATA_IDs == null || this.castFilter.INPUT_DATA_IDs.Contains(tranExam))
                            {
                                //kham chuyen vien: phong kham nay chuyen vien cho benh nhan va bac si kham nay la bac si ket luan cho benh nhan
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && tranpati.Exists(p => p.ID == o.TREATMENT_ID && o.EXECUTE_ROOM_ID == p.END_ROOM_ID)).ToList());
                            }
                            if (this.castFilter.EXAM_END_TYPE__PAUSE == true)
                            {
                                //kham ket thuc dieu tri: phong kham nay ket thuc dieu tri cho benh nhan va bac si kham nay la bac si ket luan cho benh nhan
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && o.EXAM_END_TYPE == examEndTypePause).ToList());
                            }
                            if (this.castFilter.EXAM_END_TYPE__INTREAT == true)
                            {
                                //kham nhap vien: phong kham nay nhap vien cho benh nhan
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && o.EXAM_END_TYPE == examEndTypeInTreat).ToList());
                            }
                            if (this.castFilter.EXAM_EXECUTING == true)
                            {
                                //dang kham: phong kham nay dang kham cho benh nhan
                                examServiceReq.AddRange(exam.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL).ToList());
                            }
                            examServiceReq = examServiceReq.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                            listDetail.AddRange(examServiceReq);
                            long count = examServiceReq.Count;

                            rdo.DIC_ROOM[room.ROOM_TAG] = count;
                            if (room.NUMBER == 1)
                            {
                                rdo.ROOM_1 = count;
                            }
                            else if (room.NUMBER == 2)
                            {
                                rdo.ROOM_2 = count;
                            }
                            else if (room.NUMBER == 3)
                            {
                                rdo.ROOM_3 = count;
                            }
                            else if (room.NUMBER == 4)
                            {
                                rdo.ROOM_4 = count;
                            }
                            else if (room.NUMBER == 5)
                            {
                                rdo.ROOM_5 = count;
                            }

                            else if (room.NUMBER == 6)
                            {
                                rdo.ROOM_6 = count;
                            }
                            else if (room.NUMBER == 7)
                            {
                                rdo.ROOM_7 = count;
                            }
                            else if (room.NUMBER == 8)
                            {
                                rdo.ROOM_8 = count;
                            }
                            else if (room.NUMBER == 9)
                            {
                                rdo.ROOM_9 = count;
                            }
                            else if (room.NUMBER == 10)
                            {
                                rdo.ROOM_10 = count;
                            }

                            else if (room.NUMBER == 11)
                            {
                                rdo.ROOM_11 = count;
                            }
                            else if (room.NUMBER == 12)
                            {
                                rdo.ROOM_12 = count;
                            }
                            else if (room.NUMBER == 13)
                            {
                                rdo.ROOM_13 = count;
                            }
                            else if (room.NUMBER == 14)
                            {
                                rdo.ROOM_14 = count;
                            }
                            else if (room.NUMBER == 15)
                            {
                                rdo.ROOM_15 = count;
                            }
                        }

                        var examEnd = loginname.Where(w => w.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (IsNotNullOrEmpty(examEnd))
                        {
                            rdo.TREATMENT_END_EXAM = examEnd.Where(w => !pres.Select(s => s.TREATMENT_ID).Contains(w.TREATMENT_ID)).Select(s => s.TREATMENT_ID).Distinct().Count(); //số bệnh nhân bác sĩ đó khám xong rồi nhưng chưa kê đơn
                            //rdo.SERE_SERV_END_EXAM = sereServEnd.Sum(s => s.AMOUNT); 
                        }
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
                var examServices = new HisServiceManager().Get(new HisServiceFilterQuery() { SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH })?? new List<HIS_SERVICE>();
                dicSingleTag.Add("DIS_SERVICE_IDs", examServices.ToDictionary(p=>p.ID.ToString(),q=>q.SERVICE_NAME));
                foreach (var room in listRooms)
                {
                    dicSingleTag.Add(room.ROOM_TAG, room.ROOM_NAME);
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo);
                objectTag.AddObjectData(store, "Detail", listDetail);
                objectTag.AddObjectData(store, "examRoom", listExamRooms);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
