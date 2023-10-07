using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00806
{
    public class Mrs00806Processor : AbstractProcessor

    {

        Mrs00806Filter filter;
        List<Mrs00806RDO> ListRdo = new List<Mrs00806RDO>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> ListExpMedicineMest = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_EXP_MEST_STT> ListExpMestStt = new List<HIS_EXP_MEST_STT>();
        public Mrs00806Processor(CommonParam param,string report):base(param,report)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00806Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                filter = ((Mrs00806Filter)this.reportFilter);
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK;
                expMestFilter.TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM;
                expMestFilter.TDL_INTRUCTION_TIME_TO = filter.TIME_TO;
                expMestFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                var listExpMestStt = ListExpMestStt.Where(x => x.ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE).Select(a => a.ID).ToList();
                expMestFilter.EXP_MEST_STT_IDs = listExpMestStt;
                ListExpMest = new HisExpMestManager(param).Get(expMestFilter);
                var expMestIds = ListExpMest.Select(x => x.ID).ToList();
                HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineFilter.EXP_MEST_IDs = expMestIds;
                ListExpMedicineMest = new HisExpMestMedicineManager(param).GetView(expMestMedicineFilter);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                var expMestIds = ListExpMest.Select(x => x.ID).ToList();
                var expMestMedi = ListExpMedicineMest.Where(x => expMestIds.Contains(x.EXP_MEST_ID ?? 0)).ToList();
                var group1 = expMestMedi.GroupBy(x => new { x.MEDICINE_TYPE_NAME, x.PRICE }).ToList();
                foreach (var item in group1)
                {
                    Mrs00806RDO rdo = new Mrs00806RDO();
                    rdo.EXP_MEST_PRICE=item.FirstOrDefault().PRICE??0;
                    rdo.EXP_MEST_AMOUNT = item.Sum(x => x.AMOUNT);
                    rdo.MEDICINE_NAME = item.FirstOrDefault().ACTIVE_INGR_BHYT_NAME;
                    rdo.EXP_MEST_MEDICINE_ID = item.FirstOrDefault().MEDICINE_ID??0;
                    rdo.SERVICE_UNIT_NAME = item.FirstOrDefault().SERVICE_UNIT_NAME;
                    rdo.TOTAL_PRICE = rdo.EXP_MEST_PRICE * rdo.EXP_MEST_AMOUNT;
                    rdo.MEDICINE_GROUP_NAME = item.FirstOrDefault().MEDICINE_GROUP_NAME;
                    rdo.MEDICINE_GROUP_CODE = item.FirstOrDefault().MEDICINE_GROUP_CODE;
                    var treatment = ListTreatment.Where(x => x.ID == item.FirstOrDefault().TDL_TREATMENT_ID).FirstOrDefault();
                    if (treatment!= null)
                    {
                        rdo.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME??0;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                    }
                    rdo.EXP_MEST_CODE = item.FirstOrDefault().EXP_MEST_CODE;
                    
                    ListRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex.Message);
            }
            return result;
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store,"Report", ListRdo.OrderBy(x=>x.EXP_MEST_CODE).ToList());
        }
    }
}
