using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00125
{
    public class Mrs00125Processor : AbstractProcessor
    {
        List<Mrs00125RDO> ListSereServRdo = new List<Mrs00125RDO>();
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        Mrs00125Filter CastFilter;
        private string SERVICE_REPORT_NAME;
        private string PATIENT_TYPE_NAME;
        private decimal? TOTAL_PRICE;
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_ICD> listIcd = new List<HIS_ICD>();
        List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        public Mrs00125Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00125Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00125Filter)this.reportFilter;
                var paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu filter: " +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));
                //////////////////////////////////////////////////////////////////////////////////-V_HIS_SEREVICE
                var metyFilterService = new HisServiceViewFilterQuery
                {
                    ID = CastFilter.SERVICE_ID
                };
                var service = new HisServiceManager(paramGet).GetView(metyFilterService);
                if (service == null || service.Count <= 0)
                {
                    throw new Exception("khong tim thay service");
                }

                //SERVICE_REPORT_NAME = service.Select(s => s.TDL_SERVICE_NAME).First(); 
                SERVICE_REPORT_NAME = string.Format("{0} - {1}", SERVICE_REPORT_NAME = service.Select(s => s.SERVICE_NAME).First(), SERVICE_REPORT_NAME = service.Select(s => s.SERVICE_CODE).First());
                //////////////////////////////////////////////////////////////////////////////////
                var metyFilterPatientType = new HisPatientTypeFilterQuery
                {
                    ID = CastFilter.PATIENT_TYPE_ID
                };
                var patientType = new HisPatientTypeManager(paramGet).Get(metyFilterPatientType);
                PATIENT_TYPE_NAME = string.Format("{0} - {1}", patientType.Select(s => s.PATIENT_TYPE_CODE).First(), patientType.Select(s => s.PATIENT_TYPE_NAME).First());

                //tru: thoi gian thanh toan
                if (CastFilter.TRUE_FALSE != null && (Boolean)CastFilter.TRUE_FALSE)
                {
                    ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
                    HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery();
                    filterSereServBill.CREATE_TIME_FROM = CastFilter.DATE_FROM;
                    filterSereServBill.CREATE_TIME_TO = CastFilter.DATE_TO;
                    ListSereServBill = new HisSereServBillManager(paramGet).GetView(filterSereServBill);

                    ////////////////////////////////////////////////////////////////////////////////// -V_HIS_TREATMENT
                    var listTreatmentIds = ListSereServBill.Select(s => s.TDL_TREATMENT_ID).Distinct().ToList();
                    if (IsNotNullOrEmpty(listTreatmentIds))
                    {
                        var skip = 0;
                        while (listTreatmentIds.Count - skip > 0)
                        {
                            var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var treatmentfilter = new MOS.MANAGER.HisTreatment.HisTreatmentViewFilterQuery
                            {
                                IDs = listIDs
                            };
                            var treatmentSub = new HisTreatmentManager(paramGet).GetView(treatmentfilter);
                            listTreatment.AddRange(treatmentSub);
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////////////-V_HIS_SERE_SERV
                    if (IsNotNullOrEmpty(listTreatmentIds))
                    {
                        var skip = 0;
                        listSereServ = new List<V_HIS_SERE_SERV>();
                        while (listTreatmentIds.Count - skip > 0)
                        {
                            var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var metyFilterSereVerv = new HisSereServViewFilterQuery
                            {
                                SERVICE_ID = CastFilter.SERVICE_ID,
                                PATIENT_TYPE_ID = CastFilter.PATIENT_TYPE_ID,
                                TREATMENT_IDs = listIDs
                            };
                            var sereServView = new HisSereServManager(paramGet).GetView(metyFilterSereVerv);
                            listSereServ.AddRange(sereServView);
                        }
                    }
                    listSereServ = listSereServ.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).Contains(o.ID)).ToList();
                }
                else
                {
                    ////////////////////////////////////////////////////////////////////////////////// -V_HIS_TREATMENT
                    var metyFilterTreatmentView = new HisTreatmentViewFilterQuery
                    {
                        IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE,
                        FEE_LOCK_TIME_FROM = CastFilter.DATE_FROM,
                        FEE_LOCK_TIME_TO = CastFilter.DATE_TO
                    };
                    listTreatment = new HisTreatmentManager(paramGet).GetView(metyFilterTreatmentView);
                    //////////////////////////////////////////////////////////////////////////////////-V_HIS_SERE_SERV
                    var listTreatmentIds = listTreatment.Select(s => s.ID).ToList();
                    if (IsNotNullOrEmpty(listTreatmentIds))
                    {
                        var skip = 0;
                        while (listTreatmentIds.Count - skip > 0)
                        {
                            var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var metyFilterSereVerv = new HisSereServViewFilterQuery
                            {
                                SERVICE_ID = CastFilter.SERVICE_ID,
                                PATIENT_TYPE_ID = CastFilter.PATIENT_TYPE_ID,
                                TREATMENT_IDs = listIDs
                            };
                            var sereServView = new HisSereServManager(paramGet).GetView(metyFilterSereVerv);
                            listSereServ.AddRange(sereServView);
                        }
                    }
                    //DV - thanh toan
                    var listSereServId = listSereServ.Select(s => s.ID).ToList();

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
                            ListSereServBill.AddRange(listSereServBillSub);
                        }
                    }

                    listSereServ = listSereServ.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).Contains(o.ID)).ToList();
                }

                //////////////////////////////////////////////////////////////////////////////////-V_HIS_SERVICE_REQ
                if (IsNotNullOrEmpty(listSereServ))
                {
                    List<long> reqIds = listSereServ.Select(s => s.SERVICE_REQ_ID ?? 0).Distinct().ToList();

                    var skip = 0;
                    while (reqIds.Count - skip > 0)
                    {
                        var listIDs = reqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var serviceReq = new HisServiceReqViewFilterQuery
                        {
                            IDs = listIDs
                        };
                        var serviceReqSub = new HisServiceReqManager(paramGet).GetView(serviceReq);
                        listServiceReq.AddRange(serviceReqSub);
                    }
                }

                //////////////////////////////////////////////////////////////////////////////////-HIS_TRANSACTION
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    var tranIds = ListSereServBill.Select(s => s.BILL_ID).Distinct().ToList();
                    var skip = 0;
                    while (tranIds.Count - skip > 0)
                    {
                        var listIDs = tranIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                        tranFilter.IDs = listIDs;
                        tranFilter.IS_CANCEL = false;
                        var listTransaction = new HisTransactionManager(paramGet).Get(tranFilter);
                        ListTransaction.AddRange(listTransaction);
                    }
                }

                //////////////////////////////////////////////////////////////////////////////////
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00125." +
                        Inventec.Common.Logging.LogUtil.TraceData(
                            Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
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
                if (IsNotNullOrEmpty(listSereServ))
                {
                    ProcessFillterData(listSereServ, listIcd, listTreatment);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFillterData(List<V_HIS_SERE_SERV> listSereServs, List<HIS_ICD> listIcds, List<V_HIS_TREATMENT> listTreatment)
        {
            var groupByPatientIds = listSereServs.GroupBy(ss => ss.TDL_TREATMENT_ID).ToList();
            foreach (var groupPatientId in groupByPatientIds)
            {
                var treatment = listTreatment.First(s => s.ID == groupPatientId.First().TDL_TREATMENT_ID);
                var groupByServiceIds = groupPatientId.GroupBy(s => s.SERVICE_ID).ToList();
                foreach (var groupByServiceId in groupByServiceIds)
                {
                    if (groupByServiceId.Count() == 1)
                    {
                        var vHisSereServ = groupByServiceId.First();

                        var serviceReqs = listServiceReq.Where(o => groupByServiceId.Select(s => s.SERVICE_REQ_ID).Contains(o.ID)).ToList();
                        var lstssBill = ListSereServBill.Where(o => groupByServiceId.Select(s => s.ID).Contains(o.SERE_SERV_ID)).ToList();
                        var transactions = lstssBill != null ? ListTransaction.Where(o => lstssBill.Select(s => s.BILL_ID).Contains(o.ID)).ToList() : new List<HIS_TRANSACTION>();
                        var rdo = new Mrs00125RDO(vHisSereServ, treatment, serviceReqs, transactions);
                        ListSereServRdo.Add(rdo);
                    }
                    else
                    {
                        var vHisSereServ = groupByServiceId.First();
                        var virTotalPatientPrice = groupByServiceId.Where(s => ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(s.ID)).Select(s => s.VIR_TOTAL_PATIENT_PRICE).Sum();
                        var virTotalHeinPrice = groupByServiceId.Select(s => s.VIR_TOTAL_HEIN_PRICE).Sum();
                        var virTotalPrice = groupByServiceId.Select(s => s.VIR_TOTAL_PRICE).Sum();
                        var virTotalPriceNoExpend = groupByServiceId.Select(s => s.VIR_TOTAL_PRICE_NO_EXPEND).Sum();
                        var amout = groupByServiceId.Where(s => ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(s.ID) || s.HEIN_RATIO == MRS.MANAGER.Config.HisServiceTypeCFG.SERVICE_TYPE_ID__HEIN_RATIO_0).Select(s => s.AMOUNT).Sum();
                        vHisSereServ.VIR_TOTAL_PATIENT_PRICE = virTotalPatientPrice;
                        vHisSereServ.VIR_TOTAL_HEIN_PRICE = virTotalHeinPrice;
                        vHisSereServ.VIR_TOTAL_PRICE = virTotalPrice;
                        vHisSereServ.VIR_TOTAL_PRICE_NO_EXPEND = virTotalPriceNoExpend;
                        vHisSereServ.AMOUNT = amout;

                        //var treatment = listTreatment.First(s => s.ID == vHisSereServ.TDL_TREATMENT_ID);
                        var serviceReqs = listServiceReq.Where(o => groupByServiceId.Select(s => s.SERVICE_REQ_ID).Contains(o.ID)).ToList();
                        var lstssBill = ListSereServBill.Where(o => groupByServiceId.Select(s => s.ID).Contains(o.SERE_SERV_ID)).ToList();
                        var transactions = lstssBill != null ? ListTransaction.Where(o => lstssBill.Select(s => s.BILL_ID).Contains(o.ID)).ToList() : new List<HIS_TRANSACTION>();
                        var rdo = new Mrs00125RDO(vHisSereServ, treatment, serviceReqs, transactions);
                        ListSereServRdo.Add(rdo);
                    }
                }
            }
            TOTAL_PRICE = ListSereServRdo.Select(s => s.VIR_TOTAL_PATIENT_PRICE).Sum();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("SERVICE_REPORT_NAME", SERVICE_REPORT_NAME);
                dicSingleTag.Add("PATIENT_TYPE_NAME", PATIENT_TYPE_NAME);
                dicSingleTag.Add("TOTAL_PRICE", (double?)TOTAL_PRICE);
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

                objectTag.AddObjectData(store, "Report", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
