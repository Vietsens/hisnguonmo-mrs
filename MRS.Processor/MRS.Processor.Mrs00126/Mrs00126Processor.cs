using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisOtherPaySource;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00126
{
    public class Mrs00126Processor : AbstractProcessor
    {
        List<Mrs00126RDO> _listSereServRdo = new List<Mrs00126RDO>();
        Mrs00126Filter CastFilter;
        List<HIS_TRANSACTION> listBill = new List<HIS_TRANSACTION>();
        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();
        PropertyInfo[] pRdo = null;
        PropertyInfo[] pOtherSourcePrice = null;
        decimal serviceTypeHein5 = -1;
        decimal serviceTypeHein20 = -1;
        decimal serviceTypeHein40 = -1;

        public Mrs00126Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00126Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                tryGetServiceTypeHein();

                pRdo = Properties.Get<Mrs00126RDO>();
                pOtherSourcePrice = pRdo.Where(o => o.Name.StartsWith("TOTAL_OTHER_SOURCE_PRICE_")).ToArray();
                HisOtherPaySourceFilterQuery otherPaySourceFilter = new HisOtherPaySourceFilterQuery();
                otherPaySourceFilter.ORDER_FIELD = "ID";
                otherPaySourceFilter.ORDER_DIRECTION = "DESC";
                this.listOtherPaySource = new HisOtherPaySourceManager().Get(otherPaySourceFilter) ?? new List<HIS_OTHER_PAY_SOURCE>();
                this.CastFilter = (Mrs00126Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                //-------------------------------------------------------------------------------------------------- V_HIS_TRANSACTION
                //lấy giao dịch trong khoảng thời gian chọn
                var billFilter = new HisTransactionFilterQuery
                {
                    TRANSACTION_TIME_FROM = CastFilter.DATE_FROM,
                    TRANSACTION_TIME_TO = CastFilter.DATE_TO,
                    TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT,
                    PAY_FORM_ID = CastFilter.PAY_FORM_ID,
                    IS_CANCEL = false,
                    HAS_SALL_TYPE = false
                };
                listBill = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(billFilter);

                //-------------------------------------------------------------------------------------------------- V_HIS_TRANSACTION
                //lấy toàn bộ giao dịch của mỗi hồ sơ điều trị trong khoảng thời gian chọn ở trên
                var listTreatmentViewIds = listBill.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();
                var skip = 0;
                while (listTreatmentViewIds.Count - skip > 0)
                {
                    List<HIS_TRANSACTION> listTransactionLocal = new List<HIS_TRANSACTION>();
                    List<HIS_SERE_SERV> listSereServLocal = new List<HIS_SERE_SERV>();
                    List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterLocal = new List<HIS_PATIENT_TYPE_ALTER>();

                    var listIds = listTreatmentViewIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();

                    List<HIS_TRANSACTION> listBillLocal = listBill.Where(o => listIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterTransaction2 = new HisTransactionFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        IS_CANCEL = false,
                        HAS_SALL_TYPE = false
                    };
                    listTransactionLocal = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(metyFilterTransaction2) ?? new List<HIS_TRANSACTION>();

                    var PatientTypeAlter = new HisPatientTypeAlterFilterQuery
                    {
                        TREATMENT_IDs = listIds
                    };
                    listPatientTypeAlterLocal = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).Get(PatientTypeAlter) ?? new List<HIS_PATIENT_TYPE_ALTER>();
                    listPatientTypeAlterLocal = listPatientTypeAlterLocal.GroupBy(o => o.TREATMENT_ID).Select(p => p.OrderBy(q => q.LOG_TIME).Last()).ToList();

                    var metyFilterSereServ = new HisSereServFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        HAS_EXECUTE = true,
                        PATIENT_TYPE_ID = this.CastFilter.SERE_SERV_PATIENT_TYPE_ID
                    };
                    listSereServLocal = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).Get(metyFilterSereServ) ?? new List<HIS_SERE_SERV>();

                    ProcessFilterData(listBillLocal, listTransactionLocal, listSereServLocal, listPatientTypeAlterLocal);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void tryGetServiceTypeHein()
        {
            try
            {
                this.serviceTypeHein5 = MRS.MANAGER.Config.HisServiceTypeCFG.SERVICE_TYPE_ID__HEIN_RATIO_5 ?? 0;
                this.serviceTypeHein20 = MRS.MANAGER.Config.HisServiceTypeCFG.SERVICE_TYPE_ID__HEIN_RATIO_20 ?? 0;
                this.serviceTypeHein40 = MRS.MANAGER.Config.HisServiceTypeCFG.SERVICE_TYPE_ID__HEIN_RATIO_40 ?? 0;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            var result = false;
            try
            {
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<HIS_TRANSACTION> listBillNow,
            List<HIS_TRANSACTION> listTransactionNow,
            List<HIS_SERE_SERV> listSereServNow,
            List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterNow)
        {
            if (this.CastFilter.PATIENT_TYPE_ID != null)
            {
                listBillNow = listBillNow.Where(o => listPatientTypeAlterNow.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PATIENT_TYPE_ID == this.CastFilter.PATIENT_TYPE_ID)).ToList();
            }

            if (this.CastFilter.IS_TREAT != null)
            {
                if (this.CastFilter.IS_TREAT == true)
                {
                    listBillNow = listBillNow.Where(o => listPatientTypeAlterNow.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && (p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY))).ToList();
                }
                else
                {
                    listBillNow = listBillNow.Where(o => listPatientTypeAlterNow.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && p.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && p.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)).ToList();
                }
            }

            if (this.ReportTemplateCode == "MRS0012602")
            {
                listBillNow = listBillNow.Where(o => listPatientTypeAlterNow.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && p.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && p.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)).ToList();
            }


            foreach (var bill in listBillNow.OrderBy(s => s.TRANSACTION_TIME))
            {
                var listSereServIds = new List<long>();
                List<HIS_SERE_SERV> sereServSub = listSereServNow.Where(s => s.TDL_TREATMENT_ID == bill.TREATMENT_ID).ToList();

                //thanh toán. Lấy toàn bộ giao dịch là thanh toán trừ lần giao dịch thanh toán hiện tại của hồ sơ điều trị
                var totalPay = listTransactionNow.Where(s => s.TREATMENT_ID == bill.TREATMENT_ID &&
                    s.ID != bill.ID && s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => s.AMOUNT);

                //hoàn ứng. Lấy toàn bộ giao dịch là hoàn ứng của hồ sơ điều trị
                var totalPriceRepay = listTransactionNow.Where(s => s.TREATMENT_ID == bill.TREATMENT_ID &&
                    s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT);

                //tạm ứng. Lấy toàn bộ giao dịch là tạm ứng của hồ sơ điều trị
                var totalPriceDeposit = listTransactionNow.Where(s => s.TREATMENT_ID == bill.TREATMENT_ID &&
                    s.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);

                //viện phí. Lấy toàn bộ viện phí của hồ sơ điều trị bao gồm tiền k đc hưởng BHYT và % số tiền phải trả khi đc hưởng BHYT
                var totalPriceBill = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //bảo hiểm. Lấy toàn bộ bảo hiểm của hồ sơ điều trị bao gồm tiền đc hưởng BHYT
                var totalPriceHein = sereServSub.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);

                //viện phí bảo hiểm. Lấy toàn bộ viện phí của hồ sơ điều trị bao gồm tiền  % số tiền phải trả khi đc hưởng BHYT
                var totalPriceBillHein = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                //viện phí k đc hưởng. Lấy toàn bộ viện phí của hồ sơ điều trị bao gồm tiền k đc hưởng BHYT
                var totalPriceBillDiff = sereServSub.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));

                //chi phí. Lấy toàn bộ của hồ sơ điều trị
                var totalPrice = sereServSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);

                //tiền thuốc
                var priceMedicine = sereServSub.Where(s => s.VIR_TOTAL_HEIN_PRICE == 0 &&
                    s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList();

                listSereServIds.AddRange(priceMedicine.Select(s => s.ID));

                var totalPriceMedicine = priceMedicine.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //tiền giường
                var priceBeds = sereServSub.Where(s => s.VIR_TOTAL_HEIN_PRICE == 0 &&
                    s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList();

                listSereServIds.AddRange(priceBeds.Select(s => s.ID));

                var totalPriceBeds = priceBeds.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //y cụ
                var priceMaterial = sereServSub.Where(s => s.VIR_TOTAL_HEIN_PRICE == 0 &&
                    s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();

                listSereServIds.AddRange(priceMaterial.Select(s => s.ID));

                var totalPriceMaterial = priceMaterial.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //xét nghiệm máu
                var priceTestBlood = sereServSub.Where(s => s.VIR_TOTAL_HEIN_PRICE == 0 &&
                    MRS.MANAGER.Config.HisServiceTypeCFG.SERVICE_TYPE_ID__TEST_BLOOD.Contains(s.TDL_SERVICE_TYPE_ID)).ToList();

                listSereServIds.AddRange(priceTestBlood.Select(s => s.ID));

                var totalPriceTestBlood = priceTestBlood.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //dịch vụ khác (nogià những dịch vụ trên)
                var serviceOther = sereServSub.Where(s => s.VIR_TOTAL_HEIN_PRICE == 0 &&
                    !listSereServIds.Contains(s.ID)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //BHYT 5%. bệnh nhân phải trả 5% BHYT
                var totalPriceBhyt5 = sereServSub.Where(s =>
                    s.HEIN_RATIO == serviceTypeHein5).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //BHYT 20%. bệnh nhân phải trả 20% BHYT
                var totalPriceBhyt20 = sereServSub.Where(s =>
                    s.HEIN_RATIO == serviceTypeHein20).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

                //BHYT 40%. bệnh nhân phải trả 40% BHYT
                var totalPriceBhyt40 = sereServSub.Where(s =>
                    s.HEIN_RATIO == serviceTypeHein40).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                //

                //tiền còn thừa
                decimal totalMoneyExcess = 0;
                //tiền còn thiếu
                decimal totalMoneyShortage = 0;
                //tính tiền thừa thiếu
                var totalMoney = totalPriceBill - (totalPriceDeposit + totalPay /*- totalPriceRepay*/);
                var @decimal = totalMoney >= 0 ? totalMoneyShortage = (decimal)totalMoney : totalMoneyExcess = (decimal)totalMoney;

                var rdo = new Mrs00126RDO
                {
                    TRANSACTION_CODE = bill.TRANSACTION_CODE,
                    CREATE_TIME = bill.CREATE_TIME != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(bill.CREATE_TIME.Value) : null,
                    VIR_PATIENT_NAME = bill.TDL_PATIENT_NAME,
                    TOTAL_PAY = totalPay,
                    TOTAL_PRICE_DEPOSIT = totalPriceDeposit,
                    TOTAL_PRICE_BILL = totalPriceBill,
                    TOTAL_PRICE_MEDICINE = totalPriceMedicine,
                    TOTAL_PRICE_BEDS = totalPriceBeds,
                    TOTAL_PRICE_MATERIAL = totalPriceMaterial,
                    TOTAL_PRICE_TEST_BLOOD = totalPriceTestBlood,
                    TOTAL_PRICE_SERVICE_OTHER = serviceOther,
                    TOTAL_PRICE_BHYT_5 = totalPriceBhyt5,
                    TOTAL_PRICE_BHYT_20 = totalPriceBhyt20,
                    TOTAL_PRICE_BHYT_40 = totalPriceBhyt40,
                    TOTAL_MONEY_EXCESS = totalMoneyExcess,
                    TOTAL_MONEY_SHORTAGE = totalMoneyShortage
                };
               
                //new info
                rdo.TRANSACTION_TIME = bill.TRANSACTION_TIME;
                rdo.TREATMENT_CODE = bill.TDL_TREATMENT_CODE;
                rdo.PATIENT_CODE = bill.TDL_PATIENT_CODE;
                rdo.NUM_ORDER = bill.NUM_ORDER;
                rdo.TOTAL_HEIN_PRICE = totalPriceHein;
                rdo.TOTAL_PATIENT_PRICE_BHYT = totalPriceBillHein;
                rdo.TOTAL_PATIENT_PRICE_DIFF = totalPriceBillDiff;
                rdo.TOTAL_PRICE = totalPrice;
                rdo.BILL_AMOUNT = bill.AMOUNT;
                rdo.KC_AMOUNT = bill.KC_AMOUNT ?? 0;
                rdo.TRANSFER_AMOUNT = bill.TRANSFER_AMOUNT ?? 0;
                rdo.TDL_BILL_FUND_AMOUNT = bill.TDL_BILL_FUND_AMOUNT ?? 0;
                rdo.EXEMPTION = bill.EXEMPTION ?? 0;
                var payForm = HisPayFormCFG.ListPayForm.FirstOrDefault(o=>o.ID == bill.PAY_FORM_ID);
                if(payForm != null)
                {
                    rdo.PAY_FORM_CODE = payForm.PAY_FORM_CODE;
                    rdo.PAY_FORM_NAME = payForm.PAY_FORM_NAME;
                }
                foreach (var item in sereServSub)
                {
                    this.ProcessorOtherSourcePrice(item.OTHER_PAY_SOURCE_ID, (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT, ref rdo);
                }
                _listSereServRdo.Add(rdo);

            }
            if (ReportTemplateCode == "MRS0012603") _listSereServRdo = _listSereServRdo.Where(o => o.TOTAL_MONEY_SHORTAGE != 0).ToList();
            if (ReportTemplateCode == "MRS0012604") _listSereServRdo = _listSereServRdo.Where(o => o.TOTAL_MONEY_EXCESS != 0).ToList();

        }

        private void ProcessorOtherSourcePrice(long? _OtherPaySourceId, decimal OtherSourcePrice, ref Mrs00126RDO rdo)
        {
            if (_OtherPaySourceId == null)
                return;
            int count = pOtherSourcePrice.Length;
            if (this.listOtherPaySource != null && count > this.listOtherPaySource.Count)
                count = this.listOtherPaySource.Count;
            for (int i = 0; i < count; i++)
            {
                if (_OtherPaySourceId == this.listOtherPaySource[i].ID)
                {
                    decimal value = (decimal)pOtherSourcePrice[i].GetValue(rdo);
                    pOtherSourcePrice[i].SetValue(rdo, OtherSourcePrice + value);
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

                int count = pOtherSourcePrice.Length;
                if (this.listOtherPaySource != null && count > this.listOtherPaySource.Count)
                    count = this.listOtherPaySource.Count;
                for (int i = 0; i < count; i++)
                {
                    dicSingleTag.Add(string.Format("OTHER_PAY_SOURCE_CODE_{0}", i + 1), this.listOtherPaySource[i].OTHER_PAY_SOURCE_CODE);
                    dicSingleTag.Add(string.Format("OTHER_PAY_SOURCE_NAME_{0}", i + 1), this.listOtherPaySource[i].OTHER_PAY_SOURCE_NAME);
                }
                objectTag.AddObjectData(store, "Report", _listSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
