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

namespace MRS.Processor.Mrs00386
{
    class Mrs00386Processor : AbstractProcessor
    {
        List<Mrs00386RDO> ListRdo = new List<Mrs00386RDO>();
        List<Mrs00386RDO> ListRdo1 = new List<Mrs00386RDO>();
        List<Mrs00386RDO> ListRdoInTreatment = new List<Mrs00386RDO>();
        Title Title = new Title();
        CommonParam paramGet = new CommonParam();
        const int MAX_EXAM_SERVICE_TYPE_NUM = 100;
        List<HIS_SERVICE_REQ> ListExamServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> ListExamServiceReq1 = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> ListExamSereServ = new List<HIS_SERE_SERV>();
        List<HIS_EXP_MEST> ListPrescription = new List<HIS_EXP_MEST>();
        List<SereServCat> ListSereServ = new List<SereServCat>();
        List<long> TreatmentIdKSK = new List<long>();
        List<HIS_REPORT_TYPE_CAT> ListReporttypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> ListTreatment1 = new List<HIS_TREATMENT>();
        List<HIS_TREATMENT> ListTreatmentTransfer1 = new List<HIS_TREATMENT>();
        List<HIS_TREATMENT> ListTreatmentCln1 = new List<HIS_TREATMENT>();
        List<PatientTypeAlter> ListTreatment = new List<PatientTypeAlter>();
        List<PatientTypeAlter> ListTreatmentCln = new List<PatientTypeAlter>();
        List<PatientTypeAlter> ListTreatmentTransfer = new List<PatientTypeAlter>();
        List<HIS_EXECUTE_ROOM> listExcuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<HIS_EXECUTE_ROOM> listExamRoom = new List<HIS_EXECUTE_ROOM>();
        List<long> ListServiceId = new List<long>();
        private List<PatientTypeAlter> LisPatientTypeAlter = new List<PatientTypeAlter>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();
        List<SereServType> ListSereServType = null;
        List<DepartmentInOut> DepartmentInOuts = new List<DepartmentInOut>();
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<SereServPrice> ListSereServBhyt = new List<SereServPrice>();
        List<SereServPrice> ListSereServVp = new List<SereServPrice>();
        List<SereServPrice> ListSereServKsk = new List<SereServPrice>();
        //cac config
        long PatientTypeIdBhyt = 0;
        long PatientTypeIdFee = 0;
        long PatientTypeIdFree = 0;
        long PatientTypeIdKsk = 0;
        List<long> PatientTypeIdKsks = new List<long>();
        Mrs00386Filter filter = null;
        public Mrs00386Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00386Filter);
        }


        protected override bool GetData()
        {
            filter = ((Mrs00386Filter)reportFilter);
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
                //Đối tượng điều trị
                Inventec.Common.Logging.LogSystem.Debug("start get patientTypeAlter.");
                LisPatientTypeAlter = new ManagerSql().GetPatientTypeAlter(filter);
                Inventec.Common.Logging.LogSystem.Debug("finish get patientTypeAlter.");
                //Vào viện
                ListTreatment = LisPatientTypeAlter.Where(o => o.IN_TIME < filter.TIME_TO && o.IN_TIME >= filter.TIME_FROM).GroupBy(p => p.TREATMENT_ID).Select(q => q.First()).ToList();
                //Vào nằm viện
                ListTreatmentCln = LisPatientTypeAlter.Where(o => o.CLINICAL_IN_TIME < filter.TIME_TO && o.CLINICAL_IN_TIME >= filter.TIME_FROM).GroupBy(p => p.TREATMENT_ID).Select(q => q.First()).ToList();

                //Chuyển viện
                ListTreatmentTransfer = LisPatientTypeAlter.Where(o => o.OUT_TIME < filter.TIME_TO && o.OUT_TIME >= filter.TIME_FROM && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).GroupBy(p => p.TREATMENT_ID).Select(q => q.First()).ToList();
                //
                if (filter.BRANCH_ID != null)
                {
                    ListTreatment = ListTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    ListTreatmentCln = ListTreatmentCln.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    ListTreatmentTransfer = ListTreatmentTransfer.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                var listTreatmentId = ListTreatment.Select(o => o.ID).ToList();

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

                ListTreatment1 = new ManagerSql().getTreatment(filter) ?? new List<HIS_TREATMENT>();
                ListTreatmentTransfer1 = ListTreatment1.Where(o => o.OUT_TIME < filter.TIME_TO && o.OUT_TIME >= filter.TIME_FROM && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).GroupBy(p => p.ID).Select(q => q.First()).ToList();
                ListTreatmentCln1 = ListTreatment1.Where(o => o.CLINICAL_IN_TIME < filter.TIME_TO && o.CLINICAL_IN_TIME >= filter.TIME_FROM).GroupBy(p => p.ID).Select(q => q.First()).ToList();
                if (filter.BRANCH_ID != null)
                {
                    ListTreatment1 = ListTreatment1.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    ListTreatmentCln1 = ListTreatmentCln1.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    ListTreatmentTransfer1 = ListTreatmentTransfer1.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                ListExamServiceReq1 = ListExamServiceReq.Where(o => ListTreatment1.Exists(p => p.ID == o.TREATMENT_ID) && ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0 || p.DISCOUNT > 0))).ToList();

                ListTreatment = ListTreatment.Where(o => ListExamServiceReq.Exists(p => p.TREATMENT_ID == o.ID && ListExamSereServ.Exists(q => q.SERVICE_REQ_ID == p.ID && (q.VIR_HEIN_PRICE > 0 || q.VIR_PATIENT_PRICE > 0 || q.DISCOUNT > 0)))).ToList();
                ListExamServiceReq = ListExamServiceReq.Where(o => ListTreatment.Exists(p => p.ID == o.TREATMENT_ID) && ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0 || p.DISCOUNT > 0))).ToList();
                

                //ID treatment KSK
                Inventec.Common.Logging.LogSystem.Debug("start get TreatmentIdKSK.");
                TreatmentIdKSK = new ManagerSql().GetTreatmentIdKsk(filter, HisServiceCFG.getList_SERVICE_CODE__KSK);
                Inventec.Common.Logging.LogSystem.Debug("finish get TreatmentIdKSK.");

                //đơn thuốc
                var HisPrescriptionViewfilter = new HisExpMestFilterQuery()
                {
                    TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM,
                    TDL_INTRUCTION_TIME_TO = filter.TIME_TO
                };
                Inventec.Common.Logging.LogSystem.Debug("start get ListPrescriptionSub.");
                var ListPrescriptionSub = new HisExpMestManager(paramGet).Get(HisPrescriptionViewfilter);
                Inventec.Common.Logging.LogSystem.Debug("finish get ListPrescriptionSub.");
                ListPrescription.AddRange(ListPrescriptionSub);

                HisReportTypeCatFilterQuery CatFilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00386"
                };
                ListReporttypeCat = new HisReportTypeCatManager(paramGet).Get(CatFilter);

                Inventec.Common.Logging.LogSystem.Debug("start get ListSereServ.");
                ListSereServ = new ManagerSql().GetSereServCat(filter);
                Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServ.");
                if (IsNotNullOrEmpty(ListReporttypeCat))
                {
                    ListReporttypeCat = ListReporttypeCat.Where(p => ListSereServ.Exists(q => q.REPORT_TYPE_CAT_ID == p.ID)).OrderBy(o => o.NUM_ORDER ?? 100).ToList();
                }

                //Danh sách phòng xử lý
                listExcuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());

                //Danh sách khoa xử lý
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());

                //Danh sách phòng
                dicRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery()).ToDictionary(o => o.ID);

                //Tong so dich vu theo loai
                if (filter.IS_EXTRA == true)
                {
                    Inventec.Common.Logging.LogSystem.Debug("start get ListSereServType.");
                    ListSereServType = new ManagerSql().GetSereServType(filter);
                    Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServType.");
                    Inventec.Common.Logging.LogSystem.Debug("start get DepartmentInOuts.");
                    DepartmentInOuts = new ManagerSql().GetDepartmentTran(filter);
                    Inventec.Common.Logging.LogSystem.Debug("finish get DepartmentInOuts.");
                    if (filter.PATIENT_TYPE_CODE__KSK != null)
                    {
                        var listCode = (filter.PATIENT_TYPE_CODE__KSK ?? " ").Split(',');
                        if (listCode != null)
                        {
                            List<long> listId = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => listCode.Contains(o.PATIENT_TYPE_CODE)).Select(p => p.ID).ToList();
                            Inventec.Common.Logging.LogSystem.Debug("start get ListSereServKsk.");
                            ListSereServKsk = new ManagerSql().GetSereServKsk(filter, listId);
                            Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServKsk.");
                        }
                    }
                    else if (HisPatientTypeCFG.PATIENT_TYPE_ID__KSK >= 0)
                    {
                        Inventec.Common.Logging.LogSystem.Debug("start get ListSereServKsk.");
                        ListSereServKsk = new ManagerSql().GetSereServKsk(filter);
                        Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServKsk.");
                    }
                    if (HisPatientTypeCFG.PATIENT_TYPE_ID__FEE >= 0)
                    {
                        Inventec.Common.Logging.LogSystem.Debug("start get ListSereServVp.");
                        ListSereServVp = new ManagerSql().GetSereServVp(filter);
                        Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServVp.");
                    }
                    if (HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT >= 0)
                    {
                        Inventec.Common.Logging.LogSystem.Debug("start get ListSereServBhyt.");
                        ListSereServBhyt = new ManagerSql().GetSereServBhyt(filter);
                        Inventec.Common.Logging.LogSystem.Debug("finish get ListSereServBhyt.");
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
            var result = true;
            try
            {

                ListRdo.Clear();
                filter = ((Mrs00386Filter)reportFilter);

                ListExamServiceReq = ListExamServiceReq.Where(o => dicRoom.ContainsKey(o.REQUEST_ROOM_ID) && dicRoom[o.REQUEST_ROOM_ID].ROOM_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__BUONG).ToList();
                // cac loai benh nhan
                Inventec.Common.Logging.LogSystem.Debug("start cac loai benh nhan");
                //BN khám
                var ListTreatmentExam = ListTreatment.Where(o => ListExamServiceReq.Select(p => p.TREATMENT_ID).ToList().Contains(o.ID)).ToList();
                HisBranchFilterQuery filterBranch = new HisBranchFilterQuery();
                filterBranch.ID = filter.BRANCH_ID;
                var Branch = new HisBranchManager(paramGet).Get(filterBranch);
                if (Branch == null)
                {
                    Inventec.Common.Logging.LogSystem.Error("Khong lay duoc chi nhanh");
                    return false;
                }

                //Chuyển tuyến
                var listTreatmentIdTRAN_PATI = ListTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ID).ToList();
                //BN Thông tuyến

                var listTreatmentIdRoute = IsNotNullOrEmpty(LisPatientTypeAlter) ? LisPatientTypeAlter.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && !Branch.Select(q => q.HEIN_MEDI_ORG_CODE ?? "").ToList().Contains(o.HEIN_MEDI_ORG_CODE)).Select(p => p.TREATMENT_ID).ToList() : null;

                // khám
                //BN khám BHYT
                var ListExamServiceReqIdBHYT = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == PatientTypeIdBhyt).Select(p => p.TREATMENT_ID).ToList();
                //BN khám nghỉ ốm
                var ListExamServiceReqIdSick = ListExamServiceReq.Where(o => IsSick(o, ListTreatment)).Select(p => p.TREATMENT_ID).ToList();
                //BN khám viện phí
                var ListExamServiceReqIdDV = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == PatientTypeIdFee).Select(p => p.TREATMENT_ID).ToList();
                //BN khám mien phí
                var ListExamServiceReqIdFree = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == PatientTypeIdFree).Select(p => p.TREATMENT_ID).ToList();
                
                if (filter.PATIENT_TYPE_CODE__KSK != null)
                {
                    var listCode = (filter.PATIENT_TYPE_CODE__KSK ?? " ").Split(',');
                    if (listCode != null)
                    {
                        PatientTypeIdKsks = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => listCode.Contains(o.PATIENT_TYPE_CODE)).Select(p => p.ID).ToList();
                    }
                }
                //BN khám suc khoe
                var ListExamServiceReqIdKsk = ListExamServiceReq.Where(o => patientTypeId(o, LisPatientTypeAlter) == PatientTypeIdKsk || PatientTypeIdKsks.Contains(patientTypeId(o, LisPatientTypeAlter))).Select(p => p.TREATMENT_ID).ToList();

                //BN KSK
                var ListExamSereServIdSK = TreatmentIdKSK;
                //BN nhập viện
                var ListTreatmentIdIN = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) || treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)).Select(p => p.ID).ToList();
                //BN khám có đơn thuốc
                var ListExamServiceReqIdHasMedi = ListExamServiceReq.Where(o => ListPrescription.Exists(p => o.TREATMENT_ID == p.TDL_TREATMENT_ID && o.EXECUTE_ROOM_ID == p.REQ_ROOM_ID)).Select(p => p.TREATMENT_ID).ToList();
                //

                // theo 192
                
                var listTreatmentIdBHYT = ListTreatment1.Where(p =>  p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.ID).ToList();
                
                var listTreatmentIdSick = ListTreatment1.Where(p => p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && p.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM).Select(p => p.ID).ToList();
                
                var listTreatmentIdDV = ListTreatment1.Where(p =>  p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Select(p => p.ID).ToList();
                
                var listTreatmentIdFree = ListTreatment1.Where(p =>  p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE).Select(p => p.ID).ToList();
                
                var listTreatmentIdKSK = ListTreatment1.Where(p =>  ListExamServiceReq.Exists(q => q.TREATMENT_ID == p.ID && HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == q.EXECUTE_ROOM_ID).ROOM_NAME == "Phòng Khám 5")).Select(p => p.ID).ToList();
                
                var listTreatmentEndIdIn = ListTreatment1.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(p => p.ID).ToList();

                
                var treatmentEndId = ListTreatment1.Select(p => p.ID).ToList();
                var listTreatmentIdHasMedi = new List<long>();
                if (treatmentEndId != null)
                {
                    listTreatmentIdHasMedi = ListExamServiceReq.Where(o => ListPrescription.Exists(p => treatmentEndId.Contains(p.TDL_TREATMENT_ID ?? 0) && o.EXECUTE_ROOM_ID == p.REQ_ROOM_ID)).Select(p => p.TREATMENT_ID).ToList();
                }

                

                var listTreatmentEndIdRoute = IsNotNullOrEmpty(LisPatientTypeAlter) ? LisPatientTypeAlter.Where(o => treatmentEndId.Contains(o.TREATMENT_ID) && o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && !Branch.Select(q => q.HEIN_MEDI_ORG_CODE ?? "").ToList().Contains(o.HEIN_MEDI_ORG_CODE)).Select(p => p.TREATMENT_ID).ToList() : null;
                //

                
                Inventec.Common.Logging.LogSystem.Debug("finish cac loai benh nhan.");
                //

                // DS Phòng khám
                listExamRoom = listExcuteRoom.OrderBy(o => o.NUM_ORDER ?? 100).Where(o => ListExamServiceReq.Exists(p => p.EXECUTE_ROOM_ID == o.ROOM_ID) && o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var listExamRoomCode = listExamRoom.OrderBy(o => o.NUM_ORDER ?? 100).Select(p => p.EXECUTE_ROOM_CODE).ToList();
                //
                // Nhóm loại báo cáo
                int count = 1;
                foreach (var parent in ListReporttypeCat)
                {
                    if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                    System.Reflection.PropertyInfo piParent = typeof(Title).GetProperty("PARENT_NAME_" + count);
                    piParent.SetValue(Title, parent.CATEGORY_NAME);
                    count++;
                }
                //
                // phòng khám
                count = 1;
                foreach (var room in listExamRoom)
                {
                    if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                    System.Reflection.PropertyInfo piRoom = typeof(Title).GetProperty("ROOM_NAME_" + count);
                    piRoom.SetValue(Title, room.EXECUTE_ROOM_NAME);
                    count++;
                }
                //

                var treatmentIn = ListTreatment.Where(o => treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) || treatmentTypeId(o, LisPatientTypeAlter).Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)).ToList();
                if (IsNotNullOrEmpty(treatmentIn))
                {
                    foreach (var item in treatmentIn)
                    {
                        Mrs00386RDO rdo = new Mrs00386RDO(item);
                        count = 1;
                        foreach (var room in listExamRoom)
                        {
                            System.Reflection.PropertyInfo piCRoom = typeof(Mrs00386RDO).GetProperty("C_ROOM_" + count);
                            if (getListCRoomId(item).Contains(room.ROOM_ID)) piCRoom.SetValue(rdo,1);
                            count++;
                        }
                        rdo.IN_DATE = item.IN_DATE;
                        //rdo.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IN_DATE);
                        ListRdo.Add(rdo);

                    }
                }

                var treatmentIn1 = ListTreatment1.Where(o => o.TDL_TREATMENT_TYPE_ID == (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) || o.TDL_TREATMENT_TYPE_ID == (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)).ToList();
                if (IsNotNullOrEmpty(treatmentIn1))
                {
                    foreach (var item in treatmentIn1)
                    {
                        Mrs00386RDO rdo = new Mrs00386RDO(item);
                        count = 1;
                        foreach (var room in listExamRoom)
                        {
                            System.Reflection.PropertyInfo piCRoom = typeof(Mrs00386RDO).GetProperty("C_ROOM_" + count);
                            if (getListCRoomId1(item).Contains(room.ROOM_ID)) piCRoom.SetValue(rdo, 1);
                            count++;
                        }
                        rdo.IN_DATE = item.IN_DATE;
                        //rdo.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IN_DATE);
                        ListRdo1.Add(rdo);

                    }
                }
                    //var groupByDate = ListRdoInTreatment.OrderBy(p => p.IN_DATE).GroupBy(o => o.IN_DATE).ToList();
                    //ListRdoInTreatment.Clear();
                    //foreach (var group in groupByDate)
                    //{
                    //    Mrs00386RDO rdo = new Mrs00386RDO();
                    //    List<Mrs00386RDO> listSub = group.ToList<Mrs00386RDO>();
                    //    rdo.C_ROOM_1 = listSub.Sum(x => x.C_ROOM_1);
                    //    rdo.C_ROOM_2 = listSub.Sum(x => x.C_ROOM_2);
                    //    rdo.C_ROOM_3 = listSub.Sum(x => x.C_ROOM_3);
                    //    rdo.C_ROOM_4 = listSub.Sum(x => x.C_ROOM_4);
                    //    rdo.C_ROOM_5 = listSub.Sum(x => x.C_ROOM_5);
                    //    rdo.C_ROOM_6 = listSub.Sum(x => x.C_ROOM_6);
                    //    rdo.C_ROOM_7 = listSub.Sum(x => x.C_ROOM_7);
                    //    rdo.C_ROOM_8 = listSub.Sum(x => x.C_ROOM_8);
                    //    rdo.C_ROOM_9 = listSub.Sum(x => x.C_ROOM_9);
                    //    rdo.C_ROOM_10 = listSub.Sum(x => x.C_ROOM_10);
                    //    rdo.C_ROOM_11 = listSub.Sum(x => x.C_ROOM_11);
                    //    rdo.C_ROOM_12 = listSub.Sum(x => x.C_ROOM_12);
                    //    rdo.C_ROOM_13 = listSub.Sum(x => x.C_ROOM_13);
                    //    rdo.C_ROOM_14 = listSub.Sum(x => x.C_ROOM_14);
                    //    rdo.C_ROOM_15 = listSub.Sum(x => x.C_ROOM_15);
                    //    rdo.C_ROOM_16 = listSub.Sum(x => x.C_ROOM_16);
                    //    rdo.C_ROOM_17 = listSub.Sum(x => x.C_ROOM_17);
                    //    rdo.C_ROOM_18 = listSub.Sum(x => x.C_ROOM_18);
                    //    rdo.C_ROOM_19 = listSub.Sum(x => x.C_ROOM_19);
                    //    rdo.C_ROOM_20 = listSub.Sum(x => x.C_ROOM_20);
                    //    rdo.C_ROOM_21 = listSub.Sum(x => x.C_ROOM_21);
                    //    rdo.IN_DATE = listSub.First().IN_DATE;
                    //    ListRdo.Add(rdo);
                //}

                // cách lấy cũ
                if (IsNotNullOrEmpty(ListTreatmentExam))
                {
                    // Tao listrdo
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo.");
                    foreach (var treatment in ListTreatmentExam)
                    {
                        Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        //so luot kham o cac phong
                        if (filter.IS_COUNT_EXAM != false)
                        {
                            rdo.TOTAL = ListExamServiceReq.Count(o => o.TREATMENT_ID == treatment.ID);
                            rdo.SICK = ListExamServiceReqIdSick.Count(o => o == treatment.ID);
                            rdo.HEIN = ListExamServiceReqIdBHYT.Count(o => o == treatment.ID);
                            rdo.FEE = ListExamServiceReqIdDV.Count(o => o == treatment.ID);
                            rdo.FREE = ListExamServiceReqIdFree.Count(o => o == treatment.ID);
                            rdo.KSKs = ListExamServiceReqIdKsk.Count(o => o == treatment.ID);
                            rdo.KSK = ListExamSereServIdSK.Count(o => o == treatment.ID);
                            rdo.CHILD = (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB))) ? rdo.TOTAL : 0;
                            rdo.HAS_MEDI = ListExamServiceReqIdHasMedi.Count(o => o == treatment.ID);
                            rdo.CLN_HEIN = ListTreatmentIdIN.Contains(treatment.ID) ? ListExamServiceReqIdBHYT.Count(o => o == treatment.ID) : 0;
                            rdo.CLN_FEE = ListTreatmentIdIN.Contains(treatment.ID) ? ListExamServiceReqIdDV.Count(o => o == treatment.ID) : 0;
                            rdo.CLN_FREE = ListTreatmentIdIN.Contains(treatment.ID) ? ListExamServiceReqIdFree.Count(o => o == treatment.ID) : 0;
                            rdo.TRAN_OUT = listTreatmentIdTRAN_PATI.Contains(treatment.ID) ? rdo.TOTAL : 0;
                            rdo.LEFT_LINE = listTreatmentIdRoute.Contains(treatment.ID) ? rdo.TOTAL : 0;
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.MALE = ListExamServiceReq.Count(o => o.TREATMENT_ID == treatment.ID);
                            }
                            else
                            {
                                rdo.FEMALE = ListExamServiceReq.Count(o => o.TREATMENT_ID == treatment.ID);
                            }
                        }
                        if (filter.IS_COUNT_EXAM != true)
                        {
                            rdo.T_TOTAL = 1;
                            rdo.T_SICK = ListExamServiceReqIdSick.Contains(treatment.ID) ? 1 : 0; ;
                            rdo.T_HEIN = ListExamServiceReqIdBHYT.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_FEE = ListExamServiceReqIdDV.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_FREE = ListExamServiceReqIdFree.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_KSKs = ListExamServiceReqIdKsk.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_KSK = ListExamSereServIdSK.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_CHILD = (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB))) ? 1 : 0;
                            rdo.T_HAS_MEDI = ListExamServiceReqIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_CLN_HEIN = ListExamServiceReqIdBHYT.Contains(treatment.ID) && ListTreatmentIdIN.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_CLN_FEE = ListExamServiceReqIdDV.Contains(treatment.ID) && ListTreatmentIdIN.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_CLN_FREE = ListExamServiceReqIdFree.Contains(treatment.ID) && ListTreatmentIdIN.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_TRAN_OUT = listTreatmentIdTRAN_PATI.Contains(treatment.ID) ? 1 : 0;
                            rdo.T_LEFT_LINE = listTreatmentIdRoute.Contains(treatment.ID) ? 1 : 0;

                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.MALE = 1;
                            }
                            else
                            {
                                rdo.FEMALE = 1;
                            }
                        }
                        else
                        {

                        }

                        
                        
                        //kết thúc công khám không tích hao phí
                        rdo.KH_TOTAL = ListTreatment1.Exists(P => P.ID == treatment.ID) ? 1 : 0;
                        rdo.KH_SICK = listTreatmentIdSick.Contains(treatment.ID) ? 1 : 0; ;
                        rdo.KH_HEIN = listTreatmentIdBHYT.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_FEE = listTreatmentIdDV.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_FREE = listTreatmentIdFree.Contains(treatment.ID) ? 1 : 0;
                        //rdo.KH_KSKs = listExamEndIdBHYT.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_KSK = listTreatmentIdKSK.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CHILD = (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB))) ? 1 : 0;
                        rdo.KH_HAS_MEDI = listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_HEIN = listTreatmentIdBHYT.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_FEE = listTreatmentIdDV.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_FREE = listTreatmentIdFree.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_TRAN_OUT = listTreatmentIdTRAN_PATI.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_LEFT_LINE = listTreatmentEndIdRoute.Contains(treatment.ID) ? 1 : 0;

                        rdo.IN_TIME = treatment.IN_TIME;
                        //phòng khám
                        count = 1;
                        
                        foreach (var room in listExamRoom)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            if (filter.IS_COUNT_EXAM != false)
                            {
                                System.Reflection.PropertyInfo piRoom = typeof(Mrs00386RDO).GetProperty("ROOM_" + count);
                                if (getListExamRoomId(treatment.ID, ListExamServiceReq).Contains(room.ROOM_ID)) piRoom.SetValue(rdo, 1);
                            }
                            //System.Reflection.PropertyInfo piFRoom = typeof(Mrs00386RDO).GetProperty("F_ROOM_" + count);
                            //if (getListFRoomId(treatment.ID).Contains(room.ROOM_ID)) piFRoom.SetValue(rdo, 1);
                            if (filter.IS_COUNT_EXAM != true)
                            {
                                System.Reflection.PropertyInfo piTRoom = typeof(Mrs00386RDO).GetProperty("T_ROOM_" + count);
                                if (getListTRoomId(treatment).Contains(room.ROOM_ID)) piTRoom.SetValue(rdo, 1);
                            }

                            
                            count++;
                        }
                        ListRdo.Add(rdo);
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo.");
                    //
                    // Tao listrdo
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo CLN.");
                    if (IsNotNullOrEmpty(ListTreatmentCln))
                    {

                        foreach (var treatment in ListTreatmentCln)
                        {
                            Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;

                            rdo.IN_TIME = treatment.CLINICAL_IN_TIME ?? 0;
                            rdo.IN_DATE = (treatment.CLINICAL_IN_TIME ?? 0) - (treatment.CLINICAL_IN_TIME ?? 0) % 1000000;

                            rdo.CLN_BHYT = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt ? 1 : 0;
                            rdo.CLN_VP = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdFee ? 1 : 0;
                            rdo.CLN_MP = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdFree ? 1 : 0;

                            ListRdo.Add(rdo);
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo CLN.");
                    //
                    // Tao listrdo Transfer
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo tranfer.");
                    if (IsNotNullOrEmpty(ListTreatmentTransfer))
                    {

                        foreach (var treatment in ListTreatmentTransfer)
                        {
                            Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;

                            rdo.IN_TIME = treatment.OUT_TIME ?? 0;
                            rdo.IN_DATE = (treatment.OUT_TIME ?? 0) - (treatment.OUT_TIME ?? 0) % 1000000;

                            rdo.TRANSFER_OUT = 1;

                            ListRdo.Add(rdo);
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo tranfer.");
                    //
                    // gom theo ngay

                    Inventec.Common.Logging.LogSystem.Debug("start gom theo ngay.");
                    var goupByDate = ListRdo.OrderBy(p => p.IN_DATE).GroupBy(o => o.IN_DATE).ToList();
                    ListRdo.Clear();
                    
                    
                    foreach (var group in goupByDate)
                    {
                        List<Mrs00386RDO> listSub = group.ToList<Mrs00386RDO>();
                        Mrs00386RDO rdo = new Mrs00386RDO(listSub.First());
                        rdo.IN_DATE = listSub.First().IN_DATE;
                        rdo.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSub.First().IN_DATE);
                        rdo.TOTAL = listSub.Sum(o => o.TOTAL);
                        rdo.SICK = listSub.Sum(o => o.SICK);
                        rdo.HEIN = listSub.Sum(o => o.HEIN);
                        rdo.FEE = listSub.Sum(o => o.FEE);
                        rdo.FREE = listSub.Sum(o => o.FREE);
                        rdo.KSK = listSub.Sum(o => o.KSK);
                        rdo.KSKs = listSub.Sum(o => o.KSKs);
                        rdo.CHILD = listSub.Sum(o => o.CHILD);
                        rdo.HAS_MEDI = listSub.Sum(o => o.HAS_MEDI);
                        rdo.IN_TIME = listSub.First().IN_TIME;
                        
                        //Nhóm loại báo cáo
                        count = 1;
                        foreach (var parent in ListReporttypeCat)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            System.Reflection.PropertyInfo piParent = typeof(Mrs00386RDO).GetProperty("PARENT_" + count);

                            piParent.SetValue(rdo, TotalParent(piParent, rdo.IN_TIME, parent.ID));

                            
                            count++;
                        }
                        LogSystem.Info("a");
                        //phòng khám
                        count = 1;
                        foreach (var room in listExamRoom)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            System.Reflection.PropertyInfo piRoom = typeof(Mrs00386RDO).GetProperty("ROOM_" + count);
                            piRoom.SetValue(rdo, TotalRoom(piRoom, listSub));
                            System.Reflection.PropertyInfo piFRoom = typeof(Mrs00386RDO).GetProperty("F_ROOM_" + count);
                            piFRoom.SetValue(rdo, TotalRoom(piFRoom, listSub));
                            System.Reflection.PropertyInfo piTRoom = typeof(Mrs00386RDO).GetProperty("T_ROOM_" + count);
                            piTRoom.SetValue(rdo, TotalRoom(piTRoom, listSub));
                            
                            count++;
                        }
                        rdo.CLN_HEIN = listSub.Sum(o => o.CLN_HEIN);
                        rdo.CLN_FEE = listSub.Sum(o => o.CLN_FEE);
                        rdo.CLN_FREE = listSub.Sum(o => o.CLN_FREE);
                        rdo.CLN_BHYT = listSub.Sum(o => o.CLN_BHYT);
                        rdo.CLN_VP = listSub.Sum(o => o.CLN_VP);
                        rdo.CLN_MP = listSub.Sum(o => o.CLN_MP);
                        rdo.TRAN_OUT = listSub.Sum(o => o.TRAN_OUT);
                        rdo.TRANSFER_OUT = listSub.Sum(o => o.TRANSFER_OUT);
                        rdo.LEFT_LINE = listSub.Sum(o => o.LEFT_LINE);
                        //tong theo ho so dieu tri

                        rdo.T_TOTAL = listSub.Sum(o => o.T_TOTAL);
                        rdo.T_SICK = listSub.Sum(o => o.T_SICK);
                        rdo.T_HEIN = listSub.Sum(o => o.T_HEIN);
                        rdo.T_FEE = listSub.Sum(o => o.T_FEE);
                        rdo.T_FREE = listSub.Sum(o => o.T_FREE);
                        rdo.T_KSK = listSub.Sum(o => o.T_KSK);
                        rdo.T_KSKs = listSub.Sum(o => o.T_KSKs);
                        rdo.T_CHILD = listSub.Sum(o => o.T_CHILD);
                        rdo.T_HAS_MEDI = listSub.Sum(o => o.T_HAS_MEDI);
                        rdo.T_CLN_HEIN = listSub.Sum(o => o.T_CLN_HEIN);
                        rdo.T_CLN_FEE = listSub.Sum(o => o.T_CLN_FEE);
                        rdo.T_CLN_FREE = listSub.Sum(o => o.T_CLN_FREE);
                        rdo.T_TRAN_OUT = listSub.Sum(o => o.T_TRAN_OUT);
                        rdo.T_LEFT_LINE = listSub.Sum(o => o.T_LEFT_LINE);

                        rdo.KH_TOTAL = listSub.Sum(o => o.KH_TOTAL);
                        rdo.KH_SICK = listSub.Sum(o => o.KH_SICK);
                        rdo.KH_HEIN = listSub.Sum(o => o.KH_HEIN);
                        rdo.KH_FEE = listSub.Sum(o => o.KH_FEE);
                        rdo.KH_FREE = listSub.Sum(o => o.KH_FREE);
                        rdo.KH_KSK = listSub.Sum(o => o.KH_KSK);
                        rdo.KH_KSKs = listSub.Sum(o => o.KH_KSKs);
                        rdo.KH_CHILD = listSub.Sum(o => o.KH_CHILD);
                        rdo.KH_HAS_MEDI = listSub.Sum(o => o.KH_HAS_MEDI);
                        rdo.KH_CLN_HEIN = listSub.Sum(o => o.KH_CLN_HEIN);
                        rdo.KH_CLN_FEE = listSub.Sum(o => o.KH_CLN_FEE);
                        rdo.KH_CLN_FREE = listSub.Sum(o => o.KH_CLN_FREE);
                        rdo.KH_TRAN_OUT = listSub.Sum(o => o.KH_TRAN_OUT);
                        rdo.KH_LEFT_LINE = listSub.Sum(o => o.KH_LEFT_LINE);
                        //tổng số bệnh nhân vào viện
                        rdo.C_ROOM_1 = listSub.Sum(x => x.C_ROOM_1);
                        rdo.C_ROOM_2 = listSub.Sum(x => x.C_ROOM_2);
                        rdo.C_ROOM_3 = listSub.Sum(x => x.C_ROOM_3);
                        rdo.C_ROOM_4 = listSub.Sum(x => x.C_ROOM_4);
                        rdo.C_ROOM_5 = listSub.Sum(x => x.C_ROOM_5);
                        rdo.C_ROOM_6 = listSub.Sum(x => x.C_ROOM_6);
                        rdo.C_ROOM_7 = listSub.Sum(x => x.C_ROOM_7);
                        rdo.C_ROOM_8 = listSub.Sum(x => x.C_ROOM_8);
                        rdo.C_ROOM_9 = listSub.Sum(x => x.C_ROOM_9);
                        rdo.C_ROOM_10 = listSub.Sum(x => x.C_ROOM_10);
                        rdo.C_ROOM_11 = listSub.Sum(x => x.C_ROOM_11);
                        rdo.C_ROOM_12 = listSub.Sum(x => x.C_ROOM_12);
                        rdo.C_ROOM_13 = listSub.Sum(x => x.C_ROOM_13);
                        rdo.C_ROOM_14 = listSub.Sum(x => x.C_ROOM_14);
                        rdo.C_ROOM_15 = listSub.Sum(x => x.C_ROOM_15);
                        rdo.C_ROOM_16 = listSub.Sum(x => x.C_ROOM_16);
                        rdo.C_ROOM_17 = listSub.Sum(x => x.C_ROOM_17);
                        rdo.C_ROOM_18 = listSub.Sum(x => x.C_ROOM_18);
                        rdo.C_ROOM_19 = listSub.Sum(x => x.C_ROOM_19);
                        rdo.C_ROOM_20 = listSub.Sum(x => x.C_ROOM_20);
                        rdo.C_ROOM_21 = listSub.Sum(x => x.C_ROOM_21);
                        rdo.C_ROOM_22= listSub.Sum(x => x.C_ROOM_22);
                        rdo.C_ROOM_23= listSub.Sum(x => x.C_ROOM_23);
                        rdo.C_ROOM_24= listSub.Sum(x => x.C_ROOM_24);
                        rdo.C_ROOM_25= listSub.Sum(x => x.C_ROOM_25);
                        rdo.C_ROOM_26= listSub.Sum(x => x.C_ROOM_26);
                        rdo.C_ROOM_27= listSub.Sum(x => x.C_ROOM_27);
                        rdo.C_ROOM_28= listSub.Sum(x => x.C_ROOM_28);
                        rdo.C_ROOM_29 = listSub.Sum(x => x.C_ROOM_29);
                        rdo.C_ROOM_30= listSub.Sum(x => x.C_ROOM_30);
                        rdo.C_ROOM_31 = listSub.Sum(x => x.C_ROOM_31);
                        rdo.C_ROOM_32 = listSub.Sum(x => x.C_ROOM_32);
                        rdo.C_ROOM_33 = listSub.Sum(x => x.C_ROOM_33);
                        rdo.C_ROOM_34 = listSub.Sum(x => x.C_ROOM_34);
                        rdo.C_ROOM_35 = listSub.Sum(x => x.C_ROOM_35);
                        rdo.C_ROOM_36 = listSub.Sum(x => x.C_ROOM_36);
                        rdo.C_ROOM_37 = listSub.Sum(x => x.C_ROOM_37);
                        rdo.C_ROOM_38 = listSub.Sum(x => x.C_ROOM_38);
                        rdo.C_ROOM_39 = listSub.Sum(x => x.C_ROOM_39);
                        rdo.C_ROOM_40 = listSub.Sum(x => x.C_ROOM_40);
                        rdo.C_ROOM_41 = listSub.Sum(x => x.C_ROOM_41);
                        rdo.C_ROOM_42 = listSub.Sum(x => x.C_ROOM_42);
                        rdo.C_ROOM_43 = listSub.Sum(x => x.C_ROOM_43);
                        rdo.C_ROOM_44 = listSub.Sum(x => x.C_ROOM_44);
                        rdo.C_ROOM_45 = listSub.Sum(x => x.C_ROOM_45);
                        rdo.C_ROOM_46 = listSub.Sum(x => x.C_ROOM_46);
                        rdo.C_ROOM_47 = listSub.Sum(x => x.C_ROOM_47);
                        rdo.C_ROOM_48 = listSub.Sum(x => x.C_ROOM_48);
                        rdo.C_ROOM_49 = listSub.Sum(x => x.C_ROOM_49);
                        rdo.C_ROOM_50 = listSub.Sum(x => x.C_ROOM_50);
                        ListRdo.Add(rdo);
                    }

                    Inventec.Common.Logging.LogSystem.Debug("finish gom theo ngay.");
                    //
                    // Them so luong cac loai dich vu va dieu tri

                    Inventec.Common.Logging.LogSystem.Debug("start Them so luong cac loai dich vu va dieu tri.");
                    if (filter.IS_EXTRA == true)
                    {
                        foreach (var item in ListRdo)
                        {
                            if (ListSereServType != null)
                            {
                                item.DIC_SERVICE_TYPE_AMOUNT = ListSereServType.Where(o => o.TDL_INTRUCTION_TIME - (o.TDL_INTRUCTION_TIME % 1000000) == item.IN_DATE).GroupBy(p => ServiceTypeCode(p.TDL_SERVICE_TYPE_ID)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT)) ?? new Dictionary<string, decimal>();
                            }
                            if (DepartmentInOuts != null)
                            {
                                item.DIC_IMP = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)item.IN_DATE / 1000000).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_EXP = DepartmentInOuts.Where(o => o.OUT_DATE == item.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_BEGIN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)item.IN_DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= item.IN_DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= item.IN_DATE || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                                item.DIC_END = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 <= (long)item.IN_DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME > item.IN_DATE + 235959 || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME > item.IN_DATE + 235959 || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_EXP_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == (long)item.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();
                            }
                            if (ListSereServKsk != null)
                            {
                                item.COUNT_KSK = (ListSereServKsk.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.KSK_TOTAL_PRICE = (ListSereServKsk.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                            if (ListSereServVp != null)
                            {
                                item.COUNT_VP = (ListSereServVp.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.VP_TOTAL_PRICE = (ListSereServVp.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                            if (ListSereServBhyt != null)
                            {
                                item.COUNT_BHYT = (ListSereServBhyt.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.BHYT_TOTAL_PRICE = (ListSereServBhyt.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                        }
                    }
                    //
                    Inventec.Common.Logging.LogSystem.Debug("finish Them so luong cac loai dich vu va dieu tri.");

                }
                //

                // lấy theo 192
                if (IsNotNullOrEmpty(ListTreatment1))
                {
                    // Tao listrdo
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo.");
                    foreach (var treatment in ListTreatment1)
                    {
                        Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        
                        
                        rdo.KH_TOTAL = 1;
                        rdo.KH_SICK = listTreatmentIdSick.Contains(treatment.ID) ? 1 : 0; ;
                        rdo.KH_HEIN = listTreatmentIdBHYT.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_FEE = listTreatmentIdDV.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_FREE = listTreatmentIdFree.Contains(treatment.ID) ? 1 : 0;
                        
                        rdo.KH_KSK = listTreatmentIdKSK.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CHILD = (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TDL_PATIENT_DOB))) ? 1 : 0;
                        rdo.KH_HAS_MEDI = listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_HEIN = listTreatmentIdBHYT.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_FEE = listTreatmentIdDV.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_CLN_FREE = listTreatmentIdFree.Contains(treatment.ID) && listTreatmentIdHasMedi.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_TRAN_OUT = listTreatmentIdTRAN_PATI.Contains(treatment.ID) ? 1 : 0;
                        rdo.KH_LEFT_LINE = listTreatmentEndIdRoute.Contains(treatment.ID) ? 1 : 0;

                        rdo.IN_TIME = treatment.IN_TIME;
                        //phòng khám
                        count = 1;

                        foreach (var room in listExamRoom)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            
                            System.Reflection.PropertyInfo piERoom = typeof(Mrs00386RDO).GetProperty("E_ROOM_" + count);
                            if (getListEndRoomId(treatment.ID, ListTreatment1).Contains(room.ROOM_ID)) piERoom.SetValue(rdo, 1);

                            System.Reflection.PropertyInfo piKHRoom = typeof(Mrs00386RDO).GetProperty("KH_ROOM_" + count);
                            if (getListExamFinishRoomId(treatment.ID, ListExamServiceReq1).Contains(room.ROOM_ID)) piKHRoom.SetValue(rdo, 1);

                            count++;
                        }
                        ListRdo1.Add(rdo);
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo.");
                    //
                    // Tao listrdo
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo CLN.");
                    if (IsNotNullOrEmpty(ListTreatmentCln1))
                    {

                        foreach (var treatment in ListTreatmentCln1)
                        {
                            Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;

                            rdo.IN_TIME = treatment.CLINICAL_IN_TIME ?? 0;
                            rdo.IN_DATE = (treatment.CLINICAL_IN_TIME ?? 0) - (treatment.CLINICAL_IN_TIME ?? 0) % 1000000;

                            rdo.CLN_BHYT = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt ? 1 : 0;
                            rdo.CLN_VP = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdFee ? 1 : 0;
                            rdo.CLN_MP = treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdFree ? 1 : 0;

                            ListRdo1.Add(rdo);
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo CLN.");
                    //
                    // Tao listrdo Transfer
                    Inventec.Common.Logging.LogSystem.Debug("start Tao listrdo tranfer.");
                    if (IsNotNullOrEmpty(ListTreatmentTransfer1))
                    {

                        foreach (var treatment in ListTreatmentTransfer1)
                        {
                            Mrs00386RDO rdo = new Mrs00386RDO(treatment);
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;

                            rdo.IN_TIME = treatment.OUT_TIME ?? 0;
                            rdo.IN_DATE = (treatment.OUT_TIME ?? 0) - (treatment.OUT_TIME ?? 0) % 1000000;

                            rdo.TRANSFER_OUT = 1;

                            ListRdo1.Add(rdo);
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Debug("finish Tao listrdo tranfer.");
                    //
                    // gom theo ngay

                    Inventec.Common.Logging.LogSystem.Debug("start gom theo ngay.");
                    var goupByDate = ListRdo1.OrderBy(p => p.IN_DATE).GroupBy(o => o.IN_DATE).ToList();
                    ListRdo1.Clear();


                    foreach (var group in goupByDate)
                    {
                        List<Mrs00386RDO> listSub = group.ToList<Mrs00386RDO>();
                        Mrs00386RDO rdo = new Mrs00386RDO(listSub.First());
                        rdo.IN_DATE = listSub.First().IN_DATE;
                        rdo.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSub.First().IN_DATE);
                        rdo.TOTAL = listSub.Sum(o => o.TOTAL);
                        rdo.SICK = listSub.Sum(o => o.SICK);
                        rdo.HEIN = listSub.Sum(o => o.HEIN);
                        rdo.FEE = listSub.Sum(o => o.FEE);
                        rdo.FREE = listSub.Sum(o => o.FREE);
                        rdo.KSK = listSub.Sum(o => o.KSK);
                        rdo.KSKs = listSub.Sum(o => o.KSKs);
                        rdo.CHILD = listSub.Sum(o => o.CHILD);
                        rdo.HAS_MEDI = listSub.Sum(o => o.HAS_MEDI);
                        rdo.IN_TIME = listSub.First().IN_TIME;

                        //Nhóm loại báo cáo
                        count = 1;
                        foreach (var parent in ListReporttypeCat)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            System.Reflection.PropertyInfo piParent = typeof(Mrs00386RDO).GetProperty("PARENT_" + count);

                            piParent.SetValue(rdo, TotalParent(piParent, rdo.IN_TIME, parent.ID));

                            System.Reflection.PropertyInfo piEParent = typeof(Mrs00386RDO).GetProperty("E_PARENT_" + count);

                            piEParent.SetValue(rdo, TotalEParent(piEParent, rdo.IN_TIME, parent.ID));
                            count++;
                        }
                        LogSystem.Info("a");
                        //phòng khám
                        count = 1;
                        foreach (var room in listExamRoom)
                        {
                            if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                            
                            if (room.EXECUTE_ROOM_NAME != "Phòng Khám 5")
                            {
                                System.Reflection.PropertyInfo piERoom = typeof(Mrs00386RDO).GetProperty("E_ROOM_" + count);
                                piERoom.SetValue(rdo, TotalRoom(piERoom, listSub));
                                System.Reflection.PropertyInfo piKHRoom = typeof(Mrs00386RDO).GetProperty("KH_ROOM_" + count);
                                piKHRoom.SetValue(rdo, TotalRoom(piKHRoom, listSub));
                                System.Reflection.PropertyInfo piCRoom = typeof(Mrs00386RDO).GetProperty("C_ROOM_" + count);
                                piCRoom.SetValue(rdo, TotalRoom(piCRoom, listSub));
                            }
                            count++;
                        }
                        
                        rdo.KH_TOTAL = listSub.Sum(o => o.KH_TOTAL);
                        rdo.KH_SICK = listSub.Sum(o => o.KH_SICK);
                        rdo.KH_HEIN = listSub.Sum(o => o.KH_HEIN);
                        rdo.KH_FEE = listSub.Sum(o => o.KH_FEE);
                        rdo.KH_FREE = listSub.Sum(o => o.KH_FREE);
                        rdo.KH_KSK = listSub.Sum(o => o.KH_KSK);
                        rdo.KH_KSKs = listSub.Sum(o => o.KH_KSKs);
                        rdo.KH_CHILD = listSub.Sum(o => o.KH_CHILD);
                        rdo.KH_HAS_MEDI = listSub.Sum(o => o.KH_HAS_MEDI);
                        rdo.KH_CLN_HEIN = listSub.Sum(o => o.KH_CLN_HEIN);
                        rdo.KH_CLN_FEE = listSub.Sum(o => o.KH_CLN_FEE);
                        rdo.KH_CLN_FREE = listSub.Sum(o => o.KH_CLN_FREE);
                        rdo.KH_TRAN_OUT = listSub.Sum(o => o.KH_TRAN_OUT);
                        rdo.KH_LEFT_LINE = listSub.Sum(o => o.KH_LEFT_LINE);
                        //tổng số bệnh nhân vào viện
                        rdo.C_ROOM_1 = listSub.Sum(x => x.C_ROOM_1);
                        rdo.C_ROOM_2 = listSub.Sum(x => x.C_ROOM_2);
                        rdo.C_ROOM_3 = listSub.Sum(x => x.C_ROOM_3);
                        rdo.C_ROOM_4 = listSub.Sum(x => x.C_ROOM_4);
                        rdo.C_ROOM_5 = listSub.Sum(x => x.C_ROOM_5);
                        rdo.C_ROOM_6 = listSub.Sum(x => x.C_ROOM_6);
                        rdo.C_ROOM_7 = listSub.Sum(x => x.C_ROOM_7);
                        rdo.C_ROOM_8 = listSub.Sum(x => x.C_ROOM_8);
                        rdo.C_ROOM_9 = listSub.Sum(x => x.C_ROOM_9);
                        rdo.C_ROOM_10 = listSub.Sum(x => x.C_ROOM_10);
                        rdo.C_ROOM_11 = listSub.Sum(x => x.C_ROOM_11);
                        rdo.C_ROOM_12 = listSub.Sum(x => x.C_ROOM_12);
                        rdo.C_ROOM_13 = listSub.Sum(x => x.C_ROOM_13);
                        rdo.C_ROOM_14 = listSub.Sum(x => x.C_ROOM_14);
                        rdo.C_ROOM_15 = listSub.Sum(x => x.C_ROOM_15);
                        rdo.C_ROOM_16 = listSub.Sum(x => x.C_ROOM_16);
                        rdo.C_ROOM_17 = listSub.Sum(x => x.C_ROOM_17);
                        rdo.C_ROOM_18 = listSub.Sum(x => x.C_ROOM_18);
                        rdo.C_ROOM_19 = listSub.Sum(x => x.C_ROOM_19);
                        rdo.C_ROOM_20 = listSub.Sum(x => x.C_ROOM_20);
                        rdo.C_ROOM_21 = listSub.Sum(x => x.C_ROOM_21);
                        rdo.C_ROOM_22 = listSub.Sum(x => x.C_ROOM_22);
                        rdo.C_ROOM_23 = listSub.Sum(x => x.C_ROOM_23);
                        rdo.C_ROOM_24 = listSub.Sum(x => x.C_ROOM_24);
                        rdo.C_ROOM_25 = listSub.Sum(x => x.C_ROOM_25);
                        rdo.C_ROOM_26 = listSub.Sum(x => x.C_ROOM_26);
                        rdo.C_ROOM_27 = listSub.Sum(x => x.C_ROOM_27);
                        rdo.C_ROOM_28 = listSub.Sum(x => x.C_ROOM_28);
                        rdo.C_ROOM_29 = listSub.Sum(x => x.C_ROOM_29);
                        rdo.C_ROOM_30 = listSub.Sum(x => x.C_ROOM_30);
                        rdo.C_ROOM_31 = listSub.Sum(x => x.C_ROOM_31);
                        rdo.C_ROOM_32 = listSub.Sum(x => x.C_ROOM_32);
                        rdo.C_ROOM_33 = listSub.Sum(x => x.C_ROOM_33);
                        rdo.C_ROOM_34 = listSub.Sum(x => x.C_ROOM_34);
                        rdo.C_ROOM_35 = listSub.Sum(x => x.C_ROOM_35);
                        rdo.C_ROOM_36 = listSub.Sum(x => x.C_ROOM_36);
                        rdo.C_ROOM_37 = listSub.Sum(x => x.C_ROOM_37);
                        rdo.C_ROOM_38 = listSub.Sum(x => x.C_ROOM_38);
                        rdo.C_ROOM_39 = listSub.Sum(x => x.C_ROOM_39);
                        rdo.C_ROOM_40 = listSub.Sum(x => x.C_ROOM_40);
                        rdo.C_ROOM_41 = listSub.Sum(x => x.C_ROOM_41);
                        rdo.C_ROOM_42 = listSub.Sum(x => x.C_ROOM_42);
                        rdo.C_ROOM_43 = listSub.Sum(x => x.C_ROOM_43);
                        rdo.C_ROOM_44 = listSub.Sum(x => x.C_ROOM_44);
                        rdo.C_ROOM_45 = listSub.Sum(x => x.C_ROOM_45);
                        rdo.C_ROOM_46 = listSub.Sum(x => x.C_ROOM_46);
                        rdo.C_ROOM_47 = listSub.Sum(x => x.C_ROOM_47);
                        rdo.C_ROOM_48 = listSub.Sum(x => x.C_ROOM_48);
                        rdo.C_ROOM_49 = listSub.Sum(x => x.C_ROOM_49);
                        rdo.C_ROOM_50 = listSub.Sum(x => x.C_ROOM_50);
                        ListRdo1.Add(rdo);
                    }

                    Inventec.Common.Logging.LogSystem.Debug("finish gom theo ngay.");
                    //
                    // Them so luong cac loai dich vu va dieu tri

                    Inventec.Common.Logging.LogSystem.Debug("start Them so luong cac loai dich vu va dieu tri.");
                    if (filter.IS_EXTRA == true)
                    {
                        foreach (var item in ListRdo1)
                        {
                            if (ListSereServType != null)
                            {
                                item.DIC_SERVICE_TYPE_AMOUNT = ListSereServType.Where(o => o.TDL_INTRUCTION_TIME - (o.TDL_INTRUCTION_TIME % 1000000) == item.IN_DATE).GroupBy(p => ServiceTypeCode(p.TDL_SERVICE_TYPE_ID)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT)) ?? new Dictionary<string, decimal>();
                            }
                            if (DepartmentInOuts != null)
                            {
                                item.DIC_IMP = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 == (long)item.IN_DATE / 1000000).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_EXP = DepartmentInOuts.Where(o => o.OUT_DATE == item.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_BEGIN = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 < (long)item.IN_DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME >= item.IN_DATE || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME >= item.IN_DATE || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count());

                                item.DIC_END = DepartmentInOuts.Where(o => (long)(o.DEPARTMENT_IN_TIME ?? 0) / 1000000 <= (long)item.IN_DATE / 1000000 && (o.NEXT_DEPARTMENT_IN_TIME > item.IN_DATE + 235959 || (o.NEXT_DEPARTMENT_IN_TIME == null && (o.OUT_TIME > item.IN_DATE + 235959 || o.OUT_TIME == null)))).GroupBy(r => DepartmentCode(r.DEPARTMENT_ID)).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();

                                item.DIC_EXP_END_TYPE = DepartmentInOuts.Where(o => o.OUT_DATE == (long)item.IN_DATE && o.NEXT_ID == null).GroupBy(r => DepartmentEndTypeCode(r.DEPARTMENT_ID, (r.TREATMENT_END_TYPE_ID ?? 0))).ToDictionary(p => p.Key, q => q.Count()) ?? new Dictionary<string, int>();
                            }
                            if (ListSereServKsk != null)
                            {
                                item.COUNT_KSK = (ListSereServKsk.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.KSK_TOTAL_PRICE = (ListSereServKsk.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                            if (ListSereServVp != null)
                            {
                                item.COUNT_VP = (ListSereServVp.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.VP_TOTAL_PRICE = (ListSereServVp.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                            if (ListSereServBhyt != null)
                            {
                                item.COUNT_BHYT = (ListSereServBhyt.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).COUNT_TREATMENT;
                                item.BHYT_TOTAL_PRICE = (ListSereServBhyt.FirstOrDefault(o => o.TIME == item.IN_DATE) ?? new SereServPrice()).VIR_TOTAL_PRICE;
                            }
                        }
                    }
                    //
                    Inventec.Common.Logging.LogSystem.Debug("finish Them so luong cac loai dich vu va dieu tri.");

                }
                //
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        //danh sach phong kham ket thuc kham
        private List<long> getListFRoomId(long treatmentId)
        {
            List<long> result = new List<long>();
            try
            {
                if (ListExamServiceReq != null && ListExamSereServ != null)
                {
                    var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatmentId && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT
                        /*&& ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0))*/).ToList();
                    if (IsNotNullOrEmpty(listExamServiceReqSub))
                    {
                        result = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        private List<long> getListCRoomId(PatientTypeAlter treatment)
        {
            List<long> result = new List<long>();
            try
            {
                if (ListExamServiceReq != null && ListExamSereServ != null)
                {
                    var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatment.ID/* && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT
                        && ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0))*/).ToList();
                    if (IsNotNullOrEmpty(listExamServiceReqSub))
                    {
                        var executeRoomIds = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
                        if (treatment.IN_ROOM_ID.HasValue && executeRoomIds.Contains(treatment.IN_ROOM_ID.Value))
                        {
                            result.Add(treatment.IN_ROOM_ID.Value);
                        }
                        else if (treatment.END_ROOM_ID.HasValue && executeRoomIds.Contains(treatment.END_ROOM_ID.Value))
                        {
                            result.Add(treatment.END_ROOM_ID.Value);
                        }
                        else
                        {
                            result.Add(listExamServiceReqSub.OrderBy(o => o.INTRUCTION_TIME).Select(p => p.EXECUTE_ROOM_ID).LastOrDefault());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        private List<long> getListCRoomId1(HIS_TREATMENT treatment)
        {
            List<long> result = new List<long>();
            try
            {
                if (ListExamServiceReq != null && ListExamSereServ != null)
                {
                    var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatment.ID/* && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT
                        && ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0))*/).ToList();
                    if (IsNotNullOrEmpty(listExamServiceReqSub))
                    {
                        var executeRoomIds = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
                        if (treatment.IN_ROOM_ID.HasValue && executeRoomIds.Contains(treatment.IN_ROOM_ID.Value))
                        {
                            result.Add(treatment.IN_ROOM_ID.Value);
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        //danh sach phong kham xu ly nhap vien hoac ket thuc dieu tri
        private List<long> getListTRoomId(PatientTypeAlter treatment)
        {
            List<long> result = new List<long>();
            try
            {
                if (ListExamServiceReq != null && ListExamSereServ != null)
                {
                    var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatment.ID/* && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT
                        && ListExamSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID && (p.VIR_HEIN_PRICE > 0 || p.VIR_PATIENT_PRICE > 0))*/).ToList();
                    if (IsNotNullOrEmpty(listExamServiceReqSub))
                    {
                        var executeRoomIds = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
                        if (treatment.IN_ROOM_ID.HasValue && executeRoomIds.Contains(treatment.IN_ROOM_ID.Value))
                        {
                            result.Add(treatment.IN_ROOM_ID.Value);
                        }

                        else if (treatment.END_ROOM_ID.HasValue && executeRoomIds.Contains(treatment.END_ROOM_ID.Value))
                        {
                            result.Add(treatment.END_ROOM_ID.Value);
                        }
                        else
                        {
                            result.Add(listExamServiceReqSub.OrderBy(o => o.INTRUCTION_TIME).Select(p => p.EXECUTE_ROOM_ID).LastOrDefault());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsSick(HIS_SERVICE_REQ thisData, List<PatientTypeAlter> ListTreatment)
        {
            bool result = false;
            try
            {
                if (thisData == null) return result;
                if (ListTreatment == null) return result;
                var curentListTreatment = ListTreatment.FirstOrDefault(o => o.ID == thisData.TREATMENT_ID);
                if (curentListTreatment != null)
                {
                    result = (curentListTreatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM);
                }
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private string DepartmentEndTypeCode(long departmentId, long endTypeId)
        {
            string result = "";
            try
            {
                result = ((HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "")
                     + "_" + ((listHisTreatmentEndType.FirstOrDefault(o => o.ID == endTypeId) ?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                result = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string ServiceTypeCode(long serviceTypeId)
        {
            string result = "";
            try
            {
                result = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == serviceTypeId) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }


        private long TotalParent(System.Reflection.PropertyInfo piParent, long time, long reportTypeCatId)
        {
            long result = 0;
            try
            {
                long dateFrom = time - time % 1000000;
                long dateTo = dateFrom + 235959;
                result = (long)ListSereServ.Where(o => o.TDL_INTRUCTION_TIME >= dateFrom && o.TDL_INTRUCTION_TIME < dateTo && reportTypeCatId == o.REPORT_TYPE_CAT_ID).Sum(p => p.AMOUNT);
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private long TotalEParent(System.Reflection.PropertyInfo piParent, long time, long reportTypeCatId)
        {
            long result = 0;
            long a = 0;
            try
            {
                long dateFrom = time - time % 1000000;
                long dateTo = dateFrom + 235959;
                LogSystem.Info("b");
                result = (long)ListSereServ.Where(o => o.TDL_INTRUCTION_TIME >= dateFrom && o.TDL_INTRUCTION_TIME < dateTo && o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && reportTypeCatId == o.REPORT_TYPE_CAT_ID && ListTreatment.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID && p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && p.TREATMENT_END_TYPE_ID != null)).Sum(p => p.AMOUNT) ;
                LogSystem.Info("c");
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }
        private long TotalRoom(System.Reflection.PropertyInfo piRoom, List<Mrs00386RDO> listSub)
        {
            long result = 0;
            try
            {
                var listtreatmentCode = new List<string>();
                foreach (var sub in listSub)
                {
                    //if ((long)piRoom.GetValue(sub) == 1) listtreatmentCode.Add(sub.TREATMENT_CODE);
                    result = result + (long)piRoom.GetValue(sub);
                }
                //LogSystem.Info(piRoom.Name+": " + string.Join(", ", listtreatmentCode)); 
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }
        //DS phòng
        private List<long> getListExamRoomId(long treatmentId, List<HIS_SERVICE_REQ> ListExamServiceReq)
        {
            List<long> result = new List<long>();
            try
            {
                var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatmentId).ToList();
                if (IsNotNullOrEmpty(listExamServiceReqSub)) result = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        //Ds phòng kết thúc điều trị
        private List<long> getListEndRoomId(long treatmentId, List<HIS_TREATMENT> ListTreatment)
        {
            List<long> result = new List<long>();
            try
            {
                var listTreatmentSub = ListTreatment.Where(o => o.ID == treatmentId).ToList();
                if (IsNotNullOrEmpty(listTreatmentSub)) result = listTreatmentSub.Select(o => o.END_ROOM_ID ?? 0).ToList();
            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }

        //Ds phòng kết thúc công khám không tích hao phí
        private List<long> getListExamFinishRoomId(long treatmentId, List<HIS_SERVICE_REQ> ListExamServiceReq)
        {
            List<long> result = new List<long>();
            try
            {
                var listExamServiceReqSub = ListExamServiceReq.Where(o => o.TREATMENT_ID == treatmentId && 
                                                                          o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && 
                                                                          o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                if (IsNotNullOrEmpty(listExamServiceReqSub)) result = listExamServiceReqSub.Select(o => o.EXECUTE_ROOM_ID).ToList();
            }
            catch (Exception ex)
            {
                result = new List<long>();

                LogSystem.Error(ex);
            }
            return result;
        }
        //Phòng khám cuối/ hiện tại
        //private long getExamRoomId(long treatmentId, List<HIS_SERVICE_REQ> listExamServiceReq)
        //{
        //    long result = -1;
        //    try
        //    {
        //        var listExamServiceReqSub = listExamServiceReq.Where(o => o.TREATMENT_ID == treatmentId).ToList();
        //        if (IsNotNullOrEmpty(listExamServiceReqSub))
        //            if (listExamServiceReqSub.Where(o => o.FINISH_TIME == null || o.FINISH_TIME == 0).ToList().Count > 0) result = listExamServiceReqSub.Where(o => o.FINISH_TIME == null || o.FINISH_TIME == 0).OrderBy(p => p.ID).Last().EXECUTE_ROOM_ID;
        //            else result = listExamServiceReqSub.OrderBy(p => p.FINISH_TIME).Last().EXECUTE_ROOM_ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = -1;
        //        LogSystem.Error(ex);
        //    }
        //    return result;
        //}

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
            //Nhóm loại báo cáo
            for (int count = 0; count < ListReporttypeCat.Count; count++)
            {
                if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                var i = count + 1;
                System.Reflection.PropertyInfo piParent = typeof(Title).GetProperty("PARENT_NAME_" + i);
                dicSingleTag.Add(string.Format("PARENT_{0}", count + 1), piParent.GetValue(Title));
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
            objectTag.AddObjectData(store, "Report1", ListRdo1);
            objectTag.AddObjectData(store, "ReportInTreatmemt", ListRdoInTreatment);
            objectTag.AddRelationship(store, "ReportInTreatmemt", "Report", "IN_DATE", "IN_DATE");
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            if (filter.IS_COUNT_EXAM == true)
            {
                dicSingleTag.Add("COUNT_EXAM", "1");
            }
            else
            {
                dicSingleTag.Add("COUNT_EXAM", "0");
            }
        }

        private List<long> treatmentTypeId(PatientTypeAlter thisData, List<PatientTypeAlter> LisPatientTypeAlter)
        {
            List<long> result = new List<long>();
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.ID).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.Select(o => o.TREATMENT_TYPE_ID).ToList();
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = new List<long>();
            }
            return result;
        }

        private long patientTypeId(HIS_SERVICE_REQ thisData, List<PatientTypeAlter> LisPatientTypeAlter)
        {
            long result = 0;
            try
            {
                if (thisData == null) return result;
                if (LisPatientTypeAlter == null) return result;
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME <= thisData.INTRUCTION_TIME).ToList();
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().PATIENT_TYPE_ID;
                else
                {
                    LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.TREATMENT_ID == thisData.TREATMENT_ID && o.LOG_TIME > thisData.INTRUCTION_TIME).ToList();
                    if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                        result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().PATIENT_TYPE_ID;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

                result = 0;
            }
            return result;
        }
    }

}
