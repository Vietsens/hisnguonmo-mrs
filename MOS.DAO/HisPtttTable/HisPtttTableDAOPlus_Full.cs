using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttTable
{
    public partial class HisPtttTableDAO : EntityBase
    {
        //public List<V_HIS_PTTT_TABLE> GetView(HisPtttTableSO search, CommonParam param)
        //{
        //    List<V_HIS_PTTT_TABLE> result = new List<V_HIS_PTTT_TABLE>();

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

        public HIS_PTTT_TABLE GetByCode(string code, HisPtttTableSO search)
        {
            HIS_PTTT_TABLE result = null;

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
        
        //public V_HIS_PTTT_TABLE GetViewById(long id, HisPtttTableSO search)
        //{
        //    V_HIS_PTTT_TABLE result = null;

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

        //public V_HIS_PTTT_TABLE GetViewByCode(string code, HisPtttTableSO search)
        //{
        //    V_HIS_PTTT_TABLE result = null;

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

        public Dictionary<string, HIS_PTTT_TABLE> GetDicByCode(HisPtttTableSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_TABLE> result = new Dictionary<string, HIS_PTTT_TABLE>();
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
