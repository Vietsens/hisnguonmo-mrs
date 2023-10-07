using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDeathWithin;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisReportTypeCat;

namespace MRS.Processor.Mrs00439
{
    // báo cáo công tác khám chữa bệnh

    class Mrs00439Processor : AbstractProcessor
    {
        string ProvinceCode = "";
        Mrs00439Filter castFilter = null;
        List<Mrs00439RDO> listRdo = new List<Mrs00439RDO>();

        List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_TYPE_ALTER> listPaTyAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<long> lisServiceMRI = new List<long>();
        List<long> lisServiceCT = new List<long>();
        List<long> lisServiceXQ = new List<long>();
        List<long> lisServiceGP = new List<long>();
        List<long> lisServiceVS = new List<long>();
        List<long> lisServiceHH = new List<long>();
        List<long> lisServiceSH = new List<long>();
        List<long> listServiceSurgMicros = new List<long>();
        List<long> listServiceSurgEndos = new List<long>();
        List<long> listServiceBornPtlts = new List<long>();
        List<long> listServiceBornHards = new List<long>();
        List<long> listServiceBornNormals = new List<long>();
        List<long> listServiceSurg = new List<long>();
        List<RDO_TREATMENT_LOG> ListTreatmentLogs = new List<RDO_TREATMENT_LOG>();

        long TREATMENT = 0;                  // số hsđt đk trong khoảng thời gian đã chọn (có sử dụng dịch vụ)
        long TREATMENT_EXAM = 0;             // số hsđt có khám
        decimal TOTAL_EXAM = 0;              // tổng số lượt khám
        long BHYT_LEFT = 0;                  // bhyt đúng tuyến 
        long BHYT_RIGHT = 0;                 // bhyt trái tuyến
        long OTHER_PROVINCE = 0;                //Tỉnh khác

        long TREATMENT_OUT = 0;              // số bn nhập viện ngoại trú
        long TREATMENT_OUT_DATE = 0;         // Số ngày điều trị ngoại trú

        long TREATMENT_IN = 0;               // số bn nhập viện nội trú
        long TREATMENT_IN_DATE = 0;          // số ngày điều trị nội trú


        long DIFF_DATE_REPORT = 0;          //Số ngày báo cáo
        decimal REALY_BED_AMOUNT = 0;          //Số ngày giường thực kê

        long RESULT_HEAVY = 0;              //Số ca nặng hơn

        long EMERGENCY = 0;                  // số ca cấp cứu (bn mới)
        long TRAN_PATI = 0;                  // số ca chuyển viện (bn mới)
        long TOTAL_TRAN_PATI = 0;            // số ca chuyển viện (bao gồm cả bn cũ)
        long TRAN_PATI_AMBULANCE = 0;            // số ca chuyển viện dùng xe của viện

        decimal TOTAL_SURG = 0;                 // số ca pt
        decimal TOTAL_MISU = 0;                 // số ca tt

        decimal TOTAL_SURG_GROUP_DB = 0;                 // số ca pt dặc biệt
        decimal TOTAL_SURG_GROUP_1 = 0;                 // số ca pt loại 1
        decimal TOTAL_SURG_GROUP_2 = 0;                 // số ca pt loại 2
        decimal TOTAL_SURG_GROUP_3 = 0;                 // số ca pt loại 3


        decimal TOTAL_SURG_MICRO = 0;             //Phẫu thuật vi phẫu
        decimal TOTAL_SURG_ENDO = 0;             //Phẫu thuật Nội soi

        //long TOTAL_BORN = 0;               // đẻ (tổng số ca trong tháng)
        decimal TOTAL_BORN_NORMAL = 0;       // đẻ thường
        decimal TOTAL_BORN_HARD = 0;         // đẻ khó
        decimal TOTAL_BORN_PTLT = 0;         // phẫu thuật lấy thai

        //long BORN = 0;                     // đẻ (bn mới)
        decimal BORN_NORMAL = 0;             // đẻ thường
        decimal BORN_HARD = 0;               // đẻ khó
        decimal BORN_PTLT = 0;               // phẫu thuật lấy thai

        long TOTAL_DEATH = 0;                // chết (tổng số ca trong tháng)
        long TOTAL_DEATH_BEF_24 = 0;         // chết trước 24 giờ sau vào viện
        long TOTAL_DEATH_AFT_24 = 0;         // chết sau 24 giờ sau nhập viện

