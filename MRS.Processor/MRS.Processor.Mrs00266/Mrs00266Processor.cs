using MOS.MANAGER.HisHeinServiceType;
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
using HTC.EFMODEL.DataModels; 
using HTC.Filter; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisDepartment; 
using MRS.MANAGER.Config;
using HTC.MANAGER.Manager;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get; 

namespace MRS.Processor.Mrs00266
{
    public class Mrs00266Processor : AbstractProcessor
    {
        private List<Mrs00266RDO> _listMrs00266Rdos = new List<Mrs00266RDO>(); 

        List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>(); 
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>(); 
        List<V_HTC_PERIOD_DEPARTMENT> ListPeriodDepartment = new List<V_HTC_PERIOD_DEPARTMENT>(); 
        List<HTC_EXPENSE_TYPE> ListExpenseType = new List<HTC_EXPENSE_TYPE>(); 
        List<HTC_REVENUE> ListRevenue = new List<HTC_REVENUE>(); 
        List<V_HTC_EXPENSE> ListExpense = new List<V_HTC_EXPENSE>(); 

        List<Mrs00266RDO> ListTreatmentRdo = new List<Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicServiceTypeRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicServiceRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicDepartmentRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicDepartExpenseTypeRdo = new Dictionary<long, Mrs00266RDO>(); 

        Dictionary<long, Mrs00266RDO> dicExpenseTypeTTRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicExpenseTypeGTRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicExpenseTypeCPBRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicExpenseTypeRdo = new Dictionary<long, Mrs00266RDO>(); 
        Dictionary<long, Mrs00266RDO> dicExpenseTypeLeafRdo = new Dictionary<long, Mrs00266RDO>(); 

        Dictionary<string, object> dicSingleData = new Dictionary<string, object>(); 

        const int limit = 20000; 

