
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00417
{
    public class Mrs00417Processor : AbstractProcessor
    {
        List<V_HIS_IMP_MEST> listMobaImpMest = new List<V_HIS_IMP_MEST>();

        private List<Mrs00417RDO> ListParentRdo = new List<Mrs00417RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00417RDO> ListRdo = new List<Mrs00417RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        Dictionary<long, V_HIS_EXP_MEST> dicExpMest = new Dictionary<long, V_HIS_EXP_MEST>();
        Dictionary<long, V_HIS_EXP_MEST> dicAggrExpMest = new Dictionary<long, V_HIS_EXP_MEST>();

        List<HIS_EXP_MEST_TYPE> listExpMestType = new List<HIS_EXP_MEST_TYPE>();


        HIS_DEPARTMENT hisDepartment = new HIS_DEPARTMENT();
        private string a = "";
        public Mrs00417Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00417Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                var filter = ((Mrs00417Filter)reportFilter);
                //get dữ liệu:
                List<long> listExpMestIds = new List<long>();
                List<long> listMobaExpMestIds = new List<long>();
                List<long> aggrExpMestIds = new List<long>();
                if (filter.IS_MEDICINE)
                {
                    HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMestMedicineFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMestMedicineFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                    expMestMedicineFilter.IS_EXPORT = true;
                    expMestMedicines = new HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter);
                    listMobaExpMestIds = expMestMedicines.Where(o => o.TH_AMOUNT > 0).Select(o => o.EXP_MEST_ID ?? 0).ToList();
                    listExpMestIds = expMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                    aggrExpMestIds.AddRange(expMestMedicines.Select(o => o.AGGR_EXP_MEST_ID ?? 0).GroupBy(o => o).Select(p => p.First()).ToList());

                }

                if (filter.IS_MATERIAL)
                {
                    HisExpMestMaterialViewFilterQuery expMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMestMaterialFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMestMaterialFilter.IS_EXPORT = true;
                    expMestMaterials = new HisExpMestMaterialManager(paramGet).GetView(expMestMaterialFilter);
                    if (filter.DEPARTMENT_ID != null) expMestMaterials = expMestMaterials.Where(o => o.REQ_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                    listMobaExpMestIds = expMestMaterials.Where(o => o.TH_AMOUNT > 0).Select(o => o.EXP_MEST_ID ?? 0).ToList();
                    listExpMestIds = expMestMaterials.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                    aggrExpMestIds.AddRange(expMestMaterials.Select(o => o.AGGR_EXP_MEST_ID ?? 0).GroupBy(o => o).Select(p => p.First()).ToList());

                }
                //xuat 
                if (IsNotNullOrEmpty(listExpMestIds))
                {
                    listExpMestIds = listExpMestIds.Distinct().ToList();
                    var skip = 0;
                    List<V_HIS_EXP_MEST> listExpMest = new List<V_HIS_EXP_MEST>();
                    while (listExpMestIds.Count - skip > 0)
                    {
                        var listIDs = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery AggrExpFilter = new HisExpMestViewFilterQuery()
                        {
                            IDs = listIDs

                        };
                        var x = new HisExpMestManager(paramGet).GetView(AggrExpFilter);
                        listExpMest.AddRange(x);
                    }
                    dicExpMest = listExpMest.ToDictionary(o => o.ID);
                }
                //xuat tong hop
                if (IsNotNullOrEmpty(aggrExpMestIds))
                {
                    aggrExpMestIds = aggrExpMestIds.Distinct().ToList();
                    var skip = 0;
                    List<V_HIS_EXP_MEST> listAggrExpMest = new List<V_HIS_EXP_MEST>();
                    while (aggrExpMestIds.Count - skip > 0)
                    {
                        var listIDs = aggrExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery AggrExpFilter = new HisExpMestViewFilterQuery()
                        {
                            IDs = listIDs

                        };
                        var x = new HisExpMestManager(paramGet).GetView(AggrExpFilter);
                        listAggrExpMest.AddRange(x);
                    }
                    dicAggrExpMest = listAggrExpMest.ToDictionary(o => o.ID);
                }
                //Lấy các phiếu nhập thu hồi từ các phiếu đã xuất
                var skip1 = 0;
                listMobaExpMestIds = listMobaExpMestIds.GroupBy(o => o).Select(p => p.First()).ToList();
                while (listMobaExpMestIds.Count - skip1 > 0)
                {
                    var listIDs = listMobaExpMestIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery MobaImpMestFilter = new HisImpMestViewFilterQuery()
                    {
                        MOBA_EXP_MEST_IDs = listIDs
                    };
                    var listMobaImpMestSub = new HisImpMestManager(paramGet).GetView(MobaImpMestFilter);
                    listMobaImpMest.AddRange(listMobaImpMestSub);
                    HisImpMestMedicineViewFilterQuery impMeFilter = new HisImpMestMedicineViewFilterQuery();
                    impMeFilter.IMP_MEST_IDs = listMobaImpMestSub.Select(o => o.ID).ToList();
                    listImpMestMedicine.AddRange(new HisImpMestMedicineManager().GetView(impMeFilter)??new List<V_HIS_IMP_MEST_MEDICINE>());

                    HisImpMestMaterialViewFilterQuery impMaFilter = new HisImpMestMaterialViewFilterQuery();
                    impMaFilter.IMP_MEST_IDs = listMobaImpMestSub.Select(o => o.ID).ToList();
                    listImpMestMaterial.AddRange(new HisImpMestMaterialManager().GetView(impMaFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>());
                }

                hisDepartment = new HisDepartmentManager(paramGet).GetById(filter.DEPARTMENT_ID ?? 0);
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
            bool result = false;
            try
            {
                ListRdo.Clear();
                List<Mrs00417RDO> data = new List<Mrs00417RDO>();
                //Them du lieu thuoc vao danh sach xuat bao cao
                this.AddMedicine(expMestMedicines, ref data);
                //Them du lieu vat tu vao danh sach xuat bao cao
                this.AddMaterial(expMestMaterials, ref data);
                ListRdo = data.OrderBy(o => o.TYPE).ThenBy(p => p.TYPE_NAME).ToList();
                ListParentRdo = ListRdo.GroupBy(o => o.TYPE_CODE).Select(p => p.First()).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }
        private void AddMedicine(List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines, ref List<Mrs00417RDO> ListRdo)
        {
            if (IsNotNullOrEmpty(expMestMedicines))
            {
                expMestMedicines = expMestMedicines
                    .OrderByDescending(o => o.NUM_ORDER)
                    .ThenBy(o => o.MEDICINE_TYPE_ID)
                    .ToList();
                var medicines = expMestMedicines.Select(o => new Mrs00417RDO
                {
                    TYPE = Mrs00417RDO.MEDICINE,
                    EXP_AMOUNT = o.AMOUNT,
                    MOBA_AMOUNT = listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).ToList().Count > 0 ? listImpMestMedicine.Where(p => listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).Select(q => q.ID).ToList().Contains(p.IMP_MEST_ID) && p.MEDICINE_ID == o.MEDICINE_ID).Sum(s => s.AMOUNT) : 0,
                    MANUFACTURER_NAME = o.MANUFACTURER_NAME,
                    BID_NUMBER = o.BID_NUMBER,
                    EXPIRED_DATE_STR = o.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXPIRED_DATE.Value) : null,
                    EXP_PRICE = o.PRICE ?? o.IMP_PRICE,
                    IMP_PRICE = o.IMP_PRICE,
                    EXP_TIME_STR = o.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXP_TIME.Value) : null,
                    INTERNAL_PRICE = o.INTERNAL_PRICE ?? 0,
                    PACKAGE_NUMBER = o.PACKAGE_NUMBER,
                    SERVICE_UNIT_NAME = o.SERVICE_UNIT_NAME,
                    SUPPLIER_CODE = o.SUPPLIER_CODE,
                    SUPPLIER_NAME = o.SUPPLIER_NAME,
                    TYPE_NAME = o.MEDICINE_TYPE_NAME,
                    TYPE_CODE = o.MEDICINE_TYPE_CODE,
                    EXP_MEST_CODE = dicExpMest.ContainsKey(o.EXP_MEST_ID??0) ?(dicExpMest[o.EXP_MEST_ID??0].TDL_AGGR_EXP_MEST_CODE?? o.EXP_MEST_CODE):null,
                    EXP_MEST_TYPE_NAME = dicAggrExpMest.ContainsKey(o.AGGR_EXP_MEST_ID ?? 0) ? dicAggrExpMest[o.AGGR_EXP_MEST_ID ?? 0].EXP_MEST_TYPE_NAME : (dicExpMest.ContainsKey(o.EXP_MEST_ID ?? 0) ? dicExpMest[o.EXP_MEST_ID ?? 0].EXP_MEST_TYPE_NAME : null),
                    VAT_RATIO = o.IMP_VAT_RATIO
                }).ToList();

                if (ListRdo == null)
                {
                    ListRdo = new List<Mrs00417RDO>();
                }
                ListRdo.AddRange(medicines);
            }
        }

        private void AddMaterial(List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials, ref List<Mrs00417RDO> ListRdo)
        {
            if (IsNotNullOrEmpty(expMestMaterials))
            {
                expMestMaterials = expMestMaterials
                    .OrderByDescending(o => o.NUM_ORDER)
                    .ThenBy(o => o.MATERIAL_TYPE_ID).ToList();
                var materials = expMestMaterials.Select(o => new Mrs00417RDO
                {
                    TYPE = Mrs00417RDO.MATERIAL,
                    EXP_AMOUNT = o.AMOUNT,
                    MOBA_AMOUNT = listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).ToList().Count > 0 ? listImpMestMaterial.Where(p => listMobaImpMest.Where(r => r.MOBA_EXP_MEST_ID == o.EXP_MEST_ID).Select(q => q.ID).ToList().Contains(p.IMP_MEST_ID) && p.MATERIAL_ID == o.MATERIAL_ID).Sum(s => s.AMOUNT) : 0,
                    MANUFACTURER_NAME = o.MANUFACTURER_NAME,
                    BID_NUMBER = o.BID_NUMBER,
                    EXPIRED_DATE_STR = o.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXPIRED_DATE.Value) : null,
                    EXP_PRICE = o.PRICE ?? 0,
                    IMP_PRICE = o.IMP_PRICE,
                    EXP_TIME_STR = o.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.EXP_TIME.Value) : null,
                    INTERNAL_PRICE = o.INTERNAL_PRICE ?? 0,
                    PACKAGE_NUMBER = o.PACKAGE_NUMBER,
                    SERVICE_UNIT_NAME = o.SERVICE_UNIT_NAME,
                    SUPPLIER_CODE = o.SUPPLIER_CODE,
                    SUPPLIER_NAME = o.SUPPLIER_NAME,
                    TYPE_NAME = o.MATERIAL_TYPE_NAME,
                    TYPE_CODE = o.MATERIAL_TYPE_CODE,
                    EXP_MEST_CODE = dicExpMest.ContainsKey(o.EXP_MEST_ID ?? 0) ? (dicExpMest[o.EXP_MEST_ID ?? 0].TDL_AGGR_EXP_MEST_CODE ?? o.EXP_MEST_CODE) : null,
                    EXP_MEST_TYPE_NAME = dicAggrExpMest.ContainsKey(o.AGGR_EXP_MEST_ID ?? 0) ? dicAggrExpMest[o.AGGR_EXP_MEST_ID ?? 0].EXP_MEST_TYPE_NAME : (dicExpMest.ContainsKey(o.EXP_MEST_ID ?? 0) ? dicExpMest[o.EXP_MEST_ID ?? 0].EXP_MEST_TYPE_NAME : null),
                    VAT_RATIO = o.IMP_VAT_RATIO
                }).ToList();

                if (ListRdo == null)
                {
                    ListRdo = new List<Mrs00417RDO>();
                }
                ListRdo.AddRange(materials);
            }
        }

        //private string ExpMestCode(long? aggrExpMestId)
        //{
        //    string result = null;
        //    try
        //    {
        //        if (aggrExpMestId != null)
        //            if (hisExpMest.Where(o => o.ID == aggrExpMestId).ToList().Count > 0)
        //            {
        //                result = hisExpMest.Where(o => o.ID == aggrExpMestId).First().EXP_MEST_CODE;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //        LogSystem.Error(ex);
        //    }
        //    return result;
        //}
        //private string ExpMestTypeName(long expMestTypeId, long? aggrExpMestId)
        //{
        //    string result = "";
        //    try
        //    {
        //        if (expMestTypeId != null)
        //        {
        //            if (listExpMestType == null)
        //            {
        //                HisExpMestTypeFilterQuery filter = new HisExpMestTypeFilterQuery();
        //                listExpMestType = new HisExpMestTypeManager().Get(filter)?? new List<HIS_EXP_MEST_TYPE>();
        //            }

        //            if (listExpMestType.Count>0)
        //            {
        //                result = (listExpMestType.FirstOrDefault(o=>o.ID == expMestTypeId)?? new HIS_EXP_MEST_TYPE()).EXP_MEST_TYPE_NAME;
        //            }
        //            if (aggrExpMestId != null)
        //                if (hisExpMest.Where(o => o.ID == aggrExpMestId).ToList().Count > 0)
        //                {
        //                    filter.ID = hisExpMest.Where(o => o.ID == aggrExpMestId).First().EXP_MEST_TYPE_ID;
        //                    data = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager().Get(filter).FirstOrDefault();
        //                    if (data != null)
        //                    {
        //                        result = data.EXP_MEST_TYPE_NAME;
        //                    }
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //        LogSystem.Error(ex);
        //    }
        //    return result;
        //}

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            if (((Mrs00417Filter)this.reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00417Filter)this.reportFilter).TIME_FROM));
            }
            if (((Mrs00417Filter)this.reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00417Filter)this.reportFilter).TIME_TO));
            }
            if (hisDepartment != null) dicSingleTag.Add("DEPARTMENT_NAME", hisDepartment.DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Parent", ListParentRdo);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddRelationship(store, "Parent", "Report", "TYPE_CODE", "TYPE_CODE");

        }

    }
}
