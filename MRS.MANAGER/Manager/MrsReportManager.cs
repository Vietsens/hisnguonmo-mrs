using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.SDO;
using System;

namespace MRS.MANAGER.Manager
{
    public partial class MrsReportManager : ManagerBase
    {
        public MrsReportManager(CommonParam param)
            : base(param)
        {

        }

        public object GetInput(string data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                if (InputDatGetCFG.INPUTs!= null && InputDatGetCFG.INPUTs.Keys.Count>0)
                {
                    foreach (var item in InputDatGetCFG.INPUTs.Keys)
                    {
                        if (data.Contains(item))
                        {
                            result = InputDatGetCFG.INPUTs[item];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        public object Create(CreateReportSDO data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                MrsReportBO bo = new MrsReportBO();
                var ro = bo.Create(data);
                result = (ro != null && ro.ID > 0);
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        public object CreateReq(CreateReportSDO data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                MrsReportBO bo = new MrsReportBO();
                result = bo.Create(data);
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        public object CreateData(CreateReportSDO data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                MrsReportBO bo = new MrsReportBO();
                result = bo.CreateData(data);
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        public object CreateByte(CreateByteSDO data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                MrsReportBO bo = new MrsReportBO();
                result = bo.CreateByte(data);
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        public object CreateByCalendar(CreateReportSDO data)
        {
            object result = null;
            try
            {
                //if (!IsNotNull(data)) throw new ArgumentNullException("data");
                //new TokenManager().Init();
                //MrsReportBO bo = new MrsReportBO();
                //result = bo.Create(data);
                //CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public object Refresh()
        {
            object result = null;
            try
            {
                
                //Khoi tao cac cau hinh nghiep vu he thong
                InputDatGetCFG.Refresh();
                result = MRS.MANAGER.Config.Loader.Refresh();
                var inputData = InputDatGetCFG.INPUTs;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
