using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisFilmSize
{
    partial class HisFilmSizeGet : BusinessBase
    {
        internal HIS_FILM_SIZE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisFilmSizeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FILM_SIZE GetByCode(string code, HisFilmSizeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFilmSizeDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
