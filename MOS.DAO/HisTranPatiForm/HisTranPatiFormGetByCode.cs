using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTranPatiForm
{
    partial class HisTranPatiFormGet : EntityBase
    {
        public HIS_TRAN_PATI_FORM GetByCode(string code, HisTranPatiFormSO search)
        {
            HIS_TRAN_PATI_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_TRAN_PATI_FORM.AsQueryable().Where(p => p.TRAN_PATI_FORM_CODE == code);
                        if (search.listHisTranPatiFormExpression != null && search.listHisTranPatiFormExpression.Count > 0)
                        {
                            foreach (var item in search.listHisTranPatiFormExpression)
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
