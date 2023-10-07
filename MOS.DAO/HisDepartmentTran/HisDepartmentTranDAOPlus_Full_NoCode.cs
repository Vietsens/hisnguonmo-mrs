using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDepartmentTran
{
    public partial class HisDepartmentTranDAO : EntityBase
    {
        public List<V_HIS_DEPARTMENT_TRAN> GetView(HisDepartmentTranSO search, CommonParam param)
        {
            List<V_HIS_DEPARTMENT_TRAN> result = new List<V_HIS_DEPARTMENT_TRAN>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_DEPARTMENT_TRAN GetViewById(long id, HisDepartmentTranSO search)
        {
            V_HIS_DEPARTMENT_TRAN result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
