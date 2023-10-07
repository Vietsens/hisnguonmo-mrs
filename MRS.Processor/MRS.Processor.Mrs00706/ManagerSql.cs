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

namespace MRS.Processor.Mrs00706
{
    internal  class ManagerSql
    {

        internal List<Mrs00706RDO> GetMedicineType()
        {
            List<Mrs00706RDO> result = new List<Mrs00706RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --danh sach loai thuoc \n";
            query += "select \n";
            query += "* \n";
            query += "from v_his_medicine_type mt \n";
            query += "where 1=1 \n";
            query += "and mt.is_active = 1 \n";
            query += "and mt.is_delete=0 \n";
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00706RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00706");

            return result;
        }

        internal List<MedicineTypePrice> GetPrice()
        {
            List<MedicineTypePrice> result = new List<MedicineTypePrice>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "--danh sach loai thuoc gia xuat \n";
            query += "select \n";
            query += "emm.medicine_type_id, \n";
            query += "emm.price \n";
            query += "from v_his_exp_mest_medicine emm \n";
            query += "where 1=1 \n";
            query += "and emm.is_export=1 \n";
            query += "and price>0 \n";
            query += "and emm.exp_mest_type_id in (1,8,9,11,15) \n";
            query += "order by emm.exp_time desc \n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MedicineTypePrice>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00706");

            return result;
        }

        internal List<MedicineTypeSupplier> GetSupplier()
        {
            List<MedicineTypeSupplier> result = new List<MedicineTypeSupplier>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "--danh sach loai thuoc nha cung cap \n";
            query += "select  \n";
            query += "imm.medicine_type_id, \n";
            query += "imm.supplier_name \n";
            query += "from v_his_imp_mest_medicine imm \n";
            query += "where 1=1 \n";
            query += "and imm.supplier_name is not null \n";
            query += "and imm.imp_mest_stt_id=5 \n";
            query += "and imm.imp_mest_type_id in (2,6) \n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MedicineTypeSupplier>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00706");

            return result;
        }

        internal List<MediStockMety> GetMediStockMety(Mrs00706Filter filter)
        {
            List<MediStockMety> result = new List<MediStockMety>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "--danh sach loai thuoc kho \n";
            query += "select  \n";
            query += "msm.medicine_type_id, \n";
            query += "msm.medi_stock_id \n";
            query += "from his_medi_stock_mety msm \n";
            query += "where 1=1 \n";
            if(filter.MEDI_STOCK_ID !=null)
            {
                query += string.Format("and msm.medi_stock_id ={0} \n", filter.MEDI_STOCK_ID);
            }
            if (filter.MEDI_STOCK_IDs != null)
            {
                query += string.Format("and msm.medi_stock_id in({0}) \n", string.Join(",", filter.MEDI_STOCK_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<MediStockMety>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00706");

            return result;
        }

      
       
    }
}
