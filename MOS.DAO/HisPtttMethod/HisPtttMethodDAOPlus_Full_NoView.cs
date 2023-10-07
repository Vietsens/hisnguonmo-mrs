using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttMethod
{
    public partial class HisPtttMethodDAO : EntityBase
    {
        public HIS_PTTT_METHOD GetByCode(string code, HisPtttMethodSO search)
        {
            HIS_PTTT_METHOD result = null;

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

        public Dictionary<string, HIS_PTTT_METHOD> GetDicByCode(HisPtttMethodSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_METHOD> result = new Dictionary<string, HIS_PTTT_METHOD>();
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