        public Mrs00266Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00266Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00266Filter)reportFilter); 
            var result = true; 
            try
            {
                CommonParam paramGet = new CommonParam();
                HtcExpenseViewFilterQuery expenseFilter = new HtcExpenseViewFilterQuery(); 
                expenseFilter.PERIOD_ID = filter.PERIOD_ID; 
                ListExpense = new HtcExpenseManager(paramGet).Get<List<V_HTC_EXPENSE>>(expenseFilter); 

                GetRevenue(filter);

                HtcPeriodDepartmentViewFilterQuery periodDepartmentFilter = new HtcPeriodDepartmentViewFilterQuery(); 
                periodDepartmentFilter.PERIOD_ID = filter.PERIOD_ID; 
                ListPeriodDepartment = new HtcPeriodDepartmentManager(paramGet).Get<List<V_HTC_PERIOD_DEPARTMENT>>(periodDepartmentFilter); 

                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery(); 
                ListService = new HisServiceManager(paramGet).GetView(serviceFilter); 
                ListDepartment = new HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());
                if (ListDepartment != null && ListPeriodDepartment != null)
                {
                    ListDepartment = ListDepartment.Where(o => ListPeriodDepartment.Exists(p => p.DEPARTMENT_CODE == o.DEPARTMENT_CODE)).ToList();
                }
                ListExpenseType = new HtcExpenseTypeManager(paramGet).Get<List<HTC_EXPENSE_TYPE>>(new HtcExpenseTypeFilterQuery()); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co Exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00266."); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void GetRevenue(Mrs00266Filter filter)
        {
            int count = limit; 
            int start = 0; 
            int totalCount = 0; 
            CommonParam paramGet = new CommonParam();

            HtcRevenueFilterQuery revenueFilter = new HtcRevenueFilterQuery(); 
            revenueFilter.PERIOD_ID = filter.PERIOD_ID; 
            paramGet.Start = start; 
            paramGet.Limit = limit; 
            var lstData = new HtcRevenueManager(paramGet).Get<List<HTC_REVENUE>>(revenueFilter); 
            if (paramGet.HasException)
                throw new DataMisalignedException("Co exception xay ra tai DOAGET trong qua trinh lay du lieu HTC_REVENUE Mrs00266"); 
            Inventec.Common.Logging.LogSystem.Info("=========== Bat dau check lstData"); 
            if (IsNotNullOrEmpty(lstData))
            {
                Inventec.Common.Logging.LogSystem.Info("lstData Co Du Lieu: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
                ListRevenue.AddRange(lstData); 
                totalCount = paramGet.Count ?? 0; 
                count += limit; 
                start += limit; 
                Inventec.Common.Logging.LogSystem.Info("=========== Bat Dau Vong Lap While"); 
                while (count < totalCount)
                {
                    CommonParam paramAgain = new CommonParam(); 
                    paramAgain.Start = start; 
                    paramAgain.Limit = limit; 
                    var datas = new HtcRevenueManager(paramGet).Get<List<HTC_REVENUE>>(revenueFilter); 
                    if (paramAgain.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DOAGET trong qua trinh lay du lieu HTC_REVENUE Mrs00266"); 
                    }
                    if (IsNotNullOrEmpty(datas))
                    {
                        ListRevenue.AddRange(datas); 
                    }
                    count += limit; 
                    start += limit; 
                }
                Inventec.Common.Logging.LogSystem.Info("=========== Ket thuc vong lap While tong du lieu lay duoc: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ListRevenue.Count), ListRevenue.Count)); 
            }
            Inventec.Common.Logging.LogSystem.Info("=========== Ket thuc check lstData"); 
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

                if (!IsNotNullOrEmpty(ListService))
                {
                    throw new Exception("Khong lay duoc danh sach HisService"); 
                }

                GeneralListTreatmentRdo(); 

                Dictionary<string, List<V_HTC_PERIOD_DEPARTMENT>> dicPeriodDepartment = new Dictionary<string, List<V_HTC_PERIOD_DEPARTMENT>>(); 
                Dictionary<long, Dictionary<string, List<V_HTC_EXPENSE>>> dicExpense = new Dictionary<long, Dictionary<string, List<V_HTC_EXPENSE>>>(); 
                Dictionary<string, Dictionary<string, List<HTC_REVENUE>>> dicRevenueReq = new Dictionary<string, Dictionary<string, List<HTC_REVENUE>>>(); 
                //Dictionary<string, Dictionary<string, List<HTC_REVENUE>>> dicRevenueExe = new Dictionary<string, Dictionary<string, List<HTC_REVENUE>>>(); 
                Dictionary<string, Dictionary<string, List<HTC_REVENUE>>> dicRevenueService = new Dictionary<string, Dictionary<string, List<HTC_REVENUE>>>(); 

                Dictionary<long, V_HIS_SERVICE> dicServiceParent = new Dictionary<long, V_HIS_SERVICE>(); 
                Dictionary<string, List<V_HIS_SERVICE>> dicService = new Dictionary<string, List<V_HIS_SERVICE>>(); 
                Dictionary<long, V_HIS_SERVICE> dicServiceHightTech = new Dictionary<long, V_HIS_SERVICE>(); 

                foreach (var item in ListService)
                {
                    if (!dicServiceTypeRdo.ContainsKey(item.SERVICE_TYPE_ID))
                    {
                        Mrs00266RDO rdoServiceType = new Mrs00266RDO(); 
                        rdoServiceType.SERVICE_TYPE_CODE = item.SERVICE_TYPE_CODE; 
                        rdoServiceType.SERVICE_TYPE_ID = item.SERVICE_TYPE_ID; 
                        rdoServiceType.SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME; 
                        dicServiceTypeRdo[item.SERVICE_TYPE_ID] = rdoServiceType; 
                    }

                    if (item.HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                    //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__HIGHTECH)
                    {
                        dicServiceHightTech[item.ID] = item; 
                    }
                    else if (!item.PARENT_ID.HasValue && item.HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                    //HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_ID__HIGHTECH)
                    {
                        dicServiceParent[item.ID] = item; 
                    }
                    else if (ListRevenue.Exists(o => o.SERVICE_CODE == item.SERVICE_CODE && o.SERVICE_TYPE_CODE == item.SERVICE_TYPE_CODE))
                    {
                        if (!dicService.ContainsKey(item.PARENT_ID.Value + "_" + item.SERVICE_TYPE_ID))
                            dicService[item.PARENT_ID.Value + "_" + item.SERVICE_TYPE_ID] = new List<V_HIS_SERVICE>(); 
                        dicService[item.PARENT_ID.Value + "_" + item.SERVICE_TYPE_ID].Add(item); 
                    }
                }

                //Tong tien ngân sách
                //decimal totalExpenseTypeNS = 0; 

                //Dich vu ky thuat cao
                Mrs00266RDO rdoHightTech = new Mrs00266RDO(); 
                rdoHightTech.SERVICE_TYPE_ID = 0; 
                rdoHightTech.SERVICE_TYPE_CODE = "00"; 
                rdoHightTech.SERVICE_TYPE_NAME = "DVKTC"; 
                dicServiceTypeRdo[rdoHightTech.SERVICE_TYPE_ID] = rdoHightTech; 

                if (IsNotNullOrEmpty(ListPeriodDepartment))
                {
                    foreach (var item in ListPeriodDepartment)
                    {
                        if (!dicPeriodDepartment.ContainsKey(item.DEPARTMENT_CODE))
                            dicPeriodDepartment[item.DEPARTMENT_CODE] = new List<V_HTC_PERIOD_DEPARTMENT>(); 
                        dicPeriodDepartment[item.DEPARTMENT_CODE].Add(item); 
                    }
                }

                if (IsNotNullOrEmpty(ListExpense))
                {
                    foreach (var item in ListExpense)
                    {
                        //if (item.EXPENSE_TYPE_CODE == MANAGER.Config.HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_NS)
                        //{
                        //    totalExpenseTypeNS += item.PRICE; 
                        //}
                        //else
                        //{
                        if (!dicExpense.ContainsKey(item.EXPENSE_TYPE_ID))
                            dicExpense[item.EXPENSE_TYPE_ID] = new Dictionary<string, List<V_HTC_EXPENSE>>(); 
                        if (!dicExpense[item.EXPENSE_TYPE_ID].ContainsKey(item.DEPARTMENT_CODE))
                            dicExpense[item.EXPENSE_TYPE_ID][item.DEPARTMENT_CODE] = new List<V_HTC_EXPENSE>(); 
                        dicExpense[item.EXPENSE_TYPE_ID][item.DEPARTMENT_CODE].Add(item); 
                        //}
                    }
                }
                //dicSingleData.Add("TOTAL_EXPENSE_TYPE_NS", totalExpenseTypeNS); 
                if (IsNotNullOrEmpty(ListRevenue))
                {
                    foreach (var item in ListRevenue)
                    {
                        if (!dicRevenueReq.ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                            dicRevenueReq[item.REQUEST_DEPARTMENT_CODE] = new Dictionary<string, List<HTC_REVENUE>>(); 
                        if (!dicRevenueReq[item.REQUEST_DEPARTMENT_CODE].ContainsKey(item.EXECUTE_DEPARTMENT_CODE))
                            dicRevenueReq[item.REQUEST_DEPARTMENT_CODE][item.EXECUTE_DEPARTMENT_CODE] = new List<HTC_REVENUE>(); 
                        dicRevenueReq[item.REQUEST_DEPARTMENT_CODE][item.EXECUTE_DEPARTMENT_CODE].Add(item); 

                        //if (!dicRevenueExe.ContainsKey(item.EXECUTE_DEPARTMENT_CODE))
                        //    dicRevenueExe[item.EXECUTE_DEPARTMENT_CODE] = new Dictionary<string, List<HTC_REVENUE>>(); 
                        //if (!dicRevenueExe[item.EXECUTE_DEPARTMENT_CODE].ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                        //    dicRevenueExe[item.EXECUTE_DEPARTMENT_CODE][item.REQUEST_DEPARTMENT_CODE] = new List<HTC_REVENUE>(); 
                        //dicRevenueExe[item.EXECUTE_DEPARTMENT_CODE][item.REQUEST_DEPARTMENT_CODE].Add(item); 

                        if (!dicRevenueService.ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                            dicRevenueService[item.REQUEST_DEPARTMENT_CODE] = new Dictionary<string, List<HTC_REVENUE>>(); 
                        string key = item.SERVICE_CODE + "_" + item.SERVICE_TYPE_CODE; 
                        if (!dicRevenueService[item.REQUEST_DEPARTMENT_CODE].ContainsKey(key))
                            dicRevenueService[item.REQUEST_DEPARTMENT_CODE][key] = new List<HTC_REVENUE>(); 
                        dicRevenueService[item.REQUEST_DEPARTMENT_CODE][key].Add(item); 
                    }
                }

                ListDepartment = ListDepartment.OrderBy(o => o.NUM_ORDER).ThenBy(t => t.MODIFY_TIME).ToList(); 
                int count = 1; 
                foreach (var reqDepart in ListDepartment)
                {
                    System.Reflection.PropertyInfo pi = typeof(Mrs00266RDO).GetProperty("AMOUNT_" + count); 

                    #region ListTreatmentRdo
                    if (dicPeriodDepartment.ContainsKey(reqDepart.DEPARTMENT_CODE))
                    {
                        pi.SetValue(ListTreatmentRdo[0], (decimal)dicPeriodDepartment[reqDepart.DEPARTMENT_CODE].Sum(s => s.CLINICAL_DAY_AMOUNT ?? 0)); 
                        //Benh nhan vao thang khoa
                        pi.SetValue(ListTreatmentRdo[1], (decimal)dicPeriodDepartment[reqDepart.DEPARTMENT_CODE].Sum(s => s.FROM_EXAM_CLINICAL_AMOUNT ?? 0)); 
                        //Benh nhan vao tu khoa khac
                        pi.SetValue(ListTreatmentRdo[2], (decimal)dicPeriodDepartment[reqDepart.DEPARTMENT_CODE].Sum(s => s.VIR_FROM_OTHER_CLINICAL_AMOUNT ?? 0)); 
                        //benh nhan xuat vien tu khoa
                        pi.SetValue(ListTreatmentRdo[3], (decimal)dicPeriodDepartment[reqDepart.DEPARTMENT_CODE].Sum(s => s.END_TREATMENT_AMOUNT ?? 0)); 
                        //benh nhan xuat vien tu khoa khac
                        pi.SetValue(ListTreatmentRdo[4], (decimal)dicPeriodDepartment[reqDepart.DEPARTMENT_CODE].Sum(s => s.VIR_NOT_END_TREATMENT_AMOUNT ?? 0)); 
                    }
                    else
                    {
                        pi.SetValue(ListTreatmentRdo[0], (decimal)0); 
                        //Benh nhan vao thang khoa
                        pi.SetValue(ListTreatmentRdo[1], (decimal)0); 
                        //Benh nhan vao tu khoa khac
                        pi.SetValue(ListTreatmentRdo[2], (decimal)0); 
                        //benh nhan xuat vien tu khoa
                        pi.SetValue(ListTreatmentRdo[3], (decimal)0); 
                        //benh nhan xuat vien tu khoa khac
                        pi.SetValue(ListTreatmentRdo[4], (decimal)0); 
                    }
                    #endregion

                    #region ExpenseType
                    foreach (var expenseType in ListExpenseType)
                    {
                        Mrs00266RDO rdoExpenseType = new Mrs00266RDO(); 
                        if (HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs.Contains(expenseType.ID))
                        {
                            //if (expenseType.EXPENSE_TYPE_CODE == MANAGER.Config.HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_NS)
                            //{
                            //    continue; 
                            //}

                            if (!dicDepartExpenseTypeRdo.ContainsKey(expenseType.ID))
                            {
                                rdoExpenseType.DEPARTMENT_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                rdoExpenseType.DEPARTMENT_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                dicDepartExpenseTypeRdo[expenseType.ID] = rdoExpenseType; 
                            }
                            else
                            {
                                rdoExpenseType = dicDepartExpenseTypeRdo[expenseType.ID]; 
                            }

                            if (dicExpense.ContainsKey(expenseType.ID) && dicExpense[expenseType.ID].ContainsKey(reqDepart.DEPARTMENT_CODE))
                            {
                                decimal totalPrice = dicExpense[expenseType.ID][reqDepart.DEPARTMENT_CODE].Sum(s => s.PRICE); 
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
                        else
                        {
                            if (expenseType.EXPENSE_TYPE_CODE == HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPTT)
                            {
                                ProcessExpenseTypeTT(dicExpense, reqDepart, pi, expenseType, ref rdoExpenseType); 
                            }
                            else if (expenseType.EXPENSE_TYPE_CODE == HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPGT)
                            {
                                if (!dicExpenseTypeGTRdo.ContainsKey(expenseType.ID))
                                {
                                    rdoExpenseType.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                    rdoExpenseType.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                    rdoExpenseType.EXPENSE_TYPE_ID = expenseType.ID; 
                                    dicExpenseTypeGTRdo[expenseType.ID] = rdoExpenseType; 
                                }
                                else
                                {
                                    rdoExpenseType = dicExpenseTypeGTRdo[expenseType.ID]; 
                                }
                                if (dicExpense.ContainsKey(expenseType.ID) && dicExpense[expenseType.ID].ContainsKey(reqDepart.DEPARTMENT_CODE))
                                {
                                    decimal totalPrice = dicExpense[expenseType.ID][reqDepart.DEPARTMENT_CODE].Sum(s => s.PRICE); 
                                    pi.SetValue(rdoExpenseType, totalPrice); 
                                }
                                else
                                {
                                    pi.SetValue(rdoExpenseType, (decimal)0); 
                                }
                            }
                            else if (expenseType.EXPENSE_TYPE_CODE == HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPB)
                            {
                                if (!dicExpenseTypeCPBRdo.ContainsKey(expenseType.ID))
                                {
                                    rdoExpenseType.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                    rdoExpenseType.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                    rdoExpenseType.EXPENSE_TYPE_ID = expenseType.ID; 
                                    dicExpenseTypeCPBRdo[expenseType.ID] = rdoExpenseType; 
                                }
                                else
                                {
                                    rdoExpenseType = dicExpenseTypeCPBRdo[expenseType.ID]; 
                                }
                                if (dicExpense.ContainsKey(expenseType.ID) && dicExpense[expenseType.ID].ContainsKey(reqDepart.DEPARTMENT_CODE))
                                {
                                    decimal totalPrice = dicExpense[expenseType.ID][reqDepart.DEPARTMENT_CODE].Sum(s => s.PRICE); 
                                    pi.SetValue(rdoExpenseType, totalPrice); 
                                }
                                else
                                {
                                    pi.SetValue(rdoExpenseType, (decimal)0); 
                                }
                            }
                        }
                    }
                    #endregion

                    #region Department
                    foreach (var exeDepart in ListDepartment)
                    {
                        Mrs00266RDO rdoDepart = new Mrs00266RDO(); 
                        if (!dicDepartmentRdo.ContainsKey(exeDepart.ID))
                        {
                            rdoDepart.DEPARTMENT_ID = exeDepart.ID; 
                            rdoDepart.DEPARTMENT_CODE = exeDepart.DEPARTMENT_CODE; 
                            rdoDepart.DEPARTMENT_NAME = exeDepart.DEPARTMENT_NAME; 
                            dicDepartmentRdo[exeDepart.ID] = rdoDepart; 
                        }
                        else
                        {
                            rdoDepart = dicDepartmentRdo[exeDepart.ID]; 
                        }

                        decimal totalPrice = 0; 
                        if (dicRevenueReq.ContainsKey(reqDepart.DEPARTMENT_CODE) && dicRevenueReq[reqDepart.DEPARTMENT_CODE].ContainsKey(exeDepart.DEPARTMENT_CODE))
                        {
                            totalPrice = -dicRevenueReq[reqDepart.DEPARTMENT_CODE][exeDepart.DEPARTMENT_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                        }

                        //if (dicRevenueExe.ContainsKey(reqDepart.DEPARTMENT_CODE) && dicRevenueExe[reqDepart.DEPARTMENT_CODE].ContainsKey(exeDepart.DEPARTMENT_CODE))
                        //{
                        //    totalPrice += dicRevenueExe[reqDepart.DEPARTMENT_CODE][exeDepart.DEPARTMENT_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                        //}

                        pi.SetValue(rdoDepart, totalPrice); 
                    }
                    #endregion

                    #region Service and ServiceType
                    if (IsNotNullOrEmpty(dicServiceParent))
                    {
                        foreach (var dic in dicServiceParent)
                        {
                            string key = dic.Value.ID + "_" + dic.Value.SERVICE_TYPE_ID; 
                            if (dicService.ContainsKey(key))
                            {
                                Mrs00266RDO rdoParent = new Mrs00266RDO();
                                decimal totalPrice = 0;
                                if (!dicServiceRdo.ContainsKey(dic.Value.ID))
                                {
                                    rdoParent.SERVICE_ID = dic.Value.ID;
                                    rdoParent.SERVICE_CODE = dic.Value.SERVICE_CODE;
                                    rdoParent.SERVICE_NAME = dic.Value.SERVICE_NAME;
                                    rdoParent.SERVICE_TYPE_ID = dic.Value.SERVICE_TYPE_ID;
                                    dicServiceRdo[dic.Value.ID] = rdoParent;
                                }
                                else
                                {
                                    rdoParent = dicServiceRdo[dic.Value.ID];
                                }

                                var listChild = dicService[key]; 
                                foreach (var service in listChild)
                                {
                                    if (dicRevenueService.ContainsKey(reqDepart.DEPARTMENT_CODE) && dicRevenueService[reqDepart.DEPARTMENT_CODE].ContainsKey(service.SERVICE_CODE + "_" + service.SERVICE_TYPE_CODE))
                                    {
                                        totalPrice += dicRevenueService[reqDepart.DEPARTMENT_CODE][service.SERVICE_CODE + "_" + service.SERVICE_TYPE_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                                    }
                                }
                                pi.SetValue(rdoParent, totalPrice); 
                            }
                            else if (dicRevenueService.ContainsKey(reqDepart.DEPARTMENT_CODE) && dicRevenueService[reqDepart.DEPARTMENT_CODE].ContainsKey(dic.Value.SERVICE_CODE + "_" + dic.Value.SERVICE_TYPE_CODE))
                            {
                                Mrs00266RDO rdoParent = new Mrs00266RDO();
                                if (!dicServiceRdo.ContainsKey(dic.Value.ID))
                                {
                                    rdoParent.SERVICE_ID = dic.Value.ID;
                                    rdoParent.SERVICE_CODE = dic.Value.SERVICE_CODE;
                                    rdoParent.SERVICE_NAME = dic.Value.SERVICE_NAME;
                                    rdoParent.SERVICE_TYPE_ID = dic.Value.SERVICE_TYPE_ID;
                                    dicServiceRdo[dic.Value.ID] = rdoParent;
                                }
                                else
                                {
                                    rdoParent = dicServiceRdo[dic.Value.ID];
                                }
                                pi.SetValue(rdoParent, dicRevenueService[reqDepart.DEPARTMENT_CODE][dic.Value.SERVICE_CODE + "_" + dic.Value.SERVICE_TYPE_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                            }
                        }
                    }

                    Mrs00266RDO rdoDVKTC = new Mrs00266RDO(); 
                    if (!dicServiceRdo.ContainsKey(0))
                    {
                        rdoDVKTC.SERVICE_ID = 0; 
                        rdoDVKTC.SERVICE_NAME = "Dịch vụ kỹ thuật cao"; 
                        rdoDVKTC.SERVICE_TYPE_ID = 0; 
                        dicServiceRdo[0] = rdoDVKTC; 
                    }
                    else
                    {
                        rdoDVKTC = dicServiceRdo[0]; 
                    }
                    decimal price = 0; 
                    if (IsNotNullOrEmpty(dicServiceHightTech))
                    {
                        foreach (var dic in dicServiceHightTech)
                        {
                            if (dicRevenueService.ContainsKey(reqDepart.DEPARTMENT_CODE) && dicRevenueService[reqDepart.DEPARTMENT_CODE].ContainsKey(dic.Value.SERVICE_CODE + "_" + dic.Value.SERVICE_TYPE_CODE))
                            {
                                price += dicRevenueService[reqDepart.DEPARTMENT_CODE][dic.Value.SERVICE_CODE + "_" + dic.Value.SERVICE_TYPE_CODE].Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                            }
                        }
                    }
                    pi.SetValue(rdoDVKTC, price); 
                    #endregion

                    dicSingleData.Add("DEPARTMENT_NAME_" + count, reqDepart.DEPARTMENT_NAME); 
                    count++; 
                }

                //Set Doanh thu thuc te cua khoa
                count = 1; 
                foreach (var depart in ListDepartment)
                {
                    if (!dicDepartmentRdo.ContainsKey(depart.ID))
                    {
                        continue; 
                    }
                    var rdo = dicDepartmentRdo[depart.ID]; 
                    decimal totalPrice = 0; 
                    for (int i = 1;  i <= 50;  i++)
                    {
                        if (i == count)
                            continue; 
                        System.Reflection.PropertyInfo p = typeof(Mrs00266RDO).GetProperty("AMOUNT_" + i); 
                        var amount = p.GetValue(rdo); 
                        if (amount != null)
                        {
                            totalPrice -= Convert.ToDecimal(amount); 
                        }
                    }
                    System.Reflection.PropertyInfo pi = typeof(Mrs00266RDO).GetProperty("AMOUNT_" + count); 
                    pi.SetValue(rdo, totalPrice); 
                    count++; 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private void ProcessExpenseTypeTT(Dictionary<long, Dictionary<string, List<V_HTC_EXPENSE>>> dicExpense, HIS_DEPARTMENT reqDepart, System.Reflection.PropertyInfo pi, HTC_EXPENSE_TYPE expenseType, ref Mrs00266RDO rdoExpenseType)
        {
            try
            {
                if (!dicExpenseTypeTTRdo.ContainsKey(expenseType.ID))
                {
                    rdoExpenseType.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                    rdoExpenseType.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                    rdoExpenseType.EXPENSE_TYPE_ID = expenseType.ID; 
                    dicExpenseTypeTTRdo[expenseType.ID] = rdoExpenseType; 
                }
                var listParent = ListExpenseType.Where(o => o.PARENT_ID == expenseType.ID).ToList(); 
                if (IsNotNullOrEmpty(listParent))
                {
                    foreach (var parent in listParent)
                    {
                        Mrs00266RDO rdoParent = new Mrs00266RDO(); 
                        if (!dicExpenseTypeRdo.ContainsKey(parent.ID))
                        {
                            rdoParent.EXPENSE_TYPE_ID = parent.ID; 
                            rdoParent.EXPENSE_TYPE_CODE = parent.EXPENSE_TYPE_CODE; 
                            rdoParent.EXPENSE_TYPE_NAME = parent.EXPENSE_TYPE_NAME; 
                            dicExpenseTypeRdo[parent.ID] = rdoParent; 
                        }
                        var listLeaf = ListExpenseType.Where(o => o.PARENT_ID == parent.ID).ToList(); 
                        if (IsNotNullOrEmpty(listLeaf))
                        {
                            foreach (var leaf in listLeaf)
                            {
                                Mrs00266RDO rdoLeaf = new Mrs00266RDO(); 
                                if (!dicExpenseTypeLeafRdo.ContainsKey(leaf.ID))
                                {
                                    rdoLeaf.EXPENSE_TYPE_ID = leaf.ID; 
                                    rdoLeaf.EXPENSE_TYPE_CODE = leaf.EXPENSE_TYPE_CODE; 
                                    rdoLeaf.EXPENSE_TYPE_NAME = leaf.EXPENSE_TYPE_NAME; 
                                    rdoLeaf.PARENT_ID = parent.ID; 
                                    dicExpenseTypeLeafRdo[leaf.ID] = rdoLeaf; 
                                }
                                else
                                {
                                    rdoLeaf = dicExpenseTypeLeafRdo[leaf.ID]; 
                                }

                                if (dicExpense.ContainsKey(leaf.ID) && dicExpense[leaf.ID].ContainsKey(reqDepart.DEPARTMENT_CODE))
                                {
                                    decimal totalPrice = dicExpense[leaf.ID][reqDepart.DEPARTMENT_CODE].Sum(s => s.PRICE); 
                                    pi.SetValue(rdoLeaf, totalPrice); 
                                }
                                else
                                {
                                    pi.SetValue(rdoExpenseType, (decimal)0); 
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GeneralListTreatmentRdo()
        {
            try
            {
                Mrs00266RDO rdo1 = new Mrs00266RDO(); 
                rdo1.EXPENSE_ID = 1; 
                rdo1.EXPENSE_NAME = "- Số ngày điều trị"; 
                ListTreatmentRdo.Add(rdo1); 
                Mrs00266RDO rdo2 = new Mrs00266RDO(); 
                rdo2.EXPENSE_ID = 2; 
                rdo2.EXPENSE_NAME = "- Bệnh nhân vào thẳng khoa"; 
                ListTreatmentRdo.Add(rdo2); 
                Mrs00266RDO rdo3 = new Mrs00266RDO(); 
                rdo3.EXPENSE_ID = 3; 
                rdo3.EXPENSE_NAME = "- Bệnh nhân vào từ khoa khác"; 
                ListTreatmentRdo.Add(rdo3); 
                Mrs00266RDO rdo4 = new Mrs00266RDO(); 
                rdo4.EXPENSE_ID = 4; 
                rdo4.EXPENSE_NAME = "- Bệnh nhân xuất viện từ khoa"; 
                ListTreatmentRdo.Add(rdo4); 
                Mrs00266RDO rdo5 = new Mrs00266RDO(); 
                rdo5.EXPENSE_ID = 5; 
                rdo5.EXPENSE_NAME = "- Bệnh nhân xuất viện từ khoa khác"; 
                ListTreatmentRdo.Add(rdo5); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                foreach (var dic in dicSingleData)
                {
                    dicSingleTag[dic.Key] = dic.Value; 
                }
                objectTag.AddObjectData(store, "Treatment", ListTreatmentRdo); 
                objectTag.AddObjectData(store, "ServiceType", dicServiceTypeRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "Service", dicServiceRdo.Select(s => s.Value).ToList()); 
                List<Mrs00266RDO> listDepartExpenseRdo = new List<Mrs00266RDO>(); 
                if (dicDepartExpenseTypeRdo.Count > 0)
                {
                    listDepartExpenseRdo.AddRange(dicDepartExpenseTypeRdo.Select(s => s.Value).ToList()); 
                }
                if (dicDepartmentRdo.Count > 0)
                {
                    listDepartExpenseRdo.AddRange(dicDepartmentRdo.Select(s => s.Value).ToList()); 
                }
                objectTag.AddObjectData(store, "Department", listDepartExpenseRdo); 
                objectTag.AddObjectData(store, "ExpenseTypeTT", dicExpenseTypeTTRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "ExpenseTypeGT", dicExpenseTypeGTRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "ExpenseTypeCPB", dicExpenseTypeCPBRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "ExpenseType", dicExpenseTypeRdo.Select(s => s.Value).ToList()); 
                objectTag.AddObjectData(store, "ExpenseTypeLeaf", dicExpenseTypeLeafRdo.Select(s => s.Value).ToList()); 
                objectTag.AddRelationship(store, "ServiceType", "Service", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID"); 
                objectTag.AddRelationship(store, "ExpenseType", "ExpenseTypeLeaf", "EXPENSE_TYPE_ID", "PARENT_ID"); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
