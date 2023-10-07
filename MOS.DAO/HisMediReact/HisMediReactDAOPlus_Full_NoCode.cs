using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediReact
{
    public partial class HisMediReactDAO : EntityBase
    {
        public List<V_HIS_MEDI_REACT> GetView(HisMediReactSO search, CommonParam param)
        {
            List<V_HIS_MEDI_REACT> result = new List<V_HIS_MEDI_REACT>();
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

        public V_HIS_MEDI_REACT GetViewById(long id, HisMediReactSO search)
        {
            V_HIS_MEDI_REACT result = null;

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
