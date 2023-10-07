using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisServicePaty;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00486
{
    public class Mrs00486Processor : AbstractProcessor
    {
        Mrs00486Filter castFilter = null;
        List<Mrs00486RDO> ListRdoMedi = new List<Mrs00486RDO>();
        List<Mrs00486RDO> ListRdoTect = new List<Mrs00486RDO>();
        List<long> CurentTreatmendId = new List<long>();
        List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListTectSereServ = new List<V_HIS_SERE_SERV>();
        List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListMediSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        CommonParam paramGet = new CommonParam();
        const int MAX_DEPARTMENT_COUNT = 50;
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public Mrs00486Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00486Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00486Filter)this.reportFilter);
                CommonParam paramGet = new CommonParam();
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IDs = castFilter.REQUEST_DEPARTMENT_IDs ?? castFilter.EXECUTE_DEPARTMENT_IDs;
                listDepartment = new HisDepartmentManager().Get(departmentFilter).OrderBy(o => o.NUM_ORDER).ToList();
                var serviceIds = new List<long>();
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.TIME_TO;

                filter.REQUEST_DEPARTMENT_IDs = castFilter.REQUEST_DEPARTMENT_IDs ?? castFilter.EXECUTE_DEPARTMENT_IDs;
                filter.SERVICE_TYPE_IDs = new List<long>()
                    {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT
                    };
                ListMediSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView(filter);
                serviceIds.AddRange(ListMediSereServ.Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>());
                Inventec.Common.Logging.LogSystem.Info("ListMediSereServ" + ListMediSereServ.Count);

                filter.REQUEST_DEPARTMENT_IDs = castFilter.REQUEST_DEPARTMENT_IDs;
                filter.EXECUTE_DEPARTMENT_IDs = castFilter.EXECUTE_DEPARTMENT_IDs;
                filter.SERVICE_TYPE_IDs = new List<long>()
                    {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA, 
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN
                    };
                ListTectSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView(filter);
                serviceIds.AddRange(ListTectSereServ.Select(o => o.SERVICE_ID).Distinct().ToList() ?? new List<long>());
                Inventec.Common.Logging.LogSystem.Info("ListTectSereServ" + ListTectSereServ.Count);
                //if (IsNotNullOrEmpty(serviceIds))
                //{
                //    var skip = 0; 
                //    while (serviceIds.Count - skip > 0)
                //    {
                //        var listIDs = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                //        HisServicePatyFilterQuery svptfilter = new HisServicePatyFilterQuery(); 
                //        svptfilter.IDs = listIDs; 
                //        var listServiceReqSub = new HisServicePatyManager(paramGet).Get(svptfilter); 
                //        ListServiceReq.AddRange(listServiceReqSub); 
                //    }
                //}


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListMediSereServ))
                {
                    ProcessListMediSereServ(ListMediSereServ);
                }
                if (IsNotNullOrEmpty(ListTectSereServ))
                {
                    ProcessListTectSereServ(ListTectSereServ);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private void ProcessListMediSereServ(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListMediSereServ)
        {
            try
            {

                var Groups = ListMediSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", ListMediSereServ.Select(o => new { st = o.REQUEST_DEPARTMENT_CODE + "_" + o.AMOUNT.ToString() }).ToList()));
                PropertyInfo p = null;

                int Max = listDepartment.Count() < MAX_DEPARTMENT_COUNT ? listDepartment.Count() : MAX_DEPARTMENT_COUNT;

                List<V_HIS_SERE_SERV> sub = new List<V_HIS_SERE_SERV>();
                foreach (var group in Groups)
                {

                    List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                    Mrs00486RDO rdo = new Mrs00486RDO();
                    rdo.TDL_SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                    rdo.SERVICE_TYPE_NAME = listSub.First().SERVICE_TYPE_NAME;
                    for (int i = 0; i < Max; i++)
                    {
                        sub = listSub.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_REQUEST_DEPARTMENT_ID == listDepartment[i].ID).ToList();
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("AMOUNT_BHYT_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.AMOUNT));
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("VIR_TOTAL_PRICE_BHYT_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.VIR_TOTAL_PRICE));

                        sub = listSub.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.TDL_REQUEST_DEPARTMENT_ID == listDepartment[i].ID).ToList();
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("AMOUNT_VP_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.AMOUNT));
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("VIR_TOTAL_PRICE_VP_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.VIR_TOTAL_PRICE));
                    }
                    ListRdoMedi.Add(rdo);
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoMedi.Clear();
            }
        }
        private void ProcessListTectSereServ(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV> ListTectSereServ)
        {
            try
            {

                var Groups = ListTectSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", ListTectSereServ.Select(o => new { st = o.REQUEST_DEPARTMENT_CODE + "_" + o.AMOUNT.ToString() }).ToList()));
                PropertyInfo p = null;

                int Max = listDepartment.Count() < MAX_DEPARTMENT_COUNT ? listDepartment.Count() : MAX_DEPARTMENT_COUNT;

                List<V_HIS_SERE_SERV> sub = new List<V_HIS_SERE_SERV>();
                foreach (var group in Groups)
                {

                    List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                    Mrs00486RDO rdo = new Mrs00486RDO();
                    rdo.TDL_SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                    rdo.SERVICE_TYPE_NAME = listSub.First().SERVICE_TYPE_NAME;
                    for (int i = 0; i < Max; i++)
                    {
                        sub = listSub.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && ((castFilter.EXECUTE_DEPARTMENT_IDs != null && o.TDL_EXECUTE_DEPARTMENT_ID == listDepartment[i].ID) || (castFilter.REQUEST_DEPARTMENT_IDs != null && o.TDL_REQUEST_DEPARTMENT_ID == listDepartment[i].ID))).ToList();
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("AMOUNT_BHYT_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.AMOUNT));
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("VIR_TOTAL_PRICE_BHYT_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.VIR_TOTAL_PRICE));

                        sub = listSub.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && ((castFilter.EXECUTE_DEPARTMENT_IDs != null && o.TDL_EXECUTE_DEPARTMENT_ID == listDepartment[i].ID) || (castFilter.REQUEST_DEPARTMENT_IDs != null && o.TDL_REQUEST_DEPARTMENT_ID == listDepartment[i].ID))).ToList();
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("AMOUNT_VP_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.AMOUNT));
                        p = typeof(Mrs00486RDO).GetProperty(string.Format("VIR_TOTAL_PRICE_VP_{0}", i + 1));
                        p.SetValue(rdo, sub.Sum(s => s.VIR_TOTAL_PRICE));
                    }
                    ListRdoTect.Add(rdo);
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoTect.Clear();
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                for (int i = 0; i < listDepartment.Count(); i++)
                    dicSingleTag.Add(string.Format("DEPARTMENT_NAME_{0}", i + 1), listDepartment[i].DEPARTMENT_NAME);
                objectTag.AddObjectData(store, "Medi", ListRdoMedi.OrderBy(o => o.SERVICE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "Tect", ListRdoTect.OrderBy(o => o.SERVICE_TYPE_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
