using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisFund
{
    public partial class HisFundDAO : EntityBase
    {
        public HIS_FUND GetByCode(string code, HisFundSO search)
        {
            HIS_FUND result = null;

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

        public Dictionary<string, HIS_FUND> GetDicByCode(HisFundSO search, CommonParam param)
        {
            Dictionary<string, HIS_FUND> result = new Dictionary<string, HIS_FUND>();
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
