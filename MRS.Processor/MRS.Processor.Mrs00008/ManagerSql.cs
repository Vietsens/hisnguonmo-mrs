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
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;

namespace MRS.Processor.Mrs00008
{
    public partial class ManagerSql : BusinessBase
    {
        public List<V_HIS_EXP_MEST_MATERIAL> GetView(HisExpMestMaterialViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = new List<V_HIS_EXP_MEST_MATERIAL>();
            try
            {
                //expMestMedicineViewFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                //expMestMedicineViewFilter.CREATE_TIME_FROM = castFilter.CREATE_TIME_FROM;
                //expMestMedicineViewFilter.CREATE_TIME_TO = castFilter.CREATE_TIME_TO;
                //expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                //expMestMedicineViewFilter.EXP_MEST_TYPE_IDs = new List<long> {
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                //    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT };
                //expMestMedicineViewFilter.IS_EXPORT = true;
                string query = "";
                query += "SELECT ";
                query += "EXMA.* ";

                query += "FROM V_HIS_EXP_MEST_MATERIAL EXMA ";
                query += "WHERE IS_EXPORT=1 AND IS_DELETE =0 ";
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXMA.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.EXP_MEST_STT_ID != null)
                {
                    query += string.Format("AND EXMA.EXP_MEST_STT_ID = {0} ", filter.EXP_MEST_STT_ID);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND EXMA.CREATE_TIME < {0} ", filter.CREATE_TIME_TO);
                }
                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND EXMA.CREATE_TIME >={0}   ", filter.CREATE_TIME_FROM);
                }

                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND EXMA.MEDI_STOCK_ID = {0} ", filter.MEDI_STOCK_ID);
                }
                if (filter.EXP_MEST_TYPE_IDs != null)
                {
                    query += string.Format("AND EXMA.EXP_MEST_TYPE_ID IN ({0}) ", string.Join(",", filter.EXP_MEST_TYPE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_EXP_MEST_MATERIAL>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
        public List<V_HIS_EXP_MEST_MEDICINE> GetView(HisExpMestMedicineViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = new List<V_HIS_EXP_MEST_MEDICINE>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "EXMA.* ";

                query += "FROM V_HIS_EXP_MEST_MEDICINE EXMA ";
                query += "WHERE IS_EXPORT=1 AND IS_DELETE =0 ";
                if (filter.MEDI_STOCK_IDs != null)
                {
                    query += string.Format("AND EXMA.MEDI_STOCK_ID IN ({0}) ", string.Join(",", filter.MEDI_STOCK_IDs));
                }
                if (filter.EXP_MEST_STT_ID != null)
                {
                    query += string.Format("AND EXMA.EXP_MEST_STT_ID = {0} ", filter.EXP_MEST_STT_ID);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND EXMA.CREATE_TIME < {0} ", filter.CREATE_TIME_TO);
                }
                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND EXMA.CREATE_TIME >={0}   ", filter.CREATE_TIME_FROM);
                }

                if (filter.MEDI_STOCK_ID != null)
                {
                    query += string.Format("AND EXMA.MEDI_STOCK_ID = {0} ", filter.MEDI_STOCK_ID);
                }
                if (filter.EXP_MEST_TYPE_IDs != null)
                {
                    query += string.Format("AND EXMA.EXP_MEST_TYPE_ID IN ({0}) ", string.Join(",", filter.EXP_MEST_TYPE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_EXP_MEST_MEDICINE>(query);


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
