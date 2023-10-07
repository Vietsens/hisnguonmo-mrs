using MRS.API.Base;
using SAR.EFMODEL.DataModels;
using MRS.MANAGER.Manager;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Web.Http;
using MRS.SDO;
using System.Net.Http;
using System.Linq;

namespace MRS.API.Controllers
{
    public partial class MrsReportController : BaseApiController
    {
        [HttpGet]
        [ApiParamFilter(typeof(ApiParam<string>), "param")]
        [ActionName("GetInput")]
        public ApiResult GetInput(ApiParam<string> param)
        {
            try
            {
                ApiResultObject<List<DataGetSDO>> result = new ApiResultObject<List<DataGetSDO>>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;
                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "GetInput", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    List<DataGetSDO> resultData = managerContainer.Run<List<DataGetSDO>>();
                    result = PackResult<List<DataGetSDO>>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
           
        }
        [HttpPost]
        [ActionName("Create")]
        public ApiResult Create(ApiParam<CreateReportSDO> param)
        {
            try
            {
                //ApiResultObject<SAR_REPORT> result = new ApiResultObject<SAR_REPORT>(null, false);
                ApiResultObject<bool> result = new ApiResultObject<bool>(false, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "Create", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    //SAR_REPORT resultData = managerContainer.Run<SAR_REPORT>();
                    //result = PackResult<SAR_REPORT>(resultData);
                    bool resultData = managerContainer.Run<bool>();
                    result = PackResult<bool>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        [HttpPost]
        [ActionName("CreateReq")]
        public ApiResult CreateReq(ApiParam<CreateReportSDO> param)
        {
            try
            {
                var branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == param.ApiData.BranchId);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                if (branch != null && !string.IsNullOrWhiteSpace(branch.HEIN_MEDI_ORG_CODE))
                {
                    json += "\r\nHEIN_MEDI_ORG_CODE: " + Inventec.Common.String.Convert.UnSignVNese(branch.HEIN_MEDI_ORG_CODE);
                }
                Inventec.Common.Logging.LogSystem.Error(json);
                ApiResultObject<SAR_REPORT> result = new ApiResultObject<SAR_REPORT>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "CreateReq", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    SAR_REPORT resultData = managerContainer.Run<SAR_REPORT>();
                    result = PackResult<SAR_REPORT>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        [HttpPost]
        [ActionName("CreateByte")]
        public ApiResult CreateByte(ApiParam<CreateByteSDO> param)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param));
                ApiResultObject<ByteResultSDO> result = new ApiResultObject<ByteResultSDO>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "CreateByte", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    ByteResultSDO resultData = managerContainer.Run<ByteResultSDO>();
                    result = PackResult<ByteResultSDO>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        [HttpPost]
        [ActionName("CreateData")]
        [AllowAnonymous]
        public ApiResult CreateData(ApiParam<CreateReportSDO> param)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param));
                ApiResultObject<ReportResultSDO> result = new ApiResultObject<ReportResultSDO>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "CreateData", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    ReportResultSDO resultData = managerContainer.Run<ReportResultSDO>();
                    result = PackResult<ReportResultSDO>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("CreateByCalendar")]
        public ApiResult CreateByCalendar(ApiParam<CreateReportSDO> param)
        {
            try
            {
                ApiResultObject<SAR_REPORT> result = new ApiResultObject<SAR_REPORT>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "CreateByCalendar", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    SAR_REPORT resultData = managerContainer.Run<SAR_REPORT>();
                    result = PackResult<SAR_REPORT>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("IntegrateReport")]
        public ApiResult IntegrateReport(ApiParam<CreateOtherReportSDO> param)
        {
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                Inventec.Common.Logging.LogSystem.Error(json);
                ApiResultObject<ReportResultSDO> result = new ApiResultObject<ReportResultSDO>(null, false);
                if (param != null)
                {
                    if (param.CommonParam == null) param.CommonParam = new CommonParam();
                    this.commonParam = param.CommonParam;

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsOtherSysManager), "IntegrateReport", new object[] { param.CommonParam }, new object[] { param.ApiData });
                    ReportResultSDO resultData = managerContainer.Run<ReportResultSDO>();
                    result = PackResult<ReportResultSDO>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        [HttpPost]
        [ActionName("Refresh")]
        public ApiResult Refresh()
        {
            try
            {
                ApiResultObject<bool> result = new ApiResultObject<bool>();
                {

                    ManagerContainer managerContainer = new ManagerContainer(typeof(MrsReportManager), "Refresh", new object[] { new CommonParam() }, new object[] { });
                    bool resultData = managerContainer.Run<bool>();
                    result = PackResult<bool>(resultData);
                }
                return new ApiResult(result, this.ActionContext);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

    }
}
