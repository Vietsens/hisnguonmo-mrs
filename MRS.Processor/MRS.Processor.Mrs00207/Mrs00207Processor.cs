using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
//using MOS.MANAGER.HisMobaImpMest; 
//using MOS.MANAGER.HisPrescription; 
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceReq;
using Inventec.Common.Repository;
using System.Reflection;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisMedicineGroup;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00207
{
    public class Mrs00207Processor : AbstractProcessor
    {

        CommonParam paramGet = new CommonParam();
        List<Mrs00207RDO> ListParent = new List<Mrs00207RDO>();
        List<Mrs00207RDO> ListTreatment = new List<Mrs00207RDO>();
        List<Mrs00207RDO> ListRdo = new List<Mrs00207RDO>();

        Mrs00207Filter filter = new Mrs00207Filter();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceRetyCat = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicParentMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicParentMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, HIS_MEDICINE_GROUP> dicMedicineGroup = new Dictionary<long, HIS_MEDICINE_GROUP>();
        Dictionary<string, AMOUNT_OF_EXP> dicAmountOfExp = new Dictionary<string, AMOUNT_OF_EXP>();
        Dictionary<long, HIS_SERVICE> dicService = new Dictionary<long, HIS_SERVICE>();
        public Mrs00207Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00207Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                filter = (Mrs00207Filter)this.reportFilter;
                var paramGet = new CommonParam();
                //get dữ liệu:
                if (filter.IS_MEDICINE == true || filter.IS_MEDICINE != true && filter.IS_MATERIAL != true)
                {
                    var listServiceReq = new ManagerSql().GetServiceReq(filter);
                    ListRdo.AddRange(listServiceReq);
                }
                if (filter.IS_MATERIAL == true || filter.IS_MEDICINE != true && filter.IS_MATERIAL != true)
                {
                    var listServiceReqMaterial = new ManagerSql().GetServiceReqMaterial(filter);
                    ListRdo.AddRange(listServiceReqMaterial);
                }
                listService = ListRdo.Select(o => new HIS_SERVICE() { SERVICE_CODE = o.SERVICE_CODE, SERVICE_NAME = o.SERVICE_NAME}).GroupBy(g=>g.SERVICE_CODE).Select(p=>p.First()).OrderBy(q=>q.SERVICE_NAME).ToList();
                GetServiceRetyCat();
                HisMedicineTypeViewFilterQuery medicineTypeFilter = new HisMedicineTypeViewFilterQuery();
                medicineTypeFilter.ID = filter.MEDICINE_TYPE_ID;
                var medicineTypes = new HisMedicineTypeManager().GetView(medicineTypeFilter);
                if (medicineTypes != null)
                {
                    listMedicineType = medicineTypes;
                }

                dicParentMedicineType = listMedicineType.ToDictionary(o => o.ID, q => listMedicineType.FirstOrDefault(p => p.ID == q.PARENT_ID) ?? new V_HIS_MEDICINE_TYPE());
                dicMedicineType = listMedicineType.ToDictionary(o => o.ID);

                HisMaterialTypeViewFilterQuery materialTypeFilter = new HisMaterialTypeViewFilterQuery();
                var materialTypes = new HisMaterialTypeManager().GetView(materialTypeFilter);
                if (materialTypes != null)
                {
                    listMaterialType = materialTypes;
                }

                dicParentMaterialType = listMaterialType.ToDictionary(o => o.ID, q => listMaterialType.FirstOrDefault(p => p.ID == q.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE());
                dicMaterialType = listMaterialType.ToDictionary(o => o.ID);

                //GetMedicineGroup();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetServiceRetyCat()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatfilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatfilter.REPORT_TYPE_CODE__EXACT = "MRS00207";
            var serviceRetyCats = new HisServiceRetyCatManager().GetView(serviceRetyCatfilter);
            dicServiceRetyCat = serviceRetyCats.GroupBy(o => o.SERVICE_ID).ToDictionary(o => o.Key, q => q.First());
        }
        
        //private void GetMedicineGroup()
        //{
        //    HisMedicineGroupFilterQuery medicineGroupfilter = new HisMedicineGroupFilterQuery();
        //    var medicineGroups = new HisMedicineGroupManager().Get(medicineGroups);
        //    dicMedicineGroup = medicineGroups.GroupBy(o => o.ID).ToDictionary(o => o.Key, q => q.First());
        //}
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo = ListRdo.Where(p => p.AMOUNT_TRUST > 0).OrderBy(o => o.EXP_TIME).ToList();
                long i = 0;
                foreach (var rdo in ListRdo)
                {
                    rdo.ROW_POS = i++;
                    if (dicServiceRetyCat.ContainsKey(rdo.SERVICE_ID))
                    {
                        rdo.CATEGORY_CODE = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_CODE;
                        rdo.CATEGORY_NAME = dicServiceRetyCat[rdo.SERVICE_ID].CATEGORY_NAME;
                    }

                    if (dicParentMaterialType.ContainsKey(rdo.MEDICINE_TYPE_ID) && rdo.TYPE == "VATTU")
                    {
                        rdo.PARENT_MEDICINE_TYPE_CODE = dicParentMaterialType[rdo.MEDICINE_TYPE_ID].MATERIAL_TYPE_CODE;
                        rdo.PARENT_MEDICINE_TYPE_NAME = dicParentMaterialType[rdo.MEDICINE_TYPE_ID].MATERIAL_TYPE_NAME;
                    }

                    if (dicParentMedicineType.ContainsKey(rdo.MEDICINE_TYPE_ID) && rdo.TYPE == "THUOC")
                    {
                        rdo.PARENT_MEDICINE_TYPE_CODE = dicParentMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                        rdo.PARENT_MEDICINE_TYPE_NAME = dicParentMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                    }

                    if (dicMedicineType.ContainsKey(rdo.MEDICINE_TYPE_ID) && rdo.TYPE == "THUOC")
                    {
                        rdo.MEDICINE_LINE_ID = dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_LINE_ID??0;
                        rdo.MEDICINE_LINE_CODE = dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_LINE_CODE;
                        rdo.MEDICINE_LINE_NAME = dicMedicineType[rdo.MEDICINE_TYPE_ID].MEDICINE_LINE_NAME;
                    }
                }
                var groupByExp = ListRdo.GroupBy(o => o.EXP_MEST_CODE).ToList();
                foreach (var item in groupByExp)
                {
                    var listSub = item.ToList<Mrs00207RDO>();
                    
                    item.First().COUNT_MEDICINE_TYPE_CO = listSub.Where(x => x.MEDICINE_GROUP_CODE == "CO").Select(x => x.MEDICINE_TYPE_ID).Distinct().Count();
                    item.First().MEDICINE_TYPE_NAME_CO = string.Join(",", listSub.Where(x => x.MEDICINE_GROUP_CODE == "CO").Select(x => x.MEDICINE_TYPE_NAME).Distinct().ToList().ToArray());
                    item.First().COUNT_MEDICINE_TYPE_KS = listSub.Where(x => x.MEDICINE_GROUP_CODE == "KS").Select(x => x.MEDICINE_TYPE_ID).Distinct().Count();
                    item.First().MEDICINE_TYPE_NAME_KS = string.Join(",", listSub.Where(x => x.MEDICINE_GROUP_CODE == "KS").Select(x => x.MEDICINE_TYPE_NAME).Distinct().ToList().ToArray());
                    item.First().COUNT_MEDICINE_TYPE_DT = listSub.Where(x => x.MEDICINE_GROUP_CODE == "DT").Select(x => x.MEDICINE_TYPE_ID).Distinct().Count();
                    item.First().MEDICINE_TYPE_NAME_DT = string.Join(",", listSub.Where(x => x.MEDICINE_GROUP_CODE == "DT").Select(x => x.MEDICINE_TYPE_NAME).Distinct().ToList().ToArray());
                   
                    item.First().COUNT_MEDICINE_TYPE_VTM = listSub.Where(x =>x.SERVICE_CODE!=null&& x.SERVICE_CODE.Contains("VTMIN")).Select(x => x.MEDICINE_TYPE_ID).Count();
                    item.First().MEDICINE_TYPE_NAME_VTM = string.Join(",", listSub.Where(x => x.SERVICE_CODE != null && x.SERVICE_CODE.Contains("VTMIN")).Select(x => x.MEDICINE_TYPE_NAME).Distinct().ToList().ToArray());
                    item.First().COUNT_MEDICINE_TYPE_TIEM = listSub.Where(x => x.MEDICINE_USE_FORM_NAME != null && x.MEDICINE_USE_FORM_NAME.ToLower().Contains("tiêm")).Select(x => x.MEDICINE_TYPE_ID).Count();
                    item.First().MEDICINE_TYPE_NAME_TIEM = string.Join(",", listSub.Where(x => x.MEDICINE_USE_FORM_NAME != null && x.MEDICINE_USE_FORM_NAME.ToLower().Contains("tiêm")).Select(x => x.MEDICINE_TYPE_NAME).Distinct().ToList().ToArray());

                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""] = new AMOUNT_OF_EXP();
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].EXP_MEST_CODE = listSub.First().EXP_MEST_CODE;
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].COUNT_MEDICINE_TYPE = listSub.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count();
                    //nhóm dòng G
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].DIC_BID_PACKAGE_CODE = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Sum(s => (decimal)1));
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].DIC_PARENT_METY_CODE = listSub.GroupBy(o =>o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Sum(s => (decimal)1));
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].DIC_MEDICINE_GROUP_CODE = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Sum(s => (decimal)1));
                    dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].DIC_CATEGORY_CODE = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Sum(s => (decimal)1));
                    //dicAmountOfExp[listSub.First().EXP_MEST_CODE ?? ""].DIC_PARENT_MEDICINE = listSub.GroupBy(o => o.GROUP_PARENT_MEDICINE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Sum(s => (decimal)1));
                }
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXMM") && this.dicDataFilter["KEY_GROUP_EXMM"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXMM"].ToString()))
                {
                    GroupByKey(this.dicDataFilter["KEY_GROUP_EXMM"].ToString());
                }
                ListParent = ListRdo.Count > 0 ? ListRdo.GroupBy(o => o.PATIENT_ID).Select(p => p.First()).ToList() : new List<Mrs00207RDO>();
                ListTreatment = ListRdo.Count > 0 ? ListRdo.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList() : new List<Mrs00207RDO>();
                //}
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
            return result;
        }


        private void GroupByKey(string GroupKeyExmm)
        {
            var group = ListRdo.GroupBy(o => string.Format(GroupKeyExmm, o.MEDI_STOCK_ID, o.MEDICINE_TYPE_ID, o.MEDICINE_GROUP_ID, o.VIR_PRICE, o.REQUEST_DEPARTMENT_CODE, o.REQUEST_LOGINNAME, o.TDL_TREATMENT_ID, o.REQUEST_ROOM_CODE, o.MEDICINE_ID, o.EXP_MEST_CODE, o.AGGR_EXP_MEST_CODE, o.TYPE, o.EXP_MEST_TYPE_ID, o.BED_CODE,o.MEDICINE_LINE_ID)).ToList();
            ListRdo.Clear();
            Decimal sum = 0;
            Mrs00207RDO rdo;
            List<Mrs00207RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00207RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00207RDO();
                listSub = item.ToList<Mrs00207RDO>();

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name == "AMOUNT" || field.Name == "AMOUNT_TRUST" || field.Name == "TOTAL_PRICE" || field.Name == "VIR_TOTAL_HEIN_PRICE")
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }

                rdo.DIC_CATE_METY_NAME = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => string.Join(";", q.Select(o => o.MEDICINE_TYPE_NAME).Distinct().ToList()));
                rdo.DIC_CATE_METY_COUNT = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count());
                rdo.DIC_CATE_METY_AMOUNT = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));
                rdo.DIC_CATE_METY_PRICE = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.TOTAL_PRICE));
                rdo.DIC_CATE_METY_PRES = listSub.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.EXP_MEST_ID).Distinct().Count());

                rdo.DIC_GR_METY_NAME = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => string.Join(";", q.Select(o => o.MEDICINE_TYPE_NAME).Distinct().ToList()));
                rdo.DIC_GR_METY_COUNT = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count());
                rdo.DIC_GR_METY_AMOUNT = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));
                rdo.DIC_GR_METY_PRICE = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.TOTAL_PRICE));
                rdo.DIC_GR_METY_PRES = listSub.GroupBy(o => o.MEDICINE_GROUP_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.EXP_MEST_ID).Distinct().Count());

                rdo.DIC_BID_PK_METY_NAME = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => string.Join(";", q.Select(o => o.MEDICINE_TYPE_NAME).Distinct().ToList()));
                rdo.DIC_BID_PK_METY_COUNT = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count());
                rdo.DIC_BID_PK_METY_AMOUNT = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));
                rdo.DIC_BID_PK_METY_PRICE = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.TOTAL_PRICE));
                rdo.DIC_BID_PK_METY_PRES = listSub.GroupBy(o => o.TDL_BID_PACKAGE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.EXP_MEST_ID).Distinct().Count());

                rdo.DIC_UF_METY_NAME = listSub.GroupBy(o => o.MEDICINE_USE_FORM_CODE ?? "NONE").ToDictionary(p => p.Key, q => string.Join(";", q.Select(o => o.MEDICINE_TYPE_NAME).Distinct().ToList()));
                rdo.DIC_UF_METY_COUNT = listSub.GroupBy(o => o.MEDICINE_USE_FORM_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count());
                rdo.DIC_UF_METY_AMOUNT = listSub.GroupBy(o => o.MEDICINE_USE_FORM_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));
                rdo.DIC_UF_METY_PRICE = listSub.GroupBy(o => o.MEDICINE_USE_FORM_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.TOTAL_PRICE));
                rdo.DIC_UF_METY_PRES = listSub.GroupBy(o => o.MEDICINE_USE_FORM_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.EXP_MEST_ID).Distinct().Count());

                rdo.DIC_PAR_METY_NAME = listSub.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => string.Join(";", q.Select(o => o.MEDICINE_TYPE_NAME).Distinct().ToList()));
                rdo.DIC_PAR_METY_COUNT = listSub.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.MEDICINE_TYPE_ID).Distinct().Count());
                rdo.DIC_PAR_METY_AMOUNT = listSub.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));
                rdo.DIC_PAR_METY_PRICE = listSub.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.TOTAL_PRICE));
                rdo.DIC_PAR_METY_PRES = listSub.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Select(o => o.EXP_MEST_ID).Distinct().Count());

                rdo.DIC_MONTH_DAY_AMOUNT = listSub.GroupBy(o => o.INTRUCTION_DATE != null ? o.INTRUCTION_DATE.ToString().Substring(6, 2) : "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));//yyyyMMddHhMmss
                rdo.DIC_SV_AMOUNT = listSub.GroupBy(o => o.SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => (decimal)q.Sum(s => s.AMOUNT));

                rdo.COUNT_TOTAL_MEDICINE_TYPE = listSub.Select(x => x.MEDICINE_TYPE_ID).Distinct().Count();
                rdo.AMOUNT_TOTAL_MEDICINE_TYPE = listSub.Sum(s => s.AMOUNT);

                rdo.COUNT_TOTAL_EXP_MEST = listSub.Select(x => x.EXP_MEST_ID).Distinct().Count();

                if (!hide) ListRdo.Add(rdo);


            }
        }


        private Mrs00207RDO IsMeaningful(List<Mrs00207RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").OrderByDescending(p => field.GetValue(p)).FirstOrDefault() ?? new Mrs00207RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00207Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00207Filter)this.reportFilter).TIME_TO));
            if (this.filter.DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            }
            if (this.filter.MEDI_STOCK_ID != null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == this.filter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME);
            }
            if (this.filter.MEDICINE_TYPE_ID != null)
            {
                dicSingleTag.Add("MEDICINE_TYPE_CODE", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).MEDICINE_TYPE_CODE);
                dicSingleTag.Add("MEDICINE_TYPE_NAME", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).MEDICINE_TYPE_NAME);
                dicSingleTag.Add("SERVICE_UNIT_NAME", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).SERVICE_UNIT_NAME);
                dicSingleTag.Add("CONCENTRA", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).CONCENTRA);
                dicSingleTag.Add("ACTIVE_INGR_BHYT_CODE", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).ACTIVE_INGR_BHYT_CODE);
                dicSingleTag.Add("ACTIVE_INGR_BHYT_NAME", (listMedicineType.FirstOrDefault(o => o.ID == this.filter.MEDICINE_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE()).ACTIVE_INGR_BHYT_NAME);
            }
            ListRdo = ListRdo.OrderBy(o => o.PATIENT_ID).ToList();
            objectTag.AddObjectData(store, "Name", ListParent);
            objectTag.AddObjectData(store, "Treatment", ListTreatment);

            objectTag.AddObjectData(store, "ExpMest", ListRdo.Where(o => o.ROW_POS <= 1048500).OrderBy(o => o.INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "ExpMestSecond", ListRdo.Where(o => o.ROW_POS > 1048500 && o.ROW_POS <= 1048500 * 2).ToList());
            objectTag.AddObjectData(store, "ExpMestThird", ListRdo.Where(o => o.ROW_POS > 1048500 * 2 && o.ROW_POS <= 1048500 * 3).ToList());
            objectTag.AddObjectData(store, "ExpMestFourth", ListRdo.Where(o => o.ROW_POS > 1048500 * 3 && o.ROW_POS <= 1048500 * 4).ToList());
            objectTag.AddObjectData(store, "ExpMestFifth", ListRdo.Where(o => o.ROW_POS > 1048500 * 4 && o.ROW_POS <= 1048500 * 5).ToList());
            dicSingleTag.Add("TONG_SO_TOA", ListRdo.Select(o=>o.EXP_MEST_CODE).Distinct().Count());
            objectTag.AddRelationship(store, "Name", "ExpMest", "PATIENT_ID", "PATIENT_ID");

            objectTag.AddObjectData(store, "ExpMestDate", ListRdo.GroupBy(o => new { o.EXP_MEST_ID, o.INTRUCTION_DATE }).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Date", ListRdo.GroupBy(o => o.INTRUCTION_DATE).Select(p => p.First()).OrderBy(o => o.INTRUCTION_DATE).ToList());

            objectTag.AddObjectData(store, "PatientType", ListRdo.GroupBy(o => o.TDL_PATIENT_TYPE_CODE).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Categorys", ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList());
            var categorys = ListRdo.Where(q => !string.IsNullOrEmpty(q.CATEGORY_CODE)).GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList();
            for (int i = 0; i < 20; i++)
            {
                Mrs00207RDO category = new Mrs00207RDO();
                if (categorys.Count > 0 && categorys.Count > i)
                {
                    category = categorys[i];
                }
                dicSingleTag.Add(string.Format("CATEGORY_NAME_{0}", i + 1), category.CATEGORY_NAME);
                dicSingleTag.Add(string.Format("CATEGORY_CODE_{0}", i + 1), category.CATEGORY_CODE);

                objectTag.AddObjectData(store, string.Format("MedicineTypes{0}", i + 1), ListRdo.Where(q => !string.IsNullOrEmpty(category.CATEGORY_CODE) && q.CATEGORY_CODE == category.CATEGORY_CODE).GroupBy(o => new { o.MEDICINE_TYPE_ID, o.MEDICINE_GROUP_ID }).Select(p => p.First()).ToList());

                objectTag.AddObjectData(store, string.Format("RequestUsers{0}", i + 1), ListRdo.Where(q => !string.IsNullOrEmpty(category.CATEGORY_CODE) && q.CATEGORY_CODE == category.CATEGORY_CODE).GroupBy(o => o.REQUEST_LOGINNAME).Select(p => p.First()).ToList());
            }

            objectTag.AddObjectData(store, "MedicineTypes", ListRdo.Where(p => !string.IsNullOrEmpty(p.REQUEST_DEPARTMENT_NAME)).GroupBy(o => new { o.MEDICINE_TYPE_ID, o.MEDICINE_GROUP_ID }).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "RequestUsers", ListRdo.GroupBy(o => o.REQUEST_DEPARTMENT_NAME ?? "KHAC").Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "CountExps", dicAmountOfExp.Values.ToList());

            objectTag.AddObjectData(store, "Services", listService);
        }
    }
}