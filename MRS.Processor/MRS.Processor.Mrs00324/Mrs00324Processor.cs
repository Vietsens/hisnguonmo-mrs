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
 

namespace MRS.Processor.Mrs00324
{
    public class Mrs00324Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 

        private List<Mrs00324RDO> mrs00324RDOSereServs = new List<Mrs00324RDO>(); 
        private List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> sereServs = new List<V_HIS_SERE_SERV>(); 
        private List<long> treatmentIds = new List<long>(); 
        private List<MOS.EFMODEL.DataModels.HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>(); // nhóm loại báo cáo
        private List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_RETY_CAT> serviceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 

        Mrs00324Filter filter = null; 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        string groupLabel1, groupLabel2, groupLabel3, groupLabel4, groupLabel5, groupLabel6, groupLabel7; 
        public Mrs00324Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00324Filter); 
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00324Filter)reportFilter); 
            int start = 0; 
            var result = true; 
            CommonParam param = new CommonParam(); 
            try
            {
                //get REPORT_TYPE_CATE
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery(); 
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00324"; 
                MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager reportTypeCateGet = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param); 
                reportTypeCats = reportTypeCateGet.Get(reportTypeCatFilter); 

                // get SERVICE_RETY_CAT
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
                sereServFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM; 
                sereServFilter.INTRUCTION_TIME_TO = filter.TIME_TO; 
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
                mrs00324RDOSereServs.Clear(); 
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
                var sereServGroupByRequestDepartment = sereServs.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList(); 

                // set label
                foreach (var item in reportTypeCats)
                {
                    if (item.ID == reportTypeCats[0].ID)
                    {
                        this.groupLabel1 = reportTypeCats[0].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[1].ID)
                    {
                        this.groupLabel2 = reportTypeCats[1].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[2].ID)
                    {
                        this.groupLabel3 = reportTypeCats[2].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[3].ID)
                    {
                        this.groupLabel4 = reportTypeCats[3].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[4].ID)
                    {
                        this.groupLabel5 = reportTypeCats[4].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[5].ID)
                    {
                        this.groupLabel6 = reportTypeCats[5].CATEGORY_NAME; 
                    }
                    else if (item.ID == reportTypeCats[6].ID)
                    {
                        this.groupLabel7 = reportTypeCats[6].CATEGORY_NAME; 
                    }
                }

                //set amount
                foreach (var group in sereServGroupByRequestDepartment)
                {
                    var subGroup = group.ToList<V_HIS_SERE_SERV>(); 
                    Mrs00324RDO rdo = new Mrs00324RDO(); 
                    foreach (var item in reportTypeCats)
                    {
                        var serviceRetyCateProcess = this.serviceRetyCats.Where(o => o.REPORT_TYPE_CAT_ID == item.ID).ToList(); 
                        if (serviceRetyCateProcess != null && serviceRetyCateProcess.Count > 0)
                        {
                            List<V_HIS_SERE_SERV> sereServGroup = new List<V_HIS_SERE_SERV>(); 
                            var sereServGroupProcess = subGroup.Where(o => serviceRetyCateProcess.Select(p => p.SERVICE_ID).Contains(o.SERVICE_ID)); 
                            foreach (var itemGroup in sereServGroupProcess)
                            {
                                sereServGroup.Add(itemGroup); 
                            }
                            if (sereServGroup != null && sereServGroup.Count > 0)
                            {
                                rdo.REQUEST_DEPARTMENT_ID = sereServGroup.First().TDL_REQUEST_DEPARTMENT_ID; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServGroup.First().REQUEST_DEPARTMENT_NAME; 
                                if (item.ID == reportTypeCats[0].ID)
                                {
                                    rdo.GROUP1_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[1].ID)
                                {
                                    rdo.GROUP2_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[2].ID)
                                {
                                    rdo.GROUP3_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[3].ID)
                                {
                                    rdo.GROUP4_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[4].ID)
                                {
                                    rdo.GROUP5_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[5].ID)
                                {
                                    rdo.GROUP6_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                                else if (item.ID == reportTypeCats[6].ID)
                                {
                                    rdo.GROUP7_AMOUNT = sereServGroup.Sum(o => o.AMOUNT); 
                                }
                            }
                        }
                    }
                    if (rdo.REQUEST_DEPARTMENT_ID > 0)
                    {
                        this.mrs00324RDOSereServs.Add(rdo); 
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
            if (((Mrs00324Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00324Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00324Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00324Filter)reportFilter).TIME_TO)); 
            }
            dicSingleTag.Add("GROUP_LABEL_1", groupLabel1); 
            dicSingleTag.Add("GROUP_LABEL_2", groupLabel2); 
            dicSingleTag.Add("GROUP_LABEL_3", groupLabel3); 
            dicSingleTag.Add("GROUP_LABEL_4", groupLabel4); 
            dicSingleTag.Add("GROUP_LABEL_5", groupLabel5); 
            dicSingleTag.Add("GROUP_LABEL_6", groupLabel6); 
            dicSingleTag.Add("GROUP_LABEL_7", groupLabel7); 

            objectTag.AddObjectData(store, "Report", this.mrs00324RDOSereServs); 
        }

    }
}
