using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPaanLiquid
{
    public partial class HisPaanLiquidDAO : EntityBase
    {
        public HIS_PAAN_LIQUID GetByCode(string code, HisPaanLiquidSO search)
        {
            HIS_PAAN_LIQUID result = null;

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

        public Dictionary<string, HIS_PAAN_LIQUID> GetDicByCode(HisPaanLiquidSO search, CommonParam param)
        {
            Dictionary<string, HIS_PAAN_LIQUID> result = new Dictionary<string, HIS_PAAN_LIQUID>();
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
