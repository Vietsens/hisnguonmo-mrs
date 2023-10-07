using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00556;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceRetyCat;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServPttt;

namespace MRS.Processor.Mrs00556
{
    public class Mrs00556Processor : AbstractProcessor
    {
        private List<Mrs00556RDO> ListRdoDepartment = new List<Mrs00556RDO>();
        private List<Mrs00556RDO> ListRdoService = new List<Mrs00556RDO>();
        private List<Mrs00556RDO> ListRdoTreatmentRequestRoom = new List<Mrs00556RDO>();
        private List<Mrs00556RDO> ListRdoTreatmentExecuteRoom = new List<Mrs00556RDO>();
        private List<Mrs00556RDO> ListRdoTreatmentBhytEndRoom = new List<Mrs00556RDO>();
        //private List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();
        Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety = new Dictionary<long, List<HIS_SERVICE_METY>>();
        Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty = new Dictionary<long, List<HIS_SERVICE_MATY>>();
        Mrs00556Filter filter = null;
        //List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCategory = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        List<HIS_EXECUTE_ROOM> listHisRoom = new List<HIS_EXECUTE_ROOM>();
        string thisReportTypeCode = "";

        List<Mrs00556RDO> ListRdo = new List<Mrs00556RDO>();

        //List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        //List<HIS_SERE_SERV_PTTT> listPTTT = new List<HIS_SERE_SERV_PTTT>();
        //List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        long PatientTypeIdBhyt = 0;

        public Mrs00556Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00556Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00556Filter)this.reportFilter;
            try
            {
                PatientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                //Danh sach phong
                HisExecuteRoomFilterQuery listHisRoomfilter = new HisExecuteRoomFilterQuery();
                listHisRoom = new HisExecuteRoomManager(new CommonParam()).Get(listHisRoomfilter);
                Inventec.Common.Logging.LogSystem.Info("listHisRoom" + listHisRoom.Count);

                ListRdo = new ManagerSql().GetRdo(this.filter);
                Inventec.Common.Logging.LogSystem.Info("listHisSereServ" + ListRdo.Count);

                if (filter.IS_FEE_OF_BHYT == true)
                {
                    ListRdo = ListRdo.Where(o => o.PATIENT_TYPE_ID != PatientTypeIdBhyt || (o.HEIN_LIMIT_PRICE.HasValue && o.PRICE > o.HEIN_LIMIT_PRICE.Value)).ToList();
                }
                
                //Dinh muc thuoc hao phi
                List<HIS_SERVICE_METY> listServiceMety = new List<HIS_SERVICE_METY>();
                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery();
                listServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);
                //listServiceReq = new HisServiceReqManager().Get(new HisServiceReqFilterQuery());

