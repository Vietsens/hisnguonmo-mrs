using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisVitaminA
{
    public partial class HisVitaminADAO : EntityBase
    {
        public List<V_HIS_VITAMIN_A> GetView(HisVitaminASO search, CommonParam param)
        {
            List<V_HIS_VITAMIN_A> result = new List<V_HIS_VITAMIN_A>();
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

        public V_HIS_VITAMIN_A GetViewById(long id, HisVitaminASO search)
        {
            V_HIS_VITAMIN_A result = null;

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
