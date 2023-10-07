using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttCatastrophe
{
    public partial class HisPtttCatastropheDAO : EntityBase
    {
        public HIS_PTTT_CATASTROPHE GetByCode(string code, HisPtttCatastropheSO search)
        {
            HIS_PTTT_CATASTROPHE result = null;

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

        public Dictionary<string, HIS_PTTT_CATASTROPHE> GetDicByCode(HisPtttCatastropheSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_CATASTROPHE> result = new Dictionary<string, HIS_PTTT_CATASTROPHE>();
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
