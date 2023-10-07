using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillGoods
{
    partial class HisBillGoodsGet : BusinessBase
    {
        internal HisBillGoodsGet()
            : base()
        {

        }

        internal HisBillGoodsGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BILL_GOODS> Get(HisBillGoodsFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillGoodsDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BILL_GOODS GetById(long id)
        {
            try
            {
                return GetById(id, new HisBillGoodsFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BILL_GOODS GetById(long id, HisBillGoodsFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillGoodsDAO.GetById(id, filter.Query());
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
