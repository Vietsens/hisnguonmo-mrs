using Inventec.Common.Logging;
using Inventec.Core;
using PostSharp.Aspects;
using System;

namespace MOS.MANAGER.Base
{
    [Serializable]
    public class LoggerAttribute : OnMethodBoundaryAspect
    {
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    Result t = (Result)args.ReturnValue;
        //    BusinessBase b = (BusinessBase)args.Instance;
        //    string className = "";
        //    string loginName = "";
        //    if (b != null)
        //    {
        //        className = args.Instance.GetType().Name;
        //        loginName = b.UserName;
        //    }

        //    if (t == null || !t.Success || t.Param.HasException)
        //    {
        //        string parameterValues = "";

        //        foreach (object arg in args.Arguments)
        //        {
        //            if (parameterValues.Length > 0)
        //            {
        //                parameterValues += ", ";
        //            }

        //            if (arg != null)
        //            {
        //                parameterValues += Newtonsoft.Json.JsonConvert.SerializeObject(arg);
        //            }
        //            else
        //            {
        //                parameterValues += "null";
        //            }
        //        }
        //        string log = string.Format("\n--LoginName: {0} \n--Class: {1} \n--Method: {2} \n--Input: {3} \n--Output: {4}", loginName, className, args.Method.Name, parameterValues, Newtonsoft.Json.JsonConvert.SerializeObject(args.ReturnValue));
        //        LogSystem.Error(log);
        //    }

        //    if (t != null && t.Param != null && (t.Param.HasException || (t.Param.BugCodes != null && t.Param.BugCodes.Count > 0)))
        //    {
        //        string troubleTime = Inventec.Common.DateTime.Get.NowAsTimeString();
        //        //TroubleCache.Add(string.Format("{0}__LoginName: {1}__{2}", troubleTime, loginName, (t.Param.HasException ? "param.HasException." : "") + t.Param.GetBugCode()));
        //    }
        //}
    }
}
