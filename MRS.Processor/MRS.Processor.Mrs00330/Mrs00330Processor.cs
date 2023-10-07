using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisService;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTransactionType;
using MOS.MANAGER.HisHeinServiceType;

namespace MRS.Processor.Mrs00330
{
    class Mrs00330Processor : AbstractProcessor
    {
        Mrs00330Filter castFilter = null;
        List<Mrs00330RDO> listRdo = new List<Mrs00330RDO>();
        List<Mrs00330RDO> listRdoBill = new List<Mrs00330RDO>();
        List<Mrs00330RDO> listRdoDepoRepay = new List<Mrs00330RDO>();
        List<Mrs00330RDO> listCashier = new List<Mrs00330RDO>();
        List<Mrs00330RDO> listCashierBill = new List<Mrs00330RDO>();
        List<Mrs00330RDO> listCashierDepoRepay = new List<Mrs00330RDO>();
        List<HIS_CASHIER_ROOM> ListCashierRoom = new List<HIS_CASHIER_ROOM>();
        List<HIS_PAY_FORM> ListPayForm = new List<HIS_PAY_FORM>();
        List<HIS_ACCOUNT_BOOK> ListAccountBook = new List<HIS_ACCOUNT_BOOK>();
        decimal Total = 0;
        decimal TotalEx = 0;
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<D_HIS_SERE_SERV_BILL> ListSereServBill = new List<D_HIS_SERE_SERV_BILL>();
        Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>>();
        Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();
        Dictionary<long, List<D_HIS_SERE_SERV_BILL>> dicBillDetail = new Dictionary<long, List<D_HIS_SERE_SERV_BILL>>();
        List<string> DataFromTemplate = new List<string>();

        Dictionary<long, HIS_SERVICE> dicParentService = new Dictionary<long, HIS_SERVICE>();

        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCategoryService = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        List<HIS_TRANSACTION_TYPE> ListTransactionType = new List<HIS_TRANSACTION_TYPE>();
        List<HIS_HEIN_SERVICE_TYPE> listHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();

        const string BILL = "BILL";
        const string BNTT = "BNTT";
        const string PARENT = "PARENT";
        const string CATEGORY = "CATEGORY";
        string TypePrice = BILL;
        string TypeParent = PARENT;
        string KeyGroupDetail = "";


