using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00679
{
    public class Mrs00679Processor : AbstractProcessor
    {
       Mrs00679Filter CastFilter;
        
        public Mrs00679Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00679Filter);
        }
        List<Mrs00679RDO> ListRdo = new List<Mrs00679RDO>();
        List<HIS_SERE_SERV_BILL> ListBill = new List<HIS_SERE_SERV_BILL>();
        protected override bool GetData()
        {
            var result = true;
            try
            {
                this.CastFilter = (Mrs00679Filter)this.reportFilter;
                var paramGet = new CommonParam();
                
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu filter: " +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));
               
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
            var result = true;
            try
            {
                //if (IsNotNullOrEmpty(ListRdo))
                //{
                //    foreach (var item in ListRdo)
                //    {
                //        Mrs00679RDO rdo = new Mrs00679RDO();
                //        rdo.MEDI_ORG_CODE = item.MEDI_ORG_CODE;
                //        rdo.MEDI_ORG_NAME = item.MEDI_ORG_NAME;
                //        rdo.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                //        rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                //        rdo.TDL_REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                //        rdo.TDL_SERVICE_NAME = item.TDL_SERVICE_NAME;
                //        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                //        rdo.VIR_PRICE = item.VIR_PRICE;
                //        rdo.VIR_TOTAL_PRICE = item.VIR_TOTAL_PRICE;
                //        rdo.AMOUNT_NGOAITRU = item.AMOUNT_NGOAITRU;
                //        rdo.AMOUNT_NOITRU = item.AMOUNT_NOITRU;
                //        rdo.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                //        rdo.FEE_LOCK_TIME = item.FEE_LOCK_TIME;
                //        rdo.HEIN_CARD_NUMBER = item.HEIN_CARD_NUMBER;
                //        rdo.ICD_CODE = item.ICD_CODE;
                //        rdo.ICD_NAME = item.ICD_NAME;
                //        rdo.ID = item.ID;
                //        rdo.IN_TIME = item.IN_TIME;
                //        rdo.OUT_TIME = item.OUT_TIME;
                //        rdo.PATIENT_CODE = item.PATIENT_CODE;
                //        rdo.ROOM_NAME = item.ROOM_NAME;
                //        rdo.TDL_INTRUCTION_TIME = item.TDL_INTRUCTION_TIME;
                //        rdo.SERVICE_STATUS = ListBill.Where(p => p.SERE_SERV_ID == item.ID).ToList().Count > 0 ? "X" : "";
                //        ListRdo.Add(rdo);
                //    }
                //}
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

                dicSingleTag.Add("TIME_FROM", CastFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", CastFilter.TIME_TO);

                objectTag.AddObjectData(store, "Report", new ManagerSql().GetSum(CastFilter));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
