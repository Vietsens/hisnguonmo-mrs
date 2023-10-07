using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediReactType
{
    public partial class HisMediReactTypeDAO : EntityBase
    {
        public HIS_MEDI_REACT_TYPE GetByCode(string code, HisMediReactTypeSO search)
        {
            HIS_MEDI_REACT_TYPE result = null;

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

        public Dictionary<string, HIS_MEDI_REACT_TYPE> GetDicByCode(HisMediReactTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDI_REACT_TYPE> result = new Dictionary<string, HIS_MEDI_REACT_TYPE>();
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
