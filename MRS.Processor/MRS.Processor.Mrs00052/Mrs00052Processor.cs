using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00052
{
    public class Mrs00052Processor : AbstractProcessor
    {
        Mrs00052Filter castFilter = null;
        List<Mrs00052RDO> ListRdo = new List<Mrs00052RDO>();

        List<HIS_TRANSACTION> ListBill = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_ACCOUNT_BOOK> listHisAccountBook = new List<HIS_ACCOUNT_BOOK>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();

        public Mrs00052Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00052Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00052Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00052 " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                
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
                ProcessListBill();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListBill()
        {
            try
            {
                if (ListBill != null && ListBill.Count > 0)
                {
                    CommonParam paramGet = new CommonParam();
                    var Groups = ListBill.GroupBy(g => g.ACCOUNT_BOOK_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<HIS_TRANSACTION> listSub = group.ToList<HIS_TRANSACTION>();
                        if (listSub.Count > 0)
                        {
                            ProcessListBillFollowAccountBook(paramGet, listSub);
                        }
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("co exception xa ra tai DAOGET trong qua trinh tong hop du lieu.");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                ListBill.Clear();
            }
        }

        private void ProcessListBillFollowAccountBook(CommonParam paramGet, List<HIS_TRANSACTION> listBillAccountBook)
        {
            try
            {
                Mrs00052RDO rdo = new Mrs00052RDO();
                var accountBook = listHisAccountBook.FirstOrDefault(o => o.ID == listBillAccountBook[0].ACCOUNT_BOOK_ID) ?? new HIS_ACCOUNT_BOOK();
                rdo.ACCOUNT_BOOK_CODE = accountBook.ACCOUNT_BOOK_CODE;
                rdo.ACCOUNT_BOOK_NAME = accountBook.ACCOUNT_BOOK_NAME;
                rdo.CREATOR = accountBook.CREATOR; 
                var notCancels = listBillAccountBook.Where(o => o.IS_CANCEL != 1).ToList();
                if (notCancels != null && notCancels.Count > 0)
                {
                    rdo.TOTAL_TRANSACTION = notCancels.Count;
                    ProcessSereServFollowAccountBook(paramGet, notCancels, rdo);
                }
                rdo.TOTAL_TRANSACTION_CANCEL = listBillAccountBook.Count - rdo.TOTAL_TRANSACTION;
                ListRdo.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

       
        private void ProcessSereServFollowAccountBook(CommonParam paramGet, List<HIS_TRANSACTION> bills, Mrs00052RDO rdo)
        {
            try
            {
               var sereServBills = listHisSereServBill.Where(o=>bills.Exists(p=>p.ID==o.BILL_ID)).ToList()??new List<HIS_SERE_SERV_BILL>();
               var sereServs = listHisSereServ.Where(o=>sereServBills.Exists(p=>p.SERE_SERV_ID==o.ID)).ToList()??new List<HIS_SERE_SERV>();

               var totalCostPrices = sereServs.Where(o => o.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                        if (totalCostPrices != null && totalCostPrices.Count > 0)
                        {
                            rdo.TOTAL_COST_PRICE += totalCostPrices.Sum(s => s.VIR_PRICE*service(s).COGS)??0;//review
                        }

                        rdo.VIR_TOTAL_PATIENT_PRICE += sereServs.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private HIS_SERVICE service(HIS_SERE_SERV s)
        {
            return listHisService.FirstOrDefault(o => o.ID == s.SERVICE_ID) ?? new HIS_SERVICE();
        }

        private void LoadDataToRam()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                HisTransactionFilterQuery billFilter = new HisTransactionFilterQuery();
                billFilter.MODIFY_TIME_FROM = castFilter.TIME_FROM;
                billFilter.MODIFY_TIME_TO = castFilter.TIME_TO;
                billFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                ListBill = new HisTransactionManager(paramGet).Get(billFilter);
                var listTreatmentIds = ListBill.Select(o => o.TREATMENT_ID??0).ToList();
                if (IsNotNullOrEmpty(listTreatmentIds))
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = listIDs;
                        var listSereServsSub = new HisSereServManager(paramGet).Get(HisSereServfilter);
                        if (IsNotNullOrEmpty(listSereServsSub))
                            listHisSereServ.AddRange(listSereServsSub);
                        
                    }
                    skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                        HisSereServBillfilter.TDL_TREATMENT_IDs = listIDs;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(HisSereServBillfilter);
                        if (IsNotNullOrEmpty(listSereServBillSub))
                            listHisSereServBill.AddRange(listSereServBillSub);
                    }
                }
                listHisService = new HisServiceManager(paramGet).Get(new HisServiceFilterQuery());
                listHisAccountBook = new HisAccountBookManager(paramGet).Get(new HisAccountBookFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListBill.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("MODIFY_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("MODIFY_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

   
}
