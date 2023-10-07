using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisManufacturer
{
    public partial class HisManufacturerDAO : EntityBase
    {
        public HIS_MANUFACTURER GetByCode(string code, HisManufacturerSO search)
        {
            HIS_MANUFACTURER result = null;

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

        public Dictionary<string, HIS_MANUFACTURER> GetDicByCode(HisManufacturerSO search, CommonParam param)
        {
            Dictionary<string, HIS_MANUFACTURER> result = new Dictionary<string, HIS_MANUFACTURER>();
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
