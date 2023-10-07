using MRS.Processor.Mrs00555;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;
using Inventec.Common.Logging;
using HIS.Common.Treatment;

namespace MRS.Proccessor.Mrs00555
{
    public class Mrs00555RDO
    {
        public string TREATMENT_CODE { get; set; }
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public long TREATMENT_ID { get; set; }
        public long GENDER_ID { get; set; }
        public long BRANCH_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal COUNT_NGOAITRU_OLD { get; set; }
        public decimal COUNT_NGOAITRU_NEW { get; set; }
        public decimal COUNT_NGOAITRU { get; set; }
        public decimal COUNT_NGOAITRU_FEMALE_OLD { get; set; }
        public decimal COUNT_NGOAITRU_FEMALE_NEW { get; set; }
        public decimal COUNT_NGOAITRU_FEMALE { get; set; }
        public decimal COUNT_NGOAITRU_BHYT { get; set; }
        public decimal COUNT_NGOAITRU_VP { get; set; }
        public decimal COUNT_OLD { get; set; }
        public decimal COUNT_OLD_OUT { get; set; }
        public decimal COUNT_OLD_FEMALE { get; set; }
        public decimal COUNT_OLD_BHYT { get; set; }
        public decimal COUNT_OLD_LESS6 { get; set; }
        public decimal COUNT_OLD_MORE60 { get; set; }

        public int NUM_BED { get; set; }
        public decimal COUNT_NEW_CLINIAL { get; set; }
        public decimal COUNT_NEW { get; set; }
        public decimal COUNT_NEW_FROM_KKB { get; set; }
        public decimal COUNT_EXAM { get; set; }
        public decimal COUNT_CLINICAL_IN { get; set; }
        public decimal COUNT_NEW_TE { get; set; }
        public decimal COUNT_NEW_LESS6 { get; set; }
        public decimal COUNT_NEW_MORE6_LESS18 { get; set; }
        public decimal COUNT_NEW_MORE18_LESS60 { get; set; }
        public decimal COUNT_NEW_MORE60 { get; set; }
        public decimal COUNT_NEW_EMERGENCY { get; set; }
        public decimal COUNT_NEW_BHYT { get; set; }
        public decimal COUNT_NEW_EMERGENCY_BHYT { get; set; }
        public decimal COUNT_NEW_FEE { get; set; }

        public decimal COUNT_ALL_BHYT { get; set; }

        public decimal COUNT_ALL_VP { get; set; }

        public decimal COUNT_ALL_XHH { get; set; }

        public decimal DAY { get; set; }

        public decimal DAY1 { get; set; }

        public decimal COUNT_ALL_DAY { get; set; }

        public decimal COUNT_EXP { get; set; }
        public decimal COUNT_OUT_KHOI { get; set; }
        public decimal COUNT_OUT_DO { get; set; }
        public decimal COUNT_OUT_KTD { get; set; }
        public decimal COUNT_OUT_NANG { get; set; }
        public decimal COUNT_OUT_NANG_CHET { get; set; }
        public decimal COUNT_OUT_CV { get; set; }

        public decimal COUNT_BED_DT { get; set; }
        public decimal COUNT_BED_REQ { get; set; }
        public decimal COUNT_IN { get; set; }
        public decimal COUNT_NOITRU { get; set; }

        public decimal COUNT_OUT_XINRAVIEN { get; set; }

        public decimal COUNT_OUT_TRON { get; set; }

        public decimal COUNT_OUT_RAVIEN { get; set; }

        public decimal COUNT_OUT_KHAC { get; set; }

        public decimal COUNT_OUT_HEN { get; set; }

        public decimal COUNT_OUT_CTCV { get; set; }

        public decimal COUNT_OUT_CV_LEN { get; set; }

        public decimal COUNT_DIE { get; set; }
        public decimal COUNT_DIE_TE { get; set; }
        public decimal COUNT_DIE_LESS5 { get; set; }
        public decimal COUNT_DIE_LESS1 { get; set; }
        public decimal COUNT_DIE_24 { get; set; }

        public decimal COUNT_END { get; set; }

        public decimal COUNT_MOVE { get; set; }

        public decimal POINT_DAY { get; set; }

        public long REALITY_PATIENT_BED { get; set; }

        public long THEORY_PATIENT_BED { get; set; }

