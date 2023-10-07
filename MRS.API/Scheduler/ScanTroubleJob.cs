using Inventec.Common.Logging;
using System;

namespace MRS.API.Scheduler
{
    public class ScanTroubleJob : Inventec.Common.Scheduler.Job
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
                        repeatTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MRS.API.Scheduler.ScanTroubleJob"]);
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
                //new MRS.MANAGER.Sda.SdaTrouble.SdaTroubleManager().Scan();
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