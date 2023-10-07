using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using MOS.EFMODEL.DataModels;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisAnticipate;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisAnticipateMety;
using MOS.MANAGER.HisManufacturer;
using MOS.MANAGER.HisServiceUnit;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicineGroup;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisMedicine;

namespace MRS.Processor.Mrs00802
{
    public class Mrs00802Processor : MRS.MANAGER.Core.MrsReport.AbstractProcessor
    {
        public Mrs00802Filter CastFilter;
        List<Mrs00802RDO> listRdo = new List<Mrs00802RDO>();
        List<HIS_SUPPLIER> listSupplier = new List<HIS_SUPPLIER>();
        List<HIS_MEDICINE_TYPE> listMedicineType = new List<HIS_MEDICINE_TYPE>();
        List<HIS_ANTICIPATE_METY> listAnticipateMety = new List<HIS_ANTICIPATE_METY>();
        List<HIS_ANTICIPATE> listAnticipate = new List<HIS_ANTICIPATE>();
        List<HIS_MEDICINE> listMedicine = new List<HIS_MEDICINE>();
        List<HIS_MANUFACTURER> listManufacturer = new List<HIS_MANUFACTURER>();
        List<HIS_SERVICE_UNIT> listServiceUnit = new List<HIS_SERVICE_UNIT>();
        List<HIS_MEDICINE_GROUP> listMedicineGroup = new List<HIS_MEDICINE_GROUP>();
        public Mrs00802Processor(CommonParam param, string reportTypeCode) :
            base(param, reportTypeCode)
        {
            
        }
        public override Type FilterType()
        {
            return typeof(Mrs00802Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {

                CastFilter = (Mrs00802Filter)this.reportFilter;
                CommonParam Param = new CommonParam();
              
                //danh sách chi tiết dự trù thuốc
                HisAnticipateMetyFilterQuery anticipateMetyFilter = new HisAnticipateMetyFilterQuery();
                anticipateMetyFilter.IS_ACTIVE = 1;
                anticipateMetyFilter.ANTICIPATE_ID = CastFilter.ANTICIPATE_ID;
                anticipateMetyFilter.ANTICIPATE_IDs = CastFilter.ANTICIPATE_IDs;
                anticipateMetyFilter.MEDICINE_TYPE_ID = CastFilter.MEDICINE_TYPE_ID;
                anticipateMetyFilter.MEDICINE_TYPE_IDs = CastFilter.MEDICINE_TYPE_IDs;
                anticipateMetyFilter.CREATE_TIME_FROM = CastFilter.TIME_FROM;
                anticipateMetyFilter.CREATE_TIME_TO = CastFilter.TIME_TO;
                listAnticipateMety = new HisAnticipateMetyManager(Param).Get(anticipateMetyFilter);
                if (CastFilter.SUPPLIER_ID != null)
                {
                    listAnticipateMety = listAnticipateMety.Where(o => o.SUPPLIER_ID == CastFilter.SUPPLIER_ID).ToList();
                }
                if (CastFilter.SUPPLIER_IDs != null)
                {
                    listAnticipateMety = listAnticipateMety.Where(o =>CastFilter.SUPPLIER_IDs.Contains(o.SUPPLIER_ID??0)).ToList();
                }
                //danh sách dự trù thuốc
                var anticipateIds = listAnticipateMety.Select(o => o.ANTICIPATE_ID).Distinct().ToList();
                var skip = 0;
                while (anticipateIds.Count - skip > 0)
                {
                    var listIds = anticipateIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //danh sách dự trù thuốc
                    HisAnticipateFilterQuery anticipateFilter = new HisAnticipateFilterQuery();
                    anticipateFilter.IDs = listIds;
                    var listAnticipateSub = new HisAnticipateManager(Param).Get(anticipateFilter);
                    listAnticipate.AddRange(listAnticipateSub);
                }

                //get danh sách loại thuốc
                GetMedicineType();

                //get danh sách đơn vị tính
                GetServiceUnit();

                //get hãng sản xuất
                GetManufacturer();

                //get nhóm thuốc
                GetMedicineGroup();
                
                
                //get nhà cung cấp
                GetSupplier();

                // lọc theo thời gian sử dụng
                if (!string.IsNullOrWhiteSpace(CastFilter.USE_TIME))
                {
                    listAnticipate = listAnticipate.Where(x => x.USE_TIME == CastFilter.USE_TIME).ToList();
                   
                    var anticipateIdUseTimes = listAnticipate.Select(o => o.ID).ToList();
                    listAnticipateMety = listAnticipateMety.Where(o => anticipateIdUseTimes.Contains(o.ANTICIPATE_ID)).ToList();
                }

                //lọc theo nhóm thuốc
                if (CastFilter.MEDICINE_GROUP_ID != null)
                {
                    listMedicineGroup = listMedicineGroup.Where(o => o.ID == CastFilter.MEDICINE_GROUP_ID).ToList();

                    listMedicineType = listMedicineType.Where(x => x.MEDICINE_GROUP_ID == CastFilter.MEDICINE_GROUP_ID).ToList();

                    var listMedicineTypeId = listMedicineType.Select(o => o.ID).ToList();
                    listAnticipateMety = listAnticipateMety.Where(o => listMedicineTypeId.Contains(o.MEDICINE_TYPE_ID)).ToList();

                    var anticipateIdUseTimes = listAnticipateMety.Select(o => o.ID).ToList();
                    listAnticipate = listAnticipate.Where(o => anticipateIdUseTimes.Contains(o.ID)).ToList();

                }
                if (CastFilter.MEDICINE_GROUP_IDs != null)
                {
                    listMedicineGroup = listMedicineGroup.Where(o =>CastFilter.MEDICINE_GROUP_IDs.Contains(o.ID)).ToList();

                    listMedicineType = listMedicineType.Where(x => CastFilter.MEDICINE_GROUP_IDs.Contains(x.MEDICINE_GROUP_ID??0)).ToList();

                    var listMedicineTypeId = listMedicineType.Select(o => o.ID).ToList();
                    listAnticipateMety = listAnticipateMety.Where(o => listMedicineTypeId.Contains(o.MEDICINE_TYPE_ID)).ToList();

                    var anticipateIdUseTimes = listAnticipateMety.Select(o => o.ID).ToList();
                    listAnticipate = listAnticipate.Where(o => anticipateIdUseTimes.Contains(o.ID)).ToList();

                }

                if (CastFilter.SUPPLIER_IDs != null)
                {
                    listSupplier = listSupplier.Where(o => CastFilter.SUPPLIER_IDs.Contains(o.ID)).ToList();
                }

                if (CastFilter.SUPPLIER_ID != null)
                {
                    listSupplier = listSupplier.Where(o => CastFilter.SUPPLIER_ID==o.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return false;
            }
            return result;
        }

        private void GetMedicineGroup()
        {
            HisMedicineGroupFilterQuery MedicineGroupFilter = new HisMedicineGroupFilterQuery();
            MedicineGroupFilter.IS_ACTIVE = 1;
            listMedicineGroup = new HisMedicineGroupManager().Get(MedicineGroupFilter);
        }

        private void GetManufacturer()
        {
            //lấy Nhà Nơi Xuất
            HisManufacturerFilterQuery manufacturerFilter = new HisManufacturerFilterQuery();
            listManufacturer = new HisManufacturerManager().Get(manufacturerFilter);
        }

        private void GetSupplier()
        {
            HisSupplierFilterQuery supplierFilter = new HisSupplierFilterQuery();
            supplierFilter.IS_ACTIVE = 1;
            supplierFilter.IDs = CastFilter.SUPPLIER_IDs;
            listSupplier = new HisSupplierManager().Get(supplierFilter);
        }

        private void GetServiceUnit()
        {
            //lấy Service_Unit
            HisServiceUnitFilterQuery serviceUnitFilter = new HisServiceUnitFilterQuery();
            listServiceUnit = new HisServiceUnitManager().Get(serviceUnitFilter);
        }

        private void GetMedicineType()
        {
            //lấy thuốc
            HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
            listMedicineType = new HisMedicineTypeManager().Get(medicineTypeFilter);
        }

        protected override bool ProcessData()
        {
            bool result = true;

            try
            {
                if (IsNotNullOrEmpty(listAnticipateMety))
                {
                    foreach (var item in listAnticipateMety)
                    {

                        Mrs00802RDO medicineRdo = new Mrs00802RDO();
                        medicineRdo.AMOUNT = item.AMOUNT;
                        medicineRdo.PRICE = item.IMP_PRICE;
                        decimal totalPrice = (decimal)(item.AMOUNT * item.IMP_PRICE);
                        medicineRdo.TOTAL_PRICE = totalPrice.ToString("#.#");
                        medicineRdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        var supplier = listSupplier.Where(x => x.ID == item.SUPPLIER_ID).FirstOrDefault();
                        if (supplier != null)
                        {
                            medicineRdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                            medicineRdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                        }
                        var medicineType = listMedicineType.Where(x => x.ID == item.MEDICINE_TYPE_ID).FirstOrDefault();
                        if (medicineType != null)
                        {
                            medicineRdo.ACTIVE_INGR_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;
                            medicineRdo.ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                            medicineRdo.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                            medicineRdo.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                            medicineRdo.CONCENTRA = medicineType.CONCENTRA;
                            var serviceUnit = listServiceUnit.Where(x => x.ID == medicineType.TDL_SERVICE_UNIT_ID).FirstOrDefault();
                            if (serviceUnit != null)
                            {
                                medicineRdo.SERVICE_UNIT_CODE = serviceUnit.SERVICE_UNIT_CODE;
                                medicineRdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;
                            }
                            var manu = listManufacturer.Where(x => x.ID == medicineType.MANUFACTURER_ID).FirstOrDefault();
                            if (manu != null)
                            {
                                medicineRdo.MANUFACTURER_CODE = manu.MANUFACTURER_CODE;
                                medicineRdo.MANUFACTURER_NAME = manu.MANUFACTURER_NAME;
                            }
                        }
                        listRdo.Add(medicineRdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            //if (((Mrs00802Filter)reportFilter).ANTICIPATE_ID.HasValue)
            {
                //var x = new HisAnticipateManager().Get(new HisAnticipateFilterQuery()).Where(o => ((Mrs00802Filter)reportFilter).USE_TIME == o.USE_TIME).FirstOrDefault();
                //var useTime = listAnticipate.Where(x => x.ID == CastFilter.ANTICIPATE_ID).FirstOrDefault();
                dicSingleTag.Add("USE_TIME", ((Mrs00802Filter)reportFilter).USE_TIME??"");
                //var medicineGroup = new HisMedicineGroupManager().Get(new HisMedicineGroupFilterQuery()).Where(o => ((Mrs00802Filter)reportFilter).MEDICINE_GROUP_ID == o.ID).FirstOrDefault();
                dicSingleTag.Add("MEDICINE_GROUP_NAME", (listMedicineGroup.FirstOrDefault(o => o.ID == CastFilter.MEDICINE_GROUP_ID) ?? new HIS_MEDICINE_GROUP()).MEDICINE_GROUP_NAME ?? "");
                dicSingleTag.Add("ANTICIPATE_NAME", (listAnticipate.FirstOrDefault(o => o.ID == CastFilter.ANTICIPATE_ID) ?? new HIS_ANTICIPATE()).DESCRIPTION ?? "");
                dicSingleTag.Add("REQUEST_USERNAME", (listAnticipate.FirstOrDefault(o => o.ID == CastFilter.ANTICIPATE_ID) ?? new HIS_ANTICIPATE()).REQUEST_USERNAME ?? "");
            }
            objectTag.AddObjectData(store, "AnticipateMeties", listRdo);
        }
    }
}
