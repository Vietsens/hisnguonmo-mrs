using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MRS.MANAGER.Config;
using MOS.EFMODEL.DataModels;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOCommon
    {


        public static void GenerateAge(ref int? female, ref int? male, long? dob, long? genderId)
        {
            try
            {
                if (dob.HasValue && genderId.HasValue)
                {
                    int age = Inventec.Common.DateTime.Calculation.Age(dob.Value);
                    if (IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE == genderId.Value)
                    {
                        female = age;
                    }
                    else
                    {
                        male = age;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static int? CalculateAge(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                    //if (thang > 0)
                    //{
                    //    tuoi = thang.ToString();
                    //    cboAge = "Tháng";
                    //}
                    //else
                    //{
                    //    if (ngay > 0)
                    //    {
                    //        tuoi = ngay.ToString();
                    //        cboAge = "ngày";
                    //    }
                    //    else
                    //    {
                    //        tuoi = "";
                    //        cboAge = "Giờ";
                    //    }
                    //}
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        public static int? CalculateAge(long ngaySinh, long ngayVao)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngaySinh) ?? DateTime.MinValue;
                DateTime dtNgVao = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayVao) ?? DateTime.MinValue;
                TimeSpan diff = dtNgVao - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }
    }
}
