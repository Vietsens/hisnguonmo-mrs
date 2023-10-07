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
using MOS.MANAGER.HisBid;
using System.Reflection;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisBidMedicineType;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisMaterialType;


namespace MRS.Processor.Mrs00596
{
    public class Mrs00596Processor : AbstractProcessor
    {
        Mrs00596Filter filter = null;
        private List<Mrs00596RDO> ListRDO = new List<Mrs00596RDO>();
        List<V_HIS_BID_MEDICINE_TYPE> listHisBidMedicineType = new List<V_HIS_BID_MEDICINE_TYPE>();
        List<V_HIS_MEDICINE_TYPE> listHisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MEDICINE> listHisMedicine = new List<V_HIS_MEDICINE>();
        List<V_HIS_IMP_MEST_MEDICINE> listHisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<HIS_MEDICINE_BEAN> listHisMedicineBean = new List<HIS_MEDICINE_BEAN>();


        List<V_HIS_BID_MATERIAL_TYPE> listHisBidMaterialType = new List<V_HIS_BID_MATERIAL_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listHisMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_MATERIAL> listHisMaterial = new List<V_HIS_MATERIAL>();
        List<V_HIS_IMP_MEST_MATERIAL> listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_MATERIAL_BEAN> listHisMaterialBean = new List<HIS_MATERIAL_BEAN>();

