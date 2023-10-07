using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using ACS.Filter;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00154
{
    public class Mrs00154Processor : AbstractProcessor
    {
        private Mrs00154Filter filter;
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicParentMety = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicParentMaty = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>> dicExpMestMedicine = new Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>>();
        Dictionary<long, List<V_HIS_EXP_MEST_MATERIAL>> dicExpMestMaterial = new Dictionary<long, List<V_HIS_EXP_MEST_MATERIAL>>();
        Dictionary<long, V_HIS_IMP_MEST> dicMobaImpMest = new Dictionary<long, V_HIS_IMP_MEST>();
        Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>> dicImpMestMedicine = new Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>>();
        Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>> dicImpMestMaterial = new Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>>();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<Mrs00154RDO> listRdo = new List<Mrs00154RDO>();
        private List<Mrs00154RDO> ParentMedicine = new List<Mrs00154RDO>();
        private List<Mrs00154RDO> ParentMaterial = new List<Mrs00154RDO>();
        private string MEDI_STOCK_NAME;
        private string EXP_MEST_TYPE;
        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> ListExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        CommonParam paramGet = new CommonParam();
        bool IsTakeMedi = true;
        bool IsTakeMate = true;

        public Mrs00154Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00154Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00154Filter)this.reportFilter);
            var result = true;
            try
            {
                //điều kiện lọc thuốc vât tư
                if (filter.IS_MEDICINE == true && filter.IS_MATERIAL != true)
                {
                    IsTakeMate = false;
                }
                if (filter.IS_MEDICINE != true && filter.IS_MATERIAL == true)
                {
                    IsTakeMedi = false;
                }
                //kho
                var metyMediStockId = new HisMediStockViewFilterQuery
                {
                    ID = filter.MEDI_STOCK_ID,
                };
                var mediStock = new HisMediStockManager(paramGet).GetView(metyMediStockId);
                MEDI_STOCK_NAME = mediStock.First().MEDI_STOCK_NAME;
                //loại phiếu xuất
                var metyExpMestTypeIds = new HisExpMestTypeFilterQuery
                {
                    IDs = filter.EXP_MEST_TYPE_IDs,
                };
                var expMestType = new HisExpMestTypeManager(paramGet).Get(metyExpMestTypeIds);
                EXP_MEST_TYPE = string.Join(", ", expMestType.Select(s => s.EXP_MEST_TYPE_NAME));

                //phieu xuat
                var metyFilterExpMest = new HisExpMestViewFilterQuery
                {
                    FINISH_DATE_FROM = filter.DATE_FROM,
                    FINISH_DATE_TO = filter.DATE_TO,
                    EXP_MEST_TYPE_IDs = filter.EXP_MEST_TYPE_IDs, // cac loai xuat
                    MEDI_STOCK_ID = filter.MEDI_STOCK_ID,
                };
                var listExpMestViews = new HisExpMestManager(paramGet).GetView(metyFilterExpMest);

                //lọc kho nhân
                if (filter.IMP_MEDI_STOCK_IDs != null)
                {
                    listExpMestViews = listExpMestViews.Where(o => filter.IMP_MEDI_STOCK_IDs.Contains(o.IMP_MEDI_STOCK_ID ?? 0)).ToList();
                }
                if (IsNotNullOrEmpty(listExpMestViews))
                {
                    var listExpMestIds = listExpMestViews.Select(s => s.ID).ToList();
                    //thuoc trong phieu do
                    var skip = 0;
                    if (IsTakeMedi == true)
                    {
                        while (listExpMestIds.Count - skip > 0)
                        {

                            var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var FilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                            {
                                EXP_MEST_IDs = listIds,
                                IS_EXPORT = true
                            };
                            var listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(FilterExpMestMedicine);
                            ListExpMestMedicine.AddRange(listExpMestMedicine);
                        }

                        //loai thuoc
                        var FilterMedicineType = new HisMedicineTypeViewFilterQuery();
                        listMedicineType = new HisMedicineTypeManager(paramGet).GetView(FilterMedicineType);
                        foreach (var type in listMedicineType) if (!dicMedicineType.ContainsKey(type.ID)) dicMedicineType[type.ID] = type;

                        //lấy thuốc cha
                        GetParentMedicineType();
                    }
                    //vat tu trong phieu do
                    if (IsTakeMate == true)
                    {
                        skip = 0;
                        while (listExpMestIds.Count - skip > 0)
                        {

                            var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var FilterExpMestMatial = new HisExpMestMaterialViewFilterQuery
                            {
                                EXP_MEST_IDs = listIds,
                                IS_EXPORT = true
                            };
                            var listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(FilterExpMestMatial);
                            ListExpMestMaterial.AddRange(listExpMestMaterial);
                        }
                        //loai vat tu
                        var FilterMaterialType = new HisMaterialTypeViewFilterQuery();
                        listMaterialType = new HisMaterialTypeManager(paramGet).GetView(FilterMaterialType);
                        foreach (var type in listMaterialType) if (!dicMaterialType.ContainsKey(type.ID)) dicMaterialType[type.ID] = type;

                        //lấy vật tư cha
                        GetParentMaterialType();
                    }

                    //thuốc thu hồi
                    var listMobaImpMestViews = new List<V_HIS_IMP_MEST>();
                    var mobaExpMestIds = ListExpMestMedicine.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList();
                    mobaExpMestIds.AddRange(ListExpMestMaterial.Where(o => o.TH_AMOUNT > 0).Select(p => p.EXP_MEST_ID ?? 0).ToList());
                    mobaExpMestIds = mobaExpMestIds.Distinct().ToList();
                    skip = 0;
                    while (mobaExpMestIds.Count - skip > 0)
                    {
                        var listIds = mobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var metyFilterMoboImpMest = new HisImpMestViewFilterQuery
                        {
                            MOBA_EXP_MEST_IDs = listIds,
                            IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                        };
                        var mobaImpMest = new HisImpMestManager(paramGet).GetView(metyFilterMoboImpMest);
                        listMobaImpMestViews.AddRange(mobaImpMest);
                    }
                    foreach (var moba in listMobaImpMestViews) if (!dicMobaImpMest.ContainsKey(moba.ID)) dicMobaImpMest[moba.ID] = moba;

                    var listImpMestIds = listMobaImpMestViews.Select(s => s.ID).ToList();
                    if (IsTakeMedi == true)
                    {
                        skip = 0;
                        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
                        while (listImpMestIds.Count - skip > 0)
                        {
                            var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                            {
                                IMP_MEST_IDs = listIds,
                            };
                            var impMestMedicineViews = new HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine);
                            listImpMestMedicine.AddRange(impMestMedicineViews);
                        }
                        foreach (var imp in listImpMestMedicine)
                        {
                            if (!dicImpMestMedicine.ContainsKey(imp.TH_EXP_MEST_MEDICINE_ID ?? 0)) dicImpMestMedicine[imp.TH_EXP_MEST_MEDICINE_ID ?? 0] = new List<V_HIS_IMP_MEST_MEDICINE>();
                            dicImpMestMedicine[imp.TH_EXP_MEST_MEDICINE_ID ?? 0].Add(imp);
                        }
                    }
                    if (IsTakeMate == true)
                    {
                        skip = 0;
                        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                        while (listImpMestIds.Count - skip > 0)
                        {
                            var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                            {
                                IMP_MEST_IDs = listIds,
                            };
                            var impMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial);
                            listImpMestMaterial.AddRange(impMestMaterialViews);
                        }
                        foreach (var imp in listImpMestMaterial)
                        {
                            if (!dicImpMestMaterial.ContainsKey(imp.TH_EXP_MEST_MATERIAL_ID ?? 0)) dicImpMestMaterial[imp.TH_EXP_MEST_MATERIAL_ID ?? 0] = new List<V_HIS_IMP_MEST_MATERIAL>();
                            dicImpMestMaterial[imp.TH_EXP_MEST_MATERIAL_ID ?? 0].Add(imp);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetParentMedicineType()
        {
            foreach (var item in dicMedicineType)
            {
                var medicineType = item.Value;
                long? medicineTypeId = medicineType.ID;
                V_HIS_MEDICINE_TYPE parentMety = new V_HIS_MEDICINE_TYPE();
                while (medicineTypeId != null)
                {
                    if (!dicMedicineType.ContainsKey(medicineTypeId ?? 0)) continue;
                    var nowMety = dicMedicineType[medicineTypeId ?? 0];
                    medicineTypeId = nowMety.PARENT_ID;
                    if (medicineTypeId == null)
                        parentMety = nowMety;
                }
                if (!dicParentMety.ContainsKey(item.Key))
                {
                    dicParentMety.Add(item.Key, parentMety);
                }
            }
        }

        private void GetParentMaterialType()
        {
            foreach (var item in dicMaterialType)
            {
                var materialType = item.Value;
                long? materialTypeId = materialType.ID;
                V_HIS_MATERIAL_TYPE parentMaty = new V_HIS_MATERIAL_TYPE();
                while (materialTypeId != null)
                {
                    if (!dicMaterialType.ContainsKey(materialTypeId ?? 0)) continue;
                    var nowMaty = dicMaterialType[materialTypeId ?? 0];
                    materialTypeId = nowMaty.PARENT_ID;
                    if (materialTypeId == null)
                        parentMaty = nowMaty;
                }
                if (!dicParentMaty.ContainsKey(item.Key))
                {
                    dicParentMaty.Add(item.Key, parentMaty);
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                listRdo.Clear();
                if (IsNotNullOrEmpty(ListExpMestMedicine))
                {
                    #region thuoc

                    //tạo các nhóm phiếu xuất theo theo mã thuốc cha
                    foreach (var exp in ListExpMestMedicine)
                    {
                        if (dicParentMety.ContainsKey(exp.MEDICINE_TYPE_ID))
                        {
                            long ParentId = dicParentMety[exp.MEDICINE_TYPE_ID].ID;
                            if (!dicExpMestMedicine.ContainsKey(ParentId)) dicExpMestMedicine[ParentId] = new List<V_HIS_EXP_MEST_MEDICINE>();
                            dicExpMestMedicine[ParentId].Add(exp);
                        }
                    }
                    //group theo ten thuoc, gia va nuoc san xuat
                    foreach (var key in dicExpMestMedicine.Keys)
                    {
                        var groupMedicine = dicExpMestMedicine[key].GroupBy(o => string.Format("{0}_{1}", o.MEDICINE_TYPE_ID,filter.INPUT_DATA_ID_PRICE_TYPE != 2 ? (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0)) : o.IMP_PRICE * (1 + o.IMP_VAT_RATIO))).ToList();
                        foreach (var group in groupMedicine)
                        {
                            List<V_HIS_EXP_MEST_MEDICINE> listsub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                            var fs = listsub.First();
                            Mrs00154RDO rdo = new Mrs00154RDO();
                            rdo.MEDICINE_TYPE_NAME = fs.MEDICINE_TYPE_NAME;
                            rdo.PARENT_ID = key;
                            if (dicMedicineType.ContainsKey(key))
                                rdo.PARENT_NAME = dicMedicineType[key].MEDICINE_TYPE_NAME;
                            rdo.PRICE = filter.INPUT_DATA_ID_PRICE_TYPE != 2 ? (fs.PRICE ?? 0) * (1 + (fs.VAT_RATIO ?? 0)) : fs.IMP_PRICE * (1 + fs.IMP_VAT_RATIO);
                            rdo.IMP_PRICE = fs.IMP_PRICE * (1 + fs.IMP_VAT_RATIO);
                            rdo.IMP_VAT_RATIO = 0;
                            rdo.VAT_RATIO = 0;
                            if (dicMedicineType.ContainsKey(fs.MEDICINE_TYPE_ID))
                            {
                                rdo.SERVICE_UNIT_NAME = dicMedicineType[fs.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = dicMedicineType[fs.MEDICINE_TYPE_ID].NATIONAL_NAME;
                            }

                            //thu hoi
                            decimal amountMoba = 0;
                            foreach (var mb in listsub)
                            {
                                decimal impAmount = dicImpMestMedicine.ContainsKey(mb.ID) ? dicImpMestMedicine[mb.ID].Sum(s => s.AMOUNT) : 0;
                                amountMoba += impAmount;
                            }
                            rdo.AMOUNT = listsub.Sum(o => o.AMOUNT) - amountMoba;
                            rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                            listRdo.Add(rdo);
                        }
                    }
                    #endregion
                }

                if (IsNotNullOrEmpty(ListExpMestMaterial))
                {
                    #region vat tu
                    //tạo các nhóm phiếu xuất theo theo mã thuốc cha
                    foreach (var exp in ListExpMestMaterial)
                    {
                        if (dicParentMaty.ContainsKey(exp.MATERIAL_TYPE_ID))
                        {
                            long ParentId = dicParentMaty[exp.MATERIAL_TYPE_ID].ID;
                            if (!dicExpMestMaterial.ContainsKey(ParentId)) dicExpMestMaterial[ParentId] = new List<V_HIS_EXP_MEST_MATERIAL>();
                            dicExpMestMaterial[ParentId].Add(exp);
                        }
                    }
                    //group theo ten thuoc, gia va nuoc san xuat
                    foreach (var key in dicExpMestMaterial.Keys)
                    {
                        var groupMaterial = dicExpMestMaterial[key].GroupBy(o => string.Format("{0}_{1}", o.MATERIAL_TYPE_ID, filter.INPUT_DATA_ID_PRICE_TYPE != 2 ? (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0)) : o.IMP_PRICE * (1 + o.IMP_VAT_RATIO))).ToList();
                        foreach (var group in groupMaterial)
                        {
                            List<V_HIS_EXP_MEST_MATERIAL> listsub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                            var fs = listsub.First();
                            Mrs00154RDO rdo = new Mrs00154RDO();
                            rdo.MEDICINE_TYPE_NAME = fs.MATERIAL_TYPE_NAME;
                            rdo.PARENT_ID = key;
                            if (dicMaterialType.ContainsKey(key))
                                rdo.PARENT_NAME = dicMaterialType[key].MATERIAL_TYPE_NAME;
                            rdo.PRICE = filter.INPUT_DATA_ID_PRICE_TYPE != 2 ? (fs.PRICE ?? 0) * (1 + (fs.VAT_RATIO ?? 0)) : fs.IMP_PRICE * (1 + fs.IMP_VAT_RATIO);
                            rdo.IMP_PRICE = fs.IMP_PRICE * (1 + fs.IMP_VAT_RATIO);
                            rdo.IMP_VAT_RATIO = 0;
                            rdo.VAT_RATIO = 0;
                            if (dicMaterialType.ContainsKey(fs.MATERIAL_TYPE_ID))
                            {
                                rdo.SERVICE_UNIT_NAME = dicMaterialType[fs.MATERIAL_TYPE_ID].SERVICE_UNIT_NAME;
                                rdo.NATIONAL_NAME = dicMaterialType[fs.MATERIAL_TYPE_ID].NATIONAL_NAME;
                            }

                            //thu hoi
                            decimal amountMoba = 0;
                            foreach (var mb in listsub)
                            {
                                decimal impAmount = dicImpMestMaterial.ContainsKey(mb.ID) ? dicImpMestMaterial[mb.ID].Sum(s => s.AMOUNT) : 0;
                                amountMoba += impAmount;
                            }
                            rdo.AMOUNT = listsub.Sum(o => o.AMOUNT) - amountMoba;
                            rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                            listRdo.Add(rdo);
                        }
                    }
                    #endregion
                }
                ParentMedicine = listRdo.GroupBy(o => o.PARENT_ID).Select(p => new Mrs00154RDO { PARENT_ID = p.First().PARENT_ID, MEDICINE_TYPE_NAME = p.First().PARENT_NAME, PARENT_NAME = p.First().PARENT_NAME, AMOUNT = p.Sum(s => s.AMOUNT), TOTAL_PRICE = p.Sum(s => s.TOTAL_PRICE) }).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
            dicSingleTag.Add("EXP_MEST_TYPE", EXP_MEST_TYPE);
            dicSingleTag.Add("TOTAL_ALL_PRICE", ParentMedicine.Sum(s => s.TOTAL_PRICE));
            dicSingleTag.Add("TOTAL_ALL_PRICE_TO_STRING", Inventec.Common.String.Convert.CurrencyToVneseString(ParentMedicine.Sum(s => s.TOTAL_PRICE).ToString()));
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ParentMedicine", ParentMedicine);
            objectTag.AddRelationship(store, "ParentMedicine", "Report", "PARENT_ID", "PARENT_ID");
        }
    }

}
