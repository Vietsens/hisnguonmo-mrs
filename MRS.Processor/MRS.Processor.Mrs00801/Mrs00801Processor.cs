using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00801
{
    public class Mrs00801Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Mrs00801Filter filter = new Mrs00801Filter();
        List<Mrs00801RDO> ListRdo = new List<Mrs00801RDO>();
        List<HIS_SERVICE_REQ> ListServiceReqView = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> ListTreatmentView = new List<HIS_TREATMENT>();

        private string a = "";

        public Mrs00801Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00801Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00801Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu Mrs00801:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));
                //Get dữ liệu
                var ServiceReqFilter = new HisServiceReqFilterQuery();
                ServiceReqFilter.FINISH_TIME_FROM = filter.TIME_FROM;
                ServiceReqFilter.FINISH_TIME_TO = filter.TIME_TO;
                ServiceReqFilter.SERVICE_REQ_TYPE_ID = 1;
                ServiceReqFilter.SERVICE_REQ_STT_ID = 3;
                ServiceReqFilter.EXECUTE_ROOM_IDs = filter.EXAM_ROOM_IDs;
                ListServiceReqView = new HisServiceReqManager(paramGet).Get(ServiceReqFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00801");
                }

                List<long> ServiceReqIds = new List<long>();
                ServiceReqIds = ListServiceReqView.Select(s => s.TREATMENT_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(ServiceReqIds))
                {
                    ServiceReqIds = ServiceReqIds.Distinct().ToList();
                    var skip = 0;
                    while (ServiceReqIds.Count - skip > 0)
                    {
                        var listIDs = ServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var TreatmentFilter = new HisTreatmentFilterQuery();
                        TreatmentFilter.IDs = listIDs;
                        var listTreatmentViewSub = new HisTreatmentManager(paramGet).Get(TreatmentFilter);
                        if (listTreatmentViewSub != null)
                        {
                            ListTreatmentView.AddRange(listTreatmentViewSub);
                        }
                    }
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
            var result = true;
            try
            {
                ListRdo.Clear();
                if (IsNotNullOrEmpty(ListServiceReqView))
                {
                    var ListExecuteRoomIDGR = ListServiceReqView.GroupBy(s => s.EXECUTE_ROOM_ID).ToList();
                    foreach (var item in ListExecuteRoomIDGR)
                    {
                        Mrs00801RDO rdo = new Mrs00801RDO();
                        rdo.ROOM_NAME = HisRoomCFG.HisRooms.Where(s => s.ID == item.First().EXECUTE_ROOM_ID).First().ROOM_NAME;
                        rdo.ROOM_CODE = HisRoomCFG.HisRooms.Where(s => s.ID == item.First().EXECUTE_ROOM_ID).First().ROOM_CODE;

                        rdo.COUNT_EXAM = item.Count();

                        
                        foreach (var group in item)
                        {
                            if (group.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                rdo.COUNT_EXAM_BHYT += 1;
                            }

                            rdo.COUNT_EXAM_VP = rdo.COUNT_EXAM - rdo.COUNT_EXAM_BHYT;

                            if (group.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.COUNT_FEMALE_EXAM++;
                            }

                            if (group.TDL_PATIENT_DOB > (group.INTRUCTION_TIME - 150000000000))
                            {
                                rdo.DOB15++;
                            }

                            var treatment = ListTreatmentView.FirstOrDefault(o => o.ID == group.TREATMENT_ID);
                            if (treatment != null)
                            {
                                if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV
                            && treatment.END_ROOM_ID == group.EXECUTE_ROOM_ID)
                                {
                                    rdo.COUNT_MEDI_HOME_EXAM++;
                                }

                                if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN
                                    && treatment.END_ROOM_ID == group.EXECUTE_ROOM_ID)
                                {
                                    rdo.COUNT_TRANPATI_EXAM++;
                                }

                                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                                    && treatment.CLINICAL_IN_TIME != null && group.EXECUTE_ROOM_ID == treatment.IN_ROOM_ID)
                                {
                                    rdo.COUNT_TREATMENT_IN++;
                                }
                            }
                        }

                        ListRdo.Add(rdo);

                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", ((Mrs00801Filter)this.reportFilter).TIME_FROM);
            dicSingleTag.Add("TIME_TO", ((Mrs00801Filter)this.reportFilter).TIME_TO);
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
