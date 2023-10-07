using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00644
{
    public partial class ManagerSql : BusinessBase
    {
        internal List<Mrs00644RDO> GetSereServInvoice(Mrs00644Filter filter) 
        {
            List<Mrs00644RDO> result = new List<Mrs00644RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.TREATMENT_CODE, ";
                query += "NVL(TREA.END_DEPARTMENT_ID,0) AS END_DEPARTMENT_ID, ";
                query += "TREA.TDL_PATIENT_DOB AS DOB, ";
                query += "TREA.IN_TIME, ";
                query += "NVL(TREA.OUT_TIME,0) AS OUT_TIME, ";
                query += "TREA.TDL_PATIENT_CODE AS PATIENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME AS PATIENT_NAME, ";
                query += "IVB.TEMPLATE_CODE, ";
                query += "IVB.SYMBOL_CODE, ";
                query += "IV.CREATE_TIME, ";
                query += "IV.VIR_NUM_ORDER, ";
                query += "IV.VAT_RATIO AS GTGT, ";
                query += "SUM(SS.VIR_TOTAL_PATIENT_PRICE) AS VIR_TOTAL_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS EXAM_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS KSK_EXAM_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS GDTT_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =14 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS BLOOD_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =6 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS MEDICINE_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =7 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS MATERIAL_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =8 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS BED_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID IN (4,11) THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS SURGMISU_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =2 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TEST_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =3 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS DIIM_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =10 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS SUIM_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS ECG_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS EEG_PRICE, ";
                query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =9 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS ENDO_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS CT_PRICE, ";
                if (filter.CATEGORY_CODEs != null)
                {
                    List<string> categoryCodes = filter.CATEGORY_CODEs.Split(new char[] { ',' }).ToList();
                    foreach (var item in categoryCodes)
                    {
                        query += string.Format("SUM(CASE WHEN SRC.CATEGORY_CODE='{0}' THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS {0}_PRICE, ", item);

                    }
                }
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS KTG_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS DD_PRICE, ";
                //query += "SUM(CASE WHEN SS.TDL_SERVICE_TYPE_ID =0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS OTHER_PRICE, ";
                query += "SUM(CASE WHEN SS.HEIN_RATIO BETWEEN 0.69 AND 0.71 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PRICE_30, ";
                query += "SUM(CASE WHEN SS.HEIN_RATIO BETWEEN 0.79 AND 0.81 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PRICE_20, ";
                query += "SUM(CASE WHEN SS.HEIN_RATIO BETWEEN 0.94 AND 0.96 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PRICE_5, ";
                query += "SUM(CASE WHEN SS.HEIN_RATIO BETWEEN 1 AND 0.96 OR SS.HEIN_RATIO BETWEEN 0.94 AND 0.81 OR SS.HEIN_RATIO BETWEEN 0.79 AND 0.71 OR SS.HEIN_RATIO BETWEEN 0.69 AND 0 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PRICE_OTHER, ";
                query += "SUM(SS.VIR_TOTAL_PATIENT_PRICE) AS TOTAL_PATIENT_PRICE, ";
                query += "SUM(SS.VIR_TOTAL_HEIN_PRICE) AS TOTAL_HEIN_PRICE ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00644') ";
                query += "JOIN HIS_RS.HIS_INVOICE IV ON (IV.ID = SS.INVOICE_ID AND IV.IS_CANCEL IS NULL  ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IV.INVOICE_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IV.INVOICE_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND IV.CREATE_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND IV.CREATE_TIME < {0} ", filter.TIME_TO);
                }
                long NumOrderFrom;
                if (long.TryParse(filter.NUM_ORDER_FROM ?? "0", out NumOrderFrom))
                {
                    query += string.Format("AND IV.NUM_ORDER >= {0} ", NumOrderFrom);
                }
                long NumOrderTo;
                if (long.TryParse(filter.NUM_ORDER_TO ?? "9999999999", out NumOrderTo))
                {
                    query += string.Format("AND IV.NUM_ORDER < {0} ", NumOrderTo);
                }
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND IV.CREATOR = '{0}' ", filter.CASHIER_LOGINNAME);
                }
                if (filter.INVOICE_BOOK_ID != null)
                {
                    query += string.Format("AND IV.INVOICE_BOOK_ID = '{0}' ", filter.INVOICE_BOOK_ID);
                }
                query += ") ";

                query += "JOIN HIS_RS.HIS_INVOICE_BOOK IVB  ON IVB.ID = IV.INVOICE_BOOK_ID  ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID  ";

                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";

                query += "GROUP BY ";
                query += "TREA.TREATMENT_CODE, ";
                query += "NVL(TREA.END_DEPARTMENT_ID,0), ";
                query += "TREA.TDL_PATIENT_DOB, ";
                query += "TREA.IN_TIME, ";
                query += "TREA.OUT_TIME, ";
                query += "TREA.TDL_PATIENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "IVB.TEMPLATE_CODE, ";
                query += "IVB.SYMBOL_CODE, ";
                query += "IV.VAT_RATIO, ";
                query += "IV.CREATE_TIME, ";
                query += "IV.VIR_NUM_ORDER ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00644RDO>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<Mrs00644RDO> GetInvoiceOut(Mrs00644Filter filter)
        {
            List<Mrs00644RDO> result = new List<Mrs00644RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "IV.BUYER_NAME AS PATIENT_NAME, ";
                query += "IVB.TEMPLATE_CODE, ";
                query += "IVB.SYMBOL_CODE, ";
                query += "IV.CREATE_TIME, ";
                query += "IV.VIR_NUM_ORDER, ";
                query += "IV.VAT_RATIO AS GTGT, ";
                query += "SUM(IVD.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.HIS_INVOICE_DETAIL IVD ";
                query += "JOIN HIS_RS.HIS_INVOICE IV ON (IV.ID = IVD.INVOICE_ID AND IV.IS_CANCEL IS NULL  ";

                query += "AND not exists (select 1 from his_sere_serv where invoice_id=iv.id) ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IV.INVOICE_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IV.INVOICE_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND IV.CREATE_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND IV.CREATE_TIME < {0} ", filter.TIME_TO);
                }
                long NumOrderFrom;
                if (long.TryParse(filter.NUM_ORDER_FROM ?? "0", out NumOrderFrom))
                {
                    query += string.Format("AND IV.NUM_ORDER >= {0} ", NumOrderFrom);
                }
                long NumOrderTo;
                if (long.TryParse(filter.NUM_ORDER_TO ?? "9999999999", out NumOrderTo))
                {
                    query += string.Format("AND IV.NUM_ORDER < {0} ", NumOrderTo);
                }
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND IV.CREATOR = '{0}' ", filter.CASHIER_LOGINNAME);
                }
                if (filter.INVOICE_BOOK_ID != null)
                {
                    query += string.Format("AND IV.INVOICE_BOOK_ID = '{0}' ", filter.INVOICE_BOOK_ID);
                }
                query += ") ";

                query += "JOIN HIS_RS.HIS_INVOICE_BOOK IVB  ON IVB.ID = IV.INVOICE_BOOK_ID  ";

                query += "WHERE 1=1 ";
                query += "AND IVD.IS_DELETE =0 ";

                query += "GROUP BY ";
                query += "IV.BUYER_NAME, ";
                query += "IVB.TEMPLATE_CODE, ";
                query += "IVB.SYMBOL_CODE, ";
                query += "IV.VAT_RATIO, ";
                query += "IV.CREATE_TIME, ";
                query += "IV.VIR_NUM_ORDER ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00644RDO>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<SAR_PRINT_LOG_D> GetPrintLog(Mrs00644Filter filter)
        {
            List<SAR_PRINT_LOG_D> result = new List<SAR_PRINT_LOG_D>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SPL.UNIQUE_CODE, ";
                query += "SPL.NUM_ORDER ";

                query += "FROM SAR_RS.SAR_PRINT_LOG SPL ";
                query += "WHERE 1=1 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SPL.CREATE_TIME >= {0} ", filter.TIME_FROM);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new SAR.DAO.Sql.SqlDAO().GetSql<SAR_PRINT_LOG_D>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<HIS_INVOICE_CANCEL> GetInvoiceCancel(Mrs00644Filter filter)
        {
            List<HIS_INVOICE_CANCEL> result = new List<HIS_INVOICE_CANCEL>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "IV.ID, ";
                query += "IV.VIR_UNIQUE, ";
                query += "IV.VIR_NUM_ORDER, ";

                query += "IV.CANCEL_TIME, ";
                query += "IV.CANCEL_REASON, ";
                query += "IVB.TEMPLATE_CODE, ";
                query += "SS.TDL_TREATMENT_CODE AS TREATMENT_CODE, ";
                query += "IV.IS_CANCEL, IV.CANCEL_USERNAME, IV.CANCEL_LOGINNAME, ";
                query += "IV1.INVOICE_TIME, IV1.ID AS REUSE_ID, ";
                query += "IV.NUM_ORDER, ";
                query += "IV1.CREATOR, ";
                query += "IV.INVOICE_BOOK_ID, ";
                query += "IVB.SYMBOL_CODE ";
                query += "FROM HIS_RS.HIS_INVOICE IV ";
                query += "LEFT JOIN HIS_RS.HIS_INVOICE IV1  ON (IV1.NUM_ORDER = IV.NUM_ORDER AND IV1.IS_CANCEL IS NULL AND IV1.INVOICE_BOOK_ID=IV.INVOICE_BOOK_ID)  ";
                query += "LEFT JOIN HIS_RS.HIS_INVOICE_BOOK IVB  ON IVB.ID = IV1.INVOICE_BOOK_ID  ";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV SS  ON IV1.ID = SS.INVOICE_ID  ";
                query += "WHERE 1=1 ";
                query += "AND IV.IS_CANCEL=1 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IV.CANCEL_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IV.CANCEL_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND IV.CREATOR = '{0}' ", filter.CASHIER_LOGINNAME);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_INVOICE_CANCEL>(query);

                if (rs != null)
                {
                    result = rs.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
