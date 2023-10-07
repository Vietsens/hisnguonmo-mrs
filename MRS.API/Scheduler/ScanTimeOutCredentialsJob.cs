using Inventec.Common.Logging;
using Inventec.Common.Scheduler;
using System;

namespace MRS.API.Scheduler
{
    public class ScanTimeOutCredentialsJob : Job
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
                        repeatTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MRS.API.Scheduler.ScanTimeOutCredentialsJob"]);
                        return repeatTime;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        repeatTime = 0;
                        return 300000; //5 phut
                    }
                }
                else
                {
                    return repeatTime;
                }
            }
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override void DoJob()
        {
            Inventec.Token.ResourceSystem.ResourceTokenManager.ScanTimeOutCredentials();
        }        

        public override bool IsRepeatable()
        {
            return true;
        }

        public override int GetRepetitionIntervalTime()
        {
            return RepeatTime;
        }
    }
}