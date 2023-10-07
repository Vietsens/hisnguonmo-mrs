using MOS.MANAGER.HisRoomType;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatmentEndType;

namespace MRS.Processor.Mrs00742
{
    class Mrs00742Processor : AbstractProcessor
    {
        List<Mrs00742RDO> ListRdo = new List<Mrs00742RDO>();
        Title Title = new Title();
        CommonParam paramGet = new CommonParam();
        const int MAX_EXAM_SERVICE_TYPE_NUM = 100;
        List<HIS_SERVICE_REQ> ListExamServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> ListExamSereServ = new List<HIS_SERE_SERV>();
        List<HIS_EXP_MEST> ListPrescription = new List<HIS_EXP_MEST>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<long> ListTreatmentIdRightRoute = new List<long>();
        List<HIS_EXECUTE_ROOM> listExcuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<HIS_EXECUTE_ROOM> listExamRoom = new List<HIS_EXECUTE_ROOM>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        //cac config
        long PatientTypeIdBhyt = 0;
        long PatientTypeIdFee = 0;
        long PatientTypeIdFree = 0;
        long PatientTypeIdKsk = 0;
        List<long> PatientTypeIdKsks = new List<long>();
        Mrs00742Filter filter = null;
        public Mrs00742Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00742Filter);
        }


        protected override bool GetData()
        {
            filter = ((Mrs00742Filter)reportFilter);
            bool result = true;
            try
            {
                PatientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                PatientTypeIdFee = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                PatientTypeIdFree = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
                PatientTypeIdKsk = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
                HisTreatmentEndTypeFilterQuery HisTreatmentEndTypefilter = new HisTreatmentEndTypeFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listHisTreatmentEndType = new HisTreatmentEndTypeManager().Get(HisTreatmentEndTypefilter);


                HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery()
                {
                    SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                    INTRUCTION_TIME_FROM = filter.TIME_FROM,
                    INTRUCTION_TIME_TO = filter.TIME_TO,
                    HAS_EXECUTE = true
                };

                Inventec.Common.Logging.LogSystem.Debug("start get ListExamServiceReq.");
                ListExamServiceReq = new HisServiceReqManager().Get(HisServiceReqfilter);

                Inventec.Common.Logging.LogSystem.Debug("finish get ListExamServiceReq.");
                HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery()
                {
                    TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH,
                    TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM,
                    TDL_INTRUCTION_TIME_TO = filter.TIME_TO,
                    HAS_EXECUTE = true
                };
                Inventec.Common.Logging.LogSystem.Debug("start get ListExamSereServ.");
                ListExamSereServ = new HisSereServManager().Get(HisSereServfilter);
                Inventec.Common.Logging.LogSystem.Debug("finish get ListExamSereServ.");

                Inventec.Common.Logging.LogSystem.Debug("start get ListTreatment.");
                ListTreatment = new ManagerSql().getTreatment(filter) ?? new List<HIS_TREATMENT>();
                ListTreatment = ListTreatment.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                var ListTreatmentRightRoute = new ManagerSql().getTreatmentRightRoute(filter) ?? new List<HIS_TREATMENT>();
                ListTreatmentIdRightRoute = ListTreatmentRightRoute.GroupBy(o => o.ID).Select(p => p.First().ID).ToList()?? new List<long>();
                Inventec.Common.Logging.LogSystem.Debug("finish get ListTreatment.");


                //đơn thuốc
                var HisPrescriptionViewfilter = new HisExpMestFilterQuery()
                {
                    TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM,
                    TDL_INTRUCTION_TIME_TO = filter.TIME_TO,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                };
                Inventec.Common.Logging.LogSystem.Debug("start get ListPrescriptionSub.");
                var ListPrescriptionSub = new HisExpMestManager(paramGet).Get(HisPrescriptionViewfilter);
                Inventec.Common.Logging.LogSystem.Debug("finish get ListPrescriptionSub.");
                ListPrescription.AddRange(ListPrescriptionSub);

                //Danh sách phòng xử lý
                listExcuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());

                //Danh sách khoa xử lý
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());

                //Danh sách phòng
                dicRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery()).ToDictionary(o => o.ID);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterBranch()
        {
            HisBranchFilterQuery filterBranch = new HisBranchFilterQuery();
            filterBranch.ID = filter.BRANCH_ID;
            listBranch = new HisBranchManager(paramGet).Get(filterBranch);

            if (filter.BRANCH_ID != null)
            {
                ListTreatment = ListTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                var treatmentIds = ListTreatment.Select(o => o.ID).ToList();
                ListExamServiceReq = ListExamServiceReq.Where(o => treatmentIds.Contains(o.TREATMENT_ID)).ToList();
                ListExamSereServ = ListExamSereServ.Where(o => treatmentIds.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
            }
        }
        private void FilterNotBedRoom()
        {

            ListExamServiceReq = ListExamServiceReq.Where(o => dicRoom.ContainsKey(o.REQUEST_ROOM_ID) && dicRoom[o.REQUEST_ROOM_ID].ROOM_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__BUONG).ToList();
            var treatmentIds = ListExamServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList();

            ListTreatment = ListTreatment.Where(o => treatmentIds.Contains(o.ID)).ToList();
            ListExamSereServ = ListExamSereServ.Where(o => treatmentIds.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {

                ListRdo.Clear();

                //lọc chi nhánh
                FilterBranch();

                //lọc bỏ yêu cầu khám từ buồng bệnh
                FilterNotBedRoom();
                if (filter.PATIENT_TYPE_CODE__KSK != null)
                {
                    var listCode = (filter.PATIENT_TYPE_CODE__KSK ?? " ").Split(',');
                    if (listCode != null)
                    {
                        PatientTypeIdKsks = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => listCode.Contains(o.PATIENT_TYPE_CODE)).Select(p => p.ID).ToList();
                    }
                }


                var listExamRoomId = ListExamServiceReq.Select(o => o.EXECUTE_ROOM_ID).Distinct().ToList();
                listExamRoom = listExcuteRoom.OrderBy(o => o.NUM_ORDER ?? 100).Where(o => listExamRoomId.Contains(o.ROOM_ID) && o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var listExamRoomCode = listExamRoom.OrderBy(o => o.NUM_ORDER ?? 100).Select(p => p.EXECUTE_ROOM_CODE).ToList();

                int count = 1;
                foreach (var room in listExamRoom)
                {
                    if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                    System.Reflection.PropertyInfo piRoom = typeof(Title).GetProperty("ROOM_NAME_" + count);
                    piRoom.SetValue(Title, room.EXECUTE_ROOM_NAME);
                    count++;
                }

                Inventec.Common.Logging.LogSystem.Debug("start gom theo ngay.");
                var goupByDate = ListExamServiceReq.OrderBy(p => p.INTRUCTION_DATE).GroupBy(o => o.INTRUCTION_DATE).ToList();
                ListRdo.Clear();


                foreach (var group in goupByDate)
                {
                    Mrs00742RDO rdo = new Mrs00742RDO();
                    var treatmentIds = group.Select(o => o.TREATMENT_ID).ToList();
                    var treatmentSubs = ListTreatment.Where(o => treatmentIds.Contains(o.ID)).ToList();
                    var treatmentIdNghiOms = treatmentSubs.Where(o => o.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM).Select(o => o.ID).ToList();
                    var treatmentIdTreEms = treatmentSubs.Where(o => o.TDL_PATIENT_DOB + 60000000000 > o.IN_TIME).Select(o => o.ID).ToList();
                    var treatmentIdNhapViens = treatmentSubs.Where(o => o.CLINICAL_IN_TIME > 0).Select(o => o.ID).ToList();
                    var treatmentIdChuyenViens = treatmentSubs.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Select(o => o.ID).ToList();
                    var serviceReqSub = group.ToList();
                    var serviceReqIds = group.Select(o => o.ID).ToList();
                    var sereServSub = ListExamSereServ.Where(o => serviceReqIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                    var serviceReqHasMedi = serviceReqSub.Where(p => ListPrescription.Exists(o => p.TREATMENT_ID == o.TDL_TREATMENT_ID && p.EXECUTE_ROOM_ID == o.REQ_ROOM_ID)).ToList();
                    //start so luot kham o cac phong
                    rdo.TOTAL = serviceReqSub.Count;
                    rdo.SICK = serviceReqSub.Where(o=>treatmentIdNghiOms.Contains(o.TREATMENT_ID)).Count();
                    rdo.HEIN = sereServSub.Count(o => o.PATIENT_TYPE_ID == PatientTypeIdBhyt);
                    rdo.FEE = sereServSub.Count(o => o.PATIENT_TYPE_ID == PatientTypeIdFee);
                    rdo.FREE = sereServSub.Count(o => o.PATIENT_TYPE_ID == PatientTypeIdFree);
                    rdo.KSKs = sereServSub.Count(o => PatientTypeIdKsks.Contains(o.PATIENT_TYPE_ID));
                    rdo.KSK = sereServSub.Count(o => o.PATIENT_TYPE_ID == PatientTypeIdKsk);
                    rdo.CHILD = serviceReqSub.Where(o=>treatmentIdTreEms.Contains(o.TREATMENT_ID)).Count();
                    rdo.HAS_MEDI = serviceReqHasMedi.Count();
                    rdo.CLN_HEIN = sereServSub.Count(o => treatmentIdNhapViens.Contains(o.TDL_TREATMENT_ID ?? 0) && o.PATIENT_TYPE_ID == PatientTypeIdBhyt);
                    rdo.CLN_FEE = sereServSub.Count(o => treatmentIdNhapViens.Contains(o.TDL_TREATMENT_ID ?? 0) && o.PATIENT_TYPE_ID == PatientTypeIdFee);
                    rdo.CLN_FREE = sereServSub.Count(o => treatmentIdNhapViens.Contains(o.TDL_TREATMENT_ID ?? 0) && o.PATIENT_TYPE_ID == PatientTypeIdFree);
                    rdo.CLN_BHYT = sereServSub.Count(o => treatmentIdNhapViens.Contains(o.TDL_TREATMENT_ID ?? 0) && o.PATIENT_TYPE_ID == PatientTypeIdBhyt);
                    rdo.CLN_VP = sereServSub.Count(o => treatmentIdNhapViens.Contains(o.TDL_TREATMENT_ID ?? 0) && o.PATIENT_TYPE_ID != PatientTypeIdBhyt);
                    rdo.TRAN_OUT = serviceReqSub.Where(o=>treatmentIdChuyenViens.Contains(o.TREATMENT_ID)).Count();
                    rdo.LEFT_LINE = serviceReqSub.Where(o=>ListTreatmentIdRightRoute.Contains(o.TREATMENT_ID)).Count();
                    rdo.MALE = serviceReqSub.Count(o=>o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE);
                    rdo.FEMALE = serviceReqSub.Count(o=>o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
                   
                    //finish so luot kham o cac phong

                    //start so benh nhan
                    rdo.T_TOTAL = treatmentSubs.Count();
                    rdo.T_SICK =treatmentIdNghiOms.Count ;
                    rdo.T_HEIN = treatmentSubs.Count(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt);
                    rdo.T_FEE = treatmentSubs.Count(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdFee);
                    rdo.T_FREE = treatmentSubs.Count(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdFree);
                    rdo.T_KSKs = treatmentSubs.Count(o => PatientTypeIdKsks.Contains(o.TDL_PATIENT_TYPE_ID ?? 0));
                    rdo.T_KSK = treatmentSubs.Count(o => o.TDL_PATIENT_TYPE_ID == PatientTypeIdKsk);
                    rdo.T_CHILD = treatmentSubs.Count(o => o.TDL_PATIENT_DOB + 60000000000 > o.IN_TIME);
                    rdo.T_HAS_MEDI = serviceReqHasMedi.Select(o => o.TREATMENT_ID).Distinct().Count();
                    rdo.T_CLN_HEIN = treatmentSubs.Count(o => o.CLINICAL_IN_TIME > 0 && o.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt);
                    rdo.T_CLN_FEE = treatmentSubs.Count(o => o.CLINICAL_IN_TIME > 0 && o.TDL_PATIENT_TYPE_ID == PatientTypeIdFee);
                    rdo.T_CLN_FREE = treatmentSubs.Count(o => o.CLINICAL_IN_TIME > 0 && o.TDL_PATIENT_TYPE_ID == PatientTypeIdFree);
                    rdo.T_TRAN_OUT = treatmentSubs.Count(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                    rdo.T_LEFT_LINE = treatmentSubs.Count(o => ListTreatmentIdRightRoute.Contains(o.ID));
                    rdo.IN_TIME = group.Key;
                    rdo.IN_DATE = group.Key;
                    rdo.T_MALE = treatmentSubs.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE);
                    rdo.T_FEMALE = treatmentSubs.Count(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE);
                    //phòng khám
                    count = 1;
                    foreach (var room in listExamRoom)
                    {
                        if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                        System.Reflection.PropertyInfo piTRoom = typeof(Mrs00742RDO).GetProperty("T_ROOM_" + count);
                        piTRoom.SetValue(rdo,serviceReqSub.Where(o=>o.EXECUTE_ROOM_ID == room.ROOM_ID).Select(p=>p.TREATMENT_ID).Distinct().Count());
                        System.Reflection.PropertyInfo piFRoom = typeof(Mrs00742RDO).GetProperty("F_ROOM_" + count);
                        piFRoom.SetValue(rdo, serviceReqSub.Where(o=>o.EXECUTE_ROOM_ID == room.ROOM_ID && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).Select(p=>p.TREATMENT_ID).Distinct().Count());

                        count++;
                    }
                    ListRdo.Add(rdo);
                }

                Inventec.Common.Logging.LogSystem.Debug("finish gom theo ngay.");

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            if (filter.TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            }
            if (filter.TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            }

            //phòng khám
            for (int count = 0; count < listExamRoom.Count; count++)
            {
                if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                var i = count + 1;
                System.Reflection.PropertyInfo piRoom = typeof(Title).GetProperty("ROOM_NAME_" + i);
                dicSingleTag.Add(string.Format("ROOM_{0}", count + 1), piRoom.GetValue(Title));

            }
            objectTag.AddObjectData(store, "Report", ListRdo);

        }

    }

}