                //neu la kham thi lay phong chi dinh la phong thuc hien
                foreach (var item in ListRdo)
                {
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                        item.REQUEST_DEPARTMENT_CODE = item.EXECUTE_DEPARTMENT_CODE;
                        item.REQUEST_ROOM_CODE = item.EXECUTE_ROOM_CODE;
                        item.REQUEST_DEPARTMENT_NAME = item.EXECUTE_DEPARTMENT_NAME;
                        item.REQUEST_ROOM_NAME = item.EXECUTE_ROOM_NAME;
                        if (item.TDL_FIRST_EXAM_ROOM_ID == null)
                        {
                            item.TDL_FIRST_EXAM_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                        }
                    }
                    
                }

                foreach (var item in listServiceMety)
                {
                    if (!dicServiceMety.ContainsKey(item.SERVICE_ID)) dicServiceMety[item.SERVICE_ID] = new List<HIS_SERVICE_METY>();
                    dicServiceMety[item.SERVICE_ID].Add(item);
                }

                //Dinh muc vat tu hao phi
                List<HIS_SERVICE_MATY> listServiceMaty = new List<HIS_SERVICE_MATY>();
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery();
                listServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);
                foreach (var item in listServiceMaty)
                {
                    if (!dicServiceMaty.ContainsKey(item.SERVICE_ID)) dicServiceMaty[item.SERVICE_ID] = new List<HIS_SERVICE_MATY>();
                    dicServiceMaty[item.SERVICE_ID].Add(item);
                }

                //Inventec.Common.Logging.LogSystem.Info("listServiceMaty" + listServiceMaty.Count);
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatViewfilter = new HisServiceRetyCatViewFilterQuery();
                HisServiceRetyCatViewfilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
                var listServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatViewfilter)?? new List<V_HIS_SERVICE_RETY_CAT>();
                dicCategory = listServiceRetyCat.GroupBy(g => g.SERVICE_ID).ToDictionary(p=>p.Key,q=>q.First());

                //Inventec.Common.Logging.LogSystem.Info("listServiceRetyCat" + listServiceRetyCat.Count);

                //var categori_name = listServiceRetyCat.Select(s => new { s.CATEGORY_NAME, s.REPORT_TYPE_CODE }).Distinct().ToList();
                //LogSystem.Debug(LogUtil.TraceData("categori_name", categori_name));
                //LogSystem.Debug(LogUtil.TraceData("HisServiceRetyCatViewfilter", HisServiceRetyCatViewfilter));

                HisServiceFilterQuery HisServicefilter = new HisServiceFilterQuery();
                listHisService = new HisServiceManager().Get(HisServicefilter);
                //listPTTT = new HisSereServPtttManager().Get(new HisSereServPtttFilterQuery());

                //lay loc theo khoa
                if (filter.DEPARTMENT_ID != null)
                {
                    ListRdo = ListRdo.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
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
                //lay danh sach duy nhat
                ListRdo = ListRdo.GroupBy(o => new { o.ID,o.TRANSACTION_TIME }).Select(p => p.First()).ToList();
                foreach (var item in ListRdo)
                {
                    SetExtendField(item,dicServiceMety, dicServiceMaty, this.filter, listHisRoom, listHisService);
                }

                if (filter.EXAM_ROOM_ID != null)
                    ListRdo = ListRdo.Where(o => filter.EXAM_ROOM_ID + 2000000 == o.DEPARTMENT_ROOM_ID).ToList();
                if (filter.EXAM_ROOM_IDs != null)
                    ListRdo = ListRdo.Where(o => filter.EXAM_ROOM_IDs.Exists(p=>p+ 2000000 == o.DEPARTMENT_ROOM_ID)).ToList();
                if (filter.DEPARTMENT_ID != null)
                    ListRdo = ListRdo.Where(o => filter.DEPARTMENT_ID == o.DEPARTMENT_ROOM_ID).ToList();
                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                ListRdo = ListRdo.OrderByDescending(o => o.ROOM_NUM_ORDER).ToList();
                GroupByService();
                GroupByTreatmentRequestRoom();
                GroupByTreatmentExecuteRoom();
                AddInfoExecuteRoomAndBhytEndRoom();
                GroupByRequestDepartmentID();
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SS"].ToString()))
                {
                    GroupByKey(this.dicDataFilter["KEY_GROUP_SS"].ToString());
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00556RDO>();
                result = false;
            }
            return result;
        }
        private void SetExtendField(Mrs00556RDO rdo,Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety, Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty, Mrs00556Filter mrs00556Filter, List<HIS_EXECUTE_ROOM> listHistRoom, List<HIS_SERVICE> listHisService)
        {

            //thông tin nhóm báo cáo
            if (dicCategory.ContainsKey(rdo.SERVICE_ID))
            {
                rdo.CATEGORY_CODE = dicCategory[rdo.SERVICE_ID].CATEGORY_CODE;
                rdo.CATEGORY_NAME = dicCategory[rdo.SERVICE_ID].CATEGORY_NAME;
                rdo.CATEGORY_NUM_ORDER = dicCategory[rdo.SERVICE_ID].NUM_ORDER;
            }
            #region Contructor object
            var quotaExpend = dicServiceMety.ContainsKey(rdo.SERVICE_ID) ? dicServiceMety[rdo.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : (dicServiceMaty.ContainsKey(rdo.SERVICE_ID) ? dicServiceMaty[rdo.SERVICE_ID].Sum(o => o.EXPEND_AMOUNT * o.EXPEND_PRICE ?? 0) : 0);
            //var pttt = listPTTT.FirstOrDefault(p => p.SERE_SERV_ID == rdo.ID);
            //var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == rdo.SERVICE_REQ_ID);
            var ecgServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__ECG;
            var eegServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__EEG;
            var brainBloodServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__BRAIN_BLOOD;
            var boneDensityServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__BONE_DENSITY;
            var cervicalEndoServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__CERVICAL_ENDO;
            var SHServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__SH;
            var VSServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__VS;
            var HHServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__HH;
            var NTServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__NT;
            var NSTHServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__NSTH;
            var CLVTServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__CLVT;
            var XQServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__XQ;
            var CNHHServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__CNHH;
            var GYCServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__GYC;
            var CLVTYCServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__CLVTYC;
            var KYCServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__KYC;
            var XNGXServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__XNGX;
            var HIVServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__HIV;
            var TrMServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__TrM;
            var XQKTSServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__XQKTS;
            var SAGServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__SAG;
            var digestServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__DIGEST_ENDO;
            var TMHServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__TMH_ENDO;
            var DTDServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__DTD;
            var DCDServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__DCD;
            var ABIServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__ABI;
            var DTDGSServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__DTDGS;
            var TNTServiceIds = rdo.CATEGORY_CODE == mrs00556Filter.CATEGORY_CODE__TNT;
            
            #endregion
            //thông tin người thực hiện

            if (rdo.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
            {
                if(filter.EXAM_REQ_USER_IS_NOT_EXAM_EXE_USER == true)
                {
                    rdo.TDL_REQUEST_USERNAME = rdo.TDL_REQUEST_USERNAME;
                    rdo.TDL_REQUEST_LOGINNAME = rdo.TDL_REQUEST_LOGINNAME;
                }
                else
                {
                    rdo.TDL_REQUEST_USERNAME = rdo.TDL_EXECUTE_USERNAME;
                    rdo.TDL_REQUEST_LOGINNAME = rdo.TDL_EXECUTE_LOGINNAME;
                }
                
            }

            //xử lí lại phòng và diện điều trị của sereServ

            var requestRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_REQUEST_ROOM_ID);
            if (requestRoom != null && requestRoom.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD)
            {
                var firstExamRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.TDL_FIRST_EXAM_ROOM_ID);
                if (firstExamRoom != null)
                {
                    rdo.TDL_REQUEST_ROOM_ID = firstExamRoom.ID;
                    rdo.REQUEST_ROOM_CODE = firstExamRoom.ROOM_CODE;
                    rdo.REQUEST_ROOM_NAME = firstExamRoom.ROOM_NAME;
                }
            }
            if (requestRoom != null && requestRoom.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                rdo.TDL_TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
            }
            var room = listHistRoom.FirstOrDefault(o => o.ROOM_ID == rdo.TDL_REQUEST_ROOM_ID) ?? new HIS_EXECUTE_ROOM();

            if (room.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                rdo.ROOM_NUM_ORDER = room.NUM_ORDER ?? 0;
                rdo.DEPARTMENT_ROOM_ID = room.ROOM_ID + 2000000;
                rdo.DEPARTMENT_ROOM_CODE = room.EXECUTE_ROOM_CODE;
                rdo.DEPARTMENT_ROOM_NAME = room.EXECUTE_ROOM_NAME;
            }
            else
            {
                rdo.DEPARTMENT_ROOM_ID = rdo.TDL_REQUEST_DEPARTMENT_ID;
                rdo.DEPARTMENT_ROOM_CODE = rdo.REQUEST_DEPARTMENT_CODE;
                rdo.DEPARTMENT_ROOM_NAME = rdo.REQUEST_DEPARTMENT_NAME;
                rdo.ROOM_NUM_ORDER = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.DEPARTMENT_CODE == rdo.REQUEST_DEPARTMENT_CODE) ?? new HIS_DEPARTMENT()).NUM_ORDER ?? 1000;
            }
            #region Contructor field
            var ToTalPatientPrice = rdo.VIR_TOTAL_PRICE ?? 0;
            if (rdo.HEIN_LIMIT_PRICE.HasValue && rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && rdo.PRICE > rdo.HEIN_LIMIT_PRICE.Value)
            {
                ToTalPatientPrice = rdo.AMOUNT * (rdo.PRICE - (rdo.HEIN_LIMIT_PRICE.Value));
            }

            rdo.TOTAL_PRICE = ToTalPatientPrice;
            rdo.VIR_TOTAL_PATIENT_PRICE_FEE = rdo.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT ? 0 : rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
            rdo.VIR_TOTAL_PATIENT_PRICE_XHH = rdo.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT ? rdo.VIR_TOTAL_PATIENT_PRICE ?? 0 : 0;
            if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
            {
                rdo.TOTAL_PRICE_PATIENT_BHYT = rdo.VIR_TOTAL_PATIENT_PRICE ?? 0;
            }
            #endregion

            #region process with issue
            switch (rdo.TDL_SERVICE_TYPE_ID)
            {
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN:
                    rdo.TEIN_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_TEIN = rdo.AMOUNT;
                    rdo.TEIN_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    rdo.DIFF_TEIN_TOTAL_PRICE = rdo.TEIN_TOTAL_PRICE - rdo.TEIN_QUOTA_EXPEND_TOTAL_PRICE;
                    if (XNGXServiceIds)
                    {
                        rdo.XNGX_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_XNGX = rdo.AMOUNT;
                        rdo.XNGX_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_XNGX_TOTAL_PRICE = rdo.XNGX_TOTAL_PRICE - rdo.XNGX_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (SHServiceIds)
                    {
                        rdo.SH_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_SH = rdo.AMOUNT;
                        rdo.SH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_SH_TOTAL_PRICE = rdo.SH_TOTAL_PRICE - rdo.SH_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (VSServiceIds)
                    {
                        rdo.VS_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_VS = rdo.AMOUNT;
                        rdo.VS_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_VS_TOTAL_PRICE = rdo.VS_TOTAL_PRICE - rdo.VS_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (HHServiceIds)
                    {
                        rdo.HH_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_HH = rdo.AMOUNT;
                        rdo.HH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_HH_TOTAL_PRICE = rdo.HH_TOTAL_PRICE - rdo.HH_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (NTServiceIds)
                    {
                        rdo.NT_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_NT = rdo.AMOUNT;
                        rdo.NT_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_NT_TOTAL_PRICE = rdo.NT_TOTAL_PRICE - rdo.NT_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (HIVServiceIds)
                    {
                        this._AM_HIV = rdo.AMOUNT;
                    }
                    else if (TrMServiceIds)
                    {
                        this._AM_TrM = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA:
                    if (CLVTYCServiceIds)
                    {
                        rdo.CLVTYC_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_CLVTYC = rdo.AMOUNT;
                        rdo.CLVTYC_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_CLVTYC_TOTAL_PRICE = rdo.CLVTYC_TOTAL_PRICE - rdo.CLVTYC_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (CLVTServiceIds)
                    {
                        rdo.CLVT_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_CLVT = rdo.AMOUNT;
                        rdo.CLVT_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_CLVT_TOTAL_PRICE = rdo.CLVT_TOTAL_PRICE - rdo.CLVT_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (XQServiceIds)
                    {
                        rdo.XQ_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_XQ = rdo.AMOUNT;
                        rdo.XQ_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_XQ_TOTAL_PRICE = rdo.XQ_TOTAL_PRICE - rdo.XQ_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (XQKTSServiceIds)
                    {
                        this._AM_XQ_KTS = rdo.AMOUNT;
                    }
                    
                    else
                    {
                        rdo.DIIM_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_DIIM = rdo.AMOUNT;
                        rdo.DIIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_DIIM_TOTAL_PRICE = rdo.DIIM_TOTAL_PRICE - rdo.DIIM_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA:
                    rdo.SUIM_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_SUIM = rdo.AMOUNT;
                    rdo.SUIM_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                    rdo.DIFF_SUIM_TOTAL_PRICE = rdo.SUIM_TOTAL_PRICE - rdo.SUIM_QUOTA_EXPEND_TOTAL_PRICE;
                    if (SAGServiceIds)
                    {
                        this._AM_SAG = rdo.AMOUNT;
                    }
                    else
                    {
                        this._AM_KHAC = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN:
                    if (ecgServiceIds)
                    {
                        rdo.ECG_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_ECG = rdo.AMOUNT;
                        rdo.ECG_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_ECG_TOTAL_PRICE = rdo.ECG_TOTAL_PRICE - rdo.ECG_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (eegServiceIds)
                    {
                        rdo.EEG_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_EEG = rdo.AMOUNT;
                        rdo.EEG_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_EEG_TOTAL_PRICE = rdo.EEG_TOTAL_PRICE - rdo.EEG_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (brainBloodServiceIds)
                    {
                        rdo.BRAIN_BLOOD_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_BRAIN_BLOOD = rdo.AMOUNT;
                        rdo.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_BRAIN_BLOOD_TOTAL_PRICE = rdo.BRAIN_BLOOD_TOTAL_PRICE - rdo.BRAIN_BLOOD_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (boneDensityServiceIds)
                    {
                        rdo.BONE_DENSITY_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_BONE_DENSITY = rdo.AMOUNT;
                        rdo.BONE_DENSITY_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_BONE_DENSITY_TOTAL_PRICE = rdo.BONE_DENSITY_TOTAL_PRICE - rdo.BONE_DENSITY_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (CNHHServiceIds)
                    {
                        rdo.CNHH_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_CNHH = rdo.AMOUNT;
                        rdo.CNHH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.CNHH_DENSITY_TOTAL_PRICE = rdo.CNHH_TOTAL_PRICE - rdo.CNHH_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (DTDGSServiceIds)
                    {
                        this._AM_DTDGS = rdo.AMOUNT;
                    }
                    else if (DTDServiceIds)
                    {
                        this._AM_DTD = rdo.AMOUNT;
                    }
                    else if (DCDServiceIds)
                    {
                        this._AM_DCD = rdo.AMOUNT;
                    }
                    else if (ABIServiceIds)
                    {
                        this._AM_ABI = rdo.AMOUNT;
                    }
                    else if (TNTServiceIds)
                    {
                        this._AM_TNT = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.OTHER_FUEX_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_OTHER_FUEX = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS:
                    if (cervicalEndoServiceIds)
                    {
                        rdo.CERVICAL_ENDO_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_CERVICAL_ENDO = rdo.AMOUNT;
                        rdo.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_CERVICAL_ENDO_TOTAL_PRICE = rdo.CERVICAL_ENDO_TOTAL_PRICE - rdo.CERVICAL_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (NSTHServiceIds)
                    {
                        rdo.NSTH_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_NSTH = rdo.AMOUNT;
                        rdo.NSTH_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_NSTH_TOTAL_PRICE = rdo.NSTH_TOTAL_PRICE - rdo.NSTH_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    else if (digestServiceIds)
                    {
                        this._AM_DIGEST_ENDO = rdo.AMOUNT;
                    }
                    else if (TMHServiceIds)
                    {
                        this._AM_TMH = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.OTHER_ENDO_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_OTHER_ENDO = rdo.AMOUNT;
                        rdo.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE = quotaExpend;
                        rdo.DIFF_OTHER_ENDO_TOTAL_PRICE = rdo.OTHER_ENDO_TOTAL_PRICE - rdo.OTHER_ENDO_QUOTA_EXPEND_TOTAL_PRICE;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT:
                    rdo.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_PTTT = rdo.AMOUNT;
                    rdo.PT_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_PT = rdo.AMOUNT;
                    if (rdo.PTTT_GROUP_ID != null)
                    {
                        if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                            this._AM_PT_1 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                            this._AM_PT_2 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                            this._AM_PT_3 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                            this._AM_PT_DB = rdo.AMOUNT;
                        else
                            this._AM_PT_NONE = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT:
                    rdo.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_PTTT = rdo.AMOUNT;
                    rdo.TT_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_TT = rdo.AMOUNT;
                    if (rdo.PTTT_GROUP_ID != null)
                    {
                        if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                            this._AM_TT_1 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                            this._AM_TT_2 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                            this._AM_TT_3 = rdo.AMOUNT;
                        else if (rdo.PTTT_GROUP_ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                            this._AM_TT_DB = rdo.AMOUNT;
                        else
                            this._AM_TT_NONE = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G:
                    if (GYCServiceIds)
                    {
                        rdo.GYC_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_GYC = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.BED_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_BED = rdo.AMOUNT;
                        if (rdo.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL || rdo.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                        {
                            this._AM_BED_USED = rdo.AMOUNT;
                        }
                    }

                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH:
                    if (mrs00556Filter.SERVICE_CODE__KHAMSKs != null && mrs00556Filter.SERVICE_CODE__KHAMSKs.Contains(rdo.TDL_SERVICE_CODE))
                    {
                        rdo.EXAM_KSK_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_EXAM_KSK = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.EXAM_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_EXAM = rdo.AMOUNT;
                        if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            if (!string.IsNullOrWhiteSpace(rdo.TDL_HEIN_CARD_NUMBER) && rdo.TDL_HEIN_CARD_NUMBER.Substring(0, 2) == "CA")
                            {
                                this._AM_EXAM_BHYT_CA = rdo.AMOUNT;
                            }
                            this._AM_EXAM_BHYT = rdo.AMOUNT;
                        }
                        else
                        {
                            this._AM_EXAM_DVYT = rdo.AMOUNT;
                        }
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC:
                    if (rdo.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        rdo.EXPEND_MEDICINE_TOTAL_PRICE = rdo.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        rdo._AM_EXPEND_MEDICINE = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.MEDICINE_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_MEDICINE = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT:
                    if (rdo.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        rdo.EXPEND_MATERIAL_TOTAL_PRICE = rdo.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                        rdo._AM_EXPEND_MATERIAL = rdo.AMOUNT;
                    }
                    else
                    {
                        rdo.MATERIAL_TOTAL_PRICE = ToTalPatientPrice;
                        rdo._AM_MATERIAL = rdo.AMOUNT;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU:
                    rdo.BLOOD_TOTAL_PRICE = ToTalPatientPrice;
                    rdo._AM_BLOOD = rdo.AMOUNT;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC:
                    rdo.TRANS_TOTAL_PRICE_NEW = ToTalPatientPrice;
                    rdo._AM_TRANS_NEW = rdo.AMOUNT;
                    break;
            }
            if (KYCServiceIds)
            {
                rdo.KYC_TOTAL_PRICE = ToTalPatientPrice;
                rdo._AM_KYC = rdo.AMOUNT;
            }
            
            if (mrs00556Filter.SERVICE_CODE__TRANs != null && mrs00556Filter.SERVICE_CODE__TRANs.Contains(rdo.TDL_SERVICE_CODE))
            {
                rdo.TRANS_TOTAL_PRICE = ToTalPatientPrice;
                rdo._AM_TRANS = rdo.AMOUNT;
            }

            if (mrs00556Filter.SERVICE_CODE__Z != null && mrs00556Filter.SERVICE_CODE__Z == rdo.TDL_SERVICE_CODE)
            {
                rdo.Z_TOTAL_PRICE = ToTalPatientPrice;
                rdo.Z = rdo.AMOUNT;
            }

            if (mrs00556Filter.SERVICE_CODE__P != null && mrs00556Filter.SERVICE_CODE__P == rdo.TDL_SERVICE_CODE)
            {
                rdo.P_TOTAL_PRICE = ToTalPatientPrice;
                rdo.P = rdo.AMOUNT;
            }

            if (mrs00556Filter.SERVICE_CODE__GKSK != null && mrs00556Filter.SERVICE_CODE__GKSK == rdo.TDL_SERVICE_CODE)
            {
                rdo.GKSK_TOTAL_PRICE = ToTalPatientPrice;
                rdo.GKSK = rdo.AMOUNT;
            }

            rdo.DIFF_TOTAL_PRICE = rdo.TOTAL_PRICE - quotaExpend - rdo.MEDICINE_TOTAL_PRICE - rdo.EXPEND_MEDICINE_TOTAL_PRICE - rdo.MATERIAL_TOTAL_PRICE - rdo.EXPEND_MATERIAL_TOTAL_PRICE;
            #endregion
            //nhom bao cao
            CategoryCode(rdo,rdo.SERVICE_ID, listHisService);
            //dich vu cha

            if (listHisService.Count > 0)
            {
                ParentCode(rdo,rdo.SERVICE_ID, listHisService);
            }
        }

        private void CategoryCode(Mrs00556RDO rdo,long serviceId, List<HIS_SERVICE> listHisService)
        {
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
                if (service != null)
                {
                    if (!string.IsNullOrEmpty(rdo.CATEGORY_CODE))
                    {

                    }
                    else
                    {
                        rdo.CATEGORY_NUM_ORDER = 1000 + (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).NUM_ORDER;
                        rdo.CATEGORY_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        rdo.CATEGORY_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ParentCode(Mrs00556RDO rdo, long serviceId, List<HIS_SERVICE> listHisService)
        {
            try
            {
                var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
                if (service != null)
                {
                    var parent = listHisService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                    if (parent != null)
                    {
                        rdo.PARENT_NUM_ORDER = parent.NUM_ORDER;
                        rdo.PARENT_SERVICE_CODE = parent.SERVICE_CODE;
                        rdo.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                    }
                    else
                    {
                        rdo.PARENT_NUM_ORDER = 1000 + (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).NUM_ORDER;
                        rdo.PARENT_SERVICE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                        rdo.PARENT_SERVICE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void GroupByKey(string key)
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => string.Format(key, o.SERVICE_ID, o.TDL_SERVICE_TYPE_ID, o.TDL_REQUEST_DEPARTMENT_ID, o.TDL_REQUEST_ROOM_ID, o.TREATMENT_CODE, o.PATIENT_TYPE_CODE, o.DEPARTMENT_ROOM_ID,o.CATEGORY_CODE,o.PARENT_SERVICE_CODE,o.VIR_PATIENT_PRICE)).ToList();
                ListRdo.Clear();
                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                decimal sum = 0;
                Mrs00556RDO rdo;
                List<Mrs00556RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00556RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00556RDO();
                    listSub = item.ToList<Mrs00556RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT") || field.Name.Contains("_AM_"))
                        {
                            //trong V_HIS_SERE_SERV thêm AMOUNT_TEMP
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s) ?? 0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    rdo.COUNT_TREATMENT = item.Select(o => o.TREATMENT_CODE).Distinct().Count();
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByService()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.SERVICE_ID, o.IS_EXPEND }).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                decimal sum = 0;
                Mrs00556RDO rdo;
                List<Mrs00556RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00556RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00556RDO();
                    listSub = item.ToList<Mrs00556RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT") || field.Name.Contains("_AM_"))
                        {
                            //trong V_HIS_SERE_SERV thêm AMOUNT_TEMP
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s) ?? 0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }

                    if (!hide) ListRdoService.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByTreatmentRequestRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.TDL_REQUEST_ROOM_ID }).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                decimal? sum = 0;
                Mrs00556RDO rdo;
                List<Mrs00556RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00556RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00556RDO();
                    listSub = item.ToList<Mrs00556RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s)??0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }

                    if (!hide) ListRdoTreatmentRequestRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByTreatmentExecuteRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.TDL_EXECUTE_ROOM_ID }).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                decimal sum = 0;
                Mrs00556RDO rdo;
                List<Mrs00556RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00556RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00556RDO();
                    listSub = item.ToList<Mrs00556RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s)??0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }

                    if (!hide) ListRdoTreatmentExecuteRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }


        private void GroupByRequestDepartmentID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_REQUEST_DEPARTMENT_ID }).ToList();

                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                decimal sum = 0;
                Mrs00556RDO rdo;
                List<Mrs00556RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00556RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00556RDO();
                    listSub = item.ToList<Mrs00556RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("_AM_"))
                        {
                            sum = listSub.Sum(s => (decimal?)field.GetValue(s)??0);
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    rdo.COUNT_TREATMENT = item.Select(o => o.TREATMENT_CODE).Distinct().Count();
                    if (!hide) ListRdoDepartment.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00556RDO IsMeaningful(List<Mrs00556RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00556RDO();
        }

        private void AddInfoExecuteRoomAndBhytEndRoom()
        {
            try
            {
                foreach (var item in ListRdo)
                {
                    item.COUNT_TREATMENT_EXECUTE_ROOM = ListRdoTreatmentExecuteRoom.Where(o => o.TDL_EXECUTE_ROOM_ID == item.TDL_REQUEST_ROOM_ID).ToList().Count;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("TREATMENT_TYPE_NAME", (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == filter.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME);
            dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == filter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportTreatment", ListRdoTreatmentRequestRoom);
            objectTag.AddObjectData(store, "ReportServiceMedicine", ListRdoService.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
            objectTag.AddObjectData(store, "ReportDepartment", ListRdoDepartment);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
            var NoiTruKhoa = ListRdoTreatmentRequestRoom.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.REQUEST_DEPARTMENT_CODE).Select(q => q.First()).OrderBy(o => DepartmentNumOrder(o.REQUEST_DEPARTMENT_CODE)).ToList();
            objectTag.AddObjectData(store, "NoiTruKhoa", NoiTruKhoa);
            var NgoaiTruPhong = ListRdoTreatmentRequestRoom.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.REQUEST_ROOM_CODE).Select(q => q.First()).OrderBy(o => o.REQUEST_ROOM_NAME).ToList();
            objectTag.AddObjectData(store, "NgoaiTruPhong", NgoaiTruPhong);
            objectTag.AddRelationship(store, "NoiTruKhoa", "ReportTreatment", "REQUEST_DEPARTMENT_CODE", "REQUEST_DEPARTMENT_CODE");
            objectTag.AddRelationship(store, "NgoaiTruPhong", "ReportTreatment", "REQUEST_ROOM_CODE", "REQUEST_ROOM_CODE");
            var listServ = ListRdo.GroupBy(o => o.PARENT_SERVICE_CODE).Select(p => p.First()).OrderBy(q => q.PARENT_NUM_ORDER).ToList();
            for (int i = 0; i < listServ.Count; i++)
            {
                dicSingleTag.Add(string.Format("PARENT_SERVICE_CODE__{0}", i + 1), listServ[i].PARENT_SERVICE_CODE);
                dicSingleTag.Add(string.Format("PARENT_SERVICE_NAME__{0}", i + 1), listServ[i].PARENT_SERVICE_NAME);
            }
            var listCategory = ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).OrderBy(q => q.CATEGORY_NUM_ORDER).ToList();
            for (int i = 0; i < listCategory.Count; i++)
            {
                dicSingleTag.Add(string.Format("CATEGORY_CODE__{0}", i + 1), listCategory[i].CATEGORY_CODE);
                dicSingleTag.Add(string.Format("CATEGORY_NAME__{0}", i + 1), listCategory[i].CATEGORY_NAME);
            }
            var reportNoiTru = ListRdo.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.REQUEST_DEPARTMENT_CODE).Select(q => q.First()).OrderBy(o => DepartmentNumOrder(o.REQUEST_DEPARTMENT_CODE)).ToList();
            objectTag.AddObjectData(store, "ReportNoiTruKhoa", reportNoiTru);
            var reportNgoaiTru = ListRdo.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(p => p.REQUEST_ROOM_CODE).Select(q => q.First()).OrderBy(o => o.REQUEST_ROOM_NAME).ToList();
            objectTag.AddObjectData(store, "ReportNgoaiTruKhoa", reportNgoaiTru);
        }


        private long DepartmentNumOrder(string departmentCode)
        {
            return (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.DEPARTMENT_CODE == departmentCode) ?? new HIS_DEPARTMENT()).NUM_ORDER ?? 1000;
        }

        public decimal _AM_TNT { get; set; }

        public decimal _AM_ABI { get; set; }

        public decimal _AM_BED_USED { get; set; }

        public decimal _AM_EXAM_DVYT { get; set; }

        public decimal _AM_EXAM_BHYT { get; set; }

        public decimal _AM_EXAM_BHYT_CA { get; set; }

        public decimal _AM_TT_NONE { get; set; }

        public decimal _AM_TT_DB { get; set; }

        public decimal _AM_TT_3 { get; set; }

        public decimal _AM_TT_2 { get; set; }

        public decimal _AM_TT_1 { get; set; }

        public decimal _AM_PT_NONE { get; set; }

        public decimal _AM_PT_DB { get; set; }

        public decimal _AM_PT_3 { get; set; }

        public decimal _AM_PT_2 { get; set; }

        public decimal _AM_PT_1 { get; set; }

        public decimal _AM_TMH { get; set; }

        public decimal _AM_DIGEST_ENDO { get; set; }

        public decimal _AM_DCD { get; set; }

        public decimal _AM_DTD { get; set; }

        public decimal _AM_DTDGS { get; set; }

        public decimal _AM_KHAC { get; set; }

        public decimal _AM_SAG { get; set; }

        public decimal _AM_XQ_KTS { get; set; }

        public decimal _AM_TrM { get; set; }

        public decimal _AM_HIV { get; set; }
    }
}
