using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00552
{
    class Mrs00552Processor : AbstractProcessor
    {
        string thisReportTypeCode = "MRS00373";
        Mrs00552Filter castFilter;
        private List<Mrs00552RDO> listRdos = new List<Mrs00552RDO>();
        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_DEPARTMENT> listDepartment = new List<V_HIS_DEPARTMENT>();

        public Mrs00552Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00552Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00552Filter)this.reportFilter;

                listDepartment = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).GetViewByBranchId(castFilter.BRANCH_ID);

                var treatmnetViewFilter = new HisTreatmentView4FilterQuery()
                {
                    CLINICAL_IN_TIME_FROM = castFilter.CLINICAL_IN_TIME_FROM,
                    CLINICAL_IN_TIME_TO = castFilter.CLINICAL_IN_TIME_TO,
                    FEE_LOCK_TIME_FROM = castFilter.FEE_LOCK_TIME_FROM,
                    FEE_LOCK_TIME_TO = castFilter.FEE_LOCK_TIME_TO
                };

                if (castFilter.FEE_LOCK_TIME_FROM != null) treatmnetViewFilter.CLINICAL_IN_TIME_FROM = 1;

                var listTreatments = new HisTreatmentManager(param).GetView4(treatmnetViewFilter);

                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = thisReportTypeCode;
                listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter);
                var serviceIds = listServiceRetyCats.Select(s => s.SERVICE_ID).Distinct().ToList();
                var skip = 0;
                while (listTreatments.Count() - skip > 0)
                {
                    var ListDSs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var sereServViewFilter = new HisSereServFilterQuery()
                    {
                        TREATMENT_IDs = ListDSs.Select(s => s.ID).ToList(),
                        TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
                        TDL_REQUEST_DEPARTMENT_IDs = listDepartment.Select(o => o.ID).ToList(),
                    };
                    var listSereServ = new HisSereServManager(param).Get(sereServViewFilter);
                    if (IsNotNullOrEmpty(listSereServ))
                    {
                        listSereServ = listSereServ.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            listSereServs.AddRange(listSereServ);
                        }
                    }
                }
                //Inventec.Common.Logging.LogSystem.Info("listSereServs" + listSereServs.Count);

                skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listIDs = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery();
                    sereServBillFilter.SERE_SERV_IDs = listIDs.Select(s => s.ID).ToList();
                    sereServBillFilter.IS_NOT_CANCEL = true;
                    var sereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillFilter);
                    listSereServBills.AddRange(sereServBill);
                }
                //Inventec.Common.Logging.LogSystem.Info("listSereServBills" + listSereServBills.Count);
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
                if (IsNotNullOrEmpty(listServiceRetyCats) && IsNotNullOrEmpty(listSereServBills))
                {
                    List<Mrs00552RDO> listRdo = new List<Mrs00552RDO>();
                    Dictionary<long, V_HIS_DEPARTMENT> dicDepartment = listDepartment.ToDictionary(o => o.ID);

                    var listServiceRetyCat = listServiceRetyCats.Select(s => s.SERVICE_ID).Distinct().ToList();
                    var listSereServBill = listSereServBills.Where(w => listServiceRetyCat.Contains(w.SERVICE_ID)).ToList();
                    foreach (var sereServBills in listSereServBill)
                    {
                        var sereServ = listSereServs.FirstOrDefault(w => w.ID == sereServBills.SERE_SERV_ID);
                        if (sereServ != null)
                        {
                            if (!dicDepartment.ContainsKey(sereServ.TDL_REQUEST_DEPARTMENT_ID))
                            {
                                continue;
                            }

                            Mrs00552RDO rdo = new Mrs00552RDO();
                            rdo.TOTAL_DATE = sereServ.AMOUNT;
                            rdo.TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE;
                            rdo.DEPARTMENT_NAME = dicDepartment[sereServ.TDL_REQUEST_DEPARTMENT_ID].DEPARTMENT_NAME;
                            listRdo.Add(rdo);
                        }
                    }

                    listRdos = listRdo.GroupBy(o => o.DEPARTMENT_NAME).Select(
                       g => new Mrs00552RDO
                       {
                           DEPARTMENT_NAME = g.First().DEPARTMENT_NAME,
                           TOTAL_DATE = g.Sum(s => s.TOTAL_DATE),
                           TOTAL_PRICE = g.Sum(s => s.TOTAL_PRICE)
                       }).ToList();

                    if (IsNotNullOrEmpty(listRdos))
                    {
                        listRdos = listRdos.Where(o => o.TOTAL_DATE != 0).ToList();
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listRdos1" + listRdos.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CLINICAL_IN_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.FEE_LOCK_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CLINICAL_IN_TIME_TO ?? 0));

                var branch = new MOS.MANAGER.HisBranch.HisBranchManager(new CommonParam()).GetById(castFilter.BRANCH_ID);
                if (branch != null)
                {
                    dicSingleTag.Add("BRANCH_NAME", branch.BRANCH_NAME);
                }

                objectTag.AddObjectData(store, "Report", listRdos);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
