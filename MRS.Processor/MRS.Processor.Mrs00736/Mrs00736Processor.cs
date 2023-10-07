using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00736
{
    class Mrs00736Processor : AbstractProcessor
    {
        List<Mrs00736RDO> listRdo = new List<Mrs00736RDO>();
        List<Mrs00736RDO> listData = new List<Mrs00736RDO>();
        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();
        List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        Mrs00736Filter filter = null;

        public Mrs00736Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00736Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00736Filter)this.reportFilter;
                listData = new ManagerSql().GetData(filter);
                listBranch = new ManagerSql().GetBranch(filter);
                listOtherPaySource = new ManagerSql().GetOtherSource();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
            
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (listData != null)
                {
                    
                    foreach (var item in listData)
                    {
                        var otherSource = listOtherPaySource.FirstOrDefault(p => p.ID == item.OTHER_PAY_SOURCE_ID)??new HIS_OTHER_PAY_SOURCE();
                        Mrs00736RDO rdo = new Mrs00736RDO();
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.DOB_YEAR = item.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;

                        rdo.OTHER_PAY_SOURCE_CODE = otherSource.OTHER_PAY_SOURCE_CODE??" ";
                        rdo.OTHER_PAY_SOURCE_NAME = otherSource.OTHER_PAY_SOURCE_NAME ?? " ";
                        //rdo.OTHER_SOURCE_PRICE = item.OTHER_SOURCE_PRICE;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = item.TOTAL_OTHER_SOURCE_PRICE;
                        rdo.TRANSACTION_TIME = item.TRANSACTION_TIME;
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.TRANSACTION_TIME);
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;

        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
                if (listBranch != null)
                {
                    dicSingleTag.Add("BRANCH_NAME", string.Join(",", listBranch.Select(p => p.BRANCH_NAME)));
                }
                
                Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                objectTag.AddObjectData(store, "Report", listRdo);
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
