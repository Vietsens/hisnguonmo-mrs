using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisTransaction; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServSegr;
using MOS.MANAGER.HisServiceGroup;
using MOS.MANAGER.HisServiceRetyCat; 

namespace MRS.Processor.Mrs00600
{
    public class Mrs00600Processor : AbstractProcessor
    {
        Mrs00600Filter filter = new Mrs00600Filter(); 
        CommonParam paramGet = new CommonParam();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<Mrs00600RDO> ListRdo = new List<Mrs00600RDO>(); 
        HIS_BRANCH Branch = new HIS_BRANCH(); 
        public Mrs00600Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00600Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                filter = (Mrs00600Filter)this.reportFilter; 
                //get dữ liệu:
              
                HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = filter.TRANSACTION_TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = filter.TRANSACTION_TIME_TO;
                tranFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                tranFilter.IS_CANCEL = false;
                tranFilter.HAS_SALL_TYPE = false;
                ListTransaction = new HisTransactionManager(paramGet).Get(tranFilter);
                var treatmentIds = ListTransaction.Where(p=>p.TREATMENT_ID.HasValue).Select(o => o.TREATMENT_ID.Value).Distinct().ToList();
                if (treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var lists = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                        HisTreatmentfilter.IDs = lists;
                        var listHisTreatmentSub = new HisTreatmentManager(paramGet).Get(HisTreatmentfilter);
                        if (listHisTreatmentSub != null)
                        {
                            listHisTreatment.AddRange(listHisTreatmentSub);
                        }
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = lists;
                        HisSereServfilter.HAS_EXECUTE = true;
                        var listHisSereServSub = new HisSereServManager(paramGet).Get(HisSereServfilter);
                        if (listHisSereServSub != null)
                        {
                            listHisSereServ.AddRange(listHisSereServSub);
                        }
                    }
                }
                 HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00600";
                listHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);

               if(filter.CATEGORY_CODE__KSKs!=null&&listHisServiceRetyCat!=null)
               {
                        listHisTreatment = listHisTreatment.Where(o => listHisSereServ.Exists(q =>o.ID==q.TDL_TREATMENT_ID&& listHisServiceRetyCat.Exists(p=>filter.CATEGORY_CODE__KSKs.Contains(p.CATEGORY_CODE)&&p.SERVICE_ID == q.SERVICE_ID))).ToList();
               }
               

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

                if (IsNotNullOrEmpty(listHisTreatment))
                    {
                        ListRdo = (from b in listHisTreatment select new Mrs00600RDO(b, listHisSereServ, ListTransaction, listHisServiceRetyCat, this.filter)).ToList(); 
                    }
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 

                ListRdo.Clear(); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_TO)); 
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportPCB", ListRdo.Where(o=>o.IS_ROOM_PCB==true).ToList()); 

        }

     
    }
}