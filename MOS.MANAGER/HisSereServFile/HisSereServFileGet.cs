using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServFile
{
    class HisSereServFileGet : BusinessBase
    {
        internal HisSereServFileGet()
            : base()
        {

        }

        internal HisSereServFileGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_FILE> Get(HisSereServFileFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServFileDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_FILE GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServFileFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_FILE GetById(long id, HisSereServFileFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServFileDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal FileHolder GetFile(long id)
        {
            try
            {
                FileHolder result = null;
                HIS_SERE_SERV_FILE dto = this.GetById(id);
                if (dto != null && !string.IsNullOrWhiteSpace(dto.URL))
                {
                    System.IO.MemoryStream stream = Inventec.Fss.Client.FileDownload.GetFile(dto.URL);
                    if (stream != null)
                    {
                        result = new FileHolder();
                        result.Content = stream;
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_FILE> GetBySereServIds(List<long> sereServIds)
        {
            if (sereServIds != null)
            {
                HisSereServFileFilterQuery filter = new HisSereServFileFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
                return this.Get(filter);
            }
            return null;
        }
    }
}
