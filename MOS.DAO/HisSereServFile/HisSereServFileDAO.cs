using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServFile
{
    public partial class HisSereServFileDAO : EntityBase
    {
        private HisSereServFileGet GetWorker
        {
            get
            {
                return (HisSereServFileGet)Worker.Get<HisSereServFileGet>();
            }
        }
        public List<HIS_SERE_SERV_FILE> Get(HisSereServFileSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_FILE> result = new List<HIS_SERE_SERV_FILE>();
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

        public HIS_SERE_SERV_FILE GetById(long id, HisSereServFileSO search)
        {
            HIS_SERE_SERV_FILE result = null;
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
