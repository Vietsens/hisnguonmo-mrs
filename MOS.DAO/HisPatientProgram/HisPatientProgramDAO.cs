using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientProgram
{
    public partial class HisPatientProgramDAO : EntityBase
    {
        private HisPatientProgramGet GetWorker
        {
            get
            {
                return (HisPatientProgramGet)Worker.Get<HisPatientProgramGet>();
            }
        }
        public List<HIS_PATIENT_PROGRAM> Get(HisPatientProgramSO search, CommonParam param)
        {
            List<HIS_PATIENT_PROGRAM> result = new List<HIS_PATIENT_PROGRAM>();
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

        public HIS_PATIENT_PROGRAM GetById(long id, HisPatientProgramSO search)
        {
            HIS_PATIENT_PROGRAM result = null;
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
