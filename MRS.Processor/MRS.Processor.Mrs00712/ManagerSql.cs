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
using System.Data;
using MRS.MANAGER.Config;
using SDA.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00712
{
    internal class ManagerSql
    {

        internal List<Mrs00712RDO> GetCount(Mrs00712Filter filter)
        {
            List<Mrs00712RDO> result = new List<Mrs00712RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- so luong Phau thuat thu thuat thuc hien\n");
            query += string.Format("select\n");
            query += string.Format("sv.pttt_group_id,\n");
            query += string.Format("pg.pttt_group_code,\n");
            query += string.Format("pg.pttt_group_name,\n");
            query += string.Format("(case\n");
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZBSs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZBS'\n", filter.SERVICE_CODE__LZBSs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZMMs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZMM'\n", filter.SERVICE_CODE__LZMMs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZQDs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZQD'\n", filter.SERVICE_CODE__LZQDs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__TNNs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'TNN'\n", filter.SERVICE_CODE__TNNs.Replace(",", "','"));
            }
            query += string.Format("when 1=1 then '' else '' end) category_code,\n");
            query += string.Format("ss.tdl_service_type_id,\n");
            query += string.Format("sr.treatment_type_id,\n");
            query += string.Format("sum(amount) amount\n");
            query += string.Format("from his_sere_serv ss\n");
            query += string.Format("join his_service_req sr on sr.id=ss.service_req_id\n");
            query += string.Format("join his_service sv on sv.id=ss.service_id\n");
            query += string.Format("join his_pttt_group pg on pg.id=sv.pttt_group_id\n");
            query += string.Format("where 1=1\n");
            query += string.Format("and ss.is_no_execute is null\n");
            query += string.Format("and ss.is_delete=0\n");
            query += string.Format("and ss.tdl_service_type_id in ({0},{1})\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
            query += string.Format("and ss.tdl_intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            query += string.Format("group by\n");
            query += string.Format("sv.pttt_group_id,\n");
            query += string.Format("pg.pttt_group_code,\n");
            query += string.Format("pg.pttt_group_name,\n");
            query += string.Format("(case\n");
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZBSs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZBS'\n", filter.SERVICE_CODE__LZBSs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZMMs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZMM'\n", filter.SERVICE_CODE__LZMMs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__LZQDs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'LZQD'\n", filter.SERVICE_CODE__LZQDs.Replace(",", "','"));
            }
            if (!string.IsNullOrWhiteSpace(filter.SERVICE_CODE__TNNs))
            {
                query += string.Format("when ss.tdl_service_code in ('{0}') then 'TNN'\n", filter.SERVICE_CODE__TNNs.Replace(",", "','"));
            }
            query += string.Format("when 1=1 then '' else '' end),\n");
            query += string.Format("ss.tdl_service_type_id,\n");
            query += string.Format("sr.treatment_type_id");

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00712RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00712");

            return result;
        }
    }
}
