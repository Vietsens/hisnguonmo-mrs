using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServicePaty;
using MOS.MANAGER.HisPatientType;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00822
{
    public class Mrs00822Processor:AbstractProcessor
    {
        public Mrs00822Filter filter;
        public List<Mrs00822RDO> ListRdo = new List<Mrs00822RDO>();
        public List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        public List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        public List<V_HIS_SERVICE_PATY> ListServicePaty = new List<V_HIS_SERVICE_PATY>();
        public List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        public List<HIS_SERVICE> ListServiceParent = new List<HIS_SERVICE>();
        public List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>();

        public Dictionary<string,decimal> dicPriceLast = new  Dictionary<string,decimal>();

        List<long> SERVICE_TYPE_IDs = new List<long>()
        { 
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN,
        };
        public Mrs00822Processor(CommonParam param,string reportTypeCode):base (param,reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00822Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00822Filter)this.reportFilter;
            bool result = false;
            try
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.OUT_TIME_FROM = filter.TIME_FROM;
                treatmentFilter.OUT_TIME_TO = filter.TIME_TO;
                ListTreatment = new HisTreatmentManager().Get(treatmentFilter);
                var treatIds = ListTreatment.Select(x => x.ID).Distinct().ToList();
                var skip = 0;
                while (treatIds.Count-skip>0)
                {
                    var limit = treatIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                    sereServFilter.TREATMENT_IDs = limit;
                    sereServFilter.TDL_SERVICE_TYPE_IDs = filter.SERVICE_TYPE_IDs??SERVICE_TYPE_IDs;
                    sereServFilter.IS_EXPEND = false;
                    sereServFilter.HAS_EXECUTE = true;
                    if (filter.DEPARTMENT_ID!=null)
                    {
                        sereServFilter.TDL_EXECUTE_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                    }
                    if (filter.DEPARTMENT_IDs != null)
                    {
                        sereServFilter.TDL_EXECUTE_DEPARTMENT_IDs = filter.DEPARTMENT_IDs;
                    }
                    var sereServ = new HisSereServManager().Get(sereServFilter);
                    ListSereServ.AddRange(sereServ);
                }
                skip = 0;
                var serviceIds = ListSereServ.Select(x=>x.SERVICE_ID).Distinct().ToList();
                //if (IsNotNullOrEmpty(serviceIds))
                //{
                //    GetServiceRati(serviceIds);
                //}
                while (serviceIds.Count-skip>0)
                {
                    var limit = serviceIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                    serviceFilter.IDs = limit;
                    var service = new HisServiceManager().Get(serviceFilter);
                    ListService.AddRange(service);
                    HisServicePatyViewFilterQuery servicePatyFilter = new HisServicePatyViewFilterQuery();
                    servicePatyFilter.SERVICE_IDs = limit;
                    var servicePaty = new HisServicePatyManager().GetView(servicePatyFilter);
                    ListServicePaty.AddRange(servicePaty);
                }
                //xử lý tạo dữ liệu giá mới nhất
                GetDicLastPrice();

                var parentServiceIds = ListService.Where(x=>x.PARENT_ID!= null).Select(x => x.PARENT_ID??0).Distinct().ToList();
                HisServiceFilterQuery serviceParentFilter = new HisServiceFilterQuery();
                serviceParentFilter.IDs = parentServiceIds;
                ListServiceParent = new HisServiceManager().Get(serviceParentFilter);
                HisPatientTypeFilterQuery patientTypeFilter = new HisPatientTypeFilterQuery();
                ListPatientType = new  HisPatientTypeManager().Get(patientTypeFilter);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
               result=false;
            }
            return result;
        }

        private void GetDicLastPrice()
        {
            try
            {
                dicPriceLast = ListServicePaty.GroupBy(o=>string.Format("{0}_{1}",o.SERVICE_ID,o.PATIENT_TYPE_ID)).ToDictionary(p=>p.Key,q=>q.OrderBy(o=>o.ID).Last().PRICE);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        //private void GetServiceRati(List<long> serviceIDs){

        //    List<HIS_SERVICE_RATI> result = new List<HIS_SERVICE_RATI>();
        //    try
        //    {
        //        string query = "";
        //        query += "SELECT * FROM HIS_SERVICE_RATI \n";
        //        query += "WHERE 1=1 \n";
        //        query += string.Format("AND SERVICE_ID in ({0})\n", string.Join(",", serviceIDs));
        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_RATI>(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = null;
        //        LogSystem.Error(ex);
        //    }
        
        //}
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    long patientTypeBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                    long patientTypeFee = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                    foreach (var item in ListSereServ)
                    {
                        Mrs00822RDO rdo = new Mrs00822RDO();
                        var service = ListService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault();
                        if (service!= null)
                        {
                            var prService = ListServiceParent.Where(x => x.ID == service.PARENT_ID).FirstOrDefault();
                            if (prService!= null)
                            {
                                rdo.PARENT_SERVICE_CODE = prService.SERVICE_CODE;
                                rdo.PARENT_SERVICE_NAME = prService.SERVICE_NAME;
                            }
                        }
                        rdo.VIR_PRICE = item.VIR_PRICE ?? 0;

                        if (item.PATIENT_TYPE_ID == patientTypeBhyt)
                        {
                            rdo.BHYT_PRICE = item.VIR_PRICE ?? 0;
                        }
                        else
                        {
                            if (dicPriceLast.ContainsKey(string.Format("{0}_{1}", item.SERVICE_ID, patientTypeBhyt)))
                            {
                                rdo.BHYT_PRICE = dicPriceLast[string.Format("{0}_{1}", item.SERVICE_ID, patientTypeBhyt)];
                            }
                        }
                        if (item.PATIENT_TYPE_ID == patientTypeFee)
                        {
                            rdo.VP_PRICE = item.VIR_PRICE ?? 0;
                        }
                        else
                        {
                            if (dicPriceLast.ContainsKey(string.Format("{0}_{1}", item.SERVICE_ID, patientTypeFee)))
                            {
                                rdo.VP_PRICE = dicPriceLast[string.Format("{0}_{1}", item.SERVICE_ID, patientTypeFee)];
                            }
                        }
                        //var servicePaty = ListServicePaty.Where(x => x.SERVICE_ID == item.SERVICE_ID).ToList();
                        //if (servicePaty!=null)
                        //{
                        //    foreach (var paty in servicePaty)
                        //    {
                        //        if (paty.PATIENT_TYPE_NAME == "BHYT")
                        //        {
                        //            rdo.BHYT_PRICE = paty.PRICE;
                        //        }
                        //        if (paty.PATIENT_TYPE_NAME.ToLower().Contains("viện phí"))
                        //        {
                        //            rdo.VP_PRICE = paty.PRICE;
                        //        }
                        //    }
                        //}

                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID == item.TDL_SERVICE_TYPE_ID);
                        if (serviceType != null)
                        {
                            rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                        }
                        rdo.HEIN_SERVICE_TYPE_ID = item.TDL_HEIN_SERVICE_TYPE_ID??0;
                        rdo.HEIN_SERVICE_TYPE_CODE = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;

                        if (item.PATIENT_TYPE_ID == patientTypeFee)
                        {
                            rdo.VP_AMOUNT = item.AMOUNT;
                        }
                        if (item.PATIENT_TYPE_ID == patientTypeBhyt)
                        {
                            rdo.BHYT_AMOUNT = item.AMOUNT;
                        }
                        if (item.PATIENT_TYPE_ID != patientTypeBhyt && item.PATIENT_TYPE_ID != patientTypeFee)
                        {
                            rdo.YC_AMOUNT = item.AMOUNT;
                        }

                        ListRdo.Add(rdo);
                    }
                }
                string keyGroup = "{0}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SV") && this.dicDataFilter["KEY_GROUP_SV"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SV"].ToString()))
                {
                    keyGroup=this.dicDataFilter["KEY_GROUP_SV"].ToString();//keyGroup = "{0}_{1}";
                }
                var group = ListRdo.GroupBy(x => string.Format(keyGroup, x.SERVICE_CODE, x.VIR_PRICE)).ToList();
                ListRdo.Clear();
                if (IsNotNullOrEmpty(group))
                {
                   
                    foreach (var item in group)
                    {
                        Mrs00822RDO rdo = new Mrs00822RDO();
                        rdo.PARENT_SERVICE_CODE = item.First().PARENT_SERVICE_CODE;
                        rdo.PARENT_SERVICE_NAME = item.First().PARENT_SERVICE_NAME;
                        rdo.SERVICE_CODE = item.First().SERVICE_CODE;
                        rdo.SERVICE_NAME = item.First().SERVICE_NAME;
                        rdo.HEIN_SERVICE_TYPE_CODE = item.First().HEIN_SERVICE_TYPE_CODE;
                        rdo.HEIN_SERVICE_TYPE_NAME = item.First().HEIN_SERVICE_TYPE_NAME;
                        rdo.HEIN_SERVICE_TYPE_ID = item.First().HEIN_SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = item.First().SERVICE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = item.First().SERVICE_TYPE_NAME;
                        rdo.SERVICE_TYPE_ID = item.First().SERVICE_TYPE_ID;
                        rdo.VP_PRICE = item.First().VP_PRICE;
                        rdo.BHYT_PRICE = item.First().BHYT_PRICE;
                        rdo.VP_AMOUNT = item.Sum(x => x.VP_AMOUNT);
                        rdo.BHYT_AMOUNT = item.Sum(x => x.BHYT_AMOUNT);
                        rdo.YC_AMOUNT = item.Sum(x => x.YC_AMOUNT);
                        rdo.VIR_PRICE = item.First().VIR_PRICE;
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            var listRdoTT = ListRdo.Where(x=>x.SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
            var listRdoPT = ListRdo.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
            var listRdoXN = ListRdo.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList();
            var listRdoHACN = ListRdo.Where(x => x.HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || x.HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN).ToList();
            objectTag.AddObjectData(store, "ReportTT", listRdoTT);
            objectTag.AddObjectData(store, "ReportPT", listRdoPT);
            objectTag.AddObjectData(store, "ReportXN", listRdoXN);
            objectTag.AddObjectData(store, "ReportHACN", listRdoHACN);
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
