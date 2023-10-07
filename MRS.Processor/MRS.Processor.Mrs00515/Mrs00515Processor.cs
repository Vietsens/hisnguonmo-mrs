using MOS.MANAGER.HisTransaction;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using FlexCel.Report;
using System.IO;
using FlexCel.Core;
using Inventec.Common.FlexCellExport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisCashierRoom;

namespace MRS.Processor.Mrs00515
{
    public class Mrs00515Processor : AbstractProcessor
    {
        Mrs00515Filter castFilter = null;
        List<Mrs00515RDO> ListRdo = new List<Mrs00515RDO>();
        List<HIS_TRANSACTION> ListCurrentDeposit = new List<HIS_TRANSACTION>();
        List<HIS_TRANSACTION> ListRepay = new List<HIS_TRANSACTION>();
        List<V_HIS_TREATMENT_FEE> ListTreatmentFee = new List<V_HIS_TREATMENT_FEE>();
        List<HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_CASHIER_ROOM> ListHisCashierRoom = new List<HIS_CASHIER_ROOM>();
        List<HIS_PAY_FORM> ListHisPayForm = new List<HIS_PAY_FORM>();
        List<HIS_ACCOUNT_BOOK> ListHisAccountBook = new List<HIS_ACCOUNT_BOOK>();
        Dictionary<long, HIS_DEPARTMENT_TRAN> dicDepartmentTran = new Dictionary<long, HIS_DEPARTMENT_TRAN>();

