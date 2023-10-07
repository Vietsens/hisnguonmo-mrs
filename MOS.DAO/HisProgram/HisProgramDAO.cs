using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisProgram
{
    public partial class HisProgramDAO : EntityBase
    {
        private HisProgramGet GetWorker
        {
            get
            {
                return (HisProgramGet)Worker.Get<HisProgramGet>();
            }
        }
        public List<HIS_PROGRAM> Get(HisProgramSO search, CommonParam param)
        {
            List<HIS_PROGRAM> result = new List<HIS_PROGRAM>();
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

        public HIS_PROGRAM GetById(long id, HisProgramSO search)
        {
            HIS_PROGRAM result = null;
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
