using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MRS.MANAGER.Config;
using System.Reflection;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceMachine;

namespace MRS.Processor.Mrs00619
{
    public class Mrs00619Processor : AbstractProcessor
    {
        Mrs00619Filter filter = null;
        //List<HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        //List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        //List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<Mrs00619RDO> ListSereServ = new List<Mrs00619RDO>();
        //List<HIS_SERVICE_MACHINE> listHisServiceMachine = new List<HIS_SERVICE_MACHINE>();
        CommonParam paramGet = new CommonParam();
        public Mrs00619Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00619Filter);
        }
        protected override bool GetData()///
        {
            filter = ((Mrs00619Filter)reportFilter);
            var result = true;
            try
            {
                //HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                //reportTypeCatFilter.ID = filter.REPORT_TYPE_CAT_ID;
                //ListReportTypeCat = new HisReportTypeCatManager().Get(reportTypeCatFilter);
                //if (ListReportTypeCat != null)
                //{
                //    HisServiceRetyCatFilterQuery srFilter = new HisServiceRetyCatFilterQuery();
                //    srFilter.REPORT_TYPE_CAT_ID = ListReportTypeCat.Select(o => o.ID).FirstOrDefault();
                //    ListServiceRetyCat = new HisServiceRetyCatManager().Get(srFilter);
                //}

                //var serviceIds = ListServiceRetyCat.Select(s => s.SERVICE_ID).Distinct().ToList();
                ////YC
                //HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                //filterSr.FINISH_TIME_FROM = filter.FINISH_TIME_FROM;
                //filterSr.FINISH_TIME_TO = filter.FINISH_TIME_TO;
                //filterSr.HAS_EXECUTE = true;
                //filterSr.SERVICE_REQ_STT_ID = 3;
                //var listServicereqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                //if (IsNotNullOrEmpty(listServicereqSub))
                //    ListServiceReq.AddRange(listServicereqSub);

                //if (IsNotNullOrEmpty(serviceIds))
                //{
                //    var skip = 0;
                //    while (serviceIds.Count - skip > 0)
                //    {
                //        var listIDs = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                //        var skp = 0;
                //        var serviceReqIds = ListServiceReq.Select(o => o.ID).Distinct().ToList();
                //        while (serviceReqIds.Count - skp > 0)
                //        {
                //            var ReqIds = serviceReqIds.Skip(skp).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //            skp = skp + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //            //YC - DV
                //            HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                //            filterSs.HAS_EXECUTE = true;
                //            filterSs.SERVICE_IDs = listIDs;
                //            filterSs.SERVICE_REQ_IDs = ReqIds;
                //            filterSs.PATIENT_TYPE_ID = filter.PATIENT_TYPE_ID;
                //            var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                //            if (IsNotNullOrEmpty(listSereServSub))
                //                ListSereServ.AddRange(listSereServSub);
                //        }
                //        //DV
                //        HisServiceMachineFilterQuery filterSv = new HisServiceMachineFilterQuery();
                //        //filterSv.MACHINE_IDs = filter.MACHINE_IDs;
                //        //filterSv.MACHINE_ID = filter.MACHINE_ID;
                //        filterSv.SERVICE_IDs = listIDs;
                //        var listServiceMachineSub = new HisServiceMachineManager(paramGet).Get(filterSv);
                //        if (IsNotNullOrEmpty(listServiceMachineSub))
                //            listHisServiceMachine.AddRange(listServiceMachineSub);
                //    }
                //    if (filter.MACHINE_IDs != null)
                //    {
                //        ListSereServ = ListSereServ.Where(o => listHisServiceMachine.Exists(p => p.SERVICE_ID == o.SERVICE_ID && filter.MACHINE_IDs.Contains(p.MACHINE_ID))).ToList();
                //    }
                //    if (filter.MACHINE_ID != null)
                //    {
                //        ListSereServ = ListSereServ.Where(o => listHisServiceMachine.Exists(p => p.SERVICE_ID == o.SERVICE_ID && filter.MACHINE_ID==p.MACHINE_ID)).ToList();
                //    }
                //    ListSereServ = ListSereServ.Where(o => ListServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                //}
                ListSereServ = new ManagerSql().GetSereServDO(filter);

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
                foreach (var item in ListSereServ)
                {
                    //var serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == (item.SERVICE_REQ_ID ?? 0));
                   
                    //if (serviceReq != null)
                    //{
                        //rdo.INTRUCTION_TIME = serviceReq.INTRUCTION_TIME;
                        item.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.INTRUCTION_TIME??0);
                        //rdo.FINISH_TIME = serviceReq.FINISH_TIME ?? 0;
                        item.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FINISH_TIME ?? 0);
                        //rdo.START_TIME = serviceReq.START_TIME ?? 0;
                        item.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.START_TIME ?? 0);
                        //rdo.EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;
                        //rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                        //rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE;
                        //rdo.VIR_ADDRESS = serviceReq.TDL_PATIENT_ADDRESS;
                        item.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        item.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        item.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        //rdo.ICD_NAME = serviceReq.ICD_NAME + ";" + serviceReq.ICD_TEXT;
                    //}

                    //rdo.HEIN_CARD_NUMBER = item.HEIN_CARD_NUMBER;
                    //rdo.VIR_PRICE = item.VIR_PRICE;
                    //rdo.AMOUNT = item.AMOUNT;
                    //rdo.VIR_TOTAL_PRICE = item.VIR_TOTAL_PRICE;
                    //rdo.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                    //rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                    //rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                    IsBhyt(item.PATIENT_TYPE_ID,item);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void IsBhyt(long patientTypeId, Mrs00619RDO rdo)
        {
            try
            {
                if (patientTypeId == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.FINISH_TIME_FROM?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.FINISH_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", ListSereServ.OrderByDescending(o => o.TREATMENT_CODE).ToList());

        }
    }
}
