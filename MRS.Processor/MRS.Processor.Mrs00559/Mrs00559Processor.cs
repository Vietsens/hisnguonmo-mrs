using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisExecuteRoom;
using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00559
{
    public class Mrs00559Processor : AbstractProcessor
    {
        Mrs00559Filter castFilter = null;
        List<Mrs00559RDO> listExam = new List<Mrs00559RDO>();
        List<V_HIS_EXECUTE_ROOM> ExamRooms = new List<V_HIS_EXECUTE_ROOM>();
        Dictionary<long, DepaInAmount> dicDepaIn = new Dictionary<long, DepaInAmount>();

        List<long> PatientTypeIdBHs = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP1 ?? new List<long>();
        public Mrs00559Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00559Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            List<ServiceReqDO> listTemp = null;
            try
            {
                CommonParam getParam = new CommonParam();
                this.castFilter = (Mrs00559Filter)reportFilter;

                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.CREATE_TIME_FROM = castFilter.CREATE_TIME_FROM;
                serviceReqFilter.CREATE_TIME_TO = castFilter.CREATE_TIME_TO;
                serviceReqFilter.FINISH_TIME_FROM = castFilter.FINISH_TIME_FROM;
                serviceReqFilter.FINISH_TIME_TO = castFilter.FINISH_TIME_TO;
                serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.INTRUCTION_TIME_FROM;
                serviceReqFilter.INTRUCTION_TIME_TO = castFilter.INTRUCTION_TIME_TO;
                serviceReqFilter.HAS_EXECUTE = true;
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                serviceReqFilter.ORDER_DIRECTION = "ID";
                serviceReqFilter.ORDER_FIELD = "ASC";
                listTemp = new ManagerSql().GetServiceReq(serviceReqFilter, castFilter);
                if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                {
                    listTemp = listTemp.Where(p => castFilter.TDL_PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.BRANCH_IDs != null)
                {
                    listTemp = listTemp.Where(p => castFilter.BRANCH_IDs.Contains(p.BRANCH_ID ?? 0)).ToList();
                }

                if (castFilter.BRANCH_ID != null)
                {
                    listTemp = listTemp.Where(p =>p.BRANCH_ID  == castFilter.BRANCH_ID).ToList();
                }
                HisExecuteRoomViewFilterQuery exroom = new HisExecuteRoomViewFilterQuery();
                exroom.IS_EXAM = true;
                ExamRooms = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(param).GetView(exroom);

                dicDepaIn = HisDepartmentCFG.DEPARTMENTs.OrderBy(o => o.NUM_ORDER ?? 9999).ToDictionary(p => p.ID, q => new DepaInAmount() { DEPARTMENT_CODE = q.DEPARTMENT_CODE, DEPARTMENT_NAME = q.DEPARTMENT_NAME });

                if (listTemp != null)
                {
                    var roomIds = HisRoomCFG.HisRooms.Where(p => (p.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD || p.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__XL)).Select(p => p.ID).ToList();
                    listTemp = listTemp.Where(o => roomIds.Contains(o.REQUEST_ROOM_ID)).ToList();
                    var examRoomIds = ExamRooms.Where(p => p.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ROOM_ID).ToList();
                    listTemp = listTemp.Where(o => examRoomIds.Contains(o.EXECUTE_ROOM_ID)).ToList();
                }

                if (listTemp != null && this.castFilter.EXAM_ROOM_IDs != null && this.castFilter.EXAM_ROOM_IDs.Count > 0)
                    listTemp = listTemp.Where(o => this.castFilter.EXAM_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();

                ProcessExam(listTemp);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessExam(List<ServiceReqDO> listTemp)
        {
            try
            {
                List<Mrs00559RDO> listDetailExam = (from r in listTemp select new Mrs00559RDO(r)).ToList();
                var lastTemp = listTemp.OrderBy(o => o.FINISH_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                //Inventec.Common.Logging.LogSystem.Info("listDetailExam" + listDetailExam.Count); 
                Dictionary<long, Mrs00559RDO> dicGroupByRoomExam = new Dictionary<long, Mrs00559RDO>();
                foreach (var data in listDetailExam)
                {
                    bool IsLastRoomExam = lastTemp.Exists(o => o.ID == data.ID);
                    if (!dicGroupByRoomExam.ContainsKey(data.EXECUTE_ROOM_ID))
                    {
                        dicGroupByRoomExam[data.EXECUTE_ROOM_ID] = data;
                    }
                    Mrs00559RDO temp = dicGroupByRoomExam[data.EXECUTE_ROOM_ID];
                    temp.CountExam();
                    temp.CountFemaleExam(data.TDL_PATIENT_GENDER_ID);
                    temp.CountEmergencyExam(data.IS_EMERGENCY);
                    //temp.CountChildExam(data.TDL_PATIENT_DOB);
                    temp.CountTreatmentExam(data.REQUEST_ROOM_ID);
                    temp.CountMoveRoomExam(data.ID, listTemp);
                    temp.CountExamYhct(data);
                    temp.CountAgeExam(data.TDL_PATIENT_DOB);
                    temp.CountPatientType(data.ETHNIC_NAME);
                    if (IsLastRoomExam)
                    {
                        temp.CountInTreatExam(data, ref dicDepaIn, PatientTypeIdBHs);
                        temp.CountMediHomeExam(data.EXECUTE_ROOM_ID, data);
                        temp.CountTranpatiExam(data.EXECUTE_ROOM_ID, data);
                        temp.CountOutTreatExam(data);
                    }
                    temp.CountPatientTypeGroup(data.TDL_PATIENT_TYPE_ID ?? 0, PatientTypeIdBHs);
                }
                listExam = dicGroupByRoomExam.Values.OrderBy(r => r.EXECUTE_ROOM_ID).ToList();
                addInfo(ref listExam);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void addInfo(ref List<Mrs00559RDO> listExam)
        {
            try
            {
                //phong kham
                Dictionary<long, V_HIS_EXECUTE_ROOM> dicRoom = new Dictionary<long, V_HIS_EXECUTE_ROOM>();
                foreach (var item in ExamRooms)
                {
                    if (!dicRoom.ContainsKey(item.ROOM_ID)) dicRoom[item.ROOM_ID] = item;
                }
                //them thong tin phong kham
                List<CountWithPatientType> listExamFinish = new ManagerSql().CountExamFinish(castFilter);
                List<CountWithPatientType> listExamWait = new ManagerSql().CountExamWait(castFilter) ?? new List<CountWithPatientType>();
                foreach (var item in listExam)
                {
                    item.EXECUTE_ROOM_NAME = dicRoom.ContainsKey(item.EXECUTE_ROOM_ID) ? dicRoom[item.EXECUTE_ROOM_ID].EXECUTE_ROOM_NAME : "";
                    item.EXECUTE_DEPARTMENT_NAME = dicRoom.ContainsKey(item.EXECUTE_ROOM_ID) ? dicRoom[item.EXECUTE_ROOM_ID].DEPARTMENT_NAME : "";
                    item.COUNT_FINISH_EXAM = listExamFinish.Where(o => o.Id == item.EXECUTE_ROOM_ID).Sum(s => (s.CountBhyt ?? 0) + (s.CountVp ?? 0));
                    item.COUNT_WAIT_EXAM = listExamWait.Where(o => o.Id == item.EXECUTE_ROOM_ID).Sum(s => (s.CountBhyt ?? 0) + (s.CountVp ?? 0));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            dicSingleTag.Add("CREATE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.INTRUCTION_TIME_FROM ?? 0));

            dicSingleTag.Add("CREATE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.INTRUCTION_TIME_TO ?? 0));


            objectTag.AddObjectData(store, "Report", listExam);


            objectTag.AddObjectData(store, "DepaIn", dicDepaIn.Values.Where(o => o.AMOUNT > 0).ToList());
        }
    }
}
