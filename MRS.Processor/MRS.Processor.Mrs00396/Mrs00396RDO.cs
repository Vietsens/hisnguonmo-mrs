using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00396
{
    public class Mrs00396RDO
    {
        public string TREATMENT_CODE { get; set; }
        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long BED_PLAN { get; set; }	//Giường kế hoạch
        public decimal COUNT_OLD { get; set; }//Cũ
        public decimal COUNT_NEW { get; set; }//mới
        public decimal COUNT_MOVE_IN { get; set; }	//vào khoa

        public decimal COUNT_MOV_OUT { get; set; }	//ra khoa
        public decimal COUNT_LAST { get; set; }//cuối kì
        public decimal BED_TRUST { get; set; }//giường thực kê
        public decimal NUM_DAY { get; set; }//số ngày
        public decimal POWER_PLAN { get; set; }//công suất KH
        public decimal POWER_TRUST { get; set; }//công suât thực
        public long? NUM_ORDER { get; set; }//số TT

        public string DEPARTMENT_CODE { get; set; }


        public Mrs00396RDO(HIS_DEPARTMENT_TRAN inDepartmentTran, HIS_DEPARTMENT_TRAN previousDepartmentTran, HIS_DEPARTMENT_TRAN nextDepartmentTran, HIS_TREATMENT trea, HIS_PATIENT_TYPE_ALTER lastPta, Mrs00396Filter mrs00396Filter, bool previousIsTreatIn)
        {

            this.EXECUTE_DEPARTMENT_ID = inDepartmentTran.DEPARTMENT_ID;
            //this.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            //this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            //this.POWER_PLAN = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).THEORY_PATIENT_COUNT ?? 0;
            //this.BED_TRUST = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).REALITY_PATIENT_COUNT ?? 0;
            //this.NUM_ORDER = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == inDepartmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).NUM_ORDER;

            if (trea.CLINICAL_IN_TIME >= mrs00396Filter.TIME_TO)
            {
                return;
            }


            if (nextDepartmentTran.DEPARTMENT_IN_TIME == null) nextDepartmentTran.DEPARTMENT_IN_TIME = (trea.IS_PAUSE == 1 ? trea.OUT_TIME : 99999999999999);

            if ((long)nextDepartmentTran.DEPARTMENT_IN_TIME / 100 <= (long)trea.CLINICAL_IN_TIME / 100)
            {
                return;
            }
            if ((long)inDepartmentTran.DEPARTMENT_IN_TIME / 100 < (long)trea.CLINICAL_IN_TIME / 100)
            {
                inDepartmentTran.DEPARTMENT_IN_TIME = trea.CLINICAL_IN_TIME;
            }
            //BN dau ki se lay:
            // vao khoa truoc Time_from ra khoa sau time_from
            if (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00396Filter.TIME_FROM && mrs00396Filter.TIME_FROM <= nextDepartmentTran.DEPARTMENT_IN_TIME)
            {
                this.COUNT_OLD = 1;
            }
            //Nguoi benh vao dieu tri se lay:
            //vao khoa tu time_from den time_to
            if (inDepartmentTran.DEPARTMENT_IN_TIME >= mrs00396Filter.TIME_FROM && inDepartmentTran.DEPARTMENT_IN_TIME < mrs00396Filter.TIME_TO)
            {
                //neu khoa truoc co dieu tri va khoa nay tiep nhan vao dieu tri thi goi là moi chuyen tu khoa noi tru khac den
                if ((long)inDepartmentTran.DEPARTMENT_IN_TIME / 100 > (long)trea.CLINICAL_IN_TIME / 100 &&
                previousIsTreatIn)
                {
                    this.COUNT_MOVE_IN = 1;
                }
                else
                //nguoc lai thi goi la moi vao noi tru tu phong kham
                {
                    this.COUNT_NEW = 1;

                }


            }
            //neu thoi gian vao khoa tiep theo trong khoang bao cao thi la chuyen khoa tiep theo
            if (nextDepartmentTran.ID > 0 && nextDepartmentTran.DEPARTMENT_IN_TIME >= mrs00396Filter.TIME_FROM && nextDepartmentTran.DEPARTMENT_IN_TIME < mrs00396Filter.TIME_TO)
            {
                this.COUNT_MOV_OUT = 1;
            }

            if (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00396Filter.TIME_TO && mrs00396Filter.TIME_FROM <= nextDepartmentTran.DEPARTMENT_IN_TIME)
            {

                long startTime = (inDepartmentTran.DEPARTMENT_IN_TIME < mrs00396Filter.TIME_FROM) ? mrs00396Filter.TIME_FROM : (inDepartmentTran.DEPARTMENT_IN_TIME ?? 0);
                long endTime = (nextDepartmentTran.DEPARTMENT_IN_TIME > mrs00396Filter.TIME_TO) ? mrs00396Filter.TIME_TO : (nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0);

                DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(startTime);
                DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(endTime);
                TimeSpan sp = OutTime.Date - InTime.Date;
                var days = sp.TotalDays;

                //phuong an 3: tinh theo thoi gian vao ra khoa phu thuoc loai hinh ra vien
                this.point_day_with_out(trea, (long)days, (nextDepartmentTran.ID == 0), mrs00396Filter);

            }
            if (trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME < mrs00396Filter.TIME_TO && trea.OUT_TIME >= mrs00396Filter.TIME_FROM)
            {
                if (nextDepartmentTran.ID == 0)
                {
                    this.COUNT_EXP = 1;

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
                            {
                                this.COUNT_OUT_NANG = 1;
                                if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                                {
                                    this.COUNT_OUT_NANG_CV = 1;
                                }
                            }
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET:
                            {
                                this.COUNT_OUT_NANG_CHET = 1;
                            }
                            break;
                    }
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        this.COUNT_OUT_CV = 1;
                    }
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV)
                    {
                        this.COUNT_OUT_CTCV = 1;
                    }
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN)
                        this.COUNT_OUT_HEN = 1;
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__KHAC)
                        this.COUNT_OUT_KHAC = 1;
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN)
                        this.COUNT_OUT_RAVIEN = 1;
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                    {
                        this.COUNT_OUT_TRON = 1;
                    }
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                        this.COUNT_OUT_XINRAVIEN = 1;
                    if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                    {
                        this.COUNT_DIE = 1;
                    }
                }

            }

            if (nextDepartmentTran.DEPARTMENT_IN_TIME >= mrs00396Filter.TIME_TO)
            {
                this.COUNT_LAST = 1;
                if (trea.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.COUNT_LAST_BH = 1;
                }
                else
                {
                    this.COUNT_LAST_VP = 1;
                }
            }
        }

        public Mrs00396RDO()
        {
            // TODO: Complete member initialization
        }

        public void point_day_with_out(HIS_TREATMENT trea, long diffDate, bool IsLastDepartment, Mrs00396Filter mrs00396Filter)
        {
            try
            {
                //tinh so ngay phu thuoc vao viec da ra vien chua, va ra vien loai nao
                var starttime = trea.IN_TIME > mrs00396Filter.TIME_FROM ? trea.IN_TIME : mrs00396Filter.TIME_FROM;
                var finishtime = trea.OUT_TIME.HasValue && trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME.Value < mrs00396Filter.TIME_TO ? trea.OUT_TIME.Value : mrs00396Filter.TIME_TO;
                if (starttime > finishtime)
                {
                    Inventec.Common.Logging.LogSystem.Info("Treatment_code" + trea.TREATMENT_CODE);

                }
                else
                {
                    this.NUM_DAY = diffDate;
                    if (IsLastDepartment)
                    {
                        if (trea.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && trea.OUT_TIME < mrs00396Filter.TIME_TO)
                        {
                            if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                            {
                                this.NUM_DAY += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN && trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                            {
                                this.NUM_DAY += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN && trea.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                            {
                                this.NUM_DAY += 1;
                            }
                            else if (trea.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && (trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_KHONG_LIEN_KE || trea.TRAN_PATI_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_TRAN_PATI_FORM.ID__DUOI_LEN_LIEN_KE))
                            {
                                this.NUM_DAY += 1;
                            }
                            //neu ra vien tai khoa nhung thoi gian dieu tri o khoa=0 thi gan lai =1
                            else if (this.NUM_DAY == 0)
                            {
                                this.NUM_DAY = 1;
                            }
                        }
                    }
                    int diffHour = Inventec.Common.DateTime.Calculation.DifferenceTime(starttime, finishtime, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                    if (diffHour < 4)
                    {
                        this.NUM_DAY = 0;
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

        public decimal COUNT_EXP { get; set; }
        public decimal COUNT_OUT_KHOI { get; set; }
        public decimal COUNT_OUT_DO { get; set; }
        public decimal COUNT_OUT_KTD { get; set; }
        public decimal COUNT_OUT_NANG { get; set; }
        public decimal COUNT_OUT_NANG_CV { get; set; }
        public decimal COUNT_OUT_NANG_CHET { get; set; }
        public decimal COUNT_OUT_CV { get; set; }


        public decimal COUNT_OUT_XINRAVIEN { get; set; }

        public decimal COUNT_OUT_TRON { get; set; }

        public decimal COUNT_OUT_RAVIEN { get; set; }

        public decimal COUNT_OUT_KHAC { get; set; }

        public decimal COUNT_OUT_HEN { get; set; }

        public decimal COUNT_OUT_CTCV { get; set; }

        public decimal COUNT_DIE { get; set; }

        public decimal COUNT_LAST_BH { get; set; }

        public decimal COUNT_LAST_VP { get; set; }
    }
}
