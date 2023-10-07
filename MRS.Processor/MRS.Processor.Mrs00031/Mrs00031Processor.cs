using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00031
{
    public class Mrs00031Processor : AbstractProcessor
    {
        Mrs00031Filter castFilter = null;
        List<Mrs00031RDO> ListRdo = new List<Mrs00031RDO>();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        CommonParam paramGet = new CommonParam();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();

        public Mrs00031Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00031Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00031: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                castFilter = ((Mrs00031Filter)this.reportFilter);
                LoadDataToRam();
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
                ProcessListCurrentSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentSereServ()
        {
            try
            {
                if (listHisSereServ != null && listHisSereServ.Count > 0)
                {
                    ListRdo = (from r in listHisSereServ select new Mrs00031RDO(r, listHisServiceReq)).ToList();

                    var Groups = ListRdo.OrderBy(o => o.EXECUTE_LOGINNAME).ToList().GroupBy(g => g.EXECUTE_LOGINNAME).ToList();
                    ListRdo.Clear();
                    foreach (var group in Groups)
                    {
                        List<Mrs00031RDO> listSub = group.ToList<Mrs00031RDO>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00031RDO rdo = new Mrs00031RDO();
                            rdo.EXECUTE_LOGINNAME = listSub[0].EXECUTE_LOGINNAME;
                            rdo.EXECUTE_USERNAME = listSub[0].EXECUTE_USERNAME;

                            rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                            rdo.VIR_TOTAL_HEIN_PRICE = listSub.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0);
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                ListTransaction = new HisTransactionManager().Get(filter);
                //DV - thanh toan
                var listTransactionId = ListTransaction.Select(s => s.ID).ToList();

                if (IsNotNullOrEmpty(listTransactionId))
                {
                    var skip = 0;
                    while (listTransactionId.Count - skip > 0)
                    {
                        var listIDs = listTransactionId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        
                        var listSereServSub = new HisSereServManager(paramGet).GetHisSereServByTransactionIds(listIDs);

                        if (IsNotNullOrEmpty(listSereServSub))
                            listHisSereServ.AddRange(listSereServSub);
                    }
                }
                var listTreatmentId = listHisSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(listTreatmentId))
                    listHisServiceReq = GetServiceReq(listTreatmentId);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<long> listTreatmentId)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                    HisServiceReqfilter.TREATMENT_IDs = listIDs;
                    var listServiceReqSub = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);
                    if (IsNotNullOrEmpty(listServiceReqSub))
                        result.AddRange(listServiceReqSub);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_SERVICE_REQ>();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
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
