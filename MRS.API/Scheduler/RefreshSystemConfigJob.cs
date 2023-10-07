using Inventec.Common.Logging;
using Inventec.Common.Scheduler;
using System;

namespace MRS.API.Scheduler
{
    public class RefreshSystemConfigJob : Job
    {
        private int repeatTime;
        public int RepeatTime
        {
            get
            {
                if (repeatTime <= 0)
                {
                    try
                    {
                        repeatTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MRS.API.Scheduler.RefreshSystemConfigJob"]);
                        return repeatTime;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        repeatTime = 0;
                        return 1800000;
                    }
                }
                else
                {
                    return repeatTime;
                }
            }
        }

        public override void DoJob()
        {
            try
            {
                //MRS.MANAGER.Config.Loader.Refresh();
                LogSystem.Info("RefreshSystemConfigJob thanh cong. Time=" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override int GetRepetitionIntervalTime()
        {
            return RepeatTime;
        }

        public override bool IsRepeatable()
        {
            return true;
        }
    }
}