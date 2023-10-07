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

namespace MRS.Processor.Mrs00709
{
    internal class ManagerSql
    {

        internal List<Mrs00709RDO> GetMedicine(Mrs00709Filter filter)
        {
            List<Mrs00709RDO> result = new List<Mrs00709RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --Bao cao tong hop theo doi duoc\n";
            query += " select\n";
            query += "mt.medicine_type_code service_code,\n";
            query += "mt.medicine_type_name service_name,\n";
            query += " s.supplier_code,\n";
            query += " s.supplier_name,\n";
            query += " su.service_unit_code,\n";
            query += " su.service_unit_name,\n";
            query += " m.tdl_bid_number,\n";
            query += " m.tdl_bid_year,\n";
            query += " im.document_date,\n";
            query += " im.document_number,\n";
            query += " emme.req_department_id,\n";
            query += " dp.department_code,\n";
            query += " dp.department_name,\n";
            query += "im.imp_date,\n";
            query += "imme.medicine_id mema_id,\n";
            query += "1 type,\n";
            query += "imme.amount,\n";
            query += "sum(imp_tra.amount) imp_amount,\n";
            query += "emme.exp_date,\n";
            query += "sum(nvl(emme.amount,0)) exp_amount\n";
            query += "from his_imp_mest im\n";
            query += "join his_imp_mest_medicine imme on imme.imp_mest_id=im.id and imme.is_delete=0\n";
            query += "left join v_his_exp_mest_medicine emme \n";
            query += " on (emme.medicine_id=imme.medicine_id\n";
            query += " and emme.exp_time between {0} and {1} \n";
            query += " and emme.is_export=1 \n";
            query += " and emme.is_delete =0\n";
            query += " and emme.exp_mest_type_id not in (3,5)\n";
            if (filter.MEDICINE_TYPE_ID != null)
            {
                query += string.Format("and emme.medicine_type_id ={0}\n", filter.MEDICINE_TYPE_ID);
            }
            if (filter.MEDICINE_ID != null)
            {
                query += string.Format("and emme.medicine_id ={0}\n", filter.MEDICINE_ID);
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("and emme.medicine_type_id in ({0})\n", string.Join(",",filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDICINE_IDs != null)
            {
                query += string.Format("and emme.medicine_id in ({0})\n", string.Join(",",filter.MEDICINE_IDs));
            }
            query += ")\n";
            query += "left join\n";
            query += "(\n";
            query += "select\n";
            query += "th_exp_mest_medicine_id,\n";
            query += "sum(amount) amount\n";
            query += "from v_his_imp_mest_medicine\n";
            query += "where 1=1\n";
            query += "and imp_time between {0} and {1}\n";
            query += "and imp_mest_stt_id=5 \n";
            query += "and imp_mest_type_id not in(2,1,8)\n";
            query += "and is_delete=0\n";
            if (filter.MEDICINE_TYPE_ID != null)
            {
                query += string.Format("and medicine_type_id ={0}\n", filter.MEDICINE_TYPE_ID);
            }
            if (filter.MEDICINE_ID != null)
            {
                query += string.Format("and medicine_id ={0}\n", filter.MEDICINE_ID);
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("and medicine_type_id in ({0})\n", string.Join(",",filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDICINE_IDs != null)
            {
                query += string.Format("and medicine_id in ({0})\n", string.Join(",",filter.MEDICINE_IDs));
            }
            query += "group by\n";
            query += "th_exp_mest_medicine_id\n";
            query += ")imp_tra on imp_tra.th_exp_mest_medicine_id = emme.id\n";
            query += "join his_supplier s on s.id=im.supplier_id\n";
            query += "join his_medicine m on m.id=imme.medicine_id\n";
            query += "join his_medicine_type mt on mt.id=m.medicine_type_id\n";
            query += "left join his_service_unit su on su.id=mt.tdl_service_unit_id\n";
            query += "left join his_department dp on dp.id=emme.req_department_id\n";
            query += " where 1=1\n";
            query += " and im.imp_mest_stt_id = 5\n";
            query += " and im.imp_mest_type_id = 2\n";
            query += " and im.is_delete = 0\n";
            query += " and im.imp_time between {0} and {1}\n";
            if (filter.MEDICINE_TYPE_ID != null)
            {
                query += string.Format("and mt.id ={0}\n", filter.MEDICINE_TYPE_ID);
            }
            if (filter.MEDICINE_ID != null)
            {
                query += string.Format("and m.id ={0}\n", filter.MEDICINE_ID);
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format("and mt.id in ({0})\n", string.Join(",",filter.MEDICINE_TYPE_IDs));
            }
            if (filter.MEDICINE_IDs != null)
            {
                query += string.Format("and m.id in ({0})\n", string.Join(",",filter.MEDICINE_IDs));
            }
            query += " group by\n";
            query += "mt.medicine_type_code,\n";
            query += "mt.medicine_type_name,\n";
            query += " s.supplier_code,\n";
            query += " s.supplier_name,\n";
            query += " su.service_unit_code,\n";
            query += " su.service_unit_name,\n";
            query += " m.tdl_bid_number,\n";
            query += " m.tdl_bid_year,\n";
            query += " im.document_date,\n";
            query += " im.document_number,\n";
            query += " emme.req_department_id,\n";
            query += " dp.department_code,\n";
            query += " dp.department_name,\n";
            query += "im.imp_date,\n";
            query += "imme.medicine_id,\n";
            query += "imme.amount,\n";
            query += "imp_tra.amount,\n";
            query += "emme.exp_date\n";

            query = string.Format(query, filter.TIME_FROM, filter.TIME_TO);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00709RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00709");

            return result;
        }

        internal List<Mrs00709RDO> GetMaterial(Mrs00709Filter filter)
        {
            List<Mrs00709RDO> result = new List<Mrs00709RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --Bao cao tong hop theo doi duoc\n";
            query += " select\n";
            query += "mt.material_type_code service_code,\n";
            query += "mt.material_type_name service_name,\n";
            query += " s.supplier_code,\n";
            query += " s.supplier_name,\n";
            query += " su.service_unit_code,\n";
            query += " su.service_unit_name,\n";
            query += " m.tdl_bid_number,\n";
            query += " m.tdl_bid_year,\n";
            query += " im.document_date,\n";
            query += " im.document_number,\n";
            query += " emme.req_department_id,\n";
            query += " dp.department_code,\n";
            query += " dp.department_name,\n";
            query += "im.imp_date,\n";
            query += "imme.material_id mema_id,\n";
            query += "2 type,\n";
            query += "imme.amount,\n";
            query += "sum(imp_tra.amount) imp_amount,\n";
            query += "emme.exp_date,\n";
            query += "sum(nvl(emme.amount,0)) exp_amount\n";
            query += "from his_imp_mest im\n";
            query += "join his_imp_mest_material imme on imme.imp_mest_id=im.id and imme.is_delete=0\n";
            query += "left join v_his_exp_mest_material emme \n";
            query += " on (emme.material_id=imme.material_id\n";
            query += " and emme.exp_time between {0} and {1} \n";
            query += " and emme.is_export=1 \n";
            query += " and emme.is_delete =0\n";
            query += " and emme.exp_mest_type_id not in (3,5)\n";
            if (filter.MATERIAL_TYPE_ID != null)
            {
                query += string.Format("and emme.material_type_id ={0}\n", filter.MATERIAL_TYPE_ID);
            }
            if (filter.MATERIAL_ID != null)
            {
                query += string.Format("and emme.material_id ={0}\n", filter.MATERIAL_ID);
            }
            if (filter.MATERIAL_TYPE_IDs != null)
            {
                query += string.Format("and emme.material_type_id in ({0})\n", string.Join(",",filter.MATERIAL_TYPE_IDs));
            }
            if (filter.MATERIAL_IDs != null)
            {
                query += string.Format("and emme.material_id in ({0})\n", string.Join(",",filter.MATERIAL_IDs));
            }
            query += ")\n";
            query += "left join\n";
            query += "(\n";
            query += "select\n";
            query += "th_exp_mest_material_id,\n";
            query += "sum(amount) amount\n";
            query += "from v_his_imp_mest_material\n";
            query += "where 1=1\n";
            query += "and imp_time between {0} and {1}\n";
            query += "and imp_mest_stt_id=5 \n";
            query += "and imp_mest_type_id not in(2,1,8)\n";
            query += "and is_delete=0\n";
            if (filter.MATERIAL_TYPE_ID != null)
            {
                query += string.Format("and material_type_id ={0}\n", filter.MATERIAL_TYPE_ID);
            }
            if (filter.MATERIAL_ID != null)
            {
                query += string.Format("and material_id ={0}\n", filter.MATERIAL_ID);
            }
            if (filter.MATERIAL_TYPE_IDs != null)
            {
                query += string.Format("and material_type_id in ({0})\n", string.Join(",",filter.MATERIAL_TYPE_IDs));
            }
            if (filter.MATERIAL_IDs != null)
            {
                query += string.Format("and material_id in ({0})\n", string.Join(",",filter.MATERIAL_IDs));
            }
            query += "group by\n";
            query += "th_exp_mest_material_id\n";
            query += ")imp_tra on imp_tra.th_exp_mest_material_id = emme.id\n";
            query += "join his_supplier s on s.id=im.supplier_id\n";
            query += "join his_material m on m.id=imme.material_id\n";
            query += "join his_material_type mt on mt.id=m.material_type_id\n";
            query += "left join his_service_unit su on su.id=mt.tdl_service_unit_id\n";
            query += "left join his_department dp on dp.id=emme.req_department_id\n";
            query += " where 1=1\n";
            query += " and im.imp_mest_stt_id = 5\n";
            query += " and im.imp_mest_type_id = 2\n";
            query += " and im.is_delete = 0\n";
            query += " and im.imp_time between {0} and {1}\n";
            if (filter.MATERIAL_TYPE_ID != null)
            {
                query += string.Format("and mt.id ={0}\n", filter.MATERIAL_TYPE_ID);
            }
            if (filter.MATERIAL_ID != null)
            {
                query += string.Format("and m.id ={0}\n", filter.MATERIAL_ID);
            }
            if (filter.MATERIAL_TYPE_IDs != null)
            {
                query += string.Format("and mt.id in ({0})\n", string.Join(",",filter.MATERIAL_TYPE_IDs));
            }
            if (filter.MATERIAL_IDs != null)
            {
                query += string.Format("and m.id in ({0})\n", string.Join(",",filter.MATERIAL_IDs));
            }
            query += " group by\n";
            query += "mt.material_type_code,\n";
            query += "mt.material_type_name,\n";
            query += " s.supplier_code,\n";
            query += " s.supplier_name,\n";
            query += " su.service_unit_code,\n";
            query += " su.service_unit_name,\n";
            query += " m.tdl_bid_number,\n";
            query += " m.tdl_bid_year,\n";
            query += " im.document_date,\n";
            query += " im.document_number,\n";
            query += " emme.req_department_id,\n";
            query += " dp.department_code,\n";
            query += " dp.department_name,\n";
            query += "im.imp_date,\n";
            query += "imme.material_id,\n";
            query += "imme.amount,\n";
            query += "imp_tra.amount,\n";
            query += "emme.exp_date\n";

            query = string.Format(query, filter.TIME_FROM, filter.TIME_TO);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00709RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00709");

            return result;
        }
    }
}
