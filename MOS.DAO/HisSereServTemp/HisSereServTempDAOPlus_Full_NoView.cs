using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServTemp
{
    public partial class HisSereServTempDAO : EntityBase
    {
        public HIS_SERE_SERV_TEMP GetByCode(string code, HisSereServTempSO search)
        {
            HIS_SERE_SERV_TEMP result = null;

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

        public Dictionary<string, HIS_SERE_SERV_TEMP> GetDicByCode(HisSereServTempSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERE_SERV_TEMP> result = new Dictionary<string, HIS_SERE_SERV_TEMP>();
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
