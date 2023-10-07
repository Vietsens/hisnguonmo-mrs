using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisFilmSize
{
    public partial class HisFilmSizeDAO : EntityBase
    {
        public HIS_FILM_SIZE GetByCode(string code, HisFilmSizeSO search)
        {
            HIS_FILM_SIZE result = null;

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

        public Dictionary<string, HIS_FILM_SIZE> GetDicByCode(HisFilmSizeSO search, CommonParam param)
        {
            Dictionary<string, HIS_FILM_SIZE> result = new Dictionary<string, HIS_FILM_SIZE>();
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
