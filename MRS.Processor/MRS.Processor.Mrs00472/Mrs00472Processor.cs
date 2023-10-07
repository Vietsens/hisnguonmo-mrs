using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00472
{
    //báo cáo hoạt động điều trị

    class Mrs00472Processor : AbstractProcessor
    {
        Mrs00472Filter castFilter = null;
        List<Mrs00472RDO> listRdo = new List<Mrs00472RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        //List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>(); 

        string HEIN_MEDI_ORG_CODE = "34002";
        string HEIN_PROVINCE_CODE = "34";

        List<long> listSurgNS = new List<long>();
        List<long> listSurgVP = new List<long>();

        decimal TREATMENT_EXAM = 0;
        decimal HEIN_RIGHT = 0;
        decimal OUT_PROVINCE = 0;
        decimal INTERCONNECTED = 0;
        decimal PATY_FEE = 0;

        decimal TREATMENT_OUT = 0;
        decimal TREATMENT_OUT_DATE = 0;
        decimal TREATMENT_IN = 0;
        decimal TREATMENT_IN_DATE = 0;

        decimal TRAN_PATI = 0;

        decimal TOTAL_SURG = 0;
        decimal SURG_TYPE_1 = 0;
        decimal SURG_TYPE_2 = 0;
        decimal SURG_TYPE_3 = 0;

        decimal SURG_NS = 0;
        decimal SURG_VP = 0;

        decimal TREATMENT_EMERGENCY = 0;

        public Mrs00472Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00472Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00472Filter)this.reportFilter;

                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                var listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchFilter);

                if (IsNotNullOrEmpty(listBranchs))
                {
                    HEIN_MEDI_ORG_CODE = listBranchs.First().HEIN_MEDI_ORG_CODE;
                    HEIN_PROVINCE_CODE = listBranchs.First().HEIN_PROVINCE_CODE;
                }

                HisTreatmentViewFilterQuery treatmentViewfilter = new HisTreatmentViewFilterQuery();
                treatmentViewfilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewfilter.OUT_TIME_TO = castFilter.TIME_TO;
                treatmentViewfilter.IS_PAUSE = true;
                if (castFilter.DEPARTMENT_ID != null)
                {
                    treatmentViewfilter.END_DEPARTMENT_IDs = new List<long>() { castFilter.DEPARTMENT_ID ?? 0 };
                }
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewfilter);

                //--!!--!!-!!-1@!#12312312312
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00472";
                var listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter);

                listSurgNS = listServices.Where(w => w.CATEGORY_CODE == "SURG_NS").Select(s => s.SERVICE_ID).ToList();
                listSurgVP = listServices.Where(w => w.CATEGORY_CODE == "SURG_VP").Select(s => s.SERVICE_ID).ToList();
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
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                ProcessPatientAndBhyt();
                ProcessTreatmentInAndOut();
                ProcessService();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected void ProcessPatientAndBhyt()
        {
            if (IsNotNullOrEmpty(listTreatments))
            {
                List<HIS_PATIENT> listPatients = new List<HIS_PATIENT>();
                List<V_HIS_PATIENT_TYPE_ALTER> listPatyAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();

                var skip = 0;
                while (listTreatments.Count - skip > 0)
                {
                    var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    // đối tượng điều trị
                    HisPatientTypeAlterViewFilterQuery patyAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                    patyAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    listPatyAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patyAlterViewFilter));
                }

                var listTreatmentIdClins = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(s => s.TREATMENT_ID).Distinct().ToList();
                var listTreatmentIdExams = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(s => s.TREATMENT_ID).Distinct().ToList();

                var treatmentExams = listTreatments.Where(w => listTreatmentIdExams.Contains(w.ID) && !listTreatmentIdClins.Contains(w.ID)).ToList();
                if (IsNotNullOrEmpty(treatmentExams))
                {
                    // bn chỉ khám
                    this.TREATMENT_EXAM = treatmentExams.Count();
                    // khám đúng tuyến (đăng ký khám tại bv)
                    var patyAlterBhytRights = listPatyAlters.Where(w => treatmentExams.Select(s => s.ID).Contains(w.TREATMENT_ID) && w.HEIN_MEDI_ORG_CODE == this.HEIN_MEDI_ORG_CODE).Select(s => s.TREATMENT_ID).ToList();
                    if (IsNotNullOrEmpty(patyAlterBhytRights))
                        this.HEIN_RIGHT = patyAlterBhytRights.Distinct().Count();
                    // ngoại tỉnh: có nơi dk kcb bđ != tỉnh(34)
                    var patyAlterBhytOutProvinces = listPatyAlters.Where(w => treatmentExams.Select(s => s.ID).Contains(w.TREATMENT_ID) && w.HEIN_MEDI_ORG_CODE != null && w.HEIN_MEDI_ORG_CODE.Substring(0, 2) != this.HEIN_PROVINCE_CODE).Select(s => s.TREATMENT_ID).ToList();
                    if (IsNotNullOrEmpty(patyAlterBhytOutProvinces))
                        this.OUT_PROVINCE = patyAlterBhytOutProvinces.Distinct().Count();
                    // nhân dân: ko có thẻ bhyt
                    var patyAlters = listPatyAlters.Where(w => w.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatmentExams.Select(s => s.ID).Contains(w.TREATMENT_ID)).Select(s => s.TREATMENT_ID).ToList();
                    if (IsNotNullOrEmpty(patyAlters))
                        this.PATY_FEE = patyAlters.Distinct().Count();
                    // thông tuyến (đăng ký nới kcb bđ là bv khác)
                    var patyAlterBhytInterconnecteds = listPatyAlters.Where(w => treatmentExams.Select(s => s.ID).Contains(w.TREATMENT_ID)
                        && w.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE
                        && w.HEIN_MEDI_ORG_CODE != null && w.HEIN_MEDI_ORG_CODE != this.HEIN_MEDI_ORG_CODE).Select(s => s.TREATMENT_ID).ToList();
                    if (IsNotNullOrEmpty(patyAlterBhytInterconnecteds))
                        this.INTERCONNECTED = patyAlterBhytInterconnecteds.Distinct().Count();
                }

                // bn cấp cứu
                this.TREATMENT_EMERGENCY = listTreatments.Where(o => o.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Count();
            }
        }

        protected void ProcessService()
        {
            List<V_HIS_SERE_SERV_1> listSereServs = new List<V_HIS_SERE_SERV_1>();
            List<V_HIS_SERE_SERV_PTTT> listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>();

            HisSereServView1FilterQuery sereServViewFilter = new HisSereServView1FilterQuery();
            sereServViewFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
            sereServViewFilter.FINISH_TIME_TO = castFilter.TIME_TO;
            sereServViewFilter.SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT };
            listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView1(sereServViewFilter));

            listSereServs = listSereServs.Where(w => w.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
            if (castFilter.DEPARTMENT_ID != null)
            {
                listSereServs = listSereServs.Where(o => o.TDL_EXECUTE_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
            }
            if (IsNotNullOrEmpty(listSereServs))
            {

                var skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServPtttViewFilterQuery sereServPtttFilter = new HisSereServPtttViewFilterQuery();
                    sereServPtttFilter.SERE_SERV_IDs = listIds.Select(s => s.ID).ToList();
                    listSereServPttts.AddRange(new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(param).GetView(sereServPtttFilter));
                }
                if (IsNotNullOrEmpty(listSereServPttts))
                {
                    long priorityIdCC = HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC;
                    long ptttGroupId1 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;

                    long ptttGroupId2 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;

                    long ptttGroupId3 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;

                    long ptttGroupId4 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;

                    foreach (var item in listSereServPttts)
                    {
                        var sereServ = listSereServs.FirstOrDefault(o => o.ID == item.SERE_SERV_ID);
                        if (sereServ != null)
                        {
                            var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == item.PTTT_GROUP_ID);
                            if (ptttGroup != null)
                            {
                                if (!string.IsNullOrEmpty(this.castFilter.SAME_PTTT_GROUP_CODE__1)
                                    && string.Format(",{0},", this.castFilter.SAME_PTTT_GROUP_CODE__1)
                                    .Contains(string.Format(",{0},", ptttGroup.PTTT_GROUP_CODE)))
                                {
                                    item.PTTT_GROUP_ID = ptttGroupId1;
                                }
                                else if (!string.IsNullOrEmpty(this.castFilter.SAME_PTTT_GROUP_CODE__2)
                                    && string.Format(",{0},", this.castFilter.SAME_PTTT_GROUP_CODE__2)
                                    .Contains(string.Format(",{0},", ptttGroup.PTTT_GROUP_CODE)))
                                {
                                    item.PTTT_GROUP_ID = ptttGroupId2;
                                }
                                else if (!string.IsNullOrEmpty(this.castFilter.SAME_PTTT_GROUP_CODE__3)
                                    && string.Format(",{0},", this.castFilter.SAME_PTTT_GROUP_CODE__3)
                                    .Contains(string.Format(",{0},", ptttGroup.PTTT_GROUP_CODE)))
                                {
                                    item.PTTT_GROUP_ID = ptttGroupId3;
                                }
                                else if (!string.IsNullOrEmpty(this.castFilter.SAME_PTTT_GROUP_CODE__4)
                                    && string.Format(",{0},", this.castFilter.SAME_PTTT_GROUP_CODE__4)
                                    .Contains(string.Format(",{0},", ptttGroup.PTTT_GROUP_CODE)))
                                {
                                    item.PTTT_GROUP_ID = ptttGroupId4;
                                }
                            }
                            if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                            {

                                this.TOTAL_SURG += sereServ.AMOUNT;

                                if (item.PTTT_PRIORITY_ID == priorityIdCC)
                                {
                                    this.SURG_CC += sereServ.AMOUNT;
                                }
                                if (item.PTTT_GROUP_ID == ptttGroupId1)
                                {
                                    this.SURG_TYPE_1 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId2)
                                {
                                    this.SURG_TYPE_2 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId3)
                                {
                                    this.SURG_TYPE_3 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId4)
                                {
                                    this.SURG_TYPE_4 += sereServ.AMOUNT;
                                }
                                else
                                {
                                    this.SURG_TYPE_5 += sereServ.AMOUNT;
                                }
                                if (IsNotNullOrEmpty(listSurgNS))
                                {
                                    if (listSurgNS.Contains(sereServ.SERVICE_ID))
                                    {
                                        this.SURG_NS += sereServ.AMOUNT;
                                    }
                                }
                                if (IsNotNullOrEmpty(listSurgVP))
                                {
                                    if (listSurgVP.Contains(sereServ.SERVICE_ID))
                                    {
                                        this.SURG_VP += sereServ.AMOUNT;
                                    }
                                }

                            }
                            else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                            {
                                this.TOTAL_MISU += sereServ.AMOUNT;

                                if (item.PTTT_PRIORITY_ID == priorityIdCC)
                                {
                                    this.MISU_CC += sereServ.AMOUNT;
                                }
                                if (item.PTTT_GROUP_ID == ptttGroupId1)
                                {
                                    this.MISU_TYPE_1 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId2)
                                {
                                    this.MISU_TYPE_2 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId3)
                                {
                                    this.MISU_TYPE_3 += sereServ.AMOUNT;
                                }
                                else if (item.PTTT_GROUP_ID == ptttGroupId4)
                                {
                                    this.MISU_TYPE_4 += sereServ.AMOUNT;
                                }
                                else
                                {
                                    this.MISU_TYPE_5 += sereServ.AMOUNT;
                                }
                                if (IsNotNullOrEmpty(listSurgNS))
                                {
                                    if (listSurgNS.Contains(sereServ.SERVICE_ID))
                                    {
                                        this.MISU_NS += sereServ.AMOUNT;
                                    }
                                }
                                if (IsNotNullOrEmpty(listSurgVP))
                                {
                                    if (listSurgVP.Contains(sereServ.SERVICE_ID))
                                    {
                                        this.MISU_VP += sereServ.AMOUNT;
                                    }
                                }

                            }
                        }
                    }
                }


            }
        }

        protected void ProcessTreatmentInAndOut()
        {
            //HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
            //treatmentViewFilter.OUT_TIME_FROM = castFilter.TIME_FROM; 
            //treatmentViewFilter.OUT_TIME_TO = castFilter.TIME_TO; 
            //var listTreatmentInOuts = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter); 

            var skip = 0;
            List<V_HIS_PATIENT_TYPE_ALTER> listPatyAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
            while (listTreatments.Count - skip > 0)
            {
                var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                // đối tượng điều trị
                HisPatientTypeAlterViewFilterQuery patyAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                patyAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                listPatyAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patyAlterViewFilter));
            }

            List<TREATMENT_LOG_472> listTreatmentLogs = new List<TREATMENT_LOG_472>();
            foreach (var treatment in listTreatments)
            {

                var patyAlters = listPatyAlters.Where(w => w.TREATMENT_ID == treatment.ID).OrderByDescending(o => o.LOG_TIME).ToList();
                if (IsNotNullOrEmpty(patyAlters))
                {
                    var OUT_TIME = treatment.OUT_TIME ?? castFilter.TIME_TO;
                    foreach (var patyAlter in patyAlters)
                    {
                        var treatmentLog = new TREATMENT_LOG_472();
                        treatmentLog.TREATMENT_ID = treatment.ID;
                        treatmentLog.TREATMENT_TYPE_ID = patyAlter.TREATMENT_TYPE_ID;
                        treatmentLog.IN_TIME = patyAlter.LOG_TIME;
                        treatmentLog.OUT_TIME = OUT_TIME;
                        OUT_TIME = patyAlter.LOG_TIME;
                        treatmentLog.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                        listTreatmentLogs.Add(treatmentLog);
                    }
                }
            }

            var treatInOuts = listTreatments.Where(w => w.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Select(s => s.ID).ToList();
            if (IsNotNullOrEmpty(treatInOuts))
                this.TRAN_PATI = treatInOuts.Distinct().Count();

            treatInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(s => s.TREATMENT_ID).ToList();
            if (IsNotNullOrEmpty(treatInOuts))
                this.TREATMENT_OUT = treatInOuts.Distinct().Count();
            treatInOuts = listPatyAlters.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(s => s.TREATMENT_ID).ToList();
            if (IsNotNullOrEmpty(treatInOuts))
                this.TREATMENT_IN = treatInOuts.Distinct().Count();

            treatInOuts = listTreatments.Where(w => w.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(s => s.ID).ToList();
            if (IsNotNullOrEmpty(treatInOuts))
                this.TREATMENT_OUT_NEW = treatInOuts.Distinct().Count();
            treatInOuts = listTreatments.Where(w => w.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(s => s.ID).ToList();
            if (IsNotNullOrEmpty(treatInOuts))
                this.TREATMENT_IN_NEW = treatInOuts.Distinct().Count();

            //this.TREATMENT_OUT = listTreatmentInOuts.Where(w => listPatyAlters.Where(ww => ww.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 
            //this.TREATMENT_IN = listTreatmentInOuts.Where(w => listPatyAlters.Where(ww => ww.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(s => s.TREATMENT_ID).Contains(w.ID)).Count(); 

            if (IsNotNullOrEmpty(listTreatmentLogs))
            {
                var treatmentIns = listTreatmentLogs.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                foreach (var treatmentIn in treatmentIns)
                {
                    this.TREATMENT_IN_DATE = TREATMENT_IN_DATE + HIS.Treatment.DateTime.Calculation.DayOfTreatment(treatmentIn.IN_TIME, treatmentIn.OUT_TIME);
                }
                var treatmentOuts = listTreatmentLogs.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                foreach (var treatmentOut in treatmentOuts)
                {
                    this.TREATMENT_OUT_DATE = TREATMENT_OUT_DATE + HIS.Treatment.DateTime.Calculation.DayOfTreatment(treatmentOut.IN_TIME, treatmentOut.OUT_TIME);
                }
                if (castFilter.IS_COUNT_DAY_BORDERAU == true)
                {
                    this.TREATMENT_IN_DATE = treatmentIns.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    this.TREATMENT_OUT_DATE = treatmentOuts.GroupBy(o => o.TREATMENT_ID).Select(p => p.First()).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                }
            }

        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("TREATMENT_EXAM", TREATMENT_EXAM);
                dicSingleTag.Add("HEIN_RIGHT", HEIN_RIGHT);
                //dicSingleTag.Add("HEIN_LEFT", rdo.HEIN_LEFT); 
                dicSingleTag.Add("OUT_PROVINCE", OUT_PROVINCE);
                dicSingleTag.Add("PATY_FEE", PATY_FEE);
                dicSingleTag.Add("INTERCONNECTED", INTERCONNECTED);
                dicSingleTag.Add("TREATMENT_OUT", TREATMENT_OUT);
                dicSingleTag.Add("TREATMENT_OUT_DATE", TREATMENT_OUT_DATE);
                dicSingleTag.Add("TREATMENT_IN", TREATMENT_IN);
                dicSingleTag.Add("TREATMENT_IN_DATE", TREATMENT_IN_DATE);
                dicSingleTag.Add("TRAN_PATI", TRAN_PATI);
                //dicSingleTag.Add("TRAN_PATI_HIGHT", rdo.TRAN_PATI_HIGHT); 
                dicSingleTag.Add("TREATMENT_EMERGENCY", TREATMENT_EMERGENCY);
                dicSingleTag.Add("TOTAL_SURG", TOTAL_SURG);
                dicSingleTag.Add("SURG_TYPE_1", SURG_TYPE_1);
                dicSingleTag.Add("SURG_TYPE_2", SURG_TYPE_2);
                dicSingleTag.Add("SURG_TYPE_3", SURG_TYPE_3);
                dicSingleTag.Add("SURG_TYPE_4", SURG_TYPE_4);
                dicSingleTag.Add("SURG_TYPE_5", SURG_TYPE_5);
                dicSingleTag.Add("TOTAL_MISU", TOTAL_MISU);
                dicSingleTag.Add("MISU_TYPE_1", MISU_TYPE_1);
                dicSingleTag.Add("MISU_TYPE_2", MISU_TYPE_2);
                dicSingleTag.Add("MISU_TYPE_3", MISU_TYPE_3);
                dicSingleTag.Add("MISU_TYPE_4", MISU_TYPE_4);
                dicSingleTag.Add("MISU_TYPE_5", MISU_TYPE_5);
                dicSingleTag.Add("SURG_NS", SURG_NS);
                dicSingleTag.Add("SURG_VP", SURG_VP);
                dicSingleTag.Add("MISU_NS", MISU_NS);
                dicSingleTag.Add("MISU_VP", MISU_VP);
                dicSingleTag.Add("SURG_CC", SURG_CC);
                dicSingleTag.Add("MISU_CC", MISU_CC);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        decimal SURG_TYPE_4 = 0;
        decimal SURG_TYPE_5 = 0;

        decimal MISU_TYPE_1 = 0;

        decimal MISU_TYPE_2 = 0;

        decimal MISU_TYPE_3 = 0;

        decimal MISU_TYPE_4 = 0;

        decimal MISU_TYPE_5 = 0;

        decimal MISU_CC = 0;

        decimal SURG_CC = 0;

        decimal MISU_VP = 0;

        decimal MISU_NS = 0;


        decimal TOTAL_MISU = 0;

        public int TREATMENT_OUT_NEW { get; set; }

        public int TREATMENT_IN_NEW { get; set; }
    }
}
