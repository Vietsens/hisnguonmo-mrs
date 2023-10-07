using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisTreatmentBedRoom;

namespace MRS.Processor.Mrs00601
{
    public class Mrs00601Processor : AbstractProcessor
    {
        enum TimeData
        {
            FeeLock,
            Bill
        }

        CommonParam paramGet = new CommonParam();
        Mrs00601Filter filter = null;
        List<Mrs00601RDO> ListRdo = new List<Mrs00601RDO>();
        List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_TRANSACTION> ListHisTransaction = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV> ListHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> ListExamHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV_BILL> ListHisSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_ACCOUNT_BOOK> ListHisAccountBook = new List<HIS_ACCOUNT_BOOK>();

        List<V_HIS_TREATMENT_BED_ROOM> listHisTreatmentBedRoom = new List<V_HIS_TREATMENT_BED_ROOM>();
        //List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        System.Data.DataTable ListReport;

        public Mrs00601Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00601Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00601Filter)reportFilter);
            var result = true;
            try
            {
                string sql = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15);
                
                if (!string.IsNullOrEmpty(sql))
                {
                    ListReport = new ManagerSql().GetSum(filter, sql);
                    return true;
                }
                else
                {
                    ListReport = new System.Data.DataTable();
                }
                TimeData OptionTakeData = TimeData.FeeLock;
                if (this.filter.FEE_LOCK_TIME_FROM != null && this.filter.FEE_LOCK_TIME_TO != null)
                {
                    OptionTakeData = TimeData.FeeLock;
                }
                else if (this.filter.TRANSACTION_TIME_FROM != null && this.filter.TRANSACTION_TIME_TO != null)
                {
                    OptionTakeData = TimeData.Bill;
                }

                List<long> treatmentIds = new List<long>();
                if (OptionTakeData == TimeData.FeeLock)
                {
                    #region Lay theo thoi gian khoa vien phi
                    HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                    HisTreatmentfilter.FEE_LOCK_TIME_FROM = filter.FEE_LOCK_TIME_FROM;
                    HisTreatmentfilter.FEE_LOCK_TIME_TO = filter.FEE_LOCK_TIME_TO;
                    HisTreatmentfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    ListHisTreatment = new HisTreatmentManager(paramGet).Get(HisTreatmentfilter);
                    if (filter.TREATMENT_TYPE_IDs != null)
                    {
                        ListHisTreatment = ListHisTreatment.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID??0)).ToList();
                    }
                    treatmentIds = ListHisTreatment.Select(o => o.ID).Distinct().ToList();
                    if (treatmentIds.Count > 0)
                    {
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var lists = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                            HisTransactionfilter.TREATMENT_IDs = lists;
                            HisTransactionfilter.IS_CANCEL = false;
                            var listHisTransactionSub = new HisTransactionManager(paramGet).Get(HisTransactionfilter);
                            if (listHisTransactionSub != null)
                            {
                                ListHisTransaction.AddRange(listHisTransactionSub);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Lay theo thoi gian giao dich
                    HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                    HisTransactionfilter.TRANSACTION_TIME_FROM = filter.TRANSACTION_TIME_FROM;
                    HisTransactionfilter.TRANSACTION_TIME_TO = filter.TRANSACTION_TIME_TO;
                    HisTransactionfilter.IS_CANCEL = false;
                    ListHisTransaction = new HisTransactionManager(paramGet).Get(HisTransactionfilter);
                    treatmentIds = ListHisTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                    if (treatmentIds.Count > 0)
                    {
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var lists = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                            HisTreatmentfilter.IDs = lists;
                            var listHisTreatmentSub = new HisTreatmentManager(paramGet).Get(HisTreatmentfilter);
                            if (listHisTreatmentSub != null)
                            {
                                ListHisTreatment.AddRange(listHisTreatmentSub);
                            }
                        }
                    }
                    if (filter.TREATMENT_TYPE_IDs != null)
                    {
                        ListHisTreatment = ListHisTreatment.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                        ListHisTransaction = ListHisTransaction.Where(o => ListHisTreatment.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                    }
                    #endregion
                }

                //Báo cáo chỉ lấy những đối tượng phát sinh thanh toán
                var skip1 = 0;
                while (treatmentIds.Count - skip1 > 0)
                {
                    var lists = treatmentIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                    HisSereServfilter.TREATMENT_IDs = lists;
                    HisSereServfilter.HAS_EXECUTE = true;
                    var listHisSereServSub = new HisSereServManager(paramGet).Get(HisSereServfilter);
                    if (listHisSereServSub != null)
                    {
                        ListHisSereServ.AddRange(listHisSereServSub);
                    }

                    //HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                    //HisPatientTypeAlterfilter.TREATMENT_IDs = lists;
                    //HisPatientTypeAlterfilter.ORDER_FIELD = "LOG_TIME";
                    //HisPatientTypeAlterfilter.ORDER_DIRECTION = "DESC";
                    //var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).Get(HisPatientTypeAlterfilter);
                    //if (listHisPatientTypeAlterSub != null)
                    //{
                    //    ListHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    //}

                    HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                    HisServiceReqfilter.TREATMENT_IDs = lists;

                    HisServiceReqfilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    var listHisServiceReqSub = new HisServiceReqManager(paramGet).Get(HisServiceReqfilter);
                    if (listHisServiceReqSub != null)
                    {
                        ListExamHisServiceReq.AddRange(listHisServiceReqSub);
                    }

                    HisTreatmentBedRoomViewFilterQuery HisTreatmentBedRoomfilter = new HisTreatmentBedRoomViewFilterQuery();
                    HisTreatmentBedRoomfilter.TREATMENT_IDs = lists;

                    var listHisTreatmentBedRoomSub = new HisTreatmentBedRoomManager(paramGet).GetView(HisTreatmentBedRoomfilter);
                    if (listHisTreatmentBedRoomSub != null)
                    {
                        listHisTreatmentBedRoom.AddRange(listHisTreatmentBedRoomSub);
                    }

                    HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                    HisSereServBillfilter.TDL_TREATMENT_IDs = lists;
                    var listHisSereServBillSub = new HisSereServBillManager(paramGet).Get(HisSereServBillfilter);
                    if (listHisSereServBillSub != null)
                    {
                        ListHisSereServBill.AddRange(listHisSereServBillSub);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filter.CASHIER_LOGINNAME))
                {
                    ListHisTransaction = ListHisTransaction.Where(o => o.CASHIER_LOGINNAME == filter.CASHIER_LOGINNAME).ToList();
                }

                ListHisSereServBill = ListHisSereServBill.Where(p => ListHisTransaction.Exists(q => q.ID == p.BILL_ID)).ToList();
                ListHisSereServ = ListHisSereServ.Where(o => ListHisSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                ListHisTreatment = ListHisTreatment.Where(o => ListHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o.ID)).ToList();

                if (filter.BRANCH_ID.HasValue)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.BRANCH_ID == o.BRANCH_ID).ToList();
                }

                if (filter.END_DEPARTMENT_IDs != null && filter.IS_DETAIL_DEPA_RO != true)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.END_DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID ?? 0)).ToList();
                }

                if (filter.EXAM_ROOM_IDs != null && filter.IS_DETAIL_DEPA_RO != true)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.EXAM_ROOM_IDs.Contains(o.IN_ROOM_ID ?? o.END_ROOM_ID ?? o.TDL_FIRST_EXAM_ROOM_ID ?? 0)).ToList();
                }

                if (filter.REQUEST_ROOM_IDs != null && filter.IS_DETAIL_DEPA_RO != true)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.REQUEST_ROOM_IDs.Contains(o.IN_ROOM_ID ?? o.END_ROOM_ID ?? o.TDL_FIRST_EXAM_ROOM_ID ?? 0)).ToList();
                }


                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    //foreach (var item in ListHisSereServ)
                    //{
                    //    var patientTypeAlterSub = ListHisPatientTypeAlter.Where(o => o.TREATMENT_ID == item.TDL_TREATMENT_ID).ToList();
                    //    if (patientTypeAlterSub == null) continue;
                    //    var curentPatientTypeAlter = patientTypeAlterSub.OrderByDescending(o => o.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TDL_TREATMENT_ID && o.LOG_TIME <= item.TDL_INTRUCTION_TIME);
                    //    if (curentPatientTypeAlter == null)
                    //    {
                    //        curentPatientTypeAlter = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == item.TDL_TREATMENT_ID && o.LOG_TIME > item.TDL_INTRUCTION_TIME);
                    //    }
                    //    if (curentPatientTypeAlter != null && filter.TREATMENT_TYPE_IDs.Contains(curentPatientTypeAlter.TREATMENT_TYPE_ID))
                    //    {
                    //        //nop
                    //    }
                    //    else
                    //    {
                    //        item.IS_DELETE = IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE;
                    //    }
                        
                    //}
                    //ListHisSereServ = ListHisSereServ.Where(p => p.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE).ToList();
                    //ListHisTreatment = ListHisTreatment.Where(o => ListHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o.ID)).ToList();
                }

                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.PATIENT_TYPE_ID == o.TDL_PATIENT_TYPE_ID).ToList();
                }

                if (filter.SESE_PATIENT_TYPE_ID != null)
                {
                    ListHisSereServ = ListHisSereServ.Where(p => filter.SESE_PATIENT_TYPE_ID == p.PATIENT_TYPE_ID).ToList();
                    ListHisTreatment = ListHisTreatment.Where(o => ListHisSereServ.Exists(p => p.TDL_TREATMENT_ID == o.ID)).ToList();
                }

                HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00601";
                ListHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);


                HisAccountBookFilterQuery AccountBookFilter = new HisAccountBookFilterQuery();
                ListHisAccountBook = new HisAccountBookManager(paramGet).Get(AccountBookFilter);
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

                if (IsNotNullOrEmpty(ListHisTreatment))
                {
                    foreach (var item in ListHisSereServ)
                    {
                        if ((item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) > (item.VIR_TOTAL_PATIENT_PRICE ?? 0))
                        {
                            item.VIR_TOTAL_PATIENT_PRICE_BHYT = (item.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        }
                    }
                    if(filter.IS_DETAIL_DEPA_RO!=true)
                    {
                        ListRdo = (from b in ListHisTreatment select new Mrs00601RDO(b, ListHisSereServ, ListHisTransaction, ListHisServiceRetyCat, this.filter, ListHisAccountBook, ListExamHisServiceReq, listHisTreatmentBedRoom)).ToList();
                    }    
                    else
                    {

                        if (filter.END_DEPARTMENT_IDs != null)
                        {
                            ListHisSereServ = ListHisSereServ.Where(o => filter.END_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                        }

                        if (filter.EXAM_ROOM_IDs != null)
                        {
                            ListHisSereServ = ListHisSereServ.Where(o => filter.EXAM_ROOM_IDs.Contains(o.TDL_REQUEST_ROOM_ID)).ToList();
                        }

                        if (filter.REQUEST_ROOM_IDs != null)
                        {
                            ListHisSereServ = ListHisSereServ.Where(o => filter.REQUEST_ROOM_IDs.Contains(o.TDL_REQUEST_ROOM_ID)).ToList();
                        }
                        var groupByRoom = ListHisSereServ.GroupBy(g => new { g.TDL_REQUEST_ROOM_ID, g.TDL_TREATMENT_ID }).ToList();
                        ListRdo = (from ro in groupByRoom select new Mrs00601RDO(ListHisTreatment.FirstOrDefault(o=>o.ID==ro.Key.TDL_TREATMENT_ID)??new HIS_TREATMENT(), ro.ToList<HIS_SERE_SERV>(), ListHisTransaction, ListHisServiceRetyCat, this.filter, ListHisAccountBook, ListExamHisServiceReq, listHisTreatmentBedRoom,ro.First().TDL_REQUEST_DEPARTMENT_ID, ro.Key.TDL_REQUEST_ROOM_ID)).ToList();

                    }
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
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.FEE_LOCK_TIME_FROM ?? filter.TRANSACTION_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.FEE_LOCK_TIME_TO ?? filter.TRANSACTION_TIME_TO ?? 0));
                if (filter.IS_TREAT != null)
                    dicSingleTag.Add("TREATMENT_TYPE_NAME", filter.IS_TREAT==true?"NỘI TRÚ":"NGOẠI TRÚ");
                dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
                objectTag.SetUserFunction(store, "Element", new RDOElement());
                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TRANSACTION_TIME).ToList());
                if(filter.IS_DETAIL_DEPA_RO!=true)
                {
                    objectTag.AddObjectData(store, "Report0", ListRdo.Where(o => o.LAST_DEPARTMENT_ID != o.TDL_FIRST_EXAM_DEPARTMENT_ID).OrderBy(o => o.TRANSACTION_TIME).ToList());
                    objectTag.AddObjectData(store, "Report1", ListRdo.Where(o => o.LAST_DEPARTMENT_ID == o.TDL_FIRST_EXAM_DEPARTMENT_ID).OrderBy(o => o.TRANSACTION_TIME).ToList());
                    objectTag.AddObjectData(store, "Department0", ListRdo.Where(o => o.LAST_DEPARTMENT_ID != o.TDL_FIRST_EXAM_DEPARTMENT_ID).GroupBy(o => o.LAST_DEPARTMENT_ID).Select(p => p.First()).ToList());
                    objectTag.AddObjectData(store, "Room", ListRdo.GroupBy(o => o.TDL_FIRST_EXAM_ROOM_ID).Select(p => p.First()).ToList());
                }   
                else
                {
                    List<long> listDepartmentId__Exam = HisDepartmentCFG.LIST_DEPARTMENT_ID__GROUP_KKB ?? new List<long>();
                    //foreach (var item in HisDepartmentCFG.DEPARTMENTs.Where(o=>o.is_exam))
                    //{
                    //    if()
                    //}
                    objectTag.AddObjectData(store, "Report0", ListRdo.Where(o => !listDepartmentId__Exam.Contains(o.TDL_FIRST_EXAM_DEPARTMENT_ID)).OrderBy(o => o.TRANSACTION_TIME).ToList());
                    objectTag.AddObjectData(store, "Report1", ListRdo.Where(o => listDepartmentId__Exam.Contains(o.TDL_FIRST_EXAM_DEPARTMENT_ID)).OrderBy(o => o.TRANSACTION_TIME).ToList());
                    objectTag.AddObjectData(store, "Department0", ListRdo.Where(o => !listDepartmentId__Exam.Contains(o.TDL_FIRST_EXAM_DEPARTMENT_ID)).GroupBy(o => o.LAST_DEPARTMENT_ID).Select(p => p.First()).ToList());
                    objectTag.AddObjectData(store, "Room", ListRdo.Where(o => listDepartmentId__Exam.Contains(o.TDL_FIRST_EXAM_DEPARTMENT_ID)).GroupBy(o => o.TDL_FIRST_EXAM_ROOM_ID).Select(p => p.First()).ToList());
                }    
                objectTag.AddObjectData(store, "Department", ListRdo.GroupBy(o => o.LAST_DEPARTMENT_ID).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Department", "Report", "LAST_DEPARTMENT_ID", "LAST_DEPARTMENT_ID");
                objectTag.AddRelationship(store, "Department0", "Report0", "LAST_DEPARTMENT_ID", "LAST_DEPARTMENT_ID");
                objectTag.AddRelationship(store, "Room", "Report1", "TDL_FIRST_EXAM_ROOM_ID", "TDL_FIRST_EXAM_ROOM_ID");
                objectTag.AddObjectData(store, "Year", ListReport);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
