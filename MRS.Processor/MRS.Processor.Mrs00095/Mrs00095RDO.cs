using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00095
{
    class Mrs00095RDO
    {
        public string CASHIER_LOGINNAME { get;  set;  }
        public string CASHIER_USERNAME { get;  set;  }

        public decimal SERVICE_TOTAL_BILL_AMOUNT { get;  set;  }
        public decimal MEDICINE_TOTAL_BILL_AMOUNT { get;  set;  }
        public decimal MATERIAL_TOTAL_BILL_AMOUNT { get;  set;  }
        public decimal TOTAL_DEPOSIT_AMOUNT { get;  set;  }
        public decimal TOTAL_REPAY_AMOUNT { get;  set;  }
        public decimal TOTAL_EXEMPTION { get;  set;  }

        public Mrs00095RDO(List<V_HIS_TRANSACTION> hisTransactions, CommonParam paramGet)
        {
            try
            {
                CASHIER_LOGINNAME = hisTransactions.First().CASHIER_LOGINNAME; 
                CASHIER_USERNAME = hisTransactions.First().CASHIER_USERNAME; 
                TOTAL_DEPOSIT_AMOUNT = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT); 
                TOTAL_REPAY_AMOUNT = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT); 
                TOTAL_EXEMPTION = hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => (s.EXEMPTION ?? 0)); 
                ProcessAmountService(hisTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList(), paramGet); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessAmountService(List<V_HIS_TRANSACTION> Data, CommonParam paramGet)
        {
            try
            {
                if (Data != null && Data.Count > 0)
                {
                    int start = 0; 
                    int count = Data.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        List<long> transactionIds = Data.Skip(start).Take(limit).Select(s => s.ID).ToList(); 
                        HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery(); 
                        filterSereServBill.BILL_IDs = transactionIds; 
                        var listSereServBillSub = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(filterSereServBill); 
                        var listSereServId = listSereServBillSub.Select(s => s.SERE_SERV_ID).ToList(); 
                        List<V_HIS_SERE_SERV> hisSereServs = new List<V_HIS_SERE_SERV>(); 
                        if (listSereServId.Count > 0)
                        {
                            var skip = 0; 
                            while (listSereServId.Count - skip > 0)
                            {
                                var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                                HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery(); 
                                filterSereServ.IDs = listIDs; 
                                var listSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filterSereServ); 
                                hisSereServs.AddRange(listSereServSub); 
                            }
                        }

                        if (hisSereServs != null && hisSereServs.Count > 0)
                        {
                            MEDICINE_TOTAL_BILL_AMOUNT += hisSereServs.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0)); 
                            MATERIAL_TOTAL_BILL_AMOUNT += hisSereServs.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0)); 
                            SERVICE_TOTAL_BILL_AMOUNT += hisSereServs.Where(o =>
                                o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT &&
                                o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0)); 
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Error("Khong lay duoc V_HIS_SERE_SERV bang TRANSACTION_IDs. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => transactionIds), transactionIds)); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
