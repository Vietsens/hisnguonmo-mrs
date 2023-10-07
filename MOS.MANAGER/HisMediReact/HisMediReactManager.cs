using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReact
{
    public partial class HisMediReactManager : BusinessBase
    {
        public HisMediReactManager()
            : base()
        {

        }

        public HisMediReactManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDI_REACT> Get(HisMediReactFilterQuery filter)
        {
             List<HIS_MEDI_REACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_REACT> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_MEDI_REACT GetById(long data)
        {
             HIS_MEDI_REACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_REACT resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_MEDI_REACT GetById(long data, HisMediReactFilterQuery filter)
        {
             HIS_MEDI_REACT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_REACT resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  List<HIS_MEDI_REACT> GetByMedicineId(long data)
        {
             List<HIS_MEDI_REACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_REACT> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetByMedicineId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  List<HIS_MEDI_REACT> GetByMediReactTypeId(long data)
        {
             List<HIS_MEDI_REACT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_REACT> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactGet(param).GetByMediReactTypeId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
