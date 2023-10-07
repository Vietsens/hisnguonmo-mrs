using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using ACS.EFMODEL.DataModels; 
using ACS.Filter; 
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientType; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager; 

namespace MRS.Processor.Mrs00228
{
    public class Mrs00228Processor : AbstractProcessor
    {
        private List<Mrs00228RDO> listMrs00228RDOs = new List<Mrs00228RDO>(); 
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00228RDO> ListRdo = new List<Mrs00228RDO>(); 

        List<HIS_INVOICE> listInvoice = new List<HIS_INVOICE>(); 
        List<HIS_INVOICE_DETAIL> listivDT = new List<HIS_INVOICE_DETAIL>(); 

        List<ACS_USER> listAcsUser = new List<ACS_USER>(); 

        const string EXP_LIQU = "LIQU"; //Xuất thanh lý
        private string a = ""; 
        public Mrs00228Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode; 
        }
        int i = 0; 
        public override Type FilterType()
        {
            return typeof(Mrs00228Filter); 
        }

        protected override bool GetData()
        {
            var result = false; 
            try
            {
             var paramGet = new CommonParam(); 
                //get dữ liệu
                HisInvoiceFilterQuery IVfilter = new HisInvoiceFilterQuery(); 
                IVfilter.CREATE_TIME_FROM = ((Mrs00228Filter)this.reportFilter).TIME_FROM; 
                IVfilter.CREATE_TIME_TO = ((Mrs00228Filter)this.reportFilter).TIME_TO; 
                listInvoice = new HisInvoiceManager(paramGet).Get(IVfilter); 
                
                var listInvoiceIds = listInvoice.Select(s => s.ID).ToList(); 

                
                var skip = 0; 
                while (listInvoiceIds.Count - skip > 0)
                {
                    var listIds = listInvoiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var IVDTfilter = new HisInvoiceDetailFilterQuery
                    {
                        INVOICE_IDs = listIds
                    }; 
                    var ivDT = new HisInvoiceDetailManager(paramGet).Get(IVDTfilter); 
                    listivDT.AddRange(ivDT); 
                }
                AcsUserFilterQuery AUfilter = new AcsUserFilterQuery(); 
                listAcsUser = new AcsUserManager(paramGet).Get<List<ACS_USER>>(AUfilter); 
                ProcessData(); 
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
                if(listInvoice!=null)
                foreach (var invoice in listInvoice)
                { 
                Mrs00228RDO rdo = new Mrs00228RDO()
                {
                LOGINNAME = invoice.CREATOR,
                USERNAME = listAcsUser.Where(o=>o.LOGINNAME==invoice.CREATOR).Select(p=>p.USERNAME).First(),
                TOTAL_MONEY = listivDT != null && invoice.IS_CANCEL == null ? (decimal)listivDT.Where(o => o.INVOICE_ID == invoice.ID).Select(p => p.VIR_TOTAL_PRICE).Sum() : 0
                }; 
                ListRdo.Add(rdo); 
                }
                var groupByLoginNames = ListRdo.GroupBy(s => s.LOGINNAME).ToList(); 
                ListRdo.Clear(); 
                foreach (var group in groupByLoginNames)
                {
                    List<Mrs00228RDO> listRdoSub = group.ToList<Mrs00228RDO>(); 
                    Mrs00228RDO rdo = new Mrs00228RDO()
                    {
                        LOGINNAME = listRdoSub.First().LOGINNAME,
                        USERNAME = listRdoSub.First().USERNAME,
                        TOTAL_MONEY = listRdoSub.Sum(s => s.TOTAL_MONEY)
                    }; 
                    ListRdo.Add(rdo); 
                }

                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 

            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00228Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00228Filter)this.reportFilter).TIME_TO)); 
            
             objectTag.AddObjectData(store, "Report", ListRdo); 
           
        }
    }
}