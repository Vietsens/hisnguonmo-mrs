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
using MOS.MANAGER.HisTreatment;
using Inventec.Common.Logging;
using System.Reflection;
using System.Data;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisMilitaryRank;

namespace MRS.Processor.Mrs00002
{
    public class Mrs00002Processor : AbstractProcessor
    {
        Mrs00002Filter castFilter = null;
        List<Mrs00002RDO> listExam = new List<Mrs00002RDO>();
        List<HIS_PATIENT_TYPE> listCardType = new List<HIS_PATIENT_TYPE>();
        List<TREATMENT> listHisTreatment = new List<TREATMENT>();
        List<PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<PATIENT_TYPE_ALTER>();
        private Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();
        private Dictionary<long, long> dicPatientClassify = new Dictionary<long, long>();
        List<List<DataTable>> dataObject = new List<List<DataTable>>();
        List<long> KccDepartmentIds = new List<long>();

        List<TREATMENT> listHisTreatmentIdRemove = new List<TREATMENT>();
        List<TREATMENT> listTreatOfKCC_KKB = new List<TREATMENT>();
        List<SERVICE_REQ> listServiceReqOfKCC_KKB = new List<SERVICE_REQ>();
        List<long> Holidays = new List<long>();

        List<OLD_PATIENT> listOldTreatment = new List<OLD_PATIENT>();
        List<HIS_MILITARY_RANK> ListMilitary = new List<HIS_MILITARY_RANK>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();


        long PatientTypeIdKsk = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
        long PatientTypeIdFree = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
        long PatientTypeIdFee = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        long PatientTypeIdBH = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        List<long> PatientTypeIdBHs = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP1 ?? new List<long>();
        List<long> PatientTypeIdFees = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_IDs__REPORT_GROUP2 ?? new List<long>();


        public Mrs00002Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00002Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            List<SERVICE_REQ> listTemp = null;
            try
            {
                CommonParam getParam = new CommonParam();
                this.castFilter = (Mrs00002Filter)reportFilter;

                //danh sách điều kiện lọc
                PropertyInfo[] pr = typeof(Mrs00002Filter).GetProperties();
                foreach (var item in pr)
                {
                    try
                    {
                        var value = item.GetValue(this.castFilter);
                        if (value != null && !dicSingleKey.ContainsKey(item.Name))
                        {
                            dicSingleKey[item.Name] = value.ToString();
                        }

                    }
                    catch (Exception ex)
                    {
                        Inventec.Common.Logging.LogSystem.Debug(ex);
                    }
                }

                for (int i = 0; i < 15; i++)
                {
                    dataObject.Add(new MRS.MANAGER.Core.MrsReport.Lib.ManagerSql().GetSum<Mrs00002Filter>(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 1, i + 1,1)) ?? new List<DataTable>());
                }

                //luôn có 15 dòng
                //có dữ liệu thì bỏ qua
                if (dataObject.Count > 0)
                {
                    List<DataTable> lstObject = dataObject.SelectMany(s => s).ToList();
                    foreach (DataTable item in lstObject)
                    {
                        if (item.Rows != null && item.Rows.Count > 0)
                        {
                            if (!(this.dicDataFilter.ContainsKey("KEY_GET_TOGETHER_A1") && this.dicDataFilter["KEY_GET_TOGETHER_A1"] != null && this.dicDataFilter["KEY_GET_TOGETHER_A1"].ToString() == "YES"))
                                return true;
                        }
                    }
                }

                ////xóa bỏ dữ liệu rỗng để chạy theo code ban đầu
                //dataObject = new List<List<DataTable>>();
                //HisMilitaryRankFilterQuery militaryFilter = new HisMilitaryRankFilterQuery();
                //militaryFilter.IS_ACTIVE = 1;
                ListMilitary = new HisMilitaryRankManager().Get(new HisMilitaryRankFilterQuery());// quân hàm
                listTemp = new ManagerSql().GetServiceReq(castFilter);
                Inventec.Common.Logging.LogSystem.Info("finish get listTemp. Count Record:" + listTemp.Count);
                listHisTreatment = new ManagerSql().GetTreatment(castFilter);
                Inventec.Common.Logging.LogSystem.Info("finish get listHisTreatment. Count Record:" + listHisTreatment.Count);
                LisPatientTypeAlter = new ManagerSql().GetPatientTypeAlter(castFilter);
                listOldTreatment = new ManagerSql().GetOldPatient(castFilter);

                if (castFilter.DEPARTMENT_CODE__OUTPATIENTs != null)
                {
                    List<string> KCCDepartmentCodes = castFilter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                    var KccDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)).ToList() ?? new List<HIS_DEPARTMENT>();
                    KccDepartmentIds = KccDepartments.Select(o => o.ID).ToList();
                    //Bo cac ho so thoa man thoi gian bao cao nhung vao dieu tri ngoai tru, noi tru o khoa kham benh, phong cap cuu.
                    listHisTreatmentIdRemove = new ManagerSql().GetByIdRemove(castFilter, KccDepartmentIds) ?? new List<TREATMENT>();

