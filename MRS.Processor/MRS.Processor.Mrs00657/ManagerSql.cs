using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00657
{
    class ManagerSql
    {
        internal List<D_HIS_SERE_SERV_BILL> GetSSB(Mrs00657Filter filter)
        {
            List<D_HIS_SERE_SERV_BILL> result = new List<D_HIS_SERE_SERV_BILL>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.*, ";
                //query += "SSB.BILL_ID, SSB.PRICE AS BILL_PRICE, ";
                //if (filter.TIME_TO.HasValue || filter.TIME_FROM.HasValue)
                //{
                //    query += "TRAN.TRANSACTION_TIME, ";
                //}

                query += "T.TDL_PATIENT_TYPE_ID AS TREATMENT_TDL_PATIENT_TYPE_ID, T.IN_TIME AS TREATMENT_IN_TIME, T.TDL_PATIENT_NAME, ";
                query += "T.TDL_PATIENT_DOB, T.END_DEPARTMENT_ID, T.TDL_PATIENT_GENDER_ID, T.TDL_PATIENT_GENDER_NAME, ";
                query += "SV.PARENT_ID AS SERVICE_PARENT_ID, ";
                query += "SVT.SERVICE_TYPE_NAME, SVT.NUM_ORDER ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";

                if (filter.TIME_TO.HasValue || filter.TIME_FROM.HasValue)
                {
                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB  ON SS.ID = SSB.SERE_SERV_ID ";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID = SSB.BILL_ID ";
                }

                query += "JOIN HIS_RS.HIS_SERVICE_TYPE SVT ON SVT.ID = SS.TDL_SERVICE_TYPE_ID ";
                query += "JOIN HIS_RS.HIS_TREATMENT T ON T.ID = SS.TDL_TREATMENT_ID ";
                query += "JOIN HIS_RS.HIS_SERVICE SV ON SV.ID = SS.SERVICE_ID ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.IS_DELETE = 0 ";
                if (filter.TIME_TO.HasValue || filter.TIME_FROM.HasValue)
                {
                    query += "AND SSB.IS_CANCEL IS NULL AND SSB.IS_DELETE = 0 ";
                    query += "AND TRAN.IS_CANCEL IS NULL AND TRAN.IS_DELETE = 0 ";

                    if (filter.TIME_TO.HasValue)
                    {
                        query += string.Format("AND TRAN.TRANSACTION_TIME <= {0} ", filter.TIME_TO.Value);
                    }

                    if (filter.TIME_FROM.HasValue)
                    {
                        query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TIME_FROM.Value);
                    }
                }
                else if (filter.OUT_TIME_TO.HasValue || filter.OUT_TIME_FROM.HasValue)
                {
                    if (filter.OUT_TIME_TO.HasValue)
                        query += string.Format("AND T.OUT_TIME <= {0} ", filter.OUT_TIME_TO.Value);

                    if (filter.OUT_TIME_FROM.HasValue)
                        query += string.Format("AND T.OUT_TIME >= {0} ", filter.OUT_TIME_FROM.Value);

                    query += "AND T.IS_PAUSE = 1 ";
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null && filter.REQUEST_DEPARTMENT_IDs.Count > 0)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0}) ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_SERE_SERV_BILL>(query);

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
    }

    public class D_HIS_SERE_SERV_BILL : HIS_SERE_SERV
    {
        //public long BILL_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        //public long TRANSACTION_TIME { get; set; }
        //public decimal BILL_PRICE { get; set; }
        public long? NUM_ORDER { get; set; }
        public long? SERVICE_PARENT_ID { get; set; }

        public long? TREATMENT_TDL_PATIENT_TYPE_ID { get; set; }
        public long TREATMENT_IN_TIME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
    }
}
