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
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00278
{
    class Mrs00278Processor : AbstractProcessor
    {
        List<Mrs00278RDO> ListMedicineRdo = new List<Mrs00278RDO>();
        List<Mrs00278RDO> ListMaterialRdo = new List<Mrs00278RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST> listSaleExpMest = new List<V_HIS_EXP_MEST>();
        Mrs00278Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00278Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00278Filter);
        }

        protected override bool GetData()
        {
             filter = ((Mrs00278Filter)reportFilter);
            bool result = true;
            try
            {
                HisExpMestMedicineViewFilterQuery medicinefilter = new HisExpMestMedicineViewFilterQuery();
                medicinefilter.EXP_TIME_FROM = filter.TIME_FROM;
                medicinefilter.EXP_TIME_TO = filter.TIME_TO;
                //medicinefilter.CREATOR = filter.CREATOR_LOGINNAME;
                medicinefilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                medicinefilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                medicinefilter.IS_EXPORT = true;
                listMedicine = new HisExpMestMedicineManager(paramGet).GetView(medicinefilter);
                if (IsNotNullOrEmpty(listMedicine) && !string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                {
                    listMedicine = listMedicine.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                }
                if (IsNotNullOrEmpty(listMedicine) && !string.IsNullOrWhiteSpace(filter.LOGINNAME))
                {
                    listMedicine = listMedicine.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME).ToList();
                }

                HisExpMestMaterialViewFilterQuery materialfilter = new HisExpMestMaterialViewFilterQuery();
                materialfilter.EXP_TIME_FROM = filter.TIME_FROM;
                materialfilter.EXP_TIME_TO = filter.TIME_TO;
                //materialfilter.CREATOR = filter.LOGINNAME_SALE;
                materialfilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                materialfilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                materialfilter.IS_EXPORT = true;
                listMaterial = new HisExpMestMaterialManager(paramGet).GetView(materialfilter);
                if (IsNotNullOrEmpty(listMaterial) && !string.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                {
                    listMaterial = listMaterial.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME_SALE).ToList();
                }
                if (IsNotNullOrEmpty(listMaterial) && !string.IsNullOrWhiteSpace(filter.LOGINNAME))
                {
                    listMaterial = listMaterial.Where(o => o.EXP_LOGINNAME == filter.LOGINNAME).ToList();
                }

                List<long> lstExpMestId = new List<long>();
                if (IsNotNullOrEmpty(listMedicine))
                {
                    lstExpMestId.AddRange(listMedicine.Select(s => s.EXP_MEST_ID??0).Distinct().ToList());
                }
                if (IsNotNullOrEmpty(listMaterial))
                {
                    lstExpMestId.AddRange(listMaterial.Select(s => s.EXP_MEST_ID??0).Distinct().ToList());
                }

                lstExpMestId = lstExpMestId.Distinct().ToList();
                if (IsNotNullOrEmpty(lstExpMestId))
                {
                    var skip = 0;
                    while (lstExpMestId.Count - skip > 0)
                    {
                        var listIDs = lstExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                        expMestFilter.IDs = listIDs;
                        expMestFilter.CREATOR = filter.CREATOR_LOGINNAME;
                        var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestFilter);

                        if (IsNotNullOrEmpty(expMest))
                        {
                            listSaleExpMest.AddRange(expMest);
                        }
                    }
                }
                listMedicine = listMedicine.Where(o => listSaleExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                listMaterial = listMaterial.Where(o => listSaleExpMest.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                
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
                ListMedicineRdo.Clear();
                ListMaterialRdo.Clear();
                if (IsNotNullOrEmpty(listMedicine))
                {
                    foreach (var item in listMedicine)
                    {
                        item.PRICE = item.PRICE.HasValue ? item.PRICE.Value * (1 + (item.VAT_RATIO ?? 0)) : item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        item.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    string keyGroup = "{0}_{1}";
                    if(filter.ADD_INFO_IMP_PRICE==true)
                    {
                        keyGroup = "{0}_{1}_{2}";
                    }    
                    var groupbyNameAndPrice = listMedicine.GroupBy(o => string.Format(keyGroup, o.MEDICINE_TYPE_ID, o.PRICE,o.IMP_PRICE )).ToList();
                    foreach (var group in groupbyNameAndPrice)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00278RDO rdo = new Mrs00278RDO();
                        rdo.MEDICINE_TYPE_NAME = listSub.First().MEDICINE_TYPE_NAME;
                        rdo.NATIONAL_NAME = listSub.First().NATIONAL_NAME;
                        rdo.PRICE = listSub.First().PRICE;
                        rdo.IMP_PRICE = listSub.First().IMP_PRICE;
                        rdo.IMP_VAT_RATIO = listSub.First().IMP_VAT_RATIO;
                        rdo.IMP_PRICE_BEFORE_VAT = Math.Round(listSub.First().IMP_PRICE / (1 + listSub.First().IMP_VAT_RATIO), 0);
                        rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        rdo.SERVICE_UNIT_NAME = listSub.First().SERVICE_UNIT_NAME;
                        ListMedicineRdo.Add(rdo);
                    }
                }
                listMaterial = listMaterial.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
                if (IsNotNullOrEmpty(listMaterial))
                {
                    foreach (var item in listMaterial)
                    {
                        item.PRICE = item.PRICE.HasValue ? item.PRICE.Value * (1 + (item.VAT_RATIO ?? 0)) : item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                        item.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    string keyGroup = "{0}_{1}";
                    if (filter.ADD_INFO_IMP_PRICE == true)
                    {
                        keyGroup = "{0}_{1}_{2}";
                    }
                    var groupbyNameAndPrice = listMaterial.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.PRICE,o.IMP_PRICE }).ToList();
                    foreach (var group in groupbyNameAndPrice)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00278RDO rdo = new Mrs00278RDO();
                        rdo.MATERIAL_TYPE_NAME = listSub.First().MATERIAL_TYPE_NAME;
                        rdo.NATIONAL_NAME = listSub.First().NATIONAL_NAME;
                        rdo.PRICE = listSub.First().PRICE;
                        rdo.IMP_PRICE = listSub.First().IMP_PRICE;
                        rdo.IMP_VAT_RATIO = listSub.First().IMP_VAT_RATIO;
                        rdo.IMP_PRICE_BEFORE_VAT = Math.Round(listSub.First().IMP_PRICE / (1 + listSub.First().IMP_VAT_RATIO),0);
                        rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        rdo.SERVICE_UNIT_NAME = listSub.First().SERVICE_UNIT_NAME;
                        ListMaterialRdo.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00278Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00278Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00278Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00278Filter)reportFilter).TIME_TO));
            }
            if (IsNotNullOrEmpty(((Mrs00278Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs))
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00278Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToArray()));
            }
            if (IsNotNullOrEmpty(((Mrs00278Filter)reportFilter).LOGINNAME_SALE))
            {
                var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => ((Mrs00278Filter)reportFilter).LOGINNAME_SALE == o.LOGINNAME).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("USERNAME", x.First().USERNAME);
            }
            if (IsNotNullOrEmpty(((Mrs00278Filter)reportFilter).LOGINNAME))
            {
                var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => ((Mrs00278Filter)reportFilter).LOGINNAME == o.LOGINNAME).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("USERNAME", x.First().USERNAME);
            }

            if (IsNotNullOrEmpty(((Mrs00278Filter)reportFilter).CREATOR_LOGINNAME))
            {
                var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => ((Mrs00278Filter)reportFilter).CREATOR_LOGINNAME == o.LOGINNAME).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("USERNAME", x.First().USERNAME);
            }

            objectTag.AddObjectData(store, "Medicine", ListMedicineRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList());
            objectTag.AddObjectData(store, "Material", ListMaterialRdo.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList());
        }
    }
}
