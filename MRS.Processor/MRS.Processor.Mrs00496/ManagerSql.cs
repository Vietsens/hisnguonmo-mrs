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
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using MRS.MANAGER.Config;
using MRS.Proccessor.Mrs00496;
using MOS.DAO.Sql;
using MOS.MANAGER.HisBlood;

namespace MRS.Processor.Mrs00496
{
    public partial class ManagerSql : BusinessBase
    {
        public List<V_HIS_MEDICINE> Get(HisMedicineViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE> result = new List<V_HIS_MEDICINE>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "ME.* ";
                query += "FROM HIS_RS.V_HIS_MEDICINE ME ";
                query += "WHERE 1=1 ";
                if (filter.IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("ME.ID IN (-1)");
                    while (filter.IDs.Count - skip > 0)
                    {
                        var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("ME.ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_MEDICINE>(query);

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

        public List<V_HIS_MATERIAL> Get(HisMaterialViewFilterQuery filter)
        {
            List<V_HIS_MATERIAL> result = new List<V_HIS_MATERIAL>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "MA.* ";
                query += "FROM HIS_RS.V_HIS_MATERIAL MA ";
                query += "WHERE 1=1 ";
                if (filter.IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("MA.ID IN (-1)");
                    while (filter.IDs.Count - skip > 0)
                    {
                        var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("MA.ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_MATERIAL>(query);

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

        public List<V_HIS_BLOOD> Get(HisBloodViewFilterQuery filter)
        {
            List<V_HIS_BLOOD> result = new List<V_HIS_BLOOD>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "MA.* ";
                query += "FROM HIS_RS.V_HIS_BLOOD MA ";
                query += "WHERE 1=1 ";
                if (filter.IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("MA.ID IN (-1)");
                    while (filter.IDs.Count - skip > 0)
                    {
                        var limit = filter.IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("MA.ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_BLOOD>(query);

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

        public List<HIS_MEDICINE_PATY> Get(HisMedicinePatyFilterQuery filter)
        {
            List<HIS_MEDICINE_PATY> result = new List<HIS_MEDICINE_PATY>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "ME.* ";
                query += "FROM HIS_RS.HIS_MEDICINE_PATY ME ";
                query += "WHERE 1=1 ";
                if (filter.MEDICINE_IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("ME.MEDICINE_ID IN (-1)");
                    while (filter.MEDICINE_IDs.Count - skip > 0)
                    {
                        var limit = filter.MEDICINE_IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("ME.MEDICINE_ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }
                query += string.Format("AND ME.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID ?? HisPatientTypeCFG.PATIENT_TYPE_ID__BUYMEDI);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICINE_PATY>(query);

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

        public List<HIS_MATERIAL_PATY> Get(HisMaterialPatyFilterQuery filter)
        {
            List<HIS_MATERIAL_PATY> result = new List<HIS_MATERIAL_PATY>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "MA.* ";
                query += "FROM HIS_RS.HIS_MATERIAL_PATY MA ";
                query += "WHERE 1=1 ";
                if (filter.MATERIAL_IDs != null)
                {
                    var skip = 0;
                    List<string> strIds = new List<string>();
                    strIds.Add("MA.MATERIAL_ID IN (-1)");
                    while (filter.MATERIAL_IDs.Count - skip > 0)
                    {
                        var limit = filter.MATERIAL_IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        strIds.Add(string.Format("MA.MATERIAL_ID IN ({0}) ", string.Join(",", limit)));
                    }
                    query += string.Format("AND ({0}) ", string.Join("OR ", strIds));
                }

                query += string.Format("AND MA.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID??HisPatientTypeCFG.PATIENT_TYPE_ID__BUYMEDI);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MATERIAL_PATY>(query);

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


        const string THUOC = "THUOC";
        const string VATTU = "VATTU";
        const string HOACHAT = "HOACHAT";
        const string MAU = "MAU";

        internal List<Mrs00496RDO> GetMediPeriod(List<long> mediStockIds, long timeFrom)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach medicine_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", THUOC);
            query += "mpm.MEDICINE_ID MEDI_MATE_ID,\n";
            query += "mpm.MEDICINE_TYPE_ID,\n";
            query += "sum(mpm.AMOUNT) AMOUNT\n";

            query += "FROM V_HIS_MEST_PERIOD_MEDI mpm\n ";
            query += "join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time,\n ";
            query += "max(id) keep(dense_rank last order by nvl(to_time,create_time)) id\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.id=mpm.medi_stock_period_id\n ";
            query += "WHERE 1=1\n ";
            query += "and mpm.IS_DELETE =0\n ";
            query += "group by\n ";
            query += "mpm.MEDICINE_ID,\n";
            query += "mpm.MEDICINE_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        internal List<Mrs00496RDO> GetMatePeriod(List<long> mediStockIds, long timeFrom)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach material_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", VATTU);
            query += "mpm.MATERIAL_ID MEDI_MATE_ID,\n";
            query += "mpm.MATERIAL_TYPE_ID,\n";
            query += "sum(mpm.AMOUNT) AMOUNT\n";

            query += "FROM V_HIS_MEST_PERIOD_MATE mpm\n ";
            query += "join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time,\n ";
            query += "max(id) keep(dense_rank last order by nvl(to_time,create_time)) id\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.id=mpm.medi_stock_period_id\n ";

            query += "WHERE 1=1\n ";
            query += "and mpm.IS_DELETE =0\n ";
            query += "group by\n ";
            query += "mpm.MATERIAL_ID,\n";
            query += "mpm.MATERIAL_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        internal List<Mrs00496RDO> GetBloodPeriod(List<long> mediStockIds, long timeFrom)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach blood_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", MAU);
            query += "mpm.BLOOD_ID MEDI_MATE_ID,\n";
            query += "mpm.BLOOD_TYPE_ID,\n";
            query += "sum(1) AMOUNT\n";

            query += "FROM V_HIS_MEST_PERIOD_BLOOD mpm\n ";
            query += "join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time,\n ";
            query += "max(id) keep(dense_rank last order by nvl(to_time,create_time)) id\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.id=mpm.medi_stock_period_id\n ";

            query += "WHERE 1=1\n ";
            query += "and mpm.IS_DELETE =0\n ";
            query += "group by\n ";
            query += "mpm.BLOOD_ID,\n";
            query += "mpm.BLOOD_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetMateExp(List<long> mediStockIds, long timeFrom, long timeTo, Mrs00496Filter filter)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", VATTU);
            query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_MATERIAL_TYPE_ID MATERIAL_TYPE_ID,\n";
           
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then EXMM.AMOUNT else 0 end) AS EXP_AMOUNT,\n", timeFrom);
            //nếu xuất hỏng vỡ thì tính vào key xuất hỏng vỡ
            if (!string.IsNullOrWhiteSpace(filter.REASON_CODE__HV))
            {
                query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_reason_id in (select id from his_exp_mest_reason where exp_mest_reason_code ='{1}') then EXMM.AMOUNT else 0 end) AS HV_EXP_AMOUNT,\n", timeFrom,filter.REASON_CODE__HV);
            }    
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-EXMM.AMOUNT) AS AMOUNT\n");

            query += "FROM HIS_EXP_MEST_MATERIAL EXMM \n";
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and EXMM.IS_EXPORT=1  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.EXP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "EXMM.TDL_MATERIAL_TYPE_ID, ";
            query += "EXMM.MATERIAL_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetMediExp(List<long> mediStockIds, long timeFrom, long timeTo, Mrs00496Filter filter)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", THUOC);
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_MEDICINE_TYPE_ID MEDICINE_TYPE_ID,\n";
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then EXMM.AMOUNT else 0 end) AS EXP_AMOUNT,\n", timeFrom);
            //nếu xuất hỏng vỡ thì tính vào key xuất hỏng vỡ
            if (!string.IsNullOrWhiteSpace(filter.REASON_CODE__HV))
            {
                query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_reason_id in (select id from his_exp_mest_reason where exp_mest_reason_code ='{1}') then EXMM.AMOUNT else 0 end) AS HV_EXP_AMOUNT,\n", timeFrom, filter.REASON_CODE__HV);
            }
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-EXMM.AMOUNT) AS AMOUNT\n");

            query += "FROM HIS_EXP_MEST_MEDICINE EXMM \n";
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and EXMM.IS_EXPORT=1  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.EXP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "EXMM.TDL_MEDICINE_TYPE_ID, ";
            query += "EXMM.MEDICINE_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetBloodExp(List<long> mediStockIds, long timeFrom, long timeTo, Mrs00496Filter filter)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", MAU);
            query += "NVL(EXMM.BLOOD_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_BLOOD_TYPE_ID BLOOD_TYPE_ID,\n";
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then 1 else 0 end) AS EXP_AMOUNT,\n", timeFrom);
            //nếu xuất hỏng vỡ thì tính vào key xuất hỏng vỡ
            if (!string.IsNullOrWhiteSpace(filter.REASON_CODE__HV))
            {
                query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_reason_id in (select id from his_exp_mest_reason where exp_mest_reason_code ='{1}') then 1 else 0 end) AS HV_EXP_AMOUNT,\n", timeFrom, filter.REASON_CODE__HV);
            }
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-1) AS AMOUNT\n");

            query += "FROM HIS_EXP_MEST_BLOOD EXMM \n";
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and EXMM.IS_EXPORT=1  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.EXP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "EXMM.TDL_BLOOD_TYPE_ID, ";
            query += "EXMM.BLOOD_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetMateImp(List<long> mediStockIds, long timeFrom, long timeTo)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", VATTU);
            query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, \n";
            query += "ma.MATERIAL_TYPE_ID,\n";
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then EXMM.AMOUNT else 0 end) AS IMP_AMOUNT,\n", timeFrom);
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(EXMM.AMOUNT) AS AMOUNT\n");

            query += "FROM HIS_IMP_MEST_MATERIAL EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_material ma on ma.id=exmm.material_id \n";
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.IMP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac nhap sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<em.imp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "ma.MATERIAL_TYPE_ID, ";
            query += "EXMM.MATERIAL_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetMediImp(List<long> mediStockIds, long timeFrom, long timeTo)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", THUOC);
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, \n";
            query += "me.MEDICINE_TYPE_ID,\n";
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then EXMM.AMOUNT else 0 end) AS IMP_AMOUNT,\n", timeFrom);
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(EXMM.AMOUNT) AS AMOUNT\n");

            query += "FROM HIS_IMP_MEST_MEDICINE EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_medicine me on me.id=exmm.medicine_id \n";
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.IMP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac nhap sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<em.imp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "me.MEDICINE_TYPE_ID, ";
            query += "EXMM.MEDICINE_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        public List<Mrs00496RDO> GetBloodImp(List<long> mediStockIds, long timeFrom, long timeTo)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", MAU);
            query += "NVL(EXMM.BLOOD_ID,0) AS MEDI_MATE_ID, \n";
            query += "me.BLOOD_TYPE_ID,\n";
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then 1 else 0 end) AS IMP_AMOUNT,\n", timeFrom);
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(1) AS AMOUNT\n");

