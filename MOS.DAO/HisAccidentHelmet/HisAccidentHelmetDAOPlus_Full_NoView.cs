using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHelmet
{
    public partial class HisAccidentHelmetDAO : EntityBase
    {
        public HIS_ACCIDENT_HELMET GetByCode(string code, HisAccidentHelmetSO search)
        {
            HIS_ACCIDENT_HELMET result = null;

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

        public Dictionary<string, HIS_ACCIDENT_HELMET> GetDicByCode(HisAccidentHelmetSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_HELMET> result = new Dictionary<string, HIS_ACCIDENT_HELMET>();
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
