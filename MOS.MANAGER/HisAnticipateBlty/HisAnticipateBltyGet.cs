using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateBlty
{
    partial class HisAnticipateBltyGet : BusinessBase
    {
        internal HisAnticipateBltyGet()
            : base()
        {

        }

        internal HisAnticipateBltyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ANTICIPATE_BLTY> Get(HisAnticipateBltyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateBltyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_BLTY GetById(long id)
        {
            try
            {
                return GetById(id, new HisAnticipateBltyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_BLTY GetById(long id, HisAnticipateBltyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateBltyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ANTICIPATE_BLTY> GetByAnticipateId(long anticipateId)
        {
            HisAnticipateBltyFilterQuery filter = new HisAnticipateBltyFilterQuery();
            filter.ANTICIPATE_ID = anticipateId;
            return this.Get(filter);
        }
    }
}
