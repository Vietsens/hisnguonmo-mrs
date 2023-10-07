using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMedicineTypeAcin; 
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean; 

namespace MRS.Processor.Mrs00710
{
    public class Mrs00710Processor : AbstractProcessor
    {
        private Mrs00710Filter filter;
        List<Mrs00710RDOCountTreatment> listCountTreatment = new List<Mrs00710RDOCountTreatment>();
        List<Mrs00710RDOCountService> listCountService = new List<Mrs00710RDOCountService>();
           
        CommonParam paramGet = new CommonParam(); 
        public Mrs00710Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00710Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00710Filter)reportFilter; 
            try
            {
                listCountTreatment = new MRS.Processor.Mrs00710.ManagerSql().GetCountTreatment(filter) ?? new List<Mrs00710RDOCountTreatment>();
                listCountService = new MRS.Processor.Mrs00710.ManagerSql().GetCountService(filter) ?? new List<Mrs00710RDOCountService>();
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
            var result = true; 
            try
            {

            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            if (listCountTreatment.Count > 0)
            {
                foreach (var item in Inventec.Common.Repository.Properties.Get<Mrs00710RDOCountTreatment>())
                {
                    dicSingleTag.Add(item.Name, item.GetValue(listCountTreatment[0]));
                }
            }
            foreach (var item in HisServiceTypeCFG.HisServiceTypes)
            {
                Mrs00710RDOCountService dataServiceType = listCountService.FirstOrDefault(o=>o.TDL_SERVICE_TYPE_ID == item.ID);
                if (dataServiceType != null)
                {
                    foreach (var field in Inventec.Common.Repository.Properties.Get<Mrs00710RDOCountService>())
                    {
                        dicSingleTag.Add(field.Name + "_" + item.SERVICE_TYPE_CODE, field.GetValue(dataServiceType));
                    }
                }
            }
            dicSingleTag.Add("COUNT_SV_DIENTIM", listCountService.Sum(s => s.COUNT_SV_DIENTIM));
            dicSingleTag.Add("COUNT_SV_DIENNAO", listCountService.Sum(s => s.COUNT_SV_DIENNAO));
            dicSingleTag.Add("COUNT_SV_CTSCAN", listCountService.Sum(s => s.COUNT_SV_CTSCAN));
        }

       
    }
}
