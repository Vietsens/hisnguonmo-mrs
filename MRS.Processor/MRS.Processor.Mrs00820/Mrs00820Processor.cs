using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00820;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisServiceReq;
using System.Data;
using System.Threading;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00820
{
    public class Mrs00820Processor : AbstractProcessor
    {
        private List<Mrs00820RDO> ListRdoDetail = new List<Mrs00820RDO>();
        private List<Mrs00820RDO> ListRdo = new List<Mrs00820RDO>();

        Mrs00820Filter filter = null;

        string thisReportTypeCode = "";

        List<TREATMENT_BED_ROOM> treatmentBedRooms = new List<TREATMENT_BED_ROOM>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        Dictionary<long, string> dicRoomCode = new Dictionary<long, string>();

        public Mrs00820Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00820Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00820Filter)this.reportFilter; //Khoa Lâm sàng
            Inventec.Common.Logging.LogSystem.Info("listDepartment get from db:");
            HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
            listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { IS_CLINICAL = true });

            //dicRoomCode 
            dicRoomCode = HisRoomCFG.HisRooms.ToDictionary(o => o.ID, p => p.ROOM_CODE);


            return GetDataOld();
        }

        private bool GetDataOld()
        {
            var result = true;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00820: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));

            try
            {
                treatmentBedRooms = new ManagerSql().GetTreatment(filter);
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
            return ProcessDataOld();
        }
        private bool ProcessDataOld()
        {
            bool result = false;
            try
            {
                //sửa lỗi thời gian ra viện khác thời gian ra buồng cuối cùng
                //sửa lại đối với buồng cuối cùng có thời gian ra viện thì sẽ áp dụng theo thời gian ra viện
                FixedRemoveTime();

                var groupByTreatment = treatmentBedRooms.GroupBy(g => g.TREATMENT_ID).ToList();
                foreach (var treatmentSub in groupByTreatment)
                {
                    //nếu thời gian ra viện trước khoảng báo cáo thì bỏ đi
                    if (treatmentSub.First().OUT_TIME > 0 && treatmentSub.First().OUT_TIME < filter.TIME_FROM)
                    {
                        continue;
                    }
                    var minAddTime = treatmentSub.Min(m => m.ADD_TIME);
                    var MaxRemoveTime = treatmentSub.Max(m => m.REMOVE_TIME);
                    var groupByDepartment = treatmentSub.GroupBy(g => g.DEPARTMENT_ID).ToList();
                    foreach (var treaDepa in groupByDepartment)
                    {
                        Mrs00820RDO rdo = new Mrs00820RDO();
                        rdo.TREATMENT_ID = treaDepa.First().TREATMENT_ID;
                        rdo.TREATMENT_CODE = treaDepa.First().TREATMENT_CODE;
                        rdo.IN_TIME = treaDepa.First().IN_TIME;
                        rdo.OUT_TIME = treaDepa.First().OUT_TIME;
                        rdo.TDL_TREATMENT_TYPE_ID = treaDepa.First().TDL_TREATMENT_TYPE_ID;
                        if (rdo.IN_TIME >= filter.TIME_FROM && rdo.IN_TIME <= filter.TIME_TO)
                        {
                            rdo.COUNT_IN_TIME = 1;
                        }
                        if (rdo.OUT_TIME >= filter.TIME_FROM && rdo.OUT_TIME <= filter.TIME_TO && treaDepa.First().IS_PAUSE == 1)
                        {
                            rdo.COUNT_OUT_TIME = 1;
                        }
                        rdo.PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == treaDepa.First().TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                        rdo.PATIENT_CLASSIFY_CODE = treaDepa.First().PATIENT_CLASSIFY_CODE;
                        rdo.TREATMENT_END_TYPE_CODE = treaDepa.First().TREATMENT_END_TYPE_CODE;
                        rdo.TREATMENT_RESULT_CODE = treaDepa.First().TREATMENT_RESULT_CODE;
                        if (treaDepa.First().TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                        {
                            rdo.COUNT_ALL_DIE = 1;
                            if (treaDepa.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.COUNT_DIE_FEMALE = 1;
                            }
                            if (treaDepa.First().DEATH_WITHIN_NAME != null)
                            {
                                if (treaDepa.First().DEATH_WITHIN_NAME.Contains("24"))
                                {
                                    rdo.COUNT_DIE_IN_24H = 1;
                                }
                            }
                        }
                        if (treaDepa.First().TREATMENT_END_TYPE_ID == null)
                        {
                            rdo.COUNT_IN = 1;
                        }
                        if (treaDepa.First().IS_PAUSE == 1)
                        {
                            rdo.TREATMENT_DAY_COUNT_RV = treaDepa.First().TREATMENT_DAY_COUNT ?? 0;
                        }
                        rdo.TREATMENT_DAY_COUNT_ALl = treaDepa.First().TREATMENT_DAY_COUNT ?? 0;
                        if (treaDepa.First().TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.COUNT_NT = 1;
                            if (treaDepa.First().IS_EMERGENCY == 1)
                            {
                                rdo.COUNT_NT_EMERGENCY = 1;
                            }
                            long age = Inventec.Common.DateTime.Calculation.Age(treaDepa.First().TDL_PATIENT_DOB);
                            if (age < 15)
                            {
                                rdo.COUNT_NT_15_AGE = 1;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(treaDepa.First().TDL_HEIN_CARD_NUMBER))
                        {
                            rdo.HEAD_CARD = treaDepa.First().TDL_HEIN_CARD_NUMBER.Substring(0, 2);

                        }
                        if (treatmentSub.First().TDL_PATIENT_NATIONAL_NAME != null)
                        {
                            if (treatmentSub.First().TDL_PATIENT_NATIONAL_NAME.ToLower() != "việt nam")
                            {
                                rdo.COUNT_NN = 1;
                            }
                        }
                        rdo.DEPARTMENT_ID = treaDepa.First().DEPARTMENT_ID;
                        var department = listDepartment.FirstOrDefault(o => o.ID == treaDepa.First().DEPARTMENT_ID);
                        if (department != null)
                        {
                            rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                            rdo.ORDER = department.NUM_ORDER ?? 999;
                            rdo.REALITY_PATIENT_BED = department.REALITY_PATIENT_COUNT ?? 0;
                            rdo.THEORY_PATIENT_BED = department.THEORY_PATIENT_COUNT ?? 0;
                        }
                        rdo.POINT_DAY = treaDepa.Sum(s => DateDiff(s.ADD_TIME ?? 0, s.REMOVE_TIME ?? 0));
                        var minAddTimetreaDepa = treaDepa.Min(m => m.ADD_TIME);
                        var MaxRemoveTimetreaDepa = treaDepa.Max(m => m.REMOVE_TIME);
                        rdo.COUNT_ALL = 1;
                        if (treaDepa.First().TDL_HEIN_CARD_NUMBER != null)
                        {
                            rdo.COUNT_BHYT = 1;
                        }
                        if (treaDepa.Count(c => c.TYPE == 1 || c.TYPE == 4) > 0)
                        {
                            rdo.COUNT_OLD = 1;
                            if (treaDepa.First().TDL_HEIN_CARD_NUMBER != null)
                            {
                                rdo.COUNT_OLD_BHYT = 1;
                            }
                        }
                        else
                        {
                            if (minAddTimetreaDepa == minAddTime)
                            {
                                if (treaDepa.First().PREVIOUS_DEPARTMENT_ID == null)
                                {
                                    rdo.COUNT_NEW = 1;
                                    if (treaDepa.First().IS_EMERGENCY == 1)
                                    {
                                        rdo.COUNT_NEW_EMERGENCY = 1;
                                    }
                                    if (treaDepa.First().TDL_HEIN_CARD_NUMBER != null)
                                    {
                                        rdo.COUNT_NEW_BHYT = 1;
                                    }
                                    if (!string.IsNullOrWhiteSpace(filter.EXECUTE_ROOM_CODE__PKKs) && ("," + filter.EXECUTE_ROOM_CODE__PKKs + ",").Contains("," + (treaDepa.First().IN_ROOM_CODE ?? "") + ","))
                                    {
                                        rdo.COUNT_FROM_PKK = 1;
                                    }
                                    if (!string.IsNullOrWhiteSpace(filter.EXECUTE_ROOM_CODE__KCCs) && ("," + filter.EXECUTE_ROOM_CODE__KCCs + ",").Contains("," + (treaDepa.First().IN_ROOM_CODE ?? "") + ","))
                                    {
                                        rdo.COUNT_FROM_KCC = 1;
                                    }
                                }
                                else if (treaDepa.First().PREVIOUS_DEPARTMENT_ID == treaDepa.First().DEPARTMENT_ID)
                                {
                                    rdo.COUNT_OLD = 1;
                                    if (treaDepa.First().TDL_HEIN_CARD_NUMBER != null)
                                    {
                                        rdo.COUNT_OLD_BHYT = 1;
                                    }
                                }
                                else
                                {
                                    rdo.COUNT_CHDP_IMP = 1;
                                }
                            }
                            else
                            {
                                rdo.COUNT_CHDP_IMP = 1;
                            }
                        }
                        if (treaDepa.Count(c => c.TYPE == 3 || c.TYPE == 4) > 0)
                        {
                            rdo.COUNT_END = 1;
                            if (treaDepa.First().TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_ALL_BHYT = 1;
                            }
                            else if (treaDepa.First().TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            {
                                rdo.COUNT_ALL_VP = 1;
                            }
                            else
                            {
                                rdo.COUNT_ALL_XHH = 1;
                            }
                        }
                        else
                        {
                            if (MaxRemoveTimetreaDepa == MaxRemoveTime)
                            {
                                if (treaDepa.First().NEXT_DEPARTMENT_ID == null)
                                {
                                    rdo.COUNT_EXP = 1;
                                    if (treaDepa.First().TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                                    {
                                        rdo.COUNT_OUT_CV = 1;//chuyển viện
                                    }
                                    if (treaDepa.First().TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET || treaDepa.First().TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                                    {
                                        rdo.COUNT_DIE = 1;
                                    }

                                    if (treaDepa.First().TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN && treaDepa.First().TREATMENT_RESULT_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                                    {
                                        rdo.COUNT_OUT_XINRAVIEN = 1;
                                    }

                                    if (treaDepa.First().TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__KHAC)
                                    {
                                        rdo.COUNT_OUT_KHAC = 1;
                                    }

                                }
                                else if (treaDepa.First().NEXT_DEPARTMENT_ID == treaDepa.First().DEPARTMENT_ID)
                                {
                                    rdo.COUNT_END = 1;
                                    if (treaDepa.First().TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        rdo.COUNT_ALL_BHYT = 1;
                                    }
                                    else if (treaDepa.First().TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                                    {
                                        rdo.COUNT_ALL_VP = 1;
                                    }
                                    else
                                    {
                                        rdo.COUNT_ALL_XHH = 1;
                                    }
                                }
                                else
                                {
                                    rdo.COUNT_MOVE = 1;
                                }
                            }
                            else
                            {
                                rdo.COUNT_MOVE = 1;
                            }
                        }
                        ListRdoDetail.Add(rdo);
                    }
                }
                ListRdo = GroupByDepartment(ListRdoDetail);
                ListRdo = CreateFullDepartment(ListRdo);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00820RDO>();
                result = false;
            }
            return result;
        }

        private void FixedRemoveTime()
        {
            foreach (var item in treatmentBedRooms)
            {
                if (item.ID == item.LAST_ID)
                {
                    if (item.OUT_TIME < item.ADD_TIME || item.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        continue;
                    }
                    if (item.OUT_TIME >= filter.TIME_FROM && item.OUT_TIME <= filter.TIME_TO)//ra viện trong khoảng
                    {
                        //sửa lại thời gian ra buồng và loại
                        item.REMOVE_TIME = item.OUT_TIME;
                        if (item.ADD_TIME < filter.TIME_FROM)
                        {
                            item.TYPE = 1;
                        }
                        else if (item.ADD_TIME >= filter.TIME_FROM && item.ADD_TIME <= filter.TIME_TO)
                        {
                            item.TYPE = 2;
                        }
                    }
                    else if (item.OUT_TIME > filter.TIME_TO)//ra viện sau khoảng
                    {
                        //sửa lại thời gian ra buồng và loại
                        item.REMOVE_TIME = filter.TIME_TO + 1;
                        if (item.ADD_TIME < filter.TIME_FROM)
                        {
                            item.TYPE = 4;
                        }
                        else if (item.ADD_TIME >= filter.TIME_FROM && item.ADD_TIME <= filter.TIME_TO)
                        {
                            item.TYPE = 3;
                        }
                    }
                }
            }
        }
        private decimal DateDiff(long intime, long outtime)
        {

            decimal result = 0;
            try
            {
                if (outtime < intime) return 0;
                if (outtime > filter.TIME_TO)
                {
                    outtime = filter.TIME_TO;
                }
                if (intime < filter.TIME_FROM)
                {
                    intime = filter.TIME_FROM;
                }
                DateTime InTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(intime);
                DateTime OutTime = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(outtime);
                TimeSpan sp = OutTime - InTime;
                result = (decimal)sp.TotalDays;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }


        private List<Mrs00820RDO> GroupByDepartment(List<Mrs00820RDO> ListRdo)
        {
            string errorField = "";
            List<Mrs00820RDO> result = new List<Mrs00820RDO>();
            try
            {
                var group = ListRdo.GroupBy(o => new { o.DEPARTMENT_NAME }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00820RDO rdo;
                List<Mrs00820RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00820RDO>();
                foreach (var item in group)
                {
                    if (filter.DEPARTMENT_ID != null)
                    {
                        if (item.First().DEPARTMENT_NAME != (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME)
                        {
                            continue;
                        }
                    }
                    rdo = new Mrs00820RDO();
                    listSub = item.ToList<Mrs00820RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT") || field.Name.Contains("DAY"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (hide == false) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00820RDO>();
            }
            return result;
        }


        private List<Mrs00820RDO> CreateFullDepartment(List<Mrs00820RDO> ListRdo)
        {
            List<Mrs00820RDO> result = ListRdo;
            try
            {

                List<long> hasData = ListRdo.Select(s => s.DEPARTMENT_ID).ToList();
                var lstDepartment = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == 1
                    && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && !hasData.Contains(o.ID)
                    ).OrderBy(p => !p.NUM_ORDER.HasValue).ThenBy(p => p.NUM_ORDER).ToList();
                foreach (var item in lstDepartment)
                {
                    result.Add(new Mrs00820RDO()
                    {
                        DEPARTMENT_NAME = item.DEPARTMENT_NAME,
                        ORDER = item.NUM_ORDER,
                        DEPARTMENT_CODE = item.DEPARTMENT_CODE,
                        REALITY_PATIENT_BED = item.REALITY_PATIENT_COUNT ?? 0,
                        THEORY_PATIENT_BED = item.THEORY_PATIENT_COUNT ?? 0,
                    });
                }

                result = result.OrderBy(o => o.ORDER ?? 9999).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<Mrs00820RDO>();
            }
            return result;
        }

        private Mrs00820RDO IsMeaningful(List<Mrs00820RDO> listSub, PropertyInfo field)
        {
            string errorField = "";
            Mrs00820RDO result = new Mrs00820RDO();
            try
            {
                result = listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00820RDO();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new Mrs00820RDO();
            }
            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (this.filter.DEPARTMENT_ID != null)
            {
                ListRdo = ListRdo.Where(o => o.DEPARTMENT_ID == this.filter.DEPARTMENT_ID).ToList();
            }
            if (this.filter.BRANCH_ID != null)
            {
                ListRdo = ListRdo.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.BRANCH_ID == filter.BRANCH_ID && p.ID == o.DEPARTMENT_ID)).ToList();
            }

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));

            dicSingleTag.Add("DATE_TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("DATE_TIME_TO", filter.TIME_TO);

            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);

            objectTag.AddObjectData(store, "Report", ListRdo);

            //Set thêm dữ liệu khác
            for (int i = 0; i < 15; i++)
            {
                 var dataObject = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, i + 1)) ?? new List<DataTable>();
                objectTag.AddObjectData(store, "Report" + i, dataObject.Count > 0 ? dataObject[0] : new DataTable());
                objectTag.AddObjectData(store, "Parent" + i, dataObject.Count > 1 ? dataObject[1] : new DataTable());
                objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
            }

        }
    }

}
