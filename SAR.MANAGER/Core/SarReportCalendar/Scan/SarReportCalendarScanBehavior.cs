using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using SAR.MANAGER.Core.Check;
using Inventec.Core;
using System;
using SAR.MANAGER.Core.SarReportCalendar.Get;
using Inventec.Common.Logging;
using AutoMapper;
using System.Collections.Generic;
using SAR.MANAGER.Core.SarReport;

namespace SAR.MANAGER.Core.SarReportCalendar.Scan
{
    class SarReportCalendarScanBehavior : BeanObjectBase, ISarReportCalendarScan
    {
        internal SarReportCalendarScanBehavior(CommonParam param)
            : base(param)
        {

        }

        bool ISarReportCalendarScan.Run()
        {
            bool result = false;
            try
            {
                long? now = Inventec.Common.DateTime.Get.Now();
                if (!now.HasValue) throw new NullReferenceException();

                SarReportCalendarFilterQuery filter = new SarReportCalendarFilterQuery();
                filter.EXECUTE = SAR.Filter.SarReportCalendarFilter.ExecuteEnum.NOT_EXECUTE;
                filter.SCAN_TIME = now.Value;

                List<SAR_REPORT_CALENDAR> listCalendar = new SarReportCalendarBO().Get<List<SAR_REPORT_CALENDAR>>(filter);
                if (listCalendar != null && listCalendar.Count > 0)
                {
                    foreach (var calendar in listCalendar)
                    {
                        Logging("Bat dau xu ly lich bao cao.Time=" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".Ma lich=" + calendar.REPORT_CALENDAR_CODE, LogType.Info);
                        MRS.SDO.CreateReportSDO filterTotal = Newtonsoft.Json.JsonConvert.DeserializeObject<MRS.SDO.CreateReportSDO>(calendar.FILTER_TOTAL_JSON);
                        var ro = new MRS.MANAGER.Sar.SarReport.MrsReportCreate(param).Create(filterTotal);
                        if (ro != null)
                        {
                            Logging("Xu ly thanh cong bao cao. Ma lich=" + calendar.REPORT_CALENDAR_CODE, LogType.Info);

                            GenerateNextCalendar(calendar);
                            UpdateAfterSuccess(calendar, ro.ID);
                            result = true;
                        }
                        else
                        {
                            Logging("Xu ly bao cao theo lich (calendar) khong thanh cong." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => calendar), calendar), LogType.Error);
                        }
                        Logging("Ket thuc xu ly lich bao cao.Time=" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".Ma lich=" + calendar.REPORT_CALENDAR_CODE, LogType.Info);
                    }
                }
                else
                {
                    Logging("Khong co lich bao cao nao can xu ly.", LogType.Info);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        private void UpdateAfterSuccess(SAR_REPORT_CALENDAR calendar, long reportId)
        {
            try
            {
                calendar.REPORT_ID = reportId;
                calendar.IS_DONE = 1;
                if (!new SarReportCalendarBO().Update(calendar))
                {
                    Logging("Khong update duoc report_id & is_done cho calendar sau khi xu ly xong calendar." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => calendar), calendar), LogType.Error);
                }
            }
            catch (Exception ex)
            {
                Logging("Co exception khi update report_id & is_done cho calendar.", LogType.Error);
                LogSystem.Error(ex);
            }
        }

        private void GenerateNextCalendar(SAR_REPORT_CALENDAR calendar)
        {
            try
            {
                System.DateTime? executeTimeSys = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(calendar.EXECUTE_TIME);
                if (executeTimeSys.HasValue)
                {
                    System.DateTime? nextTimeSys = null;

                    if (calendar.IS_DAY_REPEAT.HasValue && calendar.IS_DAY_REPEAT.Value == 1)
                    {
                        nextTimeSys = executeTimeSys.Value.AddDays(1);
                    }
                    else if (calendar.IS_WEEK_REPEAT.HasValue && calendar.IS_WEEK_REPEAT.Value == 1)
                    {
                        nextTimeSys = executeTimeSys.Value.AddDays(7);
                    }
                    else if (calendar.IS_MONTH_REPEAT.HasValue)
                    {
                        if (calendar.IS_MONTH_REPEAT.Value == 1)
                        {
                            //Cong them 1 thang (de dam bao sang thang tiep theo)
                            //Sau do tim den ngay cuoi thang
                            System.DateTime nextMonth = executeTimeSys.Value.AddMonths(1);
                            System.DateTime? temp = Inventec.Common.DateTime.Get.EndMonthSystemDateTime(nextMonth);
                            if (temp.HasValue)
                            {
                                nextTimeSys = temp.Value;
                            }
                        }
                        else if (calendar.IS_MONTH_REPEAT.Value == 1)
                        {
                            ///Khong the su dung ham AddMonths cua .NET vi khong chinh xac (31/10 --> 30/11)
                            ///Su dung ham DayOfNextMonth cua thu vien Common.DateTime
                            nextTimeSys = Inventec.Common.DateTime.Get.SameDayOfNextMonth(executeTimeSys);
                        }
                    }

                    long? nextTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(nextTimeSys.Value);

                    ///Sinh lich tiep theo trong truong hop calendar khong co end_time hoac end_time >= nextTime
                    if (nextTime.HasValue && (!calendar.END_TIME.HasValue || (calendar.END_TIME.HasValue && calendar.END_TIME.Value >= nextTime.Value)))
                    {
                        Mapper.CreateMap<SAR_REPORT_CALENDAR, SAR_REPORT_CALENDAR>();
                        SAR_REPORT_CALENDAR nextCalendar = Mapper.Map<SAR_REPORT_CALENDAR>(calendar);
                        nextCalendar.ID = 0;
                        nextCalendar.EXECUTE_TIME = nextTime.Value;
                        nextCalendar.SOURCE_ID = (calendar.SOURCE_ID.HasValue ? calendar.SOURCE_ID.Value : calendar.ID);
                        if (!new SarReportCalendarBO().Create(nextCalendar))
                        {
                            Logging("Khong them duoc lich tiep theo." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextCalendar), nextCalendar), LogType.Error);
                        }
                    }
                }
                else
                {
                    Logging("Khong ep duoc kieu ngay thang tu calendar.EXECUTE_TIME.Kiem tra lai db hoac lib datetime.Lich bao cao tiep theo khong duoc thuc hien cho du co cau hinh." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => calendar), calendar), LogType.Error);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error("Co exception khi sinh lich tiep theo.", ex);
            }
        }
    }
}
