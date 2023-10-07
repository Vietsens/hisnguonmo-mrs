using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisHeinServiceType;


namespace MRS.Processor.Mrs00272//issue 2473
{
    class Mrs00272Processor : AbstractProcessor
    {
        Mrs00272Filter filter = null;
        List<Mrs00272RDO> ListRdo = new List<Mrs00272RDO>();
        List<Mrs00272RDO> ListGroup = new List<Mrs00272RDO>();
        CommonParam paramGet = new CommonParam();
        List<HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<SereServSDO> listSereServ = new List<SereServSDO>();
        List<HIS_HEIN_SERVICE_TYPE> listHisHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        public Mrs00272Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00272Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00272Filter)reportFilter);
            bool result = true;
            try
            {
                listSereServ = new ManagerSql().GetSereServDO(filter);
               
                HisHeinServiceTypeFilterQuery HisHeinServiceTypefilter = new HisHeinServiceTypeFilterQuery();
                HisHeinServiceTypefilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                listHisHeinServiceType = new HisHeinServiceTypeManager().Get(HisHeinServiceTypefilter);
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
            var result = true;
            try
            {
                ListRdo.Clear();

                if (IsNotNullOrEmpty(listSereServ))
                {

                    foreach (var item in listSereServ)
                    {
                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                        item.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                        item.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                       
                    }

                    var groupByServiceId = listSereServ.GroupBy(o => new { o.SERVICE_ID, o.VIR_PRICE }).ToList();
                    foreach (var group in groupByServiceId)
                    {
                        List<HIS_SERE_SERV> listSub = group.ToList<HIS_SERE_SERV>();
                        Mrs00272RDO rdo = new Mrs00272RDO();
                        var heinServiceType = listHisHeinServiceType.FirstOrDefault(o => o.ID == listSub.First().TDL_HEIN_SERVICE_TYPE_ID) ?? new HIS_HEIN_SERVICE_TYPE();
                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == listSub.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                        
                        rdo.SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                        rdo.VAT_RATIO = listSub.First().VAT_RATIO;
                        rdo.PRICE = (listSub.First().VIR_PRICE??0);
                        rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * ((o.VIR_PRICE??0) ));
                        rdo.HEIN_SERVICE_TYPE_NUM_ORDER = heinServiceType.NUM_ORDER ?? 100;
                        rdo.SERVICE_TYPE_ID = listSub.First().TDL_SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                        ListRdo.Add(rdo);
                    }
                    ListRdo = ListRdo.OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).Where(o => o.VIR_TOTAL_PRICE > 0).ToList();

                    ListGroup = ListRdo.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM??0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO??0));
            objectTag.AddObjectData(store, "Detail", listSereServ ?? new List<SereServSDO>());
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.SERVICE_NAME).ToList());
            objectTag.AddObjectData(store, "Group", ListGroup);
            objectTag.AddRelationship(store, "Group", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            if (filter.DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
                dicSingleTag.Add("DEPARTMENT_CODE", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE);
            }
            if (filter.BRANCH_ID != null)
            {
                dicSingleTag.Add("BRANCH_NAME", (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == filter.BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME);
            }
        }
    }
}
