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

namespace MRS.Processor.Mrs00506
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00506RDO> GetSereServDO(Mrs00506Filter filter)
        {
            List<Mrs00506RDO> result = new List<Mrs00506RDO>();
            try
            {
                /*public long SERVICE_ID { get;  set;  }
        public long SERE_SERV_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long HEIN_SERVICE_TYPE_NUM_ORDER { get; set; }

        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }

        public long? PACKAGE_ID { get; set; }

        public long? PARENT_ID { get; set; }

        public short? IS_OUT_PARENT_FEE { get; set; }

        public decimal? VIR_PRICE { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }


        public decimal PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal? VIR_TOTAL_PRICE { get;  set;  }*/
                string query = " --chi tiet dich vu thuc hien\n";
                query += "SELECT\n";
                query += "SS.SERVICE_ID,\n";
                query += "SS.ID SERE_SERV_ID,\n";
                query += "SS.TDL_TREATMENT_CODE TREATMENT_CODE,\n";
                query += "SS.TDL_SERVICE_CODE SERVICE_CODE,\n";
                query += "SS.TDL_SERVICE_NAME SERVICE_NAME,\n";
                query += "SS.TDL_SERVICE_TYPE_ID SERVICE_TYPE_ID,\n";
                query += "SVT.SERVICE_TYPE_CODE,\n";
                query += "SVT.SERVICE_TYPE_NAME,\n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID,\n";
                query += "SS.PACKAGE_ID,\n";
                query += "SS.PARENT_ID,\n";
                query += "SS.IS_OUT_PARENT_FEE,\n";
                query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
                query += "SS.VIR_PRICE,\n";
                query += "SS.VIR_TOTAL_PRICE,\n";
                query += "SS.AMOUNT\n";

                query += "FROM HIS_SERE_SERV SS\n";
                //query += "join HIS_SERVICE SV on SV.id=ss.service_id\n";
                query += "join HIS_SERVICE_TYPE SVT on SVT.id=ss.tdl_service_type_id\n";
                query += "join HIS_TREATMENT TREA on trea.id=ss.tdl_treatment_id\n";
                query += "left join his_department dp on dp.id=ss.tdl_request_department_id\n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND ss.is_expend is null\n";
                query += "AND SS.IS_NO_EXECUTE IS NULL\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";


                ////phòng thực hiện
                //if (filter.EXECUTE_ROOM_ID != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0}\n", filter.EXECUTE_ROOM_ID);
                //}
                //if (filter.EXECUTE_ROOM_IDs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_ROOM_ID in ({0})\n", string.Join(",",filter.EXECUTE_ROOM_IDs));
                //}
                ////khoa thực hiện
                //if (filter.EXECUTE_DEPARTMENT_ID != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
                //}
                //if (filter.EXECUTE_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                //}

                ////phòng chỉ định
                //if (filter.REQUEST_ROOM_ID != null)
                //{
                //    query += string.Format("AND SR.REQUEST_ROOM_ID = {0}\n", filter.REQUEST_ROOM_ID);
                //}
                //if (filter.REQUEST_ROOM_IDs != null)
                //{
                //    query += string.Format("AND SR.REQUEST_ROOM_ID in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                //}
                //khoa chỉ định
                //if (filter.REQUEST_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                //}

                //if (filter.REQUEST_DEPARTMENT_ID != null)
                //{
                //    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.REQUEST_DEPARTMENT_ID);
                //}
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);
                }

                ////đối tượng thanh toán
                //if (filter.PATIENT_TYPE_ID != null)
                //{
                //    query += string.Format("AND SS.PATIENT_TYPE_ID = {0}\n", filter.PATIENT_TYPE_ID);
                //}

                ////đối tượng bệnh nhân
                //if (filter.TDL_PATIENT_TYPE_ID != null)
                //{
                //    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0}\n", filter.TDL_PATIENT_TYPE_ID);
                //}

                //if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                //{
                //    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                //{
                //    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                //{
                //    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                //{
                //    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                //{
                //    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                //{
                //    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                }
                ////loại dịch vụ
                //if (filter.SERVICE_TYPE_ID != null)
                //{
                //    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", filter.SERVICE_TYPE_ID);
                //}

                //if (filter.SERVICE_TYPE_IDs != null)
                //{
                //    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
                //}
                ////dịch vụ
                //if (filter.SERVICE_ID != null)
                //{
                //    query += string.Format("AND SS.SERVICE_ID = {0}\n", filter.SERVICE_ID);
                //}
                //if (filter.SERVICE_IDs != null)
                //{
                //    query += string.Format("AND SS.SERVICE_ID IN ({0})\n", string.Join(",", filter.SERVICE_IDs));
                //}
                //if (!string.IsNullOrEmpty(filter.SERVICE_NAME))
                //{
                //    query += string.Format("AND lower(SS.TDL_SERVICE_NAME) like '%{0}%'\n", filter.SERVICE_NAME.ToLower());
                //}
                ////nhóm dịch vụ
                //if (filter.EXACT_PARENT_SERVICE_ID != null)
                //{
                //    query += string.Format("AND SV.PARENT_ID = {0}\n", filter.EXACT_PARENT_SERVICE_ID);
                //}
                //if (filter.EXACT_PARENT_SERVICE_IDs != null)
                //{
                //    query += string.Format("AND SV.PARENT_ID IN ({0})\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                //}

                ////diện điều trị
                //if (filter.TREATMENT_TYPE_ID != null)
                //{
                //    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0}\n", filter.TREATMENT_TYPE_ID);
                //}
                //if (filter.TREATMENT_TYPE_IDs != null)
                //{
                //    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0})", string.Join(",", filter.TREATMENT_TYPE_IDs));
                //}

                ////phòng thu ngân
                //if (filter.EXACT_CASHIER_ROOM_ID != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_ROOM_ID = {0}\n", filter.EXACT_CASHIER_ROOM_ID);
                //}
                //if (filter.EXACT_CASHIER_ROOM_IDs != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_ROOM_ID in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                //}

                ////nhân viên thu ngân
                //if (filter.CASHIER_LOGINNAME != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_LOGINNAME = '{0}'\n", filter.CASHIER_LOGINNAME);
                //}
                //if (filter.CASHIER_LOGINNAMEs != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_LOGINNAME in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                //}


                ////trạng thái y lệnh
                //if (filter.SERVICE_REQ_STT_ID != null)
                //{
                //    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
                //}
                //if (filter.SERVICE_REQ_STT_IDs != null)
                //{
                //    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                //}
                ////if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                ////{
                ////    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                ////}

                ////đối tượng bệnh nhân
                //if (filter.TDL_PATIENT_TYPE_IDs != null)
                //{
                //    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                //}

                ////đối tượng thanh toán
                //if (filter.PATIENT_TYPE_IDs != null)
                //{
                //    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                //}

                ////đối tượng chi tiết
                //if (filter.PATIENT_CLASSIFY_ID != null)
                //{
                //    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID = {0}\n",  filter.PATIENT_CLASSIFY_ID);
                //}
                //if (filter.PATIENT_CLASSIFY_IDs != null)
                //{
                //    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID IN ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                //}

                //chi nhánh
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("AND DP.BRANCH_ID = {0}\n", filter.BRANCH_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00506RDO>(query);
               
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
