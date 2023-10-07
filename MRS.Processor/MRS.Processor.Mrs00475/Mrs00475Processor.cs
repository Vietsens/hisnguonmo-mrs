using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00475
{
    class Mrs00475Processor : AbstractProcessor
    {
        Mrs00475Filter castFilter = null;
        List<Mrs00475RDO> listRdo = new List<Mrs00475RDO>();
        List<Mrs00475RDO> listRdoGroup = new List<Mrs00475RDO>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>();
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>();
        List<HIS_SERE_SERV_BILL> listSereServBills = new List<HIS_SERE_SERV_BILL>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        string thisReportTypeCode = "";
        public Mrs00475Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00475Filter);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "listRdoGroup", listRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "listRdoGroup", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");


                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==castFilter.REQUEST_DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME);

                dicSingleTag.Add("BRANCH_NAME", listBranchs.First().BRANCH_NAME);

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {

                this.castFilter = (Mrs00475Filter)this.reportFilter;

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.BRANCH_ID = castFilter.BRANCH_ID;
                departmentFilter.ID = castFilter.REQUEST_DEPARTMENT_ID;
                listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter).ToList();

                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                branchFilter.ID = castFilter.BRANCH_ID;
                listBranchs.AddRange(new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchFilter).ToList());

                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.HAS_SALL_TYPE = false;
                transactionFilter.IS_CANCEL = false;
                listTransactions.AddRange(new HisTransactionManager(param).GetView(transactionFilter).ToList());
                listBills = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                var skip = 0;
                if (IsNotNullOrEmpty(listBills))
                {
                    while (listBills.Count - skip > 0)
                    {
                        var listIds = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                        sereServBillFilter.BILL_IDs = listIds.Select(s => s.ID).ToList();
                        listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).Get(sereServBillFilter).ToList());
                    }
                }
                //YC-DV
                skip = 0;
                if (IsNotNullOrEmpty(listSereServBills))
                {
                    while (listSereServBills.Count - skip > 0)
                    {
                        var listIds = listSereServBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                        sereServFilter.IDs = listIds.Select(s => s.SERE_SERV_ID).ToList();
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(param).Get(sereServFilter).ToList();
                        listSereServs.AddRange(listSereServ);
                    }
                }
                //Yeu cau
                List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
                skip = 0;
                if (IsNotNullOrEmpty(listSereServs))
                {
                    var treatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                        reqFilter.TREATMENT_IDs = listIds;
                        var listServiceReqSub = new HisServiceReqManager(param).Get(reqFilter).ToList();
                        listServiceReq.AddRange(listServiceReqSub);
                    }
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                skip = 0;
                if (IsNotNullOrEmpty(listSereServs))
                {
                    var treatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    while (listSereServs.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery patientTypeAltervFilter = new HisPatientTypeAlterFilterQuery();
                        patientTypeAltervFilter.TREATMENT_IDs = listIds;
                        listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).Get(patientTypeAltervFilter).ToList());
                    }
                }
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
                CommonParam paramGet = new CommonParam();
                FilterByTreat();
                var listBranchIdInDepartment = listDepartments.Select(s => s.ID).ToList();
                var listSereServ = listSereServs.Where(s => listBranchIdInDepartment.Contains(s.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                HIS_SERVICE_REQ req = null;
                foreach (var sereServ in listSereServ)
                {
                    Mrs00475RDO rdo = new Mrs00475RDO();
                    if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue;
                    req = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0];
                    var patientTypeAlter = listPatientTypeAlters.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).Distinct().OrderByDescending(o => o.LOG_TIME).ToList();

                    List<MY_PATIENT_TYPE_ALTER> mptas = new List<MY_PATIENT_TYPE_ALTER>();
                    var outtime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now);
                    foreach (var pta in patientTypeAlter)
                    {
                        MY_PATIENT_TYPE_ALTER mpta = new MY_PATIENT_TYPE_ALTER();
                        mpta.ID = pta.ID;
                        mpta.TREATMENT_TYPE_ID = pta.TREATMENT_TYPE_ID;
                        mpta.LOG_IN_TIME = pta.LOG_TIME;
                        mpta.LOG_OUT_TIME = outtime.Value;
                        mptas.Add(mpta);

                        //Gán thời gian bắt đầu của dtg điều trị sau bằng thời gian kết thúc của đtg điều trị trước. (sắp xếp thời gian giản dần theo LOG_TIME)
                        outtime = pta.LOG_TIME;
                    }

                    // noi tru
                    var mptasTreatIn = mptas.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    bool X = false;
                    if (IsNotNullOrEmpty(mptasTreatIn))
                    {
                        foreach (var i in mptasTreatIn)
                        {
                            if (req.INTRUCTION_TIME <= i.LOG_OUT_TIME && req.INTRUCTION_TIME >= i.LOG_IN_TIME)
                            {
                                X = true;
                                break;
                            }
                        }
                    }
                    if (X)
                    {
                        rdo.SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID==sereServ.TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.AMOUNT_TREAT_IN = sereServ.AMOUNT;
                        rdo.PRICE = sereServ.PRICE;
                        rdo.TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE ?? 0;
                    }

                    var mptasTreatOut = mptas.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                    bool Y = false;
                    if (IsNotNullOrEmpty(mptasTreatOut))
                    {
                        foreach (var i in mptasTreatOut)
                        {
                            if (req.INTRUCTION_TIME <= i.LOG_OUT_TIME && req.INTRUCTION_TIME >= i.LOG_IN_TIME)
                            {
                                Y = true;
                                break;
                            }
                        }
                    }
                    if (Y)
                    {
                        rdo.SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.AMOUNT_TREAT_OUT = sereServ.AMOUNT;
                        rdo.PRICE = sereServ.PRICE;
                        rdo.TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE ?? 0;
                    }

                    listRdo.Add(rdo);
                }

                listRdo = listRdo.GroupBy(g => new { g.SERVICE_TYPE_ID, g.SERVICE_ID, g.PRICE }).Select(s => new Mrs00475RDO
                {
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    AMOUNT_TREAT_IN = s.Sum(su => su.AMOUNT_TREAT_IN),
                    AMOUNT_TREAT_OUT = s.Sum(su => su.AMOUNT_TREAT_OUT),
                    PRICE = s.First().PRICE,
                    TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE)
                }).ToList();

                listRdoGroup = listRdo.GroupBy(g => g.SERVICE_TYPE_ID).Select(s => new Mrs00475RDO
                {
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME
                }).ToList();

                listRdoGroup = listRdoGroup.OrderBy(o => o.SERVICE_TYPE_ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByTreat()
        {
            if (castFilter.TREATMENT_TYPE_IDs!= null)
            { 
            var treatment_id = this.listPatientTypeAlters.Where(o=>castFilter.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID)).Select(p=>p.TREATMENT_ID).Distinct().ToList()?? new List<long>();
                this.listPatientTypeAlters = this.listPatientTypeAlters.Where(o => treatment_id.Contains(o.TREATMENT_ID)).ToList();
                this.listSereServs = this.listSereServs.Where(o => treatment_id.Contains(o.TDL_TREATMENT_ID??0)).ToList();
            }    
        }
    }
}