        long DEATH = 0;                      // chết (bn mới)
        long DEATH_BEF_24 = 0;
        long DEATH_AFT_24 = 0;

        decimal TOTAL_TEST_SH = 0;              // xn sinh hóa (tổng số ca trong tháng)
        decimal TOTAL_TEST_HH = 0;              // xn huyết học
        decimal TOTAL_TEST_VS = 0;              // xn ví sinh
        decimal TOTAL_TEST_GP = 0;              // xn Giải phẫu

        decimal TEST_SH = 0;                    // xn sinh hóa (bn mới)
        decimal TEST_HH = 0;                    // xn huyết học
        decimal TEST_VS = 0;                    // xn vi sinh

        decimal TOTAL_DIIM_XQ = 0;              // xquang (tất cả bn)
        decimal TOTAL_DIIM_SA = 0;              // siêu âm 
        decimal TOTAL_DIIM_NS = 0;              // nội soi
        decimal TOTAL_DIIM_MRI = 0;             //MRI
        decimal TOTAL_DIIM_CT = 0;              //Chụp CT

        decimal DIIM_XQ = 0;                    // xquang (bn mới)
        decimal DIIM_SA = 0;
        decimal DIIM_NS = 0;


        decimal CLS_NEW_COUNT = 0;                    //Tổng số cận lâm sàng mới
        decimal CLS_NEW_AMOUNT = 0;                    //Tổng số lượng sử dụng cận lâm sàng mới

        List<LIST_KEY> listKey = new List<LIST_KEY>();

