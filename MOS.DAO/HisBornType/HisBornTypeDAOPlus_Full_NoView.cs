using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBornType
{
    public partial class HisBornTypeDAO : EntityBase
    {
        public HIS_BORN_TYPE GetByCode(string code, HisBornTypeSO search)
        {
            HIS_BORN_TYPE result = null;

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

        public Dictionary<string, HIS_BORN_TYPE> GetDicByCode(HisBornTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_BORN_TYPE> result = new Dictionary<string, HIS_BORN_TYPE>();
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
