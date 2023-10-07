using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisProgram
{
    public partial class HisProgramDAO : EntityBase
    {
        public HIS_PROGRAM GetByCode(string code, HisProgramSO search)
        {
            HIS_PROGRAM result = null;

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

        public Dictionary<string, HIS_PROGRAM> GetDicByCode(HisProgramSO search, CommonParam param)
        {
            Dictionary<string, HIS_PROGRAM> result = new Dictionary<string, HIS_PROGRAM>();
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
