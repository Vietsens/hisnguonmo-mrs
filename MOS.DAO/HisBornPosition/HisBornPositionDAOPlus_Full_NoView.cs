using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBornPosition
{
    public partial class HisBornPositionDAO : EntityBase
    {
        public HIS_BORN_POSITION GetByCode(string code, HisBornPositionSO search)
        {
            HIS_BORN_POSITION result = null;

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

        public Dictionary<string, HIS_BORN_POSITION> GetDicByCode(HisBornPositionSO search, CommonParam param)
        {
            Dictionary<string, HIS_BORN_POSITION> result = new Dictionary<string, HIS_BORN_POSITION>();
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
