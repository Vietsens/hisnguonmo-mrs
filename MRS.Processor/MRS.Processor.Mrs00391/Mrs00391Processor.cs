using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceReqStt;
using MOS.MANAGER.HisReportTypeCat;
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
using MOS.MANAGER.HisPatientTypeAlter; 

namespace MRS.Processor.Mrs00391
{
    public class Mrs00391Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 

        private List<Mrs00391RDO> Mrs00391RDOSereServs = new List<Mrs00391RDO>(); 
        private List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> sereServs = new List<V_HIS_SERE_SERV>(); 
        private List<long> treatmentIds = new List<long>(); 
        private List<MOS.EFMODEL.DataModels.HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>(); // nhóm loại báo cáo
        private List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_RETY_CAT> serviceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 
        private List<MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        private List<MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER> patientTypeAlterIns = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
         Dictionary<long, V_HIS_SERVICE_REQ> dicServiceSeq = new Dictionary<long,V_HIS_SERVICE_REQ>(); 
        Mrs00391Filter filter = null; 
        string groupLabelSinhHoaNuocTieu, groupLabelHuyetHoc, groupLabelViKhuan; 
        string reportTypeCateNhomSinhHoaNuocTieu = "391SHNT", reportTypeCateHuyetHoc = "391HH", reportTypeCateViKhuan = "391VK"; 
        long countExamServiceReqs = 0; 

