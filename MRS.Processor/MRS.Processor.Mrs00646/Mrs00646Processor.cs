using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00646
{
    class Mrs00646Processor : AbstractProcessor
    {
        Mrs00646Filter castFilter = null;
        List<Mrs00646RDO> ListRdo;

        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();

        public Mrs00646Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00646Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00646Filter)this.reportFilter;

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMediFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                expMediFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMediFilter.IS_EXPORT = true;
                ListExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);

                List<long> expMestIds = new List<long>();
                if (IsNotNullOrEmpty(ListExpMestMedicine))
                    expMestIds.AddRange(ListExpMestMedicine.Select(s => s.EXP_MEST_ID ?? 0).ToList());

                expMestIds = expMestIds.Distinct().ToList();

                if (IsNotNullOrEmpty(expMestIds))
                {
                    var skip = 0;
                    while (expMestIds.Count - skip > 0)
                    {
                        var listIDs = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                        expMestFilter.IDs = listIDs;

                        var expMest = new HisExpMestManager().Get(expMestFilter);
                        if (IsNotNullOrEmpty(expMest))
                        {
                            ListExpMest.AddRange(expMest);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdo = new List<Mrs00646RDO>();
                if (IsNotNullOrEmpty(ListExpMestMedicine))
                {
                    if (castFilter.IS_NEUR_ADDI.HasValue && castFilter.IS_NEUR_ADDI.Value)
                    {
                        ListExpMestMedicine = ListExpMestMedicine.Where(o => o.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN || o.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT).ToList();
                    }

                    if (IsNotNullOrEmpty(ListExpMestMedicine))
                    {
                        var listSub = (from r in ListExpMestMedicine select new Mrs00646RDO(r, ListExpMest.FirstOrDefault(o => o.ID == r.EXP_MEST_ID))).ToList();
                        if (IsNotNullOrEmpty(listSub))
                            ListRdo.AddRange(listSub);
                    }
                }

                ProcessUpdatePackingTypeName(ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessUpdatePackingTypeName(List<Mrs00646RDO> ListRdo)
        {
            try
            {
                if (ListRdo != null && ListRdo.Count > 0)
                {
                    var medicineTypeIds = ListRdo.Select(s => s.MEDICINE_TYPE_ID).Distinct().ToList();

                    List<HIS_MEDICINE_TYPE> medicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().GetByIds(medicineTypeIds);

                    foreach (var item in ListRdo)
                    {
                        var mety = medicineType.FirstOrDefault(f => f.ID == item.MEDICINE_TYPE_ID);
                        if (mety != null)
                        {
                            item.PACKING_TYPE_NAME = mety.PACKING_TYPE_NAME;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                if (castFilter.IS_NEUR_ADDI.HasValue && castFilter.IS_NEUR_ADDI.Value)
                {
                    dicSingleTag.Add("IS_NEUR_ADDI", "1");
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
