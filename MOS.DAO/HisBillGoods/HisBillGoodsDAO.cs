using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBillGoods
{
    public partial class HisBillGoodsDAO : EntityBase
    {
        private HisBillGoodsGet GetWorker
        {
            get
            {
                return (HisBillGoodsGet)Worker.Get<HisBillGoodsGet>();
            }
        }
        public List<HIS_BILL_GOODS> Get(HisBillGoodsSO search, CommonParam param)
        {
            List<HIS_BILL_GOODS> result = new List<HIS_BILL_GOODS>();
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

        public HIS_BILL_GOODS GetById(long id, HisBillGoodsSO search)
        {
            HIS_BILL_GOODS result = null;
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
