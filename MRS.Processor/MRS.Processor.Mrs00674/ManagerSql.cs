using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00674
{
    public class ManagerSql
    {
        public DataTable GetSum(Mrs00674Filter filter, string query)
        {
            DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.MEDI_STOCK_ID != null) ? filter.MEDI_STOCK_ID.ToString() : "''"
, (filter.EXACT_EXT_CASHIER_ROOM_ID != null) ? filter.EXACT_EXT_CASHIER_ROOM_ID.ToString() : "''"
, (filter.EXAM_ROOM_IDs != null) ? string.Join(",", filter.EXAM_ROOM_IDs) : "''"
);
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<EXP_MEST_SUB> GetListExpMestSubCode(Mrs00674Filter filter, string query)
        {
            List<EXP_MEST_SUB> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.MEDI_STOCK_ID != null) ? filter.MEDI_STOCK_ID.ToString() : "''"
, (filter.EXACT_EXT_CASHIER_ROOM_ID != null) ? filter.EXACT_EXT_CASHIER_ROOM_ID.ToString() : "''"
, (filter.EXAM_ROOM_IDs != null) ? string.Join(",", filter.EXAM_ROOM_IDs) : "''"
);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<EXP_MEST_SUB>(query);
                Inventec.Common.Logging.LogSystem.Info("sql:" + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
    public class EXP_MEST_SUB
    {
        public string EXP_MEST_SUB_CODE { get; set; }
        public decimal TDL_TOTAL_PRICE { get; set; }
        public decimal? TRANSFER_AMOUNT { get; set; }
        public string EXP_MEST_SUB_CODE_1 { get; set; }
        public decimal? TDL_TOTAL_PRICE_1 { get; set; }
        public decimal? TRANSFER_AMOUNT_1 { get; set; }
        public string EXP_MEST_SUB_CODE_2 { get; set; }
        public decimal? TDL_TOTAL_PRICE_2 { get; set; }
        public decimal? TRANSFER_AMOUNT_2 { get; set; }
    }
}
