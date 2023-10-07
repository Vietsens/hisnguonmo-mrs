using SDA.EFMODEL.DataModels;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEventLog.Get.ListEv
{
    interface ISdaEventLogGetListEv
    {
        List<SDA_EVENT_LOG> Run();
    }
}
