using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.PeriodBO
{
    partial class TypeCollection
    {
        internal static readonly List<Type> HtcPeriod = new List<Type>() { typeof(HTC_PERIOD), typeof(List<HTC_PERIOD>) };
        internal static readonly List<Type> HtcPeriodDepartment = new List<Type>() { typeof(HTC_PERIOD_DEPARTMENT), typeof(List<HTC_PERIOD_DEPARTMENT>) };
    }
}
