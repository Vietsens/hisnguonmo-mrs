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

namespace MRS.Processor.Mrs00694
{
    public class Mrs00694Processor : AbstractProcessor
    {
        private Mrs00694Filter filter;
        private List<Mrs00694RDO> listRdo = new List<Mrs00694RDO>();
        CommonParam _param = null;
        private List<ServiceReqData> datas = new List<ServiceReqData>();

        private static List<long> ServiceReqTypeCLSs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PHCN,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN
        };

        private static List<long> ServiceReqTypePRESs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT
        };

        public Mrs00694Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00694Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            filter = (Mrs00694Filter)reportFilter;
            try
            {
                if (filter.OUT_DATE <= 0)
                {
                    BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__ThieuThongTinBatBuoc);
                    throw new Exception("OUT_DATE <= 0");
                }

                filter.OUT_DATE = (filter.OUT_DATE - (filter.OUT_DATE % 1000000));
                StringBuilder sb = new StringBuilder()
                .Append("SELECT SERV.ID, SERV.SERVICE_REQ_TYPE_ID, SERV.SERVICE_REQ_STT_ID, SERV.TREATMENT_ID, SERV.INTRUCTION_TIME, SERV.INTRUCTION_DATE, SERV.EXECUTE_ROOM_ID, SERV.EXECUTE_DEPARTMENT_ID, SERV.START_TIME, SERV.FINISH_TIME, TREA.IN_TIME, TREA.OUT_TIME, TREA.OUT_DATE, TREA.TDL_PATIENT_NAME, TREA.TDL_PATIENT_DOB, TREA.TDL_PATIENT_IS_HAS_NOT_DAY_DOB, TREA.TDL_PATIENT_GENDER_ID, TREA.TDL_PATIENT_GENDER_NAME")
                .Append(" FROM HIS_SERVICE_REQ SERV")
                .Append(" JOIN HIS_TREATMENT TREA ON SERV.TREATMENT_ID = TREA.ID")
                .Append(" WHERE TREA.IS_PAUSE = 1")
                .Append(" AND TREA.TDL_TREATMENT_TYPE_ID = 1")
                .Append(" AND SERV.IS_DELETE = 0 AND (SERV.IS_NO_EXECUTE IS NULL OR SERV.IS_NO_EXECUTE <> 1)")
                    //.Append(" AND SERV.SERVICE_REQ_STT_ID = 3")
                .Append(" AND TREA.OUT_DATE =: param1");
                if (filter.DEPARTMENT_ID.HasValue)
                {
                    sb.Append(String.Format(" AND TREA.END_DEPARTMENT_ID = {0}", filter.DEPARTMENT_ID.Value));
                }

                string sql = sb.ToString();
                LogSystem.Info(LogUtil.TraceData("Filter", filter));
                LogSystem.Info("Sql: " + sql);

                this.datas = new MOS.DAO.Sql.SqlDAO().GetSql<ServiceReqData>(sql, filter.OUT_DATE);
                LogSystem.Info("Count Data: " + (this.datas != null ? this.datas.Count : -1));

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
                if (IsNotNullOrEmpty(this.datas))
                {
                    var Groups = this.datas.GroupBy(g => g.TREATMENT_ID).ToList();
                    foreach (var g in Groups)
                    {
                        List<ServiceReqData> list = g.ToList();
                        Mrs00694RDO rdo = new Mrs00694RDO();
                        List<ServiceReqData> exams = list.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();
                        if (!IsNotNullOrEmpty(exams))
                        {
                            LogSystem.Warn("Benh Nhan Co TreatmentId Sau Khong Co Dich Vu Kham:" + list.FirstOrDefault().TREATMENT_ID);
                            continue;
                        }
                        long beginTime = 0;
                        long endTime = 0;

                        ServiceReqData first = list.FirstOrDefault();
                        rdo.DOB = first.TDL_PATIENT_DOB;
                        rdo.GENDER_NAME = first.TDL_PATIENT_GENDER_NAME;
                        rdo.PATIENT_NAME = first.TDL_PATIENT_NAME;
                        rdo.EXAM_ROOM_COUNT = exams.GroupBy(o => o.EXECUTE_ROOM_ID).Count();
                        rdo.IN_TIME = first.IN_TIME;
                        beginTime = first.IN_TIME;
                        rdo.IN_TIME_STR = this.SubTime(rdo.IN_TIME);
                        rdo.OUT_TIME = first.OUT_TIME;
                        if (first.OUT_DATE.HasValue)
                        {
                            rdo.OUT_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(first.OUT_DATE.Value);
                        }

                        ServiceReqData xamFirst = exams.Where(o => o.START_TIME.HasValue).OrderBy(s => s.START_TIME.Value).FirstOrDefault();
                        if (xamFirst != null)
                        {
                            rdo.START_EXAM_TIME = xamFirst.START_TIME.Value;
                            rdo.START_EXAM_TIME_STR = this.SubTime(rdo.START_EXAM_TIME.Value);
                        }

                        List<ServiceReqData> clss = list.Where(o => ServiceReqTypeCLSs.Contains(o.SERVICE_REQ_TYPE_ID)).ToList();
                        if (IsNotNullOrEmpty(clss))
                        {
                            rdo.HAS_CLS = "X";
                            ServiceReqData clsLast = clss.Where(o => o.FINISH_TIME.HasValue).OrderByDescending(s => s.FINISH_TIME.Value).FirstOrDefault();
                            if (clsLast != null)
                            {
                                rdo.RESULT_SUBCLINICAL_TIME = clsLast.FINISH_TIME.Value;
                                rdo.RESULT_SUBCLINICAL_TIME_STR = this.SubTime(rdo.RESULT_SUBCLINICAL_TIME.Value);
                            }
                        }

                        List<ServiceReqData> press = list.Where(o => ServiceReqTypePRESs.Contains(o.SERVICE_REQ_TYPE_ID)).ToList();
                        if (IsNotNullOrEmpty(press))
                        {
                            ServiceReqData pres = press.OrderByDescending(o => o.INTRUCTION_TIME).FirstOrDefault();
                            rdo.PRES_TIME = pres.INTRUCTION_TIME;
                            rdo.PRES_TIME_STR = this.SubTime(rdo.PRES_TIME.Value);
                            endTime = pres.INTRUCTION_TIME;

                            ServiceReqData presLast = press.Where(o => o.FINISH_TIME.HasValue).OrderByDescending(s => s.FINISH_TIME.Value).FirstOrDefault();
                            if (presLast != null)
                            {
                                rdo.EXP_TIME = presLast.FINISH_TIME.Value;
                                rdo.EXP_TIME_STR = this.SubTime(rdo.EXP_TIME.Value);
                                endTime = presLast.FINISH_TIME.Value;
                            }
                        }
                        else
                        {
                            rdo.PRES_TIME = first.OUT_TIME ?? 0;
                            rdo.PRES_TIME_STR = this.SubTime(rdo.PRES_TIME.Value);
                            endTime = first.OUT_TIME ?? 0;
                        }

                        if (beginTime > 0 && endTime > 0)
                        {
                            beginTime = (beginTime - (beginTime % 100));
                            endTime = (endTime - (endTime % 100));
                            DateTime dtBg = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(beginTime).Value;
                            DateTime dtEn = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(endTime).Value;

                            TimeSpan ts = dtEn - dtBg;
                            decimal total = (decimal)Math.Round(ts.TotalMinutes, 2, MidpointRounding.AwayFromZero);
                            rdo.TOTAL_TIME = total;
                            if (total > 0 && rdo.EXAM_ROOM_COUNT > 0)
                            {
                                rdo.AVG_TIME = Math.Round((total / rdo.EXAM_ROOM_COUNT), 2, MidpointRounding.AwayFromZero);
                            }
                        }
                        this.listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string SubTime(long time)
        {
            string rs = String.Empty;
            if (time > 0)
            {
                string t = time.ToString();
                rs = String.Format("{0}:{1}", t.Substring(8, 2), t.Substring(10, 2));
            }
            return rs;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (filter.DEPARTMENT_ID.HasValue)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            }
            dicSingleTag.Add("OUT_DATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.OUT_DATE));
            objectTag.AddObjectData(store, "Reports", listRdo);
        }
    }

    class ServiceReqData : TreatmentData
    {
        public long ID { get; set; }
        public long SERVICE_REQ_TYPE_ID { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long INTRUCTION_DATE { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public long EXECUTE_DEPARTMENT_ID { get; set; }
        public long? START_TIME { get; set; }
        public long? FINISH_TIME { get; set; }
    }

    class TreatmentData
    {
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? OUT_DATE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public short? TDL_PATIENT_IS_HAS_NOT_DAY_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
    }
}
