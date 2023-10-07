using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisWorkPlace
{
    public partial class HisWorkPlaceDAO : EntityBase
    {
        public HIS_WORK_PLACE GetByCode(string code, HisWorkPlaceSO search)
        {
            HIS_WORK_PLACE result = null;

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

        public Dictionary<string, HIS_WORK_PLACE> GetDicByCode(HisWorkPlaceSO search, CommonParam param)
        {
            Dictionary<string, HIS_WORK_PLACE> result = new Dictionary<string, HIS_WORK_PLACE>();
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
