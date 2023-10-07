using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00741
{
    class Mrs00741Processor : AbstractProcessor
    {
        List<Mrs00741RDO> listRdo = new List<Mrs00741RDO>();
        List<Mrs00741RDO> listData = new List<Mrs00741RDO>();
        Mrs00741Filter filter = null;
        public Mrs00741Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00741Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00741Filter)this.reportFilter;
                listData = new ManagerSql().GetData(filter) ?? new List<Mrs00741RDO>();
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
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
                        Mrs00741RDO rdo = new Mrs00741RDO();
                        rdo.PATIENT_CLASSIFY_CODE = item.PATIENT_CLASSIFY_CODE;
                        rdo.PATIENT_CLASSIFY_NAME = item.PATIENT_CLASSIFY_NAME;
                        rdo.PATIENT_TYPE_CODE = item.PATIENT_TYPE_CODE;
                        rdo.PATIENT_TYPE_NAME = item.PATIENT_TYPE_NAME;
                        rdo.PRICE = item.PRICE;
                        rdo.REAL_PRICE = item.REAL_PRICE == null ? item.PRICE : item.REAL_PRICE;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.TOTAL_BASIC_PRICE = item.PRICE * item.AMOUNT;
                        rdo.TOTAL_REAL_PRICE = item.REAL_PRICE == null ? item.PRICE * item.AMOUNT : item.REAL_PRICE * item.AMOUNT;
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                LogSystem.Info("listRdo: " + listRdo.Count);
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }
    }
}
