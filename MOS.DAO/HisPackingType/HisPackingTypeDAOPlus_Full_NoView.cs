using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPackingType
{
    public partial class HisPackingTypeDAO : EntityBase
    {
        public HIS_PACKING_TYPE GetByCode(string code, HisPackingTypeSO search)
        {
            HIS_PACKING_TYPE result = null;

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

        public Dictionary<string, HIS_PACKING_TYPE> GetDicByCode(HisPackingTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_PACKING_TYPE> result = new Dictionary<string, HIS_PACKING_TYPE>();
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
