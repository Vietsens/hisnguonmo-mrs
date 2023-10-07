using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExamSereDire
{
    public partial class HisExamSereDireDAO : EntityBase
    {
        private HisExamSereDireGet GetWorker
        {
            get
            {
                return (HisExamSereDireGet)Worker.Get<HisExamSereDireGet>();
            }
        }
        public List<HIS_EXAM_SERE_DIRE> Get(HisExamSereDireSO search, CommonParam param)
        {
            List<HIS_EXAM_SERE_DIRE> result = new List<HIS_EXAM_SERE_DIRE>();
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

        public HIS_EXAM_SERE_DIRE GetById(long id, HisExamSereDireSO search)
        {
            HIS_EXAM_SERE_DIRE result = null;
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