            query += "FROM HIS_IMP_MEST_BLOOD EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_blood me on me.id=exmm.blood_id \n";
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += "left join \n ";
            query += "(select \n ";
            query += "medi_stock_id,\n ";
            query += "max(nvl(to_time,create_time)) to_time\n ";
            query += "from HIS_MEDI_STOCK_PERIOD\n ";
            query += "where 1=1\n ";
            query += "and is_delete=0\n ";
            query += string.Format("and nvl(to_time,create_time) < {0}\n ", timeFrom);
            query += string.Format("and medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "medi_stock_id\n ";
            query += ") last_msp on last_msp.medi_stock_id=Em.medi_stock_id\n ";

            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.IMP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac nhap sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<em.imp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "me.BLOOD_TYPE_ID, ";
            query += "EXMM.BLOOD_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        internal List<Mrs00496RDO> GetMediBean(List<long> mediStockIds)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach medicine_id trong cac bean hien tai\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", THUOC);
            query += "mb.MEDICINE_ID MEDI_MATE_ID,\n";
            query += "mb.TDL_MEDICINE_TYPE_ID MEDICINE_TYPE_ID,\n";
            query += "sum(mb.AMOUNT) BEAN_AMOUNT,\n";
            query += "sum(case when mb.is_active=1 then mb.AMOUNT else 0 end) ENABLE_AMOUNT\n";

