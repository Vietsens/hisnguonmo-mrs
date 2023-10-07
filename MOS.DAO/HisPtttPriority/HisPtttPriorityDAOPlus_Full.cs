using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttPriority
{
    public partial class HisPtttPriorityDAO : EntityBase
    {
        //public List<V_HIS_PTTT_PRIORITY> GetView(HisPtttPrioritySO search, CommonParam param)
        //{
        //    List<V_HIS_PTTT_PRIORITY> result = new List<V_HIS_PTTT_PRIORITY>();

        //    try
        //    {
        //        result = GetWorker.GetView(search, param);
        //    }
        //    catch (Exception ex)
        //    {
        //        param.HasException = true;
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result.Clear();
        //    }

        //    return result;
        //}

        public HIS_PTTT_PRIORITY GetByCode(string code, HisPtttPrioritySO search)
        {
            HIS_PTTT_PRIORITY result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        //public V_HIS_PTTT_PRIORITY GetViewById(long id, HisPtttPrioritySO search)
        //{
        //    V_HIS_PTTT_PRIORITY result = null;

        //    try
        //    {
        //        result = GetWorker.GetViewById(id, search);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }

        //    return result;
        //}

        //public V_HIS_PTTT_PRIORITY GetViewByCode(string code, HisPtttPrioritySO search)
        //{
        //    V_HIS_PTTT_PRIORITY result = null;

        //    try
        //    {
        //        result = GetWorker.GetViewByCode(code, search);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }
        //    return result;
        //}

        public Dictionary<string, HIS_PTTT_PRIORITY> GetDicByCode(HisPtttPrioritySO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_PRIORITY> result = new Dictionary<string, HIS_PTTT_PRIORITY>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
