using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBillFund;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00634
{
    class Mrs00634Processor : AbstractProcessor
    {
        Mrs00634Filter castFilter = null;

        List<Mrs00634RDO> ListRdo = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListService = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListParent = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListRdoExam = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListRdoVC1 = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListRdoVC2 = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListRdoDV1 = new List<Mrs00634RDO>();
        List<Mrs00634RDO> ListRdoDV2 = new List<Mrs00634RDO>();

        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<long> ServiceVaccineId = new List<long>();

        public Mrs00634Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00634Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00634Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.CASHIER_ROOM_ID = castFilter.EXACT_CASHIER_ROOM_ID;
                transactionFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                transactionFilter.IS_CANCEL = false;
                transactionFilter.HAS_SALL_TYPE = false;
                ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(transactionFilter);

                if (IsNotNullOrEmpty(ListTransaction))
                {
                    if (castFilter.CASHIER_LOGINNAME != null)
                    {
                        ListTransaction = ListTransaction.Where(o => castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                    }

                    if (castFilter.CASHIER_LOGINNAMEs != null)
                    {
                        ListTransaction = ListTransaction.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                    }

                    List<long> listTreatmentId = ListTransaction.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        listTreatmentId = listTreatmentId.Distinct().ToList();
                        long? PatientTypeId = null;
                        if (castFilter.IS_BHYT != null)
                        {
                            PatientTypeId = castFilter.IS_BHYT.Value ? HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT : HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                        }

                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                            sereServFilter.TREATMENT_IDs = listIds;
                            sereServFilter.IS_EXPEND = false;
                            sereServFilter.HAS_EXECUTE = true;
                            sereServFilter.PATIENT_TYPE_ID = PatientTypeId;
                            var listSereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                            if (listSereServSub != null)
                            {
                                ListSereServ.AddRange(listSereServSub);
                            }

                            HisServiceReqFilterQuery ServiceReqFilter = new HisServiceReqFilterQuery();
                            ServiceReqFilter.TREATMENT_IDs = listIds;
                            ServiceReqFilter.HAS_EXECUTE = true;
                            var listServiceReqSub = new HisServiceReqManager(paramGet).Get(ServiceReqFilter);
                            if (listServiceReqSub != null)
                            {
                                ListServiceReq.AddRange(listServiceReqSub);
                            }
                        }
                    }

                    List<long> Id = ListTransaction.Select(s => s.ID).ToList();

                    if (IsNotNullOrEmpty(Id))
                    {

                        var skip = 0;
                        while (Id.Count - skip > 0)
                        {
                            var listIds = Id.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                            sereServBillFilter.BILL_IDs = listIds;
                            var listSereServBillSub = new HisSereServBillManager(paramGet).Get(sereServBillFilter);
                            if (listSereServBillSub != null)
                            {
                                ListSereServBill.AddRange(listSereServBillSub);
                            }
                        }
                    }
                }
                ListSereServ = ListSereServ.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).ToList();
                ServiceVaccineId = ManagerSql.GetServiceId();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var item in ListSereServ)
                    {
                        HIS_SERE_SERV_BILL sereServBill = ListSereServBill.FirstOrDefault(o => o.SERE_SERV_ID == item.ID) ?? new HIS_SERE_SERV_BILL();
                        HIS_TRANSACTION transaction = ListTransaction.FirstOrDefault(o => o.ID == sereServBill.BILL_ID) ?? new HIS_TRANSACTION();
                        HIS_SERVICE_REQ serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == item.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        Mrs00634RDO rdo = new Mrs00634RDO(item, serviceReq, transaction);
                        ListService.Add(rdo);
                    }

                    var groupByTreatmentId = ListService.GroupBy(o => new { o.TDL_TREATMENT_ID, o.PATIENT_TYPE_ID }).ToList();
                    foreach (var item in groupByTreatmentId)
                    {
                        HIS_TRANSACTION transaction = item.First().HisTransaction;
                        HIS_SERVICE_REQ serviceReq = item.First().HisServiceReq;
                        HIS_SERE_SERV sereServ = item.First().sereServ;
                        Mrs00634RDO rdo = new Mrs00634RDO(sereServ, serviceReq, transaction);

                        rdo.TDL_TREATMENT_ID = item.First().TDL_TREATMENT_ID;
                        rdo.VIR_TOTAL_PRICE = item.Sum(o => o.VIR_TOTAL_PRICE);
                        rdo.PATIENT_TYPE_ID = item.First().PATIENT_TYPE_ID;
                        rdo.PATIENT_TYPE_NAME = item.First().PATIENT_TYPE_NAME;
                        ListRdo.Add(rdo);
                    }
                    ListParent = ListService.GroupBy(o => o.PATIENT_TYPE_ID).Select(p => p.First()).ToList();
                }

                ProcessGroup();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessGroup()
        {
            try
            {
                if (IsNotNullOrEmpty(ListService))
                {
                    var ssExam = ListService.Where(o => o.sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();
                    if (!IsNotNullOrEmpty(ssExam)) return;

                    //gom theo phòng thực hiện và bác sĩ thực hiện.
                    var grExam = ssExam.GroupBy(g => new { g.HisServiceReq.EXECUTE_ROOM_ID, g.HisServiceReq.EXECUTE_LOGINNAME }).ToList();
                    foreach (var ss in grExam)
                    {
                        if (String.IsNullOrWhiteSpace(ss.First().HisServiceReq.EXECUTE_LOGINNAME))
                        {
                            continue;
                        }

                        var r = ss.First();
                        r.VIR_TOTAL_PRICE = ss.Sum(s => s.VIR_TOTAL_PRICE);
                        r.TOTAL_AMOUNT = ss.Sum(s => s.TOTAL_AMOUNT);

                        ListRdoExam.Add(r);
                    }

                    //dv 
                    var lstServiceTypeId = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN };

                    var ssDv = ListService.Where(o => !lstServiceTypeId.Contains(o.sereServ.TDL_SERVICE_TYPE_ID)).ToList();
                    var ssVaccine = ListService.Where(o => ServiceVaccineId.Contains(o.sereServ.SERVICE_ID)).ToList();

                    if (IsNotNullOrEmpty(ssDv))
                    {
                        var grDv = ssDv.GroupBy(g => g.HisTransaction.ID).OrderBy(o => o.First().HisTransaction.TRANSACTION_CODE).ToList();
                        for (int i = 0; i < grDv.Count; i++)
                        {
                            var dv = grDv[i].First();
                            dv.VIR_TOTAL_PRICE = grDv[i].Sum(s => s.VIR_TOTAL_PRICE);
                            dv.TOTAL_AMOUNT = grDv[i].Sum(s => s.TOTAL_AMOUNT);

                            if (i % 2 == 1)
                            {
                                ListRdoDV2.Add(dv);
                            }
                            else
                            {
                                ListRdoDV1.Add(dv);
                            }
                        }
                    }

                    if (IsNotNullOrEmpty(ssVaccine))
                    {
                        var grVc = ssVaccine.Where(o => !ServiceVaccineId.Contains(o.sereServ.SERVICE_ID)).GroupBy(g => g.HisTransaction.ID).OrderBy(o => o.First().HisTransaction.TRANSACTION_CODE).ToList();

                        for (int i = 0; i < grVc.Count; i++)
                        {
                            var dv = grVc[i].First();
                            dv.VIR_TOTAL_PRICE = grVc[i].Sum(s => s.VIR_TOTAL_PRICE);
                            dv.TOTAL_AMOUNT = grVc[i].Sum(s => s.TOTAL_AMOUNT);

                            if (i % 2 == 1)
                            {
                                ListRdoVC2.Add(dv);
                            }
                            else
                            {
                                ListRdoVC1.Add(dv);
                            }
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
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    CommonParam paramGet = new CommonParam();
                    var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => castFilter.CASHIER_LOGINNAME == o.LOGINNAME).ToList();
                    if (IsNotNullOrEmpty(x))
                    {
                        dicSingleTag.Add("CASHIER_USERNAME", x.First().USERNAME);
                    }
                }

                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    CommonParam paramGet = new CommonParam();
                    var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.LOGINNAME)).ToList();
                    if (IsNotNullOrEmpty(x))
                    {
                        dicSingleTag.Add("CASHIER_USERNAME", string.Join(" - ", x.Select(o => o.USERNAME).ToList()));
                    }
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Service", ListService);
                objectTag.AddRelationship(store, "Report", "Service", new string[] { "TDL_TREATMENT_ID", "PATIENT_TYPE_ID" }, new string[] { "TDL_TREATMENT_ID", "PATIENT_TYPE_ID" });
                objectTag.AddObjectData(store, "Parent", ListParent);
                objectTag.AddRelationship(store, "Parent", "Report", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID");

                objectTag.AddObjectData(store, "ReportExam", ListRdoExam);
                objectTag.AddObjectData(store, "ReportVc1", ListRdoVC1);
                objectTag.AddObjectData(store, "ReportVc2", ListRdoVC2);
                objectTag.AddObjectData(store, "ReportDv1", ListRdoDV1);
                objectTag.AddObjectData(store, "ReportDv2", ListRdoDV2);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
