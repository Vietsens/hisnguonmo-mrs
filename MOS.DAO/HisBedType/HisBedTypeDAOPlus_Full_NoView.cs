using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedType
{
    public partial class HisBedTypeDAO : EntityBase
    {
        public HIS_BED_TYPE GetByCode(string code, HisBedTypeSO search)
        {
            HIS_BED_TYPE result = null;

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

        public Dictionary<string, HIS_BED_TYPE> GetDicByCode(HisBedTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_BED_TYPE> result = new Dictionary<string, HIS_BED_TYPE>();
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
