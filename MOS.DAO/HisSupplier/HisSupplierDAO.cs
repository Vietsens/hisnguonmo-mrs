using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSupplier
{
    public partial class HisSupplierDAO : EntityBase
    {
        private HisSupplierGet GetWorker
        {
            get
            {
                return (HisSupplierGet)Worker.Get<HisSupplierGet>();
            }
        }
        public List<HIS_SUPPLIER> Get(HisSupplierSO search, CommonParam param)
        {
            List<HIS_SUPPLIER> result = new List<HIS_SUPPLIER>();
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

        public HIS_SUPPLIER GetById(long id, HisSupplierSO search)
        {
            HIS_SUPPLIER result = null;
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
