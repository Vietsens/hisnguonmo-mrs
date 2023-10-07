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
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00602
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT> GetTreatment(Mrs00602Filter filter)
        {
            List<HIS_TREATMENT> result = null;
            try
            {
                string query = "--danh sach benh nhan thanh toan trong thoi gian bao cao\n";
                query += "SELECT \n";
                query += "Trea.* \n";

                query += "from HIS_RS.HIS_TREATMENT TREA  \n";
                query += "left join HIS_RS.HIS_TRANSACTION TRAN on TRAN.treatment_id = trea.id and Tran.IS_CANCEL IS NULL and TRAN.transaction_type_id = 3 \n";

                query += "WHERE 1=1\n";
                
                //kiểm tra có lọc theo thời gian thanh toán không
                if (filter.CHOOSE_TIME_FILTER == 2 || filter.IS_PAY == true)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);

                    //lọc tất cả trạng thái
                    if (filter.IS_ALL_STATUS == true)
                    {
                        filter.STATUS_TREATMENT = 3;
                    }
                    // trạng thái hồ sơ
                    else if (filter.STATUS_TREATMENT == null)//đã khóa viện phí
                    {
                        query += string.Format("AND TREA.IS_ACTIVE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                    }
                    else if (filter.STATUS_TREATMENT == 0)//chưa kết thúc điều trị
                    {
                        query += string.Format("AND TREA.IS_PAUSE IS NULL \n");
                    }
                    else if (filter.STATUS_TREATMENT == 1)//đã kết thúc điều trị
                    {
                        query += string.Format("AND TREA.IS_PAUSE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                    else if (filter.STATUS_TREATMENT == 2)// đã duyệt giám định bảo hiểm
                    {
                        query += string.Format("AND TREA.IS_LOCK_HEIN = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                }
                else
                {
                    //lọc tất cả trạng thái
                    if (filter.IS_ALL_STATUS == true)
                    {
                        filter.STATUS_TREATMENT = 3;
                        query += string.Format("AND trea.in_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
                    }
                    // trạng thái hồ sơ
                    else if (filter.STATUS_TREATMENT == null)//đã khóa viện phí, lọc theo thời gian khóa viện phí
                    {
                        query += string.Format("AND TREA.IS_ACTIVE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                        query += string.Format("AND trea.fee_lock_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
                    }
                    else if (filter.STATUS_TREATMENT == 0)//chưa kết thúc điều trị, lấy theo thời gian vào viện
                    {
                        query += string.Format("AND TREA.IS_PAUSE IS NULL \n");
                        query += string.Format("AND trea.in_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
                    }
                    else if (filter.STATUS_TREATMENT == 1)//đã kết thúc điều trị, lấy theo thời gian ra viện
                    {
                        query += string.Format("AND TREA.IS_PAUSE = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                        query += string.Format("AND trea.out_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
                    }
                    else if (filter.STATUS_TREATMENT == 2)// đã duyệt giám định bảo hiểm, lấy theo thời gian khóa viện phí
                    {
                        query += string.Format("AND TREA.IS_LOCK_HEIN = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                        query += string.Format("AND exists (select 1 from his_hein_approval hap where hap.treatment_id =trea.id and hap.execute_time between {0} and {1}) \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
                    }
                }
                //diện điều trị
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                // khoa điều trị
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND TREA.LAST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
                }
                //khoa điều trị nhiều
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND TREA.LAST_DEPARTMENT_ID in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME in ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
                if (result != null)
                {
                    result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<HIS_DEPARTMENT> GetDepaments()
        {
            List<HIS_DEPARTMENT> depament = new List<HIS_DEPARTMENT>();
            CommonParam common = new CommonParam();
            string Query = "";
            Query += "Select * from HIS_RS.HIS_DEPARTMENT";
            depament = new SqlDAO().GetSql<HIS_DEPARTMENT>(common, Query);
            if (common.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00602");
            return depament;
        }
    }

}
