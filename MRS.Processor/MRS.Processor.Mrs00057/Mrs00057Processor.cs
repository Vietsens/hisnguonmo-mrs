using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00057
{
    public class Mrs00057Processor : AbstractProcessor
    {
        Mrs00057Filter castFilter = null;
        Dictionary<long, V_HIS_SERVICE> dicService = new Dictionary<long, V_HIS_SERVICE>();
        List<Mrs00057RDO> ListSereServ = new List<Mrs00057RDO>();
        List<Mrs00057RDO> ListRdo = new List<Mrs00057RDO>();
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<Mrs00057RDO> ListDual = new List<Mrs00057RDO>();
        CommonParam paramGet = new CommonParam();
        long patientTypeIdBhyt = 0;

        public Mrs00057Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00057Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                castFilter = ((Mrs00057Filter)this.reportFilter);
                CommonParam paramGet = new CommonParam();
                //dich vu
                ListService = new HisServiceManager().Get(new HisServiceFilterQuery());
                //yc - dv 

                ListSereServ = GetSereServ();
                //dich vu
                var listServiceId = ListSereServ.Select(o => o.SERVICE_ID).Distinct().ToList();

                //DV - thanh toan
                var listSereServId = ListSereServ.Select(s => s.ID).ToList();

                if (IsNotNullOrEmpty(listSereServId))
                {
                    var skip = 0;
                    while (listSereServId.Count - skip > 0)
                    {
                        var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery();
                        filterSereServBill.SERE_SERV_IDs = listIDs;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).GetView(filterSereServBill);
                        if (IsNotNullOrEmpty(listSereServBillSub))
                            ListSereServBill.AddRange(listSereServBillSub);
                    }
                }

                if (paramGet.HasException)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private List<Mrs00057RDO> GetSereServ()
        {
            List<Mrs00057RDO> result = new List<Mrs00057RDO>();
            try
            {

                //HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery();
                //filterSereServ.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                //filterSereServ.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                //filterSereServ.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN;
                //filterSereServ.IS_EXPEND = false;
                //filterSereServ.HAS_EXECUTE = true;
                result = new ManagerSql().GetRdo(castFilter);
                result = result.Where(o => o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE && o.SERVICE_REQ_ID != null).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00057RDO>();
            }
            return result;
        }


        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    ProcessBeforeGeneralData(ListSereServ, false);
                }
                else if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                {
                    ProcessBeforeGeneralData(ListSereServ, true);
                }
                else
                {
                    ProcessBeforeGeneralData(ListSereServ, null);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessBeforeGeneralData(List<Mrs00057RDO> ListCurrentSereServ, bool? isListPatientTypeId)
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    if (castFilter.IS_NOT_REQUIRE_BILL != true)
                    {
                        ListCurrentSereServ = ListCurrentSereServ.Where(o => (ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0)).ToList();
                    }
                    if (isListPatientTypeId.HasValue)
                    {
                        if (isListPatientTypeId.Value)
                        {
                            ListCurrentSereServ = ListCurrentSereServ.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();
                            ProcessDetailListCurrentSereServ(ListCurrentSereServ);
                            ListRdo = ListRdo.OrderBy(o => o.PATIENT_TYPE_ID).ThenBy(t => t.TEST_SERVICE_TYPE_CODE).ToList();
                        }
                        else
                        {
                            ProcessDetailListCurrentSereServ(ListCurrentSereServ);
                        }
                    }
                    else
                    {
                        ProcessDetailListCurrentSereServ(ListCurrentSereServ);
                        ListRdo = ListRdo.OrderBy(o => o.PATIENT_TYPE_ID).ThenBy(t => t.TEST_SERVICE_TYPE_CODE).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private V_HIS_SERVICE sv(V_HIS_SERE_SERV g)
        {
            V_HIS_SERVICE result = new V_HIS_SERVICE();
            try
            {
                if (dicService.ContainsKey(g.SERVICE_ID))
                {
                    result = dicService[g.SERVICE_ID];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new V_HIS_SERVICE();
            }
            return result;
        }

        private void ProcessDetailListCurrentSereServ(List<Mrs00057RDO> ListCurrentSereServ)
        {
            try
            {
                if (ListCurrentSereServ.Count > 0)
                {
                    var Groups = ListCurrentSereServ.OrderBy(o => o.SERVICE_ID).GroupBy(g => new { g.SERVICE_ID, sv(g).COGS, g.VIR_PRICE }).ToList();
                    foreach (var group in Groups)
                    {
                        List<Mrs00057RDO> listSub = group.ToList<Mrs00057RDO>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00057RDO rdo = new Mrs00057RDO();
                            rdo.PATIENT_TYPE_ID = listSub[0].PATIENT_TYPE_ID;
                            rdo.PATIENT_TYPE_CODE = listSub[0].PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = listSub[0].PATIENT_TYPE_NAME;
                            rdo.TEST_SERVICE_TYPE_CODE = sv(listSub[0]).SERVICE_CODE;
                            rdo.TEST_SERVICE_TYPE_NAME = listSub[0].TDL_SERVICE_NAME;
                            var service = ListService.FirstOrDefault(o => o.ID == listSub[0].SERVICE_ID);
                            if (service != null)
                            {
                                var parent = ListService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                                if (parent != null)
                                {
                                    rdo.TEST_PARENT_CODE = parent.SERVICE_CODE;
                                    rdo.TEST_PARENT_NAME = parent.SERVICE_NAME;
                                }
                                else
                                {
                                    var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == service.SERVICE_TYPE_ID);
                                    if (serviceType != null)
                                    {
                                        rdo.TEST_PARENT_CODE = serviceType.SERVICE_TYPE_CODE;
                                        rdo.TEST_PARENT_NAME = serviceType.SERVICE_TYPE_NAME;
                                    }
                                }
                            }

                            rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                            rdo.AMOUNT_NOITRU = listSub.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                            rdo.AMOUNT_NGOAITRU = listSub.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Sum(s => s.AMOUNT);
                            rdo.AMOUNT_BHYT = listSub.Where(o => o.PATIENT_TYPE_ID == this.patientTypeIdBhyt).Sum(s => s.AMOUNT);
                           
                            rdo.AMOUNT_FEE = listSub.Where(o=>o.PATIENT_TYPE_ID != this.patientTypeIdBhyt).Sum(s => s.AMOUNT);
                            rdo.TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                            rdo.TOTAL_PATIENT_PRICE = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                            rdo.COST_PRICE = sv(listSub[0]).COGS ?? 0;
                            rdo.FEE_PRICE = (listSub[0].VIR_PRICE ?? 0) - (sv(listSub[0]).COGS ?? 0);
                            ListRdo.Add(rdo);
                        }
                    }
                    foreach (var rdo in ListRdo.OrderBy(o => o.TEST_PARENT_CODE).ToList())
                    {
                        if (ListDual.Count == 0)
                        {
                            ListDual.Add(rdo);
                        }
                        else if (ListDual.Last().secondRDO == null)
                        {
                            var lastDual = ListDual.Last();
                            lastDual.secondRDO = new Mrs00057RDO();
                            if (rdo.TEST_PARENT_CODE == lastDual.TEST_PARENT_CODE)
                            {
                                lastDual.secondRDO = rdo;
                            }
                            else
                            {
                                ListDual.Add(rdo);
                            }
                        }
                        else
                        {
                            ListDual.Add(rdo);
                        }
                    }
                    if (ListDual.Last().secondRDO == null)
                    {
                        ListDual.Last().secondRDO = new Mrs00057RDO();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ListDual", ListDual);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
