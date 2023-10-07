using AutoMapper; 
using HTC.EFMODEL.DataModels; 
using HTC.Filter; 
using Inventec.Common.DateTime; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisService; 
using MRS.SDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using HTC.MANAGER.Core.ExpenseBO.HtcExpense.Get;
using HTC.MANAGER.Manager;
using HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get;
using HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get; 

namespace MRS.Processor.Mrs00264
{
    public class Mrs00264Processor : AbstractProcessor
    {

        CommonParam paramGet = new CommonParam(); 

        List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>(); 
        List<V_HIS_SERVICE> ListServiceParent = new List<V_HIS_SERVICE>(); 
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>(); 
        List<V_HTC_PERIOD_DEPARTMENT> ListPeriodDepartment = new List<V_HTC_PERIOD_DEPARTMENT>(); 
        List<HTC_EXPENSE_TYPE> ListExpenseType = new List<HTC_EXPENSE_TYPE>(); 
        List<HTC_REVENUE> ListRevenue = new List<HTC_REVENUE>(); 
        List<Mrs00264RDO> ListTreatmentRdo = new List<Mrs00264RDO>(); 
        List<V_HTC_EXPENSE> ListExpense = new List<V_HTC_EXPENSE>(); 
        Dictionary<long, Mrs00264RDO> dicServiceRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicServiceTypeRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicExpenseTypeRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicExpenseTypeLeafRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicExpenseTypeGTRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicExpenseTypeCPBRdo = new Dictionary<long, Mrs00264RDO>(); 
        Dictionary<long, Mrs00264RDO> dicExpenseTypeTTRdo = new Dictionary<long, Mrs00264RDO>(); 

        Dictionary<string, object> dicSingleData = new Dictionary<string, object>(); 

        string EXPENSE_TYPE_CODE__CPTT = HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPTT; 
        string EXPENSE_TYPE_CODE__CPGT = HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPGT; 
        string EXPENSE_TYPE_CODE__CPB = HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_CODE_CPB; 

        string period_name;

        const long SERVICE_ID__DVKTC = -1;

