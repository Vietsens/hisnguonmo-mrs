using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSaroExro
{
    partial class HisSaroExroGet : BusinessBase
    {
        internal HisSaroExroGet()
            : base()
        {

        }

        internal HisSaroExroGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SARO_EXRO> Get(HisSaroExroFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSaroExroDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SARO_EXRO> GetView(HisSaroExroViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSaroExroDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SARO_EXRO GetById(long id)
        {
            try
            {
                return GetById(id, new HisSaroExroFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SARO_EXRO GetById(long id, HisSaroExroFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSaroExroDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SARO_EXRO> GetBySampleRoomId(long sampleRoomId)
        {
            try
            {
                HisSaroExroFilterQuery filter = new HisSaroExroFilterQuery();
                filter.SAMPLE_ROOM_ID = sampleRoomId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SARO_EXRO> GetByExecuteRoomId(long executeRoomId)
        {
            try
            {
                HisSaroExroFilterQuery filter = new HisSaroExroFilterQuery();
                filter.EXECUTE_ROOM_ID = executeRoomId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SARO_EXRO> GetActive()
        {
            try
            {
                HisSaroExroFilterQuery filter = new HisSaroExroFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
