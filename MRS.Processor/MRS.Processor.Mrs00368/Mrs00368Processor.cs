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
 

namespace MRS.Processor.Mrs00368
{
    public class Mrs00368Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 

        private List<Mrs00368RDO> mrs00368RDOSereServs = new List<Mrs00368RDO>(); 
        private List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> sereServs = new List<V_HIS_SERE_SERV>(); 
        private List<long> treatmentIds = new List<long>(); 
        private List<MOS.EFMODEL.DataModels.HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>(); // nhóm loại báo cáo
        private List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_RETY_CAT> serviceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 

        Mrs00368Filter filter = null; 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        string groupLabelNhomTestKhac, groupLabelZung, groupLabelRaven, groupLabelBeck, groupLabelSieuAm, groupLabelSieuAmDropperXuyenSo, groupLabelDienTim, groupLabelLuuHuyetNao, groupLabelDienNao; 
        string reportTypeCateNhomTestKhac = "368OTHER", reportTypeCateZung = "368Z", reportTypeCateRaven = "368RV", reportTypeCateBeck = "368B", reportTypeCateSieuAm = "368SA", reportTypeCateSieuAmDropperXuyenSo = "368SAD", reportTypeCateDienTim = "368DT", reportTypeCateLuuHuyetNao = "368LHN", reportTypeCateDienNao = "368DN"; 

        public Mrs00368Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00368Filter); 
        }

        protected override bool GetData()
        {
            filter = ((Mrs00368Filter)reportFilter); 
            int start = 0; 
            var result = true; 
            CommonParam param = new CommonParam(); 
            try
            {
                //get REPORT_TYPE_CATE
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery(); 
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00368"; 
                MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager reportTypeCateGet = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param); 
                reportTypeCats = reportTypeCateGet.Get(reportTypeCatFilter); 

                // get SERVICE_RETY_CAT
                MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager serviceRetyCatGet = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param); 
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
                MOS.MANAGER.HisSereServ.HisSereServManager sereServGet = new HisSereServManager(param); 
                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                sereServFilter.INTRUCTION_DATE_FROM = filter.TIME_FROM; 
                sereServFilter.INTRUCTION_DATE_TO = filter.TIME_TO; 
                sereServs = sereServGet.GetView(sereServFilter); 
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
                mrs00368RDOSereServs.Clear(); 
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
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_PATIENT_TYPE_ALTER: MRS00368"); 
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
                    if (item.CATEGORY_CODE.Equals(this.reportTypeCateBeck))
                    {
                        this.groupLabelBeck= item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateDienNao))
                    {
                        this.groupLabelDienNao = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateDienTim))
                    {
                        this.groupLabelDienTim = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateLuuHuyetNao))
                    {
                        this.groupLabelLuuHuyetNao = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateNhomTestKhac))
                    {
                        this.groupLabelNhomTestKhac = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateRaven))
                    {
                        this.groupLabelRaven = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateSieuAm))
                    {
                        this.groupLabelSieuAm = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateSieuAmDropperXuyenSo))
                    {
                        this.groupLabelSieuAmDropperXuyenSo = item.CATEGORY_NAME; 
                    }
                    else if (item.CATEGORY_CODE.Equals(this.reportTypeCateZung))
                    {
                        this.groupLabelZung = item.CATEGORY_NAME; 
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Khong ton tai REPORT_TYPE_CATE tương ungkd"); 
                    }
                }
                var sereServGroupByRequestDepartment = sereServs.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList(); 
                foreach (var group in sereServGroupByRequestDepartment)
                {
                    var subGroup = group.ToList<V_HIS_SERE_SERV>(); 
                    Mrs00368RDO rdo = new Mrs00368RDO(); 
                    foreach (var item in reportTypeCats)
                    {
                        var serviceRetyCateProcess = this.serviceRetyCats.Where(o => o.REPORT_TYPE_CAT_ID == item.ID).ToList(); 
                        if (serviceRetyCateProcess != null && serviceRetyCateProcess.Count > 0)
                        {
                            List<V_HIS_SERE_SERV> sereServGroup = new List<V_HIS_SERE_SERV>(); 
                            var sereServGroupProcess = subGroup.Where(o => serviceRetyCateProcess.Select(p => p.SERVICE_ID).Contains(o.SERVICE_ID)); 
                            foreach (var itemGroup in sereServGroupProcess)
                            {
                                //if (dicCurrentPatyAlter.ContainsKey(itemGroup.TREATMENT_ID ?? 0))
                                //{
                                //    if (itemGroup.INTRUCTION_TIME > dicCurrentPatyAlter[itemGroup.TREATMENT_ID ?? 0].LOG_TIME)
                                //    {
                                        sereServGroup.Add(itemGroup); 
                                    //}
                                //}
                            }
                            if (sereServGroup != null && sereServGroup.Count > 0)
                            {
                                rdo.REQUEST_DEPARTMENT_ID = sereServGroup.First().TDL_REQUEST_DEPARTMENT_ID; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServGroup.First().REQUEST_DEPARTMENT_NAME; 
                                if (item.CATEGORY_CODE.Equals(this.reportTypeCateNhomTestKhac))
                                {
                                    rdo.GROUP_NHOM_TEST_KHAC_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateZung))
                                {
                                    rdo.GROUP_ZUNG_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateRaven))
                                {
                                    rdo.GROUP_RAVEN_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateBeck))
                                {
                                    rdo.GROUP_BECK_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateSieuAm))
                                {
                                    rdo.GROUP_SIEU_AM_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateSieuAmDropperXuyenSo))
                                {
                                    rdo.GROUP_SIEU_AM_DROPPLER_XN_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateDienTim))
                                {
                                    rdo.GROUP_DIEN_TIM_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateLuuHuyetNao))
                                {
                                    rdo.GROUP_LUU_HUYET_NAO_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.CATEGORY_CODE.Equals(this.reportTypeCateDienNao))
                                {
                                    rdo.GROUP_DIEN_NAO_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else
                                    Inventec.Common.Logging.LogSystem.Warn("KHông tìm thấy report_type_code"); 
                            }
                        }
                    }
                    if (rdo.REQUEST_DEPARTMENT_ID > 0)
                    {
                        this.mrs00368RDOSereServs.Add(rdo); 
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
            if (((Mrs00368Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00368Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00368Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00368Filter)reportFilter).TIME_TO)); 
            }
            dicSingleTag.Add("GROUP_LABEL_NHOM_TEST_KHAC", groupLabelNhomTestKhac); 
            dicSingleTag.Add("GROUP_LABEL_ZUNG", groupLabelZung); 
            dicSingleTag.Add("GROUP_LABEL_RAVEN", groupLabelRaven); 
            dicSingleTag.Add("GROUP_LABEL_BECK", groupLabelBeck); 
            dicSingleTag.Add("GROUP_LABEL_SIEU_AM", groupLabelSieuAm); 
            dicSingleTag.Add("GROUP_LABEL_SIEU_AM_DROPPER_XS", groupLabelSieuAmDropperXuyenSo); 
            dicSingleTag.Add("GROUP_LABEL_DIEN_TIM", groupLabelDienTim); 
            dicSingleTag.Add("GROUP_LABEL_LUU_HUYET_NAO", groupLabelLuuHuyetNao); 
            dicSingleTag.Add("GROUP_LABEL_DIEN_NAO", groupLabelDienNao); 

            bool exportSuccess = true; 
            objectTag.AddObjectData(store, "Report", this.mrs00368RDOSereServs); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
