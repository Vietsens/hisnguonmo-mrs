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
using MRS.Proccessor.Mrs00555;
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

namespace MRS.Processor.Mrs00555
{
    public class Mrs00555Processor : AbstractProcessor
    {
        private List<Mrs00555RDO> ListRdoDetail = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> ListRdoRoom = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> ListRdo = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> DepartmentDistinct = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> DepartmentMergeTreat = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> ListRdoFemale = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> ListRdoSum = new List<Mrs00555RDO>();
        private List<Mrs00555RDO> ListRdoCareer = new List<Mrs00555RDO>();

        Mrs00555Filter filter = null;

        string thisReportTypeCode = "";
        
        private Dictionary<long, int> dicCountBed = new Dictionary<long, int>();
        List<TREATMENT> listHisTreatment = new List<TREATMENT>();
        List<DEPARTMENT_TRAN> listHisDepartmentTranAll = new List<DEPARTMENT_TRAN>();
        List<PATIENT_TYPE_ALTER> listHisPatientTypeAlterTreat = new List<PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT_4> listHisTreatmentExam = new List<V_HIS_TREATMENT_4>();
        List<V_HIS_TREATMENT_4> listHisTreatmentClinicalIn = new List<V_HIS_TREATMENT_4>();
        List<HIS_SERVICE_REQ> ListExamServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE> ListBedServiceReq = new List<HIS_SERVICE>();
        List<HIS_DEPARTMENT> CLNDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_DEPARTMENT> KCCDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_DEPARTMENT> KGMHSDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_TREATMENT> listHisTreatmentAll = new List<HIS_TREATMENT>();
        Dictionary<long, HIS_TREATMENT> dicTreatmentAll = new Dictionary<long, HIS_TREATMENT>();
        List<TREATMENT_BED_ROOM> treatmentBedRooms = new List<TREATMENT_BED_ROOM>();
        List<DEPA_COUNT_BED> listDepaCountBed = new List<DEPA_COUNT_BED>();
        Dictionary<string, object> dicMini = new Dictionary<string, object>();
        string DEPARTMENT_CODE__OUTPATIENTs = "DEPARTMENT_CODE__OUTPATIENTs";
        const int TimeForOnePage = 5000;//milisencond
        public Mrs00555Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00555Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00555Filter)this.reportFilter; //Khoa Lâm sàng
            CLNDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { IS_CLINICAL = true });
            Inventec.Common.Logging.LogSystem.Info("CLNDepartment get from db:");
            this.GetDicMini(ref dicMini);
            //if (dicMini != null && dicMini.ContainsKey(listDepartmentCodeKey) && dicMini[listDepartmentCodeKey] != null)
            //{
            //    string departmentCodes = dicMini[listDepartmentCodeKey].ToString();
            //    CLNDepartment = CLNDepartment.Where(o => ("," + departmentCodes + ",").IndexOf("," + o.DEPARTMENT_CODE + ",") >= 0).ToList();
            //}
            if (dicMini != null && dicMini.ContainsKey(DEPARTMENT_CODE__OUTPATIENTs) && dicMini[DEPARTMENT_CODE__OUTPATIENTs] != null)
            {
                string departmentCodes = dicMini[DEPARTMENT_CODE__OUTPATIENTs].ToString();
                filter.DEPARTMENT_CODE__OUTPATIENTs = departmentCodes;
            }
            return GetDataOld();



        }

        private bool GetDataOld()
        {
            var result = true;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00555: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));

            try
            {

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

                listHisTreatment = new ManagerSql().GetTreatment(filter);
                
                if (this.dicDataFilter.ContainsKey("KEY_DETAIL_TREATMENT") && this.dicDataFilter["KEY_DETAIL_TREATMENT"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_DETAIL_TREATMENT"].ToString()))
                {
                    string keyDetailTreatment  = this.dicDataFilter["KEY_DETAIL_TREATMENT"].ToString();
                    if (keyDetailTreatment.ToLower() == "true")
                    {
                        filter.IS_DETAIL_TREATMENT = true;
                    }
                }
                if (filter.IS_DETAIL_TREATMENT == true)
                {
                    listHisTreatmentAll = new ManagerSql().GetTreatmentAll(filter);
                    dicTreatmentAll = listHisTreatmentAll.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());
                }

                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    treatmentBedRooms = new ManagerSql().GetTreatmentBedRoom(filter);
                }

                if (filter.IS_COUNT_BED_LOG == true)
                {
                    listDepaCountBed = new ManagerSql().GetDepaCountBed(filter)??new List<DEPA_COUNT_BED>();
                }
                listHisDepartmentTranAll = new ManagerSql().GetDepartmentTran(filter);
                if (listHisDepartmentTranAll == null)
                    Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranAll Get null");

                listHisPatientTypeAlterTreat = new ManagerSql().GetPatientTypeAlter(filter);
                if (listHisPatientTypeAlterTreat == null)
                    Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterTreat Get null");


                if (filter.BRANCH_ID != null)
                {
                    //listHisTreatment = listHisTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                    listHisTreatmentExam = listHisTreatmentExam.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }

                listHisTreatmentExam = listHisTreatmentExam.Where(o => ListExamServiceReq.Exists(p => p.TREATMENT_ID == o.ID)).ToList();
                ListExamServiceReq = ListExamServiceReq.Where(o => listHisTreatmentExam.Exists(p => p.ID == o.TREATMENT_ID)).ToList();

                ListBedServiceReq = new HisServiceManager().Get(new HisServiceFilterQuery() { SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G });

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
                listHisTreatment = listHisTreatment.GroupBy(o => o.ID).Select(p => p.First()).ToList();


                //
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;
                    while (listHisTreatment.Count - skip > 0)
                    {
                        var listHisTreatmentLocal = listHisTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listHisDepartmentTranLocal = listHisDepartmentTranAll.Where(o=>o.DEPARTMENT_IN_TIME>0).Where(o => listHisTreatmentLocal.Exists(p => p.ID == o.TREATMENT_ID)).ToList();

                        var listHisPatientTypeAlterLocal = listHisPatientTypeAlterTreat.Where(o => listHisTreatmentLocal.Exists(p => p.ID == o.TREATMENT_ID)).ToList();

                        var lastPatienttypeAlter = listHisPatientTypeAlterLocal.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                        //Khoa kham cap cuu khong dieu tri noi tru, neu dieu tri noi tru thi coi nhu cua khoa tiep theo
                        if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                        {
                            List<string> KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                            KCCDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)).ToList();
                            if (KCCDepartments != null)
                            {
                                foreach (var item in listHisDepartmentTranLocal.OrderBy(p => p.ID).Where(o => KCCDepartments.Exists(p => p.ID == o.DEPARTMENT_ID) && listHisPatientTypeAlterLocal.Exists(p => p.DEPARTMENT_TRAN_ID == o.ID && (p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))).ToList())
                                {

                                    var nextDepartmentTran = NextDepartment(item, listHisDepartmentTranLocal);
                                    if (nextDepartmentTran.ID > 0 && KCCDepartments.Exists(p => p.ID == nextDepartmentTran.DEPARTMENT_ID))
                                    {
                                        //listHisDepartmentTranAll.Remove(nextDepartmentTran);
                                        nextDepartmentTran = NextDepartment(nextDepartmentTran, listHisDepartmentTranLocal);
                                    }
                                    if (nextDepartmentTran.ID > 0)
                                    {

                                        var patientTypeAlter = listHisPatientTypeAlterLocal.OrderBy(p => p.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID && (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU));
                                        if (patientTypeAlter != null && item.ID == patientTypeAlter.DEPARTMENT_TRAN_ID)
                                        {

                                            var treatment = listHisTreatmentLocal.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                                            if (treatment != null)
                                            {
                                                if (treatment.CLINICAL_IN_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME)
                                                {
                                                    treatment.CLINICAL_IN_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME;
                                                }
                                            }
                                            if (patientTypeAlter.DEPARTMENT_TRAN_ID < nextDepartmentTran.ID)
                                            {
                                                patientTypeAlter.DEPARTMENT_TRAN_ID = nextDepartmentTran.ID;
                                            }
                                            if (patientTypeAlter.LOG_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME)
                                            {
                                                patientTypeAlter.LOG_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0;
                                            }
                                        }
                                        listHisDepartmentTranLocal.Remove(item);
                                    }
                                    else
                                    {

                                        listHisDepartmentTranLocal = listHisDepartmentTranLocal.Where(o => o.TREATMENT_ID != item.TREATMENT_ID).ToList();
                                    }
                                }
                            }
                        }
                        if (HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION != null && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION.Count > 0)
                        {
                            List<long> listNoTreatment = HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION;
                            foreach (var item in listHisDepartmentTranLocal.OrderBy(p => p.ID).Where(o => listNoTreatment.Contains(o.DEPARTMENT_ID) &&
                            (HasTreatIn(o, listHisPatientTypeAlterLocal)
                            || HasTreatOut(o, listHisPatientTypeAlterLocal))).ToList())
                            {
                                var previousDepartmentTran = listHisDepartmentTranLocal.FirstOrDefault(o => o.ID == item.PREVIOUS_ID);
                                var nextDepartmentTran = NextDepartment(item, listHisDepartmentTranLocal);
                                if (nextDepartmentTran != null && nextDepartmentTran.ID > 0)
                                {
                                    if (previousDepartmentTran != null && previousDepartmentTran.ID > 0)
                                    {
                                        nextDepartmentTran.PREVIOUS_ID = previousDepartmentTran.ID;
                                    }

                                    nextDepartmentTran.DEPARTMENT_IN_TIME = item.DEPARTMENT_IN_TIME;
                                    var patientTypeAlter = listHisPatientTypeAlterLocal.OrderBy(p => p.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID && (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU));
                                    if (patientTypeAlter != null && item.ID == patientTypeAlter.DEPARTMENT_TRAN_ID)
                                    {

                                        if (patientTypeAlter.DEPARTMENT_TRAN_ID < nextDepartmentTran.ID)
                                        {
                                            patientTypeAlter.DEPARTMENT_TRAN_ID = nextDepartmentTran.ID;
                                        }
                                    }
                                }
                                listHisDepartmentTranLocal.Remove(item);


                            }
                        }

                        var listHisDepartmentTran = listHisDepartmentTranLocal.Where(o => CLNDepartment.Exists(q => q.ID == o.DEPARTMENT_ID)).ToList();

                        if (filter.DEPARTMENT_ID != null)
                        {
                            listHisDepartmentTran = listHisDepartmentTran.Where(o => o.DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                        }

                        //chi lay cac vao khoa co thoi gian ra khoa sau time_from
                        listHisDepartmentTran = listHisDepartmentTran.Where(o => (NextDepartment(o, listHisDepartmentTranLocal).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.TIME_FROM).ToList();
                        long dateTo = filter.TIME_TO - filter.TIME_TO % 1000000;
                        List<long> treatmentids = new List<long>();
                        foreach (var item in listHisDepartmentTran)
                        {
                            if (!item.DEPARTMENT_IN_TIME.HasValue)
                                continue;
                            var patientTypeAlterSub = listHisPatientTypeAlterLocal.Where(o => o.TREATMENT_ID == item.TREATMENT_ID).ToList();

                            bool isTreatIn = this.HasTreatIn(item, patientTypeAlterSub);

                            bool isTreatOut = this.HasTreatOut(item, patientTypeAlterSub);

                           

                            //BC hiển thị BN nội trú, ngoại trú, khám
                            if (!isTreatIn && !isTreatOut) continue;

                            if (filter.IS_TREAT_IN.HasValue)
                            {
                                if (filter.IS_TREAT_IN.Value == true)
                                {
                                    if (!isTreatIn) continue;
                                }
                                else
                                {
                                    if (!isTreatOut) continue;
                                }
                            }

                            

                            bool isPrevTreatIn = this.HasTreatIn(listHisDepartmentTranLocal.FirstOrDefault(o => o.ID == item.PREVIOUS_ID), patientTypeAlterSub);

                            bool isPrevTreatOut = this.HasTreatOut(listHisDepartmentTranLocal.FirstOrDefault(o => o.ID == item.PREVIOUS_ID), patientTypeAlterSub);

                            DEPARTMENT_TRAN previousDepartmentTran = PreviousDepartment(item, listHisDepartmentTranLocal);

                            DEPARTMENT_TRAN nextDepartmentTran = NextDepartment(item, listHisDepartmentTranLocal);

                            var treatment = listHisTreatmentLocal.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                            var lastPta = lastPatienttypeAlter.FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID);

                            if (lastPta == null)
                            {
                                continue;
                            }

                            Mrs00555RDO rdo = new Mrs00555RDO(item, previousDepartmentTran, dicCountBed, nextDepartmentTran, treatment, lastPta, filter, isPrevTreatIn, isPrevTreatOut, dateTo, treatmentids, dicTreatmentAll, treatmentBedRooms);
                            if (isTreatIn)
                            {
                                rdo.IS_TREAT_IN = (short)1;
                            }
                            if (treatment.CLINICAL_IN_TIME >= filter.TIME_FROM && treatment.CLINICAL_IN_TIME <= filter.TIME_TO)
                            {
                                rdo.COUNT_NEW_CLINIAL = 1;
                            }
                            ListRdoDetail.Add(rdo);
                        }

                        foreach (var item in listHisTreatmentClinicalIn)
                        {
                            var patientTypeAlterIn = listHisPatientTypeAlterLocal.Where(q => q.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).OrderBy(p => p.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.ID);
                            
                            Mrs00555RDO rdo = new Mrs00555RDO();
                            rdo.BRANCH_ID = item.BRANCH_ID;
                            rdo.GENDER_ID = item.TDL_PATIENT_GENDER_ID;
                            if (patientTypeAlterIn != null)
                            {
                                var departmentTranIn = listHisDepartmentTranLocal.FirstOrDefault(o => o.ID == patientTypeAlterIn.DEPARTMENT_TRAN_ID);
                                if (departmentTranIn != null)
                                {
                                    rdo.DEPARTMENT_ID = departmentTranIn.DEPARTMENT_ID;
                                    rdo.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTranIn.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                    rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTranIn.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                }
                            }
                            rdo.COUNT_CLINICAL_IN = 1;
                            
                            ListRdoDetail.Add(rdo);
                        }
                    }

                }
                foreach (var item in listHisTreatmentExam)
                {
                    var ExamServiceReq = ListExamServiceReq.OrderBy(p => p.FINISH_TIME).LastOrDefault(o => o.TREATMENT_ID == item.ID);
                    
                    Mrs00555RDO rdo = new Mrs00555RDO();
                    rdo.BRANCH_ID = item.BRANCH_ID;
                    rdo.GENDER_ID = item.TDL_PATIENT_GENDER_ID;
                    if (ExamServiceReq != null)
                    {
                        rdo.DEPARTMENT_ID = ExamServiceReq.EXECUTE_DEPARTMENT_ID;
                        rdo.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ExamServiceReq.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ExamServiceReq.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        if (ExamServiceReq.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G)
                        {
                            rdo.COUNT_BED_REQ = 1;
                        }
                    }
                    rdo.COUNT_EXAM = 1;
                    
                    ListRdoDetail.Add(rdo);
                }



                //Inventec.Common.Logging.LogSystem.Info("ListRdoDetail" + ListRdoDetail.Count);
                if (filter.IS_DETAIL_TREATMENT_BEDROOM == true)
                {
                    Inventec.Common.Logging.LogSystem.Info("Start group by room.");
                    ListRdoRoom = GroupByRoom(ListRdoDetail);
                }

                Inventec.Common.Logging.LogSystem.Info("Start group by department.");
                ListRdo = GroupByDepartment(ListRdoDetail);
                DepartmentDistinct = GroupByDepartmentDistinct(ListRdoDetail);
                DepartmentMergeTreat = GroupByDepartmentMergeTreat(ListRdo);
                Inventec.Common.Logging.LogSystem.Info("Start group by branch.");
                ListRdoSum = GroupByBranch(ListRdoDetail);
                Inventec.Common.Logging.LogSystem.Info("Start group by gender.");
                ListRdoFemale = GroupByGender(ListRdoDetail.Where(o => o.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).ToList());
                Inventec.Common.Logging.LogSystem.Info("Start group by CreateFullDepartment.");
                ListRdo = CreateFullDepartment(ListRdo);
                DepartmentDistinct = CreateFullDepartment(DepartmentDistinct);
                ListRdoCareer = GroupByCareerName(ListRdoDetail);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00555RDO>();
                result = false;
            }
            return result;
        }


        private List<Mrs00555RDO> GroupByDepartmentDistinct(List<Mrs00555RDO> ListRdo)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdo.GroupBy(o => new { o.DEPARTMENT_NAME }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    if (filter.DEPARTMENT_ID != null)
                    {
                        if (item.First().DEPARTMENT_NAME != (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME)
                        {
                            continue;
                        }
                    }
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT") || field.Name.Contains("DAY"))
                        {
                            sum = listSub.Where(o => o.IS_TREAT_IN == (short)1).Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    rdo.COUNT_OLD_OUT = listSub.Where(o => o.IS_TREAT_IN != (short)1).Sum(s => s.COUNT_OLD);
                    if (hide && rdo.COUNT_OLD_OUT != 0) hide = false;
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED =  listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    rdo.COUNT_NEW_CLINIAL = listSub.Where(x=>x.CLINICAL_IN_TIME>=filter.TIME_FROM&&x.CLINICAL_IN_TIME<=filter.TIME_TO).ToList().Count;
                    
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }

        private List<Mrs00555RDO> GroupByDepartmentMergeTreat(List<Mrs00555RDO> ListRdo)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdo.GroupBy(o => new { o.DEPARTMENT_NAME }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    if (filter.DEPARTMENT_ID != null)
                    {
                        if (item.First().DEPARTMENT_NAME != (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME)
                        {
                            continue;
                        }
                    }
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }
        private List<Mrs00555RDO> CreateFullDepartment(List<Mrs00555RDO> ListRdo)
        {
            List<Mrs00555RDO> result = ListRdo;
            try
            {
                List<string> KCCDepartmentCodes = new List<string>();
                if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                {
                    KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                }
                List<long> NoTreatmentDepartmentIds = new List<long>();
                if (HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION != null && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION.Count > 0)
                {
                    NoTreatmentDepartmentIds = HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION;
                }

                List<long> hasData = ListRdo.Select(s => s.DEPARTMENT_ID).ToList();
                var lstDepartment = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == 1
                    && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                    && !KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)
                    && !NoTreatmentDepartmentIds.Contains(o.ID)
                    && !hasData.Contains(o.ID)
                    ).OrderBy(p => !p.NUM_ORDER.HasValue).ThenBy(p => p.NUM_ORDER).ToList();
                foreach (var item in lstDepartment)
                {
                    result.Add(new Mrs00555RDO()
                    {
                        DEPARTMENT_NAME = item.DEPARTMENT_NAME,
                        ORDER = item.NUM_ORDER,
                        DEPARTMENT_CODE = item.DEPARTMENT_CODE,
                        REALITY_PATIENT_BED = (filter.IS_COUNT_BED_LOG != true)?(item.REALITY_PATIENT_COUNT ?? 0):listDepaCountBed.Where(o=>o.DEPARTMENT_ID == item.ID).Count(),
                        THEORY_PATIENT_BED = item.THEORY_PATIENT_COUNT ?? 0,
                        NUM_BED = dicCountBed.ContainsKey(item.ID) ? dicCountBed[item.ID] : 0
                    });
                }

                result = result.OrderBy(o => o.ORDER ?? 9999).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<Mrs00555RDO>();
            }
            return result;
        }

        private List<Mrs00555RDO> GroupByRoom(List<Mrs00555RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.ROOM_NAME, o.IS_TREAT_IN }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    if (filter.DEPARTMENT_ID != null)
                    {
                        if (item.First().DEPARTMENT_NAME != (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME)
                        {
                            continue;
                        }
                    }
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }

        private List<Mrs00555RDO> GroupByDepartment(List<Mrs00555RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.DEPARTMENT_NAME, o.IS_TREAT_IN }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    if (filter.DEPARTMENT_ID != null)
                    {
                        if (item.First().DEPARTMENT_NAME != (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME)
                        {
                            continue;
                        }
                    }
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }
        private List<Mrs00555RDO> GroupByGender(List<Mrs00555RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.GENDER_ID }).ToList();
                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }
        private List<Mrs00555RDO> GroupByBranch(List<Mrs00555RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.BRANCH_ID }).ToList();
                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }
        private List<Mrs00555RDO> GroupByCareerName(List<Mrs00555RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00555RDO> result = new List<Mrs00555RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.PATIENT_CAREER_NAME }).ToList();
                result.Clear();
                decimal sum = 0;
                Mrs00555RDO rdo;
                List<Mrs00555RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00555RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00555RDO();
                    listSub = item.ToList<Mrs00555RDO>();

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
                    if (filter.IS_COUNT_BED_LOG == true)
                    {
                        rdo.REALITY_PATIENT_BED = listDepaCountBed.Where(o => o.DEPARTMENT_ID == rdo.DEPARTMENT_ID).Sum(s=>s.COUNT_BED);
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00555RDO>();
            }
            return result;
        }
        private Mrs00555RDO IsMeaningful(List<Mrs00555RDO> listSub, PropertyInfo field)
        {
            string errorField = "";
            Mrs00555RDO result = new Mrs00555RDO();
            try
            {
                result = listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00555RDO();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new Mrs00555RDO();
            }
            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (this.filter.DEPARTMENT_ID != null)
            {
                ListRdo = ListRdo.Where(o => o.DEPARTMENT_ID == this.filter.DEPARTMENT_ID).ToList();
            }
            dicSingleTag.Add("DIFF_CHMS", "Chênh lệch chuyển khoa đi, chuyển khoa đến: " + string.Join(",", ListRdoDetail.GroupBy(o => o.TREATMENT_CODE).Where(p => p.Count(q => q.COUNT_MOVE == 1) > 0 && p.Count(q => q.COUNT_CHDP_IMP == 1) == 0 || p.Count(q => q.COUNT_MOVE == 1) == 0 && p.Count(q => q.COUNT_CHDP_IMP == 1) > 0).Select(p => p.Key).ToList()));
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));

            dicSingleTag.Add("DATE_TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("DATE_TIME_TO", filter.TIME_TO);

            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
            var ExamTreatIn = new ManagerSql().ExamTreatIn(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)) ?? new List<IN_TREAT_INFO>();
            if (ExamTreatIn.Count > 0)
            {
                objectTag.AddObjectData(store, "ExamFinishNotTreatIn", ExamTreatIn.Where(o => !ListRdoDetail.Exists(s => s.TREATMENT_CODE == o.TREATMENT_CODE && s.COUNT_NEW == 1)).ToList());
                var s1 = string.Join(",", ExamTreatIn.Where(o => !ListRdoDetail.Exists(s => s.TREATMENT_CODE == o.TREATMENT_CODE && s.COUNT_NEW == 1)).Select(p => p.TREATMENT_CODE).ToList());
                dicSingleTag.Add("EXAM_FINISH_NOT_TREAT_IN", s1.Length > 1000 ? s1.Substring(0, 1000) : s1);
                var s2 = string.Join(",", ListRdoDetail.Where(o => !ExamTreatIn.Exists(s => s.TREATMENT_CODE == o.TREATMENT_CODE) && o.COUNT_NEW == 1).Select(q => q.TREATMENT_CODE).ToList());
                dicSingleTag.Add("EXAM_TREAT_IN_NOT_FINISH", s2.Length > 1000 ? s2.Substring(0, 1000) : s2);
            }

            var CountExam = new ManagerSql().CountExam(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_BH", CountExam.CountBhyt);
            dicSingleTag.Add("EXAM_VP", CountExam.CountVp);

            var CountExamPatient = new ManagerSql().CountExamPatient(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_BH_1", CountExamPatient.CountBhyt);
            dicSingleTag.Add("EXAM_VP_1", CountExamPatient.CountVp);

            var CountExamCc = new ManagerSql().CountExamCc(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_BH_CC", CountExamCc.CountBhyt);
            dicSingleTag.Add("EXAM_VP_CC", CountExamCc.CountVp);
            var CountExamSan = new ManagerSql().CountExamSan(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_BH_SAN", CountExamSan.CountBhyt);
            dicSingleTag.Add("EXAM_VP_SAN", CountExamSan.CountVp);

            var CountExamFinish = new ManagerSql().CountExamFinish(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_FINISH_BH", CountExamFinish.CountBhyt);
            dicSingleTag.Add("EXAM_FINISH_VP", CountExamFinish.CountVp);

            var CountExamFinishPatient = new ManagerSql().CountExamFinishPatient(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_FINISH_BH_1", CountExamFinishPatient.CountBhyt);
            dicSingleTag.Add("EXAM_FINISH_VP_1", CountExamFinishPatient.CountVp);

            dicSingleTag.Add("EXAM_TREAT_IN_BH", ListRdoSum.Sum(s => s.COUNT_NEW_BHYT));
            dicSingleTag.Add("EXAM_TREAT_IN_VP", ListRdoSum.Sum(s => s.COUNT_NEW - s.COUNT_NEW_BHYT));

            var CountExamTreatOut = new ManagerSql().CountExamTreatOut(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_TREAT_OUT_BH", CountExamTreatOut.CountBhyt);
            dicSingleTag.Add("EXAM_TREAT_OUT_VP", CountExamTreatOut.CountVp);

            var CountExamTreatOutPatient = new ManagerSql().CountExamTreatOutPatient(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_TREAT_OUT_BH_1", CountExamTreatOutPatient.CountBhyt);
            dicSingleTag.Add("EXAM_TREAT_OUT_VP_1", CountExamTreatOutPatient.CountVp);
            //var CountExamTreatInCcs = new ManagerSql().CountExamTreatInCcs(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_TREAT_IN_BH_CC", ListRdoSum.Sum(s => s.COUNT_NEW_FROM_C_BHYT));
            dicSingleTag.Add("EXAM_TREAT_IN_VP_CC", ListRdoSum.Sum(s => s.COUNT_NEW_FROM_C - s.COUNT_NEW_FROM_C_BHYT));
            dicSingleTag.Add("EXAM_TREAT_IN_BH_S", ListRdoSum.Sum(s => s.COUNT_NEW_FROM_S_BHYT));
            dicSingleTag.Add("EXAM_TREAT_IN_VP_S", ListRdoSum.Sum(s => s.COUNT_NEW_FROM_S - s.COUNT_NEW_FROM_S_BHYT));
            //var CountExamTreatInKkb = new ManagerSql().CountExamTreatInKkb(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_TREAT_IN_BH_KKB", ListRdoSum.Sum(s => s.COUNT_NEW_BHYT - s.COUNT_NEW_FROM_CC_BHYT));
            dicSingleTag.Add("EXAM_TREAT_IN_VP_KKB", ListRdoSum.Sum(s => (s.COUNT_NEW - s.COUNT_NEW_BHYT) - (s.COUNT_NEW_FROM_CC - s.COUNT_NEW_FROM_CC_BHYT)));
            var CountExamDeath = new ManagerSql().CountExamDeath(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_DEATH_BH", CountExamDeath.CountBhyt);
            dicSingleTag.Add("EXAM_DEATH_VP", CountExamDeath.CountVp);
            var CountExamTran = new ManagerSql().CountExamTran(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_TRAN_BH", CountExamTran.CountBhyt);
            dicSingleTag.Add("EXAM_TRAN_VP", CountExamTran.CountVp);
            var CountExamOther = new ManagerSql().CountExamOther(filter) ?? new CountWithPatientType();
            dicSingleTag.Add("EXAM_OTHER_BH", CountExamOther.CountBhyt);
            dicSingleTag.Add("EXAM_OTHER_VP", CountExamOther.CountVp);
            objectTag.AddObjectData(store, "Save", new ManagerSql().GetSave(filter) ?? new List<Patient>());
            //nếu lấy theo buồng thì sửa lại báo cáo hiển thị theo buồng
            if(filter.IS_DETAIL_TREATMENT_BEDROOM == true)
            {
                foreach (var item in ListRdoRoom)
                {
                    item.DEPARTMENT_NAME = item.ROOM_NAME;
                    item.DEPARTMENT_CODE = item.ROOM_CODE;
                    item.DEPARTMENT_ID = item.ROOM_ID;
                    item.REALITY_PATIENT_BED = 0;
                    item.THEORY_PATIENT_BED = 0;
                }
                objectTag.AddObjectData(store, "Report", ListRdoRoom);
                objectTag.AddObjectData(store, "department", ListRdo);
            }    
            else
            {
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            objectTag.AddObjectData(store, "InOut", DepartmentDistinct);
            objectTag.AddObjectData(store, "MergeTreat", DepartmentMergeTreat.OrderBy(o=>o.ORDER??9999).ToList());
            objectTag.AddObjectData(store, "ReportSum", ListRdoSum);
            objectTag.AddObjectData(store, "ReportFemale", ListRdoFemale);
            objectTag.AddObjectData(store, "CLNDepartment", CLNDepartment);
            objectTag.AddObjectData(store, "Death", (new ManagerSql().GetDeath(filter) ?? new List<HIS_TREATMENT_D>()).Where(o => (o.TDL_TREATMENT_TYPE_ID != 3 && o.TREATMENT_END_TYPE_ID == 1) || (o.TDL_TREATMENT_TYPE_ID == 3 && o.TREATMENT_RESULT_ID == 5)).ToList());
            objectTag.AddObjectData(store, "Else", (new ManagerSql().GetDeath(filter) ?? new List<HIS_TREATMENT_D>()).Where(o => !(o.TDL_TREATMENT_TYPE_ID != 3 && o.TREATMENT_END_TYPE_ID == 1) && !(o.TDL_TREATMENT_TYPE_ID == 3 && o.TREATMENT_RESULT_ID == 5)).ToList());
            objectTag.AddObjectData(store, "Paty", HisPatientTypeCFG.PATIENT_TYPEs);
            for (int i = 0; i < 15; i++)
            {
                List<DataTable> dataObject = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, i + 1)) ?? new List<DataTable>();
                objectTag.AddObjectData(store, "Report" + i, dataObject.Count > 0 ? dataObject[0] : new DataTable());
                objectTag.AddObjectData(store, "Parent" + i, dataObject.Count > 1 ? dataObject[1] : new DataTable());
                objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
            }
        }

        //khoa lien ke
        private DEPARTMENT_TRAN NextDepartment(DEPARTMENT_TRAN o, List<DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.PREVIOUS_ID == o.ID) ?? new DEPARTMENT_TRAN();

        }

        //khoa truoc
        private DEPARTMENT_TRAN PreviousDepartment(DEPARTMENT_TRAN o, List<DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.ID == o.PREVIOUS_ID);

        }


        private bool HasTreatIn(DEPARTMENT_TRAN departmentTran, List<PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool HasTreatOut(DEPARTMENT_TRAN departmentTran, List<PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDicMini(ref Dictionary<string, object> dicMini)
        {
            try
            {
                string jsonNewObjectKey = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 2, 1, 1);
                if (!string.IsNullOrWhiteSpace(jsonNewObjectKey))
                {
                    if (jsonNewObjectKey.StartsWith("{") && jsonNewObjectKey.EndsWith("}") || jsonNewObjectKey.StartsWith("[") && jsonNewObjectKey.EndsWith("]"))
                    {
                        dicMini = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonNewObjectKey);
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }

}
