using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEventLog
{
    partial class TypeCollection
    {
        internal static readonly List<Type> SdaEventLog = new List<Type>() { typeof(SDA_EVENT_LOG), typeof(List<SDA_EVENT_LOG>) };
    }
}
