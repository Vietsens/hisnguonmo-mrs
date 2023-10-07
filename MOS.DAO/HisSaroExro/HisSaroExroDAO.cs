using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSaroExro
{
    public partial class HisSaroExroDAO : EntityBase
    {
        private HisSaroExroGet GetWorker
        {
            get
            {
                return (HisSaroExroGet)Worker.Get<HisSaroExroGet>();
            }
        }
        public List<HIS_SARO_EXRO> Get(HisSaroExroSO search, CommonParam param)
        {
            List<HIS_SARO_EXRO> result = new List<HIS_SARO_EXRO>();
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

        public HIS_SARO_EXRO GetById(long id, HisSaroExroSO search)
        {
            HIS_SARO_EXRO result = null;
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
