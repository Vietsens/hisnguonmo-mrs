using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisPatientTypeAlter;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisSereServ; 

using MOS.MANAGER.HisServiceRetyCat; 
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq; 
 

namespace MRS.Processor.Mrs00335
{
    public class Mrs00335Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00335RDO> Mrs00335RDOSereServs = new List<Mrs00335RDO>();
        private List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ> serviceReqs = new List<HIS_SERVICE_REQ>();
        private List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> sereServs = new List<V_HIS_SERE_SERV>(); 
        private List<MOS.EFMODEL.DataModels.HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>(); // nhóm loại báo cáo
        private List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_RETY_CAT> serviceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 

        Mrs00335Filter filter = null; 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        string reportTypeCateNoiSoiTai = "335NST", reportTypeCateNoiSoiMui = "335NSM", reportTypeCateNoiSoiHong = "335NSH", reportTypeCateNoiSoiCoTuCung = "335CTC"; 
        string groupLabelNoiSoiTai, groupLabelNoiSoiMui, groupLabelNoiSoiHong, groupLabelNoiSoiCoTuCung; 
        public Mrs00335Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00335Filter); 
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00335Filter)reportFilter); 
            int start = 0; 
            var result = true; 
            CommonParam param = new CommonParam(); 
            try
            {
                //get REPORT_TYPE_CATE
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery(); 
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00335"; 
                MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager reportTypeCateGet = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param); 
                reportTypeCats = reportTypeCateGet.Get(reportTypeCatFilter); 

                // get SERVICE_RETY_CAT
                HisServiceRetyCatManager serviceRetyCatGet = new HisServiceRetyCatManager(param); 
                int count = reportTypeCats.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    List<long> reportTypeCateLimitIds = reportTypeCats.Select(o => o.ID).Skip(start).Take(limit).ToList(); 
                    HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery(); 
                    serviceRetyCatViewFilter.REPORT_TYPE_CAT_IDs = reportTypeCateLimitIds; 
                    var serviceRetyCatLimits = reportTypeCateLimitIds.Count > 0 ? new HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter) : new List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_RETY_CAT>(); 
                    this.serviceRetyCats.AddRange(serviceRetyCatLimits); 
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }

                // get sereServ

                if (filter.ROOM_IDs != null && filter.ROOM_IDs.Count > 0)
                {
                    int countRooms = filter.ROOM_IDs.Count;
                    int startRooms = 0;
                    while (countRooms > 0)
                    {
                        HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                        serviceReqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM;
                        serviceReqFilter.INTRUCTION_TIME_TO = filter.TIME_TO;
                        serviceReqFilter.FINISH_TIME_FROM = filter.FINISH_TIME_FROM;
                        serviceReqFilter.FINISH_TIME_TO = filter.FINISH_TIME_TO;
                        serviceReqFilter.EXECUTE_ROOM_ID = filter.EXECUTE_ROOM_ID;
                        //sereServFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT; 
                        int limit = (countRooms <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? countRooms : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<long> roomLimitIds = filter.ROOM_IDs.Skip(startRooms).Take(limit).ToList();
                        serviceReqFilter.REQUEST_ROOM_IDs = roomLimitIds; // lấy theo phòng thực hiện
                        var serviceReqSub = roomLimitIds.Count > 0 ? new HisServiceReqManager(param).Get(serviceReqFilter) : new List<HIS_SERVICE_REQ>();
                        serviceReqs.AddRange(serviceReqSub);
                        startRooms += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        countRooms -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
                else
                {
                    HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                    serviceReqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM;
                    serviceReqFilter.INTRUCTION_TIME_TO = filter.TIME_TO;
                    serviceReqFilter.FINISH_TIME_FROM = filter.FINISH_TIME_FROM;
                    serviceReqFilter.FINISH_TIME_TO = filter.FINISH_TIME_TO;
                    serviceReqFilter.EXECUTE_ROOM_ID = filter.EXECUTE_ROOM_ID;
                    serviceReqs = new HisServiceReqManager(param).Get(serviceReqFilter);
                }

                if (serviceReqs != null && serviceReqs.Count > 0)
                {
                    int countReqs = serviceReqs.Count;
                    int startReqs = 0;
                    while (countReqs > 0)
                    {
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                        int limit = (countReqs <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? countReqs : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<long> reqLimitIds = serviceReqs.Select(o=>o.ID).Skip(startReqs).Take(limit).ToList();
                        sereServFilter.SERVICE_REQ_IDs = reqLimitIds;
                        var sereServSub = reqLimitIds.Count > 0 ? new HisSereServManager(param).GetView(sereServFilter) : new List<V_HIS_SERE_SERV>();
                        sereServs.AddRange(sereServSub);
                        startReqs += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        countReqs -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
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
            var result = true; 
            try
            {
                Mrs00335RDOSereServs.Clear(); 
                //var sereServProcess = getSereServFilterPatientTypeAlter(this.sereServs); 
                ProcessRdos(this.sereServs); 
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private List<V_HIS_SERE_SERV> getSereServFilterPatientTypeAlter(List<V_HIS_SERE_SERV> sereServs)
        {
            List<V_HIS_SERE_SERV> result = new List<V_HIS_SERE_SERV>(); 
            try
            {
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatyAlterTreat = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 

                int start = 0; 
                int count = sereServs.Count; 
                if (sereServs == null || sereServs.Count == 0)
                {
                    result = null; 
                }
                else
                {
                    var treatmentIdSss = sereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList(); 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listTreatmentIdLimit = treatmentIdSss.Skip(start).Take(limit).ToList(); 

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patyAlterFilter.TREATMENT_IDs = listTreatmentIdLimit; 
                        patyAlterFilter.TREATMENT_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU; 
                        var listPatyAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter); 

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_PATIENT_TYPE_ALTER: Mrs00335"); 
                        }
                        List<long> listPatyAlterTreatmentInId = new List<long>(); 
                        if (IsNotNullOrEmpty(listPatyAlter))
                        {
                            var Groups = listPatyAlter.GroupBy(o => o.TREATMENT_ID).ToList(); 
                            foreach (var group in Groups)
                            {
                                var listGroup = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().OrderBy(o => o.LOG_TIME).ToList(); 
                                dicCurrentPatyAlter[listGroup.First().TREATMENT_ID] = listGroup.First(); 
                                listPatyAlterTreatmentInId.Add(listGroup.First().TREATMENT_ID); 
                                foreach (var item in listGroup)
                                {
                                    dicPatyAlterTreat[item.TREATMENT_ID] = item; 
                                    break; 
                                }
                            }
                        }

                        var result1 = sereServs.Where(o => listPatyAlterTreatmentInId.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList(); 
                        if (result1 != null && result1.Count > 0)
                        {
                            result.AddRange(result1); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                result = null; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private void ProcessRdos(List<V_HIS_SERE_SERV> sereServs)
        {
            try
            {
                // set label
                foreach (var item in reportTypeCats)
                {
                    if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiTai))
                    {
                        this.groupLabelNoiSoiTai = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiMui))
                    {
                        this.groupLabelNoiSoiMui = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiHong))
                    {
                        this.groupLabelNoiSoiHong = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiCoTuCung))
                    {
                        this.groupLabelNoiSoiCoTuCung = item.CATEGORY_NAME; 
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Khong ton tai REPORT_TYPE_CATE tương ungkd"); 
                    }
                }

                // process detail

                var sereServGroupByRequestDepartment = sereServs.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList(); 
                foreach (var group in sereServGroupByRequestDepartment)
                {
                    var subGroup = group.ToList<V_HIS_SERE_SERV>(); 
                    Mrs00335RDO rdo = new Mrs00335RDO(); 
                    foreach (var item in reportTypeCats)
                    {
                        // set amount
                        var serviceRetyCateProcess = this.serviceRetyCats.Where(o => o.REPORT_TYPE_CAT_ID == item.ID).ToList(); 
                        if (serviceRetyCateProcess != null && serviceRetyCateProcess.Count > 0)
                        {
                            //List<V_HIS_SERE_SERV> sereServGroup = new List<V_HIS_SERE_SERV>(); 
                            var sereServGroupProcess = subGroup.Where(o => serviceRetyCateProcess.Select(p => p.SERVICE_ID).Contains(o.SERVICE_ID)); 
                            
                            if (sereServGroupProcess != null && sereServGroupProcess.Count() > 0)
                            {
                                rdo.REQUEST_DEPARTMENT_ID = sereServGroupProcess.First().TDL_REQUEST_DEPARTMENT_ID; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServGroupProcess.First().REQUEST_DEPARTMENT_NAME; 
                                Inventec.Common.Logging.LogSystem.Info("Phong: " + rdo.REQUEST_DEPARTMENT_NAME); 
                                if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiTai))
                                {

                                    rdo.NSTAI_BHYT_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 

                                    Inventec.Common.Logging.LogSystem.Info("Noi soi tai BHYT: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 

                                    rdo.NSTAI_ND_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi tai ND: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiMui))
                                {
                                    rdo.NSMUI_BHYT_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi mui BHYT: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                    rdo.NSMUI_ND_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi mui ND: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiHong))
                                {
                                    rdo.NSHONG_BHYT_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi hong BHYT: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                    rdo.NSHONG_ND_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi hong ND: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNoiSoiCoTuCung))
                                {
                                    rdo.NSCTC_BHYT_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi co tu cung BHYT: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                    rdo.NSCTC_ND_AMOUNT = sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.AMOUNT); 
                                    Inventec.Common.Logging.LogSystem.Info("Noi soi co tu cung ND: " + string.Join(", ", sereServGroupProcess.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(o => o.TDL_TREATMENT_CODE).ToList())); 
                                }
                                else
                                {
                                    Inventec.Common.Logging.LogSystem.Warn("khong ton tai REPORT_TYPE_CATE"); 
                                }
                            }
                        }
                    }
                    if (rdo.REQUEST_DEPARTMENT_ID > 0)
                    {
                        this.Mrs00335RDOSereServs.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00335Filter)reportFilter).TIME_FROM ?? ((Mrs00335Filter)reportFilter).FINISH_TIME_FROM??0));
            dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00335Filter)reportFilter).TIME_TO ?? ((Mrs00335Filter)reportFilter).FINISH_TIME_TO??0));

            if (((Mrs00335Filter)this.reportFilter).EXECUTE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => ((Mrs00335Filter)this.reportFilter).EXECUTE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }
            dicSingleTag.Add("GROUP_LABEL_NOI_SOI_TAI", groupLabelNoiSoiTai); 
            dicSingleTag.Add("GROUP_LABEL_NOI_SOI_MUI", groupLabelNoiSoiMui); 
            dicSingleTag.Add("GROUP_LABEL_NOI_SOI_HONG", groupLabelNoiSoiHong); 
            dicSingleTag.Add("GROUP_LABEL_NOI_SOI_CO_TU_CUNG", groupLabelNoiSoiCoTuCung); 

            bool exportSuccess = true; 
            objectTag.AddObjectData(store, "Report", this.Mrs00335RDOSereServs); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
