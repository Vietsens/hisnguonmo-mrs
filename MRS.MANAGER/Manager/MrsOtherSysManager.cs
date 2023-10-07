using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using MRS.SDO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MRS.MANAGER.Manager
{
    public partial class MrsOtherSysManager : ManagerBase
    {
        public MrsOtherSysManager(CommonParam param)
            : base(param)
        {

        }

        public ReportResultSDO IntegrateReport(CreateOtherReportSDO data)
        {
            ReportResultSDO result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                using (var client = new HttpClient())
                {
                    string requestedUrl = "";
                    client.BaseAddress = new Uri(data.Uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    string timeRange = "";

                    if (!String.IsNullOrEmpty(data.FromTime) && !String.IsNullOrEmpty(data.ToTime))
                    {
                        timeRange = string.Format("/{0}/{1}", data.FromTime, data.ToTime);
                    }
                    requestedUrl = string.Format("api/{0}{1}", data.Action, timeRange);

                    Inventec.Common.Logging.LogSystem.Debug("begin call api: " + requestedUrl);
                    HttpResponseMessage resp = client.GetAsync(requestedUrl).Result;
                    //Inventec.Common.Logging.LogSystem.Debug("Received response api: " + uri);
                    if (!resp.IsSuccessStatusCode)
                    {
                        //LogSystem.Error(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}{4}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), Utils.SerializeObject(filter), Utils.SerializeObject(commonParam)));
                        throw new ApiException(resp.StatusCode);
                    }

                    string responseData = resp.Content.ReadAsStringAsync().Result;
                    if (responseData.StartsWith("{") && responseData.EndsWith("}") || responseData.StartsWith("[") && responseData.EndsWith("]"))
                    {
                        result = new ReportResultSDO();
                        result.DATA_DETAIL = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);
                    }
                    return result;
                }
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
