using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using HTC.Filter; 
using HTC.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisDepartment;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get;
using HTC.MANAGER.Manager;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get; 

namespace MRS.Processor.Mrs00265
{
    public class Mrs00265Processor : AbstractProcessor
    {
        List<V_HTC_EXPENSE> ListExpense; 
        List<HTC_REVENUE> ListRevenue; 
        List<HIS_DEPARTMENT> ListDepartment; 
        List<HTC_EXPENSE_TYPE> ListExpenseType; 

        List<Mrs00265RDO> listPatientRdo = new List<Mrs00265RDO>(); 
        Dictionary<long, Mrs00265RDO> dicExpenseTypeRdo = new Dictionary<long, Mrs00265RDO>(); 
        Dictionary<string, Mrs00265RDO> dicDepartmentRdo = new Dictionary<string, Mrs00265RDO>(); 

        Dictionary<string, object> dicSingleData = new Dictionary<string, object>(); 

        public Mrs00265Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00265Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00265Filter)reportFilter); 
            var result = true; 
            try
            {
                CommonParam paramGet = new CommonParam();

                HtcExpenseViewFilterQuery expenseFilter = new HtcExpenseViewFilterQuery(); 
                expenseFilter.PERIOD_ID = filter.PERIOD_ID; 
                expenseFilter.EXPENSE_TYPE_IDs = HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs; 
                ListExpense = new HtcExpenseManager(paramGet).Get<List<V_HTC_EXPENSE>>(expenseFilter);

                HtcRevenueFilterQuery revenueFilter = new HtcRevenueFilterQuery(); 
                revenueFilter.PERIOD_ID = filter.PERIOD_ID; 
                ListRevenue = new HtcRevenueManager(paramGet).Get<List<HTC_REVENUE>>(revenueFilter); 

                ListDepartment = new HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());

                HtcExpenseTypeFilterQuery expTypeFilter = new HtcExpenseTypeFilterQuery(); 
                expTypeFilter.IDs = HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs; 
                ListExpenseType = new HtcExpenseTypeManager(paramGet).Get<List<HTC_EXPENSE_TYPE>>(expTypeFilter); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co Exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00265."); 
                }
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
                if (!IsNotNullOrEmpty(ListDepartment) || ListDepartment.Count > 50)
                {
                    throw new Exception("Danh sach khoa null hoac lon hon 50."); 
                }

                if (!IsNotNullOrEmpty(ListExpenseType))
                {
                    throw new Exception("Khong lay duoc danh sach ExpenseType"); 
                }

                ListDepartment = ListDepartment.OrderBy(o => o.NUM_ORDER).ToList(); 

                Dictionary<string, decimal> dicRevenuePrice = new Dictionary<string, decimal>(); 
                Dictionary<string, Dictionary<string, List<HTC_REVENUE>>> dicRevenue = new Dictionary<string, Dictionary<string, List<HTC_REVENUE>>>(); 
                Dictionary<long, Dictionary<string, List<V_HTC_EXPENSE>>> dicExpense = new Dictionary<long, Dictionary<string, List<V_HTC_EXPENSE>>>(); 

                if (IsNotNullOrEmpty(ListExpense))
                {
                    foreach (var item in ListExpense)
                    {
                        if (!HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs.Contains(item.EXPENSE_TYPE_ID))
                            continue; 
                        if (!dicExpense.ContainsKey(item.EXPENSE_TYPE_ID))
                            dicExpense[item.EXPENSE_TYPE_ID] = new Dictionary<string, List<V_HTC_EXPENSE>>(); 
                        if (!dicExpense[item.EXPENSE_TYPE_ID].ContainsKey(item.DEPARTMENT_CODE))
                            dicExpense[item.EXPENSE_TYPE_ID][item.DEPARTMENT_CODE] = new List<V_HTC_EXPENSE>(); 
                        dicExpense[item.EXPENSE_TYPE_ID][item.DEPARTMENT_CODE].Add(item); 
                    }
                }

                if (IsNotNullOrEmpty(ListRevenue))
                {
                    foreach (var item in ListRevenue)
                    {
                        if (!dicRevenue.ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                            dicRevenue[item.REQUEST_DEPARTMENT_CODE] = new Dictionary<string, List<HTC_REVENUE>>(); 
                        if (!dicRevenue[item.REQUEST_DEPARTMENT_CODE].ContainsKey(item.EXECUTE_DEPARTMENT_CODE))
                            dicRevenue[item.REQUEST_DEPARTMENT_CODE][item.EXECUTE_DEPARTMENT_CODE] = new List<HTC_REVENUE>(); 
                        dicRevenue[item.REQUEST_DEPARTMENT_CODE][item.EXECUTE_DEPARTMENT_CODE].Add(item); 

                        if (!dicRevenuePrice.ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                            dicRevenuePrice[item.REQUEST_DEPARTMENT_CODE] = 0; 
                        dicRevenuePrice[item.REQUEST_DEPARTMENT_CODE] += (item.VIR_TOTAL_PRICE ?? 0); 
                    }
                }

                Mrs00265RDO rdoPatient = new Mrs00265RDO(); 
                int count = 1; 
                foreach (var depart in ListDepartment)
                {
                    System.Reflection.PropertyInfo pi = typeof(Mrs00265RDO).GetProperty("AMOUNT_" + count); 
                    if (dicRevenuePrice.ContainsKey(depart.DEPARTMENT_CODE))
                    {
                        pi.SetValue(rdoPatient, dicRevenuePrice[depart.DEPARTMENT_CODE]); 
                    }
                    else
                    {
                        pi.SetValue(rdoPatient, (decimal)0); 
                    }

                    foreach (var expenseType in ListExpenseType)
                    {
                        if (!HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs.Contains(expenseType.ID))
                            continue; 
                        Mrs00265RDO rdoExpenseType = new Mrs00265RDO(); 
                        if (!dicExpenseTypeRdo.ContainsKey(expenseType.ID))
                        {
                            rdoExpenseType.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                            rdoExpenseType.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                            dicExpenseTypeRdo[expenseType.ID] = rdoExpenseType; 
                        }
                        else
                        {
                            rdoExpenseType = dicExpenseTypeRdo[expenseType.ID]; 
                        }

                        if (dicExpense.ContainsKey(expenseType.ID) && dicExpense[expenseType.ID].ContainsKey(depart.DEPARTMENT_CODE))
                        {
                            decimal totalPrice = dicExpense[expenseType.ID][depart.DEPARTMENT_CODE].Sum(s => s.PRICE); 
                            if (expenseType.IS_PLUS == IMSys.DbConfig.HTC_RS.HTC_EXPENSE_TYPE.IS_PLUS__TRUE)
                            {
                                pi.SetValue(rdoExpenseType, totalPrice); 
                            }
                            else
                            {
                                pi.SetValue(rdoExpenseType, -totalPrice); 
                            }
                        }
                        else
                        {
                            pi.SetValue(rdoExpenseType, (decimal)0); 
                        }
                    }

                    foreach (var exeDepart in ListDepartment)
                    {
                        Mrs00265RDO rdoDepart = new Mrs00265RDO(); 
                        if (!dicDepartmentRdo.ContainsKey(exeDepart.DEPARTMENT_CODE))
                        {
                            rdoDepart.EXPENSE_TYPE_CODE = exeDepart.DEPARTMENT_CODE; 
                            rdoDepart.EXPENSE_TYPE_NAME = exeDepart.DEPARTMENT_NAME; 
                            dicDepartmentRdo[exeDepart.DEPARTMENT_CODE] = rdoDepart; 
                        }
                        else
                        {
                            rdoDepart = dicDepartmentRdo[exeDepart.DEPARTMENT_CODE]; 
                        }

                        if (dicRevenue.ContainsKey(depart.DEPARTMENT_CODE) && dicRevenue[depart.DEPARTMENT_CODE].ContainsKey(exeDepart.DEPARTMENT_CODE))
                        {
                            decimal totalPrice = -dicRevenue[depart.DEPARTMENT_CODE][exeDepart.DEPARTMENT_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                            pi.SetValue(rdoDepart, totalPrice); 
                        }
                        else
                        {
                            pi.SetValue(rdoDepart, (decimal)0); 
                        }
                    }
                    dicSingleData.Add("DEPARTMENT_NAME_" + count, depart.DEPARTMENT_NAME); 
                    count++; 
                }
                listPatientRdo.Add(rdoPatient); 
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                foreach (var dic in dicSingleData)
                {
                    dicSingleTag[dic.Key] = dic.Value; 
                }
                objectTag.AddObjectData(store, "Patient", listPatientRdo); 
                objectTag.AddObjectData(store, "ExpenseType", dicExpenseTypeRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "Department", dicDepartmentRdo.Select(s => s.Value).ToList()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
