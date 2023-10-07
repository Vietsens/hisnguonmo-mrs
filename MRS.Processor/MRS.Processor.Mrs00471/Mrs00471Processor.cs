using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMestType;

namespace MRS.Processor.Mrs00471
{
    class Mrs00471Processor : AbstractProcessor
    {
        Mrs00471Filter castFilter = null;
        List<Mrs00471RDO> listRdo = new List<Mrs00471RDO>();
        List<Mrs00471RDO> listRdoGroup = new List<Mrs00471RDO>();
        List<HIS_EXP_MEST> listExpMests = new List<HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<HIS_IMP_MEST_MEDICINE>();
        List<HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<HIS_IMP_MEST_MATERIAL>();

        string thisReportTypeCode = "";
        public Mrs00471Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00471Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                if (castFilter.MEDI_STOCK_IDs != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAMEs", string.Join(" - ", (HisMediStockCFG.HisMediStocks ?? new List<V_HIS_MEDI_STOCK>()).Where(p => castFilter.MEDI_STOCK_IDs.Contains(p.ID)).Select(o => o.MEDI_STOCK_NAME).ToList()));
                }

                if (castFilter.EXP_MEST_TYPE_IDs != null)
                {
                    dicSingleTag.Add("EXP_MEST_TYPE_NAMEs", string.Join(" - ", (new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery()) ?? new List<HIS_EXP_MEST_TYPE>()).Where(p => castFilter.EXP_MEST_TYPE_IDs.Contains(p.ID)).Select(o => o.EXP_MEST_TYPE_NAME).ToList()));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "listRdoGroup", listRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroup", "Report", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00471Filter)this.reportFilter;

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMediFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMediFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMediFilter.IS_EXPORT = true;
                listExpMestMedicines = new HisExpMestMedicineManager().GetView(expMediFilter);

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMateFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMateFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMateFilter.IS_EXPORT = true;
                listExpMestMaterials = new HisExpMestMaterialManager().GetView(expMateFilter);

                List<long> expMestIds = new List<long>();
                if (IsNotNullOrEmpty(listExpMestMedicines))
                    expMestIds.AddRange(listExpMestMedicines.Select(s => s.EXP_MEST_ID ?? 0).ToList());

                if (IsNotNullOrEmpty(listExpMestMaterials))
                    expMestIds.AddRange(listExpMestMaterials.Select(s => s.EXP_MEST_ID ?? 0).ToList());

                expMestIds = expMestIds.Distinct().ToList();

