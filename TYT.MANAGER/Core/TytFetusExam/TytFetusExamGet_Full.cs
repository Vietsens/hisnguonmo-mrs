using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Core.TytFetusExam.Get.Ev;
using TYT.MANAGER.Core.TytFetusExam.Get.ListEv;
using TYT.MANAGER.Core.TytFetusExam.Get.ListV;
using TYT.MANAGER.Core.TytFetusExam.Get.V;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusExam
{
    partial class TytFetusExamGet : BeanObjectBase, IDelegacyT
    {
        object entity;

        internal TytFetusExamGet(CommonParam param, object data)
            : base(param)
        {
            entity = data;
        }

        T IDelegacyT.Execute<T>()
        {
            T result = default(T);
            try
            {
                if (typeof(T) == typeof(List<TYT_FETUS_EXAM>))
                {
                    ITytFetusExamGetListEv behavior = TytFetusExamGetListEvBehaviorFactory.MakeITytFetusExamGetListEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(TYT_FETUS_EXAM))
                {
                    ITytFetusExamGetEv behavior = TytFetusExamGetEvBehaviorFactory.MakeITytFetusExamGetEv(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(List<V_TYT_FETUS_EXAM>))
                {
                    ITytFetusExamGetListV behavior = TytFetusExamGetListVBehaviorFactory.MakeITytFetusExamGetListV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
                else if (typeof(T) == typeof(V_TYT_FETUS_EXAM))
                {
                    ITytFetusExamGetV behavior = TytFetusExamGetVBehaviorFactory.MakeITytFetusExamGetV(param, entity);
                    result = behavior != null ? (T)System.Convert.ChangeType(behavior.Run(), typeof(T)) : default(T);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = default(T);
            }
            return result;
        }
    }
}