        public Mrs00330Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00330Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {

                if (this.dicDataFilter.ContainsKey("KEY_TYPE_PRICE") && this.dicDataFilter["KEY_TYPE_PRICE"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_TYPE_PRICE"].ToString()))
                {
                    TypePrice = this.dicDataFilter["KEY_TYPE_PRICE"].ToString();
                }
                if (this.dicDataFilter.ContainsKey("KEY_TYPE_PAR") && this.dicDataFilter["KEY_TYPE_PAR"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_TYPE_PAR"].ToString()))
                {
                    TypeParent = this.dicDataFilter["KEY_TYPE_PAR"].ToString();
                }

                //khi có điều kiện lọc từ template thì đổi sang key từ template
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_DETAIL") && this.dicDataFilter["KEY_GROUP_DETAIL"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_DETAIL"].ToString()))
                {
                    KeyGroupDetail = this.dicDataFilter["KEY_GROUP_DETAIL"].ToString();
                }
                this.DataFromTemplate = MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.GetBySheetIndex(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 1, 20, 10, 1);
                this.castFilter = (Mrs00330Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_MEDI_STOCK, MRS00330 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                ListCashierRoom = new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery());
                ListPayForm = HisPayFormCFG.ListPayForm;
                ListAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery());

                HisTransactionFilterQuery transFilter = new HisTransactionFilterQuery();
                transFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                if (castFilter.IS_ADD_BILL_CANCEL != true)
                {
                    transFilter.IS_CANCEL = false;
                }
                if (castFilter.ADD_OTHER_SALE_TYPE != true)
                {
                    transFilter.HAS_SALL_TYPE = false;
                }
                transFilter.PAY_FORM_ID = castFilter.PAY_FORM_ID;
                transFilter.PAY_FORM_IDs = castFilter.PAY_FORM_IDs;
                transFilter.CASHIER_ROOM_ID = castFilter.EXACT_CASHIER_ROOM_ID;
                ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(transFilter);
                
                if (!String.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                {
                    ListTransaction = ListTransaction.Where(o => o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                }
                if (!String.IsNullOrWhiteSpace(castFilter.LOGINNAME))
                {
                    ListTransaction = ListTransaction.Where(o => o.CASHIER_LOGINNAME == castFilter.LOGINNAME).ToList();
                }

                if (castFilter.IS_BILL_NORMAL.HasValue)
                {
                    if (castFilter.IS_BILL_NORMAL.Value)
                    {
                        ListTransaction = ListTransaction.Where(o => o.BILL_TYPE_ID == null).ToList();
                    }
                    else if (!castFilter.IS_BILL_NORMAL.Value)
                    {
                        ListTransaction = ListTransaction.Where(o => o.BILL_TYPE_ID == 1).ToList();
                    }
                }

                var listTreatmentId = ListTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var ListPatientTypeAlter = new HisPatientTypeAlterManager().GetByTreatmentIds(listTreatmentId);
                    dicPatientTypeAlter = ListPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());

                    if (castFilter.PATIENT_TYPE_ID != null)
                    {
                        var treatmentIds = dicPatientTypeAlter.Keys;
                        ListTransaction = ListTransaction.Where(o => treatmentIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                    }
                    //hồ sơ điều trị

                    var ListTreatment = new HisTreatmentManager().GetByIds(listTreatmentId);
                    dicTreatment = ListTreatment.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());
                }


                if (castFilter.TRAN_TREATMENT_TYPE_IDs != null)
                {
                    ListTransaction = ListTransaction.Where(o => dicPatientTypeAlter.ContainsKey(o.TREATMENT_ID ?? 0) && IsTreatmentTypeIn(castFilter.TRAN_TREATMENT_TYPE_IDs, o, dicPatientTypeAlter[o.TREATMENT_ID ?? 0])).ToList();
                }

                if (paramGet.HasException)
                {
                    LogSystem.Debug("Co exception tai DAOGET trong qua trinh tong hop du lieu HisBill.");
                }
                GetDicParentService();
                GetDicCategoryService();
                GetTransactionType();
                GetHeinServiceType();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetHeinServiceType()
        {
            listHeinServiceType = new HisHeinServiceTypeManager().Get(new HisHeinServiceTypeFilterQuery());
        }

        private void GetTransactionType()
        {
            try
            {
                ListTransactionType = new HisTransactionTypeManager().Get(new HisTransactionTypeFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }
        }

        private bool IsTreatmentTypeIn(List<long> TreatmentTypeIdsFilter, HIS_TRANSACTION transaction, List<HIS_PATIENT_TYPE_ALTER> patientTypeAlterSub)
        {
            var treatmentTypeId = (patientTypeAlterSub.OrderBy(p => p.ID).ThenBy(q => q.LOG_TIME).LastOrDefault(o => o.LOG_TIME <= transaction.TRANSACTION_TIME) ?? patientTypeAlterSub.OrderBy(p => p.ID).ThenBy(q => q.LOG_TIME).FirstOrDefault(o => o.LOG_TIME > transaction.TRANSACTION_TIME) ?? new HIS_PATIENT_TYPE_ALTER()).TREATMENT_TYPE_ID;
            return TreatmentTypeIdsFilter.Contains(treatmentTypeId);
        }

        private void GetDicParentService()
        {
            try
            {
                var services = new HisServiceManager().Get(new HisServiceFilterQuery());
                dicParentService = services.ToDictionary(p => p.ID, q => services.FirstOrDefault(o => o.ID == q.PARENT_ID));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetDicCategoryService()
        {
            try
            {
                var serviceRetyCats = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery() { REPORT_TYPE_CODE__EXACT = "MRS00330" });
                dicCategoryService = serviceRetyCats.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTransaction))
                {
                    //DV - thanh toan
                    ListSereServBill = new ManagerSql().GetSSB(castFilter);

                    ProcessSsBillDetail();
                    dicBillDetail = ListSereServBill.GroupBy(o => o.BILL_ID).ToDictionary(p => p.Key, q => q.ToList());
                    ProcessBill(ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).ToList());
                    ProcessOtherSale(ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.SALE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).ToList());
                    ProcessDeposit(ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList());
                    ProcessRepay(ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList());
                }
                grouptrea();
                createTotal();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessRepay(List<HIS_TRANSACTION> listRepay)
        {
            try
            {
                if (IsNotNullOrEmpty(listRepay))
                {
                    foreach (var item in listRepay)
                    {
                        if ((castFilter.TREATMENT_TYPE_IDs != null && castFilter.TREATMENT_TYPE_IDs.Count > 0) &&
                            ((!castFilter.TREATMENT_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                            && (castFilter.IS_TREATMENT_OUT_NOT_DEPOSIT ?? false) == true)) continue;
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && castFilter.IS_ADD_BILL_CANCEL != true)
                        {
                            continue;
                        }
                        Mrs00330RDO rdo = new Mrs00330RDO(item, ListCashierRoom, ListPayForm, ListAccountBook);
                        rdo.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME);
                        var transactionType = ListTransactionType.FirstOrDefault(o => o.ID == item.TRANSACTION_TYPE_ID);
                        if (transactionType != null)
                        {
                            rdo.TRANSACTION_TYPE_CODE = transactionType.TRANSACTION_TYPE_CODE;
                            rdo.TRANSACTION_TYPE_NAME = transactionType.TRANSACTION_TYPE_NAME;
                        }

                        var patientAlter = dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0) ? dicPatientTypeAlter[item.TREATMENT_ID ?? 0] : null;
                        if (patientAlter != null && patientAlter.Count > 0)
                        {
                            var currentPatient = patientAlter.OrderByDescending(p => p.ID).ThenByDescending(q => q.LOG_TIME).FirstOrDefault(o => o.LOG_TIME < item.TRANSACTION_TIME);
                            if (currentPatient != null)
                            {
                                rdo.HEIN_CARD_NUMBER = currentPatient.HEIN_CARD_NUMBER;
                            }

                        }

                        if (dicTreatment.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            var endDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (dicTreatment[item.TREATMENT_ID ?? 0] ?? new HIS_TREATMENT()).LAST_DEPARTMENT_ID);
                            if (endDepartment != null)
                            {
                                rdo.END_DEPARTMENT_NAME = endDepartment.DEPARTMENT_NAME;
                            }
                        }
                        listRdoDepoRepay.Add(rdo);
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessOtherSale(List<HIS_TRANSACTION> listOtherSale)
        {
            try
            {
                if (castFilter.ADD_OTHER_SALE_TYPE == true)
                {
                    if (IsNotNullOrEmpty(listOtherSale))
                    {
                        foreach (var item in listOtherSale)
                        {
                            if ((castFilter.TREATMENT_TYPE_IDs != null && castFilter.TREATMENT_TYPE_IDs.Count > 0) &&
                                ((!castFilter.TREATMENT_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                                && (castFilter.IS_TREATMENT_OUT_NOT_DEPOSIT ?? false) == true)) continue;
                            if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && castFilter.IS_ADD_BILL_CANCEL != true)
                            {
                                continue;
                            }

                            Mrs00330RDO rdo = new Mrs00330RDO(item, ListCashierRoom, ListPayForm, ListAccountBook);

                            rdo.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME);
                            var transactionType = ListTransactionType.FirstOrDefault(o => o.ID == item.TRANSACTION_TYPE_ID);
                            if (transactionType != null)
                            {
                                rdo.TRANSACTION_TYPE_CODE = transactionType.TRANSACTION_TYPE_CODE;
                                rdo.TRANSACTION_TYPE_NAME = transactionType.TRANSACTION_TYPE_NAME;
                            }

                            var patientAlter = dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0) ? dicPatientTypeAlter[item.TREATMENT_ID ?? 0] : null;
                            if (patientAlter != null && patientAlter.Count > 0)
                            {
                                var currentPatient = patientAlter.OrderByDescending(p => p.ID).ThenByDescending(q => q.LOG_TIME).FirstOrDefault(o => o.LOG_TIME < item.TRANSACTION_TIME);
                                if (currentPatient != null)
                                {
                                    rdo.HEIN_CARD_NUMBER = currentPatient.HEIN_CARD_NUMBER;
                                }

                            }

                            if (dicTreatment.ContainsKey(item.TREATMENT_ID ?? 0))
                            {
                                var endDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (dicTreatment[item.TREATMENT_ID ?? 0] ?? new HIS_TREATMENT()).LAST_DEPARTMENT_ID);
                                if (endDepartment != null)
                                {
                                    rdo.END_DEPARTMENT_NAME = endDepartment.DEPARTMENT_NAME;
                                }
                            }

                            rdo.TOTAL_BILL_PRICE = item.AMOUNT;

                            listRdoBill.Add(rdo);
                            listRdo.Add(rdo);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDeposit(List<HIS_TRANSACTION> listDeposit)
        {
            try
            {
                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var item in listDeposit)
                    {
                        if ((castFilter.TREATMENT_TYPE_IDs != null && castFilter.TREATMENT_TYPE_IDs.Count > 0) &&
                            ((!castFilter.TREATMENT_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                            && (castFilter.IS_TREATMENT_OUT_NOT_DEPOSIT ?? false) == true)) continue;
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && castFilter.IS_ADD_BILL_CANCEL != true)
                        {
                            continue;
                        }

                        Mrs00330RDO rdo = new Mrs00330RDO(item, ListCashierRoom, ListPayForm, ListAccountBook);

                        rdo.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME);
                        var transactionType = ListTransactionType.FirstOrDefault(o => o.ID == item.TRANSACTION_TYPE_ID);
                        if (transactionType != null)
                        {
                            rdo.TRANSACTION_TYPE_CODE = transactionType.TRANSACTION_TYPE_CODE;
                            rdo.TRANSACTION_TYPE_NAME = transactionType.TRANSACTION_TYPE_NAME;
                        }

                        var patientAlter = dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0) ? dicPatientTypeAlter[item.TREATMENT_ID ?? 0] : null;
                        if (patientAlter != null && patientAlter.Count > 0)
                        {
                            var currentPatient = patientAlter.OrderByDescending(p => p.ID).ThenByDescending(q => q.LOG_TIME).FirstOrDefault(o => o.LOG_TIME < item.TRANSACTION_TIME);
                            if (currentPatient != null)
                            {
                                rdo.HEIN_CARD_NUMBER = currentPatient.HEIN_CARD_NUMBER;
                            }

                        }

                        if (dicTreatment.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            var endDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (dicTreatment[item.TREATMENT_ID ?? 0] ?? new HIS_TREATMENT()).LAST_DEPARTMENT_ID);
                            if (endDepartment != null)
                            {
                                rdo.END_DEPARTMENT_NAME = endDepartment.DEPARTMENT_NAME;
                            }
                        }
                        listRdoDepoRepay.Add(rdo);
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessBill(List<HIS_TRANSACTION> listBill)
        {
            try
            {
                if (IsNotNullOrEmpty(listBill))
                {
                    foreach (var item in listBill)
                    {
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && castFilter.IS_ADD_BILL_CANCEL != true)
                        {
                            continue;
                        }
                        if (!dicBillDetail.ContainsKey(item.ID)) continue;
                        if (!item.TREATMENT_ID.HasValue) continue; //xem lại chỗ này sau
                        //sap xep cac lan chuyen doi tuong cua ho so dieu tri giam dan theo thoi gian va id
                        var patientTypeAlter = dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0) ? dicPatientTypeAlter[item.TREATMENT_ID ?? 0] : null;
                        if (patientTypeAlter == null || patientTypeAlter.Count == 0)
                        {
                            continue;
                        }
                        patientTypeAlter = patientTypeAlter.OrderByDescending(p => p.ID).ThenByDescending(q => q.LOG_TIME).ToList();

                        if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))//neu co loc theo dien dieu tri
                        {
                            HIS_PATIENT_TYPE_ALTER patType = null;
                            HIS_PATIENT_TYPE_ALTER patTypeIn = null;
                            for (int i = 0; i < patientTypeAlter.Count; i++)
                            {
                                if (patTypeIn == null && patientTypeAlter[i].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    //neu benh nhan co chuyen sang dieu tri noi tru thi lay lan chuyen doi sang noi tru cuoi cung
                                    patTypeIn = patientTypeAlter[i];
                                }

                                if (patType == null && castFilter.TREATMENT_TYPE_IDs.Contains(patientTypeAlter[i].TREATMENT_TYPE_ID))
                                {
                                    //lay dien dieu tri lon nhat cua benh nhan co trong danh sach dieu kien loc
                                    patType = patientTypeAlter[i];
                                }
                            }
                            //neu benh nhan khong co dien dieu tri nao trong danh sach dieu kien loc thi khong lay vao bao cao
                            if (patType == null) continue;
                            //neu dien dieu tri lon nhat co trong dieu kien loc la kham va dien dieu tri lon nhat cua benh nhan la noi tru va giao dich nay sau khi vao noi tru thi khong lay vao bao cao. (bo cac giao dich sau khi vao noi tru neu danh sach loc chi loc theo kham)
                            if (patType.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && patTypeIn != null && patTypeIn.LOG_TIME < item.TRANSACTION_TIME) continue;
                            //neu giao dich truoc khi chuyen vao dien dieu tri lon nhat co trong dieu kien loc thi bo qua
                            //(dieu kien nay khong hop ly, vi neu nguoi dung loc de lay ca noi tru va ngoai tru thi chi moi lay duoc cac giao dich noi tru)
                            if (patType.LOG_TIME > item.TRANSACTION_TIME) continue;//patType.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && castFilter.TREATMENT_TYPE_IDs == new List<long>(){IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM,IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU} && patType.LOG_TIME > item.TRANSACTION_TIME must be not continued;
                        }
                        if (dicBillDetail[item.ID].Count == 0)
                        {
                            dicBillDetail[item.ID].Add(new D_HIS_SERE_SERV_BILL());
                        }
                        var group = dicBillDetail[item.ID].GroupBy(g => string.Format(KeyGroupDetail, g.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                        foreach (var ssbs in group)
                        {
                            Mrs00330RDO rdo = new Mrs00330RDO(item, ListCashierRoom, ListPayForm, ListAccountBook);
                            rdo.REQUEST_DEPARTMENT_CODE = ssbs.First().TDL_REQUEST_DEPARTMENT_CODE;
                            rdo.REQUEST_DEPARTMENT_NAME = ssbs.First().TDL_REQUEST_DEPARTMENT_NAME;
                            rdo.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME);
                            var transactionType = ListTransactionType.FirstOrDefault(o => o.ID == item.TRANSACTION_TYPE_ID);
                            if (transactionType != null)
                            {
                                rdo.TRANSACTION_TYPE_CODE = transactionType.TRANSACTION_TYPE_CODE;
                                rdo.TRANSACTION_TYPE_NAME = transactionType.TRANSACTION_TYPE_NAME;
                            }

                            if (patientTypeAlter[0].HEIN_CARD_NUMBER != null)
                            {
                                rdo.HEIN_CARD_NUMBER = patientTypeAlter[0].HEIN_CARD_NUMBER;
                            }
                            if (this.TypePrice == BILL)
                            {

                                //tach tien thanh toan theo loai dich vu
                                rdo.DIC_SVT_PRICE = ssbs.GroupBy(g => g.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.BILL_PRICE));
                                //tach tien thanh toan theo loai dich vu bh
                                rdo.DIC_HSVT_PRICE = ssbs.GroupBy(g => g.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.BILL_PRICE));
                                //tach tien thanh toan theo nhom cha
                                rdo.DIC_PAR_PRICE = ssbs.GroupBy(g => (g.PARENT_SERVICE_CODE ?? "NONE")).ToDictionary(p => p.Key, q => q.Sum(s => s.BILL_PRICE));
                                //chú thích các khoản tiền nhom cha
                                rdo.NOTE_PAR_PRICE = string.Join(";", ssbs.GroupBy(g => g.PARENT_SERVICE_CODE ?? "NONE").Select(o => string.Format("{0}:{1}", o.First().PARENT_SERVICE_NAME, o.Sum(s => s.BILL_PRICE))).ToList());
                            }
                            else if (this.TypePrice == BNTT)
                            {

                                //tach tien thanh toan theo loai dich vu
                                rdo.DIC_SVT_PRICE = ssbs.GroupBy(g => g.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => (s.VIR_TOTAL_PRICE ?? 0) - (s.VIR_TOTAL_HEIN_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                                //tach tien thanh toan theo loai dich vu bh
                                rdo.DIC_HSVT_PRICE = ssbs.GroupBy(g => g.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => (s.VIR_TOTAL_PRICE ?? 0) - (s.VIR_TOTAL_HEIN_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                                //tach tien bệnh nhân tự trả theo nhom cha
                                rdo.DIC_PAR_PRICE = ssbs.GroupBy(g => (g.PARENT_SERVICE_CODE ?? "NONE")).ToDictionary(p => p.Key, q => q.Sum(s => (s.VIR_TOTAL_PRICE ?? 0) - (s.VIR_TOTAL_HEIN_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                                //chú thích các khoản tiền bệnh nhân tự trả theo nhom cha
                                rdo.NOTE_PAR_PRICE = string.Join(";", ssbs.GroupBy(g => g.PARENT_SERVICE_CODE ?? "NONE").Select(o => string.Format("{0}:{1}", o.First().PARENT_SERVICE_NAME, o.Sum(s => (s.VIR_TOTAL_PRICE ?? 0) - (s.VIR_TOTAL_HEIN_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)))).ToList());
                            }
                            //them thong tin khoa phong ket thuc
                            var endRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == (ssbs.FirstOrDefault() ?? new D_HIS_SERE_SERV_BILL()).END_ROOM_ID);
                            if (endRoom != null)
                            {
                                rdo.END_ROOM_NAME = endRoom.ROOM_NAME;
                            }
                            if (dicTreatment.ContainsKey(item.TREATMENT_ID ?? 0))
                            {
                                var endDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (dicTreatment[item.TREATMENT_ID ?? 0] ?? new HIS_TREATMENT()).LAST_DEPARTMENT_ID);
                                if (endDepartment != null)
                                {
                                    rdo.END_DEPARTMENT_NAME = endDepartment.DEPARTMENT_NAME;
                                }
                            }
                            foreach (var ssb in ssbs)
                            {
                                if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                                {
                                    rdo.PRICE_BED += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                                {
                                    rdo.PRICE_BLOOD += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA)
                                {
                                    rdo.PRICE_DIIM += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                                {
                                    rdo.PRICE_ENDO += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                                {
                                    rdo.PRICE_EXAM += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN)
                                {
                                    rdo.PRICE_FUEX += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                                {
                                    rdo.PRICE_MISU += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC)
                                {
                                    rdo.PRICE_OTHER += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                                {
                                    rdo.PRICE_PRES += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                                {
                                    rdo.PRICE_SUIM += ssb.BILL_PRICE;
                                }
                                //else if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                                //{
                                //    rdo.PRICE_SURG += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                                //}
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                                {
                                    rdo.PRICE_TEST += ssb.BILL_PRICE;
                                }
                                else if (ssb.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL)
                                {
                                    rdo.PRICE_GPBL += ssb.BILL_PRICE;
                                }
                                else
                                {
                                    rdo.PRICE_OTHER += ssb.BILL_PRICE;
                                }
                                //rdo.EXEMPTION = sereServ.DISCOUNT ?? 0;

                                rdo.PRICE_BHTT += ssb.VIR_TOTAL_HEIN_PRICE ?? 0;
                                rdo.PRICE_BNCCT += ssb.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                                rdo.PRICE_BNTT += (ssb.VIR_TOTAL_PRICE ?? 0) - (ssb.VIR_TOTAL_HEIN_PRICE ?? 0) - (ssb.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                if (castFilter.ADD_OTHER_SALE_TYPE == true)
                                {
                                    rdo.TOTAL_BILL_PRICE = item.AMOUNT;
                                }
                                else
                                {
                                    rdo.TOTAL_BILL_PRICE += ssb.BILL_PRICE;
                                }
                            }
                            listRdo.Add(rdo);
                            listRdoBill.Add(rdo);
                            
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void createTotal()
        {
            try
            {
                if (listRdo != null && listRdo.Count > 0)
                {
                    var groups = listRdo.GroupBy(g => g.CASHIER_USERNAME).ToList();
                    foreach (var item in groups)
                    {
                        Mrs00330RDO rdo = new Mrs00330RDO();
                        rdo.CASHIER_USERNAME = item.First().CASHIER_USERNAME;
                        rdo.EXEMPTION = item.Sum(s => s.EXEMPTION);
                        rdo.PRICE_BED = item.Sum(s => s.PRICE_BED);
                        rdo.PRICE_BLOOD = item.Sum(s => s.PRICE_BLOOD);
                        rdo.PRICE_DIIM = item.Sum(s => s.PRICE_DIIM);
                        rdo.PRICE_ENDO = item.Sum(s => s.PRICE_ENDO);
                        rdo.PRICE_EXAM = item.Sum(s => s.PRICE_EXAM);
                        rdo.PRICE_FUEX = item.Sum(s => s.PRICE_FUEX);
                        rdo.PRICE_MISU = item.Sum(s => s.PRICE_MISU);
                        rdo.PRICE_OTHER = item.Sum(s => s.PRICE_OTHER);
                        rdo.PRICE_PRES = item.Sum(s => s.PRICE_PRES);
                        rdo.PRICE_SUIM = item.Sum(s => s.PRICE_SUIM);
                        rdo.PRICE_SURG = item.Sum(s => s.PRICE_SURG);
                        rdo.PRICE_TEST = item.Sum(s => s.PRICE_TEST);
                        rdo.PRICE_GPBL = item.Sum(s => s.PRICE_GPBL);
                        rdo.AMOUNT_DEPOSIT = item.Sum(s => s.AMOUNT_DEPOSIT);
                        rdo.AMOUNT_REPAY = item.Sum(s => s.AMOUNT_REPAY);
                        rdo.PRICE_BNCCT = item.Sum(s => s.PRICE_BNCCT);
                        rdo.PRICE_BHTT = item.Sum(s => s.PRICE_BHTT);
                        rdo.PRICE_BNTT = item.Sum(s => s.PRICE_BNTT);
                        rdo.TOTAL_BILL_PRICE = item.Sum(s => s.TOTAL_BILL_PRICE);
                        var t = rdo.PRICE_BED + rdo.PRICE_BLOOD + rdo.PRICE_DIIM + rdo.PRICE_ENDO + rdo.PRICE_EXAM + rdo.PRICE_FUEX + rdo.PRICE_MISU + rdo.PRICE_OTHER + rdo.PRICE_PRES + rdo.PRICE_SUIM + rdo.PRICE_SURG + rdo.PRICE_TEST + rdo.PRICE_GPBL;

                        Total += t;
                        TotalEx += rdo.EXEMPTION ?? 0;
                        if (t > 0) listCashierBill.Add(rdo);
                        if (rdo.AMOUNT_DEPOSIT > 0 || rdo.AMOUNT_REPAY > 0) listCashierDepoRepay.Add(rdo);
                        listCashier.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void grouptrea()
        {
            if (castFilter.IS_MERGE_TREATMENT == true)
            {
                try
                {
                    if (listRdo != null && listRdo.Count > 0)
                    {
                        var group = listRdo.GroupBy(g => g.TREATMENT_ID).ToList();
                        listRdo.Clear();
                        foreach (var item in group)
                        {
                            Mrs00330RDO rdo = new Mrs00330RDO();
                            rdo.VIR_PATIENT_NAME = item.First().VIR_PATIENT_NAME;
                            rdo.VIR_ADDRESS = item.First().VIR_ADDRESS;
                            rdo.CASHIER_USERNAME = item.First().CASHIER_USERNAME;
                            rdo.EXEMPTION = item.Sum(s => s.EXEMPTION);
                            rdo.PRICE_BED = item.Sum(s => s.PRICE_BED);
                            rdo.PRICE_BLOOD = item.Sum(s => s.PRICE_BLOOD);
                            rdo.PRICE_DIIM = item.Sum(s => s.PRICE_DIIM);
                            rdo.PRICE_ENDO = item.Sum(s => s.PRICE_ENDO);
                            rdo.PRICE_EXAM = item.Sum(s => s.PRICE_EXAM);
                            rdo.PRICE_FUEX = item.Sum(s => s.PRICE_FUEX);
                            rdo.PRICE_MISU = item.Sum(s => s.PRICE_MISU);
                            rdo.PRICE_OTHER = item.Sum(s => s.PRICE_OTHER);
                            rdo.PRICE_PRES = item.Sum(s => s.PRICE_PRES);
                            rdo.PRICE_SUIM = item.Sum(s => s.PRICE_SUIM);
                            rdo.PRICE_SURG = item.Sum(s => s.PRICE_SURG);
                            rdo.PRICE_TEST = item.Sum(s => s.PRICE_TEST);
                            rdo.PRICE_GPBL = item.Sum(s => s.PRICE_GPBL);
                            rdo.AMOUNT_DEPOSIT = item.Sum(s => s.AMOUNT_DEPOSIT);
                            rdo.AMOUNT_REPAY = item.Sum(s => s.AMOUNT_REPAY);
                            rdo.PRICE_BNCCT = item.Sum(s => s.PRICE_BNCCT);
                            rdo.PRICE_BHTT = item.Sum(s => s.PRICE_BHTT);
                            rdo.PRICE_BNTT = item.Sum(s => s.PRICE_BNTT);
                            rdo.TOTAL_BILL_PRICE = item.Sum(s => s.TOTAL_BILL_PRICE);
                            var t = rdo.PRICE_BED + rdo.PRICE_BLOOD + rdo.PRICE_DIIM + rdo.PRICE_ENDO + rdo.PRICE_EXAM + rdo.PRICE_FUEX + rdo.PRICE_MISU + rdo.PRICE_OTHER + rdo.PRICE_PRES + rdo.PRICE_SUIM + rdo.PRICE_SURG + rdo.PRICE_TEST + rdo.PRICE_GPBL;

                            //Total += t;
                            //TotalEx += rdo.EXEMPTION ?? 0;
                            if (t > 0) listCashierBill.Add(rdo);
                            if (rdo.AMOUNT_DEPOSIT > 0 || rdo.AMOUNT_REPAY > 0) listCashierDepoRepay.Add(rdo);
                            listRdo.Add(rdo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }

            }
        }

        private void ProcessSsBillDetail()
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    foreach (var item in ListSereServBill)
                    {
                        if (item.FEE_LOCK_TIME.HasValue)
                        {
                            item.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.FEE_LOCK_TIME.Value);
                        }

                        if (item.CANCEL_TIME.HasValue)
                        {
                            item.CANCEL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.CANCEL_TIME.Value);
                        }

                        if (item.BILL_TYPE_ID == 2)
                        {
                            item.TOTAL_DV_PRICE = item.BILL_PRICE;
                            item.DV_PRICE = item.VIR_PRICE ?? 0;
                            item.DV_NUM_ORDER = item.TRANSACTION_NUM_ORDER;
                        }
                        else
                        {
                            if (item.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                item.TOTAL_BHYT_PRICE = item.BILL_PRICE;
                                item.DV_PRICE = item.VIR_PRICE ?? 0;
                                item.BHYT_NUM_ORDER = item.TRANSACTION_NUM_ORDER;
                            }
                            else
                            {
                                item.TOTAL_VP_PRICE = item.BILL_PRICE;
                                item.DV_PRICE = item.VIR_PRICE ?? 0;
                                item.VP_NUM_ORDER = item.TRANSACTION_NUM_ORDER;
                            }
                        }

                        if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            item.DOCTOR_LOGINNAME = item.EXECUTE_LOGINNAME;
                            item.DOCTOR_USERNAME = item.EXECUTE_USERNAME;
                        }
                        else
                        {
                            item.DOCTOR_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                            item.DOCTOR_USERNAME = item.TDL_REQUEST_USERNAME;
                        }

                        var executeDepa = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT());
                        item.TDL_EXECUTE_DEPARTMENT_CODE = executeDepa.DEPARTMENT_CODE;
                        item.TDL_EXECUTE_DEPARTMENT_NAME = executeDepa.DEPARTMENT_NAME;
                        var requestDepa = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT());
                        item.TDL_REQUEST_DEPARTMENT_CODE = requestDepa.DEPARTMENT_CODE;
                        item.TDL_REQUEST_DEPARTMENT_NAME = requestDepa.DEPARTMENT_NAME;

                        var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.TDL_SERVICE_TYPE_ID);
                        if (serviceType != null)
                        {
                            //loai dich vu
                            item.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                            item.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                            //nếu không có dịch vụ cha thì lấy loại dịch vụ làm nhóm cha
                            item.PARENT_SERVICE_CODE = serviceType.SERVICE_TYPE_CODE;
                            item.PARENT_SERVICE_NAME = serviceType.SERVICE_TYPE_NAME;

                        }
                        var serviceTypeBhyt = listHeinServiceType.FirstOrDefault(o => o.ID == item.TDL_HEIN_SERVICE_TYPE_ID);
                        if (serviceTypeBhyt != null)
                        {
                            //loai dich vu bh
                            item.HEIN_SERVICE_TYPE_CODE = serviceTypeBhyt.HEIN_SERVICE_TYPE_CODE;
                            item.HEIN_SERVICE_TYPE_NAME = serviceTypeBhyt.HEIN_SERVICE_TYPE_NAME;
                           

                        }
                        if (this.TypeParent == PARENT && dicParentService.ContainsKey(item.SERVICE_ID) && dicParentService[item.SERVICE_ID] != null)
                        {
                            item.PARENT_SERVICE_CODE = "PAR_" + dicParentService[item.SERVICE_ID].SERVICE_CODE;
                            item.PARENT_SERVICE_NAME = dicParentService[item.SERVICE_ID].SERVICE_NAME;
                        }
                        if (this.TypeParent == CATEGORY && dicCategoryService.ContainsKey(item.SERVICE_ID) && dicCategoryService[item.SERVICE_ID] != null)
                        {
                            item.PARENT_SERVICE_CODE = "PAR_" + dicCategoryService[item.SERVICE_ID].CATEGORY_CODE;
                            item.PARENT_SERVICE_NAME = dicCategoryService[item.SERVICE_ID].CATEGORY_NAME;
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
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                if (castFilter.IS_BILL_NORMAL.HasValue)
                {
                    if (!castFilter.IS_BILL_NORMAL.Value)
                    {
                        dicSingleTag.Add("BILL_TYPE_NAME", "Hóa đơn Dịch vụ");
                    }
                    else if (castFilter.IS_BILL_NORMAL.Value)
                    {
                        dicSingleTag.Add("BILL_TYPE_NAME", "Hóa đơn Thường");
                    }
                }
                HisCashierRoomViewFilterQuery cashierRoomfilter = new HisCashierRoomViewFilterQuery();
                cashierRoomfilter.ID = this.castFilter.EXACT_CASHIER_ROOM_ID;
                var listCashierRooms = new HisCashierRoomManager(param).GetView(cashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { LOGINNAME = castFilter.CASHIER_LOGINNAME });
                dicSingleTag.Add("CASHIER_USERNAME", (acsUser.FirstOrDefault(o => o.LOGINNAME == castFilter.CASHIER_LOGINNAME) ?? new ACS_USER()).USERNAME);
                dicSingleTag.Add("CASHIER_ROOM_NAME", (listCashierRooms.FirstOrDefault(o => o.ID == castFilter.EXACT_CASHIER_ROOM_ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);

                dicSingleTag.Add("TOTAL_PRICE", Total);
                dicSingleTag.Add("TOTAL_EX_PRICE", TotalEx);

                listCashier = listCashier.OrderBy(o => o.CASHIER_USERNAME).ToList();
                listRdo = listRdo.OrderBy(o => o.ACCOUNT_BOOK_NAME).ThenBy(t => t.VIR_PATIENT_NAME).ToList();


                bool exportSuccess = true;

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Cashier", listCashier);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", listRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "CashierBill", listCashierBill);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TransactionBill", listRdoBill);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "CashierDepoRepay", listCashierDepoRepay);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TransactionDepoRepay", listRdoDepoRepay);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Cashier", "Transaction", "CASHIER_USERNAME", "CASHIER_USERNAME");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierDepoRepay", "TransactionDepoRepay", "CASHIER_USERNAME", "CASHIER_USERNAME");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierBill", "TransactionBill", "CASHIER_USERNAME", "CASHIER_USERNAME");
                objectTag.AddObjectData(store, "Child", listRdo.Where(q => q.TEMPLATE_CODE != null).ToList());
                objectTag.AddObjectData(store, "Parent", listRdo.Where(q => q.TEMPLATE_CODE != null).GroupBy(o => new { o.TREATMENT_ID, o.CASHIER_LOGINNAME }).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "GrandParent", listRdo.Where(q => q.TEMPLATE_CODE != null).GroupBy(o => o.CASHIER_LOGINNAME).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "GrandParent", "Parent", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
                objectTag.AddRelationship(store, "Parent", "Child", new string[] { "CASHIER_LOGINNAME", "TREATMENT_ID" }, new string[] { "CASHIER_LOGINNAME", "TREATMENT_ID" });

                objectTag.AddObjectData(store, "SereServBill", this.ListSereServBill);
                objectTag.AddObjectData(store, "SereServ", this.ListSereServBill.GroupBy(o => o.ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "ParentService", this.ListSereServBill.GroupBy(o => o.PARENT_SERVICE_ID).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "ParentService", "SereServBill", "PARENT_SERVICE_ID", "PARENT_SERVICE_ID");
                objectTag.AddObjectData(store, "SereServBillDetail", this.ListSereServBill);
                if (this.DataFromTemplate.Exists(o => !string.IsNullOrWhiteSpace(o) && o.Contains("<#Treatment.")))
                {
                    objectTag.AddObjectData(store, "Treatment", new ManagerSql().GetTrea(castFilter));
                }
                objectTag.AddObjectData(store, "Room", HisRoomCFG.HisRooms);
                objectTag.AddObjectData(store, "PatientTypeAlter", dicPatientTypeAlter.Values.ToList());
                if (this.DataFromTemplate.Exists(o => !string.IsNullOrWhiteSpace(o) && o.Contains("<#TransactionDetail.")))
                {
                    objectTag.AddObjectData(store, "TransactionDetail", new ManagerSql().GetTransactionDetail(castFilter));
                }
                if (this.DataFromTemplate.Exists(o => !string.IsNullOrWhiteSpace(o) && o.Contains("<#FeeByNbill.")))
                {
                    objectTag.AddObjectData(store, "FeeByNbill", new ManagerSql().GetFeeByNbill(castFilter));
                }

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
