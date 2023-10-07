using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediReact
{
    public partial class HisMediReactDAO : EntityBase
    {
        private HisMediReactGet GetWorker
        {
            get
            {
                return (HisMediReactGet)Worker.Get<HisMediReactGet>();
            }
        }
        public List<HIS_MEDI_REACT> Get(HisMediReactSO search, CommonParam param)
        {
            List<HIS_MEDI_REACT> result = new List<HIS_MEDI_REACT>();
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

        public HIS_MEDI_REACT GetById(long id, HisMediReactSO search)
        {
            HIS_MEDI_REACT result = null;
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
