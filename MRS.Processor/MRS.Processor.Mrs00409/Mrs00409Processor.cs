using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServBill;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisHeinApproval; 

namespace MRS.Processor.Mrs00409
{
    class Mrs00409Processor : AbstractProcessor
    {
        Mrs00409Filter castFilter = null; 
        List<Mrs00409RDO> listRdo = new List<Mrs00409RDO>(); 

        public Mrs00409Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_HEIN_APPROVAL> ListHeinApprovals = new List<V_HIS_HEIN_APPROVAL>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV_2> listSereServs2 = new List<V_HIS_SERE_SERV_2>(); 
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00409Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00409Filter)this.reportFilter; 

                var skip = 0; 

                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery(); 
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00409"; 
                var listServiceRetCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(retyCastFilter); 

                var listServiceIds = listServiceRetCats.Select(s => s.SERVICE_ID).ToList(); 

                HisHeinApprovalViewFilterQuery heinApprovalBhytFilter = new HisHeinApprovalViewFilterQuery(); 
                heinApprovalBhytFilter.EXECUTE_TIME_FROM = this.castFilter.TIME_FROM; 
                heinApprovalBhytFilter.EXECUTE_TIME_TO = this.castFilter.TIME_TO; 
                //heinApprovalBhytFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                ListHeinApprovals = new HisHeinApprovalManager(paramGet).GetView(heinApprovalBhytFilter); 

                var ListHeinApprovalIds = ListHeinApprovals.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (ListHeinApprovalIds.Count - skip > 0)
                {
                    var ListHeinApprovalId = ListHeinApprovalIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //V_HIS_SERE_SERV_2 theo Hein_approval_bhyt
                    HisSereServView2FilterQuery sereServFilter2 = new HisSereServView2FilterQuery(); 
                    sereServFilter2.HEIN_APPROVAL_IDs = ListHeinApprovalId;
                    var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView2(sereServFilter2);
                    if (listSereServ != null)
                    {
                        listSereServ = listSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                    }
                    listSereServs2.AddRange(listSereServ); 
                }
                var listSereServIds = listSereServs2.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listSereServIds.Count - skip > 0)
                {
                    var listSereServId = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //V_HIS_SERE_SERV theo sere_serv_2 + listServiceids
                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                    sereServFilter.IDs = listSereServId; 
                    sereServFilter.SERVICE_IDs = listServiceIds; 
                    var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter); 
                    listSereServs.AddRange(listSereServ); 
                    
                }
                listSereServIds.Clear(); 
                listSereServIds = listSereServs.Select(s => s.ID).ToList(); 

                skip = 0; 
                while (listSereServIds.Count - skip > 0)
                {
                    var listSereServId = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery(); 
                    sereServBillFilter.SERE_SERV_IDs = listSereServId; 
                    var listSereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(sereServBillFilter); 

                    listSereServBills.AddRange(listSereServBill); 

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
                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        var heinApprovalBhyt = ListHeinApprovals.Where(w => w.ID == sereServ.HEIN_APPROVAL_ID).FirstOrDefault(); 
                        var sereServBills = listSereServBills.Where(s => s.SERE_SERV_ID == sereServ.ID).ToList(); 
                        foreach (var sereServBill in sereServBills)
                        {
                            Mrs00409RDO rdo = new Mrs00409RDO(); 
                            rdo.CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServBill.CREATE_TIME ?? 0); 
                            rdo.APPROVE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(heinApprovalBhyt.EXECUTE_TIME ?? 0); 
                            rdo.PATIENT_CODE = heinApprovalBhyt.TDL_PATIENT_CODE; 
                            rdo.PATIENT_NAME = heinApprovalBhyt.TDL_PATIENT_NAME; 
                            rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER.Substring(0, 3); 
                            if (sereServBill.TDL_BILL_TYPE_ID == MANAGER.Config.HisBillTypeCFG.bill_type_id_01 || sereServBill.TDL_BILL_TYPE_ID == null)
                            {
                                rdo.BILL_NUMBER = sereServBill.BILL_ID; 
                            }
                            if (sereServBill.TDL_BILL_TYPE_ID == MANAGER.Config.HisBillTypeCFG.bill_type_id_02)
                            {
                                rdo.INVOICE_NUMBER = sereServBill.BILL_ID; 
                            }
                            rdo.AMOUNT = sereServ.AMOUNT; 
                            rdo.PRICE = sereServ.PRICE; 
                            rdo.TOTAL_MONEY = sereServ.VIR_TOTAL_PRICE; 
                            rdo.HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE; 
                            rdo.DEPARTMENT = sereServ.REQUEST_ROOM_NAME; 
                            rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 

                            listRdo.Add(rdo); 

                        }

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



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
