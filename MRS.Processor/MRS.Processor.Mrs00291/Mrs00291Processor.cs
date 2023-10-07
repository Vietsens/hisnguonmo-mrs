using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using ACS.Filter; 
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisSereServPttt; 
using MOS.MANAGER.HisTreatment; 
using MRS.SDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPtttGroup; 

namespace MRS.Processor.Mrs00291
{
    class Mrs00291Processor : AbstractProcessor
    {
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_PTTT_GROUP> listPtttGroup = new List<HIS_PTTT_GROUP>();
        string HEIN_SERVICE_TYPE = "DICHVU";

        CommonParam paramGet = new CommonParam();
        List<Mrs00291RDO> ListRdo = new List<Mrs00291RDO>();
        //List<Mrs00291RDO> ListRdoA = new List<Mrs00291RDO>();
        //List<Mrs00291RDO> ListRdoB = new List<Mrs00291RDO>();
        //List<Mrs00291RDO> ListRdoC = new List<Mrs00291RDO>();
        public Mrs00291Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00291Filter); 
        }


        protected override bool GetData()
        {
            var filter = ((Mrs00291Filter)reportFilter); 
            bool result = true; 
            try
            {
                if (filter.HEIN_SERVICE_TYPE.HasValue)
                {
                    if (filter.HEIN_SERVICE_TYPE.Value == 0)
                    {
                        HEIN_SERVICE_TYPE = "VATTU";
                    }
                    else if (filter.HEIN_SERVICE_TYPE.Value == 1)
                    {
                        HEIN_SERVICE_TYPE = "THUOC";
                    }
                    else
                    {
                        HEIN_SERVICE_TYPE = "DICHVU";
                    }
                }
                else
                {
                    HEIN_SERVICE_TYPE = "DICHVU";
                }
                // HSDT theo in time
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                if (filter.TIME_TYPE.HasValue)
                {
                    if (filter.TIME_TYPE.Value == 0)
                    {
                        filterTreatment.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                        filterTreatment.FEE_LOCK_TIME_TO = filter.TIME_TO;
                        filterTreatment.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                    else if (filter.TIME_TYPE.Value == 1)
                    {
                        filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                        filterTreatment.OUT_TIME_TO = filter.TIME_TO;
                        filterTreatment.IS_PAUSE = true;
                    }
                    else
                    {
                        filterTreatment.IN_TIME_FROM = filter.TIME_FROM;
                        filterTreatment.IN_TIME_TO = filter.TIME_TO;
                    }
                }
                else
                {

                    filterTreatment.IN_TIME_FROM = filter.TIME_FROM;
                    filterTreatment.IN_TIME_TO = filter.TIME_TO; 
                }
                listTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatment = listTreatment.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    listTreatment = listTreatment.Where(o => filter.PATIENT_TYPE_ID == o.TDL_PATIENT_TYPE_ID).ToList();
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    listTreatment = listTreatment.Where(o => filter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID??0)).ToList();
                }
                if (filter.IS_TREAT != null)
                {
                    if (filter.IS_TREAT == true)
                    {
                        listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                    else
                    {
                        listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }
                var listTreatmentId = listTreatment.Select(o => o.ID).ToList(); 

                // DV-YC tuong ung
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0; 
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            SERVICE_TYPE_ID = filter.SERVICE_TYPE_ID,
                            PATIENT_TYPE_ID = filter.SS_PATIENT_TYPE_ID,
                            PATIENT_TYPE_IDs = filter.SS_PATIENT_TYPE_IDs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false

                        }; 
                        var sereServSub = new HisSereServManager(paramGet).GetView(filterSereServ); 
                        listSereServ.AddRange(sereServSub); 
                    }

                    // lấy ra loginname từ filter và lọc sereServ theo loginname.
                    if (IsNotNullOrEmpty(filter.LOGINNAMEs))
                    {
                        listSereServ = listSereServ.Where(o => filter.LOGINNAMEs.Contains(o.TDL_REQUEST_LOGINNAME)).ToList(); 
                    }
                    HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                    serviceFilter.IDs = listSereServ.Select(x => x.SERVICE_ID).ToList();
                    listService = new HisServiceManager(new CommonParam()).Get(serviceFilter);
                    listPtttGroup =new HisPtttGroupManager(new CommonParam()).Get(new HisPtttGroupFilterQuery());
                }


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
            var result = true; 
            try
            {

                ListRdo.Clear(); 

                if (IsNotNullOrEmpty(listSereServ))
                {
                    listSereServ = listSereServ.OrderBy(o => o.REQUEST_DEPARTMENT_NAME).ThenBy(p => p.SERVICE_ID).ToList();
                    var GroupByServiceId = listSereServ.GroupBy(o => new { o.TDL_REQUEST_ROOM_ID,o.SERVICE_ID, o.VIR_PRICE }).ToList(); 
                    foreach (var group in GroupByServiceId)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        Mrs00291RDO rdo = new Mrs00291RDO();
                        rdo.SERVICE_NAME = listSub.First().TDL_SERVICE_NAME;
                        rdo.SERVICE_CODE = listSub.First().TDL_SERVICE_CODE;
                        rdo.TDL_HEIN_SERVICE_BHYT_NAME = listSub.First().TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.TDL_HEIN_SERVICE_BHYT_CODE = listSub.First().TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.SERVICE_UNIT_NAME = listSub.First().SERVICE_UNIT_NAME;
                        rdo.ROOM_CODE = listSub.First().REQUEST_ROOM_CODE;
                        rdo.DEPARTMENT_CODE = listSub.First().REQUEST_DEPARTMENT_CODE;
                        rdo.ROOM_NAME = listSub.First().REQUEST_ROOM_NAME;
                        rdo.DEPARTMENT_NAME = listSub.First().REQUEST_DEPARTMENT_NAME;
                        rdo.SERVICE_TYPE_NAME = listSub.First().SERVICE_TYPE_NAME;
                        rdo.SERVICE_TYPE_CODE = listSub.First().SERVICE_TYPE_CODE;
                        rdo.HEIN_SERVICE_TYPE_NAME = listSub.First().HEIN_SERVICE_TYPE_NAME;
                        rdo.HEIN_SERVICE_TYPE_CODE = listSub.First().HEIN_SERVICE_TYPE_CODE??"KHAC";
                        rdo.VIR_PRICE = listSub.First().VIR_PRICE??0;
                        rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.VIR_TOTAL_PRICE = listSub.Sum(o => o.VIR_TOTAL_PRICE??0);
                        rdo.AMOUNT_NOITRU = listSub.Where(o => listTreatment.Exists(p => p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && p.ID == o.TDL_TREATMENT_ID)).Sum(o => o.AMOUNT);
                        rdo.AMOUNT_NGOAITRU = listSub.Where(o => listTreatment.Exists(p => p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && p.ID == o.TDL_TREATMENT_ID)).Sum(o => o.AMOUNT);
                        rdo.PATIENT_TYPE_NAME = listSub.First().PATIENT_TYPE_NAME;


                        var service = listService.Where(x => x.ID == listSub.First().SERVICE_ID).FirstOrDefault();
                        if (service!= null)
                        {
                            var ptttGroup = listPtttGroup.Where(x => x.ID == service.PTTT_GROUP_ID).FirstOrDefault();
                            if (ptttGroup!=null)
                            {
                                rdo.PTTT_GROUP_NAME = ptttGroup.PTTT_GROUP_NAME;
                            }
                        }
                        rdo.PRICE_HEIN = listSub.First().ORIGINAL_PRICE * (1 + listSub.First().VAT_RATIO);
                        rdo.PRICE_NO_HEIN = listSub.First().PRICE * (1 + listSub.First().VAT_RATIO);
                        rdo.PRICE_DVYC =  listSub.First().PRICE * (1 + listSub.First().VAT_RATIO);
                        rdo.TOTAL_USE = listSub.Count();
                        rdo.TOTAL_USE_BHYT = listSub.Where(x => x.HEIN_CARD_NUMBER != null).Count();
                        ListRdo.Add(rdo); 

                    }
                }

                result = true; 
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            if (((Mrs00291Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00291Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00291Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00291Filter)reportFilter).TIME_TO));
            }
            dicSingleTag.Add("HEIN_SERVICE_TYPE", HEIN_SERVICE_TYPE);
            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "Services", ListRdo.GroupBy(o => new { o.SERVICE_CODE, o.VIR_PRICE }).Select(p=>p.First()).ToList());

            objectTag.AddRelationship(store, "Services", "Report", new string[] { "SERVICE_CODE", "VIR_PRICE" }, new string[] { "SERVICE_CODE", "VIR_PRICE" });
        }


    }

}
