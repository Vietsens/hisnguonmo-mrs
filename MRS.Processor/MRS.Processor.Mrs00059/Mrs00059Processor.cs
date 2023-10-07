using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00059
{
    public class Mrs00059Processor : AbstractProcessor
    {
        Mrs00059Filter castFilter = null;
        List<Mrs00059RDO> ListRdo = new List<Mrs00059RDO>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<BillRDO> ListBill = new List<BillRDO>();
        Dictionary<long, string> dicCategory = new Dictionary<long, string>();
        public Mrs00059Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00059Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00059Filter)this.reportFilter);
                LoadDataToRam();

                //mã nhóm báo cáo
                GetCategory();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetCategory()
        {
            var serviceRetyCat = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery() { REPORT_TYPE_CODE__EXACT = "MRS00059" });
            if (serviceRetyCat != null && serviceRetyCat.Count > 0)
            {
                dicCategory = serviceRetyCat.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.First().CATEGORY_CODE);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListTreatmentLock();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatmentLock()
        {
            try
            {
                if (ListTreatment != null && ListTreatment.Count > 0)
                {
                    //ListTreatment = ListTreatmentLock.Where(o => o.IS_LOCK == 1).ToList();
                    if (ListTreatment.Count > 0)
                    {
                        CommonParam paramGet = new CommonParam();
                        int start = 0;
                        int count = ListTreatment.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            List<V_HIS_TREATMENT> hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                            ProcessDetailListTreatment(paramGet, hisTreatments);
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }

                        ListRdo = ProcessListRdo();

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("co exception xa ra tai DAOGET trong qua trinh tong hop du lieu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> Treatments)
        {
            try
            {
                //chi tiet dich vu
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSereServ = new HisSereServManager(paramGet).GetView(filter);
                //chi tiet y lenh
                HisServiceReqFilterQuery srfilter = new HisServiceReqFilterQuery();
                srfilter.TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListServiceReq = new HisServiceReqManager(paramGet).Get(srfilter);
                //chi tiet thanh toan
                HisSereServBillFilterQuery filterssBill = new HisSereServBillFilterQuery();
                filterssBill.TDL_TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSereServBill = new HisSereServBillManager(paramGet).Get(filterssBill);
                //chi tiet tam thu dich vu
                HisSereServDepositFilterQuery filterssDeposit = new HisSereServDepositFilterQuery();
                filterssDeposit.TDL_TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSereServDeposit = new HisSereServDepositManager(paramGet).Get(filterssDeposit);
                //chi tiet hoan thu dich vu
                HisSeseDepoRepayFilterQuery filterssRepay = new HisSeseDepoRepayFilterQuery();
                filterssRepay.TDL_TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSeseDepoRepay = new HisSeseDepoRepayManager(paramGet).Get(filterssRepay);
                //chi tiet giao dich
                HisTransactionViewFilterQuery filterTransaction = new HisTransactionViewFilterQuery();
                filterTransaction.TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListTransaction = new HisTransactionManager(paramGet).GetView(filterTransaction);

                //loc bot benh nhan theo cac dieu kien loc giao dich
                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    var transactionFilter = ListTransaction.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                    var treatmentIdFilter = transactionFilter.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    ListTransaction = ListTransaction.Where(o => treatmentIdFilter.Contains(o.TREATMENT_ID)).ToList();
                    ListSereServBill = ListSereServBill.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID)).ToList();
                    ListSereServ = ListSereServ.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                    ListSereServDeposit = ListSereServDeposit.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID)).ToList();
                    ListSeseDepoRepay = ListSeseDepoRepay.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID)).ToList();
                    Treatments = Treatments.Where(o => treatmentIdFilter.Contains(o.ID)).ToList();
                }
                //tao danh sach chi phi theo doi tuong thanh toan
                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    var Groups = ListSereServ.GroupBy(g => g.PATIENT_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00059RDO rdo = new Mrs00059RDO();
                            rdo.PATIENT_TYPE_CODE = listSub[0].PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = listSub[0].PATIENT_TYPE_NAME;
                            foreach (var sereServ in listSub)
                            {
                                rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                rdo.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                rdo.SERVICE_AMOUNT += sereServ.AMOUNT;
                            }
                            ListRdo.Add(rdo);
                        }
                    }
                }
                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                //tao danh sach chi phi tung hoa don thanh toan 
                foreach (var item in Treatments)
                {
                    var sereServSub = ListSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID && o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE
                    && o.IS_NO_EXECUTE == null).ToList();
                    var sereReqSub = ListServiceReq.Where(o => o.TREATMENT_ID == item.ID && o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE
                    && o.IS_NO_EXECUTE == null).ToList();

                    var transactionSub = ListTransaction.Where(o => o.TREATMENT_ID == item.ID && o.IS_CANCEL == null).ToList();
                    var billSub = ListTransaction.Where(o => o.TREATMENT_ID == item.ID && o.IS_CANCEL == null && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP).ToList();
                    var ssbSub = ListSereServBill.Where(o => o.TDL_TREATMENT_ID == item.ID && o.IS_CANCEL == null).ToList();
                    var ssdSub = ListSereServDeposit.Where(o => o.TDL_TREATMENT_ID == item.ID && o.IS_CANCEL == null).ToList();
                    var ssrSub = ListSeseDepoRepay.Where(o => o.TDL_TREATMENT_ID == item.ID && o.IS_CANCEL == null).ToList();
                        //nếu thêm bệnh nhân tự nguyện thì sẽ không lọc bỏ đổi tượng tự nguyện
                    if (castFilter.IS_ADD_VOLUNTARY != true && ( item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM 
                        || item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                    {
                        if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                        {
                            //chỉ lấy đổi tượng thanh toán là bảo hiểm
                            sereServSub = sereServSub.Where(o => o.PATIENT_TYPE_ID == patientTypeIdBhyt).ToList();
                            //chỉ lấy tam thu, hoan thu đổi tượng thanh toán là bảo hiểm
                            ssdSub = ssdSub.Where(o => o.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt).ToList();
                            ssrSub = ssrSub.Where(o => o.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt).ToList();
                        }
                    }
                    if (sereServSub.Count <= 0) continue;
                    //thu truc tiep
                    var billDirectIdSub = ssbSub.Where(o =>
                        (billSub.FirstOrDefault(p => p.ID == o.BILL_ID) ?? new V_HIS_TRANSACTION()).IS_DIRECTLY_BILLING == 1
                        || o.PRICE > 0
                                        &&
                                        (
                                        (billSub.FirstOrDefault(p => p.ID == o.BILL_ID) ?? new V_HIS_TRANSACTION()).TRANSACTION_TIME < (sereReqSub.FirstOrDefault(q => q.ID == o.TDL_SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ()).START_TIME
                                        ||
                                        (sereReqSub.FirstOrDefault(q => q.ID == o.TDL_SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ()).START_TIME == null 
                                                                                           && 
                                                                                           (
                                                                                           (billSub.FirstOrDefault(p => p.ID == o.BILL_ID) ?? new V_HIS_TRANSACTION()).TRANSACTION_TIME<item.OUT_TIME
                                                                                           ||
                                                                                           item.OUT_TIME ==null
                                                                                           )
                                        )
                        ).Select(o => o.BILL_ID).ToList();
                    billSub = billSub.Where(o => !billDirectIdSub.Contains(o.ID)).ToList();
                    var ssbDirectSub = ssbSub.Where(o => billDirectIdSub.Contains(o.BILL_ID)).ToList();


                    //neu benh nhân khong có hóa đơn thanh toán thì hiển thị 1 dòng thông tin chi phí của bệnh nhân đó
                    if (billSub.Count == 0)
                    {
                        BillRDO rdo = new BillRDO(item, null, null, ssbDirectSub, transactionSub, sereServSub, ssdSub, ssrSub, dicCategory, castFilter);
                        ListBill.Add(rdo);
                    }

                    //nếu bệnh nhân có hóa đơn thanh toán thì duyệt theo các hóa đơn thanh toán và hiển thị các dòng chi phí theo các hóa đơn đó
                    else
                    {
                        foreach (var bill in billSub)
                        {
                            var ssbs = ssbSub.Where(o => o.BILL_ID == bill.ID).ToList();
                            BillRDO rdo = new BillRDO(item, bill, ssbs, ssbDirectSub, transactionSub, sereServSub, ssdSub, ssrSub, dicCategory, castFilter);
                            ListBill.Add(rdo);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private List<Mrs00059RDO> ProcessListRdo()
        {
            List<Mrs00059RDO> result = new List<Mrs00059RDO>();
            try
            {
                if (ListRdo.Count > 0)
                {
                    var Groups = ListRdo.GroupBy(g => g.PATIENT_TYPE_CODE).ToList();
                    foreach (var group in Groups)
                    {
                        List<Mrs00059RDO> listSub = group.ToList<Mrs00059RDO>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00059RDO rdo = new Mrs00059RDO();
                            rdo.PATIENT_TYPE_CODE = listSub[0].PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = listSub[0].PATIENT_TYPE_NAME;
                            foreach (var data in listSub)
                            {
                                rdo.VIR_TOTAL_HEIN_PRICE += data.VIR_TOTAL_HEIN_PRICE;
                                rdo.VIR_TOTAL_PATIENT_PRICE += data.VIR_TOTAL_PATIENT_PRICE;
                                rdo.SERVICE_AMOUNT += data.SERVICE_AMOUNT;
                            }
                            result.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<Mrs00059RDO>();
            }
            return result;
        }

        private void LoadDataToRam()
        {
            try
            {


                if (castFilter.INPUT_DATA_ID_STT_TYPE == 1)
                {
                    HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                    filter.IN_TIME_FROM = castFilter.TIME_FROM;
                    filter.IN_TIME_TO = castFilter.TIME_TO;
                    filter.IS_PAUSE = false;
                    ListTreatment = new HisTreatmentManager().GetView(filter);
                }
                else if (castFilter.INPUT_DATA_ID_STT_TYPE == 2)
                {
                    HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                    filter.OUT_TIME_FROM = castFilter.TIME_FROM;
                    filter.OUT_TIME_TO = castFilter.TIME_TO;
                    filter.IS_PAUSE = true;
                    ListTreatment = new HisTreatmentManager().GetView(filter);
                }
                else
                {
                    HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                    filter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                    filter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    ListTreatment = new HisTreatmentManager().GetView(filter);
                }

                if (castFilter.TDL_TREATMENT_TYPE_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.TDL_TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (castFilter.TDL_PATIENT_TYPE_ID != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.TDL_PATIENT_TYPE_ID == o.TDL_PATIENT_TYPE_ID).ToList();
                }
                if (castFilter.LOGINNAMEs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME ?? "")).ToList();
                }
                if (castFilter.LAST_DEPARTMENT_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.LAST_DEPARTMENT_IDs.Contains(o.LAST_DEPARTMENT_ID ?? 0)).ToList();
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
                    dicSingleTag.Add("MODIFY_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("MODIFY_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Bill", ListBill.OrderBy(p => p.FEE_LOCK_DATE).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
