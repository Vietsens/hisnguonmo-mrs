using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00688
{
    class Mrs00688Processor : AbstractProcessor
    {
        private Mrs00688Filter CastFilter = null;
        private List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>();
        private List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        private List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        private List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        //private List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        private List<HIS_PATIENT_TYPE_ALTER> ListAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        private List<Mrs00688RDO> ListRdo = new List<Mrs00688RDO>();

        public Mrs00688Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00688Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.CastFilter = (Mrs00688Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu Mrs00688 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));

                HisTransactionViewFilterQuery transFilter = new HisTransactionViewFilterQuery();
                transFilter.TRANSACTION_TIME_FROM = CastFilter.TIME_FROM;
                transFilter.TRANSACTION_TIME_TO = CastFilter.TIME_TO;
                transFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                transFilter.IS_CANCEL = false;
                transFilter.HAS_SALL_TYPE = false;
                ListTransaction = new HisTransactionManager(paramGet).GetView(transFilter);

                int skip = 0;
                while (ListTransaction.Count - skip > 0)
                {
                    var listIds = ListTransaction.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServBillFilterQuery billFilter = new HisSereServBillFilterQuery();
                    billFilter.BILL_IDs = listIds.Select(s => s.ID).ToList();
                    billFilter.IS_NOT_CANCEL = true;
                    var sereServBill = new HisSereServBillManager(paramGet).Get(billFilter);
                    if (sereServBill != null && sereServBill.Count > 0)
                    {
                        ListSereServBill.AddRange(sereServBill);
                    }
                }

                List<long> treatmentId = ListTransaction.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();
                skip = 0;
                while (treatmentId.Count - skip > 0)
                {
                    var listIds = treatmentId.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                    treatFilter.IDs = listIds;
                    var treatments = new HisTreatmentManager(paramGet).Get(treatFilter);
                    if (treatments != null && treatments.Count > 0)
                    {
                        ListTreatment.AddRange(treatments);
                    }

                    HisPatientTypeAlterFilterQuery alterFilter = new HisPatientTypeAlterFilterQuery();
                    alterFilter.TREATMENT_IDs = listIds;
                    var alters = new HisPatientTypeAlterManager(paramGet).Get(alterFilter);
                    if (IsNotNullOrEmpty(alters))
                    {
                        ListAlter.AddRange(alters);
                    }
                }

                List<long> sereServIds = ListSereServBill.Select(s => s.SERE_SERV_ID).Distinct().ToList();
                skip = 0;
                while (sereServIds.Count - skip > 0)
                {
                    var listIds = sereServIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                    ssFilter.IDs = listIds;
                    var sereServs = new HisSereServManager(paramGet).Get(ssFilter);
                    if (sereServs != null && sereServs.Count > 0)
                    {
                        ListSereServ.AddRange(sereServs);
                    }
                }

                //if (IsNotNullOrEmpty(ListTreatment))
                //{
                //    var treatIds = ListTreatment.Select(s => s.ID).ToList();

                //    skip = 0;
                //    while (treatIds.Count - skip > 0)
                //    {
                //        var listIds = treatIds.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //        skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                //        HisDepartmentTranViewFilterQuery depaFilter = new HisDepartmentTranViewFilterQuery();
                //        depaFilter.TREATMENT_IDs = listIds;
                //        var depaTran = new HisDepartmentTranManager(paramGet).GetView(depaFilter);
                //        if (IsNotNullOrEmpty(depaTran))
                //        {
                //            ListDepartmentTran.AddRange(depaTran);
                //        }
                //    }
                //}
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
                if (IsNotNullOrEmpty(ListSereServBill))
                {
                    Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();//key treatment_id
                    Dictionary<long, V_HIS_TRANSACTION> dicTransaction = new Dictionary<long, V_HIS_TRANSACTION>();//key Bill_Id
                    Dictionary<long, HIS_SERE_SERV> dicSereServ = new Dictionary<long, HIS_SERE_SERV>();
                    //Dictionary<long, V_HIS_DEPARTMENT_TRAN> dicDepartmentTran = new Dictionary<long, V_HIS_DEPARTMENT_TRAN>();
                    Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        dicTreatment = ListTreatment.ToDictionary(o => o.ID, o => o);
                    }

                    if (IsNotNullOrEmpty(ListTransaction))
                    {
                        dicTransaction = ListTransaction.ToDictionary(o => o.ID, o => o);
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        dicSereServ = ListSereServ.ToDictionary(o => o.ID, o => o);
                    }

                    ////bản ghi departmentTran cuối cùng
                    //if (IsNotNullOrEmpty(ListDepartmentTran))
                    //{
                    //    ListDepartmentTran = ListDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME).ToList();

                    //    foreach (var item in ListDepartmentTran)
                    //    {
                    //        dicDepartmentTran[item.TREATMENT_ID] = item;
                    //    }
                    //}

                    //bản ghi patientTypeAlter cuối cùng
                    if (IsNotNullOrEmpty(ListAlter))
                    {
                        ListAlter = ListAlter.OrderBy(o => o.LOG_TIME).ToList();

                        foreach (var item in ListAlter)
                        {
                            dicAlter[item.TREATMENT_ID] = item;
                        }
                    }

                    foreach (var item in ListSereServBill)
                    {
                        if (!dicSereServ.ContainsKey(item.SERE_SERV_ID) || !dicTransaction.ContainsKey(item.BILL_ID) || !dicTreatment.ContainsKey(item.TDL_TREATMENT_ID))
                        {
                            continue;
                        }

                       
                        //if (dicDepartmentTran.ContainsKey(item.TDL_TREATMENT_ID))
                        //{
                        //    depa = dicDepartmentTran[item.TDL_TREATMENT_ID];
                        //}

                        HIS_PATIENT_TYPE_ALTER alter = null;
                        if (dicAlter.ContainsKey(item.TDL_TREATMENT_ID))
                        {
                            alter = dicAlter[item.TDL_TREATMENT_ID];
                        }

                        var rdo = new Mrs00688RDO(item, dicSereServ[item.SERE_SERV_ID], dicTreatment[item.TDL_TREATMENT_ID], dicTransaction[item.BILL_ID], alter);
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (CastFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.TIME_FROM.Value));
                }

                if (CastFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.TIME_TO.Value));
                }

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT.TREATMENT_CODE).ThenBy(o => o.TRANSACTION.NUM_ORDER).ThenBy(o => o.SERE_SERV.TDL_SERVICE_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
