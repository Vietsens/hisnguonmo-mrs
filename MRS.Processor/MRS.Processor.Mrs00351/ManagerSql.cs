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

namespace MRS.Processor.Mrs00351
{
    public partial class ManagerSql : BusinessBase
    {

        //public List<HIS_IMP_MEST_MATERIAL> Get(List<long> ThExpMestMaterialIds)
        //{
        //    List<HIS_IMP_MEST_MATERIAL> result = new List<HIS_IMP_MEST_MATERIAL>();
        //    CommonParam paramGet = new CommonParam();
        //        string query = "";
        //        query += "SELECT * ";

        //        query += "FROM HIS_IMP_MEST_MATERIAL IMMM ";
        //        query += "WHERE IMMM.IMP_MEST_STT_ID=5 AND IMMM.IS_DELETE =0 AND IMMM.IMP_MEST_ID IS NOT NULL ";
        //        query += string.Format("AND IMMM.TH_EXP_MEST_MATERIAL_ID IN ({0}) ", string.Join(",", ThExpMestMaterialIds));
              
        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        result = new SqlDAO().GetSql<HIS_IMP_MEST_MATERIAL>(paramGet, query);
        //        if (paramGet.HasException)
        //            throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00351");
        //    return result;
        //}


        public List<V_HIS_IMP_MEST_MEDICINE> Get(List<long> ThExpMestMedicineIds)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT * ";

            query += "FROM V_HIS_IMP_MEST_MEDICINE IMMM ";
            query += "WHERE IMMM.IMP_MEST_STT_ID=5 AND IMMM.IS_DELETE =0 AND IMMM.IMP_MEST_ID IS NOT NULL ";
            query += string.Format("AND IMMM.TH_EXP_MEST_MEDICINE_ID IN ({0}) ", string.Join(",", ThExpMestMedicineIds));

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00351");
            return result;
        }
    }
}
