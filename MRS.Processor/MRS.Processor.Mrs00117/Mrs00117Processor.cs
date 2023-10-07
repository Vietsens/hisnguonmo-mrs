using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServBill;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00117
{
    public class Mrs00117Processor : AbstractProcessor
    {
        List<Mrs00117RDO> ListSereServRdo = new List<Mrs00117RDO>();
        
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        Mrs00117Filter CastFilter = null;
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat;
        List<HIS_SERVICE_RETY_CAT> listServiceRetyCat;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();

        public Mrs00117Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00117Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00117Filter)this.reportFilter;
                var paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu HIS_SERVICE_RETY_CAT, filter: " +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));
                var metyFilterReportTypeCat = new HisReportTypeCatFilterQuery
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00117"
                };
                listReportTypeCat = new HisReportTypeCatManager(paramGet).Get(metyFilterReportTypeCat);

                var lstIdReportTypeCat = listReportTypeCat.Select(s => s.ID).ToList();
                var metyFilterServiceRetyCat = new HisServiceRetyCatFilterQuery
                {
                    REPORT_TYPE_CAT_IDs = lstIdReportTypeCat
                };
                listServiceRetyCat = new HisServiceRetyCatManager(paramGet).Get(metyFilterServiceRetyCat);

                var lstServiceId = listServiceRetyCat.Select(s => s.SERVICE_ID).ToList();
                var HisTransactionfilter = new HisTransactionFilterQuery
                {
                    TRANSACTION_TIME_FROM = CastFilter.DATE_FROM,
                    TRANSACTION_TIME_TO = CastFilter.TDATE_TO,
                    IS_CANCEL = false,
                    TRANSACTION_TYPE_ID=IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT
                };
                var listHisTransaction = new HisTransactionManager(paramGet).Get(HisTransactionfilter);
                var listBillId = listHisTransaction.Select(o => o.ID).ToList();
                if (IsNotNullOrEmpty(listBillId))
                {
                    var skip = 0;
                    while (listBillId.Count - skip > 0)
                    {
                        var listIDs = listBillId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                        filterSereServBill.BILL_IDs = listIDs;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                        listHisSereServBill.AddRange(listSereServBillSub);
                    }
                }
                var listTreatmentId = listHisTransaction.Select(o => o.TREATMENT_ID??0).ToList();
                //DV - thanh toan
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery();
                        filterSereServ.TREATMENT_IDs = listIDs;
                        filterSereServ.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSereServ);
                        listHisSereServ.AddRange(listSereServSub);
                    }
                    listHisSereServ = listHisSereServ.Where(o => lstServiceId.Contains(o.SERVICE_ID) 
                        && listHisSereServBill.Exists(p=>p.SERE_SERV_ID==o.ID)).ToList();
                }

                //HSDT
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery filterTreatment = new HisTreatmentFilterQuery();
                        filterTreatment.IDs = listIDs;
                        var listTreatmentSub = new HisTreatmentManager(paramGet).Get(filterTreatment);
                        listHisTreatment.AddRange(listTreatmentSub);
                    }
                }

               if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu HIS_SERVICE_RETY_CAT, MRS00117." +
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


        private void ProcessFilterData(List<HIS_REPORT_TYPE_CAT> listReportTypeCats, List<HIS_SERVICE_RETY_CAT> listServiceRetyCats,
            List<HIS_SERE_SERV> hisSereServs)
        {
            var lstReportTypeCatIds = listReportTypeCats.Select(s => s.ID).OrderBy(s => s).ToList();
            if (lstReportTypeCatIds.Count < 13)
            {
                var number = 13 - lstReportTypeCatIds.Count;
                for (var i = 0; i < number; i++)
                {
                    lstReportTypeCatIds.Add(000);
                }
            }

            var listSereSers = hisSereServs.GroupBy(s => s.TDL_TREATMENT_ID??0);
            foreach (var listSereSer in listSereSers)
            {
                var treatment = listHisTreatment.FirstOrDefault(o => o.ID == listSereSer.Key) ?? new HIS_TREATMENT();
                var listTotalPriceInOneTreatments = new List<decimal>();
                foreach (var lstReportTypeCatId in lstReportTypeCatIds)
                {
                    var listIdInGroupService = listServiceRetyCats.Where(s => s.REPORT_TYPE_CAT_ID == lstReportTypeCatId).Select(s => s.SERVICE_ID).ToList();
                    var totalPriceInOneTreatment = listSereSer.Where(s => listIdInGroupService.Contains(s.SERVICE_ID)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                    listTotalPriceInOneTreatments.Add(totalPriceInOneTreatment??0);
                }
                var rdo = new Mrs00117RDO(listTotalPriceInOneTreatments);
                rdo.PATIENT_NAME = listSereSer.Select(s => treatment.TDL_PATIENT_NAME).First();
                rdo.PATIENT_DATE_OF_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSereSer.Select(s => treatment.TDL_PATIENT_DOB).First());
                ListSereServRdo.Add(rdo);
            }
        }

        protected override bool ProcessData()
        {
            var result = false;
            try
            {
                ProcessFilterData(listReportTypeCat, listServiceRetyCat, listHisSereServ);
                result = true;
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
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TDATE_TO));
                objectTag.AddObjectData(store, "Report", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
