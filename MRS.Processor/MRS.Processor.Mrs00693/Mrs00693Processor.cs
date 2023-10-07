using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00693
{
    public class Mrs00693Processor : AbstractProcessor
    {
        private Mrs00693Filter filter;
        private List<Mrs00693RDO> listRdo = new List<Mrs00693RDO>();
        private List<Mrs00693RDO> listYearRdo = new List<Mrs00693RDO>();
        private List<Mrs00693RDO> listMonthRdo = new List<Mrs00693RDO>();
        DateTime dtNow;
        DateTime dt12MonthAgo;
        List<TreatmentData> datas = new List<TreatmentData>();

        public Mrs00693Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00693Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            filter = (Mrs00693Filter)reportFilter;
            try
            {
                if (filter.DEPARTMENT_ID <= 0)
                {
                    BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__ThieuThongTinBatBuoc);
                    throw new Exception("DEPARTMENT_ID <= 0");
                }

                this.dtNow = DateTime.Now;
                if (filter.DATE_TIME >0)
                {
                    this.dtNow = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.DATE_TIME??0)?? DateTime.Now;
                }
                DateTime dt = dtNow.AddMonths(-11);
                this.dt12MonthAgo = new DateTime(dt.Year, dt.Month, 1);
                long dateTo = Convert.ToInt64(dtNow.ToString("yyyyMMdd") + "000000");
                long dateFrom = Convert.ToInt64(dt12MonthAgo.ToString("yyyyMMdd") + "000000");

                StringBuilder sb = new StringBuilder()
                .Append("SELECT TREA.ID, TREA.IN_TIME, TREA.OUT_TIME, TREA.OUT_DATE, (TREA.OUT_DATE - MOD(TREA.OUT_DATE, 100000000)) AS OUT_MONTH")
                .Append(", (SELECT 1 FROM DUAL WHERE EXISTS (SELECT 1 FROM HIS_SERVICE_REQ XN")
                .Append(" WHERE XN.TREATMENT_ID = TREA.ID AND XN.IS_DELETE = 0")
                .Append(" AND (XN.IS_NO_EXECUTE IS NULL OR XN.IS_NO_EXECUTE <> 1)")
                .Append(" AND XN.SERVICE_REQ_TYPE_ID = 2")
                .Append(")) AS IS_HAS_TEST")
                .Append(", (SELECT 1 FROM DUAL WHERE EXISTS (SELECT 1 FROM HIS_SERVICE_REQ CDHA")
                .Append(" WHERE CDHA.TREATMENT_ID = TREA.ID AND CDHA.IS_DELETE = 0")
                .Append(" AND (CDHA.IS_NO_EXECUTE IS NULL OR CDHA.IS_NO_EXECUTE <> 1)")
                .Append(" AND CDHA.SERVICE_REQ_TYPE_ID IN (3,9,10)")
                .Append(")) AS IS_HAS_CDHA")
                .Append(", (SELECT 1 FROM DUAL WHERE EXISTS (SELECT 1 FROM HIS_SERVICE_REQ TDCH")
                .Append(" WHERE TDCH.TREATMENT_ID = TREA.ID AND TDCH.IS_DELETE = 0")
                .Append(" AND (TDCH.IS_NO_EXECUTE IS NULL OR TDCH.IS_NO_EXECUTE <> 1)")
                .Append(" AND TDCH.SERVICE_REQ_TYPE_ID = 5")
                .Append(")) AS IS_HAS_TDCN")
                .Append(" FROM HIS_TREATMENT TREA")
                .Append(" WHERE TREA.IS_PAUSE = 1")
                .Append(" AND TREA.TDL_TREATMENT_TYPE_ID = 1")
                .Append(" AND TREA.OUT_DATE BETWEEN : param1 AND : param2")
                .Append(" AND TREA.END_DEPARTMENT_ID =: param3");

                string sql = sb.ToString();
                LogSystem.Info("Sql: " + sql);
                LogSystem.Info("FROM - TO " + dateFrom + " - " + dateTo);
                this.datas = new MOS.DAO.Sql.SqlDAO().GetSql<TreatmentData>(sql, dateFrom, dateTo, this.filter.DEPARTMENT_ID);
                LogSystem.Info("COUNT: " + (this.datas != null ? this.datas.Count : -1));
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
                Mrs00693RDO yearRdo = new Mrs00693RDO();
                Mrs00693RDO monthRdo = new Mrs00693RDO();
                Mrs00693RDO avgRdo = new Mrs00693RDO();
                avgRdo.TYPE_CODE = "D";
                avgRdo.TYPE_NAME = "Thời gian TB chờ khám";

                Dictionary<string, Mrs00693RDO> dicRdo = new Dictionary<string, Mrs00693RDO>();
                Dictionary<int, List<decimal>> dicAmount = new Dictionary<int, List<decimal>>();
                for (int i = 0; i < 12; i++)
                {
                    DateTime dt = this.dt12MonthAgo.AddMonths(i);
                    long monthDate = Convert.ToInt64(dt.ToString("yyyyMM") + "00000000");
                    this.SetYear(i, dt, ref yearRdo);
                    this.SetMonth(i, dt, ref monthRdo);
                    List<TreatmentData> listOfMonth = IsNotNullOrEmpty(datas) ? datas.Where(o => o.OUT_MONTH == monthDate).ToList() : null;

                    if (IsNotNullOrEmpty(listOfMonth))
                    {
                        this.ProcessAKham(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamCdha(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamTdcn(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamXn(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamCdhaTdcn(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamCdhaXn(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamTdcnXn(i, listOfMonth, ref dicRdo, ref dicAmount);
                        this.ProcessAKhamCdhaTdcnXn(i, listOfMonth, ref dicRdo, ref dicAmount);
                    }
                    else
                    {
                        LogSystem.Warn("Thang khong co du lieu: " + monthDate);
                    }
                }
                this.listYearRdo.Add(yearRdo);
                this.listMonthRdo.Add(monthRdo);
                this.listRdo = dicRdo.Values.OrderBy(o => o.TYPE_CODE).ToList();

                if (dicAmount.Count > 0)
                {
                    foreach (var dic in dicAmount)
                    {
                        if (dic.Value.Count > 0)
                        {
                            decimal count = (decimal)dic.Value.Count;
                            decimal sum = dic.Value.Sum(s => s);
                            decimal avg = Math.Round((sum / count), 2, MidpointRounding.AwayFromZero);
                            this.SetAmount(dic.Key, avg, ref avgRdo);
                        }
                    }
                }
                this.listRdo.Add(avgRdo);
                this.ProcessAvgRdo();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessAvgRdo()
        {
            if (IsNotNullOrEmpty(this.listRdo))
            {
                foreach (var rdo in this.listRdo)
                {
                    this.ProcessAmountAvg(rdo);
                }
            }
        }

        private void ProcessAmountAvg(Mrs00693RDO rdo)
        {
            List<decimal> listAmount = new List<decimal>();
            if (rdo.AMOUNT_1 > 0)
            {
                listAmount.Add(rdo.AMOUNT_1);
            }
            if (rdo.AMOUNT_2 > 0)
            {
                listAmount.Add(rdo.AMOUNT_2);
            }
            if (rdo.AMOUNT_3 > 0)
            {
                listAmount.Add(rdo.AMOUNT_3);
            }
            if (rdo.AMOUNT_4 > 0)
            {
                listAmount.Add(rdo.AMOUNT_4);
            }
            if (rdo.AMOUNT_5 > 0)
            {
                listAmount.Add(rdo.AMOUNT_5);
            }
            if (rdo.AMOUNT_6 > 0)
            {
                listAmount.Add(rdo.AMOUNT_6);
            }
            if (rdo.AMOUNT_7 > 0)
            {
                listAmount.Add(rdo.AMOUNT_7);
            }
            if (rdo.AMOUNT_8 > 0)
            {
                listAmount.Add(rdo.AMOUNT_8);
            }
            if (rdo.AMOUNT_9 > 0)
            {
                listAmount.Add(rdo.AMOUNT_9);
            }
            if (rdo.AMOUNT_10 > 0)
            {
                listAmount.Add(rdo.AMOUNT_10);
            }
            if (rdo.AMOUNT_11 > 0)
            {
                listAmount.Add(rdo.AMOUNT_11);
            }
            if (rdo.AMOUNT_12 > 0)
            {
                listAmount.Add(rdo.AMOUNT_12);
            }

            if (listAmount.Count > 0)
            {
                decimal count = (decimal)listAmount.Count;
                decimal sum = listAmount.Sum(s => s);
                rdo.AMOUNT_AVG = Math.Round((sum / count), 2, MidpointRounding.AwayFromZero);
            }
        }

        private void ProcessAKham(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khams = listOfMonth.Where(o => !o.IS_HAS_CDHA.HasValue && !o.IS_HAS_TDCN.HasValue && !o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khams))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "A";
                rdo.TYPE_NAME = "a.KHAM";
                rdo.NOTE = "(2 giờ)";
                rdo.REGULATION_AMOUNT = "< 120";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khams)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamCdha(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamCdhas = listOfMonth.Where(o => o.IS_HAS_CDHA.HasValue && !o.IS_HAS_TDCN.HasValue && !o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamCdhas))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "B1";
                rdo.TYPE_NAME = "b.KHAM + CĐHA";
                rdo.NOTE = "(2.5 giờ)";
                rdo.REGULATION_AMOUNT = "< 150";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamCdhas)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamTdcn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamTdcns = listOfMonth.Where(o => !o.IS_HAS_CDHA.HasValue && o.IS_HAS_TDCN.HasValue && !o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamTdcns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "B2";
                rdo.TYPE_NAME = "b.KHAM + TDCN";
                rdo.NOTE = "(3 giờ)";
                rdo.REGULATION_AMOUNT = "< 180";

                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamTdcns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamXn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamXns = listOfMonth.Where(o => !o.IS_HAS_CDHA.HasValue && !o.IS_HAS_TDCN.HasValue && o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamXns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "B3";
                rdo.TYPE_NAME = "b.KHAM + XN";
                rdo.NOTE = "(3 giờ)";
                rdo.REGULATION_AMOUNT = "< 180";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamXns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamCdhaTdcn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamCdhaTdcns = listOfMonth.Where(o => o.IS_HAS_CDHA.HasValue && o.IS_HAS_TDCN.HasValue && !o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamCdhaTdcns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "C1";
                rdo.TYPE_NAME = "c.KHAM + CĐHA + TDCN";
                rdo.NOTE = "(3.5 giờ)";
                rdo.REGULATION_AMOUNT = "< 210";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamCdhaTdcns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamCdhaXn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamCdhaXns = listOfMonth.Where(o => o.IS_HAS_CDHA.HasValue && !o.IS_HAS_TDCN.HasValue && o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamCdhaXns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "C2";
                rdo.TYPE_NAME = "c.KHAM + CĐHA + XN";
                rdo.NOTE = "(3.5 giờ)";
                rdo.REGULATION_AMOUNT = "< 210";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamCdhaXns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamTdcnXn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamTdcnXns = listOfMonth.Where(o => !o.IS_HAS_CDHA.HasValue && o.IS_HAS_TDCN.HasValue && o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamTdcnXns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "C2";
                rdo.TYPE_NAME = "c.KHAM + TDCN + XN";
                rdo.NOTE = "(4 giờ)";
                rdo.REGULATION_AMOUNT = "< 240";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamTdcnXns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private void ProcessAKhamCdhaTdcnXn(int i, List<TreatmentData> listOfMonth, ref Dictionary<string, Mrs00693RDO> dicRdo, ref Dictionary<int, List<decimal>> dicAmount)
        {
            List<TreatmentData> khamTdcnCdhaTdcns = listOfMonth.Where(o => o.IS_HAS_CDHA.HasValue && o.IS_HAS_TDCN.HasValue && o.IS_HAS_TEST.HasValue).ToList();
            if (IsNotNullOrEmpty(khamTdcnCdhaTdcns))
            {
                Mrs00693RDO rdo = new Mrs00693RDO();
                rdo.TYPE_CODE = "D";
                rdo.TYPE_NAME = "d.KHAM + CĐHA + TDCN + XN";
                rdo.NOTE = "(4.5 giờ)";
                rdo.REGULATION_AMOUNT = "< 270";
                if (dicRdo.ContainsKey(rdo.TYPE_CODE))
                {
                    rdo = dicRdo[rdo.TYPE_CODE];
                }
                else
                {
                    dicRdo[rdo.TYPE_CODE] = rdo;
                }
                List<decimal> times = new List<decimal>();
                foreach (var item in khamTdcnCdhaTdcns)
                {
                    decimal t = this.GetDiffTime(item);
                    times.Add(t);
                }
                times = times.OrderBy(o => o).ToList();
                decimal total = times.Sum(s => s);
                decimal count = (decimal)times.Count;
                decimal avg = Math.Round((total / count), 0, MidpointRounding.AwayFromZero);
                this.SetAmount(i, avg, ref rdo);
                if (!dicAmount.ContainsKey(i)) dicAmount[i] = new List<decimal>();
                dicAmount[i].AddRange(times);
            }
        }

        private decimal GetDiffTime(TreatmentData item)
        {
            decimal t = 0;
            DateTime dtIntruc = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.IN_TIME).Value;
            DateTime dtFinish = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.OUT_TIME.Value).Value;
            TimeSpan ts = dtFinish - dtIntruc;
            t = (decimal)Math.Round(ts.TotalMinutes, 0, MidpointRounding.AwayFromZero);
            return t;
        }

        private void SetYear(int i, DateTime dt, ref Mrs00693RDO yearRdo)
        {
            switch (i)
            {
                case 0:
                    yearRdo.YEAR_1 = String.Format("Năm {0}", dt.Year);
                    break;
                case 1:
                    yearRdo.YEAR_2 = String.Format("Năm {0}", dt.Year);
                    break;
                case 2:
                    yearRdo.YEAR_3 = String.Format("Năm {0}", dt.Year);
                    break;
                case 3:
                    yearRdo.YEAR_4 = String.Format("Năm {0}", dt.Year);
                    break;
                case 4:
                    yearRdo.YEAR_5 = String.Format("Năm {0}", dt.Year);
                    break;
                case 5:
                    yearRdo.YEAR_6 = String.Format("Năm {0}", dt.Year);
                    break;
                case 6:
                    yearRdo.YEAR_7 = String.Format("Năm {0}", dt.Year);
                    break;
                case 7:
                    yearRdo.YEAR_8 = String.Format("Năm {0}", dt.Year);
                    break;
                case 8:
                    yearRdo.YEAR_9 = String.Format("Năm {0}", dt.Year);
                    break;
                case 9:
                    yearRdo.YEAR_10 = String.Format("Năm {0}", dt.Year);
                    break;
                case 10:
                    yearRdo.YEAR_11 = String.Format("Năm {0}", dt.Year);
                    break;
                case 11:
                    yearRdo.YEAR_12 = String.Format("Năm {0}", dt.Year);
                    break;
                default:
                    break;
            }
        }

        private void SetMonth(int i, DateTime dt, ref Mrs00693RDO monthRdo)
        {
            switch (i)
            {
                case 0:
                    monthRdo.MONTH_1 = String.Format("T.{0}", dt.Month);
                    break;
                case 1:
                    monthRdo.MONTH_2 = String.Format("T.{0}", dt.Month);
                    break;
                case 2:
                    monthRdo.MONTH_3 = String.Format("T.{0}", dt.Month);
                    break;
                case 3:
                    monthRdo.MONTH_4 = String.Format("T.{0}", dt.Month);
                    break;
                case 4:
                    monthRdo.MONTH_5 = String.Format("T.{0}", dt.Month);
                    break;
                case 5:
                    monthRdo.MONTH_6 = String.Format("T.{0}", dt.Month);
                    break;
                case 6:
                    monthRdo.MONTH_7 = String.Format("T.{0}", dt.Month);
                    break;
                case 7:
                    monthRdo.MONTH_8 = String.Format("T.{0}", dt.Month);
                    break;
                case 8:
                    monthRdo.MONTH_9 = String.Format("T.{0}", dt.Month);
                    break;
                case 9:
                    monthRdo.MONTH_10 = String.Format("T.{0}", dt.Month);
                    break;
                case 10:
                    monthRdo.MONTH_11 = String.Format("T.{0}", dt.Month);
                    break;
                case 11:
                    monthRdo.MONTH_12 = String.Format("T.{0}", dt.Month);
                    break;
                default:
                    break;
            }
        }

        private void SetAmount(int i, decimal amount, ref Mrs00693RDO rdo)
        {
            switch (i)
            {
                case 0:
                    rdo.AMOUNT_1 = amount;
                    break;
                case 1:
                    rdo.AMOUNT_2 = amount;
                    break;
                case 2:
                    rdo.AMOUNT_3 = amount;
                    break;
                case 3:
                    rdo.AMOUNT_4 = amount;
                    break;
                case 4:
                    rdo.AMOUNT_5 = amount;
                    break;
                case 5:
                    rdo.AMOUNT_6 = amount;
                    break;
                case 6:
                    rdo.AMOUNT_7 = amount;
                    break;
                case 7:
                    rdo.AMOUNT_8 = amount;
                    break;
                case 8:
                    rdo.AMOUNT_9 = amount;
                    break;
                case 9:
                    rdo.AMOUNT_10 = amount;
                    break;
                case 10:
                    rdo.AMOUNT_11 = amount;
                    break;
                case 11:
                    rdo.AMOUNT_12 = amount;
                    break;
                default:
                    break;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("YEAR", dtNow.Year);
            objectTag.AddObjectData(store, "Years", listYearRdo);
            objectTag.AddObjectData(store, "Months", listMonthRdo);
            objectTag.AddObjectData(store, "Reports", listRdo);
            objectTag.SetUserFunction(store, "FuncSameCellValue", new CustomerFuncMergeSameData());
        }
    }

    class TreatmentData
    {
        public long ID { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? OUT_DATE { get; set; }
        public long? OUT_MONTH { get; set; }
        public short? IS_HAS_TEST { get; set; }
        public short? IS_HAS_CDHA { get; set; }
        public short? IS_HAS_TDCN { get; set; }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        private string month = "";
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                string month1 = parameters[0].ToString();

                if (month == month1)
                {
                    LogSystem.Info(String.Format("TRUE: Month: {0}, Month1: {1}", month, month1));
                    return true;
                }
                else
                {
                    month = month1;
                    LogSystem.Info(String.Format("FALSE: Month: {0}, Month1: {1}", month, month1));
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }
    }
}
