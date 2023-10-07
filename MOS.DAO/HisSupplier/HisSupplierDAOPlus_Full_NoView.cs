using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSupplier
{
    public partial class HisSupplierDAO : EntityBase
    {
        public HIS_SUPPLIER GetByCode(string code, HisSupplierSO search)
        {
            HIS_SUPPLIER result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_SUPPLIER> GetDicByCode(HisSupplierSO search, CommonParam param)
        {
            Dictionary<string, HIS_SUPPLIER> result = new Dictionary<string, HIS_SUPPLIER>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