        public Mrs00439Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00439Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00439Filter)this.reportFilter;
                HisTreatmentFilterQuery treatmentViewFilter = new HisTreatmentFilterQuery();
                treatmentViewFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewFilter.IN_TIME_TO = castFilter.TIME_TO;
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).Get(treatmentViewFilter);

                var skip = 0;
                while (listTreatments.Count - skip > 0)
                {
                    var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisPatientTypeAlterFilterQuery patyAlterViewFilter = new HisPatientTypeAlterFilterQuery();
                    patyAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    patyAlterViewFilter.LOG_TIME_TO = castFilter.TIME_TO;
                    listPaTyAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).Get(patyAlterViewFilter));

                }

                //YC-DV
                HisSereServFilterQuery sereServViewFilter = new HisSereServFilterQuery();
                sereServViewFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServViewFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).Get(sereServViewFilter);

                listSereServs = listSereServs.Where(w => w.IS_ACTIVE == 1 && w.IS_NO_EXECUTE != 1).ToList();
                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00439"
                };
                var listReportTypeCat = new HisReportTypeCatManager().Get(HisReportTypeCatfilter);
                HisServiceRetyCatFilterQuery serviceRetyCatFilter = new HisServiceRetyCatFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CAT_IDs = listReportTypeCat.Select(o => o.ID).Distinct().ToList();
                listServiceRetyCat = new HisServiceRetyCatManager(param).Get(serviceRetyCatFilter);
                lisServiceMRI = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__MRI)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceCT = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__CT)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceXQ = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__XQ)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceGP = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__GP)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceVS = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__VS)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceHH = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__HH)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                lisServiceSH = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__SH)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceSurgMicros = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__SU_M)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceSurgEndos = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__SU_E)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceBornPtlts = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__PTLT)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceBornHards = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__HARD)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceBornNormals = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__NORMAL)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();
                listServiceSurg = listServiceRetyCat.Where(p => listReportTypeCat.Exists(q => q.ID == p.REPORT_TYPE_CAT_ID && q.CATEGORY_CODE == castFilter.CATEGORY_CODE__SURG)).Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>();

                this.ProvinceCode = (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.branch_id) ?? new HIS_BRANCH()).HEIN_PROVINCE_CODE;
                this.DIFF_DATE_REPORT = DateDiff.diffDate(castFilter.TIME_FROM, castFilter.TIME_TO);

                listService = new HisServiceManager().Get(new HisServiceFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE });
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
                foreach (var treatment in listTreatments)
                {
                    var patyAlters = listPaTyAlters.Where(w => w.TREATMENT_ID == treatment.ID).OrderByDescending(o => o.LOG_TIME).ToList();
                    if (IsNotNullOrEmpty(patyAlters))
                    {
                        var OUT_TIME = treatment.OUT_TIME ?? castFilter.TIME_TO;
                        foreach (var patyAlter in patyAlters)
                        {
                            var treatmentLog = new RDO_TREATMENT_LOG();
                            treatmentLog.TREATMENT_ID = treatment.ID;
                            treatmentLog.TREATMENT_TYPE_ID = patyAlter.TREATMENT_TYPE_ID;
                            treatmentLog.IN_TIME = patyAlter.LOG_TIME;
                            treatmentLog.OUT_TIME = OUT_TIME;
                            OUT_TIME = patyAlter.LOG_TIME;
                            ListTreatmentLogs.Add(treatmentLog);
                        }
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    LIST_KEY ro = new LIST_KEY();
                    ro.START_MONTH = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(new System.DateTime(DateTime.Now.Year, i+1,1));
                    ro.FINISH_MONTH = Inventec.Common.DateTime.Get.EndMonth(new System.DateTime(DateTime.Now.Year, i + 1, 1));
                    var listTreatmentSub = listTreatments.Where(o => o.IN_DATE >= ro.START_MONTH && o.IN_DATE <= ro.FINISH_MONTH && o.IN_DATE >= castFilter.TIME_FROM && o.IN_DATE <= castFilter.TIME_TO).ToList();
                    var listSereServSub = listSereServs.Where(o => listTreatmentSub.Exists(p => p.ID == o.TDL_TREATMENT_ID)).ToList();
                    #region Thông tin Hồ sơ điều trị
                    //Tổng số bệnh nhân
                    ro.COUNT = listTreatmentSub.Count();
                    //Tổng hsdt có dịch vụ
                    ro.TREATMENT = listTreatmentSub.Where(w => listSereServSub.Exists(s => s.TDL_TREATMENT_ID == w.ID)).Count();
                    //tổng hsdt khám
                    ro.TREATMENT_EXAM = listTreatmentSub.Where(w => listSereServSub.Exists(ww => ww.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && ww.TDL_TREATMENT_ID == w.ID)).Count();
                    //tổng lượt khám
                    ro.TOTAL_EXAM = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(su => su.AMOUNT);

                    //Tổng trái tuyến
                    var patyAlters = listPaTyAlters.Where(w => w.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE).ToList();
                    ro.BHYT_LEFT = listTreatmentSub.Where(w => patyAlters.Exists(s => s.TREATMENT_ID == w.ID)).Count();

                    //Tổng đúng tuyến
                    patyAlters = listPaTyAlters.Where(w => w.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE).ToList();
                    ro.BHYT_RIGHT = listTreatmentSub.Where(w => patyAlters.Exists(s => s.TREATMENT_ID == w.ID)).Count();

                    //Tổng ngoại tỉnh
                    ro.OTHER_PROVINCE = listTreatmentSub.Where(w => (w.TDL_HEIN_CARD_NUMBER != null && w.TDL_HEIN_CARD_NUMBER.Length == 15 && w.TDL_HEIN_CARD_NUMBER.Substring(3, 2) != this.ProvinceCode) || ((w.TDL_HEIN_CARD_NUMBER == null || w.TDL_HEIN_CARD_NUMBER.Length != 15) && w.TDL_PATIENT_PROVINCE_CODE != this.ProvinceCode)).Count();

                    // Tổng điều trị nội trú
                    patyAlters = listPaTyAlters.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    ro.TREATMENT_IN = listTreatmentSub.Where(w => patyAlters.Exists(s => s.TREATMENT_ID == w.ID)).Count();

                    //Tổng điều trị ngoại trú
                    patyAlters = listPaTyAlters.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    ro.TREATMENT_OUT = listTreatmentSub.Where(w => patyAlters.Exists(s => s.TREATMENT_ID == w.ID)).Count();

                    //Tính số ngày nội trú, ngoại trú tương ứng
                    ProcessTreatmentDay(ro);

                    #endregion
                    //exam
                    #region Thông tin giường, sinh đẻ, chuyển tuyến, tử vong
                    //Số giường thực kê
                    ro.REALY_BED_AMOUNT = listSereServSub.Where(w => listTreatmentSub.Exists(s => s.ID == w.TDL_TREATMENT_ID) && w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(su => su.AMOUNT);

                    //Tổng số người bệnh nặng thêm
                    ro.RESULT_HEAVY = listTreatmentSub.Where(w => w.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG).Count();

                    //Tổng cấp cứu
                    ro.EMERGENCY = listTreatmentSub.Where(w => w.IS_EMERGENCY != null).Count();

                    //Tổng chuyển viện theo bệnh nhân
                    ro.TRAN_PATI = listTreatmentSub.Where(w => w.TRANSFER_IN_MEDI_ORG_CODE != null || w.MEDI_ORG_CODE != null).Count();

                    //Tổng chuyển viện theo lượt vào ra
                    ro.TOTAL_TRAN_PATI = listTreatmentSub.Where(w => w.TRANSFER_IN_MEDI_ORG_CODE != null).Count() + listTreatmentSub.Where(w => w.MEDI_ORG_CODE != null).Count();

                    //Tổng số ca đẻ thường
                    ro.BORN_NORMAL = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0) && listServiceBornNormals.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    //Tổng số ca đẻ khó
                    ro.BORN_HARD = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0) && listServiceBornHards.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    //Tổng số ca đẻ mổ
                    ro.BORN_PTLT = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0) && listServiceBornPtlts.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    // tất cả các dịch vụ đẻ
                    ro.TOTAL_BORN_NORMAL = listSereServSub.Where(w => listServiceBornNormals.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    ro.TOTAL_BORN_HARD = listSereServSub.Where(w => listServiceBornHards.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    ro.TOTAL_BORN_PTLT = listSereServSub.Where(w => listServiceBornPtlts.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);


                    // tổng số BN tử vong
                    var tmp = HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS;
                    ro.TOTAL_DEATH = listTreatmentSub.Where(w => w.DEATH_WITHIN_ID != null).Count();
                    ro.TOTAL_DEATH_BEF_24 = listTreatmentSub.Where(w => w.DEATH_WITHIN_ID == tmp).Count();
                    ro.TOTAL_DEATH_AFT_24 = TOTAL_DEATH - TOTAL_DEATH_BEF_24;
                    #endregion

                    #region thông tin phẫu thuật thủ thuật
                    // phẫu thuật + thủ thuật
                    ro.TOTAL_SURG = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(su => su.AMOUNT)
                        + listSereServSub.Where(w => listServiceSurg.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    ro.TOTAL_MISU = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(su => su.AMOUNT)
                        - listSereServSub.Where(w => listServiceSurg.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    // Phẫu thuật loại đặc biệt
                    tmp = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;
                    ro.TOTAL_SURG_GROUP_DB = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && listService.Exists(o => o.ID == w.SERVICE_ID && o.PTTT_GROUP_ID == tmp)).Sum(su => su.AMOUNT);
                    // Phẫu thuật loại 1
                    tmp = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;
                    ro.TOTAL_SURG_GROUP_1 = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && listService.Exists(o => o.ID == w.SERVICE_ID && o.PTTT_GROUP_ID == tmp)).Sum(su => su.AMOUNT);
                    // Phẫu thuật loại 2
                    tmp = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;
                    ro.TOTAL_SURG_GROUP_2 = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && listService.Exists(o => o.ID == w.SERVICE_ID && o.PTTT_GROUP_ID == tmp)).Sum(su => su.AMOUNT);
                    // Phẫu thuật loại 3
                    tmp = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;
                    ro.TOTAL_SURG_GROUP_3 = listSereServSub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && listService.Exists(o => o.ID == w.SERVICE_ID && o.PTTT_GROUP_ID == tmp)).Sum(su => su.AMOUNT);

                    //Số phẫu thuật nội soi
                    ro.TOTAL_SURG_ENDO = listSereServSub.Where(w => listServiceSurgEndos.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    //Số phẫu thuật vi phẫu
                    ro.TOTAL_SURG_MICRO = listSereServSub.Where(w => listServiceSurgMicros.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    #endregion

                    #region Thông tin cận lâm sàng
                    // xét nghiệm
                    var temp = HisRoomCFG.ROOM_ID__XNSH ?? new List<long>();
                    ro.TEST_SH = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceSH.Contains(w.SERVICE_ID))).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__XNHH ?? new List<long>();
                    ro.TEST_HH = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceHH.Contains(w.SERVICE_ID))).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__XNVS ?? new List<long>();
                    ro.TEST_VS = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceVS.Contains(w.SERVICE_ID))).Sum(su => su.AMOUNT);
                    // tất cả xét nghiệm
                    temp = HisRoomCFG.ROOM_ID__XNSH ?? new List<long>();
                    ro.TOTAL_TEST_SH = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceSH.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__XNHH ?? new List<long>();
                    ro.TOTAL_TEST_HH = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceHH.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__XNVS ?? new List<long>();
                    ro.TOTAL_TEST_VS = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceVS.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__XNGP ?? new List<long>();
                    ro.TOTAL_TEST_GP = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceGP.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    // cđha
                    temp = HisRoomCFG.ROOM_ID__HANS ?? new List<long>();
                    ro.DIIM_NS = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HASA ?? new List<long>();
                    ro.DIIM_SA = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HAXQ ?? new List<long>();
                    ro.DIIM_XQ = listSereServSub.Where(w => listTreatmentSub.Select(s => s.ID).Contains(w.TDL_TREATMENT_ID ?? 0)
                        && (temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceXQ.Contains(w.SERVICE_ID))).Sum(su => su.AMOUNT);

                    // tất cả cđha
                    temp = HisRoomCFG.ROOM_ID__HANS ?? new List<long>();
                    ro.TOTAL_DIIM_NS = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HASA ?? new List<long>();
                    ro.TOTAL_DIIM_SA = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HAXQ ?? new List<long>();
                    ro.TOTAL_DIIM_XQ = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceXQ.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HAMRI ?? new List<long>();
                    ro.TOTAL_DIIM_MRI = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceCT.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    temp = HisRoomCFG.ROOM_ID__HACT ?? new List<long>();
                    ro.TOTAL_DIIM_CT = listSereServSub.Where(w => temp.Contains(w.TDL_EXECUTE_ROOM_ID) || lisServiceMRI.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);

                    //Tổng cận lâm sàng mới
                    var listNewService = new Mrs00439RDOManager().GetMrs00439RDOByIntructionTime(castFilter.TIME_FROM, castFilter.TIME_TO);
                    ro.CLS_NEW_COUNT = listNewService.Count;
                    ro.CLS_NEW_AMOUNT = listSereServSub.Where(w => listNewService.Contains(w.SERVICE_ID)).Sum(su => su.AMOUNT);
                    #endregion
                    listKey.Add(ro);
                }
                TREATMENT = listKey.Sum(s => s.TREATMENT);                  // số hsđt đk trong khoảng thời gian đã chọn (có sử dụng dịch vụ)
                TREATMENT_EXAM = listKey.Sum(s => s.TREATMENT_EXAM);             // số hsđt có khám
                TOTAL_EXAM = listKey.Sum(s => s.TOTAL_EXAM);              // tổng số lượt khám
                BHYT_LEFT = listKey.Sum(s => s.BHYT_LEFT);                  // bhyt đúng tuyến 
                BHYT_RIGHT = listKey.Sum(s => s.BHYT_RIGHT);                 // bhyt trái tuyến
                OTHER_PROVINCE = listKey.Sum(s => s.OTHER_PROVINCE);                //Tỉnh khác

                TREATMENT_OUT = listKey.Sum(s => s.TREATMENT_OUT);              // số bn nhập viện ngoại trú
                TREATMENT_OUT_DATE = listKey.Sum(s => s.TREATMENT_OUT_DATE);         // Số ngày điều trị ngoại trú

                TREATMENT_IN = listKey.Sum(s => s.TREATMENT_IN);               // số bn nhập viện nội trú
                TREATMENT_IN_DATE = listKey.Sum(s => s.TREATMENT_IN_DATE);          // số ngày điều trị nội trú


                DIFF_DATE_REPORT = listKey.Sum(s => s.DIFF_DATE_REPORT);          //Số ngày báo cáo
                REALY_BED_AMOUNT = listKey.Sum(s => s.REALY_BED_AMOUNT);          //Số ngày giường thực kê

                RESULT_HEAVY = listKey.Sum(s => s.RESULT_HEAVY);              //Số ca nặng hơn

                EMERGENCY = listKey.Sum(s => s.EMERGENCY);                  // số ca cấp cứu (bn mới)
                TRAN_PATI = listKey.Sum(s => s.TRAN_PATI);                  // số ca chuyển viện (bn mới)
                TOTAL_TRAN_PATI = listKey.Sum(s => s.TOTAL_TRAN_PATI);            // số ca chuyển viện (bao gồm cả bn cũ)
                TRAN_PATI_AMBULANCE = listKey.Sum(s => s.TRAN_PATI_AMBULANCE);            // số ca chuyển viện dùng xe của viện

                TOTAL_SURG = listKey.Sum(s => s.TOTAL_SURG);                 // số ca pt
                TOTAL_MISU = listKey.Sum(s => s.TOTAL_MISU);                 // số ca tt

                TOTAL_SURG_GROUP_DB = listKey.Sum(s => s.TOTAL_SURG_GROUP_DB);                 // số ca pt dặc biệt
                TOTAL_SURG_GROUP_1 = listKey.Sum(s => s.TOTAL_SURG_GROUP_1);                 // số ca pt loại 1
                TOTAL_SURG_GROUP_2 = listKey.Sum(s => s.TOTAL_SURG_GROUP_2);                 // số ca pt loại 2
                TOTAL_SURG_GROUP_3 = listKey.Sum(s => s.TOTAL_SURG_GROUP_3);                 // số ca pt loại 3


                TOTAL_SURG_MICRO = listKey.Sum(s => s.TOTAL_SURG_MICRO);             //Phẫu thuật vi phẫu
                TOTAL_SURG_ENDO = listKey.Sum(s => s.TOTAL_SURG_ENDO);             //Phẫu thuật Nội soi

                //long TOTAL_BORN = listKey.Sum(s=>s.ssss);               // đẻ (tổng số ca trong tháng)
                TOTAL_BORN_NORMAL = listKey.Sum(s => s.TOTAL_BORN_NORMAL);       // đẻ thường
                TOTAL_BORN_HARD = listKey.Sum(s => s.TOTAL_BORN_HARD);         // đẻ khó
                TOTAL_BORN_PTLT = listKey.Sum(s => s.TOTAL_BORN_PTLT);         // phẫu thuật lấy thai

                //long BORN = listKey.Sum(s=>s.ssss);                     // đẻ (bn mới)
                BORN_NORMAL = listKey.Sum(s => s.BORN_NORMAL);             // đẻ thường
                BORN_HARD = listKey.Sum(s => s.BORN_HARD);               // đẻ khó
                BORN_PTLT = listKey.Sum(s => s.BORN_PTLT);               // phẫu thuật lấy thai

                TOTAL_DEATH = listKey.Sum(s => s.TOTAL_DEATH);                // chết (tổng số ca trong tháng)
                TOTAL_DEATH_BEF_24 = listKey.Sum(s => s.TOTAL_DEATH_BEF_24);         // chết trước 24 giờ sau vào viện
                TOTAL_DEATH_AFT_24 = listKey.Sum(s => s.TOTAL_DEATH_AFT_24);         // chết sau 24 giờ sau nhập viện

                DEATH = listKey.Sum(s => s.DEATH);                      // chết (bn mới)
                DEATH_BEF_24 = listKey.Sum(s => s.DEATH_BEF_24);
                DEATH_AFT_24 = listKey.Sum(s => s.DEATH_AFT_24);

                TOTAL_TEST_SH = listKey.Sum(s => s.TOTAL_TEST_SH);              // xn sinh hóa (tổng số ca trong tháng)
                TOTAL_TEST_HH = listKey.Sum(s => s.TOTAL_TEST_HH);              // xn huyết học
                TOTAL_TEST_VS = listKey.Sum(s => s.TOTAL_TEST_VS);              // xn ví sinh
                TOTAL_TEST_GP = listKey.Sum(s => s.TOTAL_TEST_GP);              // xn Giải phẫu

                TEST_SH = listKey.Sum(s => s.TEST_SH);                    // xn sinh hóa (bn mới)
                TEST_HH = listKey.Sum(s => s.TEST_HH);                    // xn huyết học
                TEST_VS = listKey.Sum(s => s.TEST_VS);                    // xn vi sinh

                TOTAL_DIIM_XQ = listKey.Sum(s => s.TOTAL_DIIM_XQ);              // xquang (tất cả bn)
                TOTAL_DIIM_SA = listKey.Sum(s => s.TOTAL_DIIM_SA);              // siêu âm 
                TOTAL_DIIM_NS = listKey.Sum(s => s.TOTAL_DIIM_NS);              // nội soi
                TOTAL_DIIM_MRI = listKey.Sum(s => s.TOTAL_DIIM_MRI);             //MRI
                TOTAL_DIIM_CT = listKey.Sum(s => s.TOTAL_DIIM_CT);              //Chụp CT

                DIIM_XQ = listKey.Sum(s => s.DIIM_XQ);                    // xquang (bn mới)
                DIIM_SA = listKey.Sum(s => s.DIIM_SA);
                DIIM_NS = listKey.Sum(s => s.DIIM_NS);


                CLS_NEW_COUNT = listKey.Sum(s => s.CLS_NEW_COUNT);                    //Tổng số cận lâm sàng mới
                CLS_NEW_AMOUNT = listKey.Sum(s => s.CLS_NEW_AMOUNT);                    //Tổng số lượng sử dụng cận lâm sàng mới

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected void ProcessTreatmentDay(LIST_KEY ro)
        {
            try
            {


                if (IsNotNullOrEmpty(ListTreatmentLogs))
                {
                    var treatmentIns = ListTreatmentLogs.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && w.OUT_TIME >= castFilter.TIME_FROM && w.IN_TIME >= ro.START_MONTH && w.IN_TIME <= ro.FINISH_MONTH && w.IN_TIME >= castFilter.TIME_FROM && w.IN_TIME <= castFilter.TIME_TO).ToList();
                    foreach (var treatmentIn in treatmentIns)
                    {
                        var IN_TIME = treatmentIn.IN_TIME > castFilter.TIME_FROM ? treatmentIn.IN_TIME : castFilter.TIME_FROM;
                        ro.TREATMENT_IN_DATE = ro.TREATMENT_IN_DATE + DateDiff.diffDate(IN_TIME, treatmentIn.OUT_TIME);
                    }

                    var treatmentOuts = ListTreatmentLogs.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && w.OUT_TIME >= castFilter.TIME_FROM && w.OUT_TIME >= castFilter.TIME_FROM && w.IN_TIME >= ro.START_MONTH && w.IN_TIME <= ro.FINISH_MONTH && w.IN_TIME >= castFilter.TIME_FROM && w.IN_TIME <= castFilter.TIME_TO).ToList();
                    foreach (var treatmentOut in treatmentOuts)
                    {
                        var IN_TIME = treatmentOut.IN_TIME > castFilter.TIME_FROM ? treatmentOut.IN_TIME : castFilter.TIME_FROM;
                        ro.TREATMENT_OUT_DATE = ro.TREATMENT_OUT_DATE + DateDiff.diffDate(IN_TIME, treatmentOut.OUT_TIME);
                    }
                }
            }
            catch
            {

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
                objectTag.AddObjectData(store, "Report", listKey);
                dicSingleTag.Add("TREATMENT", this.TREATMENT);
                dicSingleTag.Add("TREATMENT_EXAM", this.TREATMENT_EXAM);
                dicSingleTag.Add("BHYT_LEFT", this.BHYT_LEFT);
                dicSingleTag.Add("BHYT_RIGHT", this.BHYT_RIGHT);
                dicSingleTag.Add("TOTAL_EXAM", this.TOTAL_EXAM);
                dicSingleTag.Add("TREATMENT_OUT", this.TREATMENT_OUT);
                dicSingleTag.Add("TREATMENT_OUT_DATE", this.TREATMENT_OUT_DATE);
                dicSingleTag.Add("TREATMENT_IN", this.TREATMENT_IN);
                dicSingleTag.Add("TREATMENT_IN_DATE", this.TREATMENT_IN_DATE);
                dicSingleTag.Add("EMERGENCY", this.EMERGENCY);
                dicSingleTag.Add("TRAN_PATI", this.TRAN_PATI);
                dicSingleTag.Add("TOTAL_TRAN_PATI", this.TOTAL_TRAN_PATI);
                dicSingleTag.Add("TOTAL_SURG", this.TOTAL_SURG);
                dicSingleTag.Add("TOTAL_MISU", this.TOTAL_MISU);
                dicSingleTag.Add("TOTAL_BORN_NORMAL", this.TOTAL_BORN_NORMAL);
                dicSingleTag.Add("TOTAL_BORN_HARD", this.TOTAL_BORN_HARD);
                dicSingleTag.Add("TOTAL_BORN_PTLT", this.TOTAL_BORN_PTLT);
                dicSingleTag.Add("BORN_NORMAL", this.BORN_NORMAL);
                dicSingleTag.Add("BORN_HARD", this.BORN_HARD);
                dicSingleTag.Add("BORN_PTLT", this.BORN_PTLT);
                dicSingleTag.Add("TOTAL_DEATH", this.TOTAL_DEATH);
                dicSingleTag.Add("TOTAL_DEATH_BEF_24", this.TOTAL_DEATH_BEF_24);
                dicSingleTag.Add("TOTAL_DEATH_AFT_24", this.TOTAL_DEATH_AFT_24);
                dicSingleTag.Add("DEATH", this.DEATH);
                dicSingleTag.Add("DEATH_BEF_24", this.DEATH_BEF_24);
                dicSingleTag.Add("DEATH_AFT_24", this.DEATH_AFT_24);
                dicSingleTag.Add("TOTAL_TEST_SH", this.TOTAL_TEST_SH);
                dicSingleTag.Add("TOTAL_TEST_HH", this.TOTAL_TEST_HH);
                dicSingleTag.Add("TOTAL_TEST_VS", this.TOTAL_TEST_VS);
                dicSingleTag.Add("TEST_SH", this.TEST_SH);
                dicSingleTag.Add("TEST_HH", this.TEST_HH);
                dicSingleTag.Add("TEST_VS", this.TEST_VS);
                dicSingleTag.Add("TOTAL_DIIM_XQ", this.TOTAL_DIIM_XQ);
                dicSingleTag.Add("TOTAL_DIIM_SA", this.TOTAL_DIIM_SA);
                dicSingleTag.Add("TOTAL_DIIM_NS", this.TOTAL_DIIM_NS);
                dicSingleTag.Add("DIIM_XQ", this.DIIM_XQ);
                dicSingleTag.Add("DIIM_SA", this.DIIM_SA);
                dicSingleTag.Add("DIIM_NS", this.DIIM_NS);

                dicSingleTag.Add("OTHER_PROVINCE", this.OTHER_PROVINCE);
                dicSingleTag.Add("DIFF_DATE_REPORT", this.DIFF_DATE_REPORT);
                dicSingleTag.Add("REALY_BED_AMOUNT", this.REALY_BED_AMOUNT);
                dicSingleTag.Add("RESULT_HEAVY", this.RESULT_HEAVY);
                dicSingleTag.Add("TRAN_PATI_AMBULANCE", this.TRAN_PATI_AMBULANCE);

                dicSingleTag.Add("TOTAL_SURG_GROUP_DB", this.TOTAL_SURG_GROUP_DB);
                dicSingleTag.Add("TOTAL_SURG_GROUP_1", this.TOTAL_SURG_GROUP_1);
                dicSingleTag.Add("TOTAL_SURG_GROUP_2", this.TOTAL_SURG_GROUP_2);
                dicSingleTag.Add("TOTAL_SURG_GROUP_3", this.TOTAL_SURG_GROUP_3);
                dicSingleTag.Add("TOTAL_SURG_MICRO", this.TOTAL_SURG_MICRO);
                dicSingleTag.Add("TOTAL_SURG_ENDO", this.TOTAL_SURG_ENDO);
                dicSingleTag.Add("TOTAL_DIIM_CT", this.TOTAL_DIIM_CT);
                dicSingleTag.Add("TOTAL_DIIM_MRI", this.TOTAL_DIIM_MRI);
                dicSingleTag.Add("TOTAL_TEST_GP", this.TOTAL_TEST_GP);
                dicSingleTag.Add("CLS_NEW_COUNT", this.CLS_NEW_COUNT);
                dicSingleTag.Add("CLS_NEW_AMOUNT", this.CLS_NEW_AMOUNT);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
