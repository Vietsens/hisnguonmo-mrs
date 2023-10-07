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
using MRS.Proccessor.Mrs00620;
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
using MOS.MANAGER.HisSereServ;
using HIS.Common.Treatment;

namespace MRS.Processor.Mrs00620
{
    public class Mrs00620Processor : AbstractProcessor
    {
        private List<Mrs00620Service> ListService = new List<Mrs00620Service>();
        private List<Mrs00620RDO> ListRdo = new List<Mrs00620RDO>();


        Mrs00620Filter filter = null;

        string thisReportTypeCode = "";

        private Dictionary<long, int> dicCountBed = new Dictionary<long, int>();
        private List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        private List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatienttypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT> CLNDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();

        long? PREVIOUS_TIME_FROM = null;
        long? PREVIOUS_TIME_TO = null;
        long? CURENT_TIME_FROM = null;
        long? CURENT_TIME_TO = null;


        public Mrs00620Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00620Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00620Filter)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00620: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));

            try
            {
                //Khoa Lâm sàng
                CLNDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { IS_CLINICAL = true });
                //Danh sach giuong
                HisBedFilterQuery HisBedfilter = new HisBedFilterQuery();
                HisBedfilter.IS_ACTIVE = 1;
                var listHisBed = new HisBedManager().Get(HisBedfilter);

                //Danh sach buong
                HisBedRoomViewFilterQuery HisBedRoomfilter = new HisBedRoomViewFilterQuery();
                HisBedRoomfilter.IS_ACTIVE = 1;
                var listHisBedRoom = new HisBedRoomManager().GetView(HisBedRoomfilter);
                if (IsNotNullOrEmpty(listHisBed) && IsNotNullOrEmpty(listHisBedRoom))
                {
                    dicCountBed = listHisBedRoom.GroupBy(o => o.DEPARTMENT_ID).ToDictionary(p => p.Key, p => listHisBed.Where(q => p.ToList().Exists(r => r.ID == q.BED_ROOM_ID)).Count());
                }

                DateTime Time = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(filter.MONTH);
                int year, month;
                month = Time.Month;
                year = Time.Year;
                CURENT_TIME_FROM = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(year, month, 1, 0, 0, 0));
                CURENT_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59));
                month = month - 1;
                if (month == 0)
                {
                    year = year - 1;
                    month = 12;
                }

                PREVIOUS_TIME_FROM = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(year, month, 1, 0, 0, 0));
                PREVIOUS_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59));

                HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IS_OUT = false;
                filterTreatment.IN_TIME_TO = CURENT_TIME_TO;
                var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatment);
                filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IN_TIME_TO = CURENT_TIME_TO;
                filterTreatment.IS_OUT = true;
                filterTreatment.OUT_TIME_FROM = PREVIOUS_TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatmentOut);

                if (filter.BRANCH_ID != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }

                //Inventec.Common.Logging.LogSystem.Info("listHisTreatment" + listHisTreatment.Count);
                List<long> treatmentIds = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                //
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = limit;
                        HisDepartmentTranfilter.DEPARTMENT_IN_TIME_TO = CURENT_TIME_TO;// vao khoa truoc Time_to
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub Get null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);
                    }
                    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery patientTyeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patientTyeAlterFilter.TREATMENT_IDs = limit;
                        patientTyeAlterFilter.ORDER_FIELD = "ID";
                        patientTyeAlterFilter.ORDER_DIRECTION = "ASC";

                        var listPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(patientTyeAlterFilter);
                        if (listPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                    }
                }

                lastPatienttypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                //BC hiển thị BN nội trú, ngoại trú
                listHisDepartmentTran = listHisDepartmentTran.Where(o => CLNDepartment.Exists(q => q.ID == o.DEPARTMENT_ID) &&
                    (HasTreatIn(o, listHisPatientTypeAlter))).ToList();

                //3. tổng số lần XN: số dv xét nghiệm có thời gian chỉ định trong thời gian lấy báo cáo (intruction_time)
                //4. tổng số lần CĐHA: số dv HA,SA,NS,CN có thời gian chỉ định trong thời gian lấy báo cáo (intruction_time)
                //5. tổng số lần PTT: số dv Pt,TT có thời gian chỉ định trong thời gian lấy báo cáo
                // lấy các dịch vụ chỉ định trong tháng
                HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                HisSereServfilter.TDL_INTRUCTION_TIME_FROM = PREVIOUS_TIME_FROM;
                HisSereServfilter.TDL_INTRUCTION_TIME_TO = CURENT_TIME_TO;
                HisSereServfilter.TDL_REQUEST_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                HisSereServfilter.TDL_SERVICE_TYPE_IDs = new List<long>() 
                { 
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
                };
                listHisSereServ = new HisSereServManager().Get(HisSereServfilter);
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
            bool result = false;
            try
            {

                if (filter.DEPARTMENT_ID != null)
                {
                    CLNDepartment = CLNDepartment.Where(o => o.ID == filter.DEPARTMENT_ID).ToList();
                }
                foreach (var item in CLNDepartment)
                {
                    var listDepartmentTranSub = listHisDepartmentTran.Where(o => o.DEPARTMENT_ID == item.ID).ToList();
                    var listHisSereServSub = listHisSereServ.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == item.ID).ToList();
                    Mrs00620RDO rdo = new Mrs00620RDO();
                    rdo.DEPARTMENT_ID = item.ID;
                    rdo.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                    rdo.NUM_BED = dicCountBed.ContainsKey(item.ID) ? dicCountBed[item.ID] : 0;
                    rdo.DAY = (long)listHisTreatment.Where(o => o.END_DEPARTMENT_ID == item.ID && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.OUT_TIME >= CURENT_TIME_FROM && o.OUT_TIME < CURENT_TIME_TO).Sum(s => HIS.Common.Treatment.Calculation.DayOfTreatment(s.CLINICAL_IN_TIME, s.OUT_TIME, s.TREATMENT_END_TYPE_ID, s.TREATMENT_RESULT_ID, s.TDL_HEIN_CARD_NUMBER != null ? HIS.Common.Treatment.PatientTypeEnum.TYPE.BHYT : HIS.Common.Treatment.PatientTypeEnum.TYPE.THU_PHI) ?? 0);
                    ProcessAmount(rdo.PREVIOUS_INFO, PREVIOUS_TIME_FROM, PREVIOUS_TIME_TO, listDepartmentTranSub, listHisSereServSub);
                    ProcessAmount(rdo.CURENT_INFO, CURENT_TIME_FROM, CURENT_TIME_TO, listDepartmentTranSub, listHisSereServSub);
                    ProccessServ(listHisSereServSub);
                    ListRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProccessServ(List<HIS_SERE_SERV> listHisSereServSub)
        {
            try
            {
                var groupBySV = listHisSereServSub.GroupBy(o => o.SERVICE_ID).ToList();
                foreach (var item in groupBySV)
                {
                    Mrs00620Service rdo = new Mrs00620Service();
                    rdo.DEPARTMENT_ID = item.First().TDL_REQUEST_DEPARTMENT_ID;
                    rdo.SERVICE_ID = item.Key;
                    rdo.SERVICE_CODE = item.First().TDL_SERVICE_CODE;
                    rdo.SERVICE_NAME = item.First().TDL_SERVICE_NAME;
                    rdo.PREVIOUS_AMOUNT = item.Where(o => o.TDL_INTRUCTION_TIME >= PREVIOUS_TIME_FROM && o.TDL_INTRUCTION_TIME < PREVIOUS_TIME_TO).Sum(s => s.AMOUNT);
                    rdo.CURENT_AMOUNT = item.Where(o => o.TDL_INTRUCTION_TIME >= CURENT_TIME_FROM && o.TDL_INTRUCTION_TIME < CURENT_TIME_TO).Sum(s => s.AMOUNT);
                    rdo.COUNT_TREAT = item.Where(o => o.TDL_INTRUCTION_TIME >= CURENT_TIME_FROM && o.TDL_INTRUCTION_TIME < CURENT_TIME_TO).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                    ListService.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessAmount(AMOUNT_INFO r, long? TIME_FROM, long? TIME_TO, List<HIS_DEPARTMENT_TRAN> listDepartmentTranSub,List<HIS_SERE_SERV> listHisSereServSub)
        {
            try
            {
                listHisSereServSub = listHisSereServSub.Where(o => o.TDL_INTRUCTION_TIME >= TIME_FROM && o.TDL_INTRUCTION_TIME < TIME_TO).ToList();

                r.COUNT_BEGIN = listDepartmentTranSub.Count(o => o.DEPARTMENT_IN_TIME < TIME_FROM && listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && (p.OUT_TIME ?? 99999999999999) >= TIME_FROM));
                r.COUNT_IMP = listDepartmentTranSub.Count(o => o.DEPARTMENT_IN_TIME >= TIME_FROM && o.DEPARTMENT_IN_TIME < TIME_TO);
                r.COUNT_EXP = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && (p.OUT_TIME ?? 99999999999999) < TIME_TO && p.END_DEPARTMENT_ID ==o.DEPARTMENT_ID));
                r.COUNT_EXP_BHYT = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT));
                r.COUNT_END = listDepartmentTranSub.Count(o => o.DEPARTMENT_IN_TIME < TIME_TO && listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && (p.OUT_TIME ?? 99999999999999) >= TIME_TO));
                r.COUNT_EXP_DO = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO));
                r.COUNT_EXP_KHOI = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI));
                r.COUNT_EXP_KTD = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD));
                r.COUNT_EXP_NANG = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG));
                r.COUNT_EXP_CV = listDepartmentTranSub.Count(o => listHisTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.OUT_TIME >= TIME_FROM && p.OUT_TIME < TIME_TO && p.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN));
                r.AMOUNT_TEST = listHisSereServSub.Count(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN);
                r.AMOUNT_CDHA = listHisSereServSub.Count(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN);
                r.AMOUNT_PTTT = listHisSereServSub.Count(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

      

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (filter.MONTH > 0)
            {
                dicSingleTag.Add("MONTH", filter.MONTH.ToString().Substring(4, 2));
                dicSingleTag.Add("YEAR", filter.MONTH.ToString().Substring(0, 4));
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Service", ListService);
            objectTag.AddRelationship(store, "Report", "Service", "DEPARTMENT_ID", "DEPARTMENT_ID");
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return listHisDepartmentTran.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }

        //Co dieu tri noi tru
        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }

}