        public Mrs00391Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00391Filter); 
        }

        protected override bool GetData()
        {
            filter = ((Mrs00391Filter)reportFilter); 
            int start = 0; 
            var result = true; 
            CommonParam param = new CommonParam(); 
            try
            {
                //get REPORT_TYPE_CATE
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery(); 
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00391"; 
                HisReportTypeCatManager reportTypeCateGet = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param); 
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

                // tinh tong so luot kham trong ngay
                HisServiceReqViewFilterQuery examServiceReqFilter = new HisServiceReqViewFilterQuery(); 
                HisServiceReqManager examServiceReqGet = new HisServiceReqManager(param); 
                examServiceReqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM; 
                examServiceReqFilter.INTRUCTION_TIME_TO = filter.TIME_TO; 
                examServiceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH; 
                var examServiceReqs = examServiceReqGet.GetView(examServiceReqFilter); 
                countExamServiceReqs = examServiceReqs.Count; 

                //yeu cau
                HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery(); 
                if (filter.TRUE_FALSE != null && (Boolean)filter.TRUE_FALSE)
                {
                    reqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM; 
                    reqFilter.INTRUCTION_TIME_TO = filter.TIME_TO; 
                }
                else
                {
                    reqFilter.FINISH_TIME_FROM = filter.TIME_FROM; 
                    reqFilter.FINISH_TIME_TO = filter.TIME_TO;
                }
                var listServiceReq = new HisServiceReqManager().GetView(reqFilter).ToList(); 
              dicServiceSeq = listServiceReq.ToDictionary(o => o.ID); 

                // YC-DV

                var listTreatmentIds = listServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var skip = 0; 
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                        ssFilter.TREATMENT_IDs = limit; 
                        var listSereServSub = new HisSereServManager().GetView(ssFilter); 
                        sereServs.AddRange(listSereServSub); 

 
                    }
                    
                }
                if (IsNotNullOrEmpty(sereServs)) sereServs = sereServs.Where(o => dicServiceSeq.ContainsKey(o.SERVICE_REQ_ID ?? 0)).ToList(); 

                // get patientTypeAlter

                patientTypeAlters = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatmentIds); 
                this.patientTypeAlterIns = patientTypeAlters.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList(); 
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
                Mrs00391RDOSereServs.Clear(); 
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

        private void ProcessRdos(List<V_HIS_SERE_SERV> sereServs)
        {
            try
            {
                // set label
                foreach (var item in reportTypeCats)
                {
                    if (item.CATEGORY_CODE.Equals(this.reportTypeCateNhomSinhHoaNuocTieu))
                    {
                        this.groupLabelSinhHoaNuocTieu = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateViKhuan))
                    {
                        this.groupLabelViKhuan = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateHuyetHoc))
                    {
                        this.groupLabelHuyetHoc = item.CATEGORY_NAME; 
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Khong ton tai REPORT_TYPE_CATE tương ứng"); 
                    }
                }
                var sereServGroupByRequestDepartment = sereServs.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList(); 
                foreach (var group in sereServGroupByRequestDepartment)
                {
                    var subGroup = group.ToList<V_HIS_SERE_SERV>(); 
                    Mrs00391RDO rdo = new Mrs00391RDO(); 
                    foreach (var item in reportTypeCats)
                    {
                        var serviceRetyCateProcess = this.serviceRetyCats.Where(o => o.REPORT_TYPE_CAT_ID == item.ID).ToList(); 
                        if (serviceRetyCateProcess != null && serviceRetyCateProcess.Count > 0)
                        {
                            List<V_HIS_SERE_SERV> sereServGroup = new List<V_HIS_SERE_SERV>(); 
                            var sereServGroupProcess = subGroup.Where(o => serviceRetyCateProcess.Select(p => p.SERVICE_ID).Contains(o.SERVICE_ID)); 
                            foreach (var itemGroup in sereServGroupProcess)
                            {
                                sereServGroup.Add(itemGroup); //Cac dich vu trong nhom subGroup co nhom loai bao cao la item
                            }
                            if (sereServGroup != null && sereServGroup.Count > 0)
                            {
                                List<long> treatmentIds = sereServGroup.Select(o => o.TDL_TREATMENT_ID.Value).Distinct().ToList(); 
                                List<long> treatmentIdIns = new List<long>(); 
                                List<long> treatmentIdOuts = new List<long>(); 
                                List<V_HIS_SERE_SERV> sereServIns = new List<V_HIS_SERE_SERV>(); 
                                List<V_HIS_SERE_SERV> sereServOuts = new List<V_HIS_SERE_SERV>(); 
                                if (patientTypeAlterIns != null && patientTypeAlterIns.Count > 0)
                                {
                                    treatmentIdIns = treatmentIds.Where(o => patientTypeAlterIns.Select(p => p.TREATMENT_ID).Contains(o)).ToList(); 
                                    treatmentIdOuts = treatmentIds.Where(o => !patientTypeAlterIns.Select(p => p.TREATMENT_ID).Contains(o)).ToList(); 
                                }
                                sereServIns = sereServGroup.Where(o => IsNoiTru(req(o), patientTypeAlters)).ToList(); 
                                sereServOuts = sereServGroup.Where(o => !IsNoiTru(req(o), patientTypeAlters)).ToList(); 

                                rdo.REQUEST_DEPARTMENT_ID = sereServGroup.First().TDL_REQUEST_DEPARTMENT_ID; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServGroup.First().REQUEST_DEPARTMENT_NAME; 
                                if (item.CATEGORY_CODE.Equals(this.reportTypeCateNhomSinhHoaNuocTieu))
                                {
                                    rdo.XN_AMOUNT2 = sereServIns.Sum(p => p.AMOUNT); 
                                    rdo.XN_AMOUNT1 = sereServOuts.Sum(p => p.AMOUNT); 
                                    rdo.BN_AMOUNT1 = treatmentIdOuts.Count(); 
                                    rdo.BN_AMOUNT2 = treatmentIdIns.Count(); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateHuyetHoc))
                                {
                                    rdo.XN_AMOUNT4 = sereServIns.Sum(p => p.AMOUNT); 
                                    rdo.XN_AMOUNT3 = sereServOuts.Sum(p => p.AMOUNT); 
                                    rdo.BN_AMOUNT3 = treatmentIdOuts.Count(); 
                                    rdo.BN_AMOUNT4 = treatmentIdIns.Count(); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateViKhuan))
                                {
                                    rdo.XN_AMOUNT6 = sereServIns.Sum(p => p.AMOUNT); 
                                    rdo.XN_AMOUNT5 = sereServOuts.Sum(p => p.AMOUNT); 
                                    rdo.BN_AMOUNT5 = treatmentIdOuts.Count(); 
                                    rdo.BN_AMOUNT6 = treatmentIdIns.Count(); 
                                }
                                else
                                    Inventec.Common.Logging.LogSystem.Warn("KHông tìm thấy report_type_code"); 
                            }
                        }
                    }
                    if (rdo.REQUEST_DEPARTMENT_ID > 0)
                    {
                        this.Mrs00391RDOSereServs.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private V_HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            return dicServiceSeq.ContainsKey(o.SERVICE_REQ_ID ?? 0) ? dicServiceSeq[o.SERVICE_REQ_ID ?? 0] : new V_HIS_SERVICE_REQ(); 
        }

        // lấy log_time sau cùng so với 

        public bool IsNoiTru(V_HIS_SERVICE_REQ req, List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlter)
        {
            try
            {
                bool result = false; 
                List<V_HIS_PATIENT_TYPE_ALTER> subPatientTypeAlter = patientTypeAlter.Where(o => o.TREATMENT_ID == req.TREATMENT_ID).OrderBy(p => p.LOG_TIME).ToList(); 
                var SubPatientTypeAlter = subPatientTypeAlter.Where(o => o.LOG_TIME <= req.INTRUCTION_TIME).ToList(); 
                if (SubPatientTypeAlter.Count > 0)
                {
                    result = (SubPatientTypeAlter.Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU); 
                }
                else
                {
                    var SubPatientTypeAlterGreater = subPatientTypeAlter.Where(o => o.LOG_TIME > req.INTRUCTION_TIME).ToList(); 
                    result = (SubPatientTypeAlter.First().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU); 
                }

                return result; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return false; 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00391Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00391Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00391Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00391Filter)reportFilter).TIME_TO)); 
            }

            if (((Mrs00391Filter)reportFilter).TRUE_FALSE != null && ((Mrs00391Filter)reportFilter).TRUE_FALSE == true)
            {
                dicSingleTag.Add("LABEL_TITLE", "chỉ định"); 
            }
            else
            {
                dicSingleTag.Add("LABEL_TITLE", "trả kết quả"); 
            }
            dicSingleTag.Add("GROUP_LABEL_SINH_HOA_NUOC_TIEU", groupLabelSinhHoaNuocTieu); 
            dicSingleTag.Add("GROUP_LABEL_HUYET_HOC", groupLabelHuyetHoc); 
            dicSingleTag.Add("GROUP_LABEL_VI_KHUAN", groupLabelViKhuan); 
            dicSingleTag.Add("TONG_SO_KHAM_BENH", countExamServiceReqs); 

            bool exportSuccess = true; 
            objectTag.AddObjectData(store, "Report", this.Mrs00391RDOSereServs); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
