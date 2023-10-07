using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;

namespace MRS.Processor.Mrs00160
{
    internal class Mrs00160Processor : AbstractProcessor
    {
        List<Mrs00160RDO> listRdo = new List<Mrs00160RDO>();
        Mrs00160Filter CastFilter;
        private List<KeyDepartmentName> listDepartmentNames = new List<KeyDepartmentName>();
        private List<KeyMediStockName> listMediStockNames = new List<KeyMediStockName>();
        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();
        List<HIS_IMP_MEST> ListMobaImpMest = new List<HIS_IMP_MEST>();

        List<V_HIS_MEDICINE_TYPE> hisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        public Mrs00160Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00160Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00160Filter)this.reportFilter;
                var listHisDepartment = HisDepartmentCFG.DEPARTMENTs;
                var listHisMediStock = HisMediStockCFG.HisMediStocks;
                
                var listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
                var listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
                if (!(CastFilter.IS_MEDICINE == true && CastFilter.IS_MATERIAL == false))
                {
                    var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                    {
                        EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID,
                        EXP_TIME_FROM = CastFilter.EXP_TIME_FROM,
                        EXP_TIME_TO = CastFilter.EXP_TIME_TO,
                        MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs,
                        EXP_MEST_TYPE_ID = CastFilter.EXP_MEST_TYPE_ID,
                    };
                    listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial);
                }
                if (!(CastFilter.IS_MEDICINE == false && CastFilter.IS_MATERIAL == true))
                {
                    var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                    {
                        EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID,
                        EXP_TIME_FROM = CastFilter.EXP_TIME_FROM,
                        EXP_TIME_TO = CastFilter.EXP_TIME_TO,
                        MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs,
                        EXP_MEST_TYPE_ID = CastFilter.EXP_MEST_TYPE_ID,
                    };
                    listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                }
                hisMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
                //loc lay cac khoa yeu cau
                var reqDepartmentid = new List<long>();
                if (listExpMestMaterial != null)
                {
                    reqDepartmentid.AddRange(listExpMestMaterial.Where(o=> o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS).Select(s => s.REQ_DEPARTMENT_ID).Distinct().ToList());
                }
                if (listExpMestMedicine != null)
                {
                    reqDepartmentid.AddRange(listExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS).Select(s => s.REQ_DEPARTMENT_ID).Distinct().ToList());
                }
                reqDepartmentid = reqDepartmentid.Distinct().ToList();
                listHisDepartment = listHisDepartment.Where(o => reqDepartmentid.Contains(o.ID)).ToList();

                //Phieu xuat
                var listExpMestId = new List<long>();
                if (listExpMestMedicine != null)
                {
                    listExpMestId.AddRange(listExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }

                if (listExpMestMaterial != null)
                {
                    listExpMestId.AddRange(listExpMestMaterial.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                listExpMestId = listExpMestId.Distinct().ToList();
                if (listExpMestId.Count > 0)
                {
                    var skip = 0;
                    while (listExpMestId.Count - skip > 0)
                    {
                        var limit = listExpMestId.Skip(skip).Take(500).ToList();
                        skip = skip + 500;
                        HisExpMestFilterQuery exFilter = new HisExpMestFilterQuery();
                        exFilter.IDs = limit;
                        var ListExpMestSub = new HisExpMestManager().Get(exFilter);
                        if (ListExpMestSub != null)
                        {
                            ListExpMest.AddRange(ListExpMestSub);
                        }
                    }
                }

                listHisMediStock = listHisMediStock.Where(o => ListExpMest.Exists(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && p.IMP_MEDI_STOCK_ID == o.ID) && o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                //Danh sách thu hồi
               
                if (listExpMestId.Count > 0)
                {
                    var skip = 0;
                    while (listExpMestId.Count - skip > 0)
                    {
                        var limit = listExpMestId.Skip(skip).Take(500).ToList();
                        skip = skip + 500;
                        HisImpMestFilterQuery mobaFilter = new HisImpMestFilterQuery();
                        mobaFilter.MOBA_EXP_MEST_IDs = limit;
                        mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        var ListMobaImpMestSub = new HisImpMestManager().Get(mobaFilter);
                        if (ListMobaImpMestSub != null)
                        {
                            ListMobaImpMest.AddRange(ListMobaImpMestSub);
                        }
                    }
                }
                //Chi tiết thuốc vật tư thu hồi

                var listImpMest = ListMobaImpMest.Select(o => o.ID).ToList();
                if (listImpMest.Count > 0)
                {
                    var skip = 0;
                    while (listImpMest.Count - skip > 0)
                    {
                        var limit = listImpMest.Skip(skip).Take(500).ToList();
                        skip = skip + 500;
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_MEST_IDs = limit;
                        var ListImpMestMedicineSub = new HisImpMestMedicineManager().GetView(impMediFilter);
                        if (ListImpMestMedicineSub != null)
                        {
                            ListImpMestMedicine.AddRange(ListImpMestMedicineSub);
                        }
                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_MEST_IDs = limit;
                        var ListImpMestMaterialSub = new HisImpMestMaterialManager().GetView(impMateFilter);
                        if (ListImpMestMaterialSub != null)
                        {
                            ListImpMestMaterial.AddRange(ListImpMestMaterialSub);
                        }
                    }
                }
                ProcessFilterData(listExpMestMaterial, listExpMestMedicine, listHisDepartment,listHisMediStock);

                AddInfoGroup(listRdo);
                result = true;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void AddInfoGroup(List<Mrs00160RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicineType = hisMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
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
            }
        }


        protected override bool ProcessData()
        {
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("EXP_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.EXP_TIME_FROM));
            dicSingleTag.Add("EXP_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.EXP_TIME_TO));
            if (CastFilter.MEDI_STOCK_IDs != null && HisMediStockCFG.HisMediStocks!=null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(",", HisMediStockCFG.HisMediStocks.Where(p => CastFilter.MEDI_STOCK_IDs.Contains(p.ID)).Select(o => o.MEDI_STOCK_NAME).ToList()));
            }
            foreach (var listDepartmentName in listDepartmentNames)
            {
                dicSingleTag.Add(listDepartmentName.DEPARTMENT_KEY, listDepartmentName.HIS_DEPARTMENT.DEPARTMENT_NAME);
            }
            foreach (var listMediStockName in listMediStockNames)
            {
                dicSingleTag.Add(listMediStockName.MEDI_STOCK_KEY, listMediStockName.HIS_MEDI_STOCK.MEDI_STOCK_NAME);
            }
            listRdo = listRdo.OrderBy(o => o.SERVICE_TYPE_ID).ThenBy(p => p.MEDICINE_TYPE_NAME).ThenBy(p => p.MATERIAL_TYPE_NAME).ToList();
            objectTag.AddObjectData(store, "Report", listRdo);

            objectTag.AddObjectData(store, "GrandParent", listRdo.GroupBy(o => o.MEDICINE_GROUP_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Parent", listRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.MEDICINE_GROUP_ID }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "MEDICINE_GROUP_ID", "MEDICINE_GROUP_ID");

            objectTag.AddRelationship(store, "Parent", "Report", new string[] { "MEDICINE_LINE_ID", "MEDICINE_GROUP_ID" }, new string[] { "MEDICINE_LINE_ID", "MEDICINE_GROUP_ID" });

        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial, List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine, List<HIS_DEPARTMENT> listHisDepartment, List<V_HIS_MEDI_STOCK> listHisMediStock)
        {
            try
            {
                var number = 0;
                foreach (var hisDepartment in listHisDepartment)
                {
                    var gg = new KeyDepartmentName
                    {
                        DEPARTMENT_KEY = string.Format("DEPARTMENT_NAME_{0}", number),
                        HIS_DEPARTMENT = hisDepartment
                    };
                    listDepartmentNames.Add(gg);
                    number++;
                }
                number = 0;
                foreach (var hisMediStock in listHisMediStock)
                {
                    var gg = new KeyMediStockName
                    {
                        MEDI_STOCK_KEY = string.Format("MEDI_STOCK_NAME_{0}", number),
                        HIS_MEDI_STOCK = hisMediStock
                    };
                    listMediStockNames.Add(gg);
                    number++;
                }

                var GroupByMaterialIds = listExpMestMaterial.GroupBy(s => s.TDL_MATERIAL_TYPE_ID).ToList();
                foreach (var group in GroupByMaterialIds)
                {
                    List<V_HIS_EXP_MEST_MATERIAL> listSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                    Mrs00160RDO rdo = new Mrs00160RDO();
                    rdo.MOBA_EXP_AMOUNT = ListImpMestMaterial.Where(o => o.MATERIAL_TYPE_ID == group.Key).Sum(s=>s.AMOUNT);

                    rdo.MATERIAL_TYPE_CODE = listSub.First().MATERIAL_TYPE_CODE;
                    rdo.MATERIAL_TYPE_NAME = listSub.First().MATERIAL_TYPE_NAME;
                    rdo.SERVICE_ID = listSub.First().SERVICE_ID;
                    rdo.SERVICE_UNIT_NAME = listSub.First().SERVICE_UNIT_NAME;
                    rdo.SERVICE_TYPE_ID = 2;
                    rdo.DIC_DEPARTMENT_AMOUNT = listSub.GroupBy(o => DepartmentCode(o.REQ_DEPARTMENT_ID) ?? "KHAC").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_MEDI_STOCK_AMOUNT = listSub.GroupBy(o => CabinetCode(ImpMediStockId(o.EXP_MEST_ID)) ?? "KHAC").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    for (var i = 0; i < listDepartmentNames.Count; i++)
                    {
                        var amounOfDepartment = listSub.Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && o.REQ_DEPARTMENT_ID == listDepartmentNames[i].HIS_DEPARTMENT.ID).Sum(s => s.AMOUNT);
                       
                        switch (i)
                        {
                            case 0: rdo.DEPARTMENT_0 = amounOfDepartment; break;
                            case 1: rdo.DEPARTMENT_1 = amounOfDepartment; break;
                            case 2: rdo.DEPARTMENT_2 = amounOfDepartment; break;
                            case 3: rdo.DEPARTMENT_3 = amounOfDepartment; break;
                            case 4: rdo.DEPARTMENT_4 = amounOfDepartment; break;
                            case 5: rdo.DEPARTMENT_5 = amounOfDepartment; break;
                            case 6: rdo.DEPARTMENT_6 = amounOfDepartment; break;
                            case 7: rdo.DEPARTMENT_7 = amounOfDepartment; break;
                            case 8: rdo.DEPARTMENT_8 = amounOfDepartment; break;
                            case 9: rdo.DEPARTMENT_9 = amounOfDepartment; break;
                            case 10: rdo.DEPARTMENT_10 = amounOfDepartment; break;
                            case 11: rdo.DEPARTMENT_11 = amounOfDepartment; break;
                            case 12: rdo.DEPARTMENT_12 = amounOfDepartment; break;
                            case 13: rdo.DEPARTMENT_13 = amounOfDepartment; break;
                            case 14: rdo.DEPARTMENT_14 = amounOfDepartment; break;
                            case 15: rdo.DEPARTMENT_15 = amounOfDepartment; break;
                            case 16: rdo.DEPARTMENT_16 = amounOfDepartment; break;
                            case 17: rdo.DEPARTMENT_17 = amounOfDepartment; break;
                            case 18: rdo.DEPARTMENT_18 = amounOfDepartment; break;
                            case 19: rdo.DEPARTMENT_19 = amounOfDepartment; break;
                            case 20: rdo.DEPARTMENT_20 = amounOfDepartment; break;
                            case 21: rdo.DEPARTMENT_21 = amounOfDepartment; break;
                            case 22: rdo.DEPARTMENT_22 = amounOfDepartment; break;
                            case 23: rdo.DEPARTMENT_23 = amounOfDepartment; break;
                            case 24: rdo.DEPARTMENT_24 = amounOfDepartment; break;
                            case 25: rdo.DEPARTMENT_25 = amounOfDepartment; break;
                        }
                        rdo.EXP_AMOUNT += amounOfDepartment;
                    }
                    for (var i = 0; i < listMediStockNames.Count; i++)
                    {
                        var amounOfMediStock = listSub.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && ListExpMest.Exists(p => p.ID == o.EXP_MEST_ID && p.IMP_MEDI_STOCK_ID == listMediStockNames[i].HIS_MEDI_STOCK.ID)).Sum(s => s.AMOUNT);

                        switch (i)
                        {
                            case 0: rdo.MEDI_STOCK_0 = amounOfMediStock; break;
                            case 1: rdo.MEDI_STOCK_1 = amounOfMediStock; break;
                            case 2: rdo.MEDI_STOCK_2 = amounOfMediStock; break;
                            case 3: rdo.MEDI_STOCK_3 = amounOfMediStock; break;
                            case 4: rdo.MEDI_STOCK_4 = amounOfMediStock; break;
                            case 5: rdo.MEDI_STOCK_5 = amounOfMediStock; break;
                            case 6: rdo.MEDI_STOCK_6 = amounOfMediStock; break;
                            case 7: rdo.MEDI_STOCK_7 = amounOfMediStock; break;
                            case 8: rdo.MEDI_STOCK_8 = amounOfMediStock; break;
                            case 9: rdo.MEDI_STOCK_9 = amounOfMediStock; break;
                            case 10: rdo.MEDI_STOCK_10 = amounOfMediStock; break;
                            case 11: rdo.MEDI_STOCK_11 = amounOfMediStock; break;
                            case 12: rdo.MEDI_STOCK_12 = amounOfMediStock; break;
                            case 13: rdo.MEDI_STOCK_13 = amounOfMediStock; break;
                            case 14: rdo.MEDI_STOCK_14 = amounOfMediStock; break;
                            case 15: rdo.MEDI_STOCK_15 = amounOfMediStock; break;
                            case 16: rdo.MEDI_STOCK_16 = amounOfMediStock; break;
                            case 17: rdo.MEDI_STOCK_17 = amounOfMediStock; break;
                            case 18: rdo.MEDI_STOCK_18 = amounOfMediStock; break;
                            case 19: rdo.MEDI_STOCK_19 = amounOfMediStock; break;
                            case 20: rdo.MEDI_STOCK_20 = amounOfMediStock; break;
                            case 21: rdo.MEDI_STOCK_21 = amounOfMediStock; break;
                            case 22: rdo.MEDI_STOCK_22 = amounOfMediStock; break;
                            case 23: rdo.MEDI_STOCK_23 = amounOfMediStock; break;
                            case 24: rdo.MEDI_STOCK_24 = amounOfMediStock; break;
                            case 25: rdo.MEDI_STOCK_25 = amounOfMediStock; break;
                        }
                        rdo.EXP_AMOUNT += amounOfMediStock;
                    }
                    listRdo.Add(rdo);
                }

                var GroupByMedicineIds = listExpMestMedicine.GroupBy(s => s.TDL_MEDICINE_TYPE_ID).ToList();
                foreach (var group in GroupByMedicineIds)
                {
                    List<V_HIS_EXP_MEST_MEDICINE> listSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                    Mrs00160RDO rdo = new Mrs00160RDO();

                    rdo.MEDICINE_TYPE_CODE = listSub.First().MEDICINE_TYPE_CODE;
                    rdo.MEDICINE_TYPE_NAME = listSub.First().MEDICINE_TYPE_NAME;
                    rdo.SERVICE_ID = listSub.First().SERVICE_ID;
                    rdo.SERVICE_UNIT_NAME = listSub.First().SERVICE_UNIT_NAME;
                    rdo.SERVICE_TYPE_ID = 1;
                    rdo.MOBA_EXP_AMOUNT = ListImpMestMedicine.Where(o => o.MEDICINE_TYPE_ID == group.Key).Sum(s => s.AMOUNT);
                    rdo.DIC_DEPARTMENT_AMOUNT = listSub.GroupBy(o => DepartmentCode(o.REQ_DEPARTMENT_ID) ?? "KHAC").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_MEDI_STOCK_AMOUNT = listSub.GroupBy(o => CabinetCode(ImpMediStockId(o.EXP_MEST_ID))??"KHAC").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    for (var i = 0; i < listDepartmentNames.Count; i++)
                    {
                        var amounOfDepartment = listSub.Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && o.REQ_DEPARTMENT_ID == listDepartmentNames[i].HIS_DEPARTMENT.ID).Sum(s => s.AMOUNT);
                        
                        switch (i)
                        {
                            case 0: rdo.DEPARTMENT_0 = amounOfDepartment; break;
                            case 1: rdo.DEPARTMENT_1 = amounOfDepartment; break;
                            case 2: rdo.DEPARTMENT_2 = amounOfDepartment; break;
                            case 3: rdo.DEPARTMENT_3 = amounOfDepartment; break;
                            case 4: rdo.DEPARTMENT_4 = amounOfDepartment; break;
                            case 5: rdo.DEPARTMENT_5 = amounOfDepartment; break;
                            case 6: rdo.DEPARTMENT_6 = amounOfDepartment; break;
                            case 7: rdo.DEPARTMENT_7 = amounOfDepartment; break;
                            case 8: rdo.DEPARTMENT_8 = amounOfDepartment; break;
                            case 9: rdo.DEPARTMENT_9 = amounOfDepartment; break;
                            case 10: rdo.DEPARTMENT_10 = amounOfDepartment; break;
                            case 11: rdo.DEPARTMENT_11 = amounOfDepartment; break;
                            case 12: rdo.DEPARTMENT_12 = amounOfDepartment; break;
                            case 13: rdo.DEPARTMENT_13 = amounOfDepartment; break;
                            case 14: rdo.DEPARTMENT_14 = amounOfDepartment; break;
                            case 15: rdo.DEPARTMENT_15 = amounOfDepartment; break;
                            case 16: rdo.DEPARTMENT_16 = amounOfDepartment; break;
                            case 17: rdo.DEPARTMENT_17 = amounOfDepartment; break;
                            case 18: rdo.DEPARTMENT_18 = amounOfDepartment; break;
                            case 19: rdo.DEPARTMENT_19 = amounOfDepartment; break;
                            case 20: rdo.DEPARTMENT_20 = amounOfDepartment; break;
                            case 21: rdo.DEPARTMENT_21 = amounOfDepartment; break;
                            case 22: rdo.DEPARTMENT_22 = amounOfDepartment; break;
                            case 23: rdo.DEPARTMENT_23 = amounOfDepartment; break;
                            case 24: rdo.DEPARTMENT_24 = amounOfDepartment; break;
                            case 25: rdo.DEPARTMENT_25 = amounOfDepartment; break;
                        }

                        rdo.EXP_AMOUNT += amounOfDepartment;
                    }
                    for (var i = 0; i < listMediStockNames.Count; i++)
                    {
                        var amounOfMediStock = listSub.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS && ListExpMest.Exists(p => p.ID == o.EXP_MEST_ID && p.IMP_MEDI_STOCK_ID == listMediStockNames[i].HIS_MEDI_STOCK.ID)).Sum(s => s.AMOUNT);

                        switch (i)
                        {
                            case 0: rdo.MEDI_STOCK_0 = amounOfMediStock; break;
                            case 1: rdo.MEDI_STOCK_1 = amounOfMediStock; break;
                            case 2: rdo.MEDI_STOCK_2 = amounOfMediStock; break;
                            case 3: rdo.MEDI_STOCK_3 = amounOfMediStock; break;
                            case 4: rdo.MEDI_STOCK_4 = amounOfMediStock; break;
                            case 5: rdo.MEDI_STOCK_5 = amounOfMediStock; break;
                            case 6: rdo.MEDI_STOCK_6 = amounOfMediStock; break;
                            case 7: rdo.MEDI_STOCK_7 = amounOfMediStock; break;
                            case 8: rdo.MEDI_STOCK_8 = amounOfMediStock; break;
                            case 9: rdo.MEDI_STOCK_9 = amounOfMediStock; break;
                            case 10: rdo.MEDI_STOCK_10 = amounOfMediStock; break;
                            case 11: rdo.MEDI_STOCK_11 = amounOfMediStock; break;
                            case 12: rdo.MEDI_STOCK_12 = amounOfMediStock; break;
                            case 13: rdo.MEDI_STOCK_13 = amounOfMediStock; break;
                            case 14: rdo.MEDI_STOCK_14 = amounOfMediStock; break;
                            case 15: rdo.MEDI_STOCK_15 = amounOfMediStock; break;
                            case 16: rdo.MEDI_STOCK_16 = amounOfMediStock; break;
                            case 17: rdo.MEDI_STOCK_17 = amounOfMediStock; break;
                            case 18: rdo.MEDI_STOCK_18 = amounOfMediStock; break;
                            case 19: rdo.MEDI_STOCK_19 = amounOfMediStock; break;
                            case 20: rdo.MEDI_STOCK_20 = amounOfMediStock; break;
                            case 21: rdo.MEDI_STOCK_21 = amounOfMediStock; break;
                            case 22: rdo.MEDI_STOCK_22 = amounOfMediStock; break;
                            case 23: rdo.MEDI_STOCK_23 = amounOfMediStock; break;
                            case 24: rdo.MEDI_STOCK_24 = amounOfMediStock; break;
                            case 25: rdo.MEDI_STOCK_25 = amounOfMediStock; break;
                        }
                        rdo.EXP_AMOUNT += amounOfMediStock;
                    }
                    listRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        private long? ImpMediStockId(long? expMestId)
        {
            return (this.ListExpMest.FirstOrDefault(o => o.ID == expMestId) ?? new HIS_EXP_MEST()).IMP_MEDI_STOCK_ID;
        }

        private string CabinetCode(long? mediStockId)
        {
            return (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == mediStockId) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
        }

        private string DepartmentCode(long departmentId)
        {
            return (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==departmentId)??new HIS_DEPARTMENT()).DEPARTMENT_CODE;
        }

    }
}
