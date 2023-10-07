using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMestStt;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00301
{
    public class Mrs00301Processor : AbstractProcessor
    {
        private List<Mrs00301RDO> ListRdo = new List<Mrs00301RDO>();
        private List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq = new Dictionary<long, List<HIS_SERVICE_REQ>>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        CommonParam paramGet = new CommonParam();

        public Mrs00301Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00301Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            try
            {
                HisExpMestFilterQuery PrescriptionViewFilter = new HisExpMestFilterQuery()
                 {
                     TDL_INTRUCTION_TIME_FROM = ((Mrs00301Filter)this.reportFilter).TIME_FROM,
                     TDL_INTRUCTION_TIME_TO = ((Mrs00301Filter)this.reportFilter).TIME_TO,
                     MEDI_STOCK_IDs = ((Mrs00301Filter)this.reportFilter).MEDI_STOCK_IDs,
                     MEDI_STOCK_ID = ((Mrs00301Filter)this.reportFilter).MEDI_STOCK_ID,
                     EXP_MEST_STT_IDs = new List<long>()
                     {
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DRAFT,
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__EXECUTE,
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REJECT,
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__REQUEST
                     },
                     EXP_MEST_TYPE_IDs = new List<long>()
                     {
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                         IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
                     }
                 };
                var ListPrescription = new HisExpMestManager(paramGet).Get(PrescriptionViewFilter);

                List<long> ListTreatmentId = ListPrescription.Where(o => o.TDL_TREATMENT_ID.HasValue).Select(q => q.TDL_TREATMENT_ID.Value).Distinct().ToList();
                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentViewFilterQuery ListTreatmentfilter = new HisTreatmentViewFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var listTreatmentSub = new HisTreatmentManager(paramGet).GetView(ListTreatmentfilter);

                        ListTreatment.AddRange(listTreatmentSub);

                        //Lấy đối tượng để xem các đối tượng đó 
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };

                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        if (IsNotNullOrEmpty(LisPatientTypeAlterLib))
                        {
                            LisPatientTypeAlterLib = LisPatientTypeAlterLib.OrderByDescending(o => o.LOG_TIME).ThenByDescending(p => p.ID).ToList();
                            foreach (var item in LisPatientTypeAlterLib)
                            {
                                if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                    dicPatientTypeAlter[item.TREATMENT_ID] = item;
                            }
                        }

                        //Lấy yêu cầu dịch vụ để lấy phòng xử lý
                        HisServiceReqFilterQuery filterServiceReq = new HisServiceReqFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                        };
                        var ServiceReqSub = new HisServiceReqManager(paramGet).Get(filterServiceReq);
                        if (IsNotNullOrEmpty(ServiceReqSub))
                        {
                            foreach (var item in ServiceReqSub)
                            {
                                if (!dicServiceReq.ContainsKey(item.TREATMENT_ID)) dicServiceReq[item.TREATMENT_ID] = new List<HIS_SERVICE_REQ>();
                                dicServiceReq[item.TREATMENT_ID].Add(item);
                            }
                        }
                    }

                    //Loại bỏ các trường hợp là nội trú và điều trị ngoại trú
                    ListTreatment = ListTreatment.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    var examRoomFilter = new HisExecuteRoomFilterQuery();
                    examRoomFilter.IS_EXAM = true;
                    var listExamRoom = new HisExecuteRoomManager(param).Get(examRoomFilter);
                    ListRdo = (from d in ListTreatment select new Mrs00301RDO(d, dicServiceReq, dicPatientTypeAlter, listExamRoom)).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00301Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00301Filter)this.reportFilter).TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
