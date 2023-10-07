using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00628
{
    class Mrs00628Processor : AbstractProcessor
    {
        Mrs00628Filter castFilter = null;
        List<Mrs00628RDO> ListRdo;
        List<Mrs00628RDO> ListRdoParent;

        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> ListExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();
        List<HIS_IMP_MEST> listMobaImpMests = new List<HIS_IMP_MEST>();
        List<HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<HIS_IMP_MEST_MEDICINE>();
        List<HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<HIS_IMP_MEST_MATERIAL>();

        public Mrs00628Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00628Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00628Filter)this.reportFilter;

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMediFilter.IS_EXPORT = true;
                ListExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMateFilter.IS_EXPORT = true;
                ListExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);

                List<long> expMestIds = new List<long>();
                if (IsNotNullOrEmpty(ListExpMestMedicine))
                    expMestIds.AddRange(ListExpMestMedicine.Select(s => s.EXP_MEST_ID ?? 0).ToList());

                if (IsNotNullOrEmpty(ListExpMestMaterial))
                    expMestIds.AddRange(ListExpMestMaterial.Select(s => s.EXP_MEST_ID ?? 0).ToList());

                expMestIds = expMestIds.Distinct().ToList();
                
                if (IsNotNullOrEmpty(expMestIds))
                {
                    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                    expMestFilter.IDs = expMestIds;

                    var expMest = new ManagerSql().Get(expMestFilter);
                    if (IsNotNullOrEmpty(expMest))
                    {
                        ListExpMest.AddRange(expMest);
                    }

                    var impMestMedi = new ManagerSql().GetImpMedi(expMestIds);
                    if (IsNotNullOrEmpty(impMestMedi))
                    {
                        ListImpMestMedicine.AddRange(impMestMedi);
                    }
                    // vat tu
                    var impMestMate = new ManagerSql().GetImpMate(expMestIds);
                    if (IsNotNullOrEmpty(impMestMate))
                    {
                        ListImpMestMaterial.AddRange(impMestMate);
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
                ListRdo = new List<Mrs00628RDO>();
                ListRdoParent = new List<Mrs00628RDO>();

                if (IsNotNullOrEmpty(ListExpMestMedicine))
                {
                    var listSub = (from r in ListExpMestMedicine select new Mrs00628RDO(r, ListExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID), ListImpMestMedicine)).ToList();
                    if (IsNotNullOrEmpty(listSub))
                        ListRdo.AddRange(listSub);
                }

                if (IsNotNullOrEmpty(ListExpMestMaterial))
                {
                    var listSub = (from r in ListExpMestMaterial select new Mrs00628RDO(r, ListExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID), ListImpMestMaterial)).ToList();
                    if (IsNotNullOrEmpty(listSub))
                        ListRdo.AddRange(listSub);
                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdoParent = ListRdo.GroupBy(o => o.EXP_MEST_CODE).Select(s => s.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportParent", ListRdoParent);
                objectTag.AddRelationship(store, "ReportParent", "Report", "EXP_MEST_CODE", "EXP_MEST_CODE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