                if (IsNotNullOrEmpty(expMestIds))
                {
                    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                    expMestFilter.IDs = expMestIds;

                    var expMest = new ManagerSql().Get(expMestFilter);
                    if (IsNotNullOrEmpty(expMest))
                    {
                        listExpMests.AddRange(expMest);
                    }

                    var impMestMedi = new ManagerSql().GetImpMedi(expMestIds);
                    if (IsNotNullOrEmpty(impMestMedi))
                    {
                        listImpMestMedicines.AddRange(impMestMedi);
                    }
                    // vat tu
                    var impMestMate = new ManagerSql().GetImpMate(expMestIds);
                    if (IsNotNullOrEmpty(impMestMate))
                    {
                        listImpMestMaterials.AddRange(impMestMate);
                    }
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
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                var listExpMestIds = listExpMests.Select(s => s.ID).ToList();

                //thuoc
                var listExpMestMedicine = listExpMestMedicines.Where(s => listExpMestIds.Contains(s.EXP_MEST_ID ?? 0)).ToList();
                foreach (var expMestMedicines in listExpMestMedicine)
                {
                    Mrs00471RDO rdo = new Mrs00471RDO();
                    rdo.GROUP_ID = 1;
                    rdo.GROUP_NAME = "Thuốc";
                    rdo.SERVICE_ID = expMestMedicines.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = expMestMedicines.MEDICINE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = expMestMedicines.SERVICE_UNIT_NAME;
                    rdo.PRICE_BEFORE_VAT = expMestMedicines.PRICE;
                    rdo.VAT = expMestMedicines.VAT_RATIO;

                    var listImpMestMedicine = listImpMestMedicines.Where(o=>o.TH_EXP_MEST_MEDICINE_ID==expMestMedicines.ID).ToList();

                    decimal impAmount = 0;
                    if (IsNotNullOrEmpty(listImpMestMedicine))
                    {
                        foreach (var impMestMedicines in listImpMestMedicine)
                        {
                            impAmount += impMestMedicines.AMOUNT;
                        }
                    }
                    rdo.IMP_AMOUNT = impAmount;
                    rdo.EXP_AMOUNT = expMestMedicines.AMOUNT;
                    rdo.AMOUNT = expMestMedicines.AMOUNT - impAmount; ;

                    if (rdo.PRICE_BEFORE_VAT != 0 || rdo.PRICE_BEFORE_VAT != null)
                    {
                        rdo.PRICE_AFTER_VAT = expMestMedicines.PRICE + (expMestMedicines.PRICE * expMestMedicines.VAT_RATIO);
                    }

                    else
                        rdo.PRICE_AFTER_VAT = expMestMedicines.PRICE;

                    listRdo.Add(rdo);
                }

                // vat tu
                var listExpMestMaterial = listExpMestMaterials.Where(s => listExpMestIds.Contains(s.EXP_MEST_ID ?? 0)).ToList();
                foreach (var expMestMaterials in listExpMestMaterial)
                {
                    Mrs00471RDO rdo = new Mrs00471RDO();
                    rdo.GROUP_ID = 2;
                    rdo.GROUP_NAME = "Vật tư";
                    rdo.SERVICE_ID = expMestMaterials.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = expMestMaterials.MATERIAL_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = expMestMaterials.SERVICE_UNIT_NAME;
                    rdo.PRICE_BEFORE_VAT = expMestMaterials.PRICE;
                    rdo.VAT = expMestMaterials.VAT_RATIO;

                    var listImpMestMaterial = listImpMestMaterials.Where(o => o.TH_EXP_MEST_MATERIAL_ID == expMestMaterials.ID).ToList();

                    decimal impAmount = 0;
                    if (IsNotNullOrEmpty(listImpMestMaterial))
                    {
                        foreach (var impMestMaterials in listImpMestMaterial)
                        {
                            impAmount += impMestMaterials.AMOUNT;
                        }
                    }
                    rdo.IMP_AMOUNT = impAmount;
                    rdo.EXP_AMOUNT = expMestMaterials.AMOUNT;
                    rdo.AMOUNT = expMestMaterials.AMOUNT - impAmount;

                    if (rdo.PRICE_BEFORE_VAT != 0 || rdo.PRICE_BEFORE_VAT != null)
                    {
                        rdo.PRICE_AFTER_VAT = expMestMaterials.PRICE + (expMestMaterials.PRICE * expMestMaterials.VAT_RATIO);
                    }

                    else
                        rdo.PRICE_AFTER_VAT = expMestMaterials.PRICE;

                    listRdo.Add(rdo);
                }
                listRdo = listRdo.GroupBy(s => new { s.SERVICE_ID, s.GROUP_ID, s.PRICE_BEFORE_VAT, s.VAT }).Select(s => new Mrs00471RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    AMOUNT = s.Sum(gr => gr.AMOUNT),
                    PRICE_BEFORE_VAT = s.First().PRICE_BEFORE_VAT,
                    VAT = s.First().VAT,
                    PRICE_AFTER_VAT = s.First().PRICE_AFTER_VAT,
                    TOTAL_PRICE = s.Sum(gr => gr.TOTAL_PRICE),
                }).ToList();

                listRdo = listRdo.Where(s => s.AMOUNT != 0).ToList();

                listRdoGroup = listRdo.GroupBy(s => new { s.GROUP_ID }).Select(s => new Mrs00471RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
