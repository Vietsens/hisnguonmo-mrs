using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00534;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatmentBedRoom;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00534
{
    public class Mrs00534Processor : AbstractProcessor
    {
        private List<Mrs00534RDO> ListRdoDetail = new List<Mrs00534RDO>();
        private List<Mrs00534RDO> ListRdo = new List<Mrs00534RDO>();

        HisTreatmentView4FilterQuery filter = null;

        string thisReportTypeCode = "";

        List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT_BED_ROOM> listHisTreatmentBedRoom = new List<HIS_TREATMENT_BED_ROOM>();
        List<V_HIS_BED_ROOM> listHisBedRoom = new List<V_HIS_BED_ROOM>();

        public Mrs00534Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(HisTreatmentView4FilterQuery);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (HisTreatmentView4FilterQuery)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00534: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter)); 
            
            try
            {
              //Danh sách HSDT

                listHisTreatment = new HisTreatmentManager().GetView4(filter);
                listHisBedRoom = new HisBedRoomManager().GetView(new HisBedRoomViewFilterQuery());
                List<long> treatmentIds = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                        HisServiceReqfilter.TREATMENT_IDs = limit;
                        HisServiceReqfilter.ORDER_FIELD = "ID";
                        HisServiceReqfilter.ORDER_DIRECTION = "ASC";
                        HisServiceReqfilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        HisServiceReqfilter.HAS_EXECUTE = true;
                        var listHisServiceReqSub = new HisServiceReqManager(param).Get(HisServiceReqfilter);
                        if (listHisServiceReqSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub Get null");
                        else
                            listHisServiceReq.AddRange(listHisServiceReqSub);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listHisServiceReq" + listHisServiceReq.Count);
                }
               
                 //
                 if (listHisTreatment != null && listHisTreatment.Count > 0)
                 {
                     var skip = 0;

                     while (treatmentIds.Count - skip > 0)
                     {
                         var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                         skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                         HisTreatmentBedRoomFilterQuery HisTreatmentBedRoomfilter = new HisTreatmentBedRoomFilterQuery();
                         HisTreatmentBedRoomfilter.TREATMENT_IDs = limit;
                         HisTreatmentBedRoomfilter.ORDER_FIELD = "ID";
                         HisTreatmentBedRoomfilter.ORDER_DIRECTION = "ASC";

                         var listTreatmentBedRoomSub = new HisTreatmentBedRoomManager(param).Get(HisTreatmentBedRoomfilter);
                         if (listTreatmentBedRoomSub == null)
                             Inventec.Common.Logging.LogSystem.Error("listTreatmentBedRoomSub Get null");
                         else
                             listHisTreatmentBedRoom.AddRange(listTreatmentBedRoomSub);
                     }
                     //Inventec.Common.Logging.LogSystem.Info("listHisTreatmentBedRoom" + listHisTreatmentBedRoom.Count);
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
                ListRdoDetail = (from r in listHisTreatment select new Mrs00534RDO(r, listHisServiceReq, listHisTreatmentBedRoom, listHisBedRoom)).ToList();
                ListRdo.Add(GroupSum(ListRdoDetail));
               
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00534RDO>();
                result = false;
            }
            return result;
        }

		
        private Mrs00534RDO GroupSum(List<Mrs00534RDO> list)
        {
            string errorField = "";
			Mrs00534RDO rdo = new Mrs00534RDO();
            try
            {
                if (list.Count == 0) return new Mrs00534RDO();
                Decimal sum = 0;
                PropertyInfo[] pi = Properties.Get<Mrs00534RDO>();
               
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT_EXAM") || field.Name.Contains("COUNT_TREAT"))
                        {
                            sum = list.Sum(s => (Decimal)field.GetValue(s));

                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(list, field)));
                        }
                        
                    }
					
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
				return new Mrs00534RDO();
            }
			return rdo;
        }
        private Mrs00534RDO IsMeaningful(List<Mrs00534RDO> listSub, PropertyInfo field)
        {
            return listSub.FirstOrDefault(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "") ?? new Mrs00534RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.IN_TIME_FROM??filter.CLINICAL_IN_TIME_FROM??filter.OUT_TIME_FROM??filter.CREATE_TIME_FROM??0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.IN_TIME_TO ?? filter.CLINICAL_IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.CREATE_TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            objectTag.AddObjectData(store, "Date", ListRdoDetail.GroupBy(o=>o.IN_DATE).Select(p=>p.First()).ToList());
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

        
    }

}