        public Mrs00515Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00515Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00515Filter)this.reportFilter);

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPOSIT." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                filter.CREATOR = castFilter.CASHIER_LOGINNAME;
                filter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU;
                ListCurrentDeposit = new HisTransactionManager(paramGet).Get(filter);
                ListCurrentDeposit = ListCurrentDeposit.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                var listreatmentIds = ListCurrentDeposit.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                
                    var listPatientTypeAlter = new HisPatientTypeAlterManager(new CommonParam()).GetByTreatmentIds(listreatmentIds);
                    ListCurrentDeposit = ListCurrentDeposit.Where(o => castFilter.PATIENT_TYPE_ID == null || listPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID)).ToList();

                    if (IsNotNullOrEmpty(listreatmentIds))
                    {
                        var skip = 0;
                        while (listreatmentIds.Count - skip > 0)
                        {
                            var Ids = listreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTreatmentFeeViewFilterQuery treatmentFeeFilter = new HisTreatmentFeeViewFilterQuery();
                            treatmentFeeFilter.IDs = Ids;
                            var listTreatmentFeeSub = new HisTreatmentManager(param).GetFeeView(treatmentFeeFilter);
                            if (listTreatmentFeeSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listTreatmentFeeSub GetView null");
                            else
                                ListTreatmentFee.AddRange(listTreatmentFeeSub);
                        }
                    }


                    if (IsNotNullOrEmpty(listreatmentIds))
                    {
                        var skip = 0;
                        while (listreatmentIds.Count - skip > 0)
                        {
                            var Ids = listreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery();
                            departmentTranFilter.TREATMENT_IDs = Ids;
                            //departmentTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                            departmentTranFilter.ORDER_DIRECTION = "ASC";
                            departmentTranFilter.ORDER_FIELD = "ID";
                            var listDepartmentTranSub = new HisDepartmentTranManager(param).Get(departmentTranFilter);
                            if (listDepartmentTranSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listDepartmentTranSub GetView null");
                            else
                                ListDepartmentTran.AddRange(listDepartmentTranSub);
                        }
                    }
                    ListDepartmentTran = ListDepartmentTran.OrderBy(o => o.DEPARTMENT_IN_TIME ?? 0).ToList();

                    if (IsNotNullOrEmpty(listreatmentIds))
                    {
                        var skip = 0;
                        while (listreatmentIds.Count - skip > 0)
                        {
                            var Ids = listreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                            transactionFilter.TREATMENT_IDs = Ids;
                            transactionFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU;
                            transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                            transactionFilter.ORDER_DIRECTION = "ASC";
                            transactionFilter.ORDER_FIELD = "ID";
                            transactionFilter.IS_CANCEL = false;
                            var listTransactionSub = new HisTransactionManager(param).Get(transactionFilter);
                            if (listTransactionSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listTransactionSub GetView null");
                            else
                                ListRepay.AddRange(listTransactionSub);
                        }
                        ListRepay = ListRepay.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                    }

                if (!paramGet.HasException)
                {
                    result = true;
                }
                ListHisPayForm = HisPayFormCFG.ListPayForm;
                ListHisAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery());
                ListHisCashierRoom = new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery());
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
            bool result = false;
            try
            {

                if (IsNotNullOrEmpty(ListCurrentDeposit))
                {
                   
                        ListCurrentDeposit = TakeRepayYet(ListCurrentDeposit);
                        //ListCurrentDeposit = TakeResidual(ListCurrentDeposit);
                        ListCurrentDeposit = TakeDepartmentWhenDeposit(ListCurrentDeposit);

                        ListRdo = (from r in ListCurrentDeposit select new Mrs00515RDO(r, dicDepartmentTran, ListTreatmentFee, ListHisPayForm, ListHisAccountBook, ListHisCashierRoom)).ToList();

                  
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<HIS_TRANSACTION> TakeDepartmentWhenDeposit(List<HIS_TRANSACTION> ListCurrentDeposit)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
            if (ListCurrentDeposit == null) return result;
            if (!IsNotNullOrEmpty(ListDepartmentTran)) return ListCurrentDeposit;
            foreach (var item in ListCurrentDeposit)
            {
                HIS_DEPARTMENT_TRAN departmentTran = ListDepartmentTran.LastOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID&&o.DEPARTMENT_IN_TIME<=item.CREATE_TIME) ?? new HIS_DEPARTMENT_TRAN();

                
                    if (!dicDepartmentTran.ContainsKey(item.ID)) dicDepartmentTran[item.ID] = departmentTran;
                    if ((castFilter.DEPARTMENT_ID ?? 0) == 0 || departmentTran.DEPARTMENT_ID == castFilter.DEPARTMENT_ID)
                        result.Add(item);
            }
            return result;
        }
        private List<HIS_TRANSACTION> TakeResidual(List<HIS_TRANSACTION> ListCurrentDeposit)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
            if (ListCurrentDeposit == null) return result;
            if (!IsNotNullOrEmpty(ListTreatmentFee)) return ListCurrentDeposit;
            foreach (var item in ListCurrentDeposit)
            {
                V_HIS_TREATMENT_FEE treatmentFee = ListTreatmentFee.FirstOrDefault(o => o.ID == item.TREATMENT_ID) ?? new V_HIS_TREATMENT_FEE();
                //check so tien thua >0
                //so tien thua =  (tam_ung - hoan_ung + (thanh_toan - ket_chuyen))- (tong_bn_tra-mien_giam - quy_tt)
                var Residual = (treatmentFee.TOTAL_DEPOSIT_AMOUNT - treatmentFee.TOTAL_REPAY_AMOUNT + (treatmentFee.TOTAL_BILL_AMOUNT - treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT)) - (treatmentFee.TOTAL_PATIENT_PRICE - treatmentFee.TOTAL_BILL_EXEMPTION - treatmentFee.TOTAL_BILL_FUND);

                if (Residual != 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        private List<HIS_TRANSACTION> TakeRepayYet(List<HIS_TRANSACTION> ListCurrentDeposit)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
            if (ListCurrentDeposit == null) return result;
            //if (!IsNotNullOrEmpty(ListTreatmentFee)) return ListCurrentDeposit;
            foreach (var item in ListCurrentDeposit)
            {
                HIS_TRANSACTION repay = ListRepay.FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID);
                //check chưa hoàn ứng

                if (repay==null)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
               
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM??0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO??0));
                }

                ListRdo = ListRdo.OrderBy(o => o.CREATE_DATE_STR).ThenBy(t => t.TRANSACTION_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.CASHIER_LOGINNAME).Select(p => new Mrs00515RDO() { CASHIER_LOGINNAME=p.First().CASHIER_LOGINNAME,CASHIER_USERNAME=p.First().CASHIER_USERNAME,AMOUNT= p.Sum(s=>s.AMOUNT)}).ToList());
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
