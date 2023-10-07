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
        internal HisFilmSizeGet()
            : base()
        {

        }

        internal HisFilmSizeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_FILM_SIZE> Get(HisFilmSizeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFilmSizeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FILM_SIZE GetById(long id)
        {
            try
            {
                return GetById(id, new HisFilmSizeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FILM_SIZE GetById(long id, HisFilmSizeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFilmSizeDAO.GetById(id, filter.Query());
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
