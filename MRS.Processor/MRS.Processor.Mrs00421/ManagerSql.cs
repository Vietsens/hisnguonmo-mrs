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

namespace MRS.Processor.Mrs00421
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT> GetTreatment(Mrs00421Filter filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.* ";
                query += "FROM HIS_RS.HIS_TREATMENT TREA, ";
                query += "HIS_RS.V_HIS_EXP_MEST_MEDICINE EXMM ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID = EXMM.TDL_TREATMENT_ID ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME < {0} ", filter.TIME_TO);
                }
                query += "UNION ALL ";
                query += "(SELECT ";
                query += "TREA.* ";
                query += "FROM HIS_RS.HIS_TREATMENT TREA, ";
                query += "HIS_RS.V_HIS_EXP_MEST_MATERIAL EXMM ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID = EXMM.TDL_TREATMENT_ID ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND EXMM.EXP_TIME < {0} ", filter.TIME_TO);
                }

                query += ") ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (rs != null)
                {
                    result = rs.GroupBy(o=>o.ID).Select(p=>p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        List<long> impMestTypeIds = new List<long>(){
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
    
        };
        public List<V_HIS_IMP_MEST_MEDICINE> GetMobaImpMedicine(Mrs00421Filter filter)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "EXME.* \n";
                query += "FROM V_HIS_IMP_MEST_MEDICINE EXME \n";
                query += "join his_imp_mest him on him.id= exme.imp_mest_id \n ";

                query += "WHERE 1=1 ";
                query += string.Format("AND EXME.IMP_MEST_TYPE_ID in ({0}) \n ",string.Join(",",impMestTypeIds));
                query += string.Format("AND HIM.IMP_MEST_STT_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT);
                if (filter.TIME_FROM != null && filter.TIME_TO != null)
                {
                    query += string.Format("AND HIM.IMP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM,filter.TIME_TO);
                }
                query += string.Format("AND him.MEDI_STOCK_ID in ({0}) \n",string.Join(",",filter.MEDI_STOCK_IDs));
                query += string.Format("AND EXME.REQ_DEPARTMENT_ID = {0} \n",filter.REQ_DEPARTMENT_ID);
                query += string.Format("and exme.th_exp_mest_medicine_id is not null \n"); 
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(query);
                result = rs;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<V_HIS_IMP_MEST_MATERIAL> GetMobaImpMaterial(Mrs00421Filter filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "EXME.* \n";
                query += "FROM V_HIS_IMP_MEST_MATERIAL EXME \n";
                query += "join his_imp_mest him on him.id= exme.imp_mest_id \n ";

                query += "WHERE 1=1 \n";
                query += string.Format("AND EXME.IMP_MEST_TYPE_ID in ({0}) \n ", string.Join(",", impMestTypeIds));
                query += string.Format("AND HIM.IMP_MEST_STT_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT);
                if (filter.TIME_FROM != null && filter.TIME_TO != null)
                {
                    query += string.Format("AND HIM.IMP_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                query += string.Format("AND him.MEDI_STOCK_ID in ({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
                query += string.Format("AND EXME.REQ_DEPARTMENT_ID = {0} \n", filter.REQ_DEPARTMENT_ID);
                query += string.Format("and exme.th_exp_mest_material_id is not null \n");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(query);
                result = rs;
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
