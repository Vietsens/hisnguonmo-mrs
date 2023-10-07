using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisDeathWithin;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00444
{
    class Mrs00444Processor : AbstractProcessor
    {
        #region ReClare

        CommonParam paramGet = new CommonParam();
        //số lượt khám
        private long TOTAL_EXAM = 0;
        private long EMERGENCY = 0;
        private long BHYT = 0;
        private long FEE = 0;
        private long HN = 0;
        private long TE = 0;
        private long KH_LESS_THAN_6 = 0;
        private long OUT_PROVINCE = 0;
        private long KSK = 0;
        //thông tin khám bổ sung
        private long VP = 0;
        private long KH_BHYT_LESS_THAN_6 = 0;
        private long KH_VP_LESS_THAN_6 = 0;
        private long KH_MORE_THAN_60 = 0;
        private long KH_BHYT_MORE_THAN_60 = 0;
        private long KH_VP_MORE_THAN_60 = 0;
        private long KH_FOREIGNER = 0;
        private long TOTAL_SURG_FINISH = 0;
        private long TOTAL_SURG_FINISH_GROUP_2 = 0;
        private long TOTAL_SURG_FINISH_GROUP_3 = 0;
        private long TOTAL_MISU_FINISH = 0;
        private long TOTAL_MISU_FINISH_GROUP_1 = 0;
        private long TOTAL_MISU_FINISH_GROUP_2 = 0;
        private long TOTAL_MISU_FINISH_GROUP_3 = 0;
        private long TOTAL_MISU_FINISH_GROUP_4 = 0;
        private long PATIENT_TREAT_OUT = 0;
        private long TP_TREAT_OUT_01 = 0;
        private long TP_TREAT_OUT_02 = 0;
        private long TP_TREAT_OUT_KHAC = 0;
        //số bệnh nhân khám
        private long TOTAL_TREATMENT = 0;
        private long TM_HN = 0;
        private long TM_TE = 0;
        private long TM_OUT_PROVINCE = 0;
        private long TM_KSK = 0;
        private long TM_OUT_TREAT = 0;
        //PTTT
        private long TOTAL_SURG = 0;
        private long TOTAL_MISU = 0;
        //Chuyển tuyến
        private long TRAN_PATI = 0;
        private long TP_TREAT_IN = 0;
        private long TP_TREAT_OUT = 0;
        //Tử vong
        private long DEATH = 0;
        private long ADULTS = 0;
        private long LESSER_THAN_5 = 0;
        private long LESSER_THAN_15 = 0;
        private long D_TREAT = 0;
        private long D_EXAM = 0;

        private long D24_HOUR = 0;
        private long D24_ADULTS = 0;
        private long D24_LESSER_THAN_5 = 0;
        private long D24_LESSER_THAN_15 = 0;
        private long D24_TREAT = 0;
        private long D24_EXAM = 0;
        //CĐHA
        private long DIIM = 0;
        private long SUIM = 0;
        private long SCAN = 0;
        //Thăm dò chức năng
        private long ECG = 0;
        private long ENDO = 0;
        //Điều trị
        private long OLD_TREAT = 0;
        private long NEW_TREAT = 0;
        private long NEW_TREAT_BH = 0;
        private long NEW_TREAT_VP = 0;
        private long TREAT_HN = 0;
        private long TREAT_LESSER_THAN_6 = 0;
        private long TREAT_LESSER_THAN_15 = 0;
        private long TREAT_OUT_PROVINCE = 0;
        private long TREAT_RV = 0;
        private long END_TREAT = 0;
        private long END_TREAT_BH = 0;
        private long END_TREAT_VP = 0;
        private long END_TREAT_LESSER_THAN_6 = 0;
        private long END_TREAT_LESSER_THAN_15 = 0;
        private long END_TREAT_OUT_PROVINCE = 0;
        private long OLD_TREAT_DAY = 0;
        private long OUT_TREAT_DAY = 0;
        private long patientTypeIdBhyt = 0;
        private long patientTypeIdVp = 0;
        #endregion

        public Mrs00444Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00444Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00444Filter)reportFilter);
            bool result = true;
            try
            {
                patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                patientTypeIdVp = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                long? timeNow = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now);
                if (filter.IS_TREAT_IN == false || filter.IS_TREAT_IN == null)
                {
                    //Số lượt khám
                    HisServiceReqViewFilterQuery filterExq = new HisServiceReqViewFilterQuery();
                    filterExq.INTRUCTION_TIME_FROM = filter.TIME_FROM;
                    filterExq.INTRUCTION_TIME_TO = filter.TIME_TO;
                    filterExq.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    filterExq.REQUEST_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                    filterExq.HAS_EXECUTE = true;
                    var listServiceReq = new HisServiceReqManager(paramGet).GetView(filterExq);
                    TOTAL_EXAM = listServiceReq.Count;
                    //phong cap cuu
                    Dictionary<long, HIS_EXECUTE_ROOM> dicEmergency = new Dictionary<long, HIS_EXECUTE_ROOM>();
                    HisExecuteRoomFilterQuery filterER = new HisExecuteRoomFilterQuery();
                    filterER.IS_EMERGENCY = true;
                    var listExecuteRoom = new HisExecuteRoomManager(paramGet).Get(filterER);
                    foreach (var ExRoom in listExecuteRoom)
                    {
                        if (!dicEmergency.ContainsKey(ExRoom.ROOM_ID))
                        {
                            dicEmergency[ExRoom.ROOM_ID] = ExRoom;
                        }
                    }
                    //kham cap cuu
                    EMERGENCY = listServiceReq.Where(o => dicEmergency.ContainsKey(o.EXECUTE_ROOM_ID)).ToList().Count;

                    //Đối tượng điều trị
                    var listTreatmentId = listServiceReq.GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList();
                    var listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                            {
                                TREATMENT_IDs = listIDs
                            };
                            var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                            listPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                        }
                    }
                    listPatientTypeAlter = listPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ThenByDescending(p => p.ID).ToList();
                    foreach (var PatyAlter in listPatientTypeAlter)
                    {
                        if (!dicPatientTypeAlter.ContainsKey(PatyAlter.TREATMENT_ID))
                        {
                            dicPatientTypeAlter[PatyAlter.TREATMENT_ID] = PatyAlter;
                        }
                    }

                    //kham BHYT
                    BHYT = listServiceReq.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID) && dicPatientTypeAlter[o.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdBhyt).ToList().Count;

                    //kham FEE
                    FEE = listServiceReq.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID) && dicPatientTypeAlter[o.TREATMENT_ID].PATIENT_TYPE_ID != patientTypeIdBhyt).ToList().Count;

                    //kham VP
                    VP = listServiceReq.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID) && dicPatientTypeAlter[o.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdVp).ToList().Count;

                    //kham HN
                    HN = listServiceReq.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID) && dicPatientTypeAlter[o.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdBhyt && IsNotNullOrEmpty(dicPatientTypeAlter[o.TREATMENT_ID].HNCODE)).ToList().Count;

                    //kham TE
                    var ListTE = listServiceReq.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB) != null && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild((DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TDL_PATIENT_DOB))).ToList();
                    TE = ListTE.Count;

                    //tong tre em duoi 6 tuoi cac doi tuong
                    KH_LESS_THAN_6 = listServiceReq.Where(q => timeNow < 60000000000 + q.TDL_PATIENT_DOB).ToList().Count;

                    //tong tre em duoi 6 tuoi BHYT
                    KH_BHYT_LESS_THAN_6 = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdBhyt && timeNow < 60000000000 + q.TDL_PATIENT_DOB).ToList().Count;

                    //tong tre em duoi 6 tuoi VP
                    KH_VP_LESS_THAN_6 = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].PATIENT_TYPE_ID != patientTypeIdVp && timeNow < 60000000000 + q.TDL_PATIENT_DOB).ToList().Count;

                    //tong nguoi cao tuoi tren 60 cac doi tuong
                    KH_MORE_THAN_60 = listServiceReq.Where(q => timeNow >= 600000000000 + q.TDL_PATIENT_DOB).ToList().Count;

                    //tong nguoi cao tuoi tren 60 BHYT
                    KH_BHYT_MORE_THAN_60 = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdBhyt && timeNow >= 600000000000 + q.TDL_PATIENT_DOB).ToList().Count;

                    //tong nguoi cao tuoi tren 60 VP
                    KH_VP_MORE_THAN_60 = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].PATIENT_TYPE_ID != patientTypeIdVp && timeNow >= 600000000000 + q.TDL_PATIENT_DOB).ToList().Count;


                    //bệnh nhân ngoai tinh
                    var listPatientId = listServiceReq.GroupBy(o => o.TDL_PATIENT_ID).Select(p => p.First().TDL_PATIENT_ID).ToList();
                    var listPatient = new List<HIS_PATIENT>();

                    Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();
                    if (IsNotNullOrEmpty(listPatientId))
                    {
                        var skip = 0;
                        while (listPatientId.Count - skip > 0)
                        {
                            var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientFilterQuery PatientFilter = new HisPatientFilterQuery()
                            {
                                IDs = listIDs
                            };
                            var ListPatientSub = new HisPatientManager(paramGet).Get(PatientFilter);
                            listPatient.AddRange(ListPatientSub);
                        }
                    }
                    foreach (var patient in listPatient) if (!dicPatient.ContainsKey(patient.ID)) dicPatient[patient.ID] = patient;
                    var Branch = new HisBranchManager(paramGet).Get(new HisBranchFilterQuery());

                    var ListOutProvince = listServiceReq.Where(o => dicPatient.ContainsKey(o.TDL_PATIENT_ID) && IsNotNullOrEmpty(Branch) && IsNotNullOrEmpty(Branch.First().HEIN_PROVINCE_CODE) && IsNotNullOrEmpty(dicPatient[o.TDL_PATIENT_ID].PROVINCE_CODE) && dicPatient[o.TDL_PATIENT_ID].PROVINCE_CODE != Branch.First().HEIN_PROVINCE_CODE).ToList();
                    OUT_PROVINCE = ListOutProvince.Count;


                    //tong so luot kham benh nguoi nuoc ngoai
                    KH_FOREIGNER = listServiceReq.Where(q => dicPatient.ContainsKey(q.TDL_PATIENT_ID) && dicPatient[q.TDL_PATIENT_ID].NATIONAL_CODE != "VN").ToList().Count;


                    //Khám sức khỏe
                    //dịch vụ KSK
                    var listServiceReqId = listServiceReq.GroupBy(o => o.ID).Select(p => p.First().ID).ToList();
                    var listSereServKSK = new List<HIS_SERE_SERV>();
                    if (IsNotNullOrEmpty(listServiceReqId))
                    {
                        HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                        serviceFilter.SERVICE_CODEs = HisServiceCFG.getList_SERVICE_CODE__KSK;
                        var listService = new HisServiceManager().Get(serviceFilter) ?? new List<HIS_SERVICE>();
                        var skip = 0;
                        while (listServiceReqId.Count - skip > 0)
                        {
                            var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var HisExamSerServfilter = new HisSereServFilterQuery()
                            {
                                SERVICE_REQ_IDs = listIDs,
                                SERVICE_IDs = listService.Select(o => o.ID).ToList()
                            };

                            var ListSereServSub = new HisSereServManager(paramGet).Get(HisExamSerServfilter);
                            listSereServKSK.AddRange(ListSereServSub);
                        }
                    }
                    KSK = listSereServKSK.Count;


                    //Số BN khám
                    TOTAL_TREATMENT = listServiceReq.GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;

                    //BN HN
                    TM_HN = listServiceReq.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID) && dicPatientTypeAlter[o.TREATMENT_ID].PATIENT_TYPE_ID == patientTypeIdBhyt && IsNotNullOrEmpty(dicPatientTypeAlter[o.TREATMENT_ID].HNCODE)).GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    //BN TE
                    TM_TE = ListTE.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    //BN ngoai tinh
                    TM_OUT_PROVINCE = ListOutProvince.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    //BN Khám sức khỏe
                    //dịch vụ KSK
                    TM_KSK = listSereServKSK.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList().Count;

                    //tong so benh nhan dieu tri ngoai tru
                    PATIENT_TREAT_OUT = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(p => p.TDL_PATIENT_ID).Distinct().ToList().Count;

                    //số ngày điều trị ngoại trú
                    TM_OUT_TREAT = listServiceReq.Where(q => dicPatientTypeAlter.ContainsKey(q.TREATMENT_ID) && (dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU||dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)).Select(p=>p.TREATMENT_ID).Distinct().ToList().Count;

                    //dịch vụ theo thời gian chỉ định
                    var listSese = new ManagerSql().GetSereServ(filter);
                    //PTTT
                    TOTAL_SURG = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList().Count();
                    TOTAL_MISU = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList().Count();
                    long ptttGroupId1 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;
                    long ptttGroupId2 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;
                    long ptttGroupId3 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;
                    long ptttGroupId4 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;
                    //Tong so phau thuat duoc thuc hien
                    TOTAL_SURG_FINISH = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && o.FINISH_TIME.HasValue).ToList().Count();

                    //Tong so phau thuat loai 2 duoc thuc hien
                    TOTAL_SURG_FINISH_GROUP_2 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId2).ToList().Count();

                    //Tong so phau thuat loai 3 duoc thuc hien
                    TOTAL_SURG_FINISH_GROUP_3 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId3).ToList().Count();

                    //Tong so thu thuat duoc thuc hien
                    TOTAL_MISU_FINISH = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && o.FINISH_TIME.HasValue).ToList().Count();

                    //Tong so thu thuat loai 1 duoc thuc hien
                    TOTAL_MISU_FINISH_GROUP_1 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId1).ToList().Count();

                    //Tong so thu thuat loai 2 duoc thuc hien
                    TOTAL_MISU_FINISH_GROUP_2 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId2).ToList().Count();

                    //Tong so thu thuat loai 3 duoc thuc hien
                    TOTAL_MISU_FINISH_GROUP_3 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId3).ToList().Count();

                    //Tong so thu thuat loai dac biet duoc thuc hien
                    TOTAL_MISU_FINISH_GROUP_4 = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && o.FINISH_TIME.HasValue && o.PTTT_GROUP_ID == ptttGroupId4).ToList().Count();

                    //Chuyển tuyến
                    var listTranPati = new List<HIS_TREATMENT>();
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery treatmentfilter = new HisTreatmentFilterQuery();
                            treatmentfilter.IDs = listIDs;
                            treatmentfilter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                            var listTreatmentSub = new HisTreatmentManager(paramGet).Get(treatmentfilter);
                            listTranPati.AddRange(listTreatmentSub);
                        }
                    }

                    //tong so luot kham benh chuyen tuyen
                    TP_TREAT_OUT = listServiceReq.Count(q => listTranPati.Exists(o => o.ID == q.TREATMENT_ID && o.END_ROOM_ID == q.EXECUTE_ROOM_ID));
                    //his_tran_pati_tech
                    var listTranPatiTech = new ManagerSql().GetTranPatiTech();

                    //tong so luot kham chuyen len tuyen tren ly do vuot kha nang chuyen mon
                    TP_TREAT_OUT_01 = listServiceReq.Count(q => listTranPati.Exists(o => o.ID == q.TREATMENT_ID && o.END_ROOM_ID == q.EXECUTE_ROOM_ID && listTranPatiTech != null && listTranPatiTech.Exists(p => p.ID == o.TRAN_PATI_TECH_ID && p.TRAN_PATI_TECH_CODE == "01")));

                    //tong so luot kham chuyen tuyen chuyen khoa
                    TP_TREAT_OUT_02 = listServiceReq.Count(q => listTranPati.Exists(o => o.ID == q.TREATMENT_ID && o.END_ROOM_ID == q.EXECUTE_ROOM_ID && listTranPatiTech != null && listTranPatiTech.Exists(p => p.ID == o.TRAN_PATI_TECH_ID && p.TRAN_PATI_TECH_CODE == "02")));

                    //tong so luot kham chuyen tuyen vi li do khac
                    TP_TREAT_OUT_KHAC = listServiceReq.Count(q => listTranPati.Exists(o => o.ID == q.TREATMENT_ID && o.END_ROOM_ID == q.EXECUTE_ROOM_ID && listTranPatiTech != null && listTranPatiTech.Exists(p => p.ID == o.TRAN_PATI_TECH_ID && p.TRAN_PATI_TECH_CODE != "01" && p.TRAN_PATI_TECH_CODE != "02")));

                    //Tử vong
                    var listDeath = new List<HIS_TREATMENT>();
                    Dictionary<long, HIS_TREATMENT> dicDeath = new Dictionary<long, HIS_TREATMENT>();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery treatmentfilter = new HisTreatmentFilterQuery();
                            treatmentfilter.IDs = listIDs;
                            treatmentfilter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                            var listTreatmentSub = new HisTreatmentManager(paramGet).Get(treatmentfilter);
                            listDeath.AddRange(listTreatmentSub);
                        }
                        foreach (var death in listDeath) if (!dicDeath.ContainsKey(death.ID)) dicDeath[death.ID] = death;
                        DEATH = listDeath.Count();
                        ADULTS = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && timeNow > 150000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        LESSER_THAN_5 = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && timeNow < 50000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        LESSER_THAN_15 = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && timeNow >= 50000000000 + q.TDL_PATIENT_DOB && timeNow <= 150000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D_EXAM = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D_TREAT = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;


                    }

                    //Tử vong  24
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {

                        D24_HOUR = listDeath.Where(o => o.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS).Count();
                        ADULTS = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicDeath[q.TREATMENT_ID].DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS && timeNow > 150000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D24_LESSER_THAN_5 = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicDeath[q.TREATMENT_ID].DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS && timeNow < 50000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D24_LESSER_THAN_15 = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicDeath[q.TREATMENT_ID].DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS && timeNow >= 50000000000 + q.TDL_PATIENT_DOB && timeNow <= 150000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D24_EXAM = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicDeath[q.TREATMENT_ID].DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS && dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                        D24_TREAT = listServiceReq.Where(q => dicDeath.ContainsKey(q.TREATMENT_ID) && dicDeath[q.TREATMENT_ID].DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS && dicPatientTypeAlter[q.TREATMENT_ID].TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).GroupBy(o => o.TREATMENT_ID).Select(p => p.First().TREATMENT_ID).ToList().Count;
                    }


                    //CDHA
                    DIIM = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).ToList().Count;
                    SUIM = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList().Count;

                    ENDO = listSese.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).ToList().Count;


                    listServiceReq.Clear();
                    listPatient.Clear();
                    dicPatientTypeAlter.Clear();
                    dicPatient.Clear();
                    listSese.Clear();
                    listTranPati.Clear();
                    listDeath.Clear();

                    listPatientTypeAlter = null;
                    listServiceReq = null;
                    listPatient = null;
                    dicPatientTypeAlter = null;
                    dicPatient = null;
                    listSese = null;
                    listTranPati = null;
                    listDeath = null;
                }
                if (filter.IS_TREAT_IN == true || filter.IS_TREAT_IN == null)
                {
                    //Nội trú
                    var NtListTreatment = new List<HIS_TREATMENT>();
                    //Lấy tất cả các HSDT có thời gian ra viện lớn hơn TIME_FROM
                    HisTreatmentFilterQuery filterTreatment = new HisTreatmentFilterQuery();
                    filterTreatment.IS_PAUSE = false;
                    filterTreatment.CLINICAL_IN_TIME_TO = filter.TIME_TO;
                    var ListTreatmentSub = new HisTreatmentManager(paramGet).Get(filterTreatment);
                    Inventec.Common.Logging.LogSystem.Info("ListTreatmentSub" + ListTreatmentSub.Count.ToString());
                    NtListTreatment.AddRange(ListTreatmentSub);
                    filterTreatment = new HisTreatmentFilterQuery();
                    filterTreatment.CLINICAL_IN_TIME_TO = filter.TIME_TO;
                    filterTreatment.IS_PAUSE = true;
                    filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                    var listTreatmentOut = new HisTreatmentManager(paramGet).Get(filterTreatment);
                    Inventec.Common.Logging.LogSystem.Info("listTreatmentOut" + listTreatmentOut.Count.ToString());
                    NtListTreatment.AddRange(listTreatmentOut);
                    Inventec.Common.Logging.LogSystem.Info("NtListTreatment" + NtListTreatment.Count.ToString());

                    //Đối tượng điều trị
                    var NtlistPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();

                    if (IsNotNullOrEmpty(NtListTreatment))
                    {
                        var NtlistTreatmentId = NtListTreatment.GroupBy(o => o.ID).Select(p => p.First().ID).ToList();
                        var skip = 0;
                        while (NtlistTreatmentId.Count - skip > 0)
                        {
                            var listIDs = NtlistTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                            {
                                TREATMENT_IDs = listIDs
                            };
                            var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                            NtlistPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                        }
                    }
                    NtlistPatientTypeAlter = NtlistPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ThenByDescending(p => p.ID).ToList();
                    foreach (var PatyAlter in NtlistPatientTypeAlter) if (!dicPatientTypeAlter.ContainsKey(PatyAlter.TREATMENT_ID)) dicPatientTypeAlter[PatyAlter.TREATMENT_ID] = PatyAlter;

                    var listPatientId = NtListTreatment.GroupBy(o => o.PATIENT_ID).Select(p => p.First().PATIENT_ID).ToList();
                    var listPatient = new List<HIS_PATIENT>();

                    //bệnh nhân nội trú
                    Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();
                    if (IsNotNullOrEmpty(listPatientId))
                    {
                        var skip = 0;
                        while (listPatientId.Count - skip > 0)
                        {
                            var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientFilterQuery PatientFilter = new HisPatientFilterQuery()
                            {
                                IDs = listIDs
                            };
                            var ListPatientSub = new HisPatientManager(paramGet).Get(PatientFilter);
                            listPatient.AddRange(ListPatientSub);
                        }
                    }
                    foreach (var patient in listPatient) if (!dicPatient.ContainsKey(patient.ID)) dicPatient[patient.ID] = patient;
                    var Branch = new HisBranchManager(paramGet).Get(new HisBranchFilterQuery());
                    if (filter.DEPARTMENT_ID == null)
                    {
                        //toan vien
                        OLD_TREAT = NtListTreatment.Where(o => o.CLINICAL_IN_TIME < filter.TIME_FROM).ToList().Count;

                        NEW_TREAT = NtListTreatment.Where(o => o.CLINICAL_IN_TIME > filter.TIME_FROM).ToList().Count;

                        NEW_TREAT_BH = NtListTreatment.Where(o => o.CLINICAL_IN_TIME > filter.TIME_FROM && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                        NEW_TREAT_VP = NtListTreatment.Where(o => o.CLINICAL_IN_TIME > filter.TIME_FROM && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                        TREAT_HN = NtListTreatment.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && IsNotNullOrEmpty(dicPatientTypeAlter[o.ID].HNCODE)).ToList().Count;

                        TREAT_LESSER_THAN_6 = NtListTreatment.Where(q => timeNow < 60000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;

                        //bệnh nhân ngoai tinh


                        TREAT_OUT_PROVINCE = NtListTreatment.Where(o => dicPatient.ContainsKey(o.PATIENT_ID) && IsNotNullOrEmpty(Branch) && IsNotNullOrEmpty(Branch.First().HEIN_PROVINCE_CODE) && IsNotNullOrEmpty(dicPatient[o.PATIENT_ID].PROVINCE_CODE) && dicPatient[o.PATIENT_ID].PROVINCE_CODE != Branch.First().HEIN_PROVINCE_CODE).ToList().Count;

                        TREAT_RV = NtListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO && o.OUT_TIME >= filter.TIME_FROM).ToList().Count;

                        TP_TREAT_IN = NtListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME < filter.TIME_TO && o.OUT_TIME >= filter.TIME_FROM && o.TREATMENT_END_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).ToList().Count;

                        END_TREAT = NtListTreatment.Where(o => o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null).ToList().Count;

                        END_TREAT_BH = NtListTreatment.Where(o => (o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null) && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                        END_TREAT_VP = NtListTreatment.Where(o => (o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null) && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                        END_TREAT_LESSER_THAN_6 = NtListTreatment.Where(o => (o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null) && timeNow < 60000000000 + o.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;

                        END_TREAT_LESSER_THAN_15 = NtListTreatment.Where(o => (o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null) && timeNow >= 60000000000 + o.TDL_PATIENT_DOB && timeNow <= 150000000000 + o.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;

                        END_TREAT_OUT_PROVINCE = NtListTreatment.Where(o => (o.OUT_TIME >= filter.TIME_TO && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || o.IS_PAUSE == null) && dicPatient.ContainsKey(o.PATIENT_ID) && IsNotNullOrEmpty(Branch) && IsNotNullOrEmpty(Branch.First().HEIN_PROVINCE_CODE) && IsNotNullOrEmpty(dicPatient[o.PATIENT_ID].PROVINCE_CODE) && dicPatient[o.PATIENT_ID].PROVINCE_CODE != Branch.First().HEIN_PROVINCE_CODE).ToList().Count;

                        OLD_TREAT_DAY = NtListTreatment.Sum(o => DateDiff.diffDate(o.IN_TIME, o.OUT_TIME ?? filter.TIME_TO));
                        OUT_TREAT_DAY = NtListTreatment.Where(p => p.OUT_TIME < filter.TIME_TO && p.OUT_TIME >= filter.TIME_FROM).Sum(o => DateDiff.diffDate(o.IN_TIME, o.OUT_TIME ?? filter.TIME_TO));

                    }
                    else
                    {
                        //trong khoa
                        Dictionary<long, List<HIS_DEPARTMENT_TRAN>> dicInDepartment = new Dictionary<long, List<HIS_DEPARTMENT_TRAN>>();
                        Dictionary<long, List<HIS_DEPARTMENT_TRAN>> dicOutDepartment = new Dictionary<long, List<HIS_DEPARTMENT_TRAN>>();
                        var ListDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                        var ListDepartmentTranIn = new List<HIS_DEPARTMENT_TRAN>();
                        var ListDepartmentTranOut = new List<HIS_DEPARTMENT_TRAN>();
                        if (IsNotNullOrEmpty(NtListTreatment))
                        {
                            //
                            var NtlistTreatmentId = NtListTreatment.GroupBy(o => o.ID).Select(p => p.First().ID).ToList();
                            var skip = 0;
                            //vao khoa, ra khoa
                            while (NtlistTreatmentId.Count - skip > 0)
                            {
                                var listIDs = NtlistTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery()
                                {
                                    TREATMENT_IDs = listIDs
                                };
                                var ListDepartmentTranSub = new HisDepartmentTranManager(paramGet).Get(departmentTranFilter);
                                ListDepartmentTran.AddRange(ListDepartmentTranSub);
                            }
                            //vao
                            ListDepartmentTranIn = ListDepartmentTran.Where(o => o.DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                            //ra
                            ListDepartmentTranOut = ListDepartmentTran.Where(o => previous(o, ListDepartmentTran).DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();

                            dicInDepartment = ListDepartmentTranIn.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());
                            dicOutDepartment = ListDepartmentTranOut.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());
                        }

                        //vao khoa truoc han cuoi va ra khoa sau han dau
                        NtListTreatment = NtListTreatment.Where(o => dicInDepartment.ContainsKey(o.ID) && dicInDepartment[o.ID].Where(p => p.DEPARTMENT_IN_TIME < filter.TIME_TO).Count() > 0 && ((dicOutDepartment.ContainsKey(o.ID) && dicOutDepartment[o.ID].Where(p => p.DEPARTMENT_IN_TIME > filter.TIME_FROM).Count() > 0) || !dicOutDepartment.ContainsKey(o.ID))).ToList();

                        if (IsNotNullOrEmpty(NtListTreatment))
                        {
                            OLD_TREAT = NtListTreatment.Where(o => dicInDepartment.ContainsKey(o.ID) && dicInDepartment[o.ID].FirstOrDefault().DEPARTMENT_IN_TIME < filter.TIME_FROM).ToList().Count;

                            NEW_TREAT = NtListTreatment.Where(o => dicInDepartment.ContainsKey(o.ID) && dicInDepartment[o.ID].FirstOrDefault().DEPARTMENT_IN_TIME > filter.TIME_FROM).ToList().Count;

                            NEW_TREAT_BH = NtListTreatment.Where(o => dicInDepartment.ContainsKey(o.ID) && dicInDepartment[o.ID].FirstOrDefault().DEPARTMENT_IN_TIME > filter.TIME_FROM && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                            NEW_TREAT_VP = NtListTreatment.Where(o => dicInDepartment.ContainsKey(o.ID) && dicInDepartment[o.ID].FirstOrDefault().DEPARTMENT_IN_TIME > filter.TIME_FROM && dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;

                            TREAT_HN = NtListTreatment.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && IsNotNullOrEmpty(dicPatientTypeAlter[o.ID].HNCODE)).ToList().Count;

                            TREAT_LESSER_THAN_6 = NtListTreatment.Where(q => timeNow < 60000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;

                            TREAT_LESSER_THAN_15 = NtListTreatment.Where(q => timeNow >= 60000000000 + q.TDL_PATIENT_DOB && timeNow <= 150000000000 + q.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;

                            TREAT_OUT_PROVINCE = NtListTreatment.Where(o => dicPatient.ContainsKey(o.PATIENT_ID) && IsNotNullOrEmpty(Branch) && IsNotNullOrEmpty(Branch.First().HEIN_PROVINCE_CODE) && IsNotNullOrEmpty(dicPatient[o.PATIENT_ID].PROVINCE_CODE) && dicPatient[o.PATIENT_ID].PROVINCE_CODE != Branch.First().HEIN_PROVINCE_CODE).ToList().Count;

                            TREAT_RV = NtListTreatment.Where(o =>o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME>=filter.TIME_FROM && o.OUT_TIME<filter.TIME_TO&& o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList().Count;

                            TP_TREAT_IN = NtListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME >= filter.TIME_FROM && o.OUT_TIME < filter.TIME_TO && o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).ToList().Count;

                            var endTreat = NtListTreatment.Where(o => (dicOutDepartment.ContainsKey(o.ID) && dicOutDepartment[o.ID].FirstOrDefault().DEPARTMENT_IN_TIME > filter.TIME_TO) || !dicOutDepartment.ContainsKey(o.ID)).ToList();
                            END_TREAT = endTreat.Count;
                            END_TREAT_BH = endTreat.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;
                            END_TREAT_VP = endTreat.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList().Count;
                            END_TREAT_LESSER_THAN_6 = endTreat.Where(o => timeNow < 60000000000 + o.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;
                            END_TREAT_LESSER_THAN_15 = endTreat.Where(o => timeNow >= 60000000000 + o.TDL_PATIENT_DOB && timeNow <= 150000000000 + o.TDL_PATIENT_DOB).GroupBy(o => o.ID).Select(p => p.First().ID).ToList().Count;
                            END_TREAT_OUT_PROVINCE = endTreat.Where(o => dicPatient.ContainsKey(o.PATIENT_ID) && IsNotNullOrEmpty(Branch) && IsNotNullOrEmpty(Branch.First().HEIN_PROVINCE_CODE) && IsNotNullOrEmpty(dicPatient[o.PATIENT_ID].PROVINCE_CODE) && dicPatient[o.PATIENT_ID].PROVINCE_CODE != Branch.First().HEIN_PROVINCE_CODE).ToList().Count;
                            OLD_TREAT_DAY = NtListTreatment.Sum(o => DateDiff.diffDate(o.IN_TIME, o.OUT_TIME ?? filter.TIME_TO));
                            OUT_TREAT_DAY = NtListTreatment.Where(p => p.OUT_TIME < filter.TIME_TO && p.OUT_TIME >= filter.TIME_FROM).Sum(o => DateDiff.diffDate(o.IN_TIME, o.OUT_TIME ?? filter.TIME_TO));
                            Inventec.Common.Logging.LogSystem.Info("OUT_TREAT_DAY" + OUT_TREAT_DAY.ToString());

                        }
                        NtListTreatment.Clear();
                        NtlistPatientTypeAlter.Clear();
                        dicPatientTypeAlter.Clear();
                        dicPatient.Clear();
                        ListDepartmentTran.Clear();
                        listPatient.Clear();
                        dicInDepartment.Clear();
                        dicOutDepartment.Clear();
                        ListDepartmentTranIn.Clear();
                        ListDepartmentTranOut.Clear();
                        NtListTreatment = null;
                        NtlistPatientTypeAlter = null;
                        dicPatientTypeAlter = null;
                        dicPatient = null;
                        ListDepartmentTran = null;
                        listPatient = null;
                        dicInDepartment = null;
                        dicOutDepartment = null;
                        ListDepartmentTranIn = null;
                        ListDepartmentTranOut = null;
                    }
                }
                TRAN_PATI = TP_TREAT_IN + TP_TREAT_OUT;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private HIS_DEPARTMENT_TRAN previous(HIS_DEPARTMENT_TRAN p, List<HIS_DEPARTMENT_TRAN> l)
        {
            var prev = l.Where(o => o.ID == p.PREVIOUS_ID).ToList();
            return (prev != null && prev.Count > 0) ? prev.First() : new HIS_DEPARTMENT_TRAN();
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
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

            if (((Mrs00444Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00444Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00444Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00444Filter)reportFilter).TIME_TO));
            }
            if (((Mrs00444Filter)reportFilter).DEPARTMENT_ID != null)
            {
                var departments = HisDepartmentCFG.DEPARTMENTs.Where(o => o.ID == ((Mrs00444Filter)reportFilter).DEPARTMENT_ID).ToList();
                dicSingleTag.Add("DEPARTMENT_NAME", IsNotNullOrEmpty(departments) ? departments.First().DEPARTMENT_NAME : "");
            }

            dicSingleTag.Add("TOTAL_EXAM", TOTAL_EXAM);
            dicSingleTag.Add("EMERGENCY", EMERGENCY);
            dicSingleTag.Add("BHYT", BHYT);
            dicSingleTag.Add("FEE", FEE);
            dicSingleTag.Add("HN", HN);
            dicSingleTag.Add("TE", TE);
            dicSingleTag.Add("KH_LESS_THAN_6", KH_LESS_THAN_6);
            dicSingleTag.Add("OUT_PROVINCE", OUT_PROVINCE);
            dicSingleTag.Add("KSK", KSK);
            dicSingleTag.Add("TOTAL_TREATMENT", TOTAL_TREATMENT);
            //số bệnh nhân khám
            dicSingleTag.Add("TM_HN", TM_HN);
            dicSingleTag.Add("TM_TE", TM_TE);
            dicSingleTag.Add("TM_OUT_PROVINCE", TM_OUT_PROVINCE);
            dicSingleTag.Add("TM_KSK", TM_KSK);
            dicSingleTag.Add("TM_OUT_TREAT", TM_OUT_TREAT);
            //PTTT
            dicSingleTag.Add("TOTAL_SURG", TOTAL_SURG);
            dicSingleTag.Add("TOTAL_MISU", TOTAL_MISU);
            //Chuyển tuyến
            dicSingleTag.Add("TRAN_PATI", TRAN_PATI);
            dicSingleTag.Add("TP_TREAT_IN", TP_TREAT_IN);
            dicSingleTag.Add("TP_TREAT_OUT", TP_TREAT_OUT);
            //Tử vong
            dicSingleTag.Add("DEATH", DEATH);
            dicSingleTag.Add("ADULTS", ADULTS);
            dicSingleTag.Add("LESSER_THAN_5", LESSER_THAN_5);
            dicSingleTag.Add("LESSER_THAN_15", LESSER_THAN_15);
            dicSingleTag.Add("D_TREAT", D_TREAT);
            dicSingleTag.Add("D_EXAM", D_EXAM);

            dicSingleTag.Add("D24_HOUR", D24_HOUR);
            dicSingleTag.Add("D24_ADULTS", D24_ADULTS);
            dicSingleTag.Add("D24_LESSER_THAN_5", D24_LESSER_THAN_5);
            dicSingleTag.Add("D24_LESSER_THAN_15", D24_LESSER_THAN_15);
            dicSingleTag.Add("D24_TREAT", D24_TREAT);
            dicSingleTag.Add("D24_EXAM", D24_EXAM);
            //CĐHA
            dicSingleTag.Add("DIIM", DIIM);
            dicSingleTag.Add("SUIM", SUIM);
            dicSingleTag.Add("SCAN", SCAN);
            //Thăm dò chức năng
            dicSingleTag.Add("ECG", ECG);
            dicSingleTag.Add("ENDO", ENDO);
            //Điều trị
            dicSingleTag.Add("OLD_TREAT", OLD_TREAT);
            dicSingleTag.Add("NEW_TREAT", NEW_TREAT);
            dicSingleTag.Add("NEW_TREAT_BH", NEW_TREAT_BH);
            dicSingleTag.Add("NEW_TREAT_VP", NEW_TREAT_VP);
            dicSingleTag.Add("TREAT_HN", TREAT_HN);
            dicSingleTag.Add("TREAT_LESSER_THAN_6", TREAT_LESSER_THAN_6);
            dicSingleTag.Add("TREAT_LESSER_THAN_15", TREAT_LESSER_THAN_15);
            dicSingleTag.Add("TREAT_OUT_PROVINCE", TREAT_OUT_PROVINCE);
            dicSingleTag.Add("TREAT_RV", TREAT_RV);
            dicSingleTag.Add("END_TREAT", END_TREAT);
            dicSingleTag.Add("END_TREAT_BH", END_TREAT_BH);
            dicSingleTag.Add("END_TREAT_VP", END_TREAT_VP);
            dicSingleTag.Add("END_TREAT_LESSER_THAN_6", END_TREAT_LESSER_THAN_6);
            dicSingleTag.Add("END_TREAT_LESSER_THAN_15", END_TREAT_LESSER_THAN_15);
            dicSingleTag.Add("END_TREAT_OUT_PROVINCE", END_TREAT_OUT_PROVINCE);
            dicSingleTag.Add("OLD_TREAT_DAY", OLD_TREAT_DAY);
            dicSingleTag.Add("OUT_TREAT_DAY", OUT_TREAT_DAY);
             //thông tin khám bổ sung
            dicSingleTag.Add("VP", VP);
            dicSingleTag.Add("KH_BHYT_LESS_THAN_6", KH_BHYT_LESS_THAN_6);
            dicSingleTag.Add("KH_VP_LESS_THAN_6", KH_VP_LESS_THAN_6);
            dicSingleTag.Add("KH_MORE_THAN_60", KH_MORE_THAN_60);
            dicSingleTag.Add("KH_BHYT_MORE_THAN_60", KH_BHYT_MORE_THAN_60);
            dicSingleTag.Add("KH_VP_MORE_THAN_60", KH_VP_MORE_THAN_60);
            dicSingleTag.Add("KH_FOREIGNER", KH_FOREIGNER);
            dicSingleTag.Add("TOTAL_SURG_FINISH", TOTAL_SURG_FINISH);
            dicSingleTag.Add("TOTAL_SURG_FINISH_GROUP_2", TOTAL_SURG_FINISH_GROUP_2);
            dicSingleTag.Add("TOTAL_SURG_FINISH_GROUP_3", TOTAL_SURG_FINISH_GROUP_3);
            dicSingleTag.Add("TOTAL_MISU_FINISH", TOTAL_MISU_FINISH);
            dicSingleTag.Add("TOTAL_MISU_FINISH_GROUP_1", TOTAL_MISU_FINISH_GROUP_1);
            dicSingleTag.Add("TOTAL_MISU_FINISH_GROUP_2", TOTAL_MISU_FINISH_GROUP_2);
            dicSingleTag.Add("TOTAL_MISU_FINISH_GROUP_3", TOTAL_MISU_FINISH_GROUP_3);
            dicSingleTag.Add("TOTAL_MISU_FINISH_GROUP_4", TOTAL_MISU_FINISH_GROUP_4);
            dicSingleTag.Add("PATIENT_TREAT_OUT", PATIENT_TREAT_OUT);
            dicSingleTag.Add("TP_TREAT_OUT_01", TP_TREAT_OUT_01);
            dicSingleTag.Add("TP_TREAT_OUT_02", TP_TREAT_OUT_02);
            dicSingleTag.Add("TP_TREAT_OUT_KHAC", TP_TREAT_OUT_KHAC);
        }
    }

}
