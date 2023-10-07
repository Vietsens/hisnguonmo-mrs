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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00628
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_EXP_MEST> Get(HisExpMestFilterQuery filter)
        {
            List<HIS_EXP_MEST> result = new List<HIS_EXP_MEST>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "EXM.* ";
                query += "FROM HIS_RS.HIS_EXP_MEST EXM ";
                query += "WHERE 1=1 ";
                if (filter.IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("EXM.ID IN (-1)");
                    while (filter.IDs.Count - skip > 0)
                    {
                        var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("EXM.ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EXP_MEST>(query);

                if (rs != null)
                {
                    result = rs;
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

        public List<HIS_IMP_MEST_MEDICINE> GetImpMedi(List<long> filter)
        {
            List<HIS_IMP_MEST_MEDICINE> result = new List<HIS_IMP_MEST_MEDICINE>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "IMME.* ";
                query += "FROM HIS_RS.HIS_IMP_MEST_MEDICINE IMME ";
                query += "JOIN HIS_RS.HIS_IMP_MEST IMM ON IMM.ID = IMME.IMP_MEST_ID ";
                query += "WHERE 1=1 ";
                query += "AND IMME.IS_DELETE =0 AND IMM.IMP_MEST_STT_ID =5 ";
                if (filter != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("IMM.MOBA_EXP_MEST_ID IN (-1)");
                    while (filter.Count - skip > 0)
                    {
                        var limit = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("IMM.MOBA_EXP_MEST_ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_IMP_MEST_MEDICINE>(query);

                if (rs != null)
                {
                    result = rs;
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

        public List<HIS_IMP_MEST_MATERIAL> GetImpMate(List<long> filter)
        {
            List<HIS_IMP_MEST_MATERIAL> result = new List<HIS_IMP_MEST_MATERIAL>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "IMME.* ";
                query += "FROM HIS_RS.HIS_IMP_MEST_MATERIAL IMME ";
                query += "JOIN HIS_RS.HIS_IMP_MEST IMM ON IMM.ID = IMME.IMP_MEST_ID ";
                query += "WHERE 1=1 ";
                query += "AND  IMME.IS_DELETE =0 AND IMM.IMP_MEST_STT_ID =5 ";
                if (filter != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("IMM.MOBA_EXP_MEST_ID IN (-1)");
                    while (filter.Count - skip > 0)
                    {
                        var limit = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("IMM.MOBA_EXP_MEST_ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_IMP_MEST_MATERIAL>(query);

                if (rs != null)
                {
                    result = rs;
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
