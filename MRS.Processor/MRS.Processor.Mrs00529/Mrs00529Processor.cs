using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00529;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisReceptionRoom;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00529
{
    public class Mrs00529Processor : AbstractProcessor
    {
        private List<Mrs00529RDO> ListRdoDetail = new List<Mrs00529RDO>();
        private List<Mrs00529RDO> ListRdo = new List<Mrs00529RDO>();
        Mrs00529Filter filter = null;
        string thisReportTypeCode = "";
        List<V_HIS_ROOM> listHisRoom = new List<V_HIS_ROOM>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        private List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        private List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();

        public Mrs00529Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00529Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00529Filter)this.reportFilter;
            Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00529: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter)); 
            try
            {
                //Danh sách phòng tiếp đón
                var listReceptionRoom = new HisReceptionRoomManager().Get(new HisReceptionRoomFilterQuery());
                //Cac giao dich trong thoi gian bao cao
                HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                HisTransactionfilter.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                HisTransactionfilter.TRANSACTION_TIME_TO = filter.TIME_TO;
                HisTransactionfilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                HisTransactionfilter.HAS_SALL_TYPE = false;
                listHisTransaction = new HisTransactionManager(param).Get(HisTransactionfilter);
                listHisTransaction = listHisTransaction.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                if (listHisTransaction != null && listHisTransaction.Count > 0)
                {
                    var listBillId = listHisTransaction.Select(o => o.ID).Distinct().ToList();
                    var skip = 0;

                    while (listBillId.Count - skip > 0)
                    {
                        var limit = listBillId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                        HisSereServBillfilter.BILL_IDs = limit;
                        var listHisSereServBillSub = new HisSereServBillManager(param).Get(HisSereServBillfilter);
                        if (listHisSereServBillSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServBillSub Get null");
                        else
                            listHisSereServBill.AddRange(listHisSereServBillSub);
                    }
                }
                //Cac BN khoa vien phi trong thoi gian bao cao
                 HisTreatmentView4FilterQuery HisTreatmentfilter = new HisTreatmentView4FilterQuery();
                HisTreatmentfilter.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                HisTreatmentfilter.FEE_LOCK_TIME_TO = filter.TIME_TO;
                HisTreatmentfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                listHisTreatment = new HisTreatmentManager(param).GetView4(HisTreatmentfilter);

                var treatmentIds = listHisTransaction.Select(o=>o.TREATMENT_ID??0).Distinct().ToList()??new List<long>();
                treatmentIds.AddRange(listHisTreatment.Select(o=>o.ID)??new List<long>());
                treatmentIds = treatmentIds.Distinct().ToList();
                if(treatmentIds.Count>0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = limit;
                        HisSereServfilter.ORDER_FIELD = "TDL_SERVICE_TYPE_ID";
                        HisSereServfilter.ORDER_DIRECTION = "ASC";
                        HisSereServfilter.HAS_EXECUTE = true;
                        HisSereServfilter.IS_EXPEND = false;
                        HisSereServfilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH;
                        var listHisSereServSub = new HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listHisSereServSub);
                    }
                    //Service req de loc cac yeu cau kham da thuc hien xong
                    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                        HisServiceReqfilter.TREATMENT_IDs = limit;
                        HisServiceReqfilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                        var listHisServiceReqSub = new HisServiceReqManager(param).Get(HisServiceReqfilter);
                        if (listHisServiceReqSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub Get null");
                        else
                            listHisServiceReq.AddRange(listHisServiceReqSub);
                    }

                    //Doi tuog BN
                    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = limit;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ASC";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ID";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }
                  var lastHisPatientTypeAlter  =listHisPatientTypeAlter.OrderBy(o=>o.LOG_TIME).GroupBy(p=>p.TREATMENT_ID).Select(q=>q.Last()).ToList();
                //- BN đối tượng BHYT: khi khóa viện phí thì các dịch vụ mới hiển thị trên báo cáo
                //- BN đối tượng Viện Phí: khi thanh toán thì các dịch vụ mới hiển thị trên báo cáo.
                listHisSereServ = listHisSereServ.Where(o =>
                    (lastHisPatientTypeAlter.Exists(r=>r.TREATMENT_ID==o.TDL_TREATMENT_ID&&r.PATIENT_TYPE_ID==HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    && listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID
                        && p.IS_LOCK_HEIN == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE))
                        ||
                        (lastHisPatientTypeAlter.Exists(r=>r.TREATMENT_ID==o.TDL_TREATMENT_ID&&r.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        &&listHisSereServBill.Exists(q=>q.SERE_SERV_ID==o.ID))).ToList();

                //số ca là số dv khám đã được kết thúc 
                listHisSereServ = listHisSereServ.Where(o =>listHisServiceReq.Exists(p=>p.ID==o.SERVICE_REQ_ID)).ToList();
                listHisRoom = new HisRoomManager(param).GetView(new HisRoomViewFilterQuery());
               
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

                ListRdoDetail = (from r in listHisSereServ select new Mrs00529RDO(r, listHisRoom)).ToList();

                GroupByExecuteRoom();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00529RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByExecuteRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.TDL_EXECUTE_ROOM_ID}).ToList();
                Decimal sum = 0;
                Mrs00529RDO rdo;
                List<Mrs00529RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00529RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00529RDO();
                    listSub = item.ToList<Mrs00529RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00529RDO IsMeaningful(List<Mrs00529RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00529RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            //dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==filter.PATIENT_TYPE_ID)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            objectTag.SetUserFunction(store, "MergeSampRow", new CustomerFuncMergeSameData());
        }


       
    }
    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long MediStockId;
        int SameType;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (SameType == ServiceId && MediStockId == mediId)
                    {
                        return true;
                    }
                    else
                    {
                        MediStockId = mediId;
                        SameType = ServiceId;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }


}
