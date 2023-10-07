using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using Inventec.Common.Repository;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00603
{
    public class Mrs00603RDO : V_HIS_TREATMENT
    {
        public int? AGE_MALE { get; set; }//
        public int? AGE_FEMALE { get; set; }//
        public string IS_BHYT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_IN_TIME_STR { get; set; }
        public string DEPARTMENT_OUT_TIME_STR { get; set; }
        public string PATIENT_DOB_STR { set; get; }
        public string IN_DATE_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string SEX { get; set; }
        public decimal TOTAL_DAY { get; set; }
        public Mrs00603RDO(V_HIS_TREATMENT r, List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran, HIS_DEPARTMENT_TRAN departmentTran, Mrs00603Filter filter, bool isPrevTreatIn, bool isPrevTreatOut, HIS_DEPARTMENT_TRAN previousDepartmentTran, HIS_DEPARTMENT_TRAN nextDepartmentTran, long? DepartmentOutTime)
        {
            PropertyInfo[] p = Properties.Get<V_HIS_TREATMENT>();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtensionField(this, listHisDepartmentTran, departmentTran, filter, isPrevTreatIn, isPrevTreatOut, previousDepartmentTran, nextDepartmentTran, DepartmentOutTime);
        }

        private void SetExtensionField(Mrs00603RDO r, List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran, HIS_DEPARTMENT_TRAN departmentTran,  Mrs00603Filter filter,bool isPrevTreatIn, bool isPrevTreatOut, HIS_DEPARTMENT_TRAN previousDepartmentTran, HIS_DEPARTMENT_TRAN nextDepartmentTran, long? DepartmentOutTime)
        {

            this.DEPARTMENT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTran.DEPARTMENT_IN_TIME ?? 0);
            this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            if (DepartmentOutTime > 0)
            {
                this.DEPARTMENT_OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(DepartmentOutTime ?? 0);
                DateTime outTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(DepartmentOutTime ?? 0) ?? DateTime.MinValue;
                DateTime inTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(departmentTran.DEPARTMENT_IN_TIME ?? 0) ?? DateTime.MinValue;
                this.TOTAL_DAY = ProcessDayCount(outTime, inTime, isPrevTreatIn || isPrevTreatOut || nextDepartmentTran != null && nextDepartmentTran.DEPARTMENT_IN_TIME > 0);
            }
            CalcuatorAge(r);
        }
        private decimal ProcessDayCount(DateTime end, DateTime begin, bool isTran)
        {
            decimal result = 0;
            try
            {
                var hoursBetween = end - begin;
                if (hoursBetween.TotalHours > 8)//Lay ngay ra - ngay vao + 1
                {
                    DateTime dayEnd = new DateTime(end.Year, end.Month, end.Day);
                    DateTime dayBegin = new DateTime(begin.Year, begin.Month, begin.Day);

                    if (isTran)//chuyen khoa se them 0.5 ngay
                    {
                        result = Convert.ToInt64((dayEnd - dayBegin).TotalDays)+0.5m;
                    }
                    else
                    {
                        result = Convert.ToInt64((dayEnd - dayBegin).TotalDays) + 1;
                    }
                }
                else if (hoursBetween.TotalHours <= 8 && hoursBetween.TotalHours >= 4)//Neu BN nam tu 4 - 8h --> tinh 1 ngay
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
      

        private void CalcuatorAge(V_HIS_TREATMENT r)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(r.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.AGE_MALE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        this.AGE_FEMALE = (tuoi >= 1) ? tuoi : 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    public class Mrs00603RDO_TotalDepartment
    {
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal TOTAL_DAY_IN { get; set; }
        public decimal TOTAL_DAY_END { get; set; }
    }
}
