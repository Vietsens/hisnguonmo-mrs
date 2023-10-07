using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediOrg
{
    public partial class HisMediOrgDAO : EntityBase
    {
        public HIS_MEDI_ORG GetByCode(string code, HisMediOrgSO search)
        {
            HIS_MEDI_ORG result = null;

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

        public Dictionary<string, HIS_MEDI_ORG> GetDicByCode(HisMediOrgSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDI_ORG> result = new Dictionary<string, HIS_MEDI_ORG>();
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
