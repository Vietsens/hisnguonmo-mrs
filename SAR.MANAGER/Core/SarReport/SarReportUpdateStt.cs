using SAR.MANAGER.Core.SarReport.UpdateStt;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport
{
    partial class SarReportUpdateStt : BeanObjectBase, IDelegacy
    {
        object entity;

        internal SarReportUpdateStt(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        bool IDelegacy.Execute()
        {
            bool result = false;
            try
            {
                if (TypeCollection.SarReport.Contains(entity.GetType()))
                {
                    ISarReportUpdateStt behavior = SarReportUpdateSttBehaviorFactory.MakeISarReportUpdateStt(param, entity);
                    result = behavior != null ? behavior.Run() : false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
