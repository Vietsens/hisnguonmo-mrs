using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDepartmentTran
{
    public partial class HisDepartmentTranDAO : EntityBase
    {
        private HisDepartmentTranGet GetWorker
        {
            get
            {
                return (HisDepartmentTranGet)Worker.Get<HisDepartmentTranGet>();
            }
        }
        public List<HIS_DEPARTMENT_TRAN> Get(HisDepartmentTranSO search, CommonParam param)
        {
            List<HIS_DEPARTMENT_TRAN> result = new List<HIS_DEPARTMENT_TRAN>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_DEPARTMENT_TRAN GetById(long id, HisDepartmentTranSO search)
        {
            HIS_DEPARTMENT_TRAN result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
