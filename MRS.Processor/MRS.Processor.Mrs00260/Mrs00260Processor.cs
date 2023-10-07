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

namespace MRS.Processor.Mrs00260
{
    public class Mrs00260Processor : AbstractProcessor
    {
        private Mrs00260Filter filter;
        List<Mrs00260RDO> listRdo = new List<Mrs00260RDO>();
        List<Mrs00260RDO> listRdoMedicine = new List<Mrs00260RDO>(); 
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>(); 
        List<V_HIS_MEDICINE_TYPE_ACIN> ListMedicineTypeAcin = new List<V_HIS_MEDICINE_TYPE_ACIN>(); 
        List<HIS_IMP_MEST> ListManuImpMest = new List<HIS_IMP_MEST>();

        List<HIS_MEDICINE> ListMedicine = new List<HIS_MEDICINE>();
        List<V_HIS_MEDICINE_BEAN> ListMedicineBean = new List<V_HIS_MEDICINE_BEAN>();
        List<HIS_SUPPLIER> ListSupplier = new List<HIS_SUPPLIER>(); 
        CommonParam paramGet = new CommonParam(); 
        public Mrs00260Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00260Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00260Filter)reportFilter; 
            try
            {
                List<HIS_MEDI_STOCK> filterMediStocks = new HisMediStockManager().Get(new HisMediStockFilterQuery() { IS_BUSINESS = filter.IS_MEDI_STOCK_TYPE__BUSINESS });
                List<long> filterMediStockIds = null;
                if (filterMediStocks != null)
                {
                    filterMediStockIds = filterMediStocks.Select(o => o.ID).ToList();
                }
                HisMedicineTypeViewFilterQuery filterMedicineType = new HisMedicineTypeViewFilterQuery();
                filterMedicineType.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(filterMedicineType); 

                HisMedicineTypeAcinViewFilterQuery filterMedicineTypeAcin = new HisMedicineTypeAcinViewFilterQuery(); 
                ListMedicineTypeAcin = new HisMedicineTypeAcinManager(paramGet).GetView(filterMedicineTypeAcin);

                HisMedicineBeanViewFilterQuery filterMedicineBean = new HisMedicineBeanViewFilterQuery();
                filterMedicineBean.MEDI_STOCK_IDs = filterMediStockIds;
                filterMedicineBean.IN_STOCK = MOS.Filter.HisMedicineBeanViewFilter.InStockEnum.YES;
                ListMedicineBean = new HisMedicineBeanManager(paramGet).GetView(filterMedicineBean);

                var medicineBeanIds = ListMedicineBean.Select(o => o.MEDICINE_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(medicineBeanIds))
                {
                    var skip = 0;
                    while (medicineBeanIds.Count - skip > 0)
                    {
                        var listIds = medicineBeanIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisMedicineFilterQuery medicineFilter = new HisMedicineFilterQuery();
                        medicineFilter.IDs = listIds;
                        ListMedicine.AddRange(new HisMedicineManager(param).Get(medicineFilter));
                    }
                }
                HisSupplierFilterQuery filterSupplier = new HisSupplierFilterQuery();
                ListSupplier = new HisSupplierManager(paramGet).Get(filterSupplier); 

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
                if (IsNotNullOrEmpty(ListMedicineType))
                {
                    foreach (V_HIS_MEDICINE_TYPE MedicineType in ListMedicineType)
                    {
                        var ListMedicineSub = ListMedicine.Where(o => o.MEDICINE_TYPE_ID == MedicineType.ID).ToList() ?? new List<HIS_MEDICINE>();
                        var medicineBeanSub = ListMedicineBean.Where(o => ListMedicineSub.Exists(p => p.ID == o.MEDICINE_ID)).ToList() ?? new List<V_HIS_MEDICINE_BEAN>();
                        var supplierSub = ListSupplier.Where(o => ListMedicineSub.Exists(p => p.SUPPLIER_ID == o.ID)).ToList() ?? new List<HIS_SUPPLIER>();
                        Mrs00260RDO rdo = new Mrs00260RDO()
                        {
                            ACTIVE_INGREDIENT_CODE = MedicineType.ACTIVE_INGR_BHYT_CODE,
                            ACTIVE_INGREDIENT_NAME = MedicineType.ACTIVE_INGR_BHYT_NAME,
                            MEDICINE_TYPE_CODE = MedicineType.MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = MedicineType.MEDICINE_TYPE_NAME,
                            HEIN_SERVICE_BHYT_NAME = MedicineType.HEIN_SERVICE_BHYT_NAME,
                            MEDICINE_USE_FORM_CODE = MedicineType.MEDICINE_USE_FORM_CODE,
                            MEDICINE_USE_FORM_NAME = MedicineType.MEDICINE_USE_FORM_NAME,
                            REGISTER_NUMBER = MedicineType.REGISTER_NUMBER,
                            CONCENTRA = MedicineType.CONCENTRA,
                            DESCRIPTION = MedicineType.DESCRIPTION,
                            SERVICE_UNIT_NAME = MedicineType.SERVICE_UNIT_NAME,
                            IMP_PRICE = MedicineType.IMP_PRICE,
                            MANUFACTURE_NAME = MedicineType.MANUFACTURER_NAME,
                            AMOUNT_MEDICINE = medicineBeanSub.Sum(o=>o.AMOUNT),
                            NATIONAL_NAME = MedicineType.NATIONAL_NAME,
                            SUPPLIER_NAME = string.Join(",",supplierSub.Select(o=>o.SUPPLIER_NAME).ToList()),
                            MEDICINE_LINE_NAME = MedicineType.MEDICINE_LINE_NAME,
                        }; 
                        listRdo.Add(rdo); 

                    }
                }
                if (IsNotNullOrEmpty(ListMedicine))
                {
                    foreach (HIS_MEDICINE Medicine in ListMedicine)
                    {
                        var MedicineType = ListMedicineType.FirstOrDefault(o => o.ID == Medicine.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
                        var medicineBeanSub = ListMedicineBean.Where(o => o.MEDICINE_ID==Medicine.ID).ToList() ?? new List<V_HIS_MEDICINE_BEAN>();
                        var supplierSub = ListSupplier.Where(o => Medicine.SUPPLIER_ID == o.ID).ToList() ?? new List<HIS_SUPPLIER>();
                        Mrs00260RDO rdo = new Mrs00260RDO()
                        {
                            ACTIVE_INGREDIENT_CODE = MedicineType.ACTIVE_INGR_BHYT_CODE,
                            ACTIVE_INGREDIENT_NAME = MedicineType.ACTIVE_INGR_BHYT_NAME,
                            MEDICINE_TYPE_CODE = MedicineType.MEDICINE_TYPE_CODE,
                            MEDICINE_TYPE_NAME = MedicineType.MEDICINE_TYPE_NAME,
                            PACKING_TYPE_NAME = MedicineType.PACKING_TYPE_NAME,
                            HEIN_SERVICE_BHYT_NAME = MedicineType.HEIN_SERVICE_BHYT_NAME,
                            MEDICINE_USE_FORM_CODE = MedicineType.MEDICINE_USE_FORM_CODE,
                            MEDICINE_USE_FORM_NAME = MedicineType.MEDICINE_USE_FORM_NAME,
                            REGISTER_NUMBER = MedicineType.REGISTER_NUMBER,
                            CONCENTRA = MedicineType.CONCENTRA,
                            DESCRIPTION = MedicineType.DESCRIPTION,
                            SERVICE_UNIT_NAME = MedicineType.SERVICE_UNIT_NAME,
                            IMP_PRICE = MedicineType.IMP_PRICE,
                            IMP_VAT_RATIO = Medicine.IMP_VAT_RATIO,
                            //PRICE = Medicine.PRICE,
                            //VAT_RATIO = Medicine.VAT_RATIO,
                            MANUFACTURE_NAME = MedicineType.MANUFACTURER_NAME,
                            AMOUNT_MEDICINE = medicineBeanSub.Sum(o => o.AMOUNT),
                            NATIONAL_NAME = MedicineType.NATIONAL_NAME,
                            SUPPLIER_NAME = string.Join(",", supplierSub.Select(o => o.SUPPLIER_NAME).ToList()),
                            MEDICINE_LINE_NAME = MedicineType.MEDICINE_LINE_NAME,
                            PACKAGE_NUMBER = Medicine.PACKAGE_NUMBER,
                            EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Medicine.EXPIRED_DATE??0)
                        };
                        listRdoMedicine.Add(rdo);

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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Medicine", listRdoMedicine); 
        }

       
    }
}
