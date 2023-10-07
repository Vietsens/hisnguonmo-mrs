using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServBill;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00373
{
    class Mrs00373Processor : AbstractProcessor
    {
        private List<Mrs00373RDO> listRdos = new List<Mrs00373RDO>();
        private List<Mrs00373RDO> listRdo = new List<Mrs00373RDO>();
        Mrs00373Filter castFilter = null;

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TREATMENT_4> listTreatments = new List<V_HIS_TREATMENT_4>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();
        string thisReportTypeCode = "";
        public Mrs00373Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }
        public override Type FilterType()
        {
            return typeof(Mrs00373Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00373Filter)this.reportFilter;

                var treatmnetViewFilter = new HisTreatmentView4FilterQuery()
                {
                    CLINICAL_IN_TIME_FROM = castFilter.CLINICAL_IN_TIME_FROM,
                    CLINICAL_IN_TIME_TO = castFilter.CLINICAL_IN_TIME_TO,
                    FEE_LOCK_TIME_FROM = castFilter.FEE_LOCK_TIME_FROM,
                    FEE_LOCK_TIME_TO = castFilter.FEE_LOCK_TIME_TO
                };
                if (castFilter.FEE_LOCK_TIME_FROM != null) treatmnetViewFilter.CLINICAL_IN_TIME_FROM = 1;
                listTreatments = new HisTreatmentManager(param).GetView4(treatmnetViewFilter);
                var skip = 0;
                while (listTreatments.Count() - skip > 0)
                {
                    var ListDSs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var sereServViewFilter = new HisSereServViewFilterQuery()
                    {
                        TREATMENT_IDs = ListDSs.Select(s => s.ID).ToList(),
                        SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
                        REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID,
                    };
                    var listSereServ = new HisSereServManager(param).GetView(sereServViewFilter);
                    listSereServs.AddRange(listSereServ);
                }
                Inventec.Common.Logging.LogSystem.Info("listSereServs" + listSereServs.Count);

                skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listIDs = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery();
                    sereServBillFilter.SERE_SERV_IDs = listIDs.Select(s => s.ID).ToList();
                    sereServBillFilter.IS_NOT_CANCEL = true;
                    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillFilter));
                }
                Inventec.Common.Logging.LogSystem.Info("listSereServBills" + listSereServBills.Count);

                skip = 0;
                while (listSereServBills.Count - skip > 0)
                {
                    var listIDs = listSereServBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                    serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = thisReportTypeCode;
                    serviceRetyCatFilter.SERVICE_IDs = listIDs.Select(s => s.SERVICE_ID).ToList();
                    listServiceRetyCats.AddRange(new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter));
                }
                Inventec.Common.Logging.LogSystem.Info("listServiceRetyCats" + listServiceRetyCats.Count);

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

                var listServiceRetyCat = listServiceRetyCats.Select(s => s.SERVICE_ID).Distinct().ToList();
                var listSereServBill = listSereServBills.Where(w => listServiceRetyCat.Contains(w.SERVICE_ID)).ToList();
                foreach (var sereServBills in listSereServBill)
                {
                    Mrs00373RDO rdo = new Mrs00373RDO();
                    rdo.TOTAL_DATE = sereServBills.AMOUNT;
                    rdo.TRANSACTION_TIME = sereServBills.CREATE_TIME;
                    rdo.FEE_LOCK_TIME = sereServBills.CREATE_TIME;
                    //rdo.TOTAL_PRICE = sereServBills.PRICE;
                    var sereServ = listSereServs.Where(w => w.ID == sereServBills.SERE_SERV_ID).ToList();
                    var listSereServ = sereServ != null && sereServ.Count > 0 ? sereServ.Select(s => s.TDL_TREATMENT_ID).ToList() : new List<long?>();
                    rdo.TOTAL_PRICE = sereServ != null && sereServ.Count > 0 ? sereServ.Sum(s => s.VIR_TOTAL_PRICE) : 0;
                    var listTreatment = listTreatments.Where(w => listSereServ.Contains(w.ID)).ToList();
                    foreach (var treatment in listTreatment)
                    {
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                        rdo.OUT_TIME = treatment.OUT_TIME;
                        rdo.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME;

                        listRdo.Add(rdo);
                    }
                    listRdo = listRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                }
                listRdos = listRdo.GroupBy(o => o.TREATMENT_CODE).Select(
                   g => new Mrs00373RDO
                   {
                       SERVICE_REQUEST_CODE = g.First().SERVICE_REQUEST_CODE,
                       TREATMENT_CODE = g.First().TREATMENT_CODE,
                       PATIENT_NAME = g.First().PATIENT_NAME,
                       DOB = g.First().DOB,
                       ADDRESS = g.First().ADDRESS,
                       CLINICAL_IN_TIME = g.First().CLINICAL_IN_TIME,
                       OUT_TIME = g.First().OUT_TIME,
                       TRANSACTION_TIME = g.First().TRANSACTION_TIME,
                       TOTAL_DATE = g.Sum(s => s.TOTAL_DATE),
                       NOTE = g.First().NOTE,
                       FEE_LOCK_TIME = g.First().FEE_LOCK_TIME,
                       TOTAL_PRICE = g.Sum(s => s.TOTAL_PRICE)
                   }).ToList();
                Inventec.Common.Logging.LogSystem.Info("listRdos1" + listRdos.Count);
            }

            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            return result;
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CLINICAL_IN_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CLINICAL_IN_TIME_TO ?? 0));
            HisDepartmentFilterQuery df = new HisDepartmentFilterQuery();
            df.ID = castFilter.REQUEST_DEPARTMENT_ID;
            var hisDepartment = new HisDepartmentManager().Get(df);
            if (castFilter.REQUEST_DEPARTMENT_ID != null) dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", hisDepartment.First().DEPARTMENT_NAME);

            objectTag.AddObjectData(store, "ReportSub", listRdo);
            Inventec.Common.Logging.LogSystem.Info("listRdos" + listRdos.Count);

            objectTag.AddObjectData(store, "Report", listRdos);
            //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Report", "Report1", "EXP_MEST_CODE", "EXP_MEST_CODE"); 
        }
    }
}
