using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
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
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisPatientTypeAlter;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDeposit; 

namespace MRS.Processor.Mrs00625
{
    class Mrs00625Processor : AbstractProcessor
    {
        Mrs00625Filter castFilter = null; 

        public Mrs00625Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<Mrs00625RDO> listRdo = new List<Mrs00625RDO>(); 
        List<HIS_SERVICE> listHisService= new List<HIS_SERVICE>();
        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();

        public override Type FilterType()
        {
            return typeof(Mrs00625Filter); 
        }

        protected override bool GetData()
        {
            bool valid = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00625Filter)this.reportFilter;


                this.listRdo = new Mrs00625RDOManager().GetRdo(this.castFilter);
                if (this.listRdo != null)
                { 
                    listRdo = listRdo.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }
       
        protected override bool ProcessData()
        {
            bool valid = true; 
            try
            {
               

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }
       
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
           
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TRANSACTION_TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TRANSACTION_TIME_TO));

                objectTag.AddObjectData(store, "Report", listRdo); 
           
        }

    }

}
