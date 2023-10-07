using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00024
{
    public class Mrs00024Processor : AbstractProcessor
    {
        Mrs00024Filter castFilter = null;
        List<Mrs00024RDO> ListRdo = new List<Mrs00024RDO>();
        List<long> CurrentTreatmentId = new List<long>();

        List<Mrs00024RDO> ListRdoDetail = new List<Mrs00024RDO>();

        List<Mrs00024RDO> rdoParentReq = new List<Mrs00024RDO>();
        List<Mrs00024RDO> rdoPatient = new List<Mrs00024RDO>();

        public Mrs00024Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00024Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00024Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("filter MRS00024" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                ListRdoDetail = ManagerSql.GetSereServ(castFilter);
                ListRdoDetail = ListRdoDetail.GroupBy(s => s.ID).Select(p => p.First()).ToList();
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

                if (IsNotNullOrEmpty(ListRdoDetail))
                {
                    ListRdoDetail = ListRdoDetail.OrderBy(o => o.TDL_REQUEST_LOGINNAME).ThenBy(o => o.PATIENT_CODE).ThenBy(o => o.TDL_INTRUCTION_TIME).ToList();

                    foreach (var item in ListRdoDetail)
                    {
                        item.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(item.TDL_INTRUCTION_TIME);
                        item.REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        item.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        item.EXECUTE_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        item.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        item.REQUEST_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                        item.TDL_REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                        item.TOTAL_PRICE = item.AMOUNT * item.PRICE;
                    }

                    var groupReq = ListRdoDetail.GroupBy(s => s.TDL_REQUEST_LOGINNAME).ToList();
                    foreach (var item in groupReq)
                    {
                        rdoParentReq.Add(ProcessTotal(item.ToList()));
                    }

                    var groupPatient = ListRdoDetail.GroupBy(s => new { s.TDL_REQUEST_LOGINNAME, s.PATIENT_ID }).ToList();
                    foreach (var item in groupPatient)
                    {
                        rdoPatient.Add(ProcessTotal(item.ToList()));
                    }
                    var Groups = ListRdoDetail.GroupBy(g => g.TDL_TREATMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<Mrs00024RDO> listSub = group.ToList<Mrs00024RDO>();
                        if (IsNotNullOrEmpty(listSub))
                        {
                            Mrs00024RDO rdo = new Mrs00024RDO();
                            rdo.PATIENT_ID = listSub.First().TDL_PATIENT_ID ?? 0;
                            rdo.TDL_TREATMENT_ID = listSub.First().TDL_TREATMENT_ID ?? 0;
                            rdo.TDL_TREATMENT_CODE = listSub.First().TDL_TREATMENT_CODE;
                            rdo.PATIENT_CODE = listSub.First().PATIENT_CODE;
                            rdo.PATIENT_NAME = listSub.First().PATIENT_NAME;
                            rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));
                            rdo.VIR_TOTAL_HEIN_PRICE = listSub.Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));

                            if (rdo.VIR_TOTAL_PATIENT_PRICE > 0 || rdo.VIR_TOTAL_HEIN_PRICE > 0)
                            {
                                ListRdo.Add(rdo);
                            }
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportParent", rdoParentReq);
                objectTag.AddObjectData(store, "ReportPatient", rdoPatient);
                objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);

                string[] key = new string[] { "REQUEST_LOGINNAME", "PATIENT_ID" };
                objectTag.AddRelationship(store, "ReportParent", "ReportDetail", key, key);
                objectTag.AddRelationship(store, "ReportParent", "ReportPatient", "REQUEST_LOGINNAME", "REQUEST_LOGINNAME");
                objectTag.AddRelationship(store, "ReportPatient", "ReportDetail", "PATIENT_ID", "PATIENT_ID");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Mrs00024RDO ProcessTotal(List<Mrs00024RDO> lstData)
        {
            Mrs00024RDO result = new Mrs00024RDO();
            try
            {
                if (lstData != null && lstData.Count > 0)
                {
                    result.AMOUNT = lstData.Sum(s => s.AMOUNT);
                    result.PATIENT_CODE = lstData.First().PATIENT_CODE;
                    result.PATIENT_ID = lstData.First().PATIENT_ID;
                    result.PATIENT_NAME = lstData.First().PATIENT_NAME;
                    result.REQUEST_LOGINNAME = lstData.First().TDL_REQUEST_LOGINNAME;
                    result.REQUEST_USERNAME = lstData.First().TDL_REQUEST_USERNAME;
                    result.SERVICE_CODE = lstData.First().SERVICE_CODE;
                    result.SERVICE_NAME = lstData.First().SERVICE_NAME;
                    result.TOTAL_PRICE = lstData.Sum(s => s.TOTAL_PRICE);
                    result.VIR_TOTAL_HEIN_PRICE = lstData.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    result.VIR_TOTAL_PATIENT_PRICE = lstData.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                }
            }
            catch (Exception ex)
            {
                result = new Mrs00024RDO();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
