using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisAccountBook;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Data;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisSereServDeposit;

namespace MRS.Processor.Mrs00314
{
    public class Mrs00314Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private Dictionary<string, Mrs00314RDO> dicRdo = new Dictionary<string, Mrs00314RDO>();
        private List<MOS.EFMODEL.DataModels.HIS_TREATMENT> Treatments = new List<HIS_TREATMENT>();
        private decimal TotalAmount = 0,
            testFuexPriceSum = 0,
            testPriceSum = 0,
            fuexPriceSum = 0,
            diimPriceSum = 0,
            medicinePriceSum = 0,
            bloodPriceSum = 0,
            surgMisuPriceSum = 0,
            materialPriceSum = 0,
            examPriceSum = 0,
            otherPriceSum = 0,
            anPriceSum = 0,
            tranPriceSum = 0,
            bedPriceSum = 0,
            totalPriceSum = 0,
            totalHeinPriceSum = 0,
            discountSum = 0,
            totalPrice100Sum = 0,
            totalPrice5Sum = 0,
            totalPrice20Sum = 0,
            totalPrice0Sum = 0,
            patientPricePaySum = 0,
            endoPriceSum = 0,
            suimPriceSum = 0,
            SaleMedicinePriceSum = 0;

        Mrs00314Filter filter = null;
        //private List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        private Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();
        List<List<DataTable>> dataObject = new List<List<DataTable>>();
        private List<HIS_DEPARTMENT> listEndDepartment = new List<HIS_DEPARTMENT>();

        Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();

        Dictionary<long, V_HIS_SERVICE> dicParent = new Dictionary<long, V_HIS_SERVICE>();

        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCategory = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();

        List<V_HIS_MATERIAL_TYPE> listMaterial = new List<V_HIS_MATERIAL_TYPE>();

        List<HIS_HEIN_SERVICE_TYPE> listHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();

        //private List<HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        //private List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        string MaterialPriceOption = "";

