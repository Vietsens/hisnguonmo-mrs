using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisSereServDeposit;
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

namespace MRS.Processor.Mrs00012
{
    public class Mrs00012Processor : AbstractProcessor
    {
        Mrs00012Filter castFilter = null;
        List<PatientTypeRDO> ListPatientTypeRdo = new List<PatientTypeRDO>();
        List<DepartmentRDO> ListDepartmentRdo = new List<DepartmentRDO>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<BillRDO> ListBill = new List<BillRDO>();
        public Mrs00012Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00012Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00012Filter)this.reportFilter);
                LoadDataToRam();
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

                        ListPatientTypeRdo = ProcessPatientTypeRdo();

                        ListDepartmentRdo = ProcessDepartmentRdo();

                        ListBill = ProcessBillRdo();

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
                ListPatientTypeRdo.Clear();
            }
        }

        private List<BillRDO> ProcessBillRdo()
        {
            if (castFilter.PAY_FORM_IDs != null)
            {
                ListBill = ListBill.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID ?? 0)).ToList();
            }
            if (castFilter.ACCOUNT_BOOK_IDs != null)
            {
                ListBill = ListBill.Where(o => castFilter.ACCOUNT_BOOK_IDs.Contains(o.ACCOUNT_BOOK_ID ?? 0)).ToList();
            }
            return ListBill;
        }

        private List<PatientTypeRDO> ProcessPatientTypeRdo()
        {
            List<PatientTypeRDO> result = new List<PatientTypeRDO>();
            try
            {
                if (ListPatientTypeRdo.Count > 0)
                {
                    var Groups = ListPatientTypeRdo.GroupBy(g => g.PATIENT_TYPE_CODE).ToList();
                    foreach (var group in Groups)
                    {
                        List<PatientTypeRDO> listSub = group.ToList<PatientTypeRDO>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            PatientTypeRDO rdo = new PatientTypeRDO();
                            rdo.PATIENT_TYPE_CODE = listSub[0].PATIENT_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = listSub[0].PATIENT_TYPE_NAME;
                            foreach (var data in listSub)
                            {
                                rdo.TOTAL_HEIN_PRICE += data.TOTAL_HEIN_PRICE;
                                rdo.TOTAL_PATIENT_PRICE += data.TOTAL_PATIENT_PRICE;
                            }
                            result.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<DepartmentRDO> ProcessDepartmentRdo()
        {
            List<DepartmentRDO> result = new List<DepartmentRDO>();
            try
            {
                if (ListDepartmentRdo.Count > 0)
                {
                    var Groups = ListDepartmentRdo.GroupBy(g => g.DEPARTMENT_CODE).ToList();
                    foreach (var group in Groups)
                    {
                        List<DepartmentRDO> listSub = group.ToList<DepartmentRDO>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            DepartmentRDO rdo = new DepartmentRDO();
                            rdo.DEPARTMENT_CODE = listSub[0].DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = listSub[0].DEPARTMENT_NAME;
                            foreach (var data in listSub)
                            {
                                rdo.AMOUNT += data.AMOUNT;
                            }
                            result.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDetailListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> Treatments)
        {
            try
            {
                //chi tiet dich vu
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSereServ = new HisSereServManager(paramGet).GetView(filter);
                //chi tiet thanh toan
                HisSereServBillFilterQuery filterssBill = new HisSereServBillFilterQuery();
                filterssBill.TDL_TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListSereServBill = new HisSereServBillManager(paramGet).Get(filterssBill);

                //chi tiet giao dich
                HisTransactionViewFilterQuery filterTransaction = new HisTransactionViewFilterQuery();
                filterTransaction.TREATMENT_IDs = Treatments.Select(s => s.ID).ToList();
                var ListTransaction = new HisTransactionManager(paramGet).GetView(filterTransaction);
                ListTransaction = ListTransaction.Where(o => o.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var bill = ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                
                //loc bot benh nhan theo cac dieu kien loc giao dich
                if (castFilter.ACCOUNT_BOOK_IDs != null)
                {
                    var billFilter = bill.Where(o => castFilter.ACCOUNT_BOOK_IDs.Contains(o.ACCOUNT_BOOK_ID)).ToList();
                    var treatmentIdFilter = billFilter.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    bill = bill.Where(o => treatmentIdFilter.Contains(o.TREATMENT_ID)).ToList();
                    ListSereServBill = ListSereServBill.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID)).ToList();
                    ListSereServ = ListSereServ.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                }
                if (castFilter.PAY_FORM_IDs != null)
                {
                    var billFilter = bill.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                    var treatmentIdFilter = billFilter.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    bill = bill.Where(o => treatmentIdFilter.Contains(o.TREATMENT_ID)).ToList();
                    ListSereServBill = ListSereServBill.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID)).ToList();
                    ListSereServ = ListSereServ.Where(o => treatmentIdFilter.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                }
                //end loc bot benh nhan theo cac dieu kien loc giao dich

                //loc giao dich thu truc tiep
                var listBillDirect = bill.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_DIRECTLY_BILLING == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var listBillDirectIds = listBillDirect.Select(o => o.ID).ToList();
                var listSereServBillDirect = ListSereServBill.Where(o => listBillDirectIds.Contains(o.BILL_ID)).ToList();
                var listSereServBillDirectIds = listSereServBillDirect.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServ = ListSereServ.Where(o => !listSereServBillDirectIds.Contains(o.ID)).ToList();
                var ssbDirectIds = listSereServBillDirect.Select(o => o.ID).ToList();
                ListSereServBill = ListSereServBill.Where(o => !ssbDirectIds.Contains(o.ID)).ToList();
                bill = bill.Where(o => o.IS_DIRECTLY_BILLING != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                //end loc giao dich thu truc tiep

                //loc chi lay sereServ co thanh toan hoac khong bi xoa
                var billIds = bill.Select(o => o.ID).ToList();
                ListSereServBill = ListSereServBill.Where(o => billIds.Contains(o.BILL_ID)).ToList();
                var ssbIds = ListSereServBill.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServ = ListSereServ.Where(o => ssbIds.Contains(o.ID) || o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.SERVICE_REQ_ID != null).ToList();
                //end loc chi lay sereServ co thanh toan hoac khong bi xoa

                //loc lai giao dich theo sereServ
                var ssIds = ListSereServ.Select(o => o.ID).ToList();
                ListSereServBill = ListSereServBill.Where(o => ssIds.Contains(o.SERE_SERV_ID)).ToList();
                var biIds = ListSereServBill.Select(o => o.BILL_ID).ToList();
                bill = bill.Where(o => biIds.Contains(o.ID)).ToList();
                //end loc lai giao dich theo sereServ

                //loc giao dich huy
                var listBillCancel = bill.Where(o => o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var billCancelIds = listBillCancel.Select(o => o.ID).ToList();
                var listSereServBillCancel = ListSereServBill.Where(o => billCancelIds.Contains(o.BILL_ID)).ToList();
                var ssbCancelIds = listSereServBillCancel.Select(o => o.SERE_SERV_ID).ToList();
                var listSereServNoCancel = ListSereServ.Where(o => !ssbCancelIds.Contains(o.ID)).ToList();
                //end loc giao dich huy

                //tao danh sach chi phi theo doi tuong thanh toan
                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                long patientTypeIdVp = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                long patientTypeIdYC = HisPatientTypeCFG.PATIENT_TYPE_ID__DV;
                long patientTypeIdDvyc = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => o.PATIENT_TYPE_CODE == "07").Select(p => p.ID).FirstOrDefault();
                if (listSereServNoCancel != null && listSereServNoCancel.Count > 0)
                {
                    var Groups = listSereServNoCancel.GroupBy(g => g.PATIENT_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            PatientTypeRDO rdo = new PatientTypeRDO();
                            rdo.PATIENT_TYPE_CODE = listSub[0].PATIENT_TYPE_CODE;
                            if (group.Key == patientTypeIdBhyt)
                            {
                                rdo.PATIENT_TYPE_NAME = "Bệnh nhân trả";
                            }
                            else
                            {
                                rdo.PATIENT_TYPE_NAME = listSub[0].PATIENT_TYPE_NAME;
                            }
                            foreach (var sereServ in listSub)
                            {
                                rdo.TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            ListPatientTypeRdo.Add(rdo);
                        }
                    }
                }
                if (listSereServNoCancel != null && listSereServNoCancel.Count > 0)
                {
                    var Groups = listSereServNoCancel.GroupBy(g => g.TDL_REQUEST_DEPARTMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            DepartmentRDO rdo = new DepartmentRDO();
                            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == group.Key);
                            if (department != null)
                            {
                                rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;

                                rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                            }
                            foreach (var sereServ in listSub)
                            {
                                rdo.AMOUNT += (sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0) + (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                            }
                            ListDepartmentRdo.Add(rdo);
                        }
                    }
                }
                //tao danh sach chi phi tung hoa don thanh toan 
                foreach (var item in Treatments)
                {
                    var billSub = bill.Where(o => o.TREATMENT_ID == item.ID).ToList();
                    var lastTreaBillSub = billSub.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.ID).LastOrDefault();
                    var ssbSub = ListSereServBill.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList();
                    var ssSub = ListSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList();
                    var depositSub = ListTransaction.Where(o => o.TREATMENT_ID == item.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                    var repaySub = ListTransaction.Where(o => o.TREATMENT_ID == item.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                    var dicBill = new Dictionary<string, BillRDO>();
                    foreach (var ss in ssSub)
                    {
                        var ssbs = ssbSub.Where(o => o.SERE_SERV_ID == ss.ID).ToList();
                        var ssbids = ssbs.Select(o => o.BILL_ID).ToList();
                        var bills = billSub.Where(o => ssbids.Contains(o.ID)).ToList();

                        var billCancels = bills.Where(o => o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                        if (billCancels.Count > 0)
                        {
                            foreach (var bi in billCancels)
                            {
                                AddToDic(ref dicBill, item, bi, 1, ss, patientTypeIdBhyt, patientTypeIdVp, patientTypeIdYC, patientTypeIdDvyc);
                                AddToDic(ref dicBill, item, bi, -1, ss, patientTypeIdBhyt, patientTypeIdVp, patientTypeIdYC, patientTypeIdDvyc);
                            }

                        }
                        var lastBillSub = bills.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.ID).LastOrDefault() ?? lastTreaBillSub ?? new V_HIS_TRANSACTION();

                        AddToDic(ref dicBill, item, lastBillSub, 1, ss, patientTypeIdBhyt, patientTypeIdVp, patientTypeIdYC, patientTypeIdDvyc);
                    }
                    foreach (var bi in dicBill.Values)
                    {
                        bi.FEE_LOCK_TIME = item.FEE_LOCK_TIME ?? 0;
                        bi.OUT_TIME = item.OUT_TIME;
                        bi.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID;
                        bi.TREATMENT_CODE = item.TREATMENT_CODE;
                        bi.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                        bi.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        bi.DEPARTMENT_NAME = item.END_DEPARTMENT_NAME;
                        bi.PARENT_KEY = item.FEE_LOCK_LOGINNAME;
                        bi.LOGINNAME = item.FEE_LOCK_LOGINNAME;
                        bi.USERNAME = item.FEE_LOCK_USERNAME;
                        var bil = billSub.FirstOrDefault(o => o.ID == bi.TRANSACTION_ID);
                        if (bil != null)
                        {
                            bi.TRANSACTION_TIME = bil.TRANSACTION_TIME;
                            bi.ACCOUNT_BOOK_NAME = bil.ACCOUNT_BOOK_NAME;
                            bi.NUM_ORDER = bil.NUM_ORDER;
                            bi.EINVOICE_NUM_ORDER = bil.EINVOICE_NUM_ORDER;
                            bi.BANK_TRANSACTION_CODE = bil.BANK_TRANSACTION_CODE;
                            bi.BANK_TRANSACTION_TIME = bil.BANK_TRANSACTION_TIME;
                            bi.TRANSACTION_CODE = bil.TRANSACTION_CODE;
                            bi.KI_HIEU = bil.SYMBOL_CODE + " " + bil.TEMPLATE_CODE;
                            bi.AMOUNT = bil.AMOUNT * bi.SIGN;
                            bi.KC_AMOUNT = (bil.KC_AMOUNT ?? 0)* bi.SIGN;
                            bi.TDL_BILL_FUND_AMOUNT = (bil.TDL_BILL_FUND_AMOUNT ?? 0) * bi.SIGN;
                            bi.PAY_FORM_ID = bil.PAY_FORM_ID;
                            bi.PAY_FORM_NAME = bil.PAY_FORM_NAME;
                            bi.ACCOUNT_BOOK_ID = bil.ACCOUNT_BOOK_ID;
                        }
                        bi.HAOPHI = ssSub.Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.VIR_TOTAL_PRICE_NO_EXPEND ?? 0) * bi.SIGN;
                        bi.TAMUNG = depositSub.Sum(s => s.AMOUNT) * bi.SIGN;
                        bi.HOANUNG = repaySub.Sum(s => s.AMOUNT) * bi.SIGN;
                        bi.HOANUNGTRUOC = repaySub.Where(o => o.TRANSACTION_TIME < bi.TRANSACTION_TIME && (string.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME) || castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME)).Sum(s => s.AMOUNT) * bi.SIGN;
                        ListBill.Add(bi);
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddToDic(ref Dictionary<string, BillRDO> dicBill, V_HIS_TREATMENT item, V_HIS_TRANSACTION bi, short sign, V_HIS_SERE_SERV ss, long patientTypeIdBhyt, long patientTypeIdVp, long patientTypeIdYc, long patientTypeIdDvyc)
        {
            string Key = string.Format("{0}_{1}_{2}", item.ID, bi.ID, sign);
            if (!dicBill.ContainsKey(Key))
            {
                dicBill.Add(Key, new BillRDO());
                dicBill[Key].TRANSACTION_ID = bi.ID;
                dicBill[Key].SIGN = sign;
            }

            dicBill[Key].MIENGIAM += (ss.OTHER_SOURCE_PRICE ?? 0) * ss.AMOUNT * sign;
            dicBill[Key].BHTT += (ss.VIR_TOTAL_HEIN_PRICE ?? 0) * sign;
            dicBill[Key].VIENPHI += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * sign;
            if (ss.PATIENT_TYPE_ID == patientTypeIdBhyt)
            {
                dicBill[Key].AMOUNT_BH += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * sign;
            }
            if (ss.PATIENT_TYPE_ID == patientTypeIdVp)
            {
                dicBill[Key].AMOUNT_VP += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * sign;
            }
            if (ss.PATIENT_TYPE_ID == patientTypeIdYc)
            {
                dicBill[Key].AMOUNT_YC += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * sign;
            }
            if (ss.PATIENT_TYPE_ID == patientTypeIdDvyc)
            {
                dicBill[Key].AMOUNT_DVYC += (ss.VIR_TOTAL_PATIENT_PRICE ?? 0) * sign;
            }
        }

        private void LoadDataToRam()
        {
            try
            {


                HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                filter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                filter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                ListTreatment = new HisTreatmentManager().GetView(filter);
                if (!string.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                {
                    ListTreatment = ListTreatment.Where(o => castFilter.CASHIER_LOGINNAME == o.FEE_LOCK_LOGINNAME).ToList();
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

                objectTag.AddObjectData(store, "Report0", ListPatientTypeRdo);
                objectTag.AddObjectData(store, "Report1", ListDepartmentRdo);
                objectTag.AddObjectData(store, "Report2", ListBill.OrderBy(p => p.FEE_LOCK_TIME).ToList());
                objectTag.AddObjectData(store, "Parent2", ListBill.GroupBy(o => o.PARENT_KEY).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "Father2", ListBill.GroupBy(o => o.PARENT_KEY).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Parent2", "Report2", "PARENT_KEY", "PARENT_KEY");
                objectTag.AddRelationship(store, "Father2", "Report2", "PARENT_KEY", "PARENT_KEY");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
