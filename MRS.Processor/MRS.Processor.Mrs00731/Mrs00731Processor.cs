using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00731
{
    class Mrs00731Processor : AbstractProcessor
    {
        public List<Mrs00731RDO> listRdo = new List<Mrs00731RDO>();
        public List<Mrs00731RDO> listData = new List<Mrs00731RDO>();
        public List<Mrs00731RDO> list2020 = new List<Mrs00731RDO>();
        public List<Mrs00731RDO> list2021 = new List<Mrs00731RDO>();
        public Mrs00731Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00731Filter); 
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                list2020 = new ManagerSql().Get6MonthsOf2020();
                listData.AddRange(list2020);
                list2021 = new ManagerSql().Get6MonthsOf2021();
                listData.AddRange(list2021);

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
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listData))
                {
                    foreach (var item in listData)
                    {
                        Mrs00731RDO rdo = new Mrs00731RDO();
                        rdo.ID = item.ID;
                        rdo.SERVICE_CODE = item.SERVICE_CODE;
                        rdo.SERVICE_NAME = item.SERVICE_NAME;
                        rdo.YEAR = item.TDL_INTRUCTION_TIME.ToString().Substring(0, 4);
                        rdo.SERVICE_TYPE_CODE = item.SERVICE_TYPE_CODE ?? "";
                        rdo.SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME ?? "";
                        rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(O => O.ID == item.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                        listRdo.Add(rdo);
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
