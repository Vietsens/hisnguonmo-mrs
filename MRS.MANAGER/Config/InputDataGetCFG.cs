using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisDepartment;
using MRS.SDO;
using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportTemplate.Get;
using System.Data;
using MRS.MANAGER.Core.MrsReport.Lib;

namespace MRS.MANAGER.Config
{
    public class InputDatGetCFG
    {
       

        private static Dictionary<string,List<SDO.DataGetSDO>> inputs;
        public static Dictionary<string, List<SDO.DataGetSDO>> INPUTs
        {
            get
            {
                if (inputs == null || inputs.Count == 0)
                {
                    inputs = new Dictionary<string, List<SDO.DataGetSDO>>();
                    inputs = GetAll();
                }
                return inputs;
            }
            set
            {
                inputs = value;
            }
        }

        private static Dictionary<string, List<MRS.SDO.DataGetSDO>> GetAll()
        {
            Dictionary<string, List<MRS.SDO.DataGetSDO>> result = null;
            try
            {
                var template = GetReportTemplate("MRSINPUT01");
                if(template != null)
                {
                    List<DataQuerySDO> dataObject = new List<DataQuerySDO>();
                    List<string> queryValueCells = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(template.REPORT_TEMPLATE_URL, 1, 1, 1, 15, 1);
                    if (queryValueCells != null && queryValueCells.Exists(o => o.ToUpper().Contains("SELECT")))
                    {
                        for (int i = 0; i < 15; i++)
                        {

                            Inventec.Common.Logging.LogSystem.Info("SQL: " + queryValueCells[i]);
                            var data = new MOS.DAO.Sql.SqlDAO().GetSql<DataQuerySDO>(queryValueCells[i]);
                            dataObject.AddRange(data ?? new List<DataQuerySDO>());
                        }
                    }
                    result = dataObject.GroupBy(g => g.KEY).ToDictionary(p => p.Key, q => q.Select(o=>new MRS.SDO.DataGetSDO() { CODE = o.CODE,NAME=o.NAME,ID = o.ID,PARENT = o.PARENT,GRAND_PARENT = o.GRAND_PARENT,IsChecked = o.IsChecked,IS_OUTPUT0 = o.IS_OUTPUT0}).ToList());
                }    
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        private static SAR_REPORT_TEMPLATE GetReportTemplate(string reportTemplateCode)
        {
            SAR_REPORT_TEMPLATE result = null;
            try
            {
                SarReportTemplateFilterQuery filter = new SarReportTemplateFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.SAR_RS.COMMON.IS_ACTIVE__TRUE;
                filter.REPORT_TEMPLATE_CODE = reportTemplateCode;
                var data = new SAR.MANAGER.Manager.SarReportTemplateManager(new CommonParam()).Get<List<SAR_REPORT_TEMPLATE>>(filter);
                result = (data != null && data.Count>0) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public static void Refresh()
        {
            try
            {
                inputs = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
