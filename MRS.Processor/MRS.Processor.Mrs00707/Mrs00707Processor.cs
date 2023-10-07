using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
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
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMedicineTypeAcin; 
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean; 

namespace MRS.Processor.Mrs00707
{
    public class Mrs00707Processor : AbstractProcessor
    {
        private Mrs00707Filter filter;
        List<Mrs00707RDO> listTransaction = new List<Mrs00707RDO>();
        List<PrintLogUnique> listPrintLogUnique = new List<PrintLogUnique>();

       
           
        CommonParam paramGet = new CommonParam(); 
        public Mrs00707Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00707Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00707Filter)reportFilter; 
            try
            {
                listTransaction = new MRS.Processor.Mrs00707.ManagerSql().GetTransaction(filter);
                listPrintLogUnique = new MRS.Processor.Mrs00707.ManagerSql().GetPrintLog(filter)??new List<PrintLogUnique>();
              
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

                if (IsNotNullOrEmpty(listPrintLogUnique))
                {
                    foreach (var item in listPrintLogUnique)
                    {
                        if (filter.IS_PRINT != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.UNIQUE_CODE))
                            {
                                string[] uniques = item.UNIQUE_CODE.Split('_');
                                if (uniques.Length == 4)
                                {
                                    item.TRANSACTION_CODE = uniques[2];
                                }
                            }
                        }
                        if (filter.IS_PRINT_NEW != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.UNIQUE_CODE))
                            {
                                //Mps000339 TREATMENT_CODE: TRANSACTION_CODE:000000000010 10001
                                int id = item.UNIQUE_CODE.IndexOf("TRANSACTION_CODE:");
                                if(id>=0)
                                    item.TRANSACTION_CODE = item.UNIQUE_CODE.Substring(id + 17, 12);
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(listTransaction))
                {
                    foreach (var item in listTransaction)
                    {
                        var transactionSub = listTransaction.Where(o => o.TRANSACTION_CODE == item.TRANSACTION_CODE).ToList();
                        string maxExpMestCode = transactionSub.Max(m => m.EXP_MEST_CODE);
                        if (item.EXP_MEST_CODE == maxExpMestCode)
                        {
                            item.TDL_TOTAL_PRICE = item.AMOUNT - transactionSub.Where(o => o.EXP_MEST_CODE != item.EXP_MEST_CODE).Sum(s => s.TDL_TOTAL_PRICE);
                        }
                        item.IS_PRINTED = this.IsPrinted(item)?(short)1:(short)0;
                    }
                    var isPrint = filter.IS_PRINT ?? filter.IS_PRINT_NEW;
                    if (isPrint != null)
                    {

                        if (isPrint == false)
                        {
                            listTransaction = listTransaction.Where(o => o.IS_PRINTED==0).ToList();
                        }
                        else if (isPrint == true)
                        {
                            listTransaction = listTransaction.Where(o => o.IS_PRINTED == 1).ToList();
                        }
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

        private bool IsPrinted(Mrs00707RDO transaction)
        {
            var unique = this.listPrintLogUnique.FirstOrDefault(o => o.TRANSACTION_CODE == transaction.TRANSACTION_CODE);
            return (unique!=null);
           
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "Report", listTransaction);
            objectTag.AddObjectData(store, "CashierMest", listTransaction.GroupBy(o => new { o.CASHIER_LOGINNAME, o.MEDI_STOCK_CODE }).Select(p=>p.First()).ToList());
            objectTag.AddObjectData(store, "MediStock", listTransaction.GroupBy(o => o.MEDI_STOCK_CODE).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "CashierRoom", listTransaction.GroupBy(o => o.CASHIER_ROOM_CODE).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "CashierDate", listTransaction.GroupBy(o => new { o.CASHIER_LOGINNAME, o.TRANSACTION_DATE }).Select(p => p.First()).OrderBy(q=>q.CASHIER_LOGINNAME).ThenBy(r=>r.TRANSACTION_DATE).ToList());
        }

       
    }
}
