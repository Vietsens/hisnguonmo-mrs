using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00060
{
    public class Mrs00060Processor : AbstractProcessor
    {
        Mrs00060Filter castFilter = null;
        List<Mrs00060RDO> ListRdo = new List<Mrs00060RDO>();
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_SERE_SERV_2> ListCurrentSereServ = new List<V_HIS_SERE_SERV_2>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public Mrs00060Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00060Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00060Filter)this.reportFilter);
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.BRANCH_ID = castFilter.BRANCH_ID;
                listDepartment = new HisDepartmentManager(paramGet).Get(departmentFilter);
                LoadDataToRam();
                result = true;
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
                ProcessListCurrentSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentSereServ()
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => (ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0) && o.AMOUNT > 0).ToList();
                    if (ListCurrentSereServ.Count > 0)
                    {
                        var GroupServiceTypes = ListCurrentSereServ.GroupBy(g => g.TDL_SERVICE_TYPE_ID).ToList();
                        foreach (var group in GroupServiceTypes)
                        {
                            List<V_HIS_SERE_SERV_2> listSub = group.ToList<V_HIS_SERE_SERV_2>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                ProcessListFollowServiceType(listSub);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListFollowServiceType(List<V_HIS_SERE_SERV_2> ListSereServ)
        {
            try
            {
                var Groups = ListSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                foreach (var group in Groups)
                {
                    List<V_HIS_SERE_SERV_2> listSub = group.ToList<V_HIS_SERE_SERV_2>();
                    if (listSub != null && listSub.Count > 0)
                    {
                        ProcessListSereServSub(listSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListSereServSub(List<V_HIS_SERE_SERV_2> ListSereServ)
        {
            try
            {
                if (ListSereServ.Count > 0)
                {
                    Mrs00060RDO rdo = new Mrs00060RDO();
                    rdo.SERVICE_TYPE_NAME = HisServiceTypeCFG.HisServiceTypes != null ? HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == ListSereServ[0].TDL_SERVICE_TYPE_ID).SERVICE_TYPE_NAME : "";
                    rdo.SERVICE_UNIT_NAME = ListSereServ[0].SERVICE_UNIT_NAME;
                    rdo.SERVICE_CODE = ListSereServ[0].TDL_SERVICE_CODE;
                    rdo.SERVICE_NAME = ListSereServ[0].TDL_SERVICE_NAME;
                    foreach (var sereServ in ListSereServ)
                    {
                        rdo.AMOUNT += sereServ.AMOUNT;
                        rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        rdo.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                    }
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
                filter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filter.PATIENT_TYPE_ID = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                ListCurrentSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView2(filter);
                if (IsNotNullOrEmpty(listDepartment)) ListCurrentSereServ = ListCurrentSereServ.Where(o => listDepartment.Select(p => p.ID).Contains(o.REQUEST_DEPARTMENT_ID)).ToList();

                //DV - thanh toan
                var listTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery();
                        filterSereServBill.TDL_TREATMENT_IDs = listIDs;
                        var listSereServBillSub = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(filterSereServBill);
                        if (IsNotNullOrEmpty(listSereServBillSub))
                        {
                            ListSereServBill.AddRange(listSereServBillSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentSereServ.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new RDOCustomerFuncMergeSameData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
