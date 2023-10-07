using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisExpMestStt;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineTypeAcin;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisImpMest;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00150
{
    //Báo cáo sử dụng thuốc trong nước
    class Mrs00150Processor : AbstractProcessor
    {
        List<VSarReportMrs00150RDO> _listSereServRdo = new List<VSarReportMrs00150RDO>();
        Mrs00150Filter CastFilter;
        private string MEDI_STOCK_NAME;
        private string EXP_MEST_STT_NAME;
        private string EXP_MEST_TYPE_NAME;
        private int TOTAL_ALL_ACTIVE_INGREDIENT;

        public Mrs00150Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00150Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00150Filter)this.reportFilter;
                //Tên kho xuất
                MEDI_STOCK_NAME = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetById(CastFilter.MEDI_STOCK_ID).MEDI_STOCK_NAME;
                //Trạng thái phiếu xuất
                EXP_MEST_STT_NAME = new MOS.MANAGER.HisExpMestStt.HisExpMestSttManager(paramGet).GetById(CastFilter.EXP_MEST_STT_ID).EXP_MEST_STT_NAME;
                //Loại xuất
                EXP_MEST_TYPE_NAME = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager(paramGet).GetById(CastFilter.EXP_MEST_TYPE_ID).EXP_MEST_TYPE_NAME;
                //-------------------------------------------------------------------------------------------------- EXP_MEST
                var metyFilterExpMest = new HisExpMestViewFilterQuery
                {
                    MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID,
                    EXP_MEST_STT_IDs = new List<long> { CastFilter.EXP_MEST_STT_ID },//Trạng thái phiếu xuất: đã xuất
                    EXP_MEST_TYPE_IDs = new List<long> { CastFilter.EXP_MEST_TYPE_ID }//Loại xuất kho là: Xuất cho khoa phòng (xuất sử dụng)
                };
                if (CastFilter.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                {
                    metyFilterExpMest.FINISH_DATE_FROM = CastFilter.TIME_FROM;
                    metyFilterExpMest.FINISH_DATE_TO = CastFilter.TIME_TO;
                }
                else
                {
                    metyFilterExpMest.CREATE_TIME_FROM = CastFilter.TIME_FROM;
                    metyFilterExpMest.CREATE_TIME_TO = CastFilter.TIME_TO;
                }
                var listExpMestViews = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(metyFilterExpMest);
                //-------------------------------------------------------------------------------------------------- V_HIS_EXP_MEST_MEDICINE
                var listExpMestIds = listExpMestViews.Select(s => s.ID).ToList();
                var listExpMestMedicineViews = new List<V_HIS_EXP_MEST_MEDICINE>();
                var skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        IS_EXPORT = true
                    };
                    var expMestMedicineViews = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                    listExpMestMedicineViews.AddRange(expMestMedicineViews);
                }
                //-------------------------------------------------------------------------------------------------- MEDICINE_TYPE
                var listMedicineTypeIds = listExpMestMedicineViews.Select(s => s.MEDICINE_TYPE_ID).ToList();
                var listMedicineTypeViews = new List<V_HIS_MEDICINE_TYPE>();
                skip = 0;
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterMedicineType = new HisMedicineTypeViewFilterQuery
                    {
                        IDs = listIds
                    };
                    var medicineTypeViews = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(metyFilterMedicineType);
                    listMedicineTypeViews.AddRange(medicineTypeViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_MOBA_IMP_MEST
                var listMobaImpMestViews = new List<V_HIS_IMP_MEST>();
                skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyfilterMobaImpMest = new HisImpMestViewFilterQuery
                    {
                        MOBA_EXP_MEST_IDs = listIds
                    };
                    var mobaImpMestViews = new HisImpMestManager(paramGet).GetView(metyfilterMobaImpMest);
                    listMobaImpMestViews.AddRange(mobaImpMestViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds = listMobaImpMestViews.Select(s => s.ID).ToList();
                var listImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>();
                skip = 0;
                while (listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterimpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds
                    };
                    var impMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterimpMestMedicine);
                    listImpMestMedicineViews.AddRange(impMestMedicineViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_MEDICINE_TYPE_ACIN
                var listMedicineTypeAcinViews = new List<V_HIS_MEDICINE_TYPE_ACIN>();
                skip = 0;
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterMedicineTypeAcin = new HisMedicineTypeAcinViewFilterQuery
                    {
                        MEDICINE_TYPE_IDs = listIds
                    };
                    var medicineTypeAcinViews = new MOS.MANAGER.HisMedicineTypeAcin.HisMedicineTypeAcinManager(paramGet).GetView(metyFilterMedicineTypeAcin);
                    listMedicineTypeAcinViews.AddRange(medicineTypeAcinViews);
                }
                //--------------------------------------------------------------------------------------------------


                ProcessFilterData(listExpMestMedicineViews, listMedicineTypeViews, listImpMestMedicineViews, listMedicineTypeAcinViews);

                result = true;
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
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
            dicSingleTag.Add("EXP_MEST_STT_NAME", EXP_MEST_STT_NAME);
            dicSingleTag.Add("EXP_MEST_TYPE_NAME", EXP_MEST_TYPE_NAME);
            dicSingleTag.Add("TOTAL_ALL_PRICE", _listSereServRdo.Sum(s => s.TOTAL_PRICE));
            dicSingleTag.Add("TOTAL_ACTIVE_INGREDIENT", _listSereServRdo.Where(s => !string.IsNullOrEmpty(s.ACTIVE_INGR_BHYT_CODE)).Select(s => s.ACTIVE_INGR_BHYT_CODE).Distinct().Count());
            dicSingleTag.Add("TOTAL_ALL_ACTIVE_INGREDIENT", TOTAL_ALL_ACTIVE_INGREDIENT);

            objectTag.AddObjectData(store, "Report", _listSereServRdo);

        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines, List<V_HIS_MEDICINE_TYPE> listMedicineTypes,
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines, List<V_HIS_MEDICINE_TYPE_ACIN> listMedicineTypeAcins)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00150 ===============================================================");
                var listMedicineTypeIds = listMedicineTypes.Where(s => s.NATIONAL_NAME == HisMedicineTypeCFG.MEDICINE_TYPE_NATIOANL_VN).Select(s => s.ID).ToList();
                var listExpMestMedicineViews = listExpMestMedicines.Where(s => listMedicineTypeIds.Contains(s.MEDICINE_TYPE_ID)).OrderBy(s => s.MEDICINE_TYPE_NAME).ToList();
                TOTAL_ALL_ACTIVE_INGREDIENT = listMedicineTypeAcins.Where(s => listMedicineTypeIds.Contains(s.MEDICINE_TYPE_ID)).Distinct().Count();

                var listNewExpMestMedicineGroupByMedicineTypeIds = (from listExpMestMedicineView in listExpMestMedicineViews
                                                                    let medicineType = listMedicineTypes.First(s => s.ID == listExpMestMedicineView.MEDICINE_TYPE_ID)
                                                                    select new NewExpMestMedicineViews
                                                                     {
                                                                         V_HIS_EXP_MEST_MEDICINE = listExpMestMedicineView,
                                                                         NATIONAL_NAME = medicineType.NATIONAL_NAME,
                                                                         MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME,
                                                                         MANUFACTURER_ID = medicineType.MANUFACTURER_ID,
                                                                         MEDICINE_TYPE_ID = listExpMestMedicineView.MEDICINE_TYPE_ID,
                                                                         ACTIVE_INGR_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE,
                                                                         ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME
                                                                     }).GroupBy(s => s.MEDICINE_TYPE_ID).ToList();

                foreach (var listNewExpMestMedicineGroupByMedicineTypeId in listNewExpMestMedicineGroupByMedicineTypeIds)
                {
                    var listNewExpMestMedicineGroupByManufacturers = listNewExpMestMedicineGroupByMedicineTypeId.GroupBy(s => s.MANUFACTURER_ID).ToList();
                    foreach (var listNewExpMestMedicineGroupByManufacturer in listNewExpMestMedicineGroupByManufacturers)
                    {
                        var listNewExpMestMedicineGroupByNationals = listNewExpMestMedicineGroupByManufacturer.GroupBy(s => s.NATIONAL_NAME).ToList();
                        foreach (var listNewExpMestMedicineGroupByNational in listNewExpMestMedicineGroupByNationals)
                        {
                            var listNewExpMestMedicineGroupByImpPrices = listNewExpMestMedicineGroupByNational.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.IMP_PRICE).ToList();
                            foreach (var listNewExpMestMedicineGroupByImpPrice in listNewExpMestMedicineGroupByImpPrices)
                            {
                                //đơn giá
                                var price = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.IMP_PRICE;
                                //số lượng thuốc đã thu hồi
                                var listMedicineTypeIdFromExpMestMedicines = listNewExpMestMedicineGroupByImpPrice.Select(s => s.V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_ID);
                                var impMestMedicine = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                    s.IMP_PRICE == listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.IMP_PRICE &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID).Sum(s => s.AMOUNT);
                                //số lượng thuốc đã xuất
                                var expMestMedicine = listNewExpMestMedicineGroupByImpPrice.Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                //số lượng thuốc đã sử dụng
                                var amountUser = expMestMedicine - impMestMedicine;
                                var rdo = new VSarReportMrs00150RDO
                                {
                                    MEDICINE_TYPE_CODE = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_CODE,//Mã thuốc
                                    MEDICINE_TYPE_NAME = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_NAME,//Tên thuốc
                                    ACTIVE_INGR_BHYT_CODE = listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE,//Mã hoạt chất
                                    ACTIVE_INGR_BHYT_NAME = listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_NAME,//Tên hoạt chất
                                    MANUFACTURER_NAME = listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_NAME,//Tên hãng sản xuất
                                    NATIONAL_NAME = listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME,//Nước sản xuất
                                    PRICE = price,//Đơn giá
                                    AMOUNT_USED = amountUser,//Số lượng đã sử dụng
                                    TOTAL_PRICE = price * amountUser//Thành tiền
                                };
                                _listSereServRdo.Add(rdo);
                            }
                        }
                    }
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00150 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        private class NewExpMestMedicineViews
        {
            public V_HIS_EXP_MEST_MEDICINE V_HIS_EXP_MEST_MEDICINE { get; set; }

            public string MANUFACTURER_NAME { get; set; }

            public long? MANUFACTURER_ID { get; set; }

            public string NATIONAL_NAME { get; set; }

            public long MEDICINE_TYPE_ID { get; set; }

            public string ACTIVE_INGR_BHYT_NAME { get; set; }

            public string ACTIVE_INGR_BHYT_CODE { get; set; }
        }
    }
}
