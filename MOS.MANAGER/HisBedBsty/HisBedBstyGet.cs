using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedBsty
{
    partial class HisBedBstyGet : BusinessBase
    {
        internal HisBedBstyGet()
            : base()
        {

        }

        internal HisBedBstyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BED_BSTY> Get(HisBedBstyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedBstyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_BSTY GetById(long id)
        {
            try
            {
                return GetById(id, new HisBedBstyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_BSTY GetById(long id, HisBedBstyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedBstyDAO.GetById(id, filter.Query());
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
