using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Common.Scheduler;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MRS.MANAGER.Core.MrsReport;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatmentType;
using MOS.DAO.StagingObject;
using Inventec.Core;

namespace MRS.API
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                //Cau hinh log4net
                log4net.Config.XmlConfigurator.Configure();
                LogSystem.Info("Application_Start_Begin.");
                //cập nhật mã báo cáo MRSINPUT mở
                new SAR.DAO.Sql.SqlDAO().Execute("UPDATE SAR_REPORT_TYPE SET IS_ACTIVE = 1 WHERE REPORT_TYPE_CODE='MRSINPUT'");

                //Cau hinh JsonConvert
                Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.None,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                };

                //Cau hinh api
                AreaRegistration.RegisterAllAreas();
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                Inventec.Common.Logging.LogSystem.Info("Start Init Dll");
                new ProcessorFactory().Init();

                //kết nối với his_rs
                new MOS.DAO.HisTreatmentType.HisTreatmentTypeDAO().Get(new HisTreatmentTypeSO(), new CommonParam());
                //cập nhật các báo cáo đang xử lý về trạng thái lỗi
                new SAR.DAO.Sql.SqlDAO().Execute("UPDATE SAR_REPORT SET REPORT_STT_ID = 5, ERROR = 'Application Start' WHERE REPORT_STT_ID IN (1, 2) AND APP_CREATOR ='SAR'");

                //Check cau hinh mapper
                Mapper.AssertConfigurationIsValid();

                //Khoi tao cac cau hinh nghiep vu he thong
                MRS.MANAGER.Config.Loader.Refresh();
                MRS.MANAGER.Config.Loader.ThreadRefresh();
                //MRS.MANAGER.Config.LicenseChecker.Check();

                //Khoi tao cac job scheduler
                new JobManager().ExecuteAllJobs();

                LogSystem.Info("Application_Start_End");
                //cập nhật mã báo cáo MRSINPUT khóa
                new SAR.DAO.Sql.SqlDAO().Execute("UPDATE SAR_REPORT_TYPE SET IS_ACTIVE = 0 WHERE REPORT_TYPE_CODE='MRSINPUT'");
            }
            catch (Exception ex)
            {
                LogSystem.Error("Application_Start error: " + ex.Message + ". Time=" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }
        }

        protected void Application_End()
        {
            if (!(new SAR.DAO.Sql.SqlDAO().Execute("UPDATE SAR_REPORT SET REPORT_STT_ID = 5, ERROR = 'Application End' WHERE REPORT_STT_ID IN (1, 2) AND APP_CREATOR ='SAR'")))
            {
                Inventec.Common.Logging.LogSystem.Error("Update REPORT_STT_ID error");
            }
            LogSystem.Error("Application_End. Time=" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }
    }
}