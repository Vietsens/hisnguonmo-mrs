using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Threading;
using SAR.EFMODEL.DataModels;

namespace MRS.MANAGER.Core.MrsReport
{
    public class ProcessorFactory
    {
        private const string PROCESSOR_NAMESPACE_PREFIX = "MRS.Processor.";
        private static Dictionary<string, Type> PROCESSOR_DIC = null;
        private static List<Assembly> ListAssembly = new List<Assembly>();

        public ProcessorFactory()
        {
            Init();
        }

        public AbstractProcessor GetProcessor(string reportTypeCode, CommonParam param)
        {
            string code = GetCode(reportTypeCode);
            if (code.StartsWith("TKB"))
            {
                code = "TKB";
            }

            if (PROCESSOR_DIC != null && PROCESSOR_DIC.ContainsKey(code))
            {
                Type t = PROCESSOR_DIC[code];
                return (AbstractProcessor)Activator.CreateInstance(t, param, reportTypeCode);
            }
            return null;
        }

        private string GetCode(string reportTypeCode)
        {
            string result = reportTypeCode;
            try
            {
                var reportType = new SAR.DAO.Sql.SqlDAO().GetSql<SAR_REPORT_TYPE>(string.Format("select * from sar_report_type where report_type_code='{0}'",reportTypeCode));
                if (reportType != null &&reportType.Count>0)
                {
                    result = reportType.First().DLL_CODE ?? reportTypeCode;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = reportTypeCode;
            }
            return result;
        }

        public void Init()
        {
            if (ProcessorFactory.PROCESSOR_DIC == null)
            {
                try
                {
                    Inventec.Common.Logging.LogSystem.Info("Begin LoadDll");
                    this.LoadDll();
                    Inventec.Common.Logging.LogSystem.Info("End LoadDll");
                    List<Type> parserTypes = this.GetAllTypesImplement(typeof(AbstractProcessor));
                    Inventec.Common.Logging.LogSystem.Info("End GetAllTypesImplement");
                    if (parserTypes != null)
                    {
                        foreach (Type parserType in parserTypes)
                        {
                            if (this.IsRealClass(parserType))
                            {
                                this.AddType(parserType);
                            }
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Info("Init Dll success");
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
        }

        private void AddType(Type parserType)
        {
            try
            {
                if (parserType != null)
                {
                    //Xu ly de lay ma bao cao tu namespace cua thu vien dll
                    string nameSpace = parserType.Namespace;
                    int index = !string.IsNullOrWhiteSpace(nameSpace) ? nameSpace.IndexOf(PROCESSOR_NAMESPACE_PREFIX) : -1;
                    if (index >= 0)
                    {
                        string reportTypeCode = nameSpace.Substring(index + PROCESSOR_NAMESPACE_PREFIX.Length).ToUpper();
                        if (ProcessorFactory.PROCESSOR_DIC != null && ProcessorFactory.PROCESSOR_DIC.ContainsKey(reportTypeCode))
                        {
                            throw new Exception(string.Format("{0} co thong tin ReportTypeCode trung voi Processor khac", parserType.FullName));
                        }
                        if (ProcessorFactory.PROCESSOR_DIC == null)
                        {
                            ProcessorFactory.PROCESSOR_DIC = new Dictionary<string, Type>();
                        }
                        ProcessorFactory.PROCESSOR_DIC.Add(reportTypeCode, parserType);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void LoadDll()
        {
            string dllFolderPath = ConfigurationManager.AppSettings["MRS.Processor.Instance.Dll.Folder"];
            string rootPath = HttpRuntime.AppDomainAppPath;
            string[] dllFiles = Directory.GetFiles(rootPath + dllFolderPath, "*.dll", SearchOption.TopDirectoryOnly);
            if (dllFiles != null && dllFiles.Length > 0)
            {
                //foreach (string s in dllFiles)
                //{
                //    try
                //    {
                //        Assembly assembly = Assembly.LoadFrom(s);
                //        var assemblyName = assembly.GetName();
                //        //var ass = AppDomain.CurrentDomain.Load(assemblyName);
                //        if (assemblyName.FullName.Substring(0, 14) == PROCESSOR_NAMESPACE_PREFIX)
                //        {
                //            ListAssembly.Add(assembly);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Inventec.Common.Logging.LogSystem.Error(ex);
                //    }
                //}

                ProcessTheadLoadDll(dllFiles);
            }
        }

        private List<Type> GetAllTypesImplement(Type desiredType)
        {
            //var ass = AppDomain.CurrentDomain.GetAssemblies().Where(o => o.FullName.Substring(0, 14) == PROCESSOR_NAMESPACE_PREFIX);

            if (ListAssembly != null && ListAssembly.Count > 0)
            {
                return ListAssembly
                   .SelectMany(assembly => GetLoadableTypes(assembly))
                   .Where(type => desiredType.IsAssignableFrom(type)).ToList();
            }
            return null;
        }

        private Type[] GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToArray();
            }
        }

        private bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }

        private void ProcessTheadLoadDll(string[] dllFiles)
        {
            try
            {

                if (dllFiles != null && dllFiles.Count() > 0)
                {
                    List<ThreadADO> threds = new List<ThreadADO>();
                    var skip = 0;
                    while (dllFiles.Count() - skip > 0)
                    {
                        var listFiles = dllFiles.Skip(skip).Take(100).ToList();
                        skip += 100;

                        ThreadADO ado = new ThreadADO();
                        ParamThread paramThread = new ParamThread();
                        paramThread.listFiles = listFiles;
                        //paramThread.result = new List<Assembly>();
                        ado.Th = new Thread(LoadDllFromFileName);
                        ado.Data = paramThread;
                        threds.Add(ado);
                    }

                    foreach (var item in threds)
                    {
                        item.Th.Start(item.Data);
                    }

                    foreach (var item in threds)
                    {
                        item.Th.Join();
                    }
                    ListAssembly = threds.SelectMany(o => (o.Data as ParamThread).result ?? new List<Assembly>()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDllFromFileName(object paramThread)
        {
            try
            {
                if (paramThread != null && paramThread is ParamThread)
                {
                    ParamThread paramThr = paramThread as ParamThread;
                    if (paramThr.listFiles != null)
                    {
                        paramThr.result = new List<Assembly>();
                        foreach (string s in paramThr.listFiles)
                        {
                            try
                            {
                                Assembly assembly = Assembly.LoadFrom(s);
                                var assemblyName = assembly.GetName();
                                //var ass = AppDomain.CurrentDomain.Load(assemblyName);
                                if (assemblyName.FullName.Substring(0, 14) == PROCESSOR_NAMESPACE_PREFIX)
                                {
                                    paramThr.result.Add(assembly);
                                }
                            }
                            catch (Exception ex)
                            {
                                Inventec.Common.Logging.LogSystem.Error(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private class ThreadADO
        {
            public Thread Th { get; set; }
            public object Data { get; set; }
        }

        private class ParamThread
        {
            public List<Assembly> result { get; set; }
            public List<string> listFiles { get; set; }
        }
    }
}
