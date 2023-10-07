using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisMedicineUseForm
{
    partial class HisMedicineUseFormGet : EntityBase
    {
        public HIS_MEDICINE_USE_FORM GetByCode(string code, HisMedicineUseFormSO search)
        {
            HIS_MEDICINE_USE_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNullOrEmpty(code);
                if (valid)
                {
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        var query = ctx.HIS_MEDICINE_USE_FORM.AsQueryable().Where(p => p.MEDICINE_USE_FORM_CODE == code);
                        if (search.listHisMedicineUseFormExpression != null && search.listHisMedicineUseFormExpression.Count > 0)
                        {
                            foreach (var item in search.listHisMedicineUseFormExpression)
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
