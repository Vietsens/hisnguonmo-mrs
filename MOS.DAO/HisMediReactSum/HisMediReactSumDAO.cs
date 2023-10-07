using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediReactSum
{
    public partial class HisMediReactSumDAO : EntityBase
    {
        private HisMediReactSumGet GetWorker
        {
            get
            {
                return (HisMediReactSumGet)Worker.Get<HisMediReactSumGet>();
            }
        }
        public List<HIS_MEDI_REACT_SUM> Get(HisMediReactSumSO search, CommonParam param)
        {
            List<HIS_MEDI_REACT_SUM> result = new List<HIS_MEDI_REACT_SUM>();
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

        public HIS_MEDI_REACT_SUM GetById(long id, HisMediReactSumSO search)
        {
            HIS_MEDI_REACT_SUM result = null;
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
