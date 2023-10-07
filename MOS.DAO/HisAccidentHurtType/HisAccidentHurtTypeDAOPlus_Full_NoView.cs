using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHurtType
{
    public partial class HisAccidentHurtTypeDAO : EntityBase
    {
        public HIS_ACCIDENT_HURT_TYPE GetByCode(string code, HisAccidentHurtTypeSO search)
        {
            HIS_ACCIDENT_HURT_TYPE result = null;

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

        public Dictionary<string, HIS_ACCIDENT_HURT_TYPE> GetDicByCode(HisAccidentHurtTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_HURT_TYPE> result = new Dictionary<string, HIS_ACCIDENT_HURT_TYPE>();
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
