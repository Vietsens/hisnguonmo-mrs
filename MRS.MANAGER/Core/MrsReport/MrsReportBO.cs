using Inventec.Core;
using MRS.MANAGER.Manager;
using MRS.SDO;
using SAR.EFMODEL.DataModels;
using System;

namespace MRS.MANAGER.Core.MrsReport
{
    partial class MrsReportBO : BusinessObjectBase
    {
        private static ProcessorFactory factory = new ProcessorFactory();

        internal MrsReportBO()
            : base()
        {

        }

        internal SAR_REPORT Create(CreateReportSDO data)
        {
            SAR_REPORT result = null;
            try
            {
                AbstractProcessor processor = null;
                if (IsNotNull(data))
                {
                    processor = factory.GetProcessor(data.ReportTypeCode, param);
                }

                if (processor != null)
                {
                    result = processor.Run(data);
                }
                else
                {
                    Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__KhongTimDuocDLL, data.ReportTypeCode);
                    Inventec.Common.Logging.LogSystem.Error("khong tim thay dll  cua bao cao: " + data.ReportTypeCode);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal object CreateData(CreateReportSDO data)
        {
            object result = null;
            try
            {
                AbstractProcessor processor = null;
                if (IsNotNull(data))
                {
                    processor = factory.GetProcessor(data.ReportTypeCode, param);
                }

                if (processor != null)
                {
                    result = processor.RunData(data);
                }
                else
                {
                    Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__KhongTimDuocDLL, data.ReportTypeCode);
                    Inventec.Common.Logging.LogSystem.Error("khong tim thay dll  cua bao cao: " + data.ReportTypeCode);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal object CreateByte(CreateByteSDO data)
        {
            object result = null;
            try
            {
                AbstractProcessor processor = null;
                if (IsNotNull(data))
                {
                    processor = factory.GetProcessor("TKB", param);
                }

                if (processor != null)
                {
                    result = processor.RunByte(data);
                }
                else
                {
                    Base.MessageUtil.SetMessage(param, LibraryMessage.Message.Enum.Core_MrsReport_Create__KhongTimDuocDLL, "");
                    Inventec.Common.Logging.LogSystem.Error("khong tim thay dll  cua bao cao: " +"");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
