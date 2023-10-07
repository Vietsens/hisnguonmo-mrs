using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttGroup
{
    public partial class HisPtttGroupDAO : EntityBase
    {
        //public List<V_HIS_PTTT_GROUP> GetView(HisPtttGroupSO search, CommonParam param)
        //{
        //    List<V_HIS_PTTT_GROUP> result = new List<V_HIS_PTTT_GROUP>();

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

        public HIS_PTTT_GROUP GetByCode(string code, HisPtttGroupSO search)
        {
            HIS_PTTT_GROUP result = null;

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
        
        //public V_HIS_PTTT_GROUP GetViewById(long id, HisPtttGroupSO search)
        //{
        //    V_HIS_PTTT_GROUP result = null;

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

        //public V_HIS_PTTT_GROUP GetViewByCode(string code, HisPtttGroupSO search)
        //{
        //    V_HIS_PTTT_GROUP result = null;

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

        public Dictionary<string, HIS_PTTT_GROUP> GetDicByCode(HisPtttGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_GROUP> result = new Dictionary<string, HIS_PTTT_GROUP>();
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
