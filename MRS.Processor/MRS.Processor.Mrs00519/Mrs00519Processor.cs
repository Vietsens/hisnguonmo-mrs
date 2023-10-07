using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00519;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00519
{
    public class Mrs00519Processor : AbstractProcessor
    {
        private List<Mrs00519RDO> ListRdoDetail = new List<Mrs00519RDO>();
        private List<Mrs00519RDO> ListRdo = new List<Mrs00519RDO>();
        
        Mrs00519Filter filter = null;

        string thisReportTypeCode = "";

        private List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        //private List<HIS_TRANSACTION> listHisTransaction = new List<HIS_TRANSACTION>();
		List<V_HIS_TREATMENT_4> listHisTreatment = new List<V_HIS_TREATMENT_4>();
        private List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatienttypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        public Mrs00519Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00519Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00519Filter)this.reportFilter;
            //Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00519: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter)); 
            
            try
            {
                //Khoa Lâm sàng
                var CLNDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { IS_CLINICAL = true});

                //// Cac benh nhan chua khoa vien phi hoac Duyet khoa vien phi sau time_from
                //HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                //filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                //filterTreatment.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                //var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                //listHisTreatment.AddRange(listTreatment);
                //filterTreatment.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                //filterTreatment.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                //var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                //listHisTreatment.AddRange(listTreatmentOut);
                // Cac benh nhan chua khoa vien phi hoac Duyet khoa vien phi sau time_from
                HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IS_OUT = false;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatment);
                filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IS_OUT = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                listHisTreatment.AddRange(listTreatmentOut);
                //Inventec.Common.Logging.LogSystem.Info("listHisTreatment" + listHisTreatment.Count);
                List<long> treatmentIds = listHisTreatment.Select(o => o.ID).Distinct().ToList();

                //
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;
                    //while (treatmentIds.Count - skip > 0)
                    //{
                    //    var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //    HisTransactionFilterQuery BillFilter = new HisTransactionFilterQuery();
                    //    BillFilter.TREATMENT_IDs = limit;
                    //    BillFilter.TRANSACTION_TYPE_ID =IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT ;
                    //    var listTransactionSub = new HisTransactionManager(param).Get(BillFilter);
                    //    if (listTransactionSub == null)
                    //        Inventec.Common.Logging.LogSystem.Error("listTransactionSub GetView null");
                    //    else
                    //        listHisTransaction.AddRange(listTransactionSub);
                    //}
                    //listHisTransaction = listHisTransaction.Where(o => o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE).ToList();
                    //    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = limit;
                        HisDepartmentTranfilter.DEPARTMENT_IN_TIME_TO = filter.TIME_TO;// vao khoa truoc Time_to
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub Get null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listHisDepartmentTran" + listHisDepartmentTran.Count);
                        skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery patientTyeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        patientTyeAlterFilter.TREATMENT_IDs = limit;
                        patientTyeAlterFilter.ORDER_FIELD = "ID";
                        patientTyeAlterFilter.ORDER_DIRECTION = "ASC";

                        var listPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(patientTyeAlterFilter);
                        if (listPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listHisPatientTypeAlter" + listHisPatientTypeAlter.Count);
                }
                
                 lastPatienttypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                //BC hiển thị BN nội trú
                 listHisDepartmentTran = listHisDepartmentTran.Where(o => CLNDepartment.Exists(q => q.ID == o.DEPARTMENT_ID) && treatmentTypeId(o,listHisPatientTypeAlter)==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                 //Inventec.Common.Logging.LogSystem.Info("listHisDepartmentTran" + listHisDepartmentTran.Count);
                //chi lay cac vao khoa co thoi gian ra khoa sau time_from
                 listHisDepartmentTran = listHisDepartmentTran.Where(o => (NextDepartment(o).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.TIME_FROM).ToList();
                 //Inventec.Common.Logging.LogSystem.Info("listHisDepartmentTran" + listHisDepartmentTran.Count);
                ////BN dau ki se lay:
                ////"BN vien phi chua thanh toan truoc Time_from" 
                ////hoac "BN BHYT chua khoa vien phi  truoc Time_from"
                // listHisDepartmentTran = listHisDepartmentTran.Where(o => (lastPatienttypeAlter.Exists(p =>
                //     p.TREATMENT_ID == o.TREATMENT_ID
                //     && p.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                //     )
                //     && !listHisTransaction.Exists(q => q.TREATMENT_ID == o.TREATMENT_ID
                //     && q.TRANSACTION_TIME < filter.TIME_FROM
                //     ))
                //     || (lastPatienttypeAlter.Exists(p =>
                //     p.TREATMENT_ID == o.TREATMENT_ID
                //     && p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                //     )
                //     && !listHisTreatment.Exists(q => q.ID == o.TREATMENT_ID
                //     && q.FEE_LOCK_TIME < filter.TIME_FROM
                //     ))).ToList();
                



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
                ListRdoDetail = (from r in listHisDepartmentTran select new Mrs00519RDO(r, NextDepartment(r), listHisTreatment,lastPatienttypeAlter, filter)).ToList();
                //Inventec.Common.Logging.LogSystem.Info("ListRdoDetail" + ListRdoDetail.Count);
               
                GroupByDepartment();
                //Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00519RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByDepartment()
        {
            string errorField = "";
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.DEPARTMENT_NAME }).ToList();
                ListRdo.Clear();
                decimal sum = 0;
                Mrs00519RDO rdo;
                List<Mrs00519RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00519RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00519RDO();
                    listSub = item.ToList<Mrs00519RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT") || field.Name.Contains("DAY"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
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

        private Mrs00519RDO IsMeaningful(List<Mrs00519RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00519RDO();
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {
           
                return listHisDepartmentTran.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();
            
        }
        //Dien dieu tri
        private long treatmentTypeId(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            long result = 0;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID 
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID 
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME
                        && o.LOG_TIME < (NextDepartment(departmentTran).DEPARTMENT_IN_TIME??99999999999999)).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

    }

}
