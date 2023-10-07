using Inventec.Core;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00704
{
    class ManagerSql
    {
        internal static List<PTTT_Model> GetLoaiPTTT(long serviceType)
        {
            List<PTTT_Model> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT \"TenPTTT\", \"DoanhThu\", \"ChiPhi\", \"DoanhThu\" - \"ChiPhi\" AS \"LoiNhuan\" ";
                query += "FROM (";
                query += "SELECT TDL_SERVICE_CODE, TDL_SERVICE_NAME AS \"TenPTTT\", SUM(VIR_TOTAL_PRICE) AS \"DoanhThu\", ";
                query += "SUM(NVL((SELECT SUM(AMOUNT * PRICE * (1 + VAT_RATIO)) FROM HIS_SERE_SERV WHERE SS.ID = PARENT_ID AND IS_EXPEND = 1 AND IS_DELETE = 0 AND SERVICE_REQ_ID IS NOT NULL ),0)) AS \"ChiPhi\" ";
                query += "FROM HIS_SERE_SERV SS  ";
                query += "WHERE SS.IS_DELETE = 0 AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.TDL_SERVICE_TYPE_ID = :Type ";
                query += "AND SS.TDL_INTRUCTION_TIME BETWEEN :TIME_FROM AND :TIME_TO ";
                query += "GROUP BY SS.TDL_SERVICE_CODE, SS.TDL_SERVICE_NAME ";
                query += ") ";

                long timeFrom = Inventec.Common.DateTime.Get.StartDay() ?? 0;
                long timeto = Inventec.Common.DateTime.Get.EndDay() ?? 0;

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<PTTT_Model>(paramGet, query, serviceType, timeFrom, timeto);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00704");
                return result;
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal static List<DT_PCD_Model> GetDtPcd()
        {
            List<DT_PCD_Model> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT SS.TDL_EXECUTE_ROOM_ID, R.EXECUTE_ROOM_NAME AS \"TenPhong\", ";
                query += "SUM(SSB.PRICE) AS \"TongCong\", ";
                query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID = 1 THEN SSB.PRICE ELSE 0 END) AS \"BHYT\", ";
                query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID <> 1 THEN SSB.PRICE ELSE 0 END) AS \"ThuPhi\" ";
                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID ";
                query += "JOIN HIS_TRANSACTION TRAN ON SSB.BILL_ID = TRAN.ID ";
                query += "JOIN HIS_EXECUTE_ROOM R ON SS.TDL_EXECUTE_ROOM_ID = R.ROOM_ID ";
                query += "WHERE R.IS_EXAM = 1 AND SS.IS_DELETE = 0 AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND TRAN.TRANSACTION_TIME BETWEEN :TIME_FROM AND :TIME_TO ";
                query += "GROUP BY SS.TDL_EXECUTE_ROOM_ID, R.EXECUTE_ROOM_NAME ";

                long timeFrom = Inventec.Common.DateTime.Get.StartDay() ?? 0;
                long timeto = Inventec.Common.DateTime.Get.EndDay() ?? 0;

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<DT_PCD_Model>(paramGet, query, timeFrom, timeto);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00704");
                return result;
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal static List<Khoa_DT_Model> GetKhoaDt(long type)
        {
            List<Khoa_DT_Model> result = null;
            try
            {
                CommonParam paramGet = new CommonParam();
                string query = "";
                query += "SELECT \"TenKhoa\", \"DoanhThu\", \"ChiPhi\", \"DoanhThu\" - \"ChiPhi\" AS \"LoiNhuan\" ";
                query += "FROM (";
                query += "SELECT SS.TDL_EXECUTE_DEPARTMENT_ID, DE.DEPARTMENT_NAME AS \"TenKhoa\", SUM(VIR_TOTAL_PRICE) AS \"DoanhThu\", ";
                query += "SUM(NVL((SELECT SUM(AMOUNT * PRICE * (1 + VAT_RATIO)) FROM HIS_SERE_SERV WHERE SS.ID = PARENT_ID AND IS_EXPEND = 1 AND IS_DELETE = 0 AND SERVICE_REQ_ID IS NOT NULL ),0)) AS \"ChiPhi\" ";
                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "JOIN HIS_DEPARTMENT DE ON SS.TDL_EXECUTE_DEPARTMENT_ID = DE.ID ";
                query += "WHERE SS.IS_DELETE = 0 AND SS.SERVICE_REQ_ID IS NOT NULL ";

                if (type == 1)//1:Đang điều trị:
                {
                    query += "AND TREA.OUT_TIME IS NULL ";
                }
                else if (type == 2)//2:Đã ra viện chưa thanh toán
                {
                    query += "AND TREA.IS_PAUSE = 1 AND TREA.FEE_LOCK_TIME IS NULL ";
                }
                else if (type == 3)//3:Đã thanh toán
                {
                    long timeFrom = Inventec.Common.DateTime.Get.StartDay() ?? 0;
                    long timeto = Inventec.Common.DateTime.Get.EndDay() ?? 0;
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} AND {1} ", timeFrom, timeto);
                }
                else //trả lại rỗng.
                {
                    query += "AND TREA.ID = 0";
                }

                query += "GROUP BY SS.TDL_EXECUTE_DEPARTMENT_ID, DE.DEPARTMENT_NAME ";
                query += ") ";


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<Khoa_DT_Model>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00704");
                return result;
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
