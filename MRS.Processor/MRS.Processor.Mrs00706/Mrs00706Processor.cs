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

namespace MRS.Processor.Mrs00706
{
    public class Mrs00706Processor : AbstractProcessor
    {
        private Mrs00706Filter filter;
        List<MedicineTypePrice> listMedicineTypePrice = new List<MedicineTypePrice>();
        List<MedicineTypeSupplier> listMedicineTypeSupplier = new List<MedicineTypeSupplier>();
        List<Mrs00706RDO> ListMedicineType = new List<Mrs00706RDO>();
        List<MediStockMety> ListMediStockMety = new List<MediStockMety>();

        int MaxCountPrice = 0;
        CommonParam paramGet = new CommonParam(); 
        public Mrs00706Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00706Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00706Filter)reportFilter; 
            try
            {
                ListMedicineType = new MRS.Processor.Mrs00706.ManagerSql().GetMedicineType();
                listMedicineTypePrice = new MRS.Processor.Mrs00706.ManagerSql().GetPrice();
                listMedicineTypeSupplier = new MRS.Processor.Mrs00706.ManagerSql().GetSupplier();
                if (filter.MEDI_STOCK_ID != null || filter.MEDI_STOCK_IDs != null)
                {
                    ListMediStockMety = new MRS.Processor.Mrs00706.ManagerSql().GetMediStockMety(filter);
                
                }
                if (ListMediStockMety.Count > 0)
                {
                    ListMedicineType = ListMedicineType.Where(o => ListMediStockMety.Exists(p => p.MEDICINE_TYPE_ID == o.ID)).ToList();
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
                if (IsNotNullOrEmpty(ListMedicineType))
                {
                    foreach (Mrs00706RDO item in ListMedicineType)
                    {
                        var listMedicineTypePriceSub = listMedicineTypePrice.Where(o => o.MEDICINE_TYPE_ID == item.ID).ToList();
                        var listMedicineTypeSupplierSub = listMedicineTypeSupplier.Where(o => o.MEDICINE_TYPE_ID == item.ID).ToList();
                        item.DIC_PRICE = new Dictionary<string, decimal>();
                        if (listMedicineTypePriceSub != null && listMedicineTypePriceSub.Count>0)
                        {
                            if (listMedicineTypePriceSub.Count > MaxCountPrice)
                            {
                                MaxCountPrice = listMedicineTypePriceSub.Count;
                            }
                            List<decimal> listPrice = listMedicineTypePriceSub.Select(o => o.PRICE).Distinct().ToList();
                            for (int i = 0; i < listPrice.Count; i++)
                            {
                                item.DIC_PRICE.Add((i + 1).ToString(), listPrice[i]);
                            }
                        }
                        if (listMedicineTypeSupplierSub != null && listMedicineTypeSupplierSub.Count > 0)
                        {
                            List<string> listPrice = listMedicineTypeSupplierSub.Select(o => o.SUPPLIER_NAME).Distinct().ToList();
                            item.SUPPLIER_NAMEs = string.Join(";", listPrice);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "Report", ListMedicineType);
            dicSingleTag.Add("MAX_COUNT_PRICE", MaxCountPrice);
        }

       
    }
}