        public short IS_TREAT_IN { get; set; }
        public decimal COUNT_NN { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public long? TDL_PATIENT_CLASSIFY_ID { set; get; }
        //tinh theo ngay truoc
        
        private double MyDateDiff(long intime, long outtime)
        {
            
            double result = 0;
            try
            {
                if (outtime < intime) return 0;
                DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intime);
                DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(outtime);
                TimeSpan sp = OutTime - InTime;
                var days = sp.TotalDays;
                if ((days - (long)days) < 0.25)
                {
                    result = (long)days;
                }
                else if ((days - (long)days) < 0.75)
                {
                    result = (long)days + 0.5;
                }
                else
                {
                    result = (long)days + 1;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }
        public int point_day(long treatmentid, long startday, long finishday, ref List<long> listTreatmentId)
        {
            int result = 0;
            try
            {
                if (listTreatmentId.Contains(treatmentid))
                    return 0;

                listTreatmentId.Add(treatmentid);
                result = Inventec.Common.DateTime.Calculation.DifferenceDate(startday, finishday) + 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        public void point_day_with_out(TREATMENT trea, long diffDate, bool IsLastDepartment, Mrs00555Filter mrs00555Filter)
        {
            try
            {
                //tinh so ngay phu thuoc vao viec da ra vien chua, va ra vien loai nao
                var starttime = trea.IN_TIME > mrs00555Filter.TIME_FROM ? trea.IN_TIME : mrs00555Filter.TIME_FROM;
                var finishtime = trea.OUT_TIME.HasValue && trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME.Value < mrs00555Filter.TIME_TO ? trea.OUT_TIME.Value : mrs00555Filter.TIME_TO;
                if (starttime > finishtime)
                {
                    Inventec.Common.Logging.LogSystem.Info("Treatment_code" + trea.TREATMENT_CODE);

                }
                else
                {
                    this.POINT_DAY_A = diffDate;
                    this.POINT_DAY_B = diffDate;
                    if (IsLastDepartment)
                    {
                        if (trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME < mrs00555Filter.TIME_TO)
                        {
                            this.POINT_DAY_A += 1;
                            if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                            {
                                this.POINT_DAY_B += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN && trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                            {
                                this.POINT_DAY_B += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN && trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                            {
                                this.POINT_DAY_B += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && (trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_KHONG_LIEN_KE || trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_LIEN_KE))
                            {
                                this.POINT_DAY_B += 1;
                            }
                            //neu ra vien tai khoa nhung thoi gian dieu tri o khoa=0 thi gan lai =1
                            else if (this.POINT_DAY_B == 0)
                            {
                                this.POINT_DAY_B = 1;
                            }
                        }
                    }
                    int diffHour = Inventec.Common.DateTime.Calculation.DifferenceTime(starttime, finishtime, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                    if (diffHour < 4)
                    {
                        this.POINT_DAY_A = 0;
                        this.POINT_DAY_B = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return;
            }
            return;
        }

        public Mrs00555RDO(DEPARTMENT_TRAN inDepartmentTran, DEPARTMENT_TRAN previousDepartmentTran, Dictionary<long, int> dicCountBed, DEPARTMENT_TRAN nextDepartmentTran, TREATMENT trea, PATIENT_TYPE_ALTER lastPta, Mrs00555Filter mrs00555Filter, bool previousIsTreatIn, bool previousIsTreatOut, long dateTo, List<long> treatmentids, Dictionary<long,HIS_TREATMENT> dicTreatmentAll, List<TREATMENT_BED_ROOM> treatmentBedRooms)
        {

            if (mrs00555Filter.IS_DETAIL_TREATMENT == true)
            {
                if (dicTreatmentAll.ContainsKey(inDepartmentTran.TREATMENT_ID))
                {
                    this.HIS_TREATMENT = dicTreatmentAll[inDepartmentTran.TREATMENT_ID] ?? new HIS_TREATMENT();
                }
                else
                {
                    this.HIS_TREATMENT = new HIS_TREATMENT();
                }
            }
            else
            {
                this.HIS_TREATMENT = new HIS_TREATMENT();
            }
           //nếu thời gian ra khoa null thì lấy thời gian ra viện hoặc lấy số cực lớn nếu chưa ra viện
            if (nextDepartmentTran.DEPARTMENT_IN_TIME == null) nextDepartmentTran.DEPARTMENT_IN_TIME = trea.IS_PAUSE == 1 ? trea.OUT_TIME : 99999999999999;
            //nếu thời gian ra khoa bé hơn thời gian nhập viện thì không tính
            if ((long)nextDepartmentTran.DEPARTMENT_IN_TIME / 100 <= (long)(trea.CLINICAL_IN_TIME ?? 0) / 100)
            {
                return;
            }
            //nếu tách theo buồng thì sẽ lấy buồng bệnh nhân nằm cuối cùng trước khi ra khoa
            if (mrs00555Filter.IS_DETAIL_TREATMENT_BEDROOM == true)
            {
                var treatmentBedRoom = treatmentBedRooms.OrderBy(p => p.ADD_TIME).LastOrDefault(o => o.TREATMENT_ID == inDepartmentTran.TREATMENT_ID && o.DEPARTMENT_ID == inDepartmentTran.DEPARTMENT_ID && o.ADD_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME);
                if (treatmentBedRoom != null)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatmentBedRoom.ROOM_ID);
                    if (room != null)
                    {
                        this.ROOM_ID = room.ID;
                        this.ROOM_CODE = room.ROOM_CODE;
                        this.ROOM_NAME = room.ROOM_NAME;
                    }
                }
            }
            this.TDL_PATIENT_CLASSIFY_ID = trea.TDL_PATIENT_CLASSIFY_ID;
            int age = Age(trea.IN_TIME, trea.TDL_PATIENT_DOB);
            this.TREATMENT_CODE = trea.TREATMENT_CODE;
            this.TREATMENT_ID = trea.ID;
            this.GENDER_ID = trea.TDL_PATIENT_GENDER_ID;
            this.BRANCH_ID = 1;
            this.DEPARTMENT_ID = inDepartmentTran.DEPARTMENT_ID;
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID);
            if (department != null)
            {
                //thứ tự sắp xếp các khoa
                this.ORDER = department.NUM_ORDER;
                this.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                this.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                this.THEORY_PATIENT_BED = department.THEORY_PATIENT_COUNT ?? 0;
                this.REALITY_PATIENT_BED = department.REALITY_PATIENT_COUNT ?? 0;
                this.NUM_BED = dicCountBed.ContainsKey(inDepartmentTran.DEPARTMENT_ID) ? dicCountBed[inDepartmentTran.DEPARTMENT_ID] : 0;

            }

            var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == trea.TDL_PATIENT_TYPE_ID);
            if (patientType != null)
            {
                //thứ tự sắp xếp các khoa
                this.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
            }
            if (trea.TDL_HEIN_CARD_NUMBER != null)
            {
                //thứ tự sắp xếp các khoa
                this.HEAD_CARD = (trea.TDL_HEIN_CARD_NUMBER+"  ").Substring(0,2);
            }
            TDL_PATIENT_NATIONAL_NAME = trea.TDL_PATIENT_NATIONAL_NAME;
            if (trea.TDL_PATIENT_NATIONAL_NAME != null)
            {
                //thứ tự sắp xếp các khoa
                if (trea.TDL_PATIENT_NATIONAL_NAME!= "Việt Nam")
                {
                    this.COUNT_NN = 1;
                    this.IS_NN = true; 
                }
                //this.IS_NN = !trea.TDL_PATIENT_NATIONAL_NAME.ToLower().Contains( "việt nam");
            }
            List<string> KCCDepartmentCodes = new List<string>();
            if (mrs00555Filter.DEPARTMENT_CODE__OUTPATIENTs != null)
            {
                KCCDepartmentCodes = mrs00555Filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
            }
            if (KCCDepartmentCodes.Contains(this.DEPARTMENT_CODE??""))
            {
                return;
            }

            var startday = trea.IN_DATE > mrs00555Filter.TIME_FROM ? trea.IN_DATE : mrs00555Filter.TIME_FROM;

            var finishday = trea.OUT_DATE.HasValue && trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_DATE.Value < dateTo ? trea.OUT_DATE.Value : dateTo;
            if (startday > finishday)
            {
                Inventec.Common.Logging.LogSystem.Info("Treatment_code" + trea.TREATMENT_CODE);

            }
            else
            {
                //key tính ngày điều trị theo ngày vào viện và ra viện, chỉ tính cho 1 trong số các khoa điều trị của bệnh nhân
                this.POINT_DAY = point_day(trea.ID, startday, finishday, ref treatmentids);
                if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.POINT_DAY_BHYT = this.POINT_DAY;
                }
            }

            //nếu vào khoa một thời gian rồi mới nhập viện hoặc nhập viện ở khoa trước đó nhưng khoa trước đó không điều trị thì sửa lại thời gian điều trị ở khoa bắt đầu từ lúc nhập viện
            if ((long)inDepartmentTran.DEPARTMENT_IN_TIME / 100 < (long)trea.CLINICAL_IN_TIME / 100 || !previousIsTreatIn && !previousIsTreatOut)
            {
                inDepartmentTran.DEPARTMENT_IN_TIME = trea.CLINICAL_IN_TIME;
            }
            //tinh so ngay theo cong thuc #55405
            this.CountDayOut(inDepartmentTran.DEPARTMENT_IN_TIME ?? 0, nextDepartmentTran.DEPARTMENT_IN_TIME == 99999999999999 ? mrs00555Filter.TIME_TO : (nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0));
            //BN dau ki se lay:
            // vao khoa truoc Time_from

            if (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_FROM && mrs00555Filter.TIME_FROM <= nextDepartmentTran.DEPARTMENT_IN_TIME)
            {
                SetOldTreatment(trea,lastPta,age);
            }
            //Nguoi benh vao dieu tri se lay:
            //vao khoa tu time_from den time_to
            if (inDepartmentTran.DEPARTMENT_IN_TIME >= mrs00555Filter.TIME_FROM && inDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_TO)
            {

                SetNewTreatment(inDepartmentTran,previousDepartmentTran,trea,lastPta,mrs00555Filter,previousIsTreatIn,previousIsTreatOut,age);

            }
            if (trea.IN_TIME >= mrs00555Filter.TIME_FROM && trea.IN_TIME <= mrs00555Filter.TIME_TO)
            {
                this.COUNT_IN = 1;
                if (lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.COUNT_NOITRU = 1;
                }
            }

            if (nextDepartmentTran.ID > 0 && nextDepartmentTran.DEPARTMENT_IN_TIME >= mrs00555Filter.TIME_FROM && nextDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_TO)
                this.COUNT_MOVE = 1;//chuyển khoa đi

            if (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_TO && mrs00555Filter.TIME_FROM <= nextDepartmentTran.DEPARTMENT_IN_TIME)
            {
                SetDayTreatment( inDepartmentTran,  previousDepartmentTran,  nextDepartmentTran,  trea,  lastPta,  mrs00555Filter,  previousIsTreatIn, previousIsTreatOut);

            }

            //cac loai ra vien
            if (trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME < mrs00555Filter.TIME_TO && trea.OUT_TIME >= mrs00555Filter.TIME_FROM)
            {
                SetExpTreatment(nextDepartmentTran,trea,lastPta);

            }
            //con lai cuoi ky
            if (nextDepartmentTran.DEPARTMENT_IN_TIME >= mrs00555Filter.TIME_TO && inDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_TO)
            {
                SetEndTreatment(trea,lastPta);
            }
            this.CLINICAL_IN_TIME = trea.CLINICAL_IN_TIME;
            this.IN_DATE = trea.IN_DATE;
            this.DEPARTMENT_IN_TIME = inDepartmentTran.DEPARTMENT_IN_TIME;
            this.NEXT_DEPARTMENT_IN_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME;
            this.OUT_TIME = trea.OUT_TIME;
            this.TDL_PATIENT_TYPE_ID = trea.TDL_PATIENT_TYPE_ID;
            this.TDL_TREATMENT_TYPE_ID = trea.TDL_TREATMENT_TYPE_ID;
            this.TDL_PATIENT_DOB = trea.TDL_PATIENT_DOB;
            this.TREATMENT_END_TYPE_ID = trea.TREATMENT_END_TYPE_ID;
            this.TREATMENT_RESULT_ID = trea.TREATMENT_RESULT_ID;
            this.NEXT_ID = nextDepartmentTran.ID;
            PATIENT_CAREER_NAME = trea.TDL_PATIENT_CAREER_NAME;
            if (trea.IS_PAUSE==null)
            {
                this.COUNT_END_NEW = 1;
            }
        }

        private void SetDayTreatment(DEPARTMENT_TRAN inDepartmentTran, DEPARTMENT_TRAN previousDepartmentTran, DEPARTMENT_TRAN nextDepartmentTran, TREATMENT trea, PATIENT_TYPE_ALTER lastPta, Mrs00555Filter mrs00555Filter, bool previousIsTreatIn, bool previousIsTreatOut)
        {
            //So ngay dieu tri trong khoa se tinh
            //thoi gian ra khoa - thoi gian vao khoa:
            //- Neu < 1/4 ngay thi lay = 0
            // - Neu <3/4 va >=1/4 ngay thi lay = 0.5
            // - Neu < 1 va >=3/4 ngay thi lay = 1 ngay
            //if (nextDepartmentTran.DEPARTMENT_IN_TIME < 99999999999999)
            //{
            long startTime = (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_FROM) ? mrs00555Filter.TIME_FROM : (inDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
            long endTime = (nextDepartmentTran.DEPARTMENT_IN_TIME > mrs00555Filter.TIME_TO) ? mrs00555Filter.TIME_TO : (nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
            //this.DAY = (decimal)MyDateDiff(inDepartmentTran.DEPARTMENT_IN_TIME ?? 0, nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
            //this.DAY1 = Calculation.DayOfTreatment(startTime, endTime, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0; //;
            /*
neu thoi gian ra khoa - thoi gian vao khoa < 4 tieng thi lay 1/2 ngay
ney tu 4 tieng den 8 tieng lay 1 ngay
tu 8 tieng tro len lay (ngay ra - ngay vao)+1
neu thoi gian vao khoa tiep theo hoac thoi gian vao khoa truoc do trong khoang tu den va so ngay dieu tri>=1 thi so ngay dieu tri = so ngay dieu tri -1/2
*/
            DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(startTime);
            DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(endTime);
            TimeSpan sp = OutTime - InTime;
            var days = sp.TotalDays;
            if (((previousDepartmentTran != null && previousDepartmentTran.DEPARTMENT_IN_TIME > 0 && (long)inDepartmentTran.DEPARTMENT_IN_TIME / 100 > (long)trea.CLINICAL_IN_TIME / 100) || nextDepartmentTran.DEPARTMENT_IN_TIME < mrs00555Filter.TIME_TO))// && days >= 0.16667)
            {
                this.DAY -= 0.5M;
                this.DAY1 -= 0.5M;
            }
            if (days < 0.16667)
            {
                this.DAY += 0.5M;
                this.DAY1 += 0.5M;
            }
            if (days >= 0.16667 && days < 0.334)
            {
                this.DAY += 1M;
                this.DAY1 += 1M;
            }
            if (days >= 0.334)
            {
                this.DAY += (long)days + 1;
                this.DAY1 += (long)days + 1;
            }

            DateTime InDate = InTime.Date;
            DateTime OutDate = OutTime.Date;

            TimeSpan diff = OutDate - InDate;
            var ds = diff.TotalDays;
            //phuong an 3: tinh theo thoi gian vao ra khoa phu thuoc loai hinh ra vien
            this.point_day_with_out(trea, (long)ds, (nextDepartmentTran.ID == 0), mrs00555Filter);


        }

        private void SetEndTreatment(TREATMENT trea, PATIENT_TYPE_ALTER lastPta)
        {
            this.COUNT_END = 1;
            if (lastPta.IS_POLICE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                this.COUNT_END_BHCA = 1;
            }
            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.COUNT_ALL_BHYT = 1;
            }
            else if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
            {
                this.COUNT_ALL_VP = 1;
            }
            else
            {
                this.COUNT_ALL_XHH = 1;
            }

            if (trea.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                this.COUNT_END_EMERGENCY = 1;
            }
            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.COUNT_END_BHYT = 1;

                if (lastPta.RIGHT_ROUTE_CODE == "DT")
                {
                    this.COUNT_END_BHDT = 1;
                }
                else
                {
                    this.COUNT_END_BHTT = 1;
                }
            }
            else
            {
                this.COUNT_END_VP = 1;
            }

            if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                this.COUNT_END_FEMALE = 1;
            }

            if (trea.TREATMENT_END_TYPE_ID == null)
            {
                this.COUNT_END_NEW1 = 1;
                if (lastPta.IS_POLICE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    this.COUNT_END_NEW_BHCA = 1;
                }
                if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.COUNT_ALL_NEW_BHYT = 1;
                }
                else if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    this.COUNT_ALL_NEW_VP = 1;
                }
                else
                {
                    this.COUNT_ALL_NEW_XHH = 1;
                }

                if (trea.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    this.COUNT_END_NEW_EMERGENCY = 1;
                }
                if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.COUNT_END_NEW_BHYT = 1;

                    if (lastPta.RIGHT_ROUTE_CODE == "DT")
                    {
                        this.COUNT_END_NEW_BHDT = 1;
                    }
                    else
                    {
                        this.COUNT_END_NEW_BHTT = 1;
                    }
                }
                else
                {
                    this.COUNT_END_NEW_VP = 1;
                }

                if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.COUNT_END_NEW_FEMALE = 1;
                }
            }
        }

