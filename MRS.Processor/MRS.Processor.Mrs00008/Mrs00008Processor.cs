using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00008
{
    public class Mrs00008Processor : AbstractProcessor
    {
        Mrs00008Filter castFilter = null;
        List<Mrs00008RDO> listRdo = new List<Mrs00008RDO>();
        HIS_MEDI_STOCK HisMediStock;
        List<V_HIS_EXP_MEST_MEDICINE> listTemp;

        public Mrs00008Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00008Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00008Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
                this.HisMediStock = new HisMediStockManager(getParam).GetById(castFilter.MEDI_STOCK_ID.Value);
                if (this.HisMediStock == null) throw new ArgumentNullException("Khong tim thay phieu xuat theo MEDI_STOCK_ID=" + castFilter.MEDI_STOCK_ID);

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                expMestMedicineViewFilter.CREATE_TIME_FROM = castFilter.CREATE_TIME_FROM;
                expMestMedicineViewFilter.CREATE_TIME_TO = castFilter.CREATE_TIME_TO;
                expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMedicineViewFilter.EXP_MEST_TYPE_IDs = new List<long> {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT };
                expMestMedicineViewFilter.IS_EXPORT = true;
                listTemp = new ManagerSql().GetView(expMestMedicineViewFilter);

                if (getParam.HasException)
                {
                    result = false;
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
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listTemp))
                {
                    listRdo = (from r in listTemp select new Mrs00008RDO(r)).ToList();
                }
                result = true;
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
                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.CREATE_TIME_FROM ?? 0));
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.CREATE_TIME_TO ?? 0));
                dicSingleTag.Add("MEDI_STOCK_NAME", this.HisMediStock.MEDI_STOCK_NAME);

                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