        public Mrs00264Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00264Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00264Filter)reportFilter); 
            var result = true; 
            try
            {
                HtcExpenseViewFilterQuery expenseFilter = new HtcExpenseViewFilterQuery(); 
                expenseFilter.PERIOD_ID = filter.PERIOD_ID = 161; 
                ListExpense = new HtcExpenseManager(paramGet).Get<List<V_HTC_EXPENSE>>(expenseFilter); 
                ListExpense = ListExpense.Where(o => !HtcExpenseTypeCFG.HTC_EXPENSE_TYPE_IDs.Contains(o.EXPENSE_TYPE_ID)).ToList();
                HtcRevenueFilterQuery revenueFilter = new HtcRevenueFilterQuery();
                revenueFilter.PERIOD_ID = filter.PERIOD_ID; 
                ListRevenue = new HtcRevenueManager(paramGet).Get<List<HTC_REVENUE>>(revenueFilter);

                HtcPeriodDepartmentViewFilterQuery periodDepartmentFilter = new HtcPeriodDepartmentViewFilterQuery(); 
                periodDepartmentFilter.PERIOD_ID = filter.PERIOD_ID;
                ListPeriodDepartment = new HtcPeriodDepartmentManager(paramGet).Get<List<V_HTC_PERIOD_DEPARTMENT>>(periodDepartmentFilter); 

                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery(); 
                ListService = new HisServiceManager(paramGet).GetView(serviceFilter); 
                ListServiceParent = ListService.Where(o => o.PARENT_ID == null).ToList(); 
                ListDepartment = new HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());

                ListExpenseType = new HtcExpenseTypeManager(paramGet).Get<List<HTC_EXPENSE_TYPE>>(new HtcExpenseTypeFilterQuery()); 
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
                try
                {
                    if (!IsNotNullOrEmpty(ListDepartment) || ListDepartment.Count > 50)
                    {
                        throw new Exception("Danh sach khoa null hoac lon hon 50."); 
                    }

                    Inventec.Common.Logging.LogSystem.Info("Lay danh sach TreatmentRdo"); 
                    GeneralListTreatmentRdo(); 
                    Inventec.Common.Logging.LogSystem.Info(" Ket thuc Lay danh sach TreatmentRdo"); 

                    Dictionary<string, List<V_HTC_PERIOD_DEPARTMENT>> dicPeriodDepartment = new Dictionary<string, List<V_HTC_PERIOD_DEPARTMENT>>(); 
                    Dictionary<string, Dictionary<long, List<V_HTC_EXPENSE>>> dicExpense = new Dictionary<string, Dictionary<long, List<V_HTC_EXPENSE>>>(); 
                    Dictionary<string, Dictionary<string, HTC_REVENUE>> dicRevenue = new Dictionary<string, Dictionary<string, HTC_REVENUE>>(); 

                    Inventec.Common.Logging.LogSystem.Info("Kiem tra listPeriodDepartment"); 
                    if (IsNotNullOrEmpty(ListPeriodDepartment))
                    {
                        Inventec.Common.Logging.LogSystem.Info("Duyet list periodDeparment"); 

                        foreach (var item in ListPeriodDepartment)
                        {
                            if (String.IsNullOrEmpty(period_name))
                                period_name = item.PERIOD_NAME; 
                            if (!dicPeriodDepartment.ContainsKey(item.DEPARTMENT_CODE))
                                dicPeriodDepartment[item.DEPARTMENT_CODE] = new List<V_HTC_PERIOD_DEPARTMENT>(); 
                            dicPeriodDepartment[item.DEPARTMENT_CODE].Add(item); //dic khoa 
                        }
                        Inventec.Common.Logging.LogSystem.Info("Ket thuc duyet listperiodDepartment"); 
                    }

                    Inventec.Common.Logging.LogSystem.Info(" Kiem tra list expense"); 
                    if (IsNotNullOrEmpty(ListExpense))
                    {
                        Inventec.Common.Logging.LogSystem.Info("Duyet listExpense"); 
                        foreach (var item in ListExpense)
                        {
                            if (String.IsNullOrEmpty(period_name))
                                period_name = item.PERIOD_NAME; 
                            if (!dicExpense.ContainsKey(item.DEPARTMENT_CODE))
                                dicExpense[item.DEPARTMENT_CODE] = new Dictionary<long, List<V_HTC_EXPENSE>>(); 
                            if (!dicExpense[item.DEPARTMENT_CODE].ContainsKey(item.EXPENSE_TYPE_ID))
                                dicExpense[item.DEPARTMENT_CODE][item.EXPENSE_TYPE_ID] = new List<V_HTC_EXPENSE>(); 
                            dicExpense[item.DEPARTMENT_CODE][item.EXPENSE_TYPE_ID].Add(item); //dic cac dic chi phi theo loại chi phí theo khoa
                        }
                        Inventec.Common.Logging.LogSystem.Info("Ket thuc duyet listExpense"); 
                    }

                    Inventec.Common.Logging.LogSystem.Info("Kiem tra listRevenue"); 
                    if (IsNotNullOrEmpty(ListRevenue))
                    {
                        Inventec.Common.Logging.LogSystem.Info("Duyet listRevenue"); 
                        foreach (var item in ListRevenue)
                        {
                            if (!dicRevenue.ContainsKey(item.REQUEST_DEPARTMENT_CODE))
                                dicRevenue[item.REQUEST_DEPARTMENT_CODE] = new Dictionary<string, HTC_REVENUE>(); 
                            if (!dicRevenue[item.REQUEST_DEPARTMENT_CODE].ContainsKey(item.SERVICE_CODE))
                                dicRevenue[item.REQUEST_DEPARTMENT_CODE][item.SERVICE_CODE] = new HTC_REVENUE(); 
                            dicRevenue[item.REQUEST_DEPARTMENT_CODE][item.SERVICE_CODE] = item; //dic các dic khoản thu theo dịch vụ theo khoa
                        }
                        Inventec.Common.Logging.LogSystem.Info("Ket thuc duyet listRevenue"); 
                    }

                    ListDepartment = ListDepartment.OrderBy(o => o.NUM_ORDER).ToList(); 
                    int count = 1; 
                    Inventec.Common.Logging.LogSystem.Info("Duyet listDepartment"); 
                    foreach (var item in ListDepartment)
                    {
                        #region treatmentAmountRdo
                        if (dicPeriodDepartment.ContainsKey(item.DEPARTMENT_CODE))
                        {
                            System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                            //So ngay dieu tri
                            pi.SetValue(ListTreatmentRdo[0], (decimal)dicPeriodDepartment[item.DEPARTMENT_CODE].Sum(s => s.CLINICAL_DAY_AMOUNT ?? 0)); 
                            //Benh nhan vao thang khoa
                            pi.SetValue(ListTreatmentRdo[1], (decimal)dicPeriodDepartment[item.DEPARTMENT_CODE].Sum(s => s.FROM_EXAM_CLINICAL_AMOUNT ?? 0)); 
                            //Benh nhan vao tu khoa khac
                            pi.SetValue(ListTreatmentRdo[2], (decimal)dicPeriodDepartment[item.DEPARTMENT_CODE].Sum(s => s.VIR_FROM_OTHER_CLINICAL_AMOUNT ?? 0)); 
                            //benh nhan xuat vien tu khoa
                            pi.SetValue(ListTreatmentRdo[3], (decimal)dicPeriodDepartment[item.DEPARTMENT_CODE].Sum(s => s.END_TREATMENT_AMOUNT ?? 0)); 
                            //benh nhan xuat vien tu khoa khac
                            pi.SetValue(ListTreatmentRdo[4], (decimal)dicPeriodDepartment[item.DEPARTMENT_CODE].Sum(s => s.VIR_NOT_END_TREATMENT_AMOUNT ?? 0)); 

                        }
                        #endregion

                        #region revenue
                        Inventec.Common.Logging.LogSystem.Info("Check dicRevenue"); 
                        if (dicRevenue.ContainsKey(item.DEPARTMENT_CODE))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Duyet listService"); 
                            foreach (var service in ListServiceParent)
                            {
                                Mrs00264RDO rdo = new Mrs00264RDO(); 
                                if (dicServiceRdo.ContainsKey(service.ID))
                                    rdo = dicServiceRdo[service.ID]; 
                                else
                                {
                                    rdo.SERVICE_ID = service.ID; 
                                    rdo.SERVICE_CODE = service.SERVICE_CODE; 
                                    rdo.SERVICE_NAME = service.SERVICE_NAME; 
                                    rdo.SERVICE_TYPE_ID = service.SERVICE_TYPE_ID; 
                                    rdo.SERVICE_TYPE_CODE = service.SERVICE_TYPE_CODE; 
                                    rdo.SERVICE_TYPE_NAME = service.SERVICE_TYPE_NAME; 
                                    dicServiceRdo[service.ID] = rdo; 
                                }
                                Inventec.Common.Logging.LogSystem.Info("Check dicRevenue"); 
                                List<string> listSub = ListService.Where(o => o.SERVICE_TYPE_ID == service.SERVICE_TYPE_ID && o.PARENT_ID == service.PARENT_ID && o.HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC).Select(p => p.SERVICE_CODE).ToList(); 
                                if (IsNotNullOrEmpty(listSub))
                                {
                                    Inventec.Common.Logging.LogSystem.Info("Check dicRevenue co"); 
                                    System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                    decimal amount = 0; 
                                    foreach (var revenue in listSub)
                                        if (dicRevenue[item.DEPARTMENT_CODE].ContainsKey(revenue))///
                                        {
                                            amount += dicRevenue[item.DEPARTMENT_CODE][revenue].VIR_TOTAL_PRICE ?? 0; 
                                        }
                                    pi.SetValue(rdo, amount); 
                                }
                            }
                            //Dịch vụ kỹ thuật cao:
                            Mrs00264RDO rdo1 = new Mrs00264RDO();
                            if (dicServiceRdo.ContainsKey(SERVICE_ID__DVKTC))
                                rdo1 = dicServiceRdo[SERVICE_ID__DVKTC]; 
                            else
                            {
                                rdo1.SERVICE_ID = SERVICE_ID__DVKTC; 
                                rdo1.SERVICE_CODE = "DVKTC"; 
                                rdo1.SERVICE_NAME = "Dịch vụ kỹ thuật cao";
                                rdo1.SERVICE_TYPE_ID = SERVICE_ID__DVKTC; 
                                rdo1.SERVICE_TYPE_CODE = "DVKTC"; 
                                rdo1.SERVICE_TYPE_NAME = "Dịch vụ kỹ thuật cao";
                                dicServiceRdo[SERVICE_ID__DVKTC] = rdo1; 
                            }

                            Inventec.Common.Logging.LogSystem.Info("Check dicRevenue"); 
                            List<string> listSubDVKTC = ListService.Where(o => o.HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC).Select(p => p.SERVICE_CODE).ToList(); 
                            if (IsNotNullOrEmpty(listSubDVKTC))
                            {
                                Inventec.Common.Logging.LogSystem.Info("Check dicRevenue co"); 
                                System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                decimal amount = 0; 
                                foreach (var revenue in listSubDVKTC)
                                    if (dicRevenue[item.DEPARTMENT_CODE].ContainsKey(revenue))///
                                    {
                                        amount += dicRevenue[item.DEPARTMENT_CODE][revenue].VIR_TOTAL_PRICE ?? 0; 
                                    }
                                pi.SetValue(rdo1, amount); 
                            }
                        }

                        #endregion

                        #region Expense
                        Inventec.Common.Logging.LogSystem.Info("Check dicExpense"); 
                        if (dicExpense.ContainsKey(item.DEPARTMENT_CODE))
                        {
                            var rootTT = ListExpenseType.FirstOrDefault(o => o.EXPENSE_TYPE_CODE == EXPENSE_TYPE_CODE__CPTT); 
                            Mrs00264RDO rdoRoot = new Mrs00264RDO(); 
                            Inventec.Common.Logging.LogSystem.Info("dicExpenseTypeTTRdo.ContainsKey(rootTT.ID)"); 
                            if (dicExpenseTypeTTRdo.ContainsKey(rootTT.ID))
                            {
                                rdoRoot = dicExpenseTypeTTRdo[rootTT.ID]; 
                            }
                            else
                            {
                                rdoRoot.EXPENSE_TYPE_ID = rootTT.ID; 
                                rdoRoot.EXPENSE_TYPE_CODE = rootTT.EXPENSE_TYPE_CODE; 
                                rdoRoot.EXPENSE_TYPE_NAME = rootTT.EXPENSE_TYPE_NAME; 
                                dicExpenseTypeTTRdo[rootTT.ID] = rdoRoot; 
                            }
                            Dictionary<long, decimal> dicParentAmount = new Dictionary<long, decimal>(); 
                            Inventec.Common.Logging.LogSystem.Info("foreach (var expenseType in ListExpenseType)"); 
                            foreach (var expenseType in ListExpenseType)
                            {
                                if (expenseType.EXPENSE_TYPE_CODE == EXPENSE_TYPE_CODE__CPGT)
                                {
                                    Mrs00264RDO rdo = new Mrs00264RDO(); 
                                    if (dicExpenseTypeGTRdo.ContainsKey(expenseType.ID))
                                    {
                                        rdo = dicExpenseTypeGTRdo[expenseType.ID]; 
                                    }
                                    else
                                    {
                                        rdo.EXPENSE_TYPE_ID = expenseType.ID; 
                                        rdo.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                        rdo.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                        dicExpenseTypeGTRdo[expenseType.ID] = rdo; 
                                    }
                                    if (dicExpense[item.DEPARTMENT_CODE].ContainsKey(expenseType.ID))
                                    {
                                        System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                        pi.SetValue(rdo, dicExpense[item.DEPARTMENT_CODE][expenseType.ID].Sum(s => s.PRICE)); 
                                    }
                                }
                                else if (expenseType.EXPENSE_TYPE_CODE == EXPENSE_TYPE_CODE__CPB)
                                {
                                    Mrs00264RDO rdo = new Mrs00264RDO(); 
                                    if (dicExpenseTypeCPBRdo.ContainsKey(expenseType.ID))
                                    {
                                        rdo = dicExpenseTypeCPBRdo[expenseType.ID]; 
                                    }
                                    else
                                    {
                                        rdo.EXPENSE_TYPE_ID = expenseType.ID; 
                                        rdo.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                        rdo.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                        dicExpenseTypeCPBRdo[expenseType.ID] = rdo; 
                                    }
                                    if (dicExpense[item.DEPARTMENT_CODE].ContainsKey(expenseType.ID))
                                    {
                                        System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                        pi.SetValue(rdo, dicExpense[item.DEPARTMENT_CODE][expenseType.ID].Sum(s => s.PRICE)); 
                                    }
                                }
                                else if (expenseType.ID != rootTT.ID)
                                {
                                    if (expenseType.PARENT_ID == rootTT.ID)
                                    {
                                        Inventec.Common.Logging.LogSystem.Info("!dicExpenseTypeRdo.ContainsKey(expenseType.ID)"); 
                                        if (!dicExpenseTypeRdo.ContainsKey(expenseType.ID))
                                        {
                                            Mrs00264RDO rdo = new Mrs00264RDO(); 
                                            rdo.EXPENSE_TYPE_ID = expenseType.ID; 
                                            rdo.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                            rdo.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                            rdo.PARENT_ID = expenseType.PARENT_ID; 
                                            dicExpenseTypeRdo[expenseType.ID] = rdo; 
                                        }

                                    }
                                    else
                                    {
                                        Inventec.Common.Logging.LogSystem.Info("dicExpense[item.DEPARTMENT_CODE].ContainsKey(expenseType.ID)"); 
                                        Inventec.Common.Logging.LogSystem.Info(count + ":" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => expenseType), expenseType)); 
                                        if (dicExpense[item.DEPARTMENT_CODE].ContainsKey(expenseType.ID))
                                        {
                                            Mrs00264RDO rdo = new Mrs00264RDO(); 
                                            if (dicExpenseTypeLeafRdo.ContainsKey(expenseType.ID))
                                            {
                                                rdo = dicExpenseTypeLeafRdo[expenseType.ID]; 
                                            }
                                            else
                                            {
                                                rdo.EXPENSE_TYPE_ID = expenseType.ID; 
                                                rdo.EXPENSE_TYPE_CODE = expenseType.EXPENSE_TYPE_CODE; 
                                                rdo.EXPENSE_TYPE_NAME = expenseType.EXPENSE_TYPE_NAME; 
                                                rdo.PARENT_ID = expenseType.PARENT_ID; 
                                                dicExpenseTypeLeafRdo[expenseType.ID] = rdo; 
                                            }
                                            System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                            var totalPrice = dicExpense[item.DEPARTMENT_CODE][expenseType.ID].Sum(s => s.PRICE); 
                                            pi.SetValue(rdo, totalPrice); 
                                            Inventec.Common.Logging.LogSystem.Info("check va add dicParentAmount"); 
                                            if (expenseType.PARENT_ID.HasValue)
                                            {
                                                if (!dicParentAmount.ContainsKey(expenseType.PARENT_ID.Value))
                                                    dicParentAmount[expenseType.PARENT_ID.Value] = 0; 
                                                Inventec.Common.Logging.LogSystem.Info("bat dau add dicParentAmount"); 
                                                dicParentAmount[expenseType.PARENT_ID.Value] += totalPrice; 
                                                Inventec.Common.Logging.LogSystem.Info("ket thuc check va add dicParentAmount"); 
                                            }
                                        }
                                    }
                                }
                            }
                            Inventec.Common.Logging.LogSystem.Info("Duyet listExpenseTypeRdo"); 
                            foreach (var dic in dicExpenseTypeRdo)
                            {
                                System.Reflection.PropertyInfo pi = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                                if (dicParentAmount.ContainsKey(dic.Key))
                                {
                                    pi.SetValue(dic.Value, dicParentAmount[dic.Key]); 
                                }
                            }
                            System.Reflection.PropertyInfo pi1 = typeof(Mrs00264RDO).GetProperty("AMOUNT_" + count); 
                            pi1.SetValue(rdoRoot, dicParentAmount.Sum(s => s.Value)); 

                        }
                        Inventec.Common.Logging.LogSystem.Info("ket thuc dicExpenseTypeRdo"); 
                        #endregion

                        dicSingleData.Add("DEPARTMENT_NAME_" + count, item.DEPARTMENT_NAME); 

                        count++; 
                    }

                    Inventec.Common.Logging.LogSystem.Info("Duyet dicExpenseTypeRdo"); 
                    foreach (var dic in dicExpenseTypeRdo)
                    {
                        var listLevel1 = ListExpenseType.Where(o => o.PARENT_ID == dic.Key).ToList(); 
                        foreach (var level1 in listLevel1)
                        {
                            if (dicExpenseTypeLeafRdo.ContainsKey(level1.ID))
                            {
                                var rdo = dicExpenseTypeLeafRdo[level1.ID]; 
                                dic.Value.AMOUNT_1 += rdo.AMOUNT_1 ?? 0; 
                            }
                        }
                    }

                    Inventec.Common.Logging.LogSystem.Info("Ket thuc dicExpenseTypeRdo"); 
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex); 
                }
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

            foreach (var data in dicSingleData)
                dicSingleTag.Add(data.Key, data.Value); 
            objectTag.AddObjectData(store, "Treatment", ListTreatmentRdo); 
            var typeService = dicServiceRdo.Select(s => s.Value).GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList(); 
            objectTag.AddObjectData(store, "ServiceType", typeService); 
            objectTag.AddObjectData(store, "Service", dicServiceRdo.Select(s => s.Value).ToList()); 
            objectTag.AddObjectData(store, "ExpenseType", dicExpenseTypeRdo.Select(s => s.Value).ToList()); 
            objectTag.AddObjectData(store, "ExpenseTypeLeaf", dicExpenseTypeLeafRdo.Select(s => s.Value).ToList()); 
            objectTag.AddObjectData(store, "ExpenseTypeTT", dicExpenseTypeTTRdo.Select(s => s.Value).ToList()); 
            objectTag.AddObjectData(store, "ExpenseTypeGT", dicExpenseTypeGTRdo.Select(s => s.Value).ToList()); 
            objectTag.AddObjectData(store, "ExpenseTypeCPB", dicExpenseTypeCPBRdo.Select(s => s.Value).ToList()); 
            objectTag.AddRelationship(store, "ExpenseType", "ExpenseTypeLeaf", "EXPENSE_TYPE_ID", "PARENT_ID");  objectTag.AddRelationship(store, "ServiceType", "Service", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID"); 

        }

        private void GeneralListTreatmentRdo()
        {
            try
            {
                Mrs00264RDO rdo1 = new Mrs00264RDO(); 
                rdo1.EXPENSE_ID = 1; 
                rdo1.EXPENSE_NAME = "- Số ngày điều trị"; 
                ListTreatmentRdo.Add(rdo1); 
                Mrs00264RDO rdo2 = new Mrs00264RDO(); 
                rdo2.EXPENSE_ID = 2; 
                rdo2.EXPENSE_NAME = "- Bệnh nhân vào thẳng khoa"; 
                ListTreatmentRdo.Add(rdo2); 
                Mrs00264RDO rdo3 = new Mrs00264RDO(); 
                rdo3.EXPENSE_ID = 3; 
                rdo3.EXPENSE_NAME = "- Bệnh nhân vào từ khoa khác"; 
                ListTreatmentRdo.Add(rdo3); 
                Mrs00264RDO rdo4 = new Mrs00264RDO(); 
                rdo4.EXPENSE_ID = 4; 
                rdo4.EXPENSE_NAME = "- Bệnh nhân xuất viện từ khoa"; 
                ListTreatmentRdo.Add(rdo4); 
                Mrs00264RDO rdo5 = new Mrs00264RDO(); 
                rdo5.EXPENSE_ID = 5; 
                rdo5.EXPENSE_NAME = "- Bệnh nhân xuất viện từ khoa khác"; 
                ListTreatmentRdo.Add(rdo5); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


    }
}
