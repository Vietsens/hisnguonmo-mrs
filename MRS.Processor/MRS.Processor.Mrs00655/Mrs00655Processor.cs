using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00655
{
    class Mrs00655Processor : AbstractProcessor
    {
        Mrs00655Filter castFilter;
        List<Mrs00655RDO> ListRdo = new List<Mrs00655RDO>();

        List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>();

        string cashierRoomName;
        string cashierName;

        public Mrs00655Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00655Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00655Filter)reportFilter;
                HisTransactionViewFilterQuery tranInFilter = new HisTransactionViewFilterQuery();
                tranInFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                tranInFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                tranInFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                //tranInFilter.CASHIER_ROOM_ID = castFilter.CASHIER_ROOM_ID;
                
                var transaction = new MOS.MANAGER.HisTransaction.HisTransactionManager().GetView(tranInFilter);
                if (IsNotNullOrEmpty(transaction) && castFilter.EXACT_CASHIER_ROOM_ID.HasValue)
                {
                    cashierRoomName = transaction.First(o => o.CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID.Value).CASHIER_ROOM_NAME;
                    transaction = transaction.Where(o => (o.CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID.Value)).ToList();
                }

                if (IsNotNullOrEmpty(transaction) && IsNotNull(castFilter.CASHIER_LOGINNAME))
                {
                    cashierName = transaction.First(o => o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).CASHIER_USERNAME;
                    transaction = transaction.Where(o => o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                }

                if (IsNotNullOrEmpty(transaction))
                {
                    //thu trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                    ListTransaction.AddRange(transaction);
                }

                HisTransactionViewFilterQuery tranOutFilter = new HisTransactionViewFilterQuery();
                //tranOutFilter.TRANSACTION_TIME_TO = castFilter.TIME_FROM;
                tranOutFilter.CANCEL_TIME_FROM = castFilter.TIME_FROM;
                tranOutFilter.CANCEL_TIME_TO = castFilter.TIME_TO;
                tranOutFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                
                var transactionCancel = new MOS.MANAGER.HisTransaction.HisTransactionManager().GetView(tranOutFilter);
                if (IsNotNullOrEmpty(transactionCancel) && IsNotNull(castFilter.CASHIER_LOGINNAME))
                {
                    transactionCancel = transactionCancel.Where(o => o.CANCEL_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                }

                if (IsNotNullOrEmpty(transactionCancel) && IsNotNull(castFilter.EXACT_CASHIER_ROOM_ID))
                {
                    transactionCancel = transactionCancel.Where(o => o.CANCEL_CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID).ToList();
                }

                if (IsNotNullOrEmpty(transactionCancel))
                {
                    //hủy trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                    ListTransaction.AddRange(transactionCancel);
                    //thu trong kì hoặc hủy trong kì
                }
                ListTransaction = ListTransaction.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                ListTransaction = ListTransaction.Where(o => o.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE && o.TDL_SERE_SERV_DEPOSIT_COUNT == null).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTransaction))
                {
                    ListTransaction = ListTransaction.OrderBy(o => o.TRANSACTION_TIME).ToList();
                    ListRdo = new List<Mrs00655RDO>();
                    var groupBook = ListTransaction.GroupBy(o => o.ACCOUNT_BOOK_ID).OrderBy(o => o.First().NUM_ORDER);
                    foreach (var gr in groupBook)
                    {
                        //hủy trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                        var cancelTran = gr.Where(o => o.IS_CANCEL == 1 && o.CANCEL_TIME >= castFilter.TIME_FROM && o.CANCEL_TIME <= castFilter.TIME_TO && (o.CANCEL_LOGINNAME == castFilter.CASHIER_LOGINNAME || null == castFilter.CASHIER_LOGINNAME) && (o.CANCEL_CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID || null == castFilter.EXACT_CASHIER_ROOM_ID)).OrderBy(o => o.NUM_ORDER).ToList();
                        //thu trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                        var tranNumOrder = gr.Where(o => o.TRANSACTION_TIME >= castFilter.TIME_FROM && o.TRANSACTION_TIME <= castFilter.TIME_TO && (o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME || null == castFilter.CASHIER_LOGINNAME) && (o.CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID || null == castFilter.EXACT_CASHIER_ROOM_ID)).ToList();
                        //var tranPrice = gr.Where(o => o.IS_CANCEL != 1 || (o.IS_CANCEL == 1 && (o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME || null == castFilter.CASHIER_LOGINNAME))).ToList();
                        //có hủy trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                        if (IsNotNullOrEmpty(cancelTran))
                        {
                            bool add = true;
                            for (int i = 0; i < cancelTran.Count; i++)
                            {
                                Mrs00655RDO rdo = new Mrs00655RDO();
                                rdo.ACCOUNT_BOOK_CODE = cancelTran.First().ACCOUNT_BOOK_CODE;
                                rdo.ACCOUNT_BOOK_NAME = cancelTran.First().ACCOUNT_BOOK_NAME;
                                rdo.ACCOUNT_BOOK_ID = cancelTran.First().ACCOUNT_BOOK_ID;
                                rdo.SYMBOL_CODE = cancelTran.First().SYMBOL_CODE;
                                rdo.TEMPLATE_CODE = cancelTran.First().TEMPLATE_CODE;
                                rdo.CANCEL_NUM_ORDER = cancelTran[i].NUM_ORDER;
                                //hủy trong kỳ,thu trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                                if (cancelTran[i].TRANSACTION_TIME >= castFilter.TIME_FROM && cancelTran[i].TRANSACTION_TIME <= castFilter.TIME_TO && (cancelTran[i].CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME || null == castFilter.CASHIER_LOGINNAME) && (cancelTran[i].CASHIER_ROOM_ID == castFilter.EXACT_CASHIER_ROOM_ID || null == castFilter.EXACT_CASHIER_ROOM_ID))
                                {
                                    rdo.CANCEL_TOTAL_PRICE_IN_TIME = cancelTran[i].AMOUNT;
                                    rdo.TRANSACTION_DATE = cancelTran[i].TRANSACTION_DATE / 1000000;
                                }
                                //hủy trong kỳ thu ngoài kỳ (check đồng thời thời gian, người thu và điểm thu)
                                else
                                {
                                    rdo.CANCEL_TOTAL_PRICE_OUT_TIME = cancelTran[i].AMOUNT;
                                    rdo.TRANSACTION_DATE = (castFilter.TIME_FROM ?? 0) / 1000000 - 1;
                                }

                                if (add)// && (cancelTran[i].CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME || castFilter.CASHIER_LOGINNAME == null) && (cancelTran[i].CANCEL_CASHIER_ROOM_ID == castFilter.CANCEL_CASHIER_ROOM_ID || null == castFilter.CANCEL_CASHIER_ROOM_ID))
                                {
                                    //có thu trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                                    if (IsNotNullOrEmpty(tranNumOrder))
                                    {
                                        rdo.NUM_ORDER_FROM = tranNumOrder.Min(o => o.NUM_ORDER);
                                        rdo.NUM_ORDER_TO = tranNumOrder.Max(o => o.NUM_ORDER);
                                        rdo.TOTAL_PRICE = tranNumOrder.Sum(s => s.AMOUNT);
                                    }
                                    //else
                                    //{
                                    //    rdo.NUM_ORDER_FROM = gr.Min(o => o.NUM_ORDER);
                                    //    rdo.NUM_ORDER_TO = gr.Max(o => o.NUM_ORDER);
                                    //}

                                    //if (IsNotNullOrEmpty(tranNumOrder))
                                    //else
                                    //    rdo.TOTAL_PRICE = gr.Sum(s => s.AMOUNT);

                                    add = false;
                                }

                                ListRdo.Add(rdo);
                            }
                        }
                        //không hủy trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                        else
                        {
                            //có thu trong kỳ (check đồng thời thời gian, người thu và điểm thu)
                            if (IsNotNullOrEmpty(tranNumOrder))
                            {
                            Mrs00655RDO rdo = new Mrs00655RDO();
                            rdo.ACCOUNT_BOOK_CODE = tranNumOrder.First().ACCOUNT_BOOK_CODE;
                            rdo.ACCOUNT_BOOK_NAME = tranNumOrder.First().ACCOUNT_BOOK_NAME;
                            rdo.ACCOUNT_BOOK_ID = tranNumOrder.First().ACCOUNT_BOOK_ID;
                            rdo.SYMBOL_CODE = tranNumOrder.First().SYMBOL_CODE;
                            rdo.TEMPLATE_CODE = tranNumOrder.First().TEMPLATE_CODE;
                            rdo.TOTAL_PRICE = tranNumOrder.Sum(s => s.AMOUNT);
                            rdo.TRANSACTION_DATE = (castFilter.TIME_FROM ?? 0) / 1000000;
                                rdo.NUM_ORDER_FROM = tranNumOrder.Min(o => o.NUM_ORDER);
                                rdo.NUM_ORDER_TO = tranNumOrder.Max(o => o.NUM_ORDER);

                            ListRdo.Add(rdo);
                            }
                            //else
                            //{
                               // rdo.NUM_ORDER_FROM = tranNumOrder.Min(o => o.NUM_ORDER);
                                //rdo.NUM_ORDER_TO = tranNumOrder.Max(o => o.NUM_ORDER);
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                dicSingleTag.Add("CASHIER_ROOM_NAME", cashierRoomName);
                dicSingleTag.Add("CASHIER_USERNAME", cashierName);

                decimal totalPrice = 0;
                decimal totalCancelIn = 0;
                decimal totalCancelOut = 0;
                decimal tamUngThu = 0;
                decimal tamUngNop = 0;
                if (IsNotNullOrEmpty(ListRdo))
                {
                    totalPrice = ListRdo.Sum(o => o.TOTAL_PRICE ?? 0);
                    totalCancelIn = ListRdo.Sum(o => o.CANCEL_TOTAL_PRICE_IN_TIME ?? 0);
                    totalCancelOut = ListRdo.Sum(o => o.CANCEL_TOTAL_PRICE_OUT_TIME ?? 0);
                    tamUngThu = totalPrice - totalCancelIn;
                    tamUngNop = totalPrice - totalCancelIn - totalCancelOut;
                }

                dicSingleTag.Add("TOTAL_PRICE", totalPrice);
                dicSingleTag.Add("CANCEL_TOTAL_PRICE_IN_TIME", totalCancelIn);
                dicSingleTag.Add("CANCEL_TOTAL_PRICE_OUT_TIME", totalCancelOut);

                dicSingleTag.Add("TAM_UNG_THU", Inventec.Common.Number.Convert.NumberToStringRoundAuto(tamUngThu, 0));
                dicSingleTag.Add("TAM_UNG_NOP", Inventec.Common.Number.Convert.NumberToStringRoundAuto(tamUngNop, 0));

                dicSingleTag.Add("TAM_UNG_THU_STR", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(tamUngThu).ToString()));
                dicSingleTag.Add("TAM_UNG_NOP_STR", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(tamUngNop).ToString()));

                var groupAccoutBook = ListRdo.GroupBy(o => new { o.TRANSACTION_DATE, o.ACCOUNT_BOOK_ID }).Select(s => s.First()).ToList();

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportBook", groupAccoutBook);
                string[] key = new string[] { "TRANSACTION_DATE", "ACCOUNT_BOOK_ID" };
                objectTag.AddRelationship(store, "ReportBook", "Report", key, key);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
