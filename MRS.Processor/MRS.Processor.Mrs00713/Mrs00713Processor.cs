using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using FlexCel.Report;

namespace MRS.Processor.Mrs00713
{
    public class Mrs00713Processor : AbstractProcessor
    {
        private Mrs00713Filter filter;
        List<Mrs00713RDO> listTreatment = new List<Mrs00713RDO>();
        CommonParam paramGet = new CommonParam();
        public Mrs00713Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00713Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00713Filter)reportFilter;
            try
            {
                listTreatment = new MRS.Processor.Mrs00713.ManagerSql().GetTreatment(filter) ?? new List<Mrs00713RDO>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                foreach (var item in listTreatment)
                {
                    var inTime = LongTimeToDateTime(item.IN_TIME);
                    var outTime = LongTimeToDateTime(item.OUT_TIME ?? 0);
                    var feeLockTime = LongTimeToDateTime(item.FEE_LOCK_TIME ?? 0);
                    var examIntructionTime = LongTimeToDateTime(item.EXAM_INTRUCTION_TIME ?? 0);
                    var examStartTime = LongTimeToDateTime(item.EXAM_START_TIME ?? 0);
                    var examEndTime = LongTimeToDateTime(item.EXAM_FINISH_TIME ?? 0);
                    var testIntructionTime = LongTimeToDateTime(item.TEST_INTRUCTION_TIME ?? 0);
                    var testStartTime = LongTimeToDateTime(item.TEST_START_TIME ?? 0);
                    var testResultTime = LongTimeToDateTime(item.TEST_RESULT_TIME ?? 0);
                    var testFinishTime = LongTimeToDateTime(item.TEST_FINISH_TIME ?? 0);
                    var cdhaIntructionTime = LongTimeToDateTime(item.CDHA_INTRUCTION_TIME ?? 0);
                    var cdhaStartTime = LongTimeToDateTime(item.CDHA_START_TIME ?? 0);
                    var cdhaFinishTime = LongTimeToDateTime(item.CDHA_FINISH_TIME ?? 0);
                    var tdcnIntructionTime = LongTimeToDateTime(item.TDCN_INTRUCTION_TIME ?? 0);
                    var tdcnStartTime = LongTimeToDateTime(item.TDCN_START_TIME ?? 0);
                    var tdcnFinishTime = LongTimeToDateTime(item.TDCN_FINISH_TIME ?? 0);
                    var clsFinishTime = LongTimeToDateTime(item.CLS_FINISH_TIME ?? 0);
                    var expFinishTime = LongTimeToDateTime(item.EXP_FINISH_TIME ?? 0);


                    item.WAIT_EXAM = TimeSpanOfTwoTime(inTime, examStartTime);
                    item.WAIT_OUT = TimeSpanOfTwoTime(examStartTime, outTime);
                    item.TIME_FOR_EXAM = TimeSpanOfTwoTime(examStartTime, examEndTime);
                    item.WAIT_FOR_TEST = TimeSpanOfTwoTime(testIntructionTime, testStartTime);
                    item.WAIT_TEST_RESULT = TimeSpanOfTwoTime(testStartTime, testResultTime);
                    item.WAIT_FOR_CDHA = TimeSpanOfTwoTime(cdhaIntructionTime, cdhaStartTime);
                    item.WAIT_CDHA_FINISH = TimeSpanOfTwoTime(cdhaStartTime, cdhaFinishTime);
                    item.WAIT_FOR_TDCN = TimeSpanOfTwoTime(tdcnIntructionTime, tdcnStartTime);
                    item.WAIT_TDCN_FINISH = TimeSpanOfTwoTime(tdcnStartTime, tdcnFinishTime);
                    item.WAIT_CLS_OUT = TimeSpanOfTwoTime(clsFinishTime, outTime);
                    item.WAIT_FEE_LOCK = TimeSpanOfTwoTime(outTime, feeLockTime);
                    item.WAIT_MEDI_EXP = TimeSpanOfTwoTime(feeLockTime, expFinishTime);

                    item.CREATE_TIME_STR = ConvertToString(item.CREATE_TIME); //dang ky
                    item.IN_TIME_STR = ConvertToString(item.IN_TIME); //vao vien
                    item.EXAM_START_TIME_STR = ConvertToString(item.EXAM_START_TIME ?? 0); //bat dau kham
                    item.EXAM_END_TIME_STR = ConvertToString(item.EXAM_FINISH_TIME ?? 0); //ket thuc kham
                    item.TEST_START_TIME_STR = ConvertToString(item.TEST_START_TIME ?? 0); //bat dau xn
                    item.TEST_RESULT_TIME_STR = ConvertToString(item.TEST_RESULT_TIME ?? 0); //lay mau xn
                    item.CDHA_START_TIME_STR = ConvertToString(item.CDHA_START_TIME ?? 0); //bat dau cdha
                    item.CDHA_FINISH_TIME_STR = ConvertToString(item.CDHA_FINISH_TIME ?? 0); //ket thuc cdha
                    item.TDCN_START_TIME_STR = ConvertToString(item.TDCN_START_TIME ?? 0); //bat dau tdcn
                    item.TDCN_FINISH_TIME_STR = ConvertToString(item.TDCN_FINISH_TIME ?? 0); //ket thuc tdcn
                    item.FEE_LOCK_TIME_STR = ConvertToString(item.FEE_LOCK_TIME ?? 0); //khoa vien phi
                    item.EXP_FINISH_TIME_STR = ConvertToString(item.EXP_FINISH_TIME ?? 0); //linh thuoc

                    item.EXAM_INTRUCTION_TIME_STR = ConvertToString(item.EXAM_INTRUCTION_TIME ?? 0);
                    item.TEST_INTRUCTION_TIME_STR = ConvertToString(item.TEST_INTRUCTION_TIME ?? 0);
                    item.CDHA_INTRUCTION_TIME_STR = ConvertToString(item.CDHA_INTRUCTION_TIME ?? 0);
                    item.TDCN_INTRUCTION_TIME_STR = ConvertToString(item.TDCN_INTRUCTION_TIME ?? 0);
                    item.TEST_FINISH_TIME_STR = ConvertToString(item.TEST_FINISH_TIME ?? 0);
                    item.EXAM_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                    item.EXAM_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string ConvertToString(long Time)
        {
            if (Time != 99999999999999 && Time != null)
            {
                return Inventec.Common.DateTime.Convert.TimeNumberToTimeString(Time);
            }
            else return null;
        }

        DateTime? LongTimeToDateTime(long? Time)
        {
            DateTime? result = null;
            try
            {

                if (Time == null || Time == 0) return result;
                long? longNow = 99999999999999;
                var strTime = Time.ToString();
                if (Time > longNow || strTime.Length < 14)
                {
                    return result;
                }
                result = new DateTime(int.Parse(strTime.Substring(0, 4)), int.Parse(strTime.Substring(4, 2)), int.Parse(strTime.Substring(6, 2)), int.Parse(strTime.Substring(8, 2)), int.Parse(strTime.Substring(10, 2)), int.Parse(strTime.Substring(12, 2)));
                return result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        double TimeSpanOfTwoTime(DateTime? startTime, DateTime? finishTime)
        {
            double result = 0;
            if (startTime == null)
            {
                return result;
            }
            if (finishTime == null)
            {
                return result;
            }

            TimeSpan diffTime = finishTime.Value - startTime.Value;

            result = Math.Round(diffTime.TotalMinutes, 0); //string.Format("{0}{1}:{2}:{3}", diffTime.Days > 0 ? diffTime.Days + "N " : "", diffTime.Hours, diffTime.Minutes, diffTime.Seconds);

            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", listTreatment);
        }


    }
}
