using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using ACS.Filter;
using AutoMapper;
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
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;

namespace MRS.Processor.Mrs00275
{
    class Mrs00275Processor : AbstractProcessor
    {
        List<Mrs00275RDO> listRdo = new List<Mrs00275RDO>();
        List<Mrs00275RDO> ListParrentRdo = new List<Mrs00275RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        CommonParam paramGet = new CommonParam();
        public Mrs00275Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00275Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00275Filter)reportFilter);
            bool result = true;
            try
            {
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());

                HisExpMestMedicineViewFilterQuery medicinefilter = new HisExpMestMedicineViewFilterQuery();
                medicinefilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                medicinefilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                medicinefilter.EXP_TIME_FROM = filter.TIME_FROM;
                medicinefilter.EXP_TIME_TO = filter.TIME_TO;
                medicinefilter.IS_EXPORT = true;
                var medicines = new HisExpMestMedicineManager(paramGet).GetView(medicinefilter);
                if (IsNotNullOrEmpty(medicines))
                {
                    listMedicine.AddRange(medicines);
                }

                HisExpMestMaterialViewFilterQuery materialfilter = new HisExpMestMaterialViewFilterQuery();
                materialfilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                materialfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                materialfilter.EXP_TIME_FROM = filter.TIME_FROM;
                materialfilter.EXP_TIME_TO = filter.TIME_TO;
                materialfilter.IS_EXPORT = true;
                var materials = new HisExpMestMaterialManager(paramGet).GetView(materialfilter);
                if (IsNotNullOrEmpty(materials))
                {
                    listMaterial.AddRange(materials);
                }
                if (filter.LOGINNAMEs != null)
                {
                    listMaterial = listMaterial.Where(o => filter.LOGINNAMEs.Contains(o.EXP_LOGINNAME)).ToList();
                    listMedicine = listMedicine.Where(o => filter.LOGINNAMEs.Contains(o.EXP_LOGINNAME)).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                listRdo.Clear();
                ListParrentRdo.Clear();

                if (IsNotNullOrEmpty(listMedicine))
                {
                    var GroupbyPrice = listMedicine.GroupBy(o => new { o.MEDICINE_ID, o.PRICE }).ToList();
                    foreach (var group in GroupbyPrice)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> sub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00275RDO rdo = new Mrs00275RDO();
                        rdo.PARENT_ID = ListMedicineType.Where(o => o.ID == sub.First().MEDICINE_TYPE_ID).First().PARENT_ID ?? 0;
                        rdo.PARENT_NAME = (rdo.PARENT_ID != 0) ? ListMedicineType.Where(o => o.ID == rdo.PARENT_ID).First().MEDICINE_TYPE_NAME : "NHÓM KHÁC";
                        rdo.MAME_TYPE_NAME = sub.First().MEDICINE_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = sub.First().MEDICINE_TYPE_CODE;
                        rdo.NATIONAL_NAME = sub.First().NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = sub.First().SERVICE_UNIT_NAME;
                        rdo.PRICE = sub.First().PRICE ?? sub.First().IMP_PRICE;
                        rdo.AMOUNT = sub.Sum(o => o.AMOUNT);
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;

                        rdo.PACKAGE_NUMBER = sub.First().PACKAGE_NUMBER;
                        listRdo.Add(rdo);
                    }
                    listRdo = listRdo.OrderBy(o => o.MAME_TYPE_NAME).ToList();
                }

                if (IsNotNullOrEmpty(listMaterial))
                {
                    var GroupbyPrice = listMaterial.GroupBy(o => new { o.MATERIAL_ID, o.PRICE }).ToList();
                    foreach (var group in GroupbyPrice)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> sub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00275RDO rdo = new Mrs00275RDO();
                        rdo.PARENT_ID = ListMaterialType.Where(o => o.ID == sub.First().MATERIAL_TYPE_ID).First().PARENT_ID ?? 0;
                        rdo.PARENT_NAME = (rdo.PARENT_ID != 0) ? ListMaterialType.Where(o => o.ID == rdo.PARENT_ID).First().MATERIAL_TYPE_NAME : "NHÓM KHÁC";
                        rdo.MAME_TYPE_NAME = sub.First().MATERIAL_TYPE_NAME;
                        rdo.MAME_TYPE_CODE = sub.First().MATERIAL_TYPE_CODE;
                        rdo.NATIONAL_NAME = sub.First().NATIONAL_NAME;
                        rdo.SERVICE_UNIT_NAME = sub.First().SERVICE_UNIT_NAME;
                        rdo.PRICE = sub.First().PRICE ?? sub.First().IMP_PRICE;
                        rdo.AMOUNT = sub.Sum(o => o.AMOUNT);
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;

                        rdo.PACKAGE_NUMBER = sub.First().PACKAGE_NUMBER;
                        listRdo.Add(rdo);
                    }
                    listRdo = listRdo.OrderBy(o => o.MAME_TYPE_NAME).ToList();
                }
                ListParrentRdo = listRdo.GroupBy(o => o.PARENT_ID).Select(p => p.First()).ToList();
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
            if (((Mrs00275Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00275Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00275Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00275Filter)reportFilter).TIME_TO));
            }
            if (IsNotNullOrEmpty(((Mrs00275Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs))
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00275Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToArray()));
            }
            if (IsNotNullOrEmpty(((Mrs00275Filter)reportFilter).LOGINNAMEs))
            {
                dicSingleTag.Add("SALE_USERNAME", string.Join(", ", new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => ((Mrs00275Filter)reportFilter).LOGINNAMEs.Contains(o.LOGINNAME)).Select(p => p.USERNAME).ToList()));
            }
            objectTag.AddObjectData(store, "MaMe", listRdo);
            objectTag.AddObjectData(store, "ParentServices", ListParrentRdo);
            objectTag.AddRelationship(store, "ParentServices", "MaMe", "PARENT_ID", "PARENT_ID");
        }
    }
}
