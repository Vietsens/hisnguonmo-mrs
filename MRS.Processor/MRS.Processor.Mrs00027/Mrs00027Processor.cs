using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00027
{
    public class Mrs00027Processor : AbstractProcessor
    {
        Mrs00027Filter castFilter = null;
        List<Mrs00027RDO> ListRdo = new List<Mrs00027RDO>();

        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ = new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        CommonParam paramGet = new CommonParam();

        public Mrs00027Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00027Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00027Filter)this.reportFilter);
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
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0).ToList();
                    var Groups = ListCurrentSereServ.OrderBy(o => o.TDL_REQUEST_LOGINNAME).ToList().GroupBy(g => g.TDL_REQUEST_LOGINNAME).ToList();
                    foreach (var group in Groups)
                    {
                        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> listSub = group.ToList<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00027RDO dataRDO = new Mrs00027RDO();
                            dataRDO.REQUEST_LOGINNAME = listSub[0].TDL_REQUEST_LOGINNAME;
                            dataRDO.REQUEST_USERNAME = listSub[0].TDL_REQUEST_USERNAME;
                            foreach (var sereServ in listSub)
                            {
                                //if (sereServ.BILL_ID != null)
                                //{
                                dataRDO.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                dataRDO.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                //}
                            }
                            ListRdo.Add(dataRDO);
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
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filter.HAS_EXECUTE = true;
                filter.IS_EXPEND = false;
                ListCurrentSereServ = new HisSereServManager().Get(filter);
                //DV - thanh toan
                var listTreatmentId = ListCurrentSereServ.Where(o=>o.TDL_TREATMENT_ID.HasValue).Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                        filterSereServBill.TDL_TREATMENT_IDs = listIDs;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                        if (IsNotNullOrEmpty(listSereServBillSub))
                        {
                            listSereServBillSub = listSereServBillSub.Where(o => ListCurrentSereServ.Exists(p => p.ID == o.SERE_SERV_ID)).ToList();
                            ListSereServBill.AddRange(listSereServBillSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentSereServ.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM??0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO??0));
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
