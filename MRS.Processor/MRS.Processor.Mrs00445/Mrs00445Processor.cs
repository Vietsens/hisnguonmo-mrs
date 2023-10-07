using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00445
{
    class Mrs00445Processor : AbstractProcessor
    {
        Mrs00445Filter castFilter = null; 
        List<Mrs00445RDO> listRdo = new List<Mrs00445RDO>(); 


        public Mrs00445Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 

        public List<long> listServiceXQ = null; 
        public List<long> listServiceHS = null; 
        public List<long> listServiceGDV = null; 
        public List<long> listServiceVS = null; 
        public List<long> listServiceTTNHI = null; 
        public List<long> listServiceCPK = null; 
        public List<long> listServiceDM = null; 
        public List<long> listServiceHH = null; 
        public List<long> listServicePTTH = null; 
        public List<long> listServiceTTNOI = null; 
        public List<long> listServiceTRUYEN = null; 
        public List<long> listServiceTTNGOAI = null; 
        public List<long> listServiceDT = null; 
        public List<long> listServiceRHM = null; 
        public List<long> listServiceGPB = null; 
        public List<long> listServiceASA = null; 

        public string departments = ""; 


        public override Type FilterType()
        {
            return typeof(Mrs00445Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00445Filter)this.reportFilter; 
                var skip = 0; 
                //lay khoa
                if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                    departmentFilter.IDs = this.castFilter.REQUEST_DEPARTMENT_IDs; 
                    var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(departmentFilter); 
                    if(IsNotNullOrEmpty(listDepartments))
                    {
                        foreach(var department in listDepartments)
                        {
                            departments += department.DEPARTMENT_NAME + ","; 
                        }
                        departments = departments.Substring(0, departments.Length - 1); 
                    }

                }

                //lay dich vu config
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "Mrs00445"; 
                var listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter); 

                listServiceXQ = listServices.Where(w => w.CATEGORY_CODE == "445XQ").Select(s => s.SERVICE_ID).ToList(); 
                listServiceHS = listServices.Where(w => w.CATEGORY_CODE == "445HS").Select(s => s.SERVICE_ID).ToList(); 
                listServiceGDV = listServices.Where(w => w.CATEGORY_CODE == "445GDV").Select(s => s.SERVICE_ID).ToList(); 
                listServiceVS = listServices.Where(w => w.CATEGORY_CODE == "445VS").Select(s => s.SERVICE_ID).ToList(); 
                listServiceTTNHI = listServices.Where(w => w.CATEGORY_CODE == "445TTNHI").Select(s => s.SERVICE_ID).ToList(); 
                listServiceCPK = listServices.Where(w => w.CATEGORY_CODE == "445CPK").Select(s => s.SERVICE_ID).ToList(); 
                listServiceDM = listServices.Where(w => w.CATEGORY_CODE == "445DM").Select(s => s.SERVICE_ID).ToList(); 
                listServiceHH = listServices.Where(w => w.CATEGORY_CODE == "445HH").Select(s => s.SERVICE_ID).ToList(); 
                listServicePTTH = listServices.Where(w => w.CATEGORY_CODE == "445PTTH").Select(s => s.SERVICE_ID).ToList(); 
                listServiceTTNOI = listServices.Where(w => w.CATEGORY_CODE == "445TTNOI").Select(s => s.SERVICE_ID).ToList(); 
                listServiceTRUYEN = listServices.Where(w => w.CATEGORY_CODE == "445TRUYEN").Select(s => s.SERVICE_ID).ToList(); 
                listServiceTTNGOAI = listServices.Where(w => w.CATEGORY_CODE == "445TTNGOAI").Select(s => s.SERVICE_ID).ToList(); 
                listServiceDT = listServices.Where(w => w.CATEGORY_CODE == "445DT").Select(s => s.SERVICE_ID).ToList(); 
                listServiceRHM = listServices.Where(w => w.CATEGORY_CODE == "445RHM").Select(s => s.SERVICE_ID).ToList(); 
                listServiceGPB = listServices.Where(w => w.CATEGORY_CODE == "445GPB").Select(s => s.SERVICE_ID).ToList(); 
                listServiceASA = listServices.Where(w => w.CATEGORY_CODE == "445ASA").Select(s => s.SERVICE_ID).ToList(); 

                //lay treatment

                listTreatments = new ManagerSql().GetTreatment(castFilter);
                listSereServs = new ManagerSql().GetSereServ(castFilter);
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
                if(IsNotNullOrEmpty(listTreatments))
                {
                    foreach(var treatment in listTreatments)
                    {
                        var sereServs = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID).ToList(); 
                        Mrs00445RDO rdo = new Mrs00445RDO(); 
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE; 
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                        rdo.IN_TIME = treatment.IN_TIME; 
                        rdo.OUT_TIME = treatment.OUT_TIME; 
                        if(IsNotNullOrEmpty(sereServs))
                        {
                            foreach(var sereServ in sereServs)
                            {
                                if(listServiceASA.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_ANHSIEUAM += sereServ.VIR_TOTAL_PRICE??0; 
                                }
                                if (listServiceCPK.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_CHIPHIKHAC += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceDM.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_DONGMAU += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceDT.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_DIENTIM += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceGDV.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_GIUONGDV += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceGPB.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_GPB += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceHH.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_HUYETHOC += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceHS.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_HOASINH += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServicePTTH.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_PTTH += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceRHM.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_THUTHUATRHM += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                
                                if (listServiceTRUYEN.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_TRUYENMAU += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceTTNGOAI.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_THUTHUATNGOAI += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceTTNHI.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_THUTHUATNHI += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceTTNOI.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_THUTHUATNOI += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceVS.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_VISINH += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (listServiceXQ.Contains(sereServ.TDL_SERVICE_TYPE_ID))
                                {
                                    rdo.TOTAL_PRICE_XQUANG += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }

                                if(sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                                {
                                    rdo.TOTAL_PRICE_GIUONG += sereServ.VIR_TOTAL_PRICE??0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                {
                                    rdo.TOTAL_PRICE_THUTHUAT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                                {
                                    rdo.TOTAL_PRICE_PHAUTHUAT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                                {
                                    rdo.TOTAL_PRICE_THUOC += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                                {
                                    rdo.TOTAL_PRICE_VTTH += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                                {
                                    rdo.TOTAL_PRICE_MAU += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                                {
                                    rdo.TOTAL_PRICE_KHAM += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                                {
                                    rdo.TOTAL_PRICE_SIEUAM += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                            }

                            listRdo.Add(rdo); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                dicSingleTag.Add("DEPARTMENT", departments); 
                objectTag.AddObjectData(store, "Report", listRdo); 

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }
}
