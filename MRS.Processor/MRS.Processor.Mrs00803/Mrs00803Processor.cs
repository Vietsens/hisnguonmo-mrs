using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisManufacturer;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceUnit;
using MOS.MANAGER.HisSupplier;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00803
{
    public class Mrs00803Processor : MRS.MANAGER.Core.MrsReport.AbstractProcessor
    {
        public List<Mrs00803RDO> ListMatieralRdo = new List<Mrs00803RDO>();
        public List<Mrs00803RDO> ListMedicineRdo = new List<Mrs00803RDO>();
        public List<HIS_SERVICE_UNIT> listServiceUnit = new List<HIS_SERVICE_UNIT>();
        public List<HIS_MANUFACTURER> listManufacturer = new List<HIS_MANUFACTURER>();
        public List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        public List<HIS_MEDICINE_TYPE> listMedicineType = new List<HIS_MEDICINE_TYPE>();
        public List<HIS_MEDI_CONTRACT_MATY> listMediContractMaty = new List<HIS_MEDI_CONTRACT_MATY>();
        public List<HIS_BID_MATERIAL_TYPE> listBidMaterialType = new List<HIS_BID_MATERIAL_TYPE>();
        public List<HIS_BID_MEDICINE_TYPE> listBidMedicineType = new List<HIS_BID_MEDICINE_TYPE>();
        public List<HIS_BID> listBid = new List<HIS_BID>();
        public List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        Mrs00803Filter filter;
        public Mrs00803Processor(CommonParam param, string reportTypeCode) :
            base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00803Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            filter = ((Mrs00803Filter)reportFilter);
            try
            {
                CommonParam paramGet = new CommonParam();
                listMaterialType = new HisMaterialTypeManager(paramGet).Get(new HisMaterialTypeFilterQuery());
                listManufacturer = new HisManufacturerManager(paramGet).Get(new HisManufacturerFilterQuery());
                listServiceUnit = new HisServiceUnitManager(paramGet).Get(new HisServiceUnitFilterQuery());
                listService = new HisServiceManager(paramGet).Get(new HisServiceFilterQuery());
                listBid = new HisBidManager(paramGet).Get(new HisBidFilterQuery());
                //Vật liệu
                HisMaterialTypeFilterQuery MaterialTypeFiler = new HisMaterialTypeFilterQuery();
                listMaterialType = new HisMaterialTypeManager(paramGet).Get(MaterialTypeFiler);

                //Thuốc
                HisMedicineTypeFilterQuery MedicineTypeFiler = new HisMedicineTypeFilterQuery();
                listMedicineType = new HisMedicineTypeManager(paramGet).Get(MedicineTypeFiler);

                //if (IsNotNullOrEmpty(listBidMaterialType))
                //{
                //    var medicineTypeIds = listMedicineType.Where(o => filter.SERVICE_IDs.Contains(o.SERVICE_ID)).Select(x => x.ID).ToList();
                //    var skip = 0;
                //    while (medicineTypeIds.Count - skip > 0)
                //    {
                //        HisBidMedicineTypeFilterQuery bidMedicineTypeFilter = new HisBidMedicineTypeFilterQuery();
                //        bidMedicineTypeFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                //        bidMedicineTypeFilter.CREATE_TIME_TO = filter.TIME_TO;
                //        bidMedicineTypeFilter.MEDICINE_TYPE_IDs = medicineTypeIds;
                //        var listMaterialSub = new HisBidMedicineTypeManager(paramGet).Get(bidMedicineTypeFilter);
                //    }
                //    if (filter.SERVICE_IDs != null)
                //        listBidMedicineType = listBidMedicineType.Where(x => medicineTypeIds.Contains(x.MEDICINE_TYPE_ID)).ToList();
                //}
                if (filter.SERVICE_IDs==null)
                {
                    filter.SERVICE_IDs = listService.Select(x => x.ID).ToList();
                }

                    var materialTypeIds = listMaterialType.Where(o => filter.SERVICE_IDs.Contains(o.SERVICE_ID)).Select(x => x.ID).ToList();
                       
                    if (materialTypeIds != null)
                    {
                    HisBidMaterialTypeFilterQuery bidMaterialTypeFilter = new HisBidMaterialTypeFilterQuery();
                    bidMaterialTypeFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    bidMaterialTypeFilter.CREATE_TIME_TO = filter.TIME_TO;
                    bidMaterialTypeFilter.MATERIAL_TYPE_IDs = materialTypeIds;
                    listBidMaterialType = new HisBidMaterialTypeManager(paramGet).Get(bidMaterialTypeFilter);
                    listBidMaterialType = listBidMaterialType.Where(x => materialTypeIds.Contains((long)x.MATERIAL_TYPE_ID)).ToList();
                    }    
                        
                
                ////bidMaterialTypeFilter.SERVICE_IDs = filter.MATERIAL_TYPE_IDs;
                //if (filter.SERVICE_IDs != null)
                //{
                //    /// lấy vật tư 
                    
                //    //listMaterialType = listMaterialType.Where(o => materialTypeIds.Contains(o.ID)).ToList();
                //    listBidMaterialType = listBidMaterialType.Where(x => materialTypeIds.Contains((long)x.MATERIAL_TYPE_ID)).ToList();
                //}

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listBidMaterialType))
                {
                   
                    foreach (var item in listBidMaterialType)
                    {
                        Mrs00803RDO rdo = new Mrs00803RDO();
                        rdo.BID_NUM_ORDER = item.BID_NUM_ORDER;//1.2
                        rdo.BID_PACKAGE_CODE = item.BID_PACKAGE_CODE;
                        rdo.TDL_CONTRACT_AMOUNT = item.TDL_CONTRACT_AMOUNT;//14
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.CONCENTRA = item.CONCENTRA;
                        var Mate = listMaterialType.Where(x => x.ID == item.MATERIAL_TYPE_ID).First();
                        if (Mate != null)
                        {
                            rdo.PRICE = (decimal)Mate.IMP_PRICE;//10
                            rdo.IMP_PRICE = (decimal)Mate.IMP_PRICE;//11
                            rdo.MATERIAL_TYPE_CODE = Mate.MATERIAL_TYPE_CODE;//5
                            rdo.MATERIAL_TYPE_NAME = Mate.MATERIAL_TYPE_NAME;//6
                            rdo.NATIONAL_NAME = Mate.NATIONAL_NAME;//9
                            rdo.IS_REUSABLE = Mate.IS_REUSABLE;
                            rdo.PACKING_TYPE_NAME = Mate.PACKING_TYPE_NAME;//7
                        }
                        var service = listService.Where(x => x.ID == Mate.SERVICE_ID).First();
                        if (service != null)
                        {
                            rdo.HEIN_SERVICE_TYPE_ID = service.HEIN_SERVICE_TYPE_ID;
                            rdo.HEIN_SERVICE_BHYT_CODE = service.HEIN_SERVICE_BHYT_CODE;//3
                            rdo.HEIN_SERVICE_BHYT_NAME = service.HEIN_SERVICE_BHYT_NAME;//4
                        }
                        var bid = listBid.Where(x => x.ID == item.BID_ID).First();
                        rdo.BID_NUMBER = bid.BID_NUMBER;//1
                        var manufaturer = listManufacturer.Where(x => x.ID == item.MANUFACTURER_ID).FirstOrDefault();
                        if (manufaturer != null)
                        {
                            rdo.MANUFACTURER_CODE = manufaturer.MANUFACTURER_CODE;
                            rdo.MANUFACTURER_NAME = manufaturer.MANUFACTURER_NAME;//8
                        }
                        var serviceUnit = listServiceUnit.Where(x => x.ID == Mate.TDL_SERVICE_UNIT_ID).First();
                        if (serviceUnit != null)
                        {
                            rdo.SERVICE_UNIT_CODE = serviceUnit.SERVICE_UNIT_CODE;
                            rdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;//10
                        }
                        ListMatieralRdo.Add(rdo);
                    }
                }
                #region comment
                //if (IsNotNullOrEmpty(listBidMedicineType))
                //{
                //    Mrs00803RDO rdo = new Mrs00803RDO();
                //    foreach (var item in listBidMedicineType)
                //    {

                //        var bidMate = listBidMaterialType.Where(x => x.MATERIAL_TYPE_ID == item.ID).First();
                //        if (bidMate != null)
                //        {
                //            rdo.BID_NUM_ORDER = bidMate.BID_NUM_ORDER;//1.2
                //            rdo.BID_PACKAGE_CODE = bidMate.BID_PACKAGE_CODE;
                //            rdo.TDL_CONTRACT_AMOUNT = bidMate.TDL_CONTRACT_AMOUNT;//14
                //            rdo.AMOUNT = bidMate.AMOUNT;
                //            rdo.CONCENTRA = bidMate.CONCENTRA;
                //        }
                //        var bid = listBid.Where(x => x.ID == bidMate.BID_ID).First();
                //        rdo.BID_NUMBER = bid.BID_NUMBER;//1
                //        rdo.PRICE = (decimal)item.IMP_PRICE;//10
                //        rdo.IMP_PRICE = (decimal)item.IMP_PRICE;//11
                //        rdo.CONCENTRA = item.CONCENTRA;
                //        var medicineType = listMedicineType.Where(x => x.ID == item.MEDICINE_TYPE_ID).First();
                //        if (medicineType!=null)
                //        {
                //            rdo.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                //            rdo.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                //            rdo.HEIN_SERVICE_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;//3
                //            rdo.HEIN_SERVICE_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;//4
                //            rdo.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;//7
                //        }
                //        rdo.NATIONAL_NAME = item.NATIONAL_NAME;//9
                //        //rdo.IS_REUSABLE = item.IS_REUSABLE;
                //        rdo.MANUFACTURER_ID = (long)item.MANUFACTURER_ID;
                //        var manufaturer = listManufacturer.Where(x => x.ID == item.MANUFACTURER_ID).FirstOrDefault();
                //        if (manufaturer != null)
                //        {
                //            rdo.MANUFACTURER_CODE = manufaturer.MANUFACTURER_CODE;
                //            rdo.MANUFACTURER_NAME = manufaturer.MANUFACTURER_NAME;//8
                //        }
                //        var serviceUnit = listServiceUnit.Where(x => x.ID == medicineType.TDL_SERVICE_UNIT_ID).First();
                //        if (serviceUnit != null)
                //        {
                //            rdo.SERVICE_UNIT_CODE = serviceUnit.SERVICE_UNIT_CODE;
                //            rdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;//10
                //        }

                //        ListMedicineRdo.Add(rdo);
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {

                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var vatTuKhongTaiSuDung = ListMatieralRdo.Where(x => x.IS_REUSABLE == null || x.IS_REUSABLE == 0).ToList();
            var vatTuTaiSuDung = ListMatieralRdo.Where(x => x.IS_REUSABLE==1).ToList();
            objectTag.AddObjectData(store, "MaterialNotResuable", vatTuKhongTaiSuDung);
            objectTag.AddObjectData(store, "MaterialResuable", vatTuTaiSuDung);
            objectTag.AddObjectData(store, "MaterialNotResuable2", vatTuKhongTaiSuDung);
            objectTag.AddObjectData(store, "MaterialResuable2", vatTuTaiSuDung);
            objectTag.AddObjectData(store, "Medicine", ListMedicineRdo);
        }
    }
}