        CommonParam paramGet = new CommonParam();
        public Mrs00596Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00596Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00596Filter)reportFilter);
            var result = true;
            try
            {
                //get dữ liệu loại thuốc vật tư
                GetMetyMaty();
                //get dữ liệu loại thuốc vật tư trong thầu
                GetBidMetyMaty();

                HisImpMestMedicineViewFilterQuery listHisImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery()
                    {
                        IMP_TIME_FROM = filter.IMP_TIME_FROM,
                        IMP_TIME_TO = filter.IMP_TIME_TO,
                        MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                        IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC,
                        IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT,
                        MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs
                    };

                listHisImpMestMedicine = new HisImpMestMedicineManager(new CommonParam()).GetView(listHisImpMestMedicinefilter)
                    ?? new List<V_HIS_IMP_MEST_MEDICINE>();

                listHisImpMestMedicine = listHisImpMestMedicine.Where(o => o.BID_ID.HasValue).ToList();
                if (filter.BID_IDs != null)
                {
                    listHisImpMestMedicine = listHisImpMestMedicine.Where(o => filter.BID_IDs.Contains(o.BID_ID ?? 0)).ToList();
                }

                HisImpMestMaterialViewFilterQuery listHisImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery()
                {
                    IMP_TIME_FROM = filter.IMP_TIME_FROM,
                    IMP_TIME_TO = filter.IMP_TIME_TO,
                    MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                    IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT,
                    MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs
                };
                listHisImpMestMaterial = new HisImpMestMaterialManager(new CommonParam()).GetView(listHisImpMestMaterialfilter)
                     ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                listHisImpMestMaterial = listHisImpMestMaterial.Where(o => o.BID_ID.HasValue).ToList();
                if (filter.BID_IDs != null)
                {
                    listHisImpMestMaterial = listHisImpMestMaterial.Where(o => filter.BID_IDs.Contains(o.BID_ID ?? 0)).ToList();
                }

                if (filter.IS_MEDICINE_TD == true || filter.IS_MATERIAL == true || filter.IS_MEDICINE_YHCT == true || filter.IS_MEDICINE_CPYHCT == true || filter.IS_MEDICINE_OTH == true || filter.IS_CHEMICAL_SUBSTANCE == true)
                {
                    var chemicalSubtanceIds = listHisMaterialType.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ID).ToList();
                    var materialTypeTakes = new List<V_HIS_MATERIAL_TYPE>();
                    var bidMaterialTypeTakes = new List<V_HIS_BID_MATERIAL_TYPE>();
                    var impMestMaterialTakes = new List<V_HIS_IMP_MEST_MATERIAL>();
                    if (filter.IS_MATERIAL == true)
                    {
                        materialTypeTakes.AddRange(listHisMaterialType.Where(o => !chemicalSubtanceIds.Contains(o.ID)).ToList());
                        bidMaterialTypeTakes.AddRange(listHisBidMaterialType.Where(o => !chemicalSubtanceIds.Contains(o.MATERIAL_TYPE_ID??0)).ToList());
                        impMestMaterialTakes.AddRange(listHisImpMestMaterial.Where(o => !chemicalSubtanceIds.Contains(o.MATERIAL_TYPE_ID)).ToList());
                    }
                    if (filter.IS_CHEMICAL_SUBSTANCE == true)
                    {
                        materialTypeTakes.AddRange(listHisMaterialType.Where(o => chemicalSubtanceIds.Contains(o.ID)).ToList());
                        bidMaterialTypeTakes.AddRange(listHisBidMaterialType.Where(o => chemicalSubtanceIds.Contains(o.MATERIAL_TYPE_ID??0)).ToList());
                        impMestMaterialTakes.AddRange(listHisImpMestMaterial.Where(o => chemicalSubtanceIds.Contains(o.MATERIAL_TYPE_ID)).ToList());
                    }
                    var medicineTypeTakes = new List<V_HIS_MEDICINE_TYPE>();
                    var bidMedicineTypeTakes = new List<V_HIS_BID_MEDICINE_TYPE>();
                    var impMestMedicineTakes = new List<V_HIS_IMP_MEST_MEDICINE>();
                    if (filter.IS_MEDICINE_TD == true)
                    {
                        var medicineTypeTDIds = listHisMedicineType.Where(p => p.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD).Select(p => p.ID).ToList();
                        medicineTypeTakes.AddRange(listHisMedicineType.Where(o => medicineTypeTDIds.Contains(o.ID)).ToList());
                        bidMedicineTypeTakes.AddRange(listHisBidMedicineType.Where(o => medicineTypeTDIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                        impMestMedicineTakes.AddRange(listHisImpMestMedicine.Where(o => medicineTypeTDIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                    }
                    if (filter.IS_MEDICINE_YHCT == true)
                    {
                        var medicineTypeYHCTIds = listHisMedicineType.Where(p => p.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT).Select(p => p.ID).ToList();
                        medicineTypeTakes.AddRange(listHisMedicineType.Where(o => medicineTypeYHCTIds.Contains(o.ID)).ToList());
                        bidMedicineTypeTakes.AddRange(listHisBidMedicineType.Where(o => medicineTypeYHCTIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                        impMestMedicineTakes.AddRange(listHisImpMestMedicine.Where(o => medicineTypeYHCTIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                    }
                    if (filter.IS_MEDICINE_CPYHCT == true)
                    {
                        var medicineTypeCPYHCTIds = listHisMedicineType.Where(p => p.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT).Select(p => p.ID).ToList();
                        medicineTypeTakes.AddRange(listHisMedicineType.Where(o => medicineTypeCPYHCTIds.Contains(o.ID)).ToList());
                        bidMedicineTypeTakes.AddRange(listHisBidMedicineType.Where(o => medicineTypeCPYHCTIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                        impMestMedicineTakes.AddRange(listHisImpMestMedicine.Where(o => medicineTypeCPYHCTIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                    }
                    if (filter.IS_MEDICINE_OTH == true)
                    {
                        var medicineTypeOtherIds = listHisMedicineType.Where(p => p.MEDICINE_LINE_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD && p.MEDICINE_LINE_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT && p.MEDICINE_LINE_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT).Select(p => p.ID).ToList();
                        medicineTypeTakes.AddRange(listHisMedicineType.Where(o => medicineTypeOtherIds.Contains(o.ID)).ToList());
                        bidMedicineTypeTakes.AddRange(listHisBidMedicineType.Where(o => medicineTypeOtherIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                        impMestMedicineTakes.AddRange(listHisImpMestMedicine.Where(o => medicineTypeOtherIds.Contains(o.MEDICINE_TYPE_ID)).ToList());
                    }
                    listHisMaterialType = materialTypeTakes;
                    listHisBidMaterialType = bidMaterialTypeTakes;
                    listHisImpMestMaterial = impMestMaterialTakes;

                    listHisMedicineType = medicineTypeTakes;
                    listHisBidMedicineType = bidMedicineTypeTakes;
                    listHisImpMestMedicine = impMestMedicineTakes;

                }

                // get medicine material
                GetMediMate();

                // get bean
                GetMediMateBean();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMediMateBean()
        {
            var medicineBeanId = listHisImpMestMedicine.Select(o => o.MEDICINE_ID).Distinct().ToList();
            if (medicineBeanId != null && medicineBeanId.Count > 0)
            {
                var skip = 0;
                while (medicineBeanId.Count - skip > 0)
                {
                    var Ids = medicineBeanId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisMedicineBeanFilterQuery HisMedicineBeanfilter = new HisMedicineBeanFilterQuery();
                    HisMedicineBeanfilter.MEDICINE_IDs = Ids;
                    HisMedicineBeanfilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                    var listHisMedicineBeanSub = new HisMedicineBeanManager(param).Get(HisMedicineBeanfilter);
                    if (listHisMedicineBeanSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisMedicineBeanSub Get null");
                    else
                        listHisMedicineBean.AddRange(listHisMedicineBeanSub);
                    
                }
            }
            listHisMedicineBean = listHisMedicineBean.Where(o => o.MEDI_STOCK_ID != null).ToList();
            var materialBeanId = listHisImpMestMaterial.Select(o => o.MATERIAL_ID).Distinct().ToList();
            if (materialBeanId != null && materialBeanId.Count > 0)
            {
                var skip = 0;
                while (materialBeanId.Count - skip > 0)
                {
                    var Ids = materialBeanId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisMaterialBeanFilterQuery HisMaterialBeanfilter = new HisMaterialBeanFilterQuery();
                    HisMaterialBeanfilter.MATERIAL_IDs = Ids;
                    HisMaterialBeanfilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                    var listHisMaterialBeanSub = new HisMaterialBeanManager(param).Get(HisMaterialBeanfilter);
                    if (listHisMaterialBeanSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisMaterialBeanSub Get null");
                    else
                        listHisMaterialBean.AddRange(listHisMaterialBeanSub);
                }
            }
            listHisMaterialBean = listHisMaterialBean.Where(o => o.MEDI_STOCK_ID != null).ToList();
        }

        private void GetMediMate()
        {
            var medicineId = new List<long>();
            medicineId.AddRange(listHisImpMestMedicine.Select(p => p.MEDICINE_ID).ToList());
            medicineId = medicineId.Distinct().ToList();

            if (medicineId != null && medicineId.Count > 0)
            {
                var skip = 0;
                while (medicineId.Count - skip > 0)
                {
                    var Ids = medicineId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                    HisMedicineViewFilterQuery HisMedicinefilter = new HisMedicineViewFilterQuery();
                    HisMedicinefilter.IDs = Ids;
                    var listHisMedicineSub = new HisMedicineManager(param).GetView(HisMedicinefilter);
                    if (listHisMedicineSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisMedicineSub Get null");
                    else
                        listHisMedicine.AddRange(listHisMedicineSub);

                }
            }

            var materialId = new List<long>();
            materialId.AddRange(listHisImpMestMaterial.Select(p => p.MATERIAL_ID).ToList());
            materialId = materialId.Distinct().ToList();

            if (materialId != null && materialId.Count > 0)
            {
                var skip = 0;
                while (materialId.Count - skip > 0)
                {
                    var Ids = materialId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisMaterialViewFilterQuery HisMaterialfilter = new HisMaterialViewFilterQuery();
                    HisMaterialfilter.IDs = Ids;
                    var listHisMaterialSub = new HisMaterialManager(param).GetView(HisMaterialfilter);
                    if (listHisMaterialSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisMaterialSub Get null");
                    else
                        listHisMaterial.AddRange(listHisMaterialSub);
                }
            }
        }

        private void GetMetyMaty()
        {
            HisMedicineTypeViewFilterQuery filterMety = new HisMedicineTypeViewFilterQuery();
            filterMety.IDs = filter.MEDICINE_TYPE_IDs;
            listHisMedicineType = new HisMedicineTypeManager().GetView(filterMety);
            HisMaterialTypeViewFilterQuery filterMaty = new HisMaterialTypeViewFilterQuery();
            filterMety.IDs = filter.MATERIAL_TYPE_IDs;
            listHisMaterialType = new HisMaterialTypeManager().GetView(filterMaty);
        }

        private void GetBidMetyMaty()
        {
            HisBidMedicineTypeViewFilterQuery filterBidMety = new HisBidMedicineTypeViewFilterQuery();
            filterBidMety.MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs;
            filterBidMety.BID_IDs = filter.BID_IDs;
            listHisBidMedicineType = new HisBidMedicineTypeManager().GetView(filterBidMety);
            HisBidMaterialTypeViewFilterQuery filterBidMaty = new HisBidMaterialTypeViewFilterQuery();
            filterBidMaty.MATERIAL_TYPE_IDs = filter.MATERIAL_TYPE_IDs;
            filterBidMaty.BID_IDs = filter.BID_IDs;
            listHisBidMaterialType = new HisBidMaterialTypeManager().GetView(filterBidMaty);
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRDO.Clear();
                var groupByMedicine = listHisImpMestMedicine.GroupBy(o => o.MEDICINE_ID).ToList();
                foreach (var item in groupByMedicine)
                {
                    var Metys = listHisMedicineType.Where(o => o.ID == item.First().MEDICINE_TYPE_ID).ToList();
                    var BidMetys = listHisBidMedicineType.Where(o => o.MEDICINE_TYPE_ID == item.First().MEDICINE_TYPE_ID && o.BID_ID == item.First().BID_ID).ToList();
                    var MediBeans = listHisMedicineBean.Where(o => o.MEDICINE_ID == item.Key).ToList();
                    var Medis = listHisMedicine.Where(o => o.ID == item.Key).ToList();
                    var listSub = (from r in item select new Mrs00596RDO(r, Metys, BidMetys, MediBeans, Medis)).ToList();
                    ListRDO.AddRange(listSub);

                }
                var groupByMaterial = listHisImpMestMaterial.GroupBy(o => o.MATERIAL_ID).ToList();
                foreach (var item in groupByMaterial)
                {
                    var Matys = listHisMaterialType.Where(o => o.ID == item.First().MATERIAL_TYPE_ID).ToList();
                    var BidMatys = listHisBidMaterialType.Where(o => o.MATERIAL_TYPE_ID == item.First().MATERIAL_TYPE_ID && o.BID_ID == item.First().BID_ID).ToList();
                    var MateBeans = listHisMaterialBean.Where(o => o.MATERIAL_ID == item.Key).ToList();
                    var Mates = listHisMaterial.Where(o => o.ID == item.Key).ToList();
                    var listSub = (from r in item select new Mrs00596RDO(r, Matys, BidMatys, MateBeans, Mates)).ToList();
                    ListRDO.AddRange(listSub);

                }
                if (filter.BID_CREATE_TIME_FROM > 0 && filter.BID_CREATE_TIME_TO > 0)
                {
                    var dicMatyBid = ListRDO.Where(o => o.SERVICE_TYPE_ID == 2 || o.SERVICE_TYPE_ID == 3).GroupBy(o => o.MEDICINE_TYPE_ID).ToDictionary(p => p.Key, q => q.Select(s => s.BID_ID).Distinct().ToList());
                    foreach (var item in listHisBidMaterialType)
                    {
                        if (item.CREATE_TIME > filter.BID_CREATE_TIME_TO || item.CREATE_TIME < filter.BID_CREATE_TIME_FROM) continue;
                        if (dicMatyBid.ContainsKey(item.MATERIAL_TYPE_ID ?? 0) && dicMatyBid[item.MATERIAL_TYPE_ID ?? 0].Contains(item.BID_ID)) continue;
                        var Matys = listHisMaterialType.Where(o => o.ID == item.MATERIAL_TYPE_ID).ToList();
                        var rdo = new Mrs00596RDO(item, Matys);
                        ListRDO.Add(rdo);
                    }
                    var dicMetyBid = ListRDO.Where(o => o.SERVICE_TYPE_ID == 1).GroupBy(o => o.MEDICINE_TYPE_ID).ToDictionary(p => p.Key, q => q.Select(s => s.BID_ID).Distinct().ToList());
                    foreach (var item in listHisBidMedicineType)
                    {
                        if (item.CREATE_TIME > filter.BID_CREATE_TIME_TO || item.CREATE_TIME < filter.BID_CREATE_TIME_FROM) continue;
                        if (dicMetyBid.ContainsKey(item.MEDICINE_TYPE_ID) && dicMetyBid[item.MEDICINE_TYPE_ID].Contains(item.BID_ID)) continue;
                        var Metys = listHisMedicineType.Where(o => o.ID == item.MEDICINE_TYPE_ID).ToList();
                        var rdo = new Mrs00596RDO(item, Metys);
                        ListRDO.Add(rdo);
                    }

                }
                AddInfoGroup(ListRDO);
                Group();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        private void AddInfoGroup(List<Mrs00596RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicineType = listHisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                if (item.SERVICE_TYPE_ID == 1)
                {
                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
                    }
                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID != null)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                    }
                }
                if (item.SERVICE_TYPE_ID == 2)
                {
                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                }
                if (item.SERVICE_TYPE_ID == 3)
                {
                    item.MEDICINE_LINE_CODE = "DHC";
                    item.MEDICINE_LINE_NAME = "Hóa chất";
                    item.MEDICINE_GROUP_CODE = "DHC";
                    item.MEDICINE_GROUP_NAME = "Hóa chất";
                }
            }
        }
        private void Group()
        {
            string KeyGroup = "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}";
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_IMP") && this.dicDataFilter["KEY_GROUP_IMP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_IMP"].ToString()))
            {
                KeyGroup = this.dicDataFilter["KEY_GROUP_IMP"].ToString();
            }
            var group = ListRDO.GroupBy(x => string.Format(KeyGroup, x.SERVICE_TYPE_ID, x.MEDICINE_TYPE_ID, x.MEDICINE_TYPE_CODE, x.MEDICINE_LINE_CODE, x.MEDICINE_GROUP_ID, x.SUPPLIER_ID, x.BID_ID, x.MEDI_STOCK_ID,x.PACKAGE_NUMBER,x.EXPIRED_DATE_STR)).ToList();
            ListRDO.Clear();
            foreach (var item in group)
            {
                Mrs00596RDO rdo = new Mrs00596RDO();
                rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                rdo.BID_AMOUNT = item.First().BID_AMOUNT;
                rdo.BID_GROUP_CODE = item.First().BID_GROUP_CODE;
                rdo.BID_PACKAGE_CODE = item.First().BID_PACKAGE_CODE;
                rdo.BID_ID = item.First().BID_ID;
                rdo.BID_CREATE_TIME = item.First().BID_CREATE_TIME;
                rdo.BID_NUM_ORDER = item.First().BID_NUM_ORDER;
                rdo.BID_NUMBER = item.First().BID_NUMBER;
                rdo.BID_TOTAL_PRICE = item.First().BID_TOTAL_PRICE;
                rdo.CONCENTRA = item.First().CONCENTRA;
                rdo.DIFF_AMOUNT = item.First().DIFF_AMOUNT;
                rdo.DIFF_TOTAL_PRICE = item.First().DIFF_TOTAL_PRICE;
                rdo.END_AMOUNT = item.GroupBy(o => new { o.SERVICE_TYPE_ID, o.MEMA_ID }).Sum(p => p.Max(q => q.END_AMOUNT));
                rdo.END_TOTAL_PRICE = item.GroupBy(o => new { o.SERVICE_TYPE_ID, o.MEMA_ID }).Sum(p => p.Max(q => q.END_TOTAL_PRICE));
                rdo.AVAILABLE_AMOUNT = item.GroupBy(o => new { o.SERVICE_TYPE_ID, o.MEMA_ID }).Sum(p => p.Max(q => q.AVAILABLE_AMOUNT));
                rdo.AVAILABLE_TOTAL_PRICE = item.GroupBy(o => new { o.SERVICE_TYPE_ID, o.MEMA_ID }).Sum(p => p.Max(q => q.AVAILABLE_TOTAL_PRICE));
                rdo.EXPIRED_DATE_STR = item.First().EXPIRED_DATE_STR;
                rdo.HEIN_SERVICE_CODE = item.First().HEIN_SERVICE_CODE;
                rdo.HEIN_SERVICE_NAME = item.First().HEIN_SERVICE_NAME;
                rdo.IMP_AMOUNT = item.Sum(x => x.IMP_AMOUNT);
                rdo.IMP_PRICE = item.First().IMP_PRICE;
                rdo.IMP_TIME = item.First().IMP_TIME;
                rdo.IMP_TOTAL_PRICE = item.Sum(x => x.IMP_TOTAL_PRICE);
                rdo.MANUFACTURER_NAME = item.First().MANUFACTURER_NAME;
                rdo.MEDI_STOCK_ID = item.First().MEDI_STOCK_ID;
                rdo.MEDI_STOCK_NAME = item.First().MEDI_STOCK_NAME;
                rdo.MEDICINE_GROUP_CODE = item.First().MEDICINE_GROUP_CODE;
                rdo.MEDICINE_GROUP_ID = item.First().MEDICINE_GROUP_ID;
                rdo.MEDICINE_GROUP_NAME = item.First().MEDICINE_GROUP_NAME;
                rdo.MEDICINE_LINE_CODE = item.First().MEDICINE_LINE_CODE;
                rdo.MEDICINE_LINE_ID = item.First().MEDICINE_LINE_ID;
                rdo.MEDICINE_LINE_NAME = item.First().MEDICINE_LINE_NAME;
                rdo.MEDICINE_TYPE_CODE = item.First().MEDICINE_TYPE_CODE;
                rdo.MEDICINE_TYPE_ID = item.First().MEDICINE_TYPE_ID;
                rdo.MEDICINE_TYPE_NAME = item.First().MEDICINE_TYPE_NAME;
                rdo.MEDICINE_TYPE_PROPRIETARY_NAME = item.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                rdo.NATIONAL_NAME = item.First().NATIONAL_NAME;
                rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                rdo.PACKING_TYPE_NAME = item.First().PACKING_TYPE_NAME;
                rdo.REGISTER_NUMBER = item.First().REGISTER_NUMBER;
                rdo.SERVICE_ID = item.First().SERVICE_ID;
                rdo.SERVICE_TYPE_ID = item.First().SERVICE_TYPE_ID;
                rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                rdo.SUPPLIER_CODE = item.First().SUPPLIER_CODE;
                rdo.SUPPLIER_ID = item.First().SUPPLIER_ID;
                rdo.SUPPLIER_NAME = item.First().SUPPLIER_NAME;
                rdo.MONTH_1_AMOUNT = item.Sum(x => x.MONTH_1_AMOUNT);
                rdo.MONTH_2_AMOUNT = item.Sum(x => x.MONTH_2_AMOUNT);
                rdo.MONTH_3_AMOUNT = item.Sum(x => x.MONTH_3_AMOUNT);
                rdo.MONTH_4_AMOUNT = item.Sum(x => x.MONTH_4_AMOUNT);
                rdo.MONTH_5_AMOUNT = item.Sum(x => x.MONTH_5_AMOUNT);
                rdo.MONTH_6_AMOUNT = item.Sum(x => x.MONTH_6_AMOUNT);
                rdo.MONTH_7_AMOUNT = item.Sum(x => x.MONTH_7_AMOUNT);
                rdo.MONTH_8_AMOUNT = item.Sum(x => x.MONTH_8_AMOUNT);
                rdo.MONTH_9_AMOUNT = item.Sum(x => x.MONTH_9_AMOUNT);
                rdo.MONTH_10_AMOUNT = item.Sum(x => x.MONTH_10_AMOUNT);
                rdo.MONTH_11_AMOUNT = item.Sum(x => x.MONTH_11_AMOUNT);
                rdo.MONTH_12_AMOUNT = item.Sum(x => x.MONTH_12_AMOUNT);
                ListRDO.Add(rdo);
            }
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (filter.IMP_TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IMP_TIME_FROM));
            }
            if (filter.IMP_TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IMP_TIME_TO));
            }
            dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == filter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);

            ListRDO = ListRDO.OrderBy(o => o.SERVICE_TYPE_ID).ThenBy(t1 => t1.MEDICINE_TYPE_NAME).ThenBy(t3 => t3.IMP_TIME).ToList();
            RdoCaculatorDiff(ListRDO);
            ListRDO = FilterBeanZero(ListRDO);
            ListRDO = FilterAvailableZero(ListRDO);
            objectTag.AddObjectData(store, "Services", ListRDO);

            objectTag.AddObjectData(store, "GrandParent", ListRDO.GroupBy(o => o.MEDICINE_GROUP_CODE).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Parent", ListRDO.GroupBy(o => new {  o.MEDICINE_LINE_CODE, o.MEDICINE_GROUP_CODE }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "MEDICINE_GROUP_CODE", "MEDICINE_GROUP_CODE");

            objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_CODE", "MEDICINE_GROUP_CODE" }, new string[] { "MEDICINE_LINE_CODE", "MEDICINE_GROUP_CODE" });
            
        }

        private List<Mrs00596RDO> FilterBeanZero(List<Mrs00596RDO> list)
        {
            string keyHideBean0 = "No";
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_HIDE_BEAN_0") && this.dicDataFilter["KEY_HIDE_BEAN_0"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_HIDE_BEAN_0"].ToString()))
            {
                keyHideBean0 = this.dicDataFilter["KEY_HIDE_BEAN_0"].ToString();
                if (keyHideBean0 == "Yes")
                {
                    return list.Where(o => o.END_AMOUNT > 0).ToList();
                }
            }
            return list;
        }

        private List<Mrs00596RDO> FilterAvailableZero(List<Mrs00596RDO> list)
        {
            string keyHideAvailable0 = "No";
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_HIDE_AVAILABLE_0") && this.dicDataFilter["KEY_HIDE_AVAILABLE_0"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_HIDE_AVAILABLE_0"].ToString()))
            {
                keyHideAvailable0 = this.dicDataFilter["KEY_HIDE_AVAILABLE_0"].ToString();
                if (keyHideAvailable0 == "Yes")
                {
                    return list.Where(o => o.AVAILABLE_AMOUNT > 0).ToList();
                }
            }
            return list;
        }

        private void RdoCaculatorDiff(List<Mrs00596RDO> list)
        {
            foreach (var item in list)
            {
                item.DIFF_AMOUNT = item.BID_AMOUNT - list.Where(p => p.SERVICE_TYPE_ID == item.SERVICE_TYPE_ID && p.MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID && p.BID_ID == item.BID_ID && p.IMP_TIME <= item.IMP_TIME).Sum(o => o.IMP_AMOUNT);
                item.DIFF_TOTAL_PRICE = item.DIFF_AMOUNT * item.IMP_PRICE;
            }

        }
    }
}
