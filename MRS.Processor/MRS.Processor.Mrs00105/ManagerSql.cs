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
using MOS.DAO.Sql;
using MRS.Proccessor.Mrs00105;
using System.Data;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00105
{
    public partial class ManagerSql : BusinessBase
    {

        const long THUOC = 1;
        const long VATTU = 2;
        const long HOACHAT = 3;
        const long MAU = 4;

        internal List<Mrs00105RDO> GetMediPeriod(List<long> mediStockIds, long timeFrom, string medicineTypeCodes, Mrs00105Filter filter)
        {

            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_MEDICINE != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach medicine_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", THUOC);
            query += "last_msp.medi_stock_id MEDI_STOCK_ID,\n";
            query += "mpm.MEDICINE_ID MEDI_MATE_ID,\n";
            query += "mpm.MEDICINE_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            query += "sum(mpm.AMOUNT) BEGIN_AMOUNT,\n";
            query += "sum(mpm.AMOUNT) END_AMOUNT\n";

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

            FilterMety(ref query, "mpm.medicine_type_id", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND mpm.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND mpm.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND mpm.BID_ID = {0}\n", filter.BID_ID);
            }
            if (!string.IsNullOrWhiteSpace(medicineTypeCodes))
            {
                query += string.Format("AND mpm.MEDICINE_TYPE_CODE in ('{0}')\n", medicineTypeCodes.Replace(",", "','"));
            }
            query += "group by\n ";
            query += "last_msp.medi_stock_id,\n";
            query += "mpm.MEDICINE_ID,\n";
            query += "mpm.MEDICINE_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        internal List<Mrs00105RDO> GetBloodPeriod(List<long> mediStockIds, long timeFrom, string bloodTypeCodes, Mrs00105Filter filter)
        {
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_BLOOD != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach blood_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", MAU);
            query += "last_msp.medi_stock_id MEDI_STOCK_ID,\n";
            query += "mpm.BLOOD_ID MEDI_MATE_ID,\n";
            query += "bl.BLOOD_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            query += "sum(1) BEGIN_AMOUNT,\n";
            query += "sum(1) END_AMOUNT\n";

            query += "FROM HIS_MEST_PERIOD_BLOOD mpm\n ";
            query += "JOIN V_HIS_BLOOD bl on bl.id= mpm.blood_id\n ";
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

            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("AND bl.BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }
            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND bl.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND bl.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND bl.BID_ID = {0}\n", filter.BID_ID);
            }
            if (!string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("AND bl.BLOOD_TYPE_CODE in ('{0}')\n", bloodTypeCodes.Replace(",", "','"));
            }
            query += "group by\n ";
            query += "last_msp.medi_stock_id,\n";
            query += "mpm.BLOOD_ID,\n";
            query += "bl.BLOOD_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }
        internal List<Mrs00105RDO> GetMatePeriod(List<long> mediStockIds, long timeFrom, string materialTypeCodes, Mrs00105Filter filter)
        {
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach material_id trong cac chot ky gan nhat\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", VATTU);
            query += "last_msp.medi_stock_id MEDI_STOCK_ID,\n";
            query += "mpm.MATERIAL_ID MEDI_MATE_ID,\n";
            query += "mpm.MATERIAL_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            query += "sum(mpm.AMOUNT) BEGIN_AMOUNT,\n";
            query += "sum(mpm.AMOUNT) END_AMOUNT\n";

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

            FilterMaty(ref query, "mpm.MATERIAL_TYPE_ID", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND mpm.BID_ID is not null\n");
            }

            if (filter.BID_IDs != null)
            {
                query += string.Format("AND mpm.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND mpm.BID_ID ={0}\n", filter.BID_ID);
            }
            if (!string.IsNullOrWhiteSpace(materialTypeCodes))
            {
                query += string.Format("AND mpm.MATERIAL_TYPE_CODE in ('{0}')\n", materialTypeCodes.Replace(",", "','"));
            }
            query += "group by\n ";
            query += "last_msp.medi_stock_id,\n";
            query += "mpm.MATERIAL_ID,\n";
            query += "mpm.MATERIAL_TYPE_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<Mrs00105RDO> GetMateExp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string materialTypeCodes = filter.MATERIAL_TYPE_CODEs;
            bool? AddExpAmountBhyt = filter.ADD_EXP_AMOUNT_BHYT;
            string MEDI_STOCK_CODE__NTs = filter.MEDI_STOCK_CODE__NTs;
            string MEDI_STOCK_CODE__IMPs = filter.MEDI_STOCK_CODE__IMPs;
            string REQ_DEPARTMENT_CODE__IMPs = filter.REQ_DEPARTMENT_CODE__IMPs;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", VATTU);
            query += "max(exmm.exp_time) LAST_EXP_TIME_NUM, \n";
            query += "max(exmm.patient_type_id) keep(dense_rank last order by (case when exmm.patient_type_id is null then 0 else exmm.exp_mest_id end)) PATIENT_TYPE_ID, \n";
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_MATERIAL_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end) AS REQ_DEPARTMENT_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end) AS exp_mest_type_id,\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end) AS exp_mest_reason_id,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);
            //neu cho phep them truong xuat bhyt va xuat sau timefrom thi tinh so luong xuat cho doi tuong bao hiem y te
            if (AddExpAmountBhyt == true)
            {
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id = {1} then nvl(exmm.amount,0) else 0 end) AS BHYT_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id <> {1} then nvl(exmm.amount,0) else 0 end) AS VP_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            }

            //neu cho phep them truong xuat nhap noi bo va xuat sau timefrom thi tinh so luong xuat cho doi tuong bao hiem y te
            if (filter.ADD_IMP_LOCAL == true)
            {
                query += string.Format("sum(case when exmm.exp_time>{0} and chmsImp.id is not null then nvl(exmm.amount,0) else 0 end) AS LOCAL_EXP_TOTAL_AMOUNT,\n", timeFrom);
            }
            //xuat cho benh nhan dieu tri noi tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} then EXMM.AMOUNT else 0 end) AS EXP_NOI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri ngoai tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} then EXMM.AMOUNT else 0 end) AS EXP_NGOAI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri noi tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} and exmm.patient_type_id={2} then EXMM.AMOUNT else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //xuat cho benh nhan dieu tri ngoai tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} and exmm.patient_type_id={2} then EXMM.AMOUNT else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NGT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //neu danh sach kho noi tru khac null va xuat sau timefrom thi tinh so luong xuat cho kho noi tru
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__NTs))
            {
                var mediStockIdNts = HisMediStockCFG.HisMediStocks.Where(o => string.Format(",{0},", MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", o.MEDI_STOCK_CODE))).Select(p => p.ID).ToList() ?? new List<long>();

                if (mediStockIdNts.Count > 0)
                    query += string.Format("sum(case when exmm.exp_time>{0} and em.imp_medi_stock_id in ({1}) then nvl(exmm.amount,0) else 0 end) AS KNT_EXP_TOTAL_AMOUNT,\n", timeFrom, string.Join(",", mediStockIdNts));
            }

            //neu danh sach kho nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac kho nhan
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                var mediStockCodeImps = MEDI_STOCK_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeImps.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and imps.medi_stock_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                }
            }
            query += "'{'" + strDicImp + "||'}'  AS JSON_IMP_MEDI_STOCK,\n";

            //neu danh sach khoa nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac khoa nhan
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__IMPs))
            {
                var reqDepartmentCodeImps = REQ_DEPARTMENT_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeImps.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_BHYT == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_BHYT\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} and exmm.is_expend is null then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)));

                    }
                    if (filter.IS_IMP_DEPA_VP == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_VP\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} and exmm.is_expend is null then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)));

                    }
                    if (filter.IS_IMP_DEPA_HP == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_HP\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.is_expend = 1 then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));

                    }
                }
            }

            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_IMP_REQ_DEPARTMENT,\n";
            //neu xuat truoc timeFrom thi tru vao ton dau
            query += string.Format("SUM(case when exmm.exp_time<={0} then -EXMM.AMOUNT else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then EXMM.AMOUNT else 0 end) AS EXP_TOTAL_AMOUNT,\n", timeFrom);
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-EXMM.AMOUNT) AS END_AMOUNT,\n");

            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là htcs thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS CABIN_HTCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);


            //query += string.Format("(case when  em.exp_mest_type_id = {17} then nvl(exmm.PRICE,0) else 0 end) AS VACCIN_PRICE,\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN);
            //query += string.Format("(case when  em.exp_mest_type_id = {17} then nvl(exmm.VAT_RATIO,0) else 0 end) AS VACCIN_VAT_RATIO,\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN);

            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end) AS SALE_PRICE,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end) AS SALE_VAT_RATIO,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then EXMM.AMOUNT * nvl(EXMM.PRICE,0) * (1 + nvl(EXMM.VAT_RATIO,0)) else 0 end) AS SALE_TOTAL_PRICE\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);


            

            query += "FROM HIS_EXP_MEST_MATERIAL EXMM \n";
            if (!string.IsNullOrWhiteSpace(materialTypeCodes))
            {
                query += string.Format("JOIN HIS_MATERIAL ME ON ME.ID=EXMM.MATERIAL_ID\n JOIN HIS_MATERIAL_TYPE MT ON MT.ID=ME.MATERIAL_TYPE_ID AND MT.MATERIAL_TYPE_CODE in ('{0}')\n", materialTypeCodes.Replace(",", "','"));
            }
            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("JOIN HIS_MATERIAL ME1 ON ME1.ID=EXMM.MATERIAL_ID\n");
                query += string.Format("AND me1.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("JOIN HIS_MATERIAL ME1 ON ME1.ID=EXMM.MATERIAL_ID\n");
                query += string.Format("AND me1.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("JOIN HIS_MATERIAL ME1 ON ME1.ID=EXMM.MATERIAL_ID\n");
                query += string.Format("AND me1.BID_ID ={0}\n", filter.BID_ID);
            }
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join his_treatment trea on exmm.tdl_treatment_id = trea.id \n";
            //if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                query += string.Format("LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EM.IMP_MEDI_STOCK_ID\n");
            }
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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
            if (filter.ADD_IMP_LOCAL == true)
            {
                query += string.Format("LEFT JOIN HIS_IMP_MEST chmsImp ON chmsImp.CHMS_EXP_MEST_ID=EM.ID and chmsImp.IMP_MEST_STT_ID = {0} and chmsImp.IMP_TIME BETWEEN {1} and {2} and chmsImp.MEDI_STOCK_ID IN ({3})\n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT, timeFrom, timeTo, string.Join(",", mediStockIds));
            }

            query += "WHERE 1=1  \n";
            query += "and EXMM.IS_EXPORT=1  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.EXP_MEST_ID IS NOT NULL \n";
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            FilterMaty(ref query, "exmm.TDL_MATERIAL_TYPE_ID", filter);

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "EXMM.TDL_MATERIAL_TYPE_ID, ";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end),\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);
            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);

            //query += string.Format("(case when  em.exp_mest_type_id = {17} then nvl(exmm.PRICE,0) else 0 end) ,\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN);
            //query += string.Format("(case when  em.exp_mest_type_id = {17} then nvl(exmm.VAT_RATIO,0) else 0 end) ,\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN);


            query += "EXMM.MATERIAL_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<Mrs00105RDO> GetMediExp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string medicineTypeCodes = filter.MEDICINE_TYPE_CODEs;
            bool? AddExpAmountBhyt = filter.ADD_EXP_AMOUNT_BHYT;
            string MEDI_STOCK_CODE__NTs = filter.MEDI_STOCK_CODE__NTs;
            string MEDI_STOCK_CODE__IMPs = filter.MEDI_STOCK_CODE__IMPs;
            string REQ_DEPARTMENT_CODE__IMPs = filter.REQ_DEPARTMENT_CODE__IMPs;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_MEDICINE != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", THUOC);
            query += "max(exmm.exp_time) LAST_EXP_TIME_NUM, \n";
            query += "max(exmm.patient_type_id) keep(dense_rank last order by (case when exmm.patient_type_id is null then 0 else exmm.exp_mest_id end)) PATIENT_TYPE_ID, \n";
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_MEDICINE_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end) AS REQ_DEPARTMENT_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end) AS exp_mest_type_id,\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end) AS exp_mest_reason_id,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);

            //neu cho phep them truong xuat bhyt va xuat sau timefrom thi tinh so luong xuat cho doi tuong bao hiem y te
            if (AddExpAmountBhyt == true)
            {
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id = {1} then nvl(exmm.amount,0) else 0 end) AS BHYT_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id <> {1} then nvl(exmm.amount,0) else 0 end) AS VP_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            }

            //neu cho phep them truong xuat nhap noi bo va xuat sau timefrom thi tinh so luong xuat cho doi tuong bao hiem y te
            if (filter.ADD_IMP_LOCAL == true)
            {
                query += string.Format("sum(case when exmm.exp_time>{0} and chmsImp.id is not null then nvl(exmm.amount,0) else 0 end) AS LOCAL_EXP_TOTAL_AMOUNT,\n", timeFrom);
            }
            //xuat cho benh nhan dieu tri noi tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} then nvl(EXMM.AMOUNT,0) else 0 end) AS EXP_NOI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri ngoai tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} then nvl(EXMM.AMOUNT,0) else 0 end) AS EXP_NGOAI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri noi tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} and exmm.patient_type_id={2} then EXMM.AMOUNT else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //xuat cho benh nhan dieu tri ngoai tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} and exmm.patient_type_id={2} then EXMM.AMOUNT else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NGT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //neu danh sach kho noi tru khac null va xuat sau timefrom thi tinh so luong xuat cho kho noi tru
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__NTs))
            {
                var mediStockIdNts = HisMediStockCFG.HisMediStocks.Where(o => string.Format(",{0},", MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", o.MEDI_STOCK_CODE))).Select(p => p.ID).ToList() ?? new List<long>();
                if (mediStockIdNts.Count > 0)
                    query += string.Format("sum(case when exmm.exp_time>{0} and em.imp_medi_stock_id in ({1}) then nvl(exmm.amount,0) else 0 end) AS KNT_EXP_TOTAL_AMOUNT,\n", timeFrom, string.Join(",", mediStockIdNts));
            }
            //neu danh sach kho nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac kho nhan
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                var mediStockCodeImps = MEDI_STOCK_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeImps.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and imps.medi_stock_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                }
            }
            query += "'{'" + strDicImp + "||'}'  AS JSON_IMP_MEDI_STOCK,\n";

            //neu danh sach khoa nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac khoa nhan
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__IMPs))
            {
                var reqDepartmentCodeImps = REQ_DEPARTMENT_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeImps.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_BHYT == true)
                    {
                        strDicImpDepa += "||','"+string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_BHYT\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} and exmm.is_expend is null then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)));

                    }
                    if (filter.IS_IMP_DEPA_VP == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_VP\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} and exmm.is_expend is null then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)));

                    }
                    if (filter.IS_IMP_DEPA_HP == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_HP\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.is_expend = 1 then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));

                    }
                }
            }
            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_IMP_REQ_DEPARTMENT,\n";
            //neu xuat truoc timeFrom thi tru vao ton dau
            query += string.Format("SUM(case when exmm.exp_time<={0} then -EXMM.AMOUNT else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then EXMM.AMOUNT else 0 end) AS EXP_TOTAL_AMOUNT,\n", timeFrom);
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-EXMM.AMOUNT) AS END_AMOUNT,\n");

            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là HTCS thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS CABIN_HTCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);

            query += string.Format("(case when  em.exp_mest_type_id = {0} then nvl(exmm.PRICE,0) else 0 end) AS VACCIN_PRICE,\n",IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN) ;
            query += string.Format("(case when  em.exp_mest_type_id = {0} then nvl(exmm.VAT_RATIO,0) else 0 end) AS VACCIN_VAT_RATIO,\n", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__VACCIN);

            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end) AS SALE_PRICE,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end) AS SALE_VAT_RATIO,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1}  then EXMM.AMOUNT * nvl(EXMM.PRICE,0) * (1 + nvl(EXMM.VAT_RATIO,0)) else 0 end) AS SALE_TOTAL_PRICE\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);


            

            query += "FROM HIS_EXP_MEST_MEDICINE EXMM \n";
            if (!string.IsNullOrWhiteSpace(medicineTypeCodes))
            {
                query += string.Format("JOIN HIS_MEDICINE ME ON ME.ID=EXMM.MEDICINE_ID\n JOIN HIS_MEDICINE_TYPE MT ON MT.ID=ME.MEDICINE_TYPE_ID AND MT.MEDICINE_TYPE_CODE in ('{0}')\n", medicineTypeCodes.Replace(",", "','"));
            }

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=EXMM.MEDICINE_ID\n");
                query += string.Format("AND me1.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=EXMM.MEDICINE_ID\n");
                query += string.Format("AND ME1.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=EXMM.MEDICINE_ID\n");
                query += string.Format("AND ME1.BID_ID ={0}\n", filter.BID_ID);
            }
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join his_treatment trea on exmm.tdl_treatment_id = trea.id \n";
            //if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                query += string.Format("LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EM.IMP_MEDI_STOCK_ID\n");
            }
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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
            if (filter.ADD_IMP_LOCAL == true)
            {
                query += string.Format("LEFT JOIN HIS_IMP_MEST chmsImp ON chmsImp.CHMS_EXP_MEST_ID=EM.ID and chmsImp.IMP_MEST_STT_ID = {0} and chmsImp.IMP_TIME BETWEEN {1} and {2} and chmsImp.MEDI_STOCK_ID IN ({3})\n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT, timeFrom, timeTo, string.Join(",", mediStockIds));
            }

            query += "WHERE 1=1  \n";
            query += "and EXMM.IS_EXPORT=1  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.EXP_MEST_ID IS NOT NULL \n";
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            FilterMety(ref query, "exmm.TDL_MEDICINE_TYPE_ID",filter);
            
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "EXMM.TDL_MEDICINE_TYPE_ID, ";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end),\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);
            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);

            query += string.Format("(case when  em.exp_mest_type_id = 17 then nvl(exmm.PRICE,0) else 0 end) ,\n");
            query += string.Format("(case when   em.exp_mest_type_id = 17 then nvl(exmm.VAT_RATIO,0) else 0 end),\n");

            query += "EXMM.MEDICINE_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        private void FilterMety(ref string query, string filterMetyStr, Mrs00105Filter filter)
        {
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("AND {1} in ({0})\n", string.Join(",", filter.MEDICINE_TYPE_IDs), filterMetyStr);
            }
            if (filter.MEDICINE_GROUP_IDs != null)
            {
                query += string.Format("AND {1} in (select id from his_medicine_type where medicine_group_id in ({0}))\n", string.Join(",", filter.MEDICINE_GROUP_IDs), filterMetyStr);
            }
            if (filter.HEIN_SERVICE_TYPE_IDs != null)
            {
                query += string.Format("AND {1} in (select id from v_his_medicine_type where HEIN_SERVICE_TYPE_ID in ({0}))\n", string.Join(",", filter.HEIN_SERVICE_TYPE_IDs), filterMetyStr);
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND {1} in (select id from his_medicine_type where parent_id in ((select id from his_medicine_type where service_id in ({0}))))\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs), filterMetyStr);
            }
        }

        private void FilterMaty(ref string query, string filterMatyStr, Mrs00105Filter filter)
        {
            if (filter.MATERIAL_TYPE_IDs != null)
            {
                query += string.Format("AND {1} in ({0})\n", string.Join(",", filter.MATERIAL_TYPE_IDs), filterMatyStr);
            }
            if (filter.MEDICINE_GROUP_IDs != null)
            {
                query += string.Format("AND 1=0 \n");
            }
            if (filter.HEIN_SERVICE_TYPE_IDs != null)
            {
                query += string.Format("AND {1} in (select id from v_his_material_type where HEIN_SERVICE_TYPE_ID in ({0}))\n", string.Join(",", filter.HEIN_SERVICE_TYPE_IDs), filterMatyStr);
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND {1} in (select id from his_material_type where parent_id in ((select id from his_material_type where service_id in ({0}))))\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs), filterMatyStr);
            }
        }

        public List<Mrs00105RDO> GetBloodExp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string bloodTypeCodes = filter.BLOOD_TYPE_CODEs;
            bool? AddExpAmountBhyt = filter.ADD_EXP_AMOUNT_BHYT;
            string MEDI_STOCK_CODE__NTs = filter.MEDI_STOCK_CODE__NTs;
            string MEDI_STOCK_CODE__IMPs = filter.MEDI_STOCK_CODE__IMPs;
            string REQ_DEPARTMENT_CODE__IMPs = filter.REQ_DEPARTMENT_CODE__IMPs;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_BLOOD != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach xuat tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", MAU);
            query += "max(exmm.exp_time) LAST_EXP_TIME_NUM, \n";
            query += "max(exmm.patient_type_id) keep(dense_rank last order by (case when exmm.patient_type_id is null then 0 else exmm.exp_mest_id end)) PATIENT_TYPE_ID, \n";
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.BLOOD_ID,0) AS MEDI_MATE_ID, \n";
            query += "exmm.TDL_BLOOD_TYPE_ID MEDI_MATE_TYPE_ID,\n";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end) AS REQ_DEPARTMENT_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end) AS exp_mest_type_id,\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end) AS exp_mest_reason_id,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);

            //neu cho phep them truong xuat bhyt va xuat sau timefrom thi tinh so luong xuat cho doi tuong bao hiem y te
            if (AddExpAmountBhyt == true)
            {
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id = {1} then 1 else 0 end) AS BHYT_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when exmm.exp_time>{0} and exmm.patient_type_id <> {1} then 1 else 0 end) AS VP_EXP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            }
            //xuat cho benh nhan dieu tri noi tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} then 1 else 0 end) AS EXP_NOI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri ngoai tru
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} then 1 else 0 end) AS EXP_NGOAI_TRU_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
            //xuat cho benh nhan dieu tri noi tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID = {1} and exmm.patient_type_id={2} then 1 else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //xuat cho benh nhan dieu tri ngoai tru bảo hiểm
            query += string.Format("SUM(case when exmm.exp_time>{0} and trea.TDL_TREATMENT_TYPE_ID <> {1} and exmm.patient_type_id={2} then 1 else 0 end) AS BHYT_EXP_TOTAL_AMOUNT_NGT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
            //neu danh sach kho noi tru khac null va xuat sau timefrom thi tinh so luong xuat cho kho noi tru
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__NTs))
            {
                var mediStockIdNts = HisMediStockCFG.HisMediStocks.Where(o => string.Format(",{0},", MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", o.MEDI_STOCK_CODE))).Select(p => p.ID).ToList() ?? new List<long>();
                if (mediStockIdNts.Count > 0)
                    query += string.Format("sum(case when exmm.exp_time>{0} and em.imp_medi_stock_id in ({1}) then 1 else 0 end) AS KNT_EXP_TOTAL_AMOUNT,\n", timeFrom, string.Join(",", mediStockIdNts));
            }
            //neu danh sach kho nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac kho nhan
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                var mediStockCodeImps = MEDI_STOCK_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeImps.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and imps.medi_stock_code ='{1}' then 10000 else 0 end)", timeFrom, o)));
                }
            }
            query += "'{'" + strDicImp + "||'}'  AS JSON_IMP_MEDI_STOCK,\n";

            //neu danh sach khoa nhan khac null va nhap sau timefrom thi tinh so luong xuat theo cac khoa nhan
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__IMPs))
            {
                var reqDepartmentCodeImps = REQ_DEPARTMENT_CODE__IMPs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeImps.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' then 10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_BHYT == true)
                    {
                        strDicImpDepa += "||','"+string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_BHYT\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} then 10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)));

                    }
                    if (filter.IS_IMP_DEPA_VP == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeImps.Select(o => string.Format("||'\"{1}_VP\":'||sum(case when exmm.exp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and exmm.patient_type_id={2} then 10000 else 0 end)", timeFrom, o, HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)));

                    }
                }
            }
            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_IMP_REQ_DEPARTMENT,\n";
            //neu xuat truoc timeFrom thi tru vao ton dau
            query += string.Format("SUM(case when exmm.exp_time<={0} then -1 else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu xuat sau timefrom thi cong vao xuat
            query += string.Format("SUM(case when exmm.exp_time>{0} then 1 else 0 end) AS EXP_TOTAL_AMOUNT,\n", timeFrom);
            //neu xuat thi luon tru vao ton cuoi
            query += string.Format("SUM(-1) AS END_AMOUNT,\n");

            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then 1 else 0 end) AS BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là HTCS thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then 1 else 0 end) AS CABIN_HTCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end) AS SALE_PRICE,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end) AS SALE_VAT_RATIO,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("SUM(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1}  then nvl(EXMM.PRICE,0) * (1 + nvl(EXMM.VAT_RATIO,0)) else 0 end) AS SALE_TOTAL_PRICE\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);

            query += "FROM HIS_EXP_MEST_BLOOD EXMM \n";
            if (!string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("JOIN HIS_BLOOD ME ON ME.ID=EXMM.BLOOD_ID\n JOIN HIS_BLOOD_TYPE MT ON MT.ID=ME.BLOOD_TYPE_ID AND MT.BLOOD_TYPE_CODE in ('{0}')\n", bloodTypeCodes.Replace(",", "','"));
            }

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("JOIN HIS_BLOOD ME1 ON ME1.ID=EXMM.BLOOD_ID\n");
                query += string.Format("AND me1.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("JOIN HIS_BLOOD ME1 ON ME1.ID=EXMM.BLOOD_ID\n");
                query += string.Format("AND ME1.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("JOIN HIS_BLOOD ME1 ON ME1.ID=EXMM.BLOOD_ID\n");
                query += string.Format("AND ME1.BID_ID ={0}\n", filter.BID_ID);
            }
            query += "join his_exp_mest em on em.id=exmm.exp_mest_id \n";
            query += "left join his_treatment trea on exmm.tdl_treatment_id = trea.id \n";
            //if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__IMPs))
            {
                query += string.Format("LEFT JOIN V_HIS_MEDI_STOCK IMPS ON IMPS.ID=EM.IMP_MEDI_STOCK_ID\n");
            }
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("AND exmm.TDL_BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }
            //khong co chot ky hoac xuat sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<exmm.exp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND EXMM.EXP_TIME < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "EXMM.TDL_BLOOD_TYPE_ID, ";
            //neu xuat sau timefrom thi can tach so luong theo khoa yeu cau
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id in ({1},{2},{3}) then nvl(IMPS.department_id,0) when exmm.exp_time>{0} then nvl(em.req_department_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);
            //neu xuat sau timefrom thi can tach so luong theo loai xuat
            query += string.Format("(case when exmm.exp_time>{0} then nvl(em.exp_mest_type_id,0) else null end),\n", timeFrom);
            //neu xuat sau timefrom va loai xuat la xuat khac thi can tach so luong theo li do xuat
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(em.exp_mest_reason_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);
            //neu xuat sau timefrom va loai xuat chuyen kho là htcs tach so luong theo tung kho cha
            query += string.Format("(case when exmm.exp_time>{0} and em.CHMS_TYPE_ID = {1} then nvl(em.imp_medi_stock_id,0) else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);
            //neu xuat sau timefrom va loai xuat la xuat ban thi tach lo theo gia ban, vat va tinh tong tien ban
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.PRICE,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += string.Format("(case when exmm.exp_time>{0} and em.exp_mest_type_id = {1} then nvl(exmm.VAT_RATIO,0) else 0 end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);
            query += "EXMM.BLOOD_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<Mrs00105RDO> GetMateImp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string materialTypeCodes = filter.MATERIAL_TYPE_CODEs;
            string MEDI_STOCK_CODE__CHMSs = filter.MEDI_STOCK_CODE__CHMSs;
            string REQ_DEPARTMENT_CODE__CHMSs = filter.REQ_DEPARTMENT_CODE__CHMSs;
            bool? AddImpAmountBhyt = filter.ADD_IMP_AMOUNT_BHYT;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();

            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", VATTU);
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, \n";
            query += "ma.MATERIAL_TYPE_ID MEDI_MATE_TYPE_ID,\n";

            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end) AS imp_mest_type_id,\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);

            //neu danh sach kho chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac kho chuyen
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                var mediStockCodeChms = MEDI_STOCK_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeChms.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and chms.medi_stock_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                }
            }

            query += "'{'" + strDicImp + "||'}'  AS JSON_CHMS_MEDI_STOCK,\n";

            //neu danh sach khoa chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac khoa chuyen
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__CHMSs))
            {
                var reqDepartmentCodeChms = REQ_DEPARTMENT_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeChms.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and dp.department_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_TL == true)
                    {
                        strDicImpDepa += "||','"+string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}_TL\":'||sum(case when em.imp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and em.imp_mest_type_id not in (2,6) then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));

                    }
                }
            }

            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_CHMS_REQ_DEPARTMENT,\n";
            //neu nhap truoc timeFrom thi cong vao ton dau
            query += string.Format("SUM(case when em.imp_time<={0} then EXMM.AMOUNT else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then EXMM.AMOUNT else 0 end) AS IMP_TOTAL_AMOUNT,\n", timeFrom);

            //neu cho phep them truong nhap bhyt va nhap sau timefrom thi tinh so luong nhap tu doi tuong bao hiem y te
            if (AddImpAmountBhyt == true)
            {
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id = {1} then nvl(exmm.amount,0) else 0 end) AS BHYT_IMP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id <> {1} then nvl(exmm.amount,0) else 0 end) AS VP_IMP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id = {1} and em.imp_mest_type_id ={2} then nvl(exmm.amount,0) else 0 end) AS BHYT_DTTTL_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id <> {1} and em.imp_mest_type_id ={2} then nvl(exmm.amount,0) else 0 end) AS VP_DTTTL_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
            }
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(EXMM.AMOUNT) AS END_AMOUNT,\n");

            //neu nhap sau timefrom va loai nhap chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS CABIN_BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS HTCS_AMOUNT\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);

            query += "FROM HIS_IMP_MEST_MATERIAL EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_material ma on ma.id=exmm.material_id \n";
            if (!string.IsNullOrWhiteSpace(materialTypeCodes))
            {
                query += string.Format("JOIN HIS_MATERIAL_TYPE MT ON MT.ID=MA.MATERIAL_TYPE_ID AND MT.MATERIAL_TYPE_CODE in ('{0}')\n", materialTypeCodes.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                query += string.Format("LEFT JOIN HIS_MEDI_STOCK CHMS ON CHMS.ID=EM.CHMS_MEDI_STOCK_ID\n");
            }
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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

            //neu cho phep them truong nhap bhyt va nhap sau timefrom thi tinh so luong nhap tu doi tuong bao hiem y te
            if (AddImpAmountBhyt == true)
            {
                query += string.Format("LEFT JOIN his_exp_mest_material thexp ON thexp.ID=exmm.th_exp_mest_material_id\n");
            }

            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.IMP_MEST_ID IS NOT NULL \n";

            FilterMaty(ref query, "ma.MATERIAL_TYPE_ID", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND ma.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND ma.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND ma.BID_ID ={0}\n", filter.BID_ID);
            }
            //khong co chot ky hoac nhap sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<em.imp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "ma.MATERIAL_TYPE_ID, ";
            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end),\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);
            query += "EXMM.MATERIAL_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<Mrs00105RDO> GetMediImp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string medicineTypeCodes = filter.MEDICINE_TYPE_CODEs;
            string MEDI_STOCK_CODE__CHMSs = filter.MEDI_STOCK_CODE__CHMSs;
            string REQ_DEPARTMENT_CODE__CHMSs = filter.REQ_DEPARTMENT_CODE__CHMSs;
            bool? AddImpAmountBhyt = filter.ADD_IMP_AMOUNT_BHYT;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            if (filter.IS_MEDICINE != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", THUOC);
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, \n";
            query += "me.MEDICINE_TYPE_ID MEDI_MATE_TYPE_ID,\n";

            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end) AS imp_mest_type_id,\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);
            //neu danh sach kho chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac kho chuyen
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                var mediStockCodeChms = MEDI_STOCK_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeChms.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and chms.medi_stock_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                }
            }

            query += "'{'" + strDicImp + "||'}'  AS JSON_CHMS_MEDI_STOCK,\n";

            //neu danh sach khoa chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac khoa chuyen
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__CHMSs))
            {
                var reqDepartmentCodeChms = REQ_DEPARTMENT_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeChms.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and dp.department_code ='{1}' then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_TL == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}_TL\":'||sum(case when em.imp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and em.imp_mest_type_id not in (2,6) then nvl(exmm.amount,0)*10000 else 0 end)", timeFrom, o)));

                    }
                }
            }

            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_CHMS_REQ_DEPARTMENT,\n";
            //neu nhap truoc timeFrom thi cong vao ton dau
            query += string.Format("SUM(case when em.imp_time<={0} then EXMM.AMOUNT else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then EXMM.AMOUNT else 0 end) AS IMP_TOTAL_AMOUNT,\n", timeFrom);

            //neu cho phep them truong nhap bhyt va nhap sau timefrom thi tinh so luong nhap tu doi tuong bao hiem y te
            if (AddImpAmountBhyt == true)
            {
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id = {1} then nvl(exmm.amount,0) else 0 end) AS BHYT_IMP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id <> {1} then nvl(exmm.amount,0) else 0 end) AS VP_IMP_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id = {1} and em.imp_mest_type_id ={2} then nvl(exmm.amount,0) else 0 end) AS BHYT_DTTTL_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
                query += string.Format("sum(case when em.imp_time>{0} and thexp.patient_type_id <> {1} and em.imp_mest_type_id ={2} then nvl(exmm.amount,0) else 0 end) AS VP_DTTTL_TOTAL_AMOUNT,\n", timeFrom, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
            }
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(EXMM.AMOUNT) AS END_AMOUNT,\n");

            //neu nhap sau timefrom va loai nhap chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS CABIN_BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then exmm.amount else 0 end) AS HTCS_AMOUNT\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);

            query += "FROM HIS_IMP_MEST_MEDICINE EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_medicine me on me.id=exmm.medicine_id \n";

            if (!string.IsNullOrWhiteSpace(medicineTypeCodes))
            {
                query += string.Format("JOIN HIS_MEDICINE_TYPE MT ON MT.ID=ME.MEDICINE_TYPE_ID AND MT.MEDICINE_TYPE_CODE in ('{0}')\n", medicineTypeCodes.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                query += string.Format("LEFT JOIN HIS_MEDI_STOCK CHMS ON CHMS.ID=EM.CHMS_MEDI_STOCK_ID\n");
            }
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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

            //neu cho phep them truong nhap bhyt va nhap sau timefrom thi tinh so luong nhap tu doi tuong bao hiem y te
            if (AddImpAmountBhyt == true)
            {
                query += string.Format("LEFT JOIN his_exp_mest_medicine thexp ON thexp.ID=exmm.th_exp_mest_medicine_id\n");
            }

            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND EXMM.IS_DELETE =0  \n";
            query += "AND EXMM.IMP_MEST_ID IS NOT NULL \n";
            //khong co chot ky hoac nhap sau khi chot ky
            query += "AND (last_msp.to_time is null or last_msp.to_time<em.imp_time) \n";
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));

            FilterMety(ref query, "me.medicine_type_id", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND me.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND me.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }

            if (filter.BID_ID != null)
            {
                query += string.Format("AND me.BID_ID={0}\n", filter.BID_ID);
            }
            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "me.MEDICINE_TYPE_ID, ";
            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end),\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);
            query += "EXMM.MEDICINE_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<Mrs00105RDO> GetBloodImp(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
            string bloodTypeCodes = filter.BLOOD_TYPE_CODEs;
            string MEDI_STOCK_CODE__CHMSs = filter.MEDI_STOCK_CODE__CHMSs;
            string REQ_DEPARTMENT_CODE__CHMSs = filter.REQ_DEPARTMENT_CODE__CHMSs;
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();

            if (filter.IS_BLOOD != true) return result;
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach nhap tu khi chot ky den thoi diem timeTo\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", MAU);
            query += "Em.MEDI_STOCK_ID AS MEDI_STOCK_ID, \n";
            query += "NVL(EXMM.BLOOD_ID,0) AS MEDI_MATE_ID, \n";
            query += "me.BLOOD_TYPE_ID MEDI_MATE_TYPE_ID,\n";

            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end) AS imp_mest_type_id,\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end) AS PREVIOUS_MEDI_STOCK_ID,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);
            //neu danh sach kho chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac kho chuyen
            string strDicImp = "";
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                var mediStockCodeChms = MEDI_STOCK_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (mediStockCodeChms.Count > 0)
                {
                    strDicImp = string.Join("||','", mediStockCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and chms.medi_stock_code ='{1}' then 10000 else 0 end)", timeFrom, o)));
                }
            }

            query += "'{'" + strDicImp + "||'}'  AS JSON_CHMS_MEDI_STOCK,\n";

            //neu danh sach khoa chuyen khac null va nhap sau timefrom thi tinh so luong nhap theo cac khoa chuyen
            string strDicImpDepa = "";
            if (!string.IsNullOrWhiteSpace(REQ_DEPARTMENT_CODE__CHMSs))
            {
                var reqDepartmentCodeChms = REQ_DEPARTMENT_CODE__CHMSs.Split(',').ToList() ?? new List<string>();
                if (reqDepartmentCodeChms.Count > 0)
                {
                    strDicImpDepa = string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}\":'||sum(case when em.imp_time>{0} and dp.department_code ='{1}' then 10000 else 0 end)", timeFrom, o)));
                    if (filter.IS_IMP_DEPA_TL == true)
                    {
                        strDicImpDepa += "||','" + string.Join("||','", reqDepartmentCodeChms.Select(o => string.Format("||'\"{1}_TL\":'||sum(case when em.imp_time>{0} and DP.DEPARTMENT_CODE ='{1}' and em.imp_mest_type_id not in (2,6) then 10000 else 0 end)", timeFrom, o)));

                    }
                }
            }

            query += "'{'" + strDicImpDepa + "||'}'  AS JSON_CHMS_REQ_DEPARTMENT,\n";
            //neu nhap truoc timeFrom thi cong vao ton dau
            query += string.Format("SUM(case when em.imp_time<={0} then 1 else 0 end) AS BEGIN_AMOUNT,\n", timeFrom);
            //neu nhap sau timefrom thi cong vao nhap
            query += string.Format("SUM(case when em.imp_time>{0} then 1 else 0 end) AS IMP_TOTAL_AMOUNT,\n", timeFrom);
            //neu nhap thi luon cong vao ton cuoi
            query += string.Format("SUM(1) AS END_AMOUNT,\n");

            //neu nhap sau timefrom va loai nhap chuyen kho là bscs thi tinh vao BSCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then 1 else 0 end) AS CABIN_BSCS_AMOUNT,\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__ADDITION);
            //neu xuat sau timefrom va loai xuat chuyen kho là bscs thi tinh vao HTCS_AMOUNT
            query += string.Format("sum(case when em.imp_time>{0} and em.CHMS_TYPE_ID = {1} then 1 else 0 end) AS HTCS_AMOUNT\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST.CHMS_TYPE__ID__REDUCTION);

            query += "FROM HIS_IMP_MEST_BLOOD EXMM \n";
            query += "join his_imp_mest em on em.id=exmm.imp_mest_id \n";
            query += "join his_blood me on me.id=exmm.blood_id \n";

            if (!string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("JOIN HIS_BLOOD_TYPE MT ON MT.ID=ME.BLOOD_TYPE_ID AND MT.BLOOD_TYPE_CODE in ('{0}')\n", bloodTypeCodes.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(MEDI_STOCK_CODE__CHMSs))
            {
                query += string.Format("LEFT JOIN HIS_MEDI_STOCK CHMS ON CHMS.ID=EM.CHMS_MEDI_STOCK_ID\n");
            }
            query += "join his_medi_stock ms on ms.id=em.medi_stock_id \n";
            query += string.Format("LEFT JOIN HIS_DEPARTMENT DP ON DP.ID=EM.REQ_DEPARTMENT_ID\n");
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

            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("AND me.BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND me.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND me.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }

            if (filter.BID_ID != null)
            {
                query += string.Format("AND me.BID_ID={0}\n", filter.BID_ID);
            }
            query += string.Format("AND em.imp_time < {0} ", timeTo);

            query += "GROUP BY ";
            query += "Em.MEDI_STOCK_ID, ";
            query += "me.BLOOD_TYPE_ID, ";
            //neu nhap sau timefrom thi can tach so luong theo loai nhap
            query += string.Format("(case when em.imp_time>{0} then nvl(em.imp_mest_type_id,0) else null end),\n", timeFrom);
            //neu nhap sau timefrom va loai nhap chuyen kho va kho nhap la tu truc thi can tach theo kho cha
            query += string.Format("(case when em.imp_time>{0} and em.imp_mest_type_id in({1},{2}) and ms.is_cabinet=1 then em.chms_medi_stock_id else null end),\n", timeFrom, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);
            query += "EXMM.BLOOD_ID ";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        public List<V_HIS_IMP_MEST_MEDICINE> GetMediInput(Mrs00105Filter filter, List<long> mediStockIds)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            if (filter.IS_MEDICINE != true) return result;
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach nhap thuoc dau vao kiem ke\n";
            query += "SELECT \n";
            query += "IMMM.* \n";

            query += "FROM V_HIS_IMP_MEST_MEDICINE IMMM \n";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 \n";

            FilterMety(ref query, "immm.medicine_type_id", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND immm.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND immm.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND immm.BID_ID ={0}\n", filter.BID_ID);
            }
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} \n", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        public List<V_HIS_IMP_MEST_MATERIAL> GetMateInput(Mrs00105Filter filter, List<long> mediStockIds)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            CommonParam paramGet = new CommonParam();
            string query = " -- danh sach nhap vat tu dau vao kiem ke\n";
            query += "SELECT \n";
            query += "IMMM.* \n";

            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMM \n";

            query += "WHERE IMMM.IMP_MEST_STT_ID =2 AND IMMM.IMP_MEST_TYPE_ID = 5 \n";

            FilterMaty(ref query, "immm.MATERIAL_TYPE_ID", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND IMMM.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND IMMM.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND IMMM.BID_ID ={0}\n", filter.BID_ID);
            }
            query += string.Format("AND IMMM.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));


            if (filter.TIME_FROM != null)
            {
                query += string.Format("AND IMMM.CREATE_TIME >={0} \n", filter.TIME_FROM);
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<MediStockMetyMaty> GetMediStockMety(Mrs00105Filter filter, List<long> mediStockIds)
        {

            List<MediStockMetyMaty> result = new List<MediStockMetyMaty>();
            if (filter.IS_MEDICINE != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --thong tin kho xuat hien tai\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", THUOC);
            query += "MEDICINE_TYPE_ID MEDI_MATE_TYPE_ID, \n";
            query += "MEDI_STOCK_ID, \n";
            query += "EXP_MEDI_STOCK_ID \n";
            query += "FROM HIS_MEDI_STOCK_METY \n";
            query += "WHERE 1 = 1\n";
            query += "and IS_DELETE=0\n";

            query += string.Format("AND MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));

            FilterMety(ref query, "medicine_type_id", filter);

            if (filter.EXP_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXP_MEDI_STOCK_ID in ({0})\n", string.Join(",", filter.EXP_MEDI_STOCK_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MediStockMetyMaty>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<MediStockMetyMaty> GetMediStockMaty(Mrs00105Filter filter, List<long> mediStockIds)
        {

            List<MediStockMetyMaty> result = new List<MediStockMetyMaty>();
            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --thong tin kho xuat hien tai\n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", VATTU);
            query += "MATERIAL_TYPE_ID MEDI_MATE_TYPE_ID, \n";
            query += "MEDI_STOCK_ID, \n";
            query += "EXP_MEDI_STOCK_ID \n";
            query += "FROM HIS_MEDI_STOCK_MATY \n";
            query += "WHERE 1 = 1\n";
            query += "and IS_DELETE=0\n";

            query += string.Format("AND MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));

            FilterMaty(ref query, "MATERIAL_TYPE_ID", filter);

            if (filter.EXP_MEDI_STOCK_IDs != null)
            {
                query += string.Format("AND EXP_MEDI_STOCK_ID in ({0})\n", string.Join(",", filter.EXP_MEDI_STOCK_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MediStockMetyMaty>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }


        internal List<MediMateIdChmsMediStockId> GetChmsMediStockId(List<long> mediStockIds, List<long> parentMediStockIds, long timeFrom, long timeTo, string medicineTypeCodes, string materialTypeCodes, string bloodTypeCodes, Mrs00105Filter filter)
        {

            List<MediMateIdChmsMediStockId> result = new List<MediMateIdChmsMediStockId>();
            if (filter.IS_MEDICINE != true && filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            CommonParam paramGet = new CommonParam();
            string query = " --thong tin kho xuat va thoi gian nhap chuyen kho tu truoc time_to\n";
            query += "SELECT \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end) IS_ON, \n", timeFrom);
            query += "IMME.MEDICINE_ID MEDI_MATE_ID, \n";
            query += "max(im.imp_time) max_imp_time, \n";
            query += "1 TYPE_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID CHMS_MEDI_STOCK_ID \n";
            query += "FROM HIS_IMP_MEST_MEDICINE IMME \n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMME.IMP_MEST_ID \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=IM.CHMS_EXP_MEST_ID \n";
            query += "JOIN HIS_MEDI_STOCK MS ON MS.ID=IM.MEDI_STOCK_ID \n";

            if (!string.IsNullOrWhiteSpace(medicineTypeCodes) || !string.IsNullOrWhiteSpace(materialTypeCodes) || !string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("JOIN HIS_MEDICINE ME ON ME.ID=IMME.MEDICINE_ID\n JOIN HIS_MEDICINE_TYPE MT ON MT.ID=ME.MEDICINE_TYPE_ID AND MT.MEDICINE_TYPE_CODE in ('{0}')\n", (medicineTypeCodes ?? "").Replace(",", "','"));
            }

            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME ON ME.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME.MEDICINE_TYPE_ID in ({0})\n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDICINE_GROUP_IDs != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME ON ME.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME.MEDICINE_TYPE_ID in (select id from his_medicine_type where medicine_group_id in ({0}))\n", string.Join(",", filter.MEDICINE_GROUP_IDs));
            }

            if (filter.HEIN_SERVICE_TYPE_IDs != null)
            { 
                query += string.Format("JOIN HIS_MEDICINE ME ON ME.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME.MEDICINE_TYPE_ID in (select id from v_his_medicine_type where hein_service_type_id in ({0}))\n", string.Join(",", filter.HEIN_SERVICE_TYPE_IDs));
            
            }

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME1.BID_ID is not null\n");
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME1.BID_ID ={0}\n", filter.BID_ID);
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("JOIN HIS_MEDICINE ME1 ON ME1.ID=IMME.MEDICINE_ID\n");
                query += string.Format("AND ME1.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            query += "WHERE 1 = 1\n";
            query += "and IM.IMP_MEST_STT_ID=5\n";
            query += "and IM.IS_DELETE=0\n";
            query += "and MS.IS_CABINET = 1\n";
            query += string.Format("AND IM.IMP_MEST_TYPE_ID IN({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);

            query += string.Format("AND IM.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));

            if (parentMediStockIds != null && parentMediStockIds.Count > 0)
            {
                query += string.Format("AND EX.MEDI_STOCK_ID in ({0})\n", string.Join(",", parentMediStockIds));
            }
            query += string.Format("AND IM.IMP_TIME <{0}\n", timeTo);
            query += "GROUP BY \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end), \n", timeFrom);
            query += "IMME.MEDICINE_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID \n";
            query += "UNION ALL \n";
            query += "SELECT \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end) IS_ON, \n", timeFrom);
            query += "IMMA.MATERIAL_ID MEDI_MATE_ID, \n";
            query += "max(im.imp_time) max_imp_time, \n";
            query += "2 TYPE_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID CHMS_MEDI_STOCK_ID \n";
            query += "FROM V_HIS_IMP_MEST_MATERIAL IMMA \n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMMA.IMP_MEST_ID \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=IM.CHMS_EXP_MEST_ID \n";
            query += "JOIN HIS_MEDI_STOCK MS ON MS.ID=IM.MEDI_STOCK_ID  \n";

            if (!string.IsNullOrWhiteSpace(medicineTypeCodes) || !string.IsNullOrWhiteSpace(materialTypeCodes) || !string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("JOIN HIS_MATERIAL ME ON ME.ID=IMMA.MATERIAL_ID\n JOIN HIS_MATERIAL_TYPE MT ON MT.ID=ME.MATERIAL_TYPE_ID AND MT.MATERIAL_TYPE_CODE in ('{0}')\n", (materialTypeCodes ?? "").Replace(",", "','"));
            }

            FilterMaty(ref query, "imma.MATERIAL_TYPE_ID", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND IMMA.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND IMMA.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND IMMA.BID_ID={0}\n", filter.BID_ID);
            }
            query += "WHERE 1 = 1\n";
            query += "and IM.IMP_MEST_STT_ID=5\n";
            query += "and IM.IS_DELETE=0\n";
            query += "and MS.IS_CABINET = 1\n";
            query += string.Format("AND IM.IMP_MEST_TYPE_ID IN({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);

            query += string.Format("AND IM.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));
            if (parentMediStockIds != null && parentMediStockIds.Count > 0)
            {
                query += string.Format("AND EX.MEDI_STOCK_ID in ({0})\n", string.Join(",", parentMediStockIds));
            }
            query += string.Format("AND IM.IMP_TIME <{0}\n", timeTo);
            query += "GROUP BY \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end), \n", timeFrom);
            query += "IMMA.MATERIAL_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID \n";
            query += "UNION ALL \n";
            query += "SELECT \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end) IS_ON, \n", timeFrom);
            query += "IMBL.BLOOD_ID MEDI_MATE_ID, \n";
            query += "max(im.imp_time) max_imp_time, \n";
            query += "4 TYPE_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID CHMS_MEDI_STOCK_ID \n";
            query += "FROM V_HIS_IMP_MEST_BLOOD IMBL \n";
            query += "JOIN HIS_IMP_MEST IM ON IM.ID=IMBL.IMP_MEST_ID \n";
            query += "JOIN HIS_EXP_MEST EX ON EX.ID=IM.CHMS_EXP_MEST_ID \n";
            query += "JOIN HIS_MEDI_STOCK MS ON MS.ID=IM.MEDI_STOCK_ID  \n";

            if (!string.IsNullOrWhiteSpace(medicineTypeCodes) || !string.IsNullOrWhiteSpace(materialTypeCodes) || !string.IsNullOrWhiteSpace(bloodTypeCodes))
            {
                query += string.Format("JOIN HIS_BLOOD ME ON ME.ID=IMBL.BLOOD_ID\n JOIN HIS_BLOOD_TYPE MT ON MT.ID=ME.BLOOD_TYPE_ID AND MT.BLOOD_TYPE_CODE in ('{0}')\n", (materialTypeCodes ?? "").Replace(",", "','"));
            }

            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("JOIN HIS_BLOOD ME ON ME.ID=IMBL.BLOOD_ID\n\n");
                query += string.Format("AND ME.BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND IMBL.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND IMBL.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND IMBL.BID_ID={0}\n", filter.BID_ID);
            }
            query += "WHERE 1 = 1\n";
            query += "and IM.IMP_MEST_STT_ID=5\n";
            query += "and IM.IS_DELETE=0\n";
            query += "and MS.IS_CABINET = 1\n";
            query += string.Format("AND IM.IMP_MEST_TYPE_ID IN({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS);

            query += string.Format("AND IM.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));
            if (parentMediStockIds != null && parentMediStockIds.Count > 0)
            {
                query += string.Format("AND EX.MEDI_STOCK_ID in ({0})\n", string.Join(",", parentMediStockIds));
            }
            query += string.Format("AND IM.IMP_TIME <{0}\n", timeTo);
            query += "GROUP BY \n";
            query += string.Format("(case when im.imp_time>{0} then 1 else 0 end), \n", timeFrom);
            query += "IMBL.BLOOD_ID, \n";
            query += "IM.MEDI_STOCK_ID, \n";
            query += "EX.MEDI_STOCK_ID \n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MediMateIdChmsMediStockId>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        public List<AnticipateMety> GetAnticipateMety(Mrs00105Filter filter, List<long> mediStockIds)
        {
            List<AnticipateMety> result = new List<AnticipateMety>();
            if (filter.IS_MEDICINE != true) return result;
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "AM.MEDICINE_TYPE_ID,\n";
            query += "MS.ID MEDI_STOCK_ID,\n";
            query += "AM.SUPPLIER_ID,\n";
            query += "SUM(AM.AMOUNT) AMOUNT\n";

            query += "FROM HIS_ANTICIPATE A\n";
            query += "JOIN HIS_ANTICIPATE_METY AM ON A.ID = AM.ANTICIPATE_ID\n";
            query += "JOIN HIS_MEDI_STOCK MS ON MS.ROOM_ID = A.REQUEST_ROOM_ID\n";

            query += "WHERE 1 = 1\n";
            query += string.Format("AND MS.ID IN ({0})\n", string.Join(",", mediStockIds));

            FilterMety(ref query, "AM.medicine_type_id", filter);
          
            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND AM.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND AM.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND AM.BID_ID={0}\n", filter.BID_ID);
            }
            query += string.Format("AND A.CREATE_TIME > {0}\n", filter.TIME_TO);
            query += "GROUP BY\n";
            query += "AM.MEDICINE_TYPE_ID,\n";
            query += "MS.ID,\n";
            query += "AM.SUPPLIER_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<AnticipateMety>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        public List<AnticipateMaty> GetAnticipateMaty(Mrs00105Filter filter, List<long> mediStockIds)
        {
            List<AnticipateMaty> result = new List<AnticipateMaty>();
            if (filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true) return result;
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "AM.MATERIAL_TYPE_ID,\n";
            query += "MS.ID MEDI_STOCK_ID,\n";
            query += "AM.SUPPLIER_ID,\n";
            query += "SUM(AM.AMOUNT) AMOUNT\n";

            query += "FROM HIS_ANTICIPATE A\n";
            query += "JOIN HIS_ANTICIPATE_MATY AM ON A.ID = AM.ANTICIPATE_ID\n";
            query += "JOIN HIS_MEDI_STOCK MS ON MS.ROOM_ID = A.REQUEST_ROOM_ID\n";

            query += "WHERE 1 = 1\n";
            query += string.Format("AND MS.ID IN ({0})\n", string.Join(",", mediStockIds));
            query += string.Format("AND A.CREATE_TIME > {0}\n", filter.TIME_TO);

            FilterMaty(ref query, "am.MATERIAL_TYPE_ID", filter);

            if (filter.IS_ONLY_IN_BID == true && filter.BID_IDs == null && filter.BID_ID == null)
            {
                query += string.Format("AND AM.BID_ID is not null\n");
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND AM.BID_ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND AM.BID_ID ={0}\n", filter.BID_ID);
            }
            query += "GROUP BY\n";
            query += "AM.MATERIAL_TYPE_ID,\n";
            query += "MS.ID,\n";
            query += "AM.SUPPLIER_ID\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<AnticipateMaty>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        public List<HIS_SALE_PROFIT_CFG> GetSaleProfitCfg()
        {
            List<HIS_SALE_PROFIT_CFG> result = new List<HIS_SALE_PROFIT_CFG>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "*\n";

            query += "FROM HIS_SALE_PROFIT_CFG\n";

            query += "WHERE 1 = 1\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_SALE_PROFIT_CFG>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        public List<HIS_BLOOD_ABO> GetBloodAbo()
        {
            List<HIS_BLOOD_ABO> result = new List<HIS_BLOOD_ABO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT\n";
            query += "*\n";

            query += "FROM HIS_BLOOD_ABO\n";

            query += "WHERE 1 = 1\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<HIS_BLOOD_ABO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<IMP_MEST> GetDocumentInfo()
        {
            List<IMP_MEST> result = new List<IMP_MEST>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select IMP_MEST_CODE, DOCUMENT_NUMBER,DOCUMENT_DATE from his_imp_mest where document_number is not null\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<IMP_MEST>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }
        public List<Mrs00105RDO> GetDiffDocumentPrice(List<long> mediStockIds, Mrs00105Filter filter)
        {
            long timeFrom = filter.TIME_FROM ?? 0;
            long timeTo = filter.TIME_TO ?? 0;
           
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            CommonParam paramGet = new CommonParam();
            if (mediStockIds == null || mediStockIds.Count == 0)
                return result;
            if (timeTo == null || timeFrom == null)
                return result;
            string query = "--danh sach lệch tiền hóa đơn\n";
            query += "SELECT \n";
            query += "Em.MEDI_STOCK_ID, \n";
            query += "Em.ID, \n";
            query += "min(EXMM.MEDI_MATE_ID) keep(dense_rank first order by exmm.total_price,exmm.TYPE,exmm.medi_mate_id) MEDI_MATE_ID, \n";
            query += "min(EXMM.TYPE) keep(dense_rank first order by exmm.total_price,exmm.TYPE,exmm.medi_mate_id) TYPE, \n";
            query += string.Format("round(min(nvl(em.document_price,0))-sum(nvl(exmm.total_price,0)),4) DIFF_DOCUMENT_TOTAL_PRICE\n");

            query += "FROM his_imp_mest em\n";
            query += "join lateral \n";
            query += "( \n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", THUOC);
            query += "NVL(EXMM.MEDICINE_ID,0) AS MEDI_MATE_ID, \n";
            query += string.Format("exmm.amount*exmm.price*(1+exmm.vat_ratio) total_price\n");
            query += "FROM HIS_IMP_MEST_MEDICINE EXMM \n";
            query += "WHERE 1=1  \n";
            query += "AND exmm.IS_DELETE =0  \n";
            query += "AND exmm.imp_mest_id = em.id  \n";
            query += "union all \n";
            query += "SELECT \n";
            query += string.Format("{0} TYPE,\n", VATTU);
            query += "NVL(EXMM.MATERIAL_ID,0) AS MEDI_MATE_ID, \n";
            query += string.Format("exmm.amount*exmm.price*(1+exmm.vat_ratio) total_price\n");
            query += "FROM HIS_IMP_MEST_MATERIAL EXMM \n";
            query += "WHERE 1=1  \n";
            query += "AND exmm.IS_DELETE =0  \n";
            query += "AND exmm.imp_mest_id = em.id  \n";
            query += ") exmm on 1=1\n";
            query += "WHERE 1=1  \n";
            query += "and em.imp_mest_stt_id=5  \n";
            query += "AND em.imp_mest_type_id =2  \n";
            query += "AND em.IS_DELETE =0  \n";
            //khong co chot ky hoac nhap sau khi chot ky
            query += string.Format("AND Em.MEDI_STOCK_ID IN ({0}) \n", string.Join(",", mediStockIds));


            query += string.Format("and  em.imp_time>{0} AND em.imp_time < {1} ", timeFrom, timeTo);

            query += "group by \n";
            query += "Em.MEDI_STOCK_ID, \n";
            query += "Em.ID \n";
            query += "having \n";
            query += "round(min(nvl(em.document_price,0))-sum(nvl(exmm.total_price,0)),4)<>0 \n";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00105RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            return result;
        }

        internal List<BID_IMP> GetBidImpMediInfo(List<long> mediStockIds, Mrs00105Filter filter,string KeyGroupBidDetail)
        {
            List<BID_IMP> result = new List<BID_IMP>();
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach nhap tu thau\n";
            query += "SELECT \n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.medicine_type_id", THUOC, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(immm.amount) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "max(bmt.adjust_amount) bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_imp_mest_medicine immm\n";
            query += "join his_medicine me on me.id=immm.medicine_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_medicine_type bmt on bmt.bid_id=me.bid_id and bmt.medicine_type_id=me.medicine_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.imp_mest_stt_id=5\n";
            query += "and immm.imp_mest_type_id in (2,6)\n";
            query += string.Format("AND immm.imp_time <= {0}\n", filter.TIME_TO);

            FilterMety(ref query, "immm.medicine_type_id", filter);

            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID ={0}\n", filter.BID_ID);
            }
            if (mediStockIds != null && mediStockIds.Count > 0)
            {
                query += string.Format("AND immm.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.medicine_type_id", THUOC, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";
            query += "union all\n";
            query += "select\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.medicine_type_id", THUOC, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(-immm.amount) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "max(bmt.adjust_amount) bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_exp_mest_medicine immm\n";
            query += "join his_medicine me on me.id=immm.medicine_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_medicine_type bmt on bmt.bid_id=me.bid_id and bmt.medicine_type_id=me.medicine_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.is_export=1\n";
            query += "and immm.exp_mest_type_id=4\n";
            query += string.Format("AND immm.exp_time <= {0}\n", filter.TIME_TO);

            FilterMety(ref query, "immm.medicine_type_id", filter);

            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID ={0}\n", filter.BID_ID);
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.medicine_type_id", THUOC, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<BID_IMP>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<BID_IMP> GetBidImpMateInfo(List<long> mediStockIds, Mrs00105Filter filter, string KeyGroupBidDetail)
        {
            List<BID_IMP> result = new List<BID_IMP>();
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach nhap tu thau\n";
            query += "SELECT \n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.material_type_id", VATTU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(immm.amount) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "max(bmt.adjust_amount) bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_imp_mest_material immm\n";
            query += "join his_material me on me.id=immm.material_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_material_type bmt on bmt.bid_id=me.bid_id and bmt.material_type_id=me.material_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.imp_mest_stt_id=5\n";
            query += "and immm.imp_mest_type_id in (2,6)\n";
            query += string.Format("AND immm.imp_time <= {0}\n", filter.TIME_TO);

            FilterMaty(ref query, "immm.MATERIAL_TYPE_ID", filter);

            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID={0}\n", filter.BID_ID);
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.material_type_id", VATTU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";
            query += "union all\n";
            query += "select\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.material_type_id", VATTU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(-immm.amount) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "max(bmt.adjust_amount) bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_exp_mest_material immm\n";
            query += "join his_material me on me.id=immm.material_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_material_type bmt on bmt.bid_id=me.bid_id and bmt.material_type_id=me.material_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.is_export=1\n";
            query += "and immm.exp_mest_type_id=4\n";
            query += string.Format("AND immm.exp_time <= {0}\n", filter.TIME_TO);

            FilterMety(ref query, "immm.MATERIAL_TYPE_ID", filter);
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID ={0}\n", filter.BID_ID);
            }
            if (mediStockIds != null && mediStockIds.Count > 0)
            {
                query += string.Format("AND immm.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.material_type_id", VATTU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<BID_IMP>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<BID_IMP> GetBidImpBloodInfo(List<long> mediStockIds, Mrs00105Filter filter, string KeyGroupBidDetail)
        {
            List<BID_IMP> result = new List<BID_IMP>();
            CommonParam paramGet = new CommonParam();
            string query = " --danh sach nhap tu thau\n";
            query += "SELECT \n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.blood_type_id", MAU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(1) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "0 bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_imp_mest_blood immm\n";
            query += "join his_blood me on me.id=immm.blood_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_blood_type bmt on bmt.bid_id=me.bid_id and bmt.blood_type_id=me.blood_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.imp_mest_stt_id=5\n";
            query += "and immm.imp_mest_type_id in (2,6)\n";
            query += string.Format("AND immm.imp_time <= {0}\n", filter.TIME_TO);
            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("AND immm.BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID={0}\n", filter.BID_ID);
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.blood_type_id", MAU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";
            query += "union all\n";
            query += "select\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + " KEY_BID_IMP,\n", "bid.id", "me.blood_type_id", MAU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "sum(-1) bid_imp_amount,\n";
            query += "max(bmt.amount) bid_amount,\n";
            query += "0 bid_adjust_amount,\n";
            query += "1\n";
            query += "from v_his_exp_mest_blood immm\n";
            query += "join his_blood me on me.id=immm.blood_id\n";
            query += "join his_bid bid on bid.id=me.bid_id\n";
            query += "left join his_bid_blood_type bmt on bmt.bid_id=me.bid_id and bmt.blood_type_id=me.blood_type_id and me.supplier_id = bmt.supplier_id\n";
            query += "where 1=1\n";
            query += "and immm.is_export=1\n";
            query += "and immm.exp_mest_type_id=4\n";
            query += string.Format("AND immm.exp_time <= {0}\n", filter.TIME_TO);
            if (filter.BLOOD_TYPE_IDs != null)
            {
                query += string.Format("AND immm.BLOOD_TYPE_ID in ({0})\n", string.Join(",", filter.BLOOD_TYPE_IDs));
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format("AND BID.ID in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.BID_ID != null)
            {
                query += string.Format("AND BID.ID ={0}\n", filter.BID_ID);
            }
            if (mediStockIds != null && mediStockIds.Count > 0)
            {
                query += string.Format("AND immm.MEDI_STOCK_ID in ({0})\n", string.Join(",", mediStockIds));
            }

            query += "group by\n";
            query += string.Format(KeyGroupBidDetail.Replace("_", "||'_'||") + ",\n", "bid.id", "me.blood_type_id", MAU, "me.supplier_id", "bid.bid_number", "bid.bid_name", "bid.bid_type_id", "bid.bid_year", "bid.valid_from_time", "bid.valid_to_time", "bid.bid_extra_code");
            query += "1\n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<BID_IMP>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<MEDI_STOCK_CODE_IMPs> GetMediStockCodeImps(List<long> mediStockIds, Mrs00105Filter filter)
        {
            List<MEDI_STOCK_CODE_IMPs> result = new List<MEDI_STOCK_CODE_IMPs>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select medi_stock_code from his_medi_stock where 1=1 \n";
            query += string.Format("and id in (select distinct imp_medi_stock_id from his_exp_mest where 1=1 and exp_mest_type_id in (3,5,13)\n");
            if (mediStockIds != null && mediStockIds.Count > 0)
            {
                query += string.Format("AND medi_stock_id in ({0})\n", string.Join(",", mediStockIds));
            }
            query += string.Format("and (id in (select exp_mest_id from his_exp_mest_medicine where is_export=1 and exp_time between {0} and {1}) or id in (select exp_mest_id from his_exp_mest_material where is_export=1 and exp_time between {0} and {1})))\n", filter.TIME_FROM, filter.TIME_TO);


            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MEDI_STOCK_CODE_IMPs>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }

        internal List<MEDI_STOCK_CODE_IMPs> GetMediStockCodeChmss(List<long> mediStockIds, Mrs00105Filter filter)
        {
            List<MEDI_STOCK_CODE_IMPs> result = new List<MEDI_STOCK_CODE_IMPs>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select medi_stock_code from his_medi_stock where 1=1 \n";
            query += string.Format("and id in (select distinct chms_medi_stock_id from his_imp_mest where 1=1 and imp_mest_stt_id = 5 and is_delete=0 and imp_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            if (mediStockIds != null && mediStockIds.Count > 0)
            {
                query += string.Format("AND medi_stock_id in ({0})\n", string.Join(",", mediStockIds));
            }
            query += string.Format(")\n", filter.TIME_FROM, filter.TIME_TO);


            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MEDI_STOCK_CODE_IMPs>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");

            return result;
        }
    }
}
