using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;

namespace MOS.DAO.HisAccidentBodyPart
{
    partial class HisAccidentBodyPartGet : EntityBase
    {
        public HIS_ACCIDENT_BODY_PART GetByCode(string code, HisAccidentBodyPartSO search)
        {
            HIS_ACCIDENT_BODY_PART result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_ACCIDENT_BODY_PART.AsQueryable().Where(p => p.ACCIDENT_BODY_PART_CODE == code);
                        if (search.listHisAccidentBodyPartExpression != null && search.listHisAccidentBodyPartExpression.Count > 0)
                        {
                            foreach (var item in search.listHisAccidentBodyPartExpression)
                            {
                                query = query.Where(item);
                            }
                        }
                        result = query.SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
