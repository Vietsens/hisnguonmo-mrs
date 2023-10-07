using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttCatastrophe
{
    partial class HisPtttCatastropheGet : BusinessBase
    {
        internal HisPtttCatastropheGet()
            : base()
        {

        }

        internal HisPtttCatastropheGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PTTT_CATASTROPHE> Get(HisPtttCatastropheFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttCatastropheDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CATASTROPHE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPtttCatastropheFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CATASTROPHE GetById(long id, HisPtttCatastropheFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttCatastropheDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CATASTROPHE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPtttCatastropheFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PTTT_CATASTROPHE GetByCode(string code, HisPtttCatastropheFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPtttCatastropheDAO.GetByCode(code, filter.Query());
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
