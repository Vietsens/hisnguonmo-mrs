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

namespace MRS.Processor.Mrs00369
{
    public partial class ManagerSql : BusinessBase
    {
        public DataTable GetImpMedicine(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.id) OVER(PARTITION BY im_all.noi_xuat,im_all.imp_mest_type_name) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.imp_mest_type_name) as min_mest_type,im_all.* from (select (case when im.aggr_imp_mest_id is not null then 'Tổng hợp trả' else imt.imp_mest_type_name end) as imp_mest_type_name,imm.id,(case when imm.imp_mest_type_id =2 then imm.supplier_name when imm.imp_mest_type_id in(16,3,4,5,10,11,12,13,15) then dp.department_name when ms.id is not null then ms.medi_stock_name else  null end) as noi_xuat,(case when im.aggr_imp_mest_id is not null then aggr.imp_mest_code else imm.imp_mest_code end) as imp_mest_code,imm.imp_time,imm.document_number,im.DOCUMENT_DATE,imm.medicine_type_name,imm.medicine_type_code,imm.service_unit_name,imm.concentra,imm.imp_price,imm.imp_vat_ratio,imm.package_number,imm.expired_date,imm.amount,imm.amount*imm.imp_price*(imm.imp_vat_ratio+1) as total_price from his_rs.v_his_imp_mest_medicine imm left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {

                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) OR AGGR.ID IS NOT NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) AND AGGR.ID IS NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                }
                query += ") im_all order by imp_mest_type_name,noi_xuat,id,medicine_type_name ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetImpMaterial(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.id) OVER(PARTITION BY im_all.noi_xuat,im_all.imp_mest_type_name) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.imp_mest_type_name) as min_mest_type,im_all.*  from (select (case when im.aggr_imp_mest_id is not null then 'Tổng hợp trả' else imt.imp_mest_type_name end) as imp_mest_type_name,imm.id,(case when imm.imp_mest_type_id =2 then imm.supplier_name when imm.imp_mest_type_id in(16,3,4,5,10,11,12,13,15) then dp.department_name when ms.id is not null then ms.medi_stock_name else  null end) as noi_xuat,(case when im.aggr_imp_mest_id is not null then aggr.imp_mest_code else imm.imp_mest_code end) as imp_mest_code,imm.imp_time,imm.document_number,im.DOCUMENT_DATE,imm.material_type_name,imm.material_type_code,imm.service_unit_name,imm.imp_price,imm.imp_vat_ratio,imm.package_number,imm.expired_date,imm.amount,imm.amount*imm.imp_price*(imm.imp_vat_ratio+1) as total_price from his_rs.v_his_imp_mest_material imm join his_rs.his_material_type maty on (maty.id=imm.material_type_id and maty.is_CHEMICAL_SUBSTANCE is null) left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) OR AGGR.ID IS NOT NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) AND AGGR.ID IS NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    
                }
                query += ") im_all order by imp_mest_type_name,noi_xuat,id,material_type_name ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetImpChemical(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.id) OVER(PARTITION BY im_all.noi_xuat,im_all.imp_mest_type_name) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.imp_mest_type_name) as min_mest_type,im_all.*  from (select (case when im.aggr_imp_mest_id is not null then 'Tổng hợp trả' else imt.imp_mest_type_name end) as imp_mest_type_name,imm.id,(case when imm.imp_mest_type_id =2 then imm.supplier_name when imm.imp_mest_type_id in(16,3,4,5,10,11,12,13,15) then dp.department_name when ms.id is not null then ms.medi_stock_name else  null end) as noi_xuat,(case when im.aggr_imp_mest_id is not null then aggr.imp_mest_code else imm.imp_mest_code end) as imp_mest_code,imm.imp_time,imm.document_number,im.DOCUMENT_DATE,imm.material_type_name,imm.material_type_code,imm.service_unit_name,imm.imp_price,imm.imp_vat_ratio,imm.package_number,imm.expired_date,imm.amount,imm.amount*imm.imp_price*(imm.imp_vat_ratio+1) as total_price from his_rs.v_his_imp_mest_material imm join his_rs.his_material_type maty on (maty.id=imm.material_type_id and maty.is_CHEMICAL_SUBSTANCE =1) left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) OR AGGR.ID IS NOT NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND (IMM.IMP_MEST_TYPE_ID IN ({0}) AND AGGR.ID IS NULL) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }

                }
                query += ") im_all order by imp_mest_type_name,noi_xuat,id,material_type_name ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetImpMedicineType(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.key) OVER(PARTITION BY im_all.noi_xuat,im_all.IMP_GROUP) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.IMP_GROUP) as min_mest_type,im_all.*  from (select imm.medicine_type_name||'_'||imm.medicine_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END) as key,min( CASE WHEN (ms.is_cabinet is null or ms.is_cabinet<>1) and  imm.imp_mest_type_id =1 THEN 'CHUYỂN KHO' ELSE 'HOÀN TRẢ' END ) AS IMP_GROUP,min( CASE WHEN im.aggr_imp_mest_id IS NOT NULL THEN 'Tổng hợp trả' ELSE imt.imp_mest_type_name END ) AS imp_mest_type_name, min(  CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.medi_stock_name ELSE NULL END ) AS noi_xuat, min(imm.medicine_type_name) medicine_type_name, min(imm.medicine_type_code) medicine_type_code, min(imm.service_unit_name) service_unit_name, min(imm.imp_price) imp_price, min(imm.imp_vat_ratio) imp_vat_ratio, sum(imm.amount) as amount, sum(imm.amount * imm.imp_price * ( imm.imp_vat_ratio + 1 )) AS total_price from his_rs.v_his_imp_mest_medicine imm join his_rs.his_medicine_type maty on (maty.id=imm.medicine_type_id) left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }

                }
                query += "group by imm.medicine_type_name||'_'||imm.medicine_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END)) im_all order by IMP_GROUP,noi_xuat,key ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetImpMaterialType(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.key) OVER(PARTITION BY im_all.noi_xuat,im_all.IMP_GROUP) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.IMP_GROUP) as min_mest_type,im_all.*  from (select  imm.material_type_name||'_'||imm.material_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END) as key,min( CASE WHEN (ms.is_cabinet is null or ms.is_cabinet<>1)  and imm.imp_mest_type_id =1 THEN 'CHUYỂN KHO' ELSE 'HOÀN TRẢ' END ) AS IMP_GROUP,min( CASE WHEN im.aggr_imp_mest_id IS NOT NULL THEN 'Tổng hợp trả' ELSE imt.imp_mest_type_name END ) AS imp_mest_type_name, min(  CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.medi_stock_name ELSE NULL END ) AS noi_xuat, min(imm.material_type_name) material_type_name, min(imm.material_type_code) material_type_code, min(imm.service_unit_name) service_unit_name, min(imm.imp_price) imp_price, min(imm.imp_vat_ratio) imp_vat_ratio, sum(imm.amount) as amount, sum(imm.amount * imm.imp_price * ( imm.imp_vat_ratio + 1 )) AS total_price from his_rs.v_his_imp_mest_material imm join his_rs.his_material_type maty on (maty.id=imm.material_type_id and maty.is_CHEMICAL_SUBSTANCE is null) left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }

                }
                query += "group by imm.material_type_name||'_'||imm.material_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END)) im_all order by IMP_GROUP,noi_xuat,key ";


                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetImpChemicalType(Mrs00369Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select min(im_all.key) OVER(PARTITION BY im_all.noi_xuat,im_all.IMP_GROUP) as min_noi_xuat,min(im_all.noi_xuat) OVER(PARTITION BY im_all.IMP_GROUP) as min_mest_type,im_all.*  from (select imm.material_type_name||'_'||imm.material_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END) as key,min( CASE WHEN (ms.is_cabinet is null or ms.is_cabinet<>1) and  imm.imp_mest_type_id =1 THEN 'CHUYỂN KHO' ELSE 'HOÀN TRẢ' END ) AS IMP_GROUP,min(  CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.medi_stock_name ELSE NULL END ) AS noi_xuat, min(imm.material_type_name) material_type_name, min(imm.material_type_code) material_type_code, min(imm.service_unit_name) service_unit_name, min(imm.imp_price) imp_price, min(imm.imp_vat_ratio) imp_vat_ratio, sum(imm.amount) as amount, sum(imm.amount * imm.imp_price * ( imm.imp_vat_ratio + 1 )) AS total_price from his_rs.v_his_imp_mest_material imm join his_rs.his_material_type maty on (maty.id=imm.material_type_id and maty.is_CHEMICAL_SUBSTANCE =1) left join his_rs.his_imp_mest im on im.id = imm.imp_mest_id left join his_rs.his_imp_mest aggr on aggr.id = im.aggr_imp_mest_id join his_rs.his_imp_mest_type imt on imt.id = im.imp_mest_type_id left join his_rs.his_exp_mest chms on chms.id = im.chms_exp_mest_id left join his_rs.his_medi_stock ms on ms.id = chms.medi_stock_id left join his_rs.his_department dp on dp.id = imm.req_department_id where  imm.imp_mest_stt_id =5 and imm.is_delete =0 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND IMM.IMP_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND IMM.IMP_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.MEDI_BIG_STOCK_ID != null)
                {
                    query += string.Format("AND IMM.MEDI_STOCK_ID ={0} ", filter.MEDI_BIG_STOCK_ID);
                }

                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    if (filter.IMP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT))
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }
                    else
                    {
                        query += string.Format("AND ( imm.imp_mest_type_id IN (8, 1, 10, 12, 16, 11, 13, 7, 4, 3) OR aggr.id IS NOT NULL ) ", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                    }

                }
                query += "group by imm.material_type_name||'_'||imm.material_type_id||'_'||imm.imp_mest_type_id||'_'||imm.medi_stock_id||'_'||imm.imp_price||'_'||imm.imp_vat_ratio||'_'||( CASE WHEN (ms.IS_CABINET is null or ms.IS_CABINET<>1) and imm.imp_mest_type_id =1 THEN  ms.id ELSE NULL END)) im_all order by IMP_GROUP,noi_xuat,key ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
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
