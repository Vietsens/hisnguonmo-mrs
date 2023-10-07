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
using MRS.Proccessor.Mrs00517;
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

namespace MRS.Processor.Mrs00517
{
    public class Mrs00517Processor : AbstractProcessor
    {
        private List<Mrs00517RDO> ListRdo = new List<Mrs00517RDO>();
        private List<Mrs00517RDO> ListRdoMedium = new List<Mrs00517RDO>();
        private List<Mrs00517RDO> ListRdoParent = new List<Mrs00517RDO>();
        Mrs00517Filter filter = null;
        string thisReportTypeCode = "";
        List<V_HIS_ROOM> listHisRoom = new List<V_HIS_ROOM>();
        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        private List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        private List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();

        public Mrs00517Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00517Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00517Filter)this.reportFilter;
            try
            {
                //Danh sách phòng tiếp đón
                var listReceptionRoom = new HisReceptionRoomManager().Get(new HisReceptionRoomFilterQuery());
                
                HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                HisTransactionfilter.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                HisTransactionfilter.TRANSACTION_TIME_TO = filter.TIME_TO;
                HisTransactionfilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                HisTransactionfilter.HAS_SALL_TYPE = false;
                listHisTransaction = new HisTransactionManager(param).Get(HisTransactionfilter);
                listHisTransaction = listHisTransaction.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                if (listHisTransaction != null && listHisTransaction.Count > 0)
                {
                    var listTreatmentId = listHisTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                    var skip = 0;

                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        treatmentFilter.IDs = limit;
                        var listHisTreatmentSub = new HisTreatmentManager(param).Get(treatmentFilter);
                        if (listHisTreatmentSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisTreatmentSub Get null");
                        else
                            listHisTreatment.AddRange(listHisTreatmentSub);
                    }
                    skip = 0;

                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = limit;
                        HisSereServfilter.ORDER_FIELD = "TDL_SERVICE_TYPE_ID";
                        HisSereServfilter.ORDER_DIRECTION = "ASC";
                        HisSereServfilter.HAS_EXECUTE = true;
                        HisSereServfilter.IS_EXPEND = false;
                        HisSereServfilter.PATIENT_TYPE_ID = filter.PATIENT_TYPE_ID;
                        var listHisSereServSub = new HisSereServManager(param).Get(HisSereServfilter);
                        if (listHisSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                        else
                            listHisSereServ.AddRange(listHisSereServSub);
                    }
                    skip = 0;
                    var transactionIds = listHisTransaction.Select(o => o.ID).Distinct().ToList();
                    while (transactionIds.Count - skip > 0)
                    {
                        var limit = transactionIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                        HisSereServBillfilter.BILL_IDs = limit;
                        
                        var listHisSereServBillSub = new HisSereServBillManager(param).Get(HisSereServBillfilter);
                        if (listHisSereServBillSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisSereServBillSub Get null");
                        else
                            listHisSereServBill.AddRange(listHisSereServBillSub);
                    }
                    listHisSereServ = listHisSereServ.Where(o => listHisSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                }
                //- BN đối tượng BHYT: khi khóa giám định thì các dịch vụ mới hiển thị trên báo cáo
                //- BN đối tượng Viện Phí: khi thanh toán thì các dịch vụ mới hiển thị trên báo cáo.
                listHisSereServ = listHisSereServ.Where(o =>
                    //(!new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU }.Contains(o.TDL_SERVICE_TYPE_ID))
                    //&&
                    ((o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                    && listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID
                        && p.IS_LOCK_HEIN == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE))
                        ||(o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT))).ToList();

                //neu la kham thi lay phong chi dinh la phong thuc hien
                //neu la phong tiep don chi dinh thi lay phong thuc hien
                foreach (var item in listHisSereServ)
                {
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                        || listReceptionRoom.Exists(o=>item.TDL_REQUEST_ROOM_ID == o.ROOM_ID))
                    {
                        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                    }

                }
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

                ListRdo = (from r in listHisSereServ select new Mrs00517RDO(r, listHisRoom)).ToList();

                GroupByRoomAndService();
                ListRdoMedium = ListRdo.GroupBy(o => new { o.TDL_SERVICE_TYPE_ID, o.TDL_REQUEST_ROOM_ID }).Select(o => o.First()).ToList();
                ListRdoParent = ListRdo.GroupBy(o => o.TDL_SERVICE_TYPE_ID).Select(o => o.First()).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00517RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByRoomAndService()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_REQUEST_ROOM_ID, o.SERVICE_ID }).ToList();
                ListRdo.Clear();
                Decimal sum = 0;
                Mrs00517RDO rdo;
                List<Mrs00517RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00517RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00517RDO();
                    listSub = item.ToList<Mrs00517RDO>();

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

        private Mrs00517RDO IsMeaningful(List<Mrs00517RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00517RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==filter.PATIENT_TYPE_ID)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Medium", ListRdoMedium);
            objectTag.AddObjectData(store, "Parent", ListRdoParent);
            string[] masterKeyFields = { "TDL_REQUEST_ROOM_ID", "TDL_SERVICE_TYPE_ID" };
            objectTag.AddRelationship(store, "Medium", "Report", masterKeyFields, masterKeyFields);
            objectTag.AddRelationship(store, "Parent", "Medium", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
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
