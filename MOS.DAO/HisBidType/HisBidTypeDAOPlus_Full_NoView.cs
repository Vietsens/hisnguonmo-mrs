using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidType
{
    public partial class HisBidTypeDAO : EntityBase
    {
        public HIS_BID_TYPE GetByCode(string code, HisBidTypeSO search)
        {
            HIS_BID_TYPE result = null;

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

        public Dictionary<string, HIS_BID_TYPE> GetDicByCode(HisBidTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_BID_TYPE> result = new Dictionary<string, HIS_BID_TYPE>();
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