            query += "FROM HIS_MEDICINE_BEAN mb\n ";
            query += "WHERE 1=1\n ";
            query += string.Format("and mb.medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "mb.MEDICINE_ID,\n";
            query += "mb.TDL_MEDICINE_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }

        internal List<Mrs00496RDO> GetMateBean(List<long> mediStockIds)
        {
            List<Mrs00496RDO> result = new List<Mrs00496RDO>();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach material_id trong cac bean hien tai\n";
            query += "SELECT \n";
            query += string.Format("'{0}' TYPE,\n", VATTU);
            query += "mb.MATERIAL_ID MEDI_MATE_ID,\n";
            query += "mb.TDL_MATERIAL_TYPE_ID MATERIAL_TYPE_ID,\n";
            query += "sum(mb.AMOUNT) BEAN_AMOUNT,\n";
            query += "sum(case when mb.is_active=1 then mb.AMOUNT else 0 end) ENABLE_AMOUNT\n";

            query += "FROM HIS_MATERIAL_BEAN mb\n ";
            query += "WHERE 1=1\n ";
            query += string.Format("and mb.medi_stock_id in ({0})\n ", string.Join(",", mediStockIds));
            query += "group by\n ";
            query += "mb.MATERIAL_ID,\n";
            query += "mb.TDL_MATERIAL_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00496RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00496");
            return result;
        }
    }
}
