using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00005
{
    public partial class HisManagerSql : BusinessBase
    {
        public List<V_HIS_IMP_MEST_MEDICINE> GetMedi(Mrs00005Filter filter)
        {
            try
            {
                List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
                string query = "";
                query += "SELECT * ";

                query += "FROM V_HIS_IMP_MEST_MEDICINE EXPME ";
                query += "WHERE EXPME.IMP_MEST_STT_ID=5 ";

                if (filter.EXPIRED_TIME_FROM != null)
                {
                    query += string.Format("AND EXPME.EXPIRED_DATE >={0} ", filter.EXPIRED_TIME_FROM);
                }
                if (filter.EXPIRED_TIME_TO != null)
                {
                    query += string.Format("AND EXPME.EXPIRED_DATE <{0} ", filter.EXPIRED_TIME_TO);
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND EXPME.MEDI_STOCK_ID = {0} ", filter.MEDI_STOCK_ID);
                }
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXPME.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.IMP_MEST_TYPE_ID != null)
                {
                    query += string.Format("AND EXPME.IMP_MEST_TYPE_ID = {0} ", filter.IMP_MEST_TYPE_ID);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(query);
                    return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        public List<V_HIS_IMP_MEST_MATERIAL> GetMate(Mrs00005Filter filter)
        {
            try
            {
                List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
                string query = "";
                query += "SELECT * ";

                query += "FROM V_HIS_IMP_MEST_MATERIAL EXPME ";
                query += "WHERE EXPME.IMP_MEST_STT_ID=5 ";

                if (filter.EXPIRED_TIME_FROM != null)
                {
                    query += string.Format("AND EXPME.EXPIRED_DATE >={0} ", filter.EXPIRED_TIME_FROM);
                }
                if (filter.EXPIRED_TIME_TO != null)
                {
                    query += string.Format("AND EXPME.EXPIRED_DATE <{0} ", filter.EXPIRED_TIME_TO);
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND EXPME.MEDI_STOCK_ID = {0} ", filter.MEDI_STOCK_ID);
                }
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXPME.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.IMP_MEST_TYPE_ID != null)
                {
                    query += string.Format("AND EXPME.IMP_MEST_TYPE_ID = {0} ", filter.IMP_MEST_TYPE_ID);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(query);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICAL_CONTRACT> GetMedicalContract()
        {
            try
            {
                List<HIS_MEDICAL_CONTRACT> result = new List<HIS_MEDICAL_CONTRACT>();
                string query = "";
                query += "SELECT * ";

                query += "FROM HIS_MEDICAL_CONTRACT  ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICAL_CONTRACT>(query);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