        private void SetExpTreatment(DEPARTMENT_TRAN nextDepartmentTran, TREATMENT trea, PATIENT_TYPE_ALTER lastPta)
        {
            if (nextDepartmentTran.ID == 0)
            {
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                {
                    this.COUNT_OUT_CV = 1;//chuyển viện
                    if (trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_KHONG_LIEN_KE || trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_LIEN_KE)
                    {
                        this.COUNT_OUT_CV_LEN = 1;
                    }

                    if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_OUT_CV_BHYT = 1;
                    }

                    if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_OUT_CV_FEMALE = 1;
                    }
                }
                this.TREATMENT_DAY_COUNT = trea.TREATMENT_DAY_COUNT ?? 0;
                this.COUNT_TOTAL_DATE_TREATMENT = HIS.Treatment.DateTime.Calculation.DayOfTreatment(trea.IN_TIME, trea.OUT_TIME.Value);
                this.COUNT_EXP = 1;
                //if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN) {
                //    this.COUNT_EXP = 0;
                //}
                //else
                //{
                //    this.COUNT_EXP = 1;//ra viện
                //}
                

                if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.TREATMENT_DAY_COUNT_BH = trea.TREATMENT_DAY_COUNT ?? 0;
                    this.COUNT_EXP_BHYT = 1;
                    if (lastPta.IS_POLICE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.COUNT_EXP_BHCA = 1;
                    }
                    if (lastPta.RIGHT_ROUTE_CODE == "DT")
                    {
                        this.COUNT_EXP_BHDT = 1;
                    }
                    else
                    {
                        this.COUNT_EXP_BHTT = 1;
                    }
                }
                else
                {
                    this.COUNT_EXP_VP = 1;
                }

                if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.COUNT_EXP_FEMALE = 1;
                }
                switch (trea.TREATMENT_RESULT_ID)
                {
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI:
                        this.COUNT_OUT_KHOI = 1;
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO:
                        this.COUNT_OUT_DO = 1;
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD:
                        this.COUNT_OUT_KTD = 1;
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG:
                        this.COUNT_OUT_NANG = 1;
                        if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                        {
                            this.COUNT_OUT_NANG_CV = 1;
                        }
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET:
                        {
                            this.COUNT_OUT_NANG_CHET = 1;
                            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                this.COUNT_OUT_NANG_CHET_BHYT = 1;
                            }

                            if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                this.COUNT_OUT_NANG_CHET_FEMALE = 1;
                            }
                        }
                        break;
                }
                
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV)
                {
                    this.COUNT_OUT_CTCV = 1;
                }
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN)
                    this.COUNT_OUT_HEN = 1;
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__KHAC)
                    this.COUNT_OUT_KHAC = 1;
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN&& trea.TREATMENT_RESULT_ID!= IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                    this.COUNT_OUT_RAVIEN = 1;
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                {
                    this.COUNT_OUT_TRON = 1;
                    if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_OUT_TRON_BHYT = 1;
                    }

                    if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_OUT_TRON_FEMALE = 1;
                    }
                }
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN && trea.TREATMENT_RESULT_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                    this.COUNT_OUT_XINRAVIEN = 1;
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET||trea.TREATMENT_RESULT_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                {
                    this.COUNT_DIE = 1;
                    if (Inventec.Common.DateTime.Calculation.Age(trea.TDL_PATIENT_DOB) < 15)
                        this.COUNT_DIE_TE = 1;
                    if (Inventec.Common.DateTime.Calculation.Age(trea.TDL_PATIENT_DOB) < 5)
                        this.COUNT_DIE_LESS5 = 1;
                    if (Inventec.Common.DateTime.Calculation.Age(trea.TDL_PATIENT_DOB) < 1)
                        this.COUNT_DIE_LESS1 = 1;

                    if (trea.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                        this.COUNT_DIE_24 = 1;
                }
                //Count Death Info
                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET || trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                {
                    this.COUNT_DEATH_INFO = 1;
                    if (Inventec.Common.DateTime.Calculation.Age(trea.TDL_PATIENT_DOB) < 15)
                        this.COUNT_DEATH_INFO_TE = 1;
                    if (trea.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                        this.COUNT_DEATH_INFO_24 = 1;
                }
                if ((trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN || trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN) && trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                {
                    this.COUNT_HEAVY_OUT = 1;
                }
            }
        }

        private void SetNewTreatment(DEPARTMENT_TRAN inDepartmentTran, DEPARTMENT_TRAN previousDepartmentTran, TREATMENT trea, PATIENT_TYPE_ALTER lastPta, Mrs00555Filter mrs00555Filter, bool previousIsTreatIn, bool previousIsTreatOut, int age)
        {
            if ((long)inDepartmentTran.DEPARTMENT_IN_TIME / 100 > (long)trea.CLINICAL_IN_TIME / 100 &&
            (previousIsTreatIn || previousIsTreatOut))
            {
                this.COUNT_CHDP_IMP = 1;//chuyển khoa đến
                if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.COUNT_CHDP_IMP_BHYT = 1;
                }
            }
            else
            {
                if (lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    this.COUNT_NGOAITRU_NEW = 1;
                    if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_NGOAITRU_FEMALE_NEW = 1;
                    }
                    if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_NGOAITRU_BHYT = 1;
                    }
                    else
                    {
                        this.COUNT_NGOAITRU_VP = 1;
                    }
                }
                //else if (lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM || lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                {
                    this.COUNT_NEW = 1;//mới
                    if (lastPta.IS_POLICE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.COUNT_NEW_BHCA = 1;
                    }
                    if (age >= 60)
                    {
                        this.COUNT_NEW_MORE60 = 1;
                    }
                    if (age >= 18 && age < 60)
                    {
                        this.COUNT_NEW_MORE18_LESS60 = 1;
                    }
                    if (age >= 6 && age < 18)
                    {
                        this.COUNT_NEW_MORE6_LESS18 = 1;
                    }
                    if (age < 6)
                    {
                        this.COUNT_NEW_LESS6 = 1;
                    }
                    if (mrs00555Filter.DEPARTMENT_CODE__KKB != null && previousDepartmentTran != null)
                    {
                        List<string> KKBDepartmentCodes = mrs00555Filter.DEPARTMENT_CODE__KKB.Split(',').ToList();
                        var preDepartmentCode = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == previousDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        if (preDepartmentCode != null && KKBDepartmentCodes.Contains(preDepartmentCode))
                        {
                            this.COUNT_NEW_FROM_KKB = 1;
                        }
                    }
                    if (Inventec.Common.DateTime.Calculation.Age(trea.TDL_PATIENT_DOB) < 15)
                        this.COUNT_NEW_TE = 1;

                    var inRoomCode = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == trea.IN_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                    if (mrs00555Filter.ROOM_CODE__CC != null)
                    {
                        if (inRoomCode == mrs00555Filter.ROOM_CODE__CC)
                        {
                            this.COUNT_NEW_FROM_CC = 1;
                            this.COUNT_NEW_FROM_C = 1;
                            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                this.COUNT_NEW_FROM_CC_BHYT = 1;
                                this.COUNT_NEW_FROM_C_BHYT = 1;
                            }
                        }
                    }
                    if (mrs00555Filter.ROOM_CODE__SAN != null)
                    {
                        if (inRoomCode == mrs00555Filter.ROOM_CODE__SAN)
                        {
                            this.COUNT_NEW_FROM_CC = 1;
                            this.COUNT_NEW_FROM_S = 1;
                            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                this.COUNT_NEW_FROM_CC_BHYT = 1;
                                this.COUNT_NEW_FROM_S_BHYT = 1;
                            }
                        }
                    }

                    if (trea.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.COUNT_NEW_EMERGENCY = 1;
                        if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            this.COUNT_NEW_EMERGENCY_BHYT = 1;
                        }
                    }


                    if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        this.COUNT_NEW_BHYT = 1;
                        if (lastPta.RIGHT_ROUTE_CODE == "DT")
                        {
                            this.COUNT_NEW_BHDT = 1;
                        }
                        else
                        {
                            this.COUNT_NEW_BHTT = 1;
                        }
                    }
                    else
                    {
                        this.COUNT_NEW_VP = 1;
                    }

                    if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.COUNT_NEW_FEMALE = 1;
                    }



                }
            }
        }

        private void SetOldTreatment(TREATMENT trea, PATIENT_TYPE_ALTER lastPta,int age)
        {
            if (lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || lastPta.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                this.COUNT_NGOAITRU_OLD = 1;
                if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.COUNT_NGOAITRU_FEMALE_OLD = 1;
                }
            }
            this.COUNT_OLD = 1;//cũ
            if (age >= 60)
            {
                this.COUNT_OLD_MORE60 = 1;
            }
            if (age < 6)
            {
                this.COUNT_OLD_LESS6 = 1;
            }

            if (lastPta.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                this.COUNT_OLD_BHYT = 1;
                if (lastPta.RIGHT_ROUTE_CODE == "DT")
                {
                    this.COUNT_OLD_BHDT = 1;
                }
                else
                {
                    this.COUNT_OLD_BHTT = 1;
                }
            }
            else
            {
                this.COUNT_OLD_VP = 1;
            }

            if (trea.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                this.COUNT_OLD_FEMALE = 1;
            }
        }

        private void CountDayOut(long beforeTime, long afterTime)
        {
            //Nhờ code sửa lại số liệu 2 cột trên như sau:
            //1.Cột "Số ngày ĐT (BYT)" = TS(Ra1 - vào + 1) + Chuyển khoa(Ra1 - vào) + Còn lại(Ra2 - vào).

            //2.Cột "Số ngày ĐT (TT39)" = TS(Ra1 - vào) + Chuyển khoa(Ra1 - vào) + Còn lại(Ra2 - vào).

            //Trong đó:
            //+Ra1: Thời gian ra khoa của bệnh nhân
            //+ Ra2: Thời gian "Đến ngày" khi lọc báo cáo.
            //+Vào: Thời gian vào khoa
            afterTime = afterTime - afterTime % 1000000;
            beforeTime = beforeTime - beforeTime % 1000000;
            this.POINT_DAY_OUT = Inventec.Common.DateTime.Calculation.DifferenceDate(beforeTime, afterTime);
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }


        public Mrs00555RDO()
        {
        }
        public decimal COUNT_END_NEW { get; set; }
        public decimal COUNT_CHDP_IMP { get; set; }

        public string PATIENT_CAREER_NAME { set; get; }
        public long PATIENT_CAREER_ID { set; get; }

        public decimal TREATMENT_DAY_COUNT { get; set; }

        public decimal COUNT_TOTAL_DATE_TREATMENT { get; set; }

        public decimal COUNT_NEW_FROM_CC { get; set; }

        public decimal COUNT_NEW_FROM_CC_BHYT { get; set; }

        public decimal COUNT_NEW_FROM_C { get; set; }

        public decimal COUNT_NEW_FROM_S { get; set; }

        public decimal COUNT_NEW_FROM_S_BHYT { get; set; }

        public decimal COUNT_NEW_FROM_C_BHYT { get; set; }

        public decimal COUNT_DEATH_INFO { get; set; }

        public decimal COUNT_DEATH_INFO_TE { get; set; }

        public decimal COUNT_DEATH_INFO_24 { get; set; }

        public decimal COUNT_NEW_FEMALE { get; set; }

        public decimal COUNT_EXP_BHYT { get; set; }

        public decimal COUNT_EXP_FEMALE { get; set; }

        public decimal COUNT_OUT_CV_BHYT { get; set; }

        public decimal COUNT_OUT_CV_FEMALE { get; set; }

        public decimal COUNT_OUT_TRON_FEMALE { get; set; }

        public decimal COUNT_OUT_TRON_BHYT { get; set; }

        public decimal COUNT_OUT_NANG_CHET_BHYT { get; set; }
        public decimal COUNT_OUT_NANG_CHET_FEMALE { get; set; }

        public decimal COUNT_END_FEMALE { get; set; }

        public decimal COUNT_END_BHYT { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public long IN_DATE { get; set; }

        public long? NEXT_DEPARTMENT_IN_TIME { get; set; }

        public long? DEPARTMENT_IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public long TDL_PATIENT_DOB { get; set; }

        public long? TREATMENT_RESULT_ID { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public long NEXT_ID { get; set; }

        public long? ORDER { get; set; }

        public decimal COUNT_OLD_VP { get; set; }

        public decimal COUNT_OLD_BHDT { get; set; }

        public decimal COUNT_OLD_BHTT { get; set; }

        public decimal COUNT_NEW_VP { get; set; }

        public decimal COUNT_NEW_BHTT { get; set; }

        public decimal COUNT_NEW_BHDT { get; set; }

        public decimal COUNT_EXP_VP { get; set; }

        public decimal COUNT_EXP_BHTT { get; set; }

        public decimal COUNT_EXP_BHDT { get; set; }

        public decimal COUNT_END_VP { get; set; }

        public decimal COUNT_END_BHTT { get; set; }

        public decimal COUNT_END_BHDT { get; set; }

        public decimal POINT_DAY_A { get; set; }

        public decimal POINT_DAY_B { get; set; }

        public decimal COUNT_HEAVY_OUT { get; set; }

        public decimal POINT_DAY_BHYT { get; set; }

        public decimal COUNT_CHDP_IMP_BHYT { get; set; }

        public decimal TREATMENT_DAY_COUNT_BH { get; set; }
        public decimal POINT_DAY_OUT { get; set; }
        public long ROOM_ID { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }

        public decimal COUNT_EXP_BHCA { get; set; }

        public decimal COUNT_NEW_BHCA { get; set; }

        public decimal COUNT_END_BHCA { get; set; }

        public decimal COUNT_OUT_NANG_CV { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }

        public string HEAD_CARD { get; set; }

        public bool IS_NN { get; set; }

        public decimal COUNT_END_EMERGENCY { get; set; }

        public decimal COUNT_END_NEW1 { get; set; }

        public decimal COUNT_END_NEW_BHCA { get; set; }

        public decimal COUNT_ALL_NEW_BHYT { get; set; }

        public decimal COUNT_ALL_NEW_VP { get; set; }

        public decimal COUNT_ALL_NEW_XHH { get; set; }

        public decimal COUNT_END_NEW_EMERGENCY { get; set; }

        public decimal COUNT_END_NEW_BHYT { get; set; }

        public decimal COUNT_END_NEW_BHDT { get; set; }

        public decimal COUNT_END_NEW_BHTT { get; set; }

        public decimal COUNT_END_NEW_VP { get; set; }

        public decimal COUNT_END_NEW_FEMALE { get; set; }
    }

    public class PATIENT_TYPE_ALTER
    {
        public long TREATMENT_ID { get; set; }
        public long LOG_TIME { get; set; }
        public long DEPARTMENT_TRAN_ID { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public short? IS_POLICE { get; set; }

        public string RIGHT_ROUTE_CODE { get; set; }
    }

    public class DEPARTMENT_TRAN
    {
        public long TREATMENT_ID { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long ID { get; set; }
        public long? PREVIOUS_ID { get; set; }
    }

    public class TREATMENT
    {
        public long ID { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public long? TRAN_PATI_FORM_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CAREER_NAME{set;get;}
        public long IN_DATE { get; set; }

        public long? OUT_DATE { get; set; }

        public long? IN_ROOM_ID { get; set; }

        public short? IS_EMERGENCY { get; set; }

        public decimal? TREATMENT_DAY_COUNT { get; set; }

        public long? DEATH_WITHIN_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
    }

    public class TREATMENT_BED_ROOM
    {
        public long TREATMENT_ID { get; set; }
        public long? ADD_TIME { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long ROOM_ID { get; set; }
    }
    //public class TreatInAndTreatOut
    //{
    //    public Mrs00555RDO TreatIn { get; set; }
    //    public Mrs00555RDO TreatOut { get; set; }
    //}
    public class DEPA_COUNT_BED
    {
        public long DEPARTMENT_ID { get; set; }
        public long COUNT_BED { get; set; }
    }
}

