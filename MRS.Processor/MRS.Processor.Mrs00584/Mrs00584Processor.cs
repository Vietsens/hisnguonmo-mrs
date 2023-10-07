using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00584
{
    class Mrs00584Processor : AbstractProcessor
    {
        Mrs00584Filter castFilter = null;
        public const int LIMIT = 2147483646;
        List<Mrs00584RDO> ListRdo = new List<Mrs00584RDO>();
        List<Mrs00584RDO> ListRdoMedicine = new List<Mrs00584RDO>();
        List<Mrs00584RDO> ListRdoFromSupplier = new List<Mrs00584RDO>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBeforeAlls = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<Mrs00584RDO> listExpMestMedicineBeforeAlls = new List<Mrs00584RDO>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialBeforeAlls = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<Mrs00584RDO> listExpMestMaterialBeforeAlls = new List<Mrs00584RDO>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineOnAlls = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOnAlls = new List<V_HIS_EXP_MEST_MEDICINE>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialOnAlls = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOnAlls = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<HIS_MEDI_STOCK> listMediStock = new List<HIS_MEDI_STOCK>();

        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();

        List<ExpMestIdReason> ExpMestIdReasons = new List<ExpMestIdReason>(); // DS phieu xuat va li do xuat trong ky

        List<V_HIS_IMP_MEST_MEDICINE> InputMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc dau vao kiem ke

        List<V_HIS_IMP_MEST_MATERIAL> InputMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu dau vao kiem ke

        public Mrs00584Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00584Filter);
        }
        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = (Mrs00584Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                listMediStock = new HisMediStockManager().Get(new HisMediStockFilterQuery() { IDs = castFilter.MEDI_STOCK_IDs });

                //Loai thuoc, vat tu
                GetMedicineTypeMaterialType();

                #region Vùng get dữ liệu tổng hợp

                //================================================================================================================Truoc ki
                if (castFilter.IS_MEDICINE != false)
                {
                    CommonParam imMedicineParam = new CommonParam();
                    imMedicineParam.Start = 0;
                    imMedicineParam.Limit = LIMIT;
                    int iMe = 0;
                    do
                    {
                        iMe++;
                        HisImpMestMedicineViewFilterQuery impMestMedicineBeforeAllViewFilter = new HisImpMestMedicineViewFilterQuery();
                        impMestMedicineBeforeAllViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                        impMestMedicineBeforeAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        impMestMedicineBeforeAllViewFilter.ORDER_DIRECTION = "ASC";
                        impMestMedicineBeforeAllViewFilter.ORDER_FIELD = "ID";
                        impMestMedicineBeforeAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        var listImpMestMedicineBefore = new HisImpMestMedicineManager(imMedicineParam).GetView(impMestMedicineBeforeAllViewFilter);
                        listImpMestMedicineBeforeAlls.AddRange(listImpMestMedicineBefore);
                        imMedicineParam.Start += listImpMestMedicineBefore.Count;
                    }
                    while ((LIMIT * iMe) == imMedicineParam.Start);

                    HisExpMestMedicineViewFilterQuery expMestMedicineBeforeAllViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineBeforeAllViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMedicineBeforeAllViewFilter.IS_EXPORT = true;
                    expMestMedicineBeforeAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listExpMestMedicineBeforeAlls = new ManagerSql().Get(expMestMedicineBeforeAllViewFilter);
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE != false || castFilter.IS_MATERIAL != false)
                {
                    CommonParam imMaterialParam = new CommonParam();
                    imMaterialParam.Start = 0;
                    imMaterialParam.Limit = LIMIT;
                    int iMa = 0;
                    do
                    {
                        iMa++;
                        HisImpMestMaterialViewFilterQuery impMestMaterialBeforeAllViewFilter = new HisImpMestMaterialViewFilterQuery();
                        impMestMaterialBeforeAllViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                        impMestMaterialBeforeAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        impMestMaterialBeforeAllViewFilter.ORDER_DIRECTION = "ASC";
                        impMestMaterialBeforeAllViewFilter.ORDER_FIELD = "ID";
                        impMestMaterialBeforeAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        var listImpMestMaterialBefore = new HisImpMestMaterialManager(imMaterialParam).GetView(impMestMaterialBeforeAllViewFilter);
                        listImpMestMaterialBeforeAlls.AddRange(listImpMestMaterialBefore);
                        imMaterialParam.Start += listImpMestMaterialBefore.Count;
                    }
                    while ((LIMIT * iMa) == imMaterialParam.Start);
                    HisExpMestMaterialViewFilterQuery expMestMaterialBeforeAllViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialBeforeAllViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMaterialBeforeAllViewFilter.IS_EXPORT = true;
                    expMestMaterialBeforeAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listExpMestMaterialBeforeAlls = new ManagerSql().Get(expMestMaterialBeforeAllViewFilter);
                   
                }
                if (castFilter.IS_MEDICINE != false)
                {
                    //================================================================================================================Nhap Trong ki thuốc

                    HisImpMestMedicineViewFilterQuery impMestMedicineOnAllViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineOnAllViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMestMedicineOnAllViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMestMedicineOnAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMestMedicineOnAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listImpMestMedicineOnAlls = new HisImpMestMedicineManager(param).GetView(impMestMedicineOnAllViewFilter);
                    //================================================================================================================Xuat Trong ki thuốc

                    HisExpMestMedicineViewFilterQuery expMestMedicineOnAllViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineOnAllViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMedicineOnAllViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMedicineOnAllViewFilter.IS_EXPORT = true;
                    expMestMedicineOnAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listExpMestMedicineOnAlls = new HisExpMestMedicineManager(param).GetView(expMestMedicineOnAllViewFilter);
                }

                if (castFilter.IS_CHEMICAL_SUBSTANCE != false || castFilter.IS_MATERIAL != false)
                {
                    //================================================================================================================Nhap Trong ki vật tư
                    HisImpMestMaterialViewFilterQuery impMestMaterialOnAllViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialOnAllViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMestMaterialOnAllViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMestMaterialOnAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMestMaterialOnAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listImpMestMaterialOnAlls = new HisImpMestMaterialManager(param).GetView(impMestMaterialOnAllViewFilter);

                    //================================================================================================================Xuat Trong ki vật tư
                    HisExpMestMaterialViewFilterQuery expMestMaterialOnAllViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialOnAllViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMaterialOnAllViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMaterialOnAllViewFilter.IS_EXPORT = true;
                    expMestMaterialOnAllViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    listExpMestMaterialOnAlls = new HisExpMestMaterialManager(param).GetView(expMestMaterialOnAllViewFilter);

                }
                #endregion


                //Tao loai nhap xuat
                makeRdo();

                //Lay thong tin medicine, material cua cac thuoc vat tu
                GetMediMate();

                //Lay cac li do xuat cua cac phieu xuat trong ki
                GetExpMestOn();

                //Lay cac thuoc vat tu dau vao kiem ke
                GetInputEndAmount();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetExpMestOn()
        {
            try
            {
                ExpMestIdReasons = new ManagerSql().Get(this.castFilter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetInputEndAmount()
        {
            try
            {
                InputMedicines = new ManagerSql().GetMediInput(this.castFilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                InputMaterials = new ManagerSql().GetMateInput(this.castFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                var listSub = (from r in listImpMestMedicineBeforeAlls select new Mrs00584RDO(r, dicMedicineType, Medicines)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialBeforeAlls select new Mrs00584RDO(r, dicMaterialType, Materials)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineBeforeAlls select new Mrs00584RDO(r, dicMedicineType, Medicines)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialBeforeAlls select new Mrs00584RDO(r, dicMaterialType, Materials)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineOnAlls select new Mrs00584RDO(r, dicMedicineType, dicImpAmountType, Medicines)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialOnAlls select new Mrs00584RDO(r, dicMaterialType, dicImpAmountType, Materials)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineOnAlls select new Mrs00584RDO(r, dicMedicineType, dicExpAmountType, Medicines, ExpMestIdReasons)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialOnAlls select new Mrs00584RDO(r, dicMaterialType, dicExpAmountType, Materials, ExpMestIdReasons)).ToList();
                ListRdo.AddRange(listSub);
                ListRdoFromSupplier = ListRdo.Where(o => o.IS_FROM_SUPPLIER).ToList();
                ListRdoFromSupplier = groupByMedicineIdMaterialId(ListRdoFromSupplier);
                ListRdoFromSupplier = groupByServiceAndPriceAndSupplier(ListRdoFromSupplier);
                AddInfoGroup(ListRdoFromSupplier);
                ListRdoMedicine = groupByMedicineIdMaterialId(ListRdo);
                ListRdo = groupByMedicineIdMaterialId(ListRdo);
                ListRdo = groupByServiceAndPriceAndSupplier(ListRdo);
                AddInfoGroup(ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        private List<Mrs00584RDO> groupByMedicineIdMaterialId(List<Mrs00584RDO> ListRdo)
        {
            List<Mrs00584RDO> result = new List<Mrs00584RDO>();
            try
            {
                var group = ListRdo.GroupBy(g => new { g.MEDICINE_ID, g.MATERIAL_ID}).ToList();
                Decimal sum = 0;
                Mrs00584RDO rdo;
                List<Mrs00584RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00584RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00584RDO();
                    listSub = item.ToList<Mrs00584RDO>();
                    foreach (var i in listSub)
                    {
                        if (i.DIC_EXP_MEST_REASON != null)
                        {
                            if (rdo.DIC_EXP_MEST_REASON == null)
                            {
                                rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                            }
                            foreach (var dc in i.DIC_EXP_MEST_REASON)
                            {
                                if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                                {
                                    rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                                }
                                else
                                {
                                    rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                                }
                            }

                        }
                    }
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else if (!field.Name.Contains("DIC_EXP_MEST_REASON"))
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00584RDO()));
                        }
                    }
                    if (this.castFilter.IS_MERGE_BY_SERVICE == true)
                    {
                        rdo.IMP_PRICE = 0;
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00584RDO>();
            }
            return result;
        }

        private void AddInfoGroup(List<Mrs00584RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDICINE_ID);
                if (medicine != null)
                {
                    item.BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;
                    item.MEDICINE_REGISTER_NUMBER = medicine.MEDICINE_REGISTER_NUMBER;
                }
                var material = Materials.FirstOrDefault(o => o.ID == item.MATERIAL_ID);
                if (material != null)
                {
                    item.BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;
                    //item.MATERIAL_REGISTER_NUMBER = material.MATERIAL_REGISTER_NUMBER;
                }
                
                item.INPUT_END_AMOUNT = item.SERVICE_TYPE_ID == 1 ? InputMedicines.Where(o => o.SERVICE_ID == item.SERVICE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID).Sum(s => s.AMOUNT) : InputMaterials.Where(o => o.SERVICE_ID == item.SERVICE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID).Sum(s => s.AMOUNT);
                var medicineType = dicMedicineType.Values.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
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
                    
                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = dicMedicineType.Values.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                    }
                }
                if (item.SERVICE_TYPE_ID == 2)
                {
                    var materialType = dicMaterialType.Values.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư"; 
                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm Vật tư khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm Vật tư khác";
                    }
                }
            }
        }

        private void GetMedicineTypeMaterialType()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            var ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            if (IsNotNullOrEmpty(ListMedicineType))
            {
                foreach (var item in ListMedicineType)
                {
                    dicMedicineType[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListMaterialType))
            {
                foreach (var item in ListMaterialType)
                {
                    dicMaterialType[item.ID] = item;
                }
            }
        }

        private void makeRdo()
        {
            //Danh sach loai nhap, loai xuat

            Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
            Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00584RDO>();

            foreach (var item in piAmount)
            {
                if (dicImpMestType.ContainsKey(item.Name))
                {
                    if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item;
                }
                else if (dicExpMestType.ContainsKey(item.Name))
                {
                    if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item;
                }
            }

        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();

                if (listImpMestMedicineBeforeAlls != null)
                {
                    medicineIds.AddRange(listImpMestMedicineBeforeAlls.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listExpMestMedicineBeforeAlls != null)
                {
                    medicineIds.AddRange(listExpMestMedicineBeforeAlls.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listImpMestMedicineOnAlls != null)
                {
                    medicineIds.AddRange(listImpMestMedicineOnAlls.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listExpMestMedicineOnAlls != null)
                {
                    medicineIds.AddRange(listExpMestMedicineOnAlls.Select(o => o.MEDICINE_ID??0).ToList());
                }
                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                        Medicinefilter.IDs = limit;
                        var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                        Medicines.AddRange(MedicineSub);
                    }
                }

                List<long> materialIds = new List<long>();
                if (listImpMestMaterialBeforeAlls != null)
                {
                    materialIds.AddRange(listImpMestMaterialBeforeAlls.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listExpMestMaterialBeforeAlls != null)
                {
                    materialIds.AddRange(listExpMestMaterialBeforeAlls.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listImpMestMaterialOnAlls != null)
                {
                    materialIds.AddRange(listImpMestMaterialOnAlls.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listExpMestMaterialOnAlls != null)
                {
                    materialIds.AddRange(listExpMestMaterialOnAlls.Select(o => o.MATERIAL_ID??0).ToList());
                }
                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        Materials.AddRange(MaterialSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }
        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<Mrs00584RDO> groupByServiceAndPriceAndSupplier(List<Mrs00584RDO> ListRdo)
        {
            List<Mrs00584RDO> result = new List<Mrs00584RDO>();
            try
            {
                var group = ListRdo.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00584RDO rdo;
                List<Mrs00584RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00584RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00584RDO();
                    listSub = item.ToList<Mrs00584RDO>();
                    foreach (var i in listSub)
                    {
                        if (i.DIC_EXP_MEST_REASON != null)
                        {
                            if (rdo.DIC_EXP_MEST_REASON == null)
                            {
                                rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                            }
                            foreach (var dc in i.DIC_EXP_MEST_REASON)
                            {
                                if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                                {
                                    rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                                }
                                else
                                {
                                    rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                                }
                            }

                        }
                    }
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else if (!field.Name.Contains("DIC_EXP_MEST_REASON"))
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00584RDO()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00584RDO>();
            }
            return result;
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            #region Cac the Single
            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            }
            dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(",", listMediStock.Select(o => o.MEDI_STOCK_NAME).ToList()));
            #endregion

            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenBy(t3 => t3.SERVICE_NAME).ToList();
            objectTag.AddObjectData(store, "Medicines", ListRdoMedicine.Where(p => p.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT || p.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN).ToList());
            objectTag.AddObjectData(store, "Services", ListRdo);
            objectTag.AddObjectData(store, "ServicesNNCC", ListRdoFromSupplier);

            objectTag.AddObjectData(store, "GrandParent", ListRdo.GroupBy(o => o.PARENT_MEDICINE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "PARENT_MEDICINE_TYPE_ID", "PARENT_MEDICINE_TYPE_ID");

            objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" });
            objectTag.AddRelationship(store, "Parent", "ServicesNNCC", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" });
            objectTag.SetUserFunction(store, "Element", new RDOElement());

            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());

        }
        class CustomerFuncMergeSameData : TFlexCelUserFunction
        {
            long MediStockId;
            int SameType;
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {
                    long mediId = Convert.ToInt64(parameters[0]);
                    int ServiceId = Convert.ToInt32(parameters[1]);

                    if (mediId > 0 && ServiceId > 0)
                    {
                        if (SameType == ServiceId && MediStockId == mediId)
                        {
                            return true;
                        }
                        else
                        {
                            MediStockId = mediId;
                            SameType = ServiceId;
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }

        class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
        {
            long Medi_Stock_Id;
            int Service_Type_Id;
            long num_order = 0;
            public RDOCustomerFuncManyRownumberData()
            {
            }
            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length < 1)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                try
                {
                    long mediId = Convert.ToInt64(parameters[0]);
                    int ServiceId = Convert.ToInt32(parameters[1]);

                    if (mediId > 0 && ServiceId > 0)
                    {
                        if (Service_Type_Id == ServiceId && Medi_Stock_Id == mediId)
                        {
                            num_order = num_order + 1;
                        }
                        else
                        {
                            Medi_Stock_Id = mediId;
                            Service_Type_Id = ServiceId;
                            num_order = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Debug(ex);
                }

                return num_order;
            }
        }
    }
}