using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisFilmSize
{
    public partial class HisFilmSizeManager : BusinessBase
    {
        public HisFilmSizeManager()
            : base()
        {

        }

        public HisFilmSizeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_FILM_SIZE> Get(HisFilmSizeFilterQuery filter)
        {
            List<HIS_FILM_SIZE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_FILM_SIZE> resultData = null;
                if (valid)
                {
                    resultData = new HisFilmSizeGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_FILM_SIZE GetById(long data)
        {
            HIS_FILM_SIZE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_FILM_SIZE resultData = null;
                if (valid)
                {
                    resultData = new HisFilmSizeGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_FILM_SIZE GetById(long data, HisFilmSizeFilterQuery filter)
        {
            HIS_FILM_SIZE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_FILM_SIZE resultData = null;
                if (valid)
                {
                    resultData = new HisFilmSizeGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_FILM_SIZE GetByCode(string data)
        {
            HIS_FILM_SIZE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_FILM_SIZE resultData = null;
                if (valid)
                {
                    resultData = new HisFilmSizeGet(param).GetByCode(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_FILM_SIZE GetByCode(string data, HisFilmSizeFilterQuery filter)
        {
            HIS_FILM_SIZE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_FILM_SIZE resultData = null;
                if (valid)
                {
                    resultData = new HisFilmSizeGet(param).GetByCode(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
