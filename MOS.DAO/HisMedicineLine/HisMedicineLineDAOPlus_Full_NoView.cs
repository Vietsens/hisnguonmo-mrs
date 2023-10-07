using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineLine
{
    public partial class HisMedicineLineDAO : EntityBase
    {
        public HIS_MEDICINE_LINE GetByCode(string code, HisMedicineLineSO search)
        {
            HIS_MEDICINE_LINE result = null;

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

        public Dictionary<string, HIS_MEDICINE_LINE> GetDicByCode(HisMedicineLineSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDICINE_LINE> result = new Dictionary<string, HIS_MEDICINE_LINE>();
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
