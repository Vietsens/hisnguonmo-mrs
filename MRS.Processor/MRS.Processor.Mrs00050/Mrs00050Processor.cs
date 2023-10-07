using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00050
{
    public class Mrs00050Processor : AbstractProcessor
    {
        Mrs00050Filter castFilter = null;
        List<Mrs00050RDO> ListRdo = new List<Mrs00050RDO>();

        List<Mrs00050RDO> ListCurrentTransaction = new List<Mrs00050RDO>();

        public Mrs00050Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00050Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00050Filter)this.reportFilter);
                LoadDataToRam();
                result = true;
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
                AddInfo(ref ListCurrentTransaction);
                ProcessListCurrentTransaction();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddInfo(ref List<Mrs00050RDO> trans)
        {
            foreach (var item in trans)
            {
                item.CANCEL_DATE = (item.CANCEL_TIME ?? 0) - (item.CANCEL_TIME ?? 0) % 1000000;
            }
        }

        private void ProcessListCurrentTransaction()
        {
            try
            {
                if (ListCurrentTransaction != null && ListCurrentTransaction.Count > 0)
                {
                    if (ListCurrentTransaction.Count > 0)
                    {
                        var Groups = ListCurrentTransaction.GroupBy(g => g.CANCEL_LOGINNAME).ToList();
                        foreach (var group in Groups)
                        {
                            List<Mrs00050RDO> listSub = group.ToList<Mrs00050RDO>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                Mrs00050RDO rdo = new Mrs00050RDO();
                                rdo.CANCEL_LOGINNAME = listSub[0].CANCEL_LOGINNAME;
                                rdo.CANCEL_USERNAME = listSub[0].CANCEL_USERNAME;
                                foreach (var tran in listSub)
                                {
                                    if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                                    {
                                        rdo.TOTAL_BILL_AMOUNT += tran.AMOUNT;
                                        rdo.TOTAL_EXEMPTION += tran.EXEMPTION ?? 0;
                                    }
                                    else if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                                    {
                                        rdo.TOTAL_DEPOSIT_AMOUNT += tran.AMOUNT;
                                    }
                                    else if (tran.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                                    {
                                        rdo.TOTAL_REPAY_AMOUNT += tran.AMOUNT;
                                    }
                                }
                                ListRdo.Add(rdo);
                            }
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
                ListCurrentTransaction = new ManagerSql().GetTransaction(castFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentTransaction.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("MODIFY_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("MODIFY_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);

                objectTag.AddObjectData(store, "ReportDetail", ListCurrentTransaction);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
