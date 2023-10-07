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

namespace MRS.Processor.Mrs00595
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT> GetTreatment(Mrs00595Filter filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.* ";
                query += "FROM HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }
                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(Mrs00595Filter filter)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = new List<HIS_PATIENT_TYPE_ALTER>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PTA.* ";
                query += "FROM HIS_PATIENT_TYPE_ALTER PTA, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID=PTA.TREATMENT_ID ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }
                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_TYPE_ALTER>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_DEPARTMENT_TRAN> GetDepartmentTran(Mrs00595Filter filter)
        {
            List<HIS_DEPARTMENT_TRAN> result = new List<HIS_DEPARTMENT_TRAN>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DEPT.* ";
                query += "FROM HIS_DEPARTMENT_TRAN DEPT, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID=DEPT.TREATMENT_ID ";
                query += "AND TREA.CLINICAL_IN_TIME IS NOT NULL ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }
                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEPARTMENT_TRAN>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_SERVICE_REQ> GetServiceReq(Mrs00595Filter filter)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SR.* ";
                query += "FROM HIS_SERVICE_REQ SR, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND SR.IS_DELETE=0 ";
                query += "AND SR.IS_NO_EXECUTE IS NULL ";
                query += "AND TREA.ID=SR.TREATMENT_ID ";
                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID IN ({0},{1},{2}) ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT);
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }
                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >={0}  ", filter.IN_TIME_FROM);
                }
                query += "ORDER BY SR.ID ASC ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_REQ>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_PATIENT> GetPatient(Mrs00595Filter filter)
        {
            List<HIS_PATIENT> result = new List<HIS_PATIENT>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PT.* ";
                query += "FROM HIS_PATIENT PT, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.PATIENT_ID=PT.ID ";
               
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }
                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }
                query += "ORDER BY PT.ID ASC ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT>(query);
                if (rs != null)
                {
                    result = rs.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