        public Mrs00314Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00314Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            filter = ((Mrs00314Filter)reportFilter);
            var result = true;
            CommonParam param = new CommonParam();
            try
            {
                listMaterial = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                if (new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell<Mrs00314Filter>(ref dicSingleKey, ref dataObject, filter, this.reportTemplate.REPORT_TEMPLATE_URL, 15))
                {
                    return true;
                }
                //if (filter.TRANSACTION_TIME_FROM != null && filter.TRANSACTION_TIME_TO != null)
                //{
                //    Treatments = new ManagerSql().GetTreatment(filter);
                //}
                //else
                {
                    HisTreatmentFilterQuery filtermain = new HisTreatmentFilterQuery();
                    if (filter.TIME_FROM != null && filter.TIME_TO != null)
                    {
                        filtermain.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                        filtermain.FEE_LOCK_TIME_TO = filter.TIME_TO;
                        filtermain.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                    if (filter.TRANSACTION_TIME_FROM != null && filter.TRANSACTION_TIME_TO != null)
                    {
                        filtermain.FEE_LOCK_TIME_FROM = filter.TRANSACTION_TIME_FROM;
                        filtermain.FEE_LOCK_TIME_TO = filter.TRANSACTION_TIME_TO;
                        filtermain.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }

                    if (filter.OUT_TIME_FROM != null && filter.OUT_TIME_TO != null)
                    {
                        filtermain.OUT_TIME_FROM = filter.OUT_TIME_FROM;
                        filtermain.OUT_TIME_TO = filter.OUT_TIME_TO;
                        filtermain.IS_PAUSE = true;
                    }

                    filtermain.END_DEPARTMENT_IDs = filter.END_DEPARTMENT_IDs;

                    Treatments = new HisTreatmentManager(paramGet).Get(filtermain);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    Treatments = Treatments.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    Treatments = Treatments.Where(o => filter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                dicTreatment = Treatments.ToDictionary(o => o.ID);


                List<long> endDepartmentIds = Treatments.Select(o => o.END_DEPARTMENT_ID ?? 0).Distinct().ToList();
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IDs = endDepartmentIds;
                listEndDepartment = new MOS.MANAGER.HisDepartment.HisDepartmentManager(new CommonParam()).Get(departmentFilter);
                GetParent();
                GetCategory();
                HeinServiceType();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetParent()
        {
            HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
            var services = new HisServiceManager(new CommonParam()).GetView(serviceFilter);
            if (services != null)
            {
                foreach (var item in services)
                {
                    if (item.PARENT_ID != null)
                    {
                        var parent = services.FirstOrDefault(o => o.ID == item.PARENT_ID);
                        if (parent != null)
                        {
                            dicParent.Add(item.ID, parent);
                        }
                        else
                        {
                            dicParent.Add(item.ID, new V_HIS_SERVICE());
                        }
                    }
                }
            }
        }

        private void GetCategory()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
            var serviceRetyCats = new HisServiceRetyCatManager(new CommonParam()).GetView(serviceRetyCatFilter);
            if (serviceRetyCats != null)
            {
                foreach (var item in serviceRetyCats)
                {
                    if (!dicCategory.ContainsKey(item.SERVICE_ID))
                    {
                        dicCategory.Add(item.SERVICE_ID, item);
                    }
                }
            }
        }

        private void HeinServiceType()
        {
            HisHeinServiceTypeFilterQuery HeinServiceTypeFilter = new HisHeinServiceTypeFilterQuery();
            listHeinServiceType = new HisHeinServiceTypeManager(new CommonParam()).Get(HeinServiceTypeFilter);
            
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (dataObject.Count > 0)
                {
                    return true;
                }
                dicRdo.Clear();
                string key = "{0}_{1}_{2}";
                if (filter.IS_NOT_SPLIT_ROOM == true)
                {
                    key = key.Replace("_{2}", "");
                }
                if (filter.IS_NOT_SPLIT_DEPA == true)
                {
                    key = key.Replace("_{1}", "");
                }

                int start = 0;

                if (Treatments != null && Treatments.Count > 0)
                {
                    int count = Treatments.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var treatmentLimit = Treatments.Skip(start).Take(limit).ToList();
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.TDL_SERVICE_TYPE_IDs = filter.SERVICE_TYPE_IDs;
                        sereServFilter.PATIENT_TYPE_IDs = filter.PATIENT_TYPE_IDs;
                        //sereServFilter.IS_EXPEND = false;
                        sereServFilter.HAS_EXECUTE = true;
                        sereServFilter.TREATMENT_IDs = treatmentLimit.Select(o => o.ID).ToList();

                        var sereServLimits = treatmentLimit.Count > 0 ? new HisSereServManager(param).Get(sereServFilter) : new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                        //ListSereServ.AddRange(sereServLimits);

                        //danh sách chuyển khoa
                        var departmentTranLimists = GetDepartmentTran(treatmentLimit.Select(o => o.ID).ToList());

                        //danh sách giao dịch
                        var transactionLimists = GetTransaction(treatmentLimit.Select(o => o.ID).ToList());

                        //danh sách tạm thu dịch vụ
                        var sereServDepositLimists = GetSereServDeposit(treatmentLimit.Select(o => o.ID).ToList());

                        ////danh sách thanh toán dịch vụ
                        //var sereServBillLimists = GetSereServBill(treatmentLimit.Select(o => o.ID).ToList());

                        //mua thuoc
                        var saleExpMestLimists = GetSaleExpMest(treatmentLimit.Select(o => o.ID).ToList());
                        if (saleExpMestLimists != null)
                        {
                            var groupByTreatment = saleExpMestLimists.GroupBy(o => o.TDL_TREATMENT_ID).ToList();
                            foreach (var item in groupByTreatment)
                            {
                                var treatment = treatmentLimit.FirstOrDefault(o => o.ID == item.First().TDL_TREATMENT_ID);
                                if (treatment != null)
                                {
                                    var groupKey = string.Format(key, treatment.ID, treatment.LAST_DEPARTMENT_ID, treatment.END_ROOM_ID??0);
                                    var transactionSub = transactionLimists.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                                    if (!dicRdo.ContainsKey(groupKey))
                                    {
                                        dicRdo[groupKey] = new Mrs00314RDO();
                                        if (filter.IS_NOT_SPLIT_DEPA != true)
                                        {
                                            dicRdo[groupKey].REQUEST_DEPARTMENT_ID = treatment.LAST_DEPARTMENT_ID??0;
                                            dicRdo[groupKey].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                            dicRdo[groupKey].DEPARTMENT_ROOM_NAME = dicRdo[groupKey].REQUEST_DEPARTMENT_NAME;
                                        }
                                        if (filter.IS_NOT_SPLIT_ROOM != true)
                                        {
                                            dicRdo[groupKey].REQUEST_ROOM_ID = treatment.END_ROOM_ID ?? 0;
                                            dicRdo[groupKey].REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                                            dicRdo[groupKey].DEPARTMENT_ROOM_NAME = dicRdo[groupKey].REQUEST_ROOM_NAME;
                                        }
                                        var departmentTranSub = departmentTranLimists.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                                        if (treatment != null)
                                        {
                                            ProcessInfoTreatment(dicRdo[groupKey], treatment, departmentTranSub, transactionSub);
                                        }
                                    }
                                    dicRdo[groupKey].SALE_MEDICINE_PRICE += item.Sum(s => s.AMOUNT * (s.PRICE??0)*(1+(s.VAT_RATIO??0)));
                                    this.SaleMedicinePriceSum += item.Sum(s => s.AMOUNT * (s.PRICE ?? 0) * (1 + (s.VAT_RATIO ?? 0)));
                                }
                            }

                        }
                        if (sereServLimits != null && sereServLimits.Count > 0)
                        {
                            foreach (var item in sereServLimits)
                            {
                                
                                //neu co loc tach chi phi ngoai bao hiem chi tra thi sua lai tong chi phi truoc
                                if (filter.REDU_OVER_HEIN_LIMIT == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                {
                                    if (item.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        item.IS_NO_EXECUTE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                        continue;
                                    }
                                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                                    item.VIR_TOTAL_PRICE = Math.Round(Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => room.BRANCH_ID == o.ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1"));
                                    if (item.VIR_TOTAL_PRICE == 0 && item.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    {
                                        item.IS_NO_EXECUTE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                        continue;
                                    }
                                    item.VIR_TOTAL_PATIENT_PRICE = (item.VIR_TOTAL_PRICE ?? 0) - (item.VIR_TOTAL_HEIN_PRICE ?? 0);
                                }
                                if (filter.REDU_OVER_HEIN_LIMIT == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE)
                                {
                                    if (item.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        item.IS_NO_EXECUTE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                        continue;
                                    }
                                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                                    item.VIR_TOTAL_PRICE = Math.Round((item.VIR_TOTAL_PRICE ?? 0) - Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => room.BRANCH_ID == o.ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1"));
                                    if (item.VIR_TOTAL_PRICE == 0 && item.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    {
                                        item.IS_NO_EXECUTE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                        continue;
                                    }
                                    item.VIR_TOTAL_PATIENT_PRICE = item.VIR_TOTAL_PRICE;

                                    item.VIR_TOTAL_PATIENT_PRICE_BHYT = 0;
                                    item.VIR_TOTAL_HEIN_PRICE = 0;
                                }

                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    continue;

                                if (IsNotNullOrEmpty(this.filter.DEPARTMENT_IDs) && !this.filter.DEPARTMENT_IDs.Contains(item.TDL_REQUEST_DEPARTMENT_ID))
                                {
                                    continue;
                                }
                                if (filter.IS_EXAM_REQ_TO_EXE_ROOM == true)
                                {
                                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                                    {
                                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                                    }
                                }
                                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                                {
                                    if (!dicParent.ContainsKey(item.SERVICE_ID) || !filter.EXACT_PARENT_SERVICE_IDs.Contains(dicParent[item.SERVICE_ID].ID))
                                    {
                                        continue;
                                    }
                                }
                                var groupKey = string.Format(key, item.TDL_TREATMENT_ID ?? 0, item.TDL_REQUEST_DEPARTMENT_ID, item.TDL_REQUEST_ROOM_ID);
                                var transactionSub = transactionLimists.Where(o => o.TREATMENT_ID == item.TDL_TREATMENT_ID).ToList();
                                if (!dicRdo.ContainsKey(groupKey))
                                {
                                    dicRdo[groupKey] = new Mrs00314RDO();
                                    if (filter.IS_NOT_SPLIT_DEPA != true)
                                    {
                                        dicRdo[groupKey].REQUEST_DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID;
                                        dicRdo[groupKey].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                        dicRdo[groupKey].DEPARTMENT_ROOM_NAME = dicRdo[groupKey].REQUEST_DEPARTMENT_NAME;
                                        dicRdo[groupKey].EXECUTE_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                                        dicRdo[groupKey].EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                        
                                    }
                                    if (filter.IS_NOT_SPLIT_ROOM != true)
                                    {
                                        dicRdo[groupKey].REQUEST_ROOM_ID = item.TDL_REQUEST_ROOM_ID;
                                        dicRdo[groupKey].REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                                        dicRdo[groupKey].DEPARTMENT_ROOM_NAME = dicRdo[groupKey].REQUEST_ROOM_NAME;
                                    }
                                    var treatment = treatmentLimit.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID);
                                    var departmentTranSub = departmentTranLimists.Where(o => o.TREATMENT_ID == item.TDL_TREATMENT_ID).ToList();
                                    if (treatment != null)
                                    {
                                        ProcessInfoTreatment(dicRdo[groupKey], treatment, departmentTranSub, transactionSub);
                                    }
                                }
                                //var sereServBillSub = sereServBillLimists.Where(o => o.SERE_SERV_ID == item.ID).ToList();
                                var sereServDepositSub = sereServDepositLimists.Where(o => o.SERE_SERV_ID == item.ID).ToList();
                                ProcessTotalPrice(dicRdo[groupKey], item, transactionSub, sereServDepositSub);
                            }
                        }


                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_TRANSACTION> GetTransaction(List<long> treatmentIds)
        {
            HisTransactionFilterQuery TransactionFilter = new HisTransactionFilterQuery();
            TransactionFilter.TREATMENT_IDs = treatmentIds;
            TransactionFilter.IS_CANCEL = false;
            return new HisTransactionManager(param).Get(TransactionFilter) ?? new List<MOS.EFMODEL.DataModels.HIS_TRANSACTION>();
        }

        //private List<HIS_SERE_SERV_BILL> GetSereServBill(List<long> treatmentIds)
        //{
        //    HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
        //    sereServBillFilter.TDL_TREATMENT_IDs = treatmentIds;
        //    sereServBillFilter.IS_NOT_CANCEL = true;
        //    return new HisSereServBillManager(param).Get(sereServBillFilter) ?? new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV_BILL>();
        //}

        private List<HIS_SERE_SERV_DEPOSIT> GetSereServDeposit(List<long> treatmentIds)
        {
            HisSereServDepositFilterQuery sereServDepositFilter = new HisSereServDepositFilterQuery();
            sereServDepositFilter.TDL_TREATMENT_IDs = treatmentIds;
            sereServDepositFilter.IS_CANCEL = false;
            return new HisSereServDepositManager(param).Get(sereServDepositFilter) ?? new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV_DEPOSIT>();
        }

        private List<V_HIS_EXP_MEST_MEDICINE> GetSaleExpMest(List<long> treatmentIds)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                HisExpMestFilterQuery efilter = new HisExpMestFilterQuery();
                efilter.TDL_TREATMENT_IDs = treatmentIds;
                efilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                efilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var expMests = new HisExpMestManager().Get(efilter);
                if (expMests != null)
                {
                    HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
                    filter.EXP_MEST_IDs = expMests.Select(o => o.ID).ToList();
                    filter.IS_EXPORT = true;
                    result = new HisExpMestMedicineManager(param).GetView(filter) ?? new List<MOS.EFMODEL.DataModels.V_HIS_EXP_MEST_MEDICINE>();
                    var trea = result.Where(o => o.TDL_TREATMENT_ID == null).ToList();
                    foreach (var item in trea)
                    {
                        var ex = expMests.FirstOrDefault(o => o.ID == item.EXP_MEST_ID);
                        if (ex != null)
                        {
                            item.TDL_TREATMENT_ID = ex.TDL_TREATMENT_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private List<HIS_DEPARTMENT_TRAN> GetDepartmentTran(List<long> treatmentIds)
        {
            if (this.filter.IS_COUNT_DAY_DEPA == true)
            {
                HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery();
                departmentTranFilter.TREATMENT_IDs = treatmentIds;
                departmentTranFilter.DEPARTMENT_IN_TIME_FROM = 1;
                return treatmentIds.Count > 0 ? new HisDepartmentTranManager(param).Get(departmentTranFilter) : new List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT_TRAN>();
            }
            return new List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT_TRAN>();
        }

        private void ProcessInfoTreatment(Mrs00314RDO rdo, HIS_TREATMENT treatment, List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT_TRAN> departmentTranSub, List<MOS.EFMODEL.DataModels.HIS_TRANSACTION> transactionSub)
        {
            try
            {

                if (treatment.TDL_PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_PATIENT_TYPE_BHYT = "X";
                    rdo.PATIENT_BHYT = treatment.TDL_HEIN_CARD_NUMBER;
                    rdo.TDL_HEIN_CARD_FROM_TIME = treatment.TDL_HEIN_CARD_FROM_TIME;
                    rdo.TDL_HEIN_CARD_TO_TIME = treatment.TDL_HEIN_CARD_TO_TIME;
                }
                else
                {
                    rdo.IS_PATIENT_TYPE_BHYT = "";
                    rdo.PATIENT_BHYT = "";
                }
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID);

                if (patientType != null)
                {
                    rdo.TDL_PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                }

                var treatmentType = HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treatment.TDL_TREATMENT_TYPE_ID);

                if (treatmentType != null)
                {
                    rdo.TREATMENT_TYPE_CODE = treatmentType.TREATMENT_TYPE_CODE;
                }
                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    rdo.OPEN_TIME = treatment.IN_TIME;
                    rdo.OPEN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                }
                else
                {

                    rdo.OPEN_TIME = treatment.CLINICAL_IN_TIME;
                    rdo.OPEN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME ?? 0);
                }

                rdo.CLOSE_TIME = treatment.OUT_TIME;
                rdo.CLOSE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                if (filter.IS_COUNT_DAY_DEPA == true)
                {
                    rdo.TREATMENT_DAY_COUNT = CountDayOfDepament(rdo.REQUEST_DEPARTMENT_ID, departmentTranSub, treatment);
                }
                else
                {
                    rdo.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                }

                if (treatment.END_DEPARTMENT_ID != null)
                {
                    var checkEndDepartment = listEndDepartment.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                    if (checkEndDepartment != null)
                    {
                        rdo.END_DEPARTMENT_NAME = checkEndDepartment.DEPARTMENT_NAME;
                    }
                }
                if (treatment.END_ROOM_ID != null)
                {
                    var checkEndRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID);
                    if (checkEndRoom != null)
                    {
                        rdo.END_ROOM_NAME = checkEndRoom.ROOM_NAME;
                        if (checkEndRoom.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            rdo.END_EXAM_ROOM_NAME = checkEndRoom.ROOM_NAME;
                        }
                    }
                }

                rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                rdo.PATIENT_GENDER = treatment.TDL_PATIENT_GENDER_NAME;
                rdo.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                rdo.TDL_HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
                rdo.STORE_CODE = treatment.STORE_CODE;

                rdo.ICD_CODE = treatment.ICD_CODE;
                rdo.DOB = treatment.TDL_PATIENT_DOB;
                if (treatment.TDL_PATIENT_DOB > 0)
                {
                    rdo.DOB_YEAR = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                if (transactionSub != null && transactionSub.Count > 0)
                {
                    rdo.PATIENT_TU = transactionSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                    rdo.PATIENT_HU = transactionSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT);
                    rdo.PATIENT_TT = transactionSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).Sum(s => s.AMOUNT);
                    rdo.PATIENT_TT_CODE = string.Join(",",transactionSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).Select(s => s.TRANSACTION_CODE).ToList());
                    rdo.PATIENT_TTK = transactionSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.SALE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).Sum(s => s.AMOUNT);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private decimal CountDayOfDepament(long requestDepartmentId, List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT_TRAN> departmentTranSub, HIS_TREATMENT trea)
        {
            decimal result = 0;
            try
            {
                var departmentTrans = departmentTranSub.Where(o => o.DEPARTMENT_ID == requestDepartmentId && o.TREATMENT_ID == trea.ID).OrderBy(p => p.ID).ToList();

                foreach (var item in departmentTrans)
                {
                    if (HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION != null && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION.Contains(item.DEPARTMENT_ID))
                    {
                        continue;
                    }
                    var nextDepartmentTran = departmentTrans.FirstOrDefault(o => item.ID == o.PREVIOUS_ID);
                    if (nextDepartmentTran != null)
                    {
                        if (HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION != null && HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_RESUSCITATION.Contains(nextDepartmentTran.DEPARTMENT_ID))
                        {
                            nextDepartmentTran = departmentTrans.FirstOrDefault(o => nextDepartmentTran.ID == o.PREVIOUS_ID);
                            if (nextDepartmentTran != null)
                            {

                                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.DEPARTMENT_IN_TIME ?? 0, nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                            }
                            else if (trea.OUT_TIME > 0)
                            {
                                result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.DEPARTMENT_IN_TIME ?? 0, trea.OUT_TIME ?? 0, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                            }
                        }
                        else
                            result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.DEPARTMENT_IN_TIME ?? 0, nextDepartmentTran.DEPARTMENT_IN_TIME ?? 0, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                    }
                    else if (trea.OUT_TIME > 0)
                    {
                        result += Inventec.Common.DateTime.Calculation.DifferenceTime(item.DEPARTMENT_IN_TIME ?? 0, trea.OUT_TIME ?? 0, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR);
                    }
                }
                result = Math.Round(result * 10 / 24) / 10;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessTotalPrice(Mrs00314RDO rdo, HIS_SERE_SERV ss, List<MOS.EFMODEL.DataModels.HIS_TRANSACTION> transactionSub,List<MOS.EFMODEL.DataModels.HIS_SERE_SERV_DEPOSIT> sereServDepositSub)
        {
            try
            {

                //đối tượng thanh toán
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;

                //mã loại dịch vụ, mã dịch vụ cha
                string serviceTypeCode = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == ss.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "";
                string heinServiceTypeCode = (listHeinServiceType.FirstOrDefault(o => o.ID == ss.TDL_HEIN_SERVICE_TYPE_ID) ?? new HIS_HEIN_SERVICE_TYPE()).HEIN_SERVICE_TYPE_CODE ?? "";
                string serviceCode = "";
                if (dicParent.ContainsKey(ss.SERVICE_ID))
                {
                    serviceCode = dicParent[ss.SERVICE_ID].SERVICE_CODE ?? "";
                }
                string categoryCode = dicCategory.ContainsKey(ss.SERVICE_ID) ? dicCategory[ss.SERVICE_ID].CATEGORY_CODE : "";

                var material = listMaterial.FirstOrDefault(p => p.SERVICE_ID == ss.SERVICE_ID);
                if (rdo.DIC_PATIENT_PRICE == null)
                {
                    rdo.DIC_PATIENT_PRICE = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_HEIN_PRICE == null)
                {
                    rdo.DIC_HEIN_PRICE = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_TOTAL_PRICE == null)
                {
                    rdo.DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_AMOUNT == null)
                {
                    rdo.DIC_AMOUNT = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_PARENT_PATIENT_PRICE == null)
                {
                    rdo.DIC_PARENT_PATIENT_PRICE = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY == null)
                {
                    rdo.DIC_CATEGORY = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY_BHYT == null)
                {
                    rdo.DIC_CATEGORY_BHYT = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY_DCT == null)
                {
                    rdo.DIC_CATEGORY_DCT = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY_BN == null)
                {
                    rdo.DIC_CATEGORY_BN = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY_AMOUNT == null)
                {
                    rdo.DIC_CATEGORY_AMOUNT = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_CATEGORY_TT == null)
                {
                    rdo.DIC_CATEGORY_TT = new Dictionary<string, decimal>();
                }
                if (rdo.DIC_TYPE_PRICE == null)
                {
                    rdo.DIC_TYPE_PRICE = new Dictionary<string, decimal>();
                }
                if (ss.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    rdo.TOTAL_EXPEND_PRICE += ss.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                    return;
                }

                if (this.dicDataFilter.ContainsKey("KEY_TOTALs") && this.dicDataFilter["KEY_TOTALs"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_TOTALs"].ToString()))
                {
                    //keyTypePrices = new List<string>() { "DETAIL_{0}_{1}_{2}_{3}_{4}" };
                    
                    List<string> keyTypePrices = this.dicDataFilter["KEY_TOTALs"].ToString().Split(',').ToList();
                    foreach (var item in keyTypePrices)
                    {
                        //key mã đối tượng bệnh nhân, mã đối tượng thanh toán, mã loại dịch vụ, mã dịch vụ cha, đối tượng chi trả, nhóm báo cáo 
                        string TypePrice = string.Format(item, rdo.TDL_PATIENT_TYPE_CODE, rdo.PATIENT_TYPE_CODE, serviceTypeCode, serviceCode, categoryCode, heinServiceTypeCode);
                        //tổng tiền
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice] += ss.VIR_TOTAL_PRICE ?? 0;
                        //tổng tiền Bảo hiểm trả
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice + "_BHTT"))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice + "_BHTT"] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice + "_BHTT"] += ss.VIR_TOTAL_HEIN_PRICE ?? 0;
                        //tổng tiền BN cùng chi trả
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice + "_BNCCT"))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice + "_BNCCT"] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice + "_BNCCT"] += ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        //tổng tiền BN chi trả
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice + "_BN"))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice + "_BN"] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice + "_BN"] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        //tổng tiền BN tự chi trả
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice + "_BNTUTRA"))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice + "_BNTUTRA"] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice + "_BNTUTRA"] += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        //tổng tiền BN tạm trả
                        if (!rdo.DIC_TYPE_PRICE.ContainsKey(TypePrice + "_BNTAMTRA"))
                        {
                            rdo.DIC_TYPE_PRICE[TypePrice + "_BNTAMTRA"] = 0;
                        }
                        rdo.DIC_TYPE_PRICE[TypePrice + "_BNTAMTRA"] += sereServDepositSub.Sum(s=>s.AMOUNT);

                    }

                    //
                }
                else
                {
                    string keySvtPtt = string.Format("{0}_{1}", ss.TDL_SERVICE_TYPE_ID == 4 ? 11 : ss.TDL_SERVICE_TYPE_ID, patientType.PATIENT_TYPE_CODE);
                    if (!rdo.DIC_PATIENT_PRICE.ContainsKey(keySvtPtt))
                    {
                        rdo.DIC_PATIENT_PRICE[keySvtPtt] = 0;
                    }
                    rdo.DIC_PATIENT_PRICE[keySvtPtt] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    if (!rdo.DIC_HEIN_PRICE.ContainsKey(keySvtPtt))
                    {
                        rdo.DIC_HEIN_PRICE[keySvtPtt] = 0;
                    }
                    rdo.DIC_HEIN_PRICE[keySvtPtt] += ss.VIR_TOTAL_HEIN_PRICE ?? 0;

                    string keyParent = string.Format("{0}_{1}", serviceTypeCode, serviceCode);
                    if (!rdo.DIC_PARENT_PATIENT_PRICE.ContainsKey(keyParent))
                    {
                        rdo.DIC_PARENT_PATIENT_PRICE[keyParent] = 0;
                    }
                    rdo.DIC_PARENT_PATIENT_PRICE[keyParent] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    if (!rdo.DIC_PARENT_PATIENT_PRICE.ContainsKey(serviceTypeCode))
                    {
                        rdo.DIC_PARENT_PATIENT_PRICE[serviceTypeCode] = 0;
                    }
                    rdo.DIC_PARENT_PATIENT_PRICE[serviceTypeCode] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;

                    if (!rdo.DIC_AMOUNT.ContainsKey(serviceTypeCode))
                    {
                        rdo.DIC_AMOUNT[serviceTypeCode] = 0;
                    }
                    rdo.DIC_AMOUNT[serviceTypeCode] += ss.AMOUNT;

                    //tổng tiền theo các dịch vụ cha
                    if (!rdo.DIC_TOTAL_PRICE.ContainsKey(keyParent))
                    {
                        rdo.DIC_TOTAL_PRICE[keyParent] = 0;
                    }
                    rdo.DIC_TOTAL_PRICE[keyParent] += ss.VIR_TOTAL_PRICE ?? 0;

                    if (!rdo.DIC_PATIENT_PRICE.ContainsKey(keyParent))
                    {
                        rdo.DIC_PATIENT_PRICE[keyParent] = 0;
                    }
                    rdo.DIC_PATIENT_PRICE[keyParent] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    if (!rdo.DIC_HEIN_PRICE.ContainsKey(keyParent))
                    {
                        rdo.DIC_HEIN_PRICE[keyParent] = 0;
                    }
                    rdo.DIC_HEIN_PRICE[keyParent] += ss.VIR_TOTAL_HEIN_PRICE ?? 0;

                    //tổng tiền theo các nhóm báo cáo
                    if (!rdo.DIC_CATEGORY.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY[categoryCode] += ss.VIR_TOTAL_PRICE ?? 0;

                    //tổng tiền Bảo hiểm trả
                    if (!rdo.DIC_CATEGORY_BHYT.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY_BHYT[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY_BHYT[categoryCode] += ss.VIR_TOTAL_HEIN_PRICE ?? 0;

                    //tổng tiền bệnh nhân đồng chi trả
                    if (!rdo.DIC_CATEGORY_DCT.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY_DCT[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY_DCT[categoryCode] += ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;

                    //tổng tiền bệnh nhân trả
                    if (!rdo.DIC_CATEGORY_BN.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY_BN[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY_BN[categoryCode] += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;

                    //tổng tiền thực thu
                    if (!rdo.DIC_CATEGORY_TT.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY_TT[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY_TT[categoryCode] += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.DISCOUNT ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * transactionSub.Sum(s => s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && s.AMOUNT > 0 ? ((s.EXEMPTION ?? 0) / s.AMOUNT) : 0);

                    //tổng số lượng
                    if (!rdo.DIC_CATEGORY_AMOUNT.ContainsKey(categoryCode))
                    {
                        rdo.DIC_CATEGORY_AMOUNT[categoryCode] = 0;
                    }
                    rdo.DIC_CATEGORY_AMOUNT[categoryCode] += ss.AMOUNT;

                }
                
                rdo.TOTAL_PATIENT_PRICE_PHI += patientType.PATIENT_TYPE_CODE == filter.PATIENT_TYPE_CODE__PHI ? (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0;
                rdo.TOTAL_PATIENT_PRICE_YC += patientType.PATIENT_TYPE_CODE == filter.PATIENT_TYPE_CODE__YC ? (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) : 0;

                
                if (ss.TDL_HEIN_SERVICE_TYPE_ID != null && ss.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                {
                    rdo.TRAN_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.tranPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN || ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN)
                {
                    rdo.TEST_FUEX_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.testFuexPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                    if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                    {
                        rdo.TEST_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                        this.testPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                    }
                    else
                    {
                        rdo.FUEX_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                        this.fuexPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                    }
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
                {
                    rdo.DIIM_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.diimPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                {
                    rdo.MEDICINE_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.medicinePriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                {
                    rdo.ENDO_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.endoPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                {
                    rdo.SUIM_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.suimPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                {
                    rdo.BLOOD_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.bloodPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                {
                    rdo.SURGMISU_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.surgMisuPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                {
                    rdo.MATERIAL_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.materialPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                    if (material != null && material.IS_REUSABLE != null)
                    {
                        rdo.MATERIAL_REUSEABLE_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                        this.materialReuseablePriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                    }
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                {
                    rdo.BED_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.bedPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                {
                    rdo.EXAM_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.examPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC)
                {
                    rdo.OTHER_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.otherPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }
                else if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN)
                {
                    rdo.AN_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                    this.anPriceSum += ss.VIR_TOTAL_PRICE ?? 0;
                }

                if (ss.HEIN_RATIO == 1)
                {
                    rdo.TOTAL_PRICE_100 += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice100Sum += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    rdo.TOTAL_PRICE_0 += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice0Sum += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                }
                else if (((1 - ss.HEIN_RATIO) * 100 ?? 0) == 20)
                {
                    rdo.TOTAL_PRICE_20 += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice20Sum += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    rdo.TOTAL_PRICE_0 += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice0Sum += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                }
                else if (((1 - ss.HEIN_RATIO) * 100 ?? 0) == 5)
                {
                    rdo.TOTAL_PRICE_5 += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice5Sum += (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    rdo.TOTAL_PRICE_0 += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.totalPrice0Sum += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) - (ss.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                }
                else
                {
                    rdo.TOTAL_PRICE_0 += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    this.totalPrice0Sum += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0);
                }

                rdo.TOTAL_OTHER_SOURCE_PRICE += (ss.OTHER_SOURCE_PRICE ?? 0) * ss.AMOUNT;

                rdo.PATIENT_PRICE_PAY = rdo.TOTAL_PRICE_100 + rdo.TOTAL_PRICE_20 + rdo.TOTAL_PRICE_5 + rdo.TOTAL_PRICE_0;
                this.patientPricePaySum += (rdo.PATIENT_PRICE_PAY);
                rdo.TOTAL_PRICE += ss.VIR_TOTAL_PRICE ?? 0;
                rdo.TOTAL_PATIENT_PRICE += ss.VIR_TOTAL_PATIENT_PRICE ?? 0;
                rdo.TOTAL_HEIN_PRICE += ss.VIR_TOTAL_HEIN_PRICE ?? 0;
                rdo.DISCOUNT += ss.DISCOUNT ?? 0;
                rdo.EXEMPTION += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * transactionSub.Sum(s => s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && s.AMOUNT > 0 ? ((s.EXEMPTION ?? 0) / s.AMOUNT) : 0);
                this.totalHeinPriceSum += ss.VIR_TOTAL_HEIN_PRICE ?? 0;
                this.discountSum += ss.DISCOUNT ?? 0;
                this.totalPriceSum += ss.VIR_TOTAL_PRICE ?? 0;

                TotalAmount += ss.VIR_TOTAL_HEIN_PRICE ?? 0;
                //thêm tiền khám tự nguyện
                if (ss.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && ss.PRIMARY_PATIENT_TYPE_ID != null && (ss.PRIMARY_PRICE ?? 0) > (ss.LIMIT_PRICE ?? 0))
                {
                    rdo.DIFF_PRIMARY_PRICE += (ss.PRIMARY_PRICE ?? 0) - (ss.LIMIT_PRICE ?? 0);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CalcuatorAge(long DOB)
        {
            int? AGE = null;
            try
            {
                int? tuoi = RDOCommon.CalculateAge(DOB);
                if (tuoi >= 0)
                {
                    AGE = (tuoi >= 1) ? tuoi : 1;
                }
                return AGE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("CLOSE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM ?? filter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("CLOSE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO ?? filter.OUT_TIME_TO ?? 0));
            dicSingleTag.Add("TEST_FUEX_PRICE_SUM", this.testFuexPriceSum);
            dicSingleTag.Add("TEST_PRICE_SUM", this.testPriceSum);
            dicSingleTag.Add("FUEX_PRICE_SUM", this.fuexPriceSum);
            dicSingleTag.Add("DIIM_PRICE_SUM", this.diimPriceSum);
            dicSingleTag.Add("MEDICINE_PRICE_SUM", this.medicinePriceSum);
            dicSingleTag.Add("BLOOD_PRICE_SUM", this.bloodPriceSum);
            dicSingleTag.Add("SURGMISU_PRICE_SUM", this.surgMisuPriceSum);
            dicSingleTag.Add("MATERIAL_PRICE_SUM", this.materialPriceSum);
            dicSingleTag.Add("MATERIAL_REUSEABLE_PRICE_SUM", this.materialReuseablePriceSum);
            dicSingleTag.Add("EXAM_PRICE_SUM", this.examPriceSum);
            dicSingleTag.Add("TRAN_PRICE_SUM", this.tranPriceSum);
            dicSingleTag.Add("BED_PRICE_SUM", this.bedPriceSum);
            dicSingleTag.Add("ENDO_PRICE_SUM", this.endoPriceSum);
            dicSingleTag.Add("SUIM_PRICE_SUM", this.suimPriceSum);
            dicSingleTag.Add("OTHER_PRICE_SUM", this.otherPriceSum);
            dicSingleTag.Add("AN_PRICE_SUM", this.anPriceSum);
            dicSingleTag.Add("TOTAL_PRICE_SUM", this.totalPriceSum);
            dicSingleTag.Add("TOTAL_HEIN_PRICE_SUM", this.totalHeinPriceSum);
            dicSingleTag.Add("DISCOUNT_SUM", this.discountSum);
            dicSingleTag.Add("TOTAL_PRICE_100_SUM", this.totalPrice100Sum);
            dicSingleTag.Add("TOTAL_PRICE_5_SUM", this.totalPrice5Sum);
            dicSingleTag.Add("TOTAL_PRICE_20_SUM", this.totalPrice20Sum);
            dicSingleTag.Add("TOTAL_PRICE_0_SUM", this.totalPrice0Sum);
            dicSingleTag.Add("PATIENT_PRICE_PAY_SUM", this.patientPricePaySum);
            dicSingleTag.Add("SALE_MEDICINE_PRICE_SUM", this.SaleMedicinePriceSum);

            //cac ma doi tuong thanh toan

            dicSingleTag.Add("BHYT", ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE)??this.filter.PATIENT_TYPE_CODE__BHYT);
            dicSingleTag.Add("PHI", ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE) ?? this.filter.PATIENT_TYPE_CODE__PHI);
            dicSingleTag.Add("YC", ((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DV) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE) ?? this.filter.PATIENT_TYPE_CODE__YC);

            bool exportSuccess = true;
            //if (!IsNotNullOrEmpty(this.filter.DEPARTMENT_IDs))
            //{
            //    foreach (var item in Mrs00314RDODepartments)
            //    {
            //        item.REQUEST_DEPARTMENT_NAME = "";
            //    }
            //}


            {
                objectTag.AddObjectData(store, "Department", this.dicRdo.Values.GroupBy(o => o.REQUEST_DEPARTMENT_ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "SereServ", this.dicRdo.Values.ToList());
                objectTag.AddRelationship(store, "Department", "SereServ", "REQUEST_DEPARTMENT_ID", "REQUEST_DEPARTMENT_ID");
            }

            objectTag.AddObjectData(store, "Treatment", this.Treatments);

            var listServ = dicParent.Values.GroupBy(o => o.SERVICE_CODE).Select(p => p.First()).OrderBy(q => !q.NUM_ORDER.HasValue).ThenBy(q => q.NUM_ORDER).ToList();
            var groupByType = listServ.GroupBy(o => o.SERVICE_TYPE_ID).ToList();
            foreach (var item in groupByType)
            {
                for (int i = 0; i < item.ToList().Count; i++)
                {
                    var service = item.ToList()[i];
                    dicSingleTag.Add(string.Format("CODE_{0}__{1}", service.SERVICE_TYPE_CODE, i + 1), string.Format("{0}_{1}", service.SERVICE_TYPE_CODE, service.SERVICE_CODE));
                    dicSingleTag.Add(string.Format("NAME_{0}__{1}", service.SERVICE_TYPE_CODE, i + 1), service.SERVICE_NAME);
                }
                dicSingleTag.Add(string.Format("CODE_{0}__{1}", item.ToList().First().SERVICE_TYPE_CODE, item.ToList().Count + 1), string.Format("{0}_{1}", item.ToList().First().SERVICE_TYPE_CODE, ""));
                dicSingleTag.Add(string.Format("NAME_{0}__{1}", item.ToList().First().SERVICE_TYPE_CODE, item.ToList().Count + 1), item.ToList().First().SERVICE_TYPE_NAME + " khác");
            }

            if (dicSingleKey != null && dicSingleKey.Count > 0)
            {
                foreach (var item in dicSingleKey)
                {
                    if (!dicSingleTag.ContainsKey(item.Key))
                    {
                        dicSingleTag.Add(item.Key, item.Value);
                    }
                    else
                    {
                        dicSingleTag[item.Key] = item.Value;
                    }
                }
            }

            if (dataObject.Count > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    objectTag.AddObjectData(store, "Report" + i, dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                    objectTag.AddObjectData(store, "Parent" + i, dataObject[i].Count > 1 ? dataObject[i][1] : new DataTable());
                    objectTag.AddObjectData(store, "GrandParent" + i, dataObject[i].Count > 2 ? dataObject[i][2] : new DataTable());
                    objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
                    objectTag.AddRelationship(store, "GrandParent" + i, "Parent" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");
                }
            }

            exportSuccess = exportSuccess && store.SetCommonFunctions();
        }

        public decimal materialReuseablePriceSum { get; set; }
    }
    //select t.* from his_treatment t where t.is_pause = 1 and t.out_time BETWEEN 20170801000000 and 20170810235959 and
    //exists (select 1 from HIS_PATIENT_TYPE_ALTER where treatment_id =t.id and treatment_type_id = 3); 
}
