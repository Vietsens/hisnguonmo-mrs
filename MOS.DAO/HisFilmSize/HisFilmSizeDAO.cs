using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisFilmSize
{
    public partial class HisFilmSizeDAO : EntityBase
    {
        private HisFilmSizeGet GetWorker
        {
            get
            {
                return (HisFilmSizeGet)Worker.Get<HisFilmSizeGet>();
            }
        }
        public List<HIS_FILM_SIZE> Get(HisFilmSizeSO search, CommonParam param)
        {
            List<HIS_FILM_SIZE> result = new List<HIS_FILM_SIZE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_FILM_SIZE GetById(long id, HisFilmSizeSO search)
        {
            HIS_FILM_SIZE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