                    //Them cac ho so dieu tri noi tru, ngoai tru tai phong cap cuu, khoa kham benh ra khoi khoa trong thoi gian bao cao:
                    listTreatOfKCC_KKB = new ManagerSql().GetByTreatmentOutOfKCC_KKB(castFilter, KccDepartmentIds) ?? new List<TREATMENT>();
                    var listHisTreatmentIdTreatOfKCC_KKB = listTreatOfKCC_KKB.Select(o => o.ID).ToList();
                    listServiceReqOfKCC_KKB = new ManagerSql().GetServiceReqOutOfKCC_KKB(castFilter, KccDepartmentIds) ?? new List<SERVICE_REQ>();
                    var listPtaOfKCC_KKB = new ManagerSql().GetPtaOfKCC_KKB(castFilter, KccDepartmentIds) ?? new List<PATIENT_TYPE_ALTER>();
                    if (listTreatOfKCC_KKB != null && listTreatOfKCC_KKB.Count > 0)
                    {
                        listHisTreatment.AddRange(listTreatOfKCC_KKB);
                    }
                    if (listServiceReqOfKCC_KKB != null && listServiceReqOfKCC_KKB.Count > 0)
                    {
                        listTemp.AddRange(listServiceReqOfKCC_KKB);
                    }
                    if (listPtaOfKCC_KKB != null && listPtaOfKCC_KKB.Count > 0)
                    {
                        LisPatientTypeAlter.AddRange(listPtaOfKCC_KKB);
                    }
                }

                if (castFilter.IS_COUNT_REQ != true)
                {
                    //Gộp dịch vụ khám theo phòng thực hiện
                    listTemp = listTemp.OrderBy(q => q.INTRUCTION_TIME).GroupBy(o => new { o.TREATMENT_ID, o.EXECUTE_ROOM_ID }).Select(p => p.First()).ToList();
                }
                Inventec.Common.Logging.LogSystem.Info("listTemp" + listTemp.Count);

                listCardType = listHisTreatment.Where(o => o.TDL_HEIN_CARD_NUMBER != null).GroupBy(p => TypeCard(p.TDL_HEIN_CARD_NUMBER)).Select(q => new HIS_PATIENT_TYPE() { PATIENT_TYPE_NAME = q.Key, ID = q.Count() }).ToList();


                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    listTemp = listTemp.Where(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                }

                GetHolidays();
                if (castFilter.IS_HOLIDAYS == true && castFilter.IS_NOT_HOLIDAYS != true)
                {
                    listTemp = listTemp.Where(o => Holidays.Contains(o.INTRUCTION_DATE)).ToList();
                }
                if (castFilter.IS_NOT_HOLIDAYS == true && castFilter.IS_HOLIDAYS != true)
                {
                    listTemp = listTemp.Where(o => !Holidays.Contains(o.INTRUCTION_DATE)).ToList();
                }
                var patientId = listHisTreatment.Select(x => x.PATIENT_ID).Distinct().ToList();
                var skip = 0;
                while (patientId.Count - skip > 0)
                {
                    var limit = patientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                    patientFilter.IDs = limit;
                    var patients = new MOS.MANAGER.HisPatient.HisPatientManager().Get(patientFilter);
                    listPatient.AddRange(patients);
                }
                dicRoom = HisRoomCFG.HisRooms.ToDictionary(o => o.ID);
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
        private void GetHolidays()
        {
            try
            {
                List<string> strHolidays = new List<string>() { "1_1", "30_4", "1_5", "2_9" };
                List<string> strLunarHolidays = new List<string>() { "10_3", "30_12", "29_12", "1_1", "2_1", "3_1" };
                List<DateTime> dateHolidays = new List<DateTime>();
                int year = DateTime.Today.Year;
                DateTime startDate = new DateTime(year - 1, 1, 1);
                DateTime endDate = new DateTime(year + 1, 1, 1);
                for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                {
                    if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                    //var vcal = new convertSolar2Lunar();
                    //int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    //var tempDay = arr[0] + "_" + arr[1];
                    if (strHolidays.Contains(string.Format("{0}_{1}", i.Day, i.Month)))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }

                    var vcal = new convertSolar2Lunar();
                    int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    var tempDay = arr[0] + "_" + arr[1];
                    if (strLunarHolidays.Contains(tempDay))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }


        private string TypeCard(string HeinCardNumber)
        {
            string result = "";
            try
            {
                result = HeinCardNumber.Substring(0, 3) ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = "";
            }
            return result;
        }

        private void ProcessExam(List<SERVICE_REQ> listTemp)
        {
            try
            {
                Dictionary<long, Mrs00002RDO> dicGroupByRoomExam = new Dictionary<long, Mrs00002RDO>();
                //listTemp = listTemp.Where(o => HisRoomCFG.HisRooms.Exists(p => p.ID == o.REQUEST_ROOM_ID
                //        && (p.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD || p.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE))).ToList();
                //var groupByRoom = listTemp.GroupBy(o => o.EXECUTE_ROOM_ID).ToList();

                foreach (var item in listTemp)
                {
                    var treatment = listHisTreatment.FirstOrDefault(p => p.ID == item.TREATMENT_ID);
                    if (castFilter.BANNGAY_IS_NOITRU !=false && treatment != null && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    {
                        treatment.TDL_TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                    }
                    var IsOldTreatment = listOldTreatment.Select(o => o.TREATMENT_ID).Contains(item.TREATMENT_ID);
                    var patientTypeAlter = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == item.TREATMENT_ID).ToList();
                    //Bo cac ho so thoa man thoi gian bao cao nhung vao dieu tri ngoai tru, noi tru o khoa kham benh, phong cap cuu.
                    if (castFilter.DEPARTMENT_CODE__OUTPATIENTs != null)
                    {
                        var treatmentIdRemove = listHisTreatmentIdRemove.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                        var treatmentIdAdd = listTreatOfKCC_KKB.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                        if (treatmentIdRemove != null && treatmentIdAdd == null)
                        {
                            continue;
                        }
                    }
                    if (treatment == null)
                    {
                        continue;
                    }
                    if (patientTypeAlter == null)
                    {
                        continue;
                    }

                    var lastPati = patientTypeAlter.OrderByDescending(o => o.LOG_TIME).FirstOrDefault() ?? new PATIENT_TYPE_ALTER();
                    var listServiceReqSame = listTemp.Where(o => o.TREATMENT_ID == item.TREATMENT_ID).ToList();
                    bool IsLastRoomExam = (item.EXECUTE_ROOM_ID == treatment.IN_ROOM_ID || item.EXECUTE_ROOM_ID == treatment.END_ROOM_ID) 
                        //phòng cuối cùng là phòng cho nhập viện hoặc cho về. thêm check các công khám khác ở phòng để loại bỏ trường hợp phòng nhập viện hoặc cho về có 2 công khám thì chỉ lấy công khám sau
                        && !listServiceReqSame.Exists(o => o.ID != item.ID && o.INTRUCTION_TIME > item.INTRUCTION_TIME && o.EXECUTE_ROOM_ID == item.EXECUTE_ROOM_ID);
                    Mrs00002RDO rdo = new Mrs00002RDO();
                    if (!dicGroupByRoomExam.ContainsKey(item.EXECUTE_ROOM_ID))
                    {
                        dicGroupByRoomExam[item.EXECUTE_ROOM_ID] = rdo;
                        rdo.EXECUTE_ROOM_ID = item.EXECUTE_ROOM_ID;
                    }
                    else
                    {
                        rdo = dicGroupByRoomExam[item.EXECUTE_ROOM_ID];
                    }
                    var patient = listPatient.Where(x => x.ID == treatment.PATIENT_ID).FirstOrDefault();
                    if (patient != null)
                    {
                        var militerRank = ListMilitary.Where(x => x.ID == patient.MILITARY_RANK_ID).FirstOrDefault();
                        if (militerRank != null)
                        {
                            rdo.MILITARY_RANK_NUM_ORDER = militerRank.NUM_ORDER;
                        }
                    }

                    rdo.HEIN_CARD_TYPE = !string.IsNullOrEmpty(treatment.TDL_HEIN_CARD_NUMBER) ? treatment.TDL_HEIN_CARD_NUMBER.Substring(0, 2) : "";
                    rdo.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                    rdo.TDL_PATIENT_CLASSIFY_ID = item.TDL_PATIENT_CLASSIFY_ID ?? 0;
                    rdo.PATIENT_TYPE_ID = lastPati.PATIENT_TYPE_ID;
                    rdo.HEIN_RATIO = item.HEIN_RATIO ?? 0;
                    rdo.AGE = RDOCommon.CalculateAge(item.TDL_PATIENT_DOB);
                    CountPatientClassifyGroup(rdo, item.EXECUTE_ROOM_ID, item.TDL_PATIENT_CLASSIFY_ID);
                    CountExam(rdo);
                    CountExamBhyt(rdo, lastPati.PATIENT_TYPE_ID);
                    CountExeReq(item.EXECUTE_ROOM_ID, item.REQUEST_ROOM_ID, rdo);
                    CountExamCACC(rdo);
                    CountKsknnExam(treatment.TDL_PATIENT_NATIONAL_NAME, rdo);
                    CountKskExam(lastPati.PATIENT_TYPE_ID, treatment.TDL_HEIN_CARD_NUMBER, rdo);
                    CountOldExam(IsOldTreatment, rdo);
                    CountExamMain(item.IS_MAIN_EXAM, rdo);
                    CountExamMainThisProvince(item.IS_MAIN_EXAM, treatment.IS_IN_PROVINCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE, rdo);
                    CountExamMainOtherProvince(item.IS_MAIN_EXAM, treatment.IS_IN_PROVINCE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE, rdo);
                    CountExamMainBH(item.IS_MAIN_EXAM, lastPati.PATIENT_TYPE_ID, treatment.TDL_HEIN_CARD_NUMBER, rdo);
                    CountExamMainDV(item.IS_MAIN_EXAM, lastPati.PATIENT_TYPE_ID, rdo);
                    CountExamExtentBH(item.IS_MAIN_EXAM, lastPati.PATIENT_TYPE_ID, treatment.TDL_HEIN_CARD_NUMBER, rdo);
                    CountExamExtentDV(item.IS_MAIN_EXAM, lastPati.PATIENT_TYPE_ID, rdo);
                    CountExamFromRea(item.REQUEST_ROOM_ID, rdo);
                    CountExamFromReaBH(item.REQUEST_ROOM_ID, lastPati.PATIENT_TYPE_ID, rdo);
                    CountExamFromReaDV(item.REQUEST_ROOM_ID, lastPati.PATIENT_TYPE_ID, rdo);
                    CountFemaleExam(item.TDL_PATIENT_GENDER_ID ?? 0, rdo);
                    CountMaleExam(item.TDL_PATIENT_GENDER_ID ?? 0, rdo);
                    CountEmergencyExam(item.IS_EMERGENCY, lastPati.PATIENT_TYPE_ID, treatment.TDL_HEIN_CARD_NUMBER, rdo);
                    CountKskContractExam(treatment.TDL_KSK_CONTRACT_ID, rdo);
                    CountChildExam(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo);
                    CountChildExamBhyt(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo,lastPati.PATIENT_TYPE_ID);
                    CountChildExamExeReq(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo, item.EXECUTE_ROOM_ID, item.REQUEST_ROOM_ID);
                    CountElderlyExam(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo);
                    CountElderlyExamExeReq(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo, item.EXECUTE_ROOM_ID, item.REQUEST_ROOM_ID);
                    if (lastPati.PATIENT_TYPE_ID == PatientTypeIdBH)
                    {
                        CountElderlyExamBH(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo);
                    }

                    else
                    {
                        CountElderlyExamVP(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, rdo);
                    }

                    CountTreatmentExam(item.REQUEST_ROOM_ID, rdo);
                    CountOldTreatmentExam(item.REQUEST_ROOM_ID, rdo,IsOldTreatment);
                    CountMoveRoomExam(item.ID, listServiceReqSame, rdo);
                    CountImRoRoomExam(item.PREVIOUS_SERVICE_REQ_ID??0, listServiceReqSame, rdo);
                    CountDoneExam(item.SERVICE_REQ_STT_ID, rdo);
                    if (IsLastRoomExam)
                    {
                        CountInTreatExam(treatment, item.EXECUTE_ROOM_ID, item.FINISH_TIME ?? 0, KccDepartmentIds, rdo);
                        CountClinicalIn(treatment, item.EXECUTE_ROOM_ID, rdo);
                        CountClinicalInNotNight(treatment, item.EXECUTE_ROOM_ID, rdo);
                        CountMediHomeExam(item.EXECUTE_ROOM_ID, treatment, rdo);
                        if (lastPati.PATIENT_TYPE_ID == PatientTypeIdBH)
                        {
                            CountMediHomeExamBH(item.EXECUTE_ROOM_ID, treatment, rdo);
                        }
                        if (treatment.TDL_KSK_CONTRACT_ID > 0)
                        {
                            CountMediHomeKskCon(item.EXECUTE_ROOM_ID, treatment, rdo);
                        }
                        CountMediHomeKsknn(item.EXECUTE_ROOM_ID, treatment, rdo);
                        CountMediHomeOld(item.EXECUTE_ROOM_ID, IsOldTreatment, treatment, rdo);
                        CountTranpatiExam(item.EXECUTE_ROOM_ID, treatment, rdo);
                        CountTranpatiNotTreatIn(item.EXECUTE_ROOM_ID, treatment, item.FINISH_TIME ?? 0, KccDepartmentIds, rdo);
                        CountOutTreatExam(treatment, item.EXECUTE_ROOM_ID, item.FINISH_TIME ?? 0, KccDepartmentIds, rdo);

                        CountOutTreatExamChild(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, treatment, item.EXECUTE_ROOM_ID, rdo);

                        CountDeathExam(item.EXECUTE_ROOM_ID, treatment, rdo);
                        CountTreatExamChild(item.TDL_PATIENT_DOB, item.INTRUCTION_TIME, treatment, item.EXECUTE_ROOM_ID, rdo);
                    }
                    CountPatientTypeGroup(lastPati.PATIENT_TYPE_ID, rdo, rdo.HEIN_CARD_TYPE);
                    CountPatientTypeGroupExeReq(lastPati.PATIENT_TYPE_ID, rdo, item.EXECUTE_ROOM_ID, item.REQUEST_ROOM_ID);
                    CountPatientTypeKsk(lastPati.PATIENT_TYPE_ID, PatientTypeIdKsk, rdo);
                    CountPatientTypeFree(lastPati.PATIENT_TYPE_ID, PatientTypeIdFree, rdo);


                    CountLeftRouteExam(item, patientTypeAlter, rdo);
                }

                listExam = dicGroupByRoomExam.Values.OrderBy(r => r.EXECUTE_ROOM_ID).ToList();

                addInfo(ref listExam, listTemp);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CountMediHomeKsknn(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    if (!string.IsNullOrWhiteSpace(treatment.TDL_PATIENT_NATIONAL_NAME) && treatment.TDL_PATIENT_NATIONAL_NAME.ToLower() != "việt nam")
                    {
                        rdo.COUNT_MEDI_HOME_KSKNN++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void CountMediHomeOld(long executeRoomId, bool IsOldTreatment, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                   && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                   && treatment.END_ROOM_ID == executeRoomId)
                {
                    if (IsOldTreatment)
                    {
                        rdo.COUNT_MEDI_HOME_OLD++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private void addInfo(ref List<Mrs00002RDO> listExam, List<SERVICE_REQ> listTemp)
        {
            try
            {
                //them thong tin phong kham
                List<CountWithPatientType> listExamFinish = new ManagerSql().CountExamFinish(castFilter) ?? new List<CountWithPatientType>();
                List<CountWithPatientType> listExamWait = new ManagerSql().CountExamWait(castFilter) ?? new List<CountWithPatientType>();
                List<CountWithPatientClassify> listClassify = new ManagerSql().CountPatientClassify(castFilter) ?? new List<CountWithPatientClassify>();
                LogSystem.Info("count:" + string.Join(",", listClassify.Where(p => p.TDL_PATIENT_CLASSIFY_ID != null).Select(p => p.TDL_PATIENT_CLASSIFY_ID)));
                foreach (var item in listExam)
                {
                    item.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    item.EXECUTE_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_NAME;
                    item.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                    item.EXECUTE_DEPARTMENT_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_CODE;
                    item.DIC_COUNT_PATIENT_CLASSIFY = listClassify.Where(p => p.EXECUTE_ROOM_ID == item.EXECUTE_ROOM_ID && p.TDL_PATIENT_CLASSIFY_ID != null).GroupBy(p => p.PATIENT_CLASSIFY_CODE).ToDictionary(p => p.Key, y => y.Sum(p => p.COUNT_CLASSIFY ?? 0));
                    item.COUNT_CA_EXAM_PATIENT_CLASSIFY = item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT ? listClassify.Where(p => p.EXECUTE_ROOM_ID == item.EXECUTE_ROOM_ID && (p.PATIENT_CLASSIFY_NAME == "Công nhân viên công an" || p.PATIENT_CLASSIFY_NAME == "Công an hưu cao cấp")).Sum(s => s.COUNT_CLASSIFY) : 0;
                    item.COUNT_OTHER_EXAM_PATIENT_CLASSIFY = item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT ? listClassify.Where(p => p.EXECUTE_ROOM_ID == item.EXECUTE_ROOM_ID && p.PATIENT_CLASSIFY_NAME != "Công an hưu cao cấp" && p.PATIENT_CLASSIFY_NAME != "Công an hưu cao cấp").Sum(s => s.COUNT_CLASSIFY) : 0;
                    item.COUNT_FINISH_EXAM = listExamFinish.Where(o => o.ID == item.EXECUTE_ROOM_ID).Sum(s => (s.CountBhyt ?? 0) + (s.CountVp ?? 0));
                    item.COUNT_IMP = listTreatOfKCC_KKB.Where(o => o.IN_ROOM_ID == item.EXECUTE_ROOM_ID).Count();
                    item.COUNT_WAIT_EXAM = listExamWait.Where(o => o.ID == item.EXECUTE_ROOM_ID).Sum(s => (s.CountBhyt ?? 0) + (s.CountVp ?? 0));
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
        void CountExam(Mrs00002RDO rdo)
        {
            try
            {
                rdo.COUNT_EXAM++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamBhyt(Mrs00002RDO rdo,long patientTypeId)
        {
            try
            {
                if (PatientTypeIdBHs.Contains(rdo.PATIENT_TYPE_ID) || patientTypeId == PatientTypeIdBH)
                    rdo.COUNT_BH_EXAM++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExeReq(long executeRoomId, long requestRoomId, Mrs00002RDO rdo)
        {
            try
            {
                if (rdo.DIC_COUNT_EXE_REQ == null)
                {
                    rdo.DIC_COUNT_EXE_REQ = new Dictionary<string, long>();
                }
                if (dicRoom.ContainsKey(executeRoomId) && dicRoom.ContainsKey(requestRoomId))
                {
                    var executeRoom = dicRoom[executeRoomId];
                    var requestRoom = dicRoom[requestRoomId];
                    if (!rdo.DIC_COUNT_EXE_REQ.ContainsKey(string.Format("{0}_TO_{1}", requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)))
                    {
                        rdo.DIC_COUNT_EXE_REQ[string.Format("{0}_TO_{1}", requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)] = 1;
                    }
                    else
                    {
                        rdo.DIC_COUNT_EXE_REQ[string.Format("{0}_TO_{1}", requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)]++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExeReq(long executeRoomId, long requestRoomId, Mrs00002RDO rdo, string TypeKey)
        {
            try
            {
                if (rdo.DIC_COUNT_EXE_REQ == null)
                {
                    rdo.DIC_COUNT_EXE_REQ = new Dictionary<string, long>();
                }
                if (dicRoom.ContainsKey(executeRoomId) && dicRoom.ContainsKey(requestRoomId))
                {
                    var executeRoom = dicRoom[executeRoomId];
                    var requestRoom = dicRoom[requestRoomId];
                    if (!rdo.DIC_COUNT_EXE_REQ.ContainsKey(string.Format("{0}_TO_{1}" + TypeKey, requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)))
                    {
                        rdo.DIC_COUNT_EXE_REQ[string.Format("{0}_TO_{1}" + TypeKey, requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)] = 1;
                    }
                    else
                    {
                        rdo.DIC_COUNT_EXE_REQ[string.Format("{0}_TO_{1}" + TypeKey, requestRoom.ROOM_CODE, executeRoom.ROOM_CODE)]++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamCACC(Mrs00002RDO rdo)
        {
            try
            {
                if (rdo.MILITARY_RANK_NUM_ORDER != null)
                {
                    if (rdo.MILITARY_RANK_NUM_ORDER > 0 && rdo.MILITARY_RANK_NUM_ORDER <= 6)
                    {
                        rdo.COUNT_CA_NORMAL++;
                    }
                    else if (rdo.MILITARY_RANK_NUM_ORDER > 6)
                    {
                        rdo.COUNT_CA_HIGH_LEVEL++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountKsknnExam(string nationalName, Mrs00002RDO rdo)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nationalName) && nationalName.ToLower() != "việt nam")
                {
                    rdo.COUNT_KSKNN_EXAM++;
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountKskExam(long patientType, string heinCard, Mrs00002RDO rdo)
        {
            try
            {
                if (patientType != null)
                {
                    if (patientType == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        if (heinCard.Substring(0, 2) == "CA")
                        {
                            rdo.COUNT_KSK_EXAM_BHYT_CA++;
                        }
                        else
                        {
                            rdo.COUNT_KSK_EXAM_BHYT++;
                        }
                    }

                    else if (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == patientType).PATIENT_TYPE_NAME.ToLower() == "phạm nhân")
                    {
                        rdo.COUNT_KSK_EXAM_PN++;
                    }
                    else if (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == patientType).PATIENT_TYPE_NAME.ToLower().Contains("Cán bộ cao cấp"))
                    {
                        rdo.COUNT_KSK_EXAM_CBCC++;
                    }
                    else
                    {
                        rdo.COUNT_KSK_EXAM_VP++;
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountOldExam(bool IsOldTreatment, Mrs00002RDO rdo)
        {
            try
            {
                if (IsOldTreatment)
                {
                    rdo.COUNT_OLD_EXAM++;
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamFromRea(long RequestRoomId, Mrs00002RDO rdo)
        {
            try
            {
                V_HIS_ROOM RequestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == RequestRoomId);

                bool IsFromRea = RequestRoom != null && RequestRoom.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD;
                if (IsFromRea)
                    rdo.COUNT_EXAM_FROM_REA++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamFromReaBH(long RequestRoomId, long patientTypeId, Mrs00002RDO rdo)
        {
            try
            {
                V_HIS_ROOM RequestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == RequestRoomId);

                bool IsFromRea = RequestRoom != null && RequestRoom.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD;
                if (IsFromRea && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    rdo.COUNT_EXAM_FROM_REA_BH++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamFromReaDV(long RequestRoomId, long patientTypeId, Mrs00002RDO rdo)
        {
            try
            {
                V_HIS_ROOM RequestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == RequestRoomId);

                bool IsFromRea = RequestRoom != null && RequestRoom.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD;
                if (IsFromRea && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__DV)
                    rdo.COUNT_EXAM_FROM_REA_DV++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamMain(short? IsMainExam, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    rdo.COUNT_EXAM_MAIN++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamMainThisProvince(short? IsMainExam, bool IsThisProvince, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsThisProvince)
                    rdo.COUNT_EXAM_MAIN_IN_PROVIN++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountExamMainOtherProvince(short? IsMainExam, bool IsOtherProvince, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsOtherProvince)
                    rdo.COUNT_EXAM_MAIN_OUT_PROVIN++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamMainBH(short? IsMainExam, long patientTypeId, string heinCardNumber, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.COUNT_EXAM_MAIN_BH++;

                    if (!string.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.StartsWith("CA"))
                    {
                        rdo.COUNT_EXAM_MAIN_CA++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountExamMainDV(short? IsMainExam, long patientTypeId, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__DV)
                {
                    rdo.COUNT_EXAM_MAIN_DV++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountExamExtentBH(short? IsMainExam, long patientTypeId, string heinCardNumber, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.COUNT_EXAM_EXT_BH++;
                    if (!string.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.StartsWith("CA"))
                    {
                        rdo.COUNT_EXAM_EXT_CA++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountExamExtentDV(short? IsMainExam, long patientTypeId, Mrs00002RDO rdo)
        {
            try
            {
                if (IsMainExam != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__DV)
                    rdo.COUNT_EXAM_EXT_DV++;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountEmergencyExam(short? isEmergency, long patientType, string heinCard, Mrs00002RDO rdo)
        {
            try
            {
                if (isEmergency == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    rdo.COUNT_EMERGENCY_EXAM++;
                    if (patientType != null)
                    {
                        if (patientType == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            if (heinCard.Substring(0, 2) == "CA")
                            {
                                rdo.COUNT_EMERGENCY_EXAM_BHYT_CA++;
                            }
                            else
                            {
                                rdo.COUNT_EMERGENCY_EXAM_BHYT++;
                            }
                        }
                        else if (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == patientType).PATIENT_TYPE_NAME.ToLower() == "nước ngoài")
                        {
                            rdo.COUNT_EMERGENCY_EXAM_NN++;
                        }
                        else if (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == patientType).PATIENT_TYPE_NAME.ToLower() == "phạm nhân")
                        {
                            rdo.COUNT_EMERGENCY_EXAM_PN++;
                        }
                        else if (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == patientType).PATIENT_TYPE_NAME.ToLower().Contains("Cán bộ cao cấp"))
                        {
                            rdo.COUNT_EMERGENCY_EXAM_CBCC++;
                        }
                        else
                        {
                            rdo.COUNT_EMERGENCY_EXAM_VP++;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountKskContractExam(long? KskContractId, Mrs00002RDO rdo)
        {
            try
            {
                if (KskContractId > 0)
                {
                    rdo.COUNT_KSK_CONTRACT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountFemaleExam(long GenderId, Mrs00002RDO rdo)
        {
            try
            {
                if (GenderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    rdo.COUNT_FEMALE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountMaleExam(long GenderId, Mrs00002RDO rdo)
        {
            try
            {
                if (GenderId == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    rdo.COUNT_MALE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountTreatmentExam(long requestRoomId, Mrs00002RDO rdo)
        {
            try
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == requestRoomId) ?? new V_HIS_ROOM();
                if (room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD)
                {
                    rdo.COUNT_TREATMENT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountOldTreatmentExam(long requestRoomId, Mrs00002RDO rdo,bool IsOldTreatment)
        {
            try
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == requestRoomId) ?? new V_HIS_ROOM();
                if (room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD && IsOldTreatment)
                {
                    rdo.COUNT_OLD_TREATMENT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountChildExam(long Dob, long IntructionTime, Mrs00002RDO rdo)
        {
            try
            {
                if (this.Less(Dob, IntructionTime, 15))
                {
                    rdo.COUNT_CHILD_EXAM++;
                    if (this.Less(Dob, IntructionTime, 7))
                    {

                        rdo.COUNT_CHILD_EXAM_LESS7++;
                        if (this.Less(Dob, IntructionTime, 6))
                        {
                            rdo.COUNT_CHILD_EXAM_LESS6++;
                            if (this.Less(Dob, IntructionTime, 5))
                            {
                                rdo.COUNT_CHILD_EXAM_LESS5++;
                                if (this.Less(Dob, IntructionTime, 1))
                                {
                                    rdo.COUNT_CHILD_EXAM_LESS1++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountChildExamBhyt(long Dob, long IntructionTime, Mrs00002RDO rdo, long patientTypeId)
        {
            try
            {
                if ((PatientTypeIdBHs.Contains(rdo.PATIENT_TYPE_ID) || patientTypeId == PatientTypeIdBH) && this.Less(Dob, IntructionTime, 15))
                {
                    rdo.COUNT_CHILD_EXAM_BHYT++;
                    if (this.Less(Dob, IntructionTime, 7))
                    {

                        rdo.COUNT_CHILD_EXAM_BH_LESS7++;
                        if (this.Less(Dob, IntructionTime, 6))
                        {
                            rdo.COUNT_CHILD_EXAM_BH_LESS6++;
                            if (this.Less(Dob, IntructionTime, 5))
                            {
                                rdo.COUNT_CHILD_EXAM_BH_LESS5++;
                                if (this.Less(Dob, IntructionTime, 1))
                                {
                                    rdo.COUNT_CHILD_EXAM_BH_LESS1++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CountChildExamExeReq(long Dob, long IntructionTime, Mrs00002RDO rdo, long executeRoomId, long requestRoomId)
        {
            try
            {
                if (this.Less(Dob, IntructionTime, 15))
                {
                    CountExeReq(executeRoomId, requestRoomId, rdo, "_CHILD");
                    if (this.Less(Dob, IntructionTime, 7))
                    {
                        CountExeReq(executeRoomId, requestRoomId, rdo, "_LESS7");
                        if (this.Less(Dob, IntructionTime, 6))
                        {
                            CountExeReq(executeRoomId, requestRoomId, rdo, "_LESS6");
                            if (this.Less(Dob, IntructionTime, 5))
                            {
                                CountExeReq(executeRoomId, requestRoomId, rdo, "_LESS5");
                                if (this.Less(Dob, IntructionTime, 1))
                                {
                                    CountExeReq(executeRoomId, requestRoomId, rdo, "_LESS1");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountTreatExamChild(long Dob, long IntructionTime, TREATMENT treatment, long executeRoomId, Mrs00002RDO rdo)
        {
            try
            {
                bool check = false;
                if (castFilter.IS_NOT_CHECK_IN_TREAT == true && executeRoomId == treatment.IN_ROOM_ID && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    check = true;
                }
                if (castFilter.IS_NOT_CHECK_IN_TREAT != true && executeRoomId == treatment.IN_ROOM_ID && treatment.IN_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    check = true;
                }

                if (check)
                {
                    if (this.Less(Dob, IntructionTime, 15))
                    {
                        rdo.COUNT_TREAT_EXAM_LESS15++;
                        if (this.Less(Dob, IntructionTime, 7))
                        {

                            rdo.COUNT_TREAT_EXAM_LESS7++;
                            if (this.Less(Dob, IntructionTime, 6))
                            {
                                rdo.COUNT_TREAT_EXAM_LESS6++;
                                if (this.Less(Dob, IntructionTime, 5))
                                {
                                    rdo.COUNT_TREAT_EXAM_LESS5++;
                                    if (this.Less(Dob, IntructionTime, 1))
                                    {
                                        rdo.COUNT_TREAT_EXAM_LESS1++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountOutTreatExamChild(long Dob, long IntructionTime, TREATMENT treatment, long executeRoomId, Mrs00002RDO rdo)
        {
            try
            {
                bool check = false;
                if (castFilter.IS_NOT_CHECK_IN_TREAT == true && executeRoomId == treatment.IN_ROOM_ID && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    check = true;
                }
                if (castFilter.IS_NOT_CHECK_IN_TREAT != true && executeRoomId == treatment.IN_ROOM_ID && treatment.IN_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    check = true;
                }

                if (check)
                {
                    if (this.Less(Dob, IntructionTime, 15))
                    {
                        rdo.COUNT_OUT_TREAT_EXAM_LESS15++;
                        if (this.Less(Dob, IntructionTime, 7))
                        {

                            rdo.COUNT_OUT_TREAT_EXAM_LESS7++;
                            if (this.Less(Dob, IntructionTime, 6))
                            {
                                rdo.COUNT_OUT_TREAT_EXAM_LESS6++;
                                if (this.Less(Dob, IntructionTime, 5))
                                {
                                    rdo.COUNT_OUT_TREAT_EXAM_LESS5++;
                                    if (this.Less(Dob, IntructionTime, 1))
                                    {
                                        rdo.COUNT_OUT_TREAT_EXAM_LESS1++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private bool Less(long Dob, long IntructionTime, long Age)
        {
            return IntructionTime - Age * 10000000000 < Dob;
        }

        private bool More(long Dob, long IntructionTime, long Age)
        {
            return IntructionTime - Age * 10000000000 >= Dob;
        }

        void CountElderlyExam(long Dob, long IntructionTime, Mrs00002RDO rdo)
        {
            try
            {
                if (this.More(Dob, IntructionTime, 60))
                {
                    rdo.COUNT_ELDERLY_EXAM_MORE60++;
                    if (this.More(Dob, IntructionTime, 90))
                    {
                        rdo.COUNT_ELDERLY_EXAM_MORE90++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountElderlyExamExeReq(long Dob, long IntructionTime, Mrs00002RDO rdo, long executeRoomId, long requestRoomId)
        {
            try
            {
                if (this.More(Dob, IntructionTime, 60))
                {
                    CountExeReq(executeRoomId, requestRoomId, rdo, "_MORE60");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountElderlyExamBH(long Dob, long IntructionTime, Mrs00002RDO rdo)
        {
            try
            {
                if (this.More(Dob, IntructionTime, 60))
                {
                    rdo.COUNT_ELDERLY_EXAM_BH_MORE60++;
                    if (this.More(Dob, IntructionTime, 90))
                    {
                        rdo.COUNT_ELDERLY_EXAM_BH_MORE90++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountElderlyExamVP(long Dob, long IntructionTime, Mrs00002RDO rdo)
        {
            try
            {
                if (this.More(Dob, IntructionTime, 60))
                {
                    rdo.COUNT_ELDERLY_EXAM_VP_MORE60++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountMoveRoomExam(long serviceReqId, List<SERVICE_REQ> listPrevHisServiceReq, Mrs00002RDO rdo)
        {
            try
            {
                if (listPrevHisServiceReq.Exists(o => o.PREVIOUS_SERVICE_REQ_ID == serviceReqId))
                {
                    rdo.COUNT_MOVE_ROOM_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountImRoRoomExam(long previousServiceReqId, List<SERVICE_REQ> listPrevHisServiceReq, Mrs00002RDO rdo)
        {
            try
            {
                if (listPrevHisServiceReq.Exists(o => o.ID == previousServiceReqId))
                {
                    rdo.COUNT_IMRO_ROOM_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountInTreatExam(TREATMENT treatment, long executeRoomId, long? finishTime, List<long> KccDepartmentIds, Mrs00002RDO rdo)
        {
            try
            {
                bool check = false;
                if (castFilter.IS_NOT_CHECK_IN_TREAT == true && executeRoomId == treatment.IN_ROOM_ID && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && (!KccDepartmentIds.Contains(treatment.END_DEPARTMENT_ID ?? 0)))
                {
                    check = true;
                }
                if (castFilter.IS_NOT_CHECK_IN_TREAT != true && executeRoomId == treatment.IN_ROOM_ID && treatment.IN_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && (!KccDepartmentIds.Contains(treatment.END_DEPARTMENT_ID ?? 0)) && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.IS_PAUSE == null || treatment.OUT_DATE > finishTime))
                {
                    check = true;
                }

                if (check)
                {
                    rdo.COUNT_IN_TREAT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountClinicalIn(TREATMENT treatment, long executeRoomId, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && treatment.CLINICAL_IN_TIME != null && executeRoomId == treatment.IN_ROOM_ID)
                {
                    rdo.COUNT_CLINICAL_IN++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountClinicalInNotNight(TREATMENT treatment, long executeRoomId, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY && treatment.CLINICAL_IN_TIME != null && executeRoomId == treatment.IN_ROOM_ID)
                {
                    rdo.COUNT_CLINICAL_IN_NOT_NIGHT++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountOutTreatExam(TREATMENT treatment, long executeRoomId, long? finishTime, List<long> KccDepartmentIds, Mrs00002RDO rdo)
        {
            try
            {
                bool check = false;
                if (castFilter.IS_NOT_CHECK_IN_TREAT == true && executeRoomId == treatment.IN_ROOM_ID && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && (!KccDepartmentIds.Contains(treatment.END_DEPARTMENT_ID ?? 0)))
                {
                    check = true;
                }
                if (castFilter.IS_NOT_CHECK_IN_TREAT != true && executeRoomId == treatment.IN_ROOM_ID && treatment.IN_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && (!KccDepartmentIds.Contains(treatment.END_DEPARTMENT_ID ?? 0)) && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.IS_PAUSE == null || treatment.OUT_DATE > finishTime))
                {
                    check = true;
                }

                if (check)
                {
                    rdo.COUNT_OUT_TREAT_EXAM++;
                    rdo.DAY_OUT_TREAT_EXAM += DateDiff.diffDate(treatment.CLINICAL_IN_TIME ?? 0, treatment.OUT_TIME ?? 0);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountTranpatiExam(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_TRANPATI_EXAM++;
                    if (treatment.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_LIEN_KE || treatment.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_KHONG_LIEN_KE)
                    {
                        rdo.COUNT_TRANPATI_EXAM_LEN++;
                    }
                    if (treatment.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_LIEN_KE)
                    {
                        rdo.COUNT_EXAM_LEN_LIEN_KE++;
                    }
                    if (treatment.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__CUNG_TUYEN)
                    {
                        rdo.COUNT_EXAM_CUNG_TUYEN++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountTranpatiNotTreatIn(long executeRoomId, TREATMENT treatment, long? finishTime, List<long> KccDepartmentIds, Mrs00002RDO rdo)
        {
            try
            {
                bool check = false;
                if (castFilter.IS_NOT_CHECK_IN_TREAT == true && treatment.END_ROOM_ID == executeRoomId && treatment.IS_PAUSE == 1)
                {
                    check = true;
                }
                if (castFilter.IS_NOT_CHECK_IN_TREAT != true && treatment.IS_PAUSE == 1 && (treatment.END_ROOM_ID == executeRoomId || treatment.IN_ROOM_ID == executeRoomId) && !(treatment.IN_ROOM_ID == executeRoomId && (treatment.IN_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || treatment.IN_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU) && (!KccDepartmentIds.Contains(treatment.END_DEPARTMENT_ID ?? 0)) && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && (treatment.IS_PAUSE == null || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.OUT_DATE > finishTime)))
                {
                    check = true;
                }
                if (check)
                {
                    if (treatment.TRAN_PATI_FORM_ID.HasValue && treatment.TRAN_PATI_FORM_ID > 0)
                    {
                        rdo.COUNT_TRANPATI_NOT_TREAT_IN++;

                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void TickTranPatiForm(long? tranPatiFormId)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private bool IsRequestIn(long? treatmentTypeId, long? clinicalInTime, long? inRoomId, long executeRoomId)
        {
            if (treatmentTypeId.HasValue && treatmentTypeId > 0)
            {
                return IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU == treatmentTypeId.Value && clinicalInTime != null && inRoomId == executeRoomId;
            }
            else
            {
                return false;
            }
        }

        private bool IsExam(long requestRoomId, List<V_HIS_EXECUTE_ROOM> listRoom)
        {
            return listRoom.Exists(o => o.ROOM_ID == requestRoomId && o.IS_EXAM == 1);
        }

        void CountMediHomeExam(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    //&& treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_END_EXAM++;
                }
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_MEDI_HOME_EXAM++;

                }
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_APPOINTMENT_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountMediHomeExamBH(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                //if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                //    //&& treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                //    && treatment.END_ROOM_ID == executeRoomId)
                //{
                //    rdo.COUNT_END_EXAM++;
                //}
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_MEDI_HOME_EXAM_BH++;
                    if (!string.IsNullOrWhiteSpace(treatment.TDL_HEIN_CARD_NUMBER) && treatment.TDL_HEIN_CARD_NUMBER.StartsWith("CA"))
                    {
                        rdo.COUNT_MEDI_HOME_EXAM_CA++;
                    }

                }
                //if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                //    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN
                //    && treatment.END_ROOM_ID == executeRoomId)
                //{
                //    rdo.COUNT_APPOINTMENT_EXAM++;
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountMediHomeKskCon(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {

                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_MEDI_HOME_KSK_CON++;
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountDeathExam(long executeRoomId, TREATMENT treatment, Mrs00002RDO rdo)
        {
            try
            {
                if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET
                    && treatment.END_ROOM_ID == executeRoomId)
                {
                    rdo.COUNT_DEATH_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountLeftRouteExam(SERVICE_REQ data, List<PATIENT_TYPE_ALTER> listPatientTypeAlter, Mrs00002RDO rdo)
        {
            try
            {
                var patientTypeAlter = listPatientTypeAlter.OrderBy(o => o.LOG_TIME).LastOrDefault(p => p.TREATMENT_ID == data.TREATMENT_ID && p.LOG_TIME <= data.INTRUCTION_TIME) ?? listPatientTypeAlter.OrderBy(o => o.LOG_TIME).FirstOrDefault(p => p.TREATMENT_ID == data.TREATMENT_ID && p.LOG_TIME > data.INTRUCTION_TIME);
                if (patientTypeAlter != null && patientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                {
                    rdo.COUNT_LEFT_ROUTE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountPatientTypeGroup(long patientTypeId, Mrs00002RDO rdo, string heinType)
        {
            try
            {
                if (PatientTypeIdBHs.Contains(patientTypeId) || patientTypeId == PatientTypeIdBH)
                {
                    if (!string.IsNullOrWhiteSpace(heinType))
                    {
                        if (heinType == "CA")
                        {
                            rdo.COUNT_CA_EXAM_PATIENT_TYPE++;
                        }
                        else
                        {
                            rdo.COUNT_OTHER_EXAM_PATIENT_TYPE++;
                        }
                    }
                    rdo.COUNT_GROUP1++;
                }
                else if (PatientTypeIdFees.Contains(patientTypeId) || patientTypeId == PatientTypeIdFee)
                {
                    rdo.COUNT_GROUP2++;
                }
                else
                {
                    rdo.COUNT_GROUP3++;
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountPatientTypeGroupExeReq(long patientTypeId, Mrs00002RDO rdo, long executeRoomId, long requestRoomId)
        {
            try
            {
                if (PatientTypeIdBHs.Contains(patientTypeId) || patientTypeId == PatientTypeIdBH)
                {
                    CountExeReq(executeRoomId, requestRoomId, rdo, "_GROUP1");
                }
                else if (PatientTypeIdFees.Contains(patientTypeId) || patientTypeId == PatientTypeIdFee)
                {
                    CountExeReq(executeRoomId, requestRoomId, rdo, "_GROUP2");
                }
                else
                {
                    CountExeReq(executeRoomId, requestRoomId, rdo, "_GROUP3");
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        //void CountPatientTypeBHYT(long patientTypeId, Mrs00002RDO rdo, string heinCardType)
        //{
        //    try
        //    {
        //        if (patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
        //        {
        //            if (heinCardType == "CA")
        //                rdo.COUNT_CA_EXAM++;
        //            else
        //                rdo.COUNT_OTHER_EXAM++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //    }
        //}

        void CountPatientTypeKsk(long patientTypeId, long patientTypeIdKsk, Mrs00002RDO rdo)
        {
            try
            {
                if (patientTypeId == patientTypeIdKsk)
                {
                    rdo.COUNT_KSK++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountPatientTypeFree(long patientTypeId, long patientTypeIdFree, Mrs00002RDO rdo)
        {
            try
            {
                if (patientTypeId == patientTypeIdFree)
                {
                    rdo.COUNT_FREE++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountPatientClassifyGroup(Mrs00002RDO rdo, long executeRoomId, long? patientClassifyId)
        {
            try
            {

                rdo.COUNT_CLASSIFY = patientClassifyId > 0 ? 1 : 0;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void CountDoneExam(long serviceReqSttId, Mrs00002RDO rdo)
        {
            try
            {
                if (serviceReqSttId == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                {
                    rdo.COUNT_DONE_EXAM++;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            dicSingleTag.Add("CREATE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.INTRUCTION_TIME_FROM ?? 0));

            dicSingleTag.Add("CREATE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.CREATE_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.INTRUCTION_TIME_TO ?? 0));
            //if (dataObject.Count > 0)
            {

                if (dataObject.Count > 0 && dicSingleKey != null && dicSingleKey.Count > 0)
                {
                    foreach (var item in dicSingleKey)
                    {
                        if (!dicSingleTag.ContainsKey(item.Key))
                        {
                            dicSingleTag.Add(item.Key, item.Value);
                        }
                        else
                        {
                            dicSingleTag[item.Key] = item.Value;
                        }
                    }
                }
                for (int i = 0; i < 15; i++)
                {

                    objectTag.AddObjectData(store, "Report" + i, dataObject.Count > 0 && dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                    objectTag.AddObjectData(store, "Parent" + i, dataObject.Count > 0 && dataObject[i].Count > 1 ? dataObject[i][1] : new DataTable());
                    objectTag.AddObjectData(store, "GrandParent" + i, dataObject.Count > 0 && dataObject[i].Count > 2 ? dataObject[i][2] : new DataTable());
                    objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
                    objectTag.AddRelationship(store, "GrandParent" + i, "Parent" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");
                }
                //if (!(this.dicDataFilter.ContainsKey("KEY_GET_TOGETHER_A1") && this.dicDataFilter["KEY_GET_TOGETHER_A1"] != null && this.dicDataFilter["KEY_GET_TOGETHER_A1"].ToString() == "YES"))
                //{
                //    return;
                //}
            }
            Inventec.Common.Logging.LogSystem.Info("listRdoPrint" + listExam.Sum(s => s.COUNT_EXAM));

            objectTag.AddObjectData(store, "Report", listExam.OrderBy(o => o.EXECUTE_ROOM_NAME).ToList());
            objectTag.AddObjectData(store, "ReportA", listExam.OrderBy(o => o.EXECUTE_ROOM_NAME).ToList());

            objectTag.AddObjectData(store, "Department", listExam.GroupBy(o => o.EXECUTE_DEPARTMENT_NAME).Select(o => new Mrs00002RDO() { EXECUTE_DEPARTMENT_NAME = o.First().EXECUTE_DEPARTMENT_NAME, EXECUTE_DEPARTMENT_CODE = o.First().EXECUTE_DEPARTMENT_CODE, COUNT_EXAM = o.Sum(s => s.COUNT_EXAM), COUNT_EXAM_FROM_REA = o.Sum(s => s.COUNT_EXAM_FROM_REA), COUNT_FEMALE_EXAM = o.Sum(s => s.COUNT_FEMALE_EXAM), COUNT_MALE_EXAM = o.Sum(s => s.COUNT_MALE_EXAM), COUNT_GROUP1 = o.Sum(s => s.COUNT_GROUP1), COUNT_GROUP2 = o.Sum(s => s.COUNT_GROUP2) }).ToList());
            objectTag.AddObjectData(store, "CardType", listCardType);

            dicSingleTag.Add("TOTAL_PATIENT", listExam.Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_PATIENT_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_PATIENT_BHYT_100", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.HEIN_RATIO == 1).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_PATIENT_BHYT_95", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.HEIN_RATIO == 0.95).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_PATIENT_BHYT_80", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.HEIN_RATIO == 0.80).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_PATIENT_NOT_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));

            dicSingleTag.Add("TOTAL_CHILD_LESS6", listExam.Where(p => p.AGE < 6).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS6_BHYT", listExam.Where(p => p.AGE < 6 && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS6_NOT_BHYT", listExam.Where(p => p.AGE < 6 && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS15_MORE6", listExam.Where(p => p.AGE >= 6 && p.AGE < 15).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS15_MORE6_BHYT", listExam.Where(p => p.AGE >= 6 && p.AGE < 15 && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS15_MORE6_NOT_BHYT", listExam.Where(p => p.AGE >= 6 && p.AGE < 15 && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_CHILD_LESS15", listExam.Where(p => p.AGE < 15).Sum(p => p.COUNT_EXAM));

            dicSingleTag.Add("TOTAL_MAN_LESS60_MORE15", listExam.Where(p => p.AGE >= 15 && p.AGE < 60).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_MAN_LESS60_MORE15_BHYT", listExam.Where(p => p.AGE >= 15 && p.AGE < 60 && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_MAN_LESS60_MORE15_NOT_BHYT", listExam.Where(p => p.AGE >= 15 && p.AGE < 60 && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));

            dicSingleTag.Add("TOTAL_FEMALE", listExam.Sum(p => p.COUNT_FEMALE_EXAM));
            dicSingleTag.Add("TOTAL_FEMALE_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_FEMALE_EXAM));
            dicSingleTag.Add("TOTAL_FEMALE_NOT_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_FEMALE_EXAM));

            dicSingleTag.Add("TOTAL_MALE", listExam.Sum(p => p.COUNT_MALE_EXAM));
            dicSingleTag.Add("TOTAL_MALE_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_MALE_EXAM));
            dicSingleTag.Add("TOTAL_MALE_NOT_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_MALE_EXAM));

            dicSingleTag.Add("TOTAL_ELDERLY_LESS90_MORE60", listExam.Where(p => p.AGE >= 60 && p.AGE < 90).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_ELDERLY_LESS90_MORE60_BHYT", listExam.Where(p => p.AGE >= 60 && p.AGE < 90 && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_ELDERLY_LESS90_MORE60_NOT_BHYT", listExam.Where(p => p.AGE >= 60 && p.AGE < 90 && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));

            dicSingleTag.Add("TOTAL_ELDERLY_MORE90", listExam.Where(p => p.AGE >= 90).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_ELDERLY_MORE90_BHYT", listExam.Where(p => p.AGE >= 90 && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));
            dicSingleTag.Add("TOTAL_ELDERLY_MORE90_NOT_BHYT", listExam.Where(p => p.AGE >= 90 && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_EXAM));

            dicSingleTag.Add("TOTAL_EMERGENCY", listExam.Sum(p => p.COUNT_EMERGENCY_EXAM));
            dicSingleTag.Add("TOTAL_EXAM_KSK", listExam.Sum(P => P.COUNT_KSK));
            dicSingleTag.Add("TOTAL_CLINICAL_IN", listExam.Sum(p => p.COUNT_CLINICAL_IN));
            dicSingleTag.Add("TOTAL_CLINICAL_IN_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_CLINICAL_IN));
            dicSingleTag.Add("TOTAL_CLINICAL_IN_NOT_BHYT", listExam.Where(p => p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(p => p.COUNT_CLINICAL_IN));

            dicSingleTag.Add("TOTAL_TRANSFER", listExam.Sum(p => p.COUNT_TRANPATI_EXAM));
            dicSingleTag.Add("TOTAL_OUT", listExam.Sum(p => p.COUNT_MEDI_HOME_EXAM));

            string query0 = "select * from his_patient_classify where patient_classify_name <> 'BHYT'";
            List<HIS_PATIENT_CLASSIFY> listClassify0 = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query0);
            int X = 1;
            if (listClassify0 != null)
            {
                foreach (var item in listClassify0.OrderBy(P => P.ID).ToList())
                {
                    dicSingleTag.Add("PATIENT_CLASSIFY_ID___" + X, item.ID);
                    dicSingleTag.Add("PATIENT_CLASSIFY_CODE___" + X, item.PATIENT_CLASSIFY_CODE);
                    dicSingleTag.Add("PATIENT_CLASSIFY_NAME___" + X, item.PATIENT_CLASSIFY_NAME);
                    X++;
                }
            }

            string query = "select * from his_patient_classify";
            List<HIS_PATIENT_CLASSIFY> listClassify = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query);
            int a = 1;
            if (listClassify != null)
            {
                foreach (var item in listClassify.OrderBy(P => P.ID).ToList())
                {
                    dicSingleTag.Add("PATIENT_CLASSIFY_ID__" + a, item.ID);
                    dicSingleTag.Add("PATIENT_CLASSIFY_CODE__" + a, item.PATIENT_CLASSIFY_CODE);
                    dicSingleTag.Add("PATIENT_CLASSIFY_NAME__" + a, item.PATIENT_CLASSIFY_NAME);
                    a++;
                }
            }

        }
    }
}
