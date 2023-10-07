using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSaroExro
{
    public partial class HisSaroExroDAO : EntityBase
    {
        public List<V_HIS_SARO_EXRO> GetView(HisSaroExroSO search, CommonParam param)
        {
            List<V_HIS_SARO_EXRO> result = new List<V_HIS_SARO_EXRO>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_SARO_EXRO GetViewById(long id, HisSaroExroSO search)
        {
            V_HIS_SARO_EXRO result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
