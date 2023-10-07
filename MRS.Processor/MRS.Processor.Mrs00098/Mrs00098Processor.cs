using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServExt;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00098
{
    public class Mrs00098Processor : AbstractProcessor
    {
        Mrs00098Filter castFilter = null; 
        List<Mrs00098RDO> ListRdo = new List<Mrs00098RDO>(); 
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>(); 
        CommonParam paramGet = new CommonParam(); 
        List<V_HIS_SERVICE_REQ> ListServiceReq = new List<V_HIS_SERVICE_REQ>(); 
        List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>(); 
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>(); 
        string Treatment_Type_Names; 
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();

        List<string> executeRoomCodes = null;
        public Mrs00098Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00098Filter); 
        }

        protected override bool GetData()///
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00098Filter)this.reportFilter); 
                //yeu cau
                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery(); 
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA; 
                serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                serviceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                serviceReqFilter.FINISH_TIME_FROM = castFilter.FINISH_TIME_FROM;
                serviceReqFilter.FINISH_TIME_TO = castFilter.FINISH_TIME_TO;
                serviceReqFilter.EXECUTE_ROOM_ID = castFilter.EXECUTE_ROOM_ID;
                ListServiceReq = new HisServiceReqManager().GetView(serviceReqFilter);
                executeRoomCodes = SuimCodeFilter(castFilter);
                if (executeRoomCodes != null && executeRoomCodes.Count>0)
                {
                    ListServiceReq = ListServiceReq.Where(o => executeRoomCodes.Contains(o.EXECUTE_ROOM_CODE)).ToList();
                }
                //yc-dv
                GetSereServ(ListServiceReq); 
                var treatmentIds = ListServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                GetSereServExt(ListSereServ); 
                //chuyen doi tuong
                dicPatientTypeAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(treatmentIds).OrderBy(q=>q.LOG_TIME).ThenBy(o=>o.ID).GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList()); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }
        private List<string> SuimCodeFilter(Mrs00098Filter castFilter)
        {
            List<string> result = null;
            try
            {
                result = new List<string>();
                if ((castFilter.IS_SUIM_1 ?? false) && castFilter.SUIM_ROOM_CODE_1 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_1);
                }
                if ((castFilter.IS_SUIM_2 ?? false) && castFilter.SUIM_ROOM_CODE_2 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_2);
                }
                if ((castFilter.IS_SUIM_3 ?? false) && castFilter.SUIM_ROOM_CODE_3 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_3);
                }
                if ((castFilter.IS_SUIM_4 ?? false) && castFilter.SUIM_ROOM_CODE_4 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_4);
                }
                if ((castFilter.IS_SUIM_5 ?? false) && castFilter.SUIM_ROOM_CODE_5 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_5);
                }
                if ((castFilter.IS_SUIM_6 ?? false) && castFilter.SUIM_ROOM_CODE_6 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_6);
                }
                if ((castFilter.IS_SUIM_7 ?? false) && castFilter.SUIM_ROOM_CODE_7 != null)
                {
                    result.Add(castFilter.SUIM_ROOM_CODE_7);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    ListSereServ = ListSereServ.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(treatmentTypeId(o.TDL_TREATMENT_ID.Value))).ToList(); 
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    this.GetSereServExt(ListSereServ);
                    ListRdo.AddRange((from r in ListSereServ select new Mrs00098RDO(r, ListServiceReq, ListSereServExt, dicPatientTypeAlter)).ToList()); 
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private long treatmentTypeId(long p)
        {
            return dicPatientTypeAlter.ContainsKey(p)?dicPatientTypeAlter[p].Last().TREATMENT_TYPE_ID:0; 
        }


        private void GetSereServExt(List<V_HIS_SERE_SERV> hisSereServs)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 

                List<long> sereServIds = hisSereServs.Select(s => s.ID).Distinct().ToList(); 

                if (IsNotNullOrEmpty(sereServIds))
                {
                    var skip = 0; 
                    while (sereServIds.Count - skip > 0)
                    {
                        var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServExtFilterQuery serExtFilter = new HisSereServExtFilterQuery(); 
                        serExtFilter.SERE_SERV_IDs = listIDs; 
                        var hisSereServExts = new MOS.MANAGER.HisSereServExt.HisSereServExtManager().Get(serExtFilter); 
                        if (IsNotNullOrEmpty(hisSereServExts))
                        {
                            ListSereServExt.AddRange(hisSereServExts); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GetSereServ(List<V_HIS_SERVICE_REQ> hisServcieReqs)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 

                List<long> serviceReqIds = hisServcieReqs.Select(s => s.ID).Distinct().ToList(); 

                if (IsNotNullOrEmpty(serviceReqIds))
                {
                    var skip = 0; 
                    while (serviceReqIds.Count - skip > 0)
                    {
                        var listIDs = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                        sereServFilter.SERVICE_REQ_IDs = listIDs; 
                        var hisSereServs = new HisSereServManager(paramGet).GetView(sereServFilter); 
                        if (IsNotNullOrEmpty(hisSereServs))
                        {
                            ListSereServ.AddRange(hisSereServs); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


        private void GetTreatmentType()
        {
            try
            {
                HisTreatmentTypeFilterQuery filter = new HisTreatmentTypeFilterQuery(); 
                if (castFilter.TREATMENT_TYPE_ID.HasValue)
                {
                    filter.ID = castFilter.TREATMENT_TYPE_ID.Value; 
                }
                else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    filter.IDs = castFilter.TREATMENT_TYPE_IDs; 
                }
                var treatmentTypes = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager().Get(filter); 
                if (IsNotNullOrEmpty(treatmentTypes))
                {
                    foreach (var treatmentType in treatmentTypes)
                    {
                        if (String.IsNullOrEmpty(Treatment_Type_Names))
                        {
                            Treatment_Type_Names = treatmentType.TREATMENT_TYPE_NAME.ToUpper(); 
                        }
                        else
                        {
                            Treatment_Type_Names = Treatment_Type_Names + " - " + treatmentType.TREATMENT_TYPE_NAME.ToUpper(); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0 + castFilter.FINISH_TIME_FROM ?? 0)); 
            dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0 + castFilter.FINISH_TIME_TO ?? 0));
            GetTreatmentType();
            if (castFilter.EXECUTE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => castFilter.EXECUTE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }
            dicSingleTag.Add("TREATMENT_TYPE_NAMEs", Treatment_Type_Names); 

            ListRdo = ListRdo.OrderBy(o => o.INTRUCTION_TIME).ThenBy(t => t.PATIENT_ID).ToList(); 
            objectTag.AddObjectData(store, "Report", ListRdo); 
        }
    }
}
