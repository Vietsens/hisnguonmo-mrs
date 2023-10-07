using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentResult;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisBedLog;
using MOS.MANAGER.HisTreatmentEndType;


namespace MRS.Processor.Mrs00205
{
    
    public class Mrs00205Processor : AbstractProcessor
    {
        enum TimeData
        {
            FeeLock,
            Bill
        }
        Mrs00205Filter CastFilter = null;
        private List<Mrs00205RDO> listRdo = new List<Mrs00205RDO>();
        List<Mrs00205RDO> listRdoTreatment = new List<Mrs00205RDO>();
        CommonParam paramGet = new CommonParam();
        private decimal? TOTAL_PRICE;
        private string SERVICE_REPORT_NAME;
        private List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        private List<HIS_SERE_SERV_BILL> listSereServBills = new List<HIS_SERE_SERV_BILL>();
        private List<HIS_SERE_SERV_DEPOSIT> listSereServDeposits = new List<HIS_SERE_SERV_DEPOSIT>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> listHisPatient = new List<HIS_PATIENT>();
        List<HIS_TRANSACTION> listHisBill = new List<HIS_TRANSACTION>();
        List<HIS_TREATMENT_RESULT> listHisTreamentResult = new List<HIS_TREATMENT_RESULT>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<V_HIS_BED_LOG> listBedLog = new List<V_HIS_BED_LOG>();
        List<HIS_TREATMENT_END_TYPE> listTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        private string a = "";
        public Mrs00205Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            a = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00205Filter);
        }
        
        protected override bool GetData()
        {
            
            var result = true;
            try
            {
                
                this.CastFilter = (Mrs00205Filter)this.reportFilter;
                
                var metyFilterService = new HisServiceFilterQuery
                {
                    IDs = this.CastFilter.SERVICE_IDs
                };
                if (this.CastFilter.SERVICE_ID != null)
                {
                    listServiceReq = listServiceReq.Where(o => o.ID == this.CastFilter.SERVICE_ID).ToList();
                
                }
                
               
                var service = new HisServiceManager(paramGet).Get(metyFilterService);
               
                //get treatmentresult
                HisTreatmentResultFilterQuery filterTreatmentResult = new HisTreatmentResultFilterQuery();
                listHisTreamentResult = new HisTreatmentResultManager(paramGet).Get(filterTreatmentResult);
                //
                //get service_req
                //HisServiceReqFilterQuery filterServiceReq = new HisServiceReqFilterQuery();
                //listServiceReq = new HisServiceReqManager(paramGet).Get(filterServiceReq);
                //
                //ten va ma dich vu
                SERVICE_REPORT_NAME = string.Format("{0} - {1}", SERVICE_REPORT_NAME = service.Select(s => s.SERVICE_NAME).First(), SERVICE_REPORT_NAME = service.Select(s => s.SERVICE_CODE).First());
                TimeData OptionTakeData = TimeData.FeeLock;
                if (this.CastFilter.DATE_FROM != null && this.CastFilter.DATE_TO != null)
                {
                    OptionTakeData = TimeData.FeeLock;
                }
                else if (this.CastFilter.TRANSACTION_TIME_FROM != null && this.CastFilter.TRANSACTION_TIME_TO != null)
                {
                    OptionTakeData = TimeData.Bill;
                }
                if (OptionTakeData == TimeData.FeeLock)
                {
                    #region Lay theo thoi gian khoa vien phi
                    //HSDT da khoa vien phi
                var metyFilterTreatment = new HisTreatmentFilterQuery
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE,
                    FEE_LOCK_TIME_FROM = this.CastFilter.DATE_FROM,
                    FEE_LOCK_TIME_TO = this.CastFilter.DATE_TO
                };
                listHisTreatment = new HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                if (this.CastFilter.TREAT_PATIENT_TYPE_ID != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => o.TDL_PATIENT_TYPE_ID == this.CastFilter.TREAT_PATIENT_TYPE_ID).ToList();
                }
              
                //YC-DV
                if (listHisTreatment != null)
                {
                    var listTreatmentIds = listHisTreatment.Select(s => s.ID).ToList();
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var Ids = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.TREATMENT_IDs = Ids;
                        sereServFilter.TDL_SERVICE_TYPE_IDs = this.CastFilter.SERVICE_TYPE_IDs;
                        sereServFilter.TDL_SERVICE_TYPE_ID = this.CastFilter.SERVICE_TYPE_ID;
                        sereServFilter.PATIENT_TYPE_IDs = this.CastFilter.PATIENT_TYPE_IDs;
                        sereServFilter.PATIENT_TYPE_ID = this.CastFilter.PATIENT_TYPE_ID;
                        var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                        if (listSereServSub != null)
                        {
                            listSereServs.AddRange(listSereServSub);
                        }
                        HisSereServBillFilterQuery sereServBillFilter = new HisSereServBillFilterQuery();
                        sereServBillFilter.TDL_TREATMENT_IDs = Ids;
                        var listSereServBillSub = new HisSereServBillManager(param).Get(sereServBillFilter);
                        if (listSereServBillSub != null)
                        {
                            listSereServBills.AddRange(listSereServBillSub);
                        }
                        HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                        HisTransactionfilter.TREATMENT_IDs = Ids;
                        HisTransactionfilter.IS_CANCEL = false;
                        HisTransactionfilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                        var listTransactionSub = new HisTransactionManager(param).Get(HisTransactionfilter);
                        if (listTransactionSub != null)
                        {
                            listHisBill.AddRange(listTransactionSub);
                        }
                        HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                        serviceReqFilter.TREATMENT_IDs = Ids;
                        var services = new HisServiceReqManager().Get(serviceReqFilter);
                        if (services!=null)
                        {
                            listServiceReq.AddRange(services);
                        }

                        //HisBedLogViewFilterQuery bedLogFilter = new HisBedLogViewFilterQuery();
                        
                        //var bedlog = new HisBedLogManager(param).GetView(bedLogFilter);
                        //if (bedlog!=null)
                        //{
                        //    listBedLog.AddRange(bedlog);
                        //}
                          
                    
                    }
                }
                    #endregion
                }
                else if (OptionTakeData == TimeData.Bill)
                {
                    #region Lay theo thoi gian thanh toan
                    
                    Mrs00205GDO gdo = null;
                    if (this.CastFilter.HAS_BILL==true)
                    {
                        gdo=new ManagerSql().GetByBillTime(this.CastFilter);
                    }
                    else
                    {
                        gdo = new ManagerSql().GetByIntructionTime(this.CastFilter);
                    }
                    if (gdo != null && gdo.HIS_TRANSACTION != null)
                    {
                        listHisBill = gdo.HIS_TRANSACTION.GroupBy(o=>o.ID).Select(p=>p.First()).ToList();
                    }
                    if (gdo != null && gdo.HIS_SERE_SERV != null)
                    {
                        listSereServs = gdo.HIS_SERE_SERV.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    }
                    if (gdo != null && gdo.HIS_TREATMENT != null)
                    {
                        listHisTreatment = gdo.HIS_TREATMENT.GroupBy(o => o.ID).Select(p => p.First()).Where(q=>listSereServs.Exists(r=>r.TDL_TREATMENT_ID==q.ID)).ToList();
                    }
                   
                    #endregion

                }

                if (this.CastFilter.SERVICE_IDs != null)
                {
                    listSereServs = listSereServs.Where(o => this.CastFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).ToList();
                }
              

                if (this.CastFilter.SERVICE_ID != null)
                {
                    listSereServs = listSereServs.Where(o => this.CastFilter.SERVICE_ID==o.SERVICE_ID).ToList();
                }


                if (listHisTreatment != null)
                {
                    HisBedLogViewFilterQuery bedLogFilter = new HisBedLogViewFilterQuery();

                    var bedlog = new HisBedLogManager(param).GetView(bedLogFilter);
                    if (bedlog != null)
                    {
                      
                        listBedLog.AddRange(bedlog);
                    }

                   
                }

                if (listHisTreatment != null)
                {
                    HisTreatmentEndTypeFilterQuery treatmentEndTypeFilter = new HisTreatmentEndTypeFilterQuery();
                  var treatmentEndType = new HisTreatmentEndTypeManager(param).Get(treatmentEndTypeFilter);
                    if (treatmentEndType != null)
                    {
                    listTreatmentEndType.AddRange(treatmentEndType);
                    }


                }
                

                if (listHisTreatment != null)
                {
                    var listPatientIds = listHisTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                    var skip = 0;
                    while (listPatientIds.Count - skip > 0)
                    {
                        var Ids = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery PatientFilter = new HisPatientFilterQuery();
                        PatientFilter.IDs = Ids;
                        var listPatientSub = new HisPatientManager(param).Get(PatientFilter);
                        if (listPatientSub != null)
                        {
                            listHisPatient.AddRange(listPatientSub);
                        }
                       
                    }
                }
                result = true;
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
                var groupByPatientIds = listSereServs.GroupBy(ss => ss.TDL_TREATMENT_ID ?? 0).ToList();
                foreach (var groupPatientId in groupByPatientIds)
                {
                    var treatment = listHisTreatment.FirstOrDefault(o => o.ID == groupPatientId.First().TDL_TREATMENT_ID);
                    if (treatment == null)
                    {
                        continue;
                    }
                    Mrs00205RDO rdoTreatment = new Mrs00205RDO(treatment,listTreatmentEndType,listBedLog, groupPatientId.ToList<HIS_SERE_SERV>(), listHisBill, listHisPatient,listHisTreamentResult,listServiceReq, this.CastFilter);
                    if (this.CastFilter.SPLIT_REQUSER_REQROOM == true)
                    {
                        var groupByServiceIds = groupPatientId.GroupBy(s => new { s.SERVICE_ID, s.PATIENT_TYPE_ID, s.TDL_REQUEST_LOGINNAME,s.TDL_REQUEST_ROOM_ID }).ToList();
                        foreach (var groupByServiceId in groupByServiceIds)
                        {

                            var rdo = new Mrs00205RDO(treatment, listTreatmentEndType, listBedLog, groupByServiceId.ToList<HIS_SERE_SERV>(), listHisBill, listHisPatient, listHisTreamentResult, listServiceReq, this.CastFilter);
                            listRdo.Add(rdo);
                            
                        }
                    }
                    else
                    {
                        var groupByServiceIds = groupPatientId.GroupBy(s => new { s.SERVICE_ID, s.PATIENT_TYPE_ID,s.SERVICE_REQ_ID}).ToList();
                        foreach (var groupByServiceId in groupByServiceIds)
                        {

                            var rdo = new Mrs00205RDO(treatment, listTreatmentEndType, listBedLog, groupByServiceId.ToList<HIS_SERE_SERV>(), listHisBill, listHisPatient, listHisTreamentResult, listServiceReq, this.CastFilter);
                            listRdo.Add(rdo);

                        }
                    }
                    listRdoTreatment.Add(rdoTreatment);
                }
                TOTAL_PRICE = listRdo.Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (this.CastFilter.PATIENT_TYPE_ID != null)
            {
                dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == this.CastFilter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            }
            if (this.CastFilter.PATIENT_TYPE_IDs != null)
            {
                dicSingleTag.Add("PATIENT_TYPE_NAME", string.Join(" - ", HisPatientTypeCFG.PATIENT_TYPEs.Where(o => this.CastFilter.PATIENT_TYPE_IDs.Contains(o.ID)).Select(p => p.PATIENT_TYPE_NAME).Distinct().ToList()));
            }
            if (this.CastFilter.TREAT_PATIENT_TYPE_ID != null)
            {
                dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == this.CastFilter.TREAT_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            }
            dicSingleTag.Add("SERVICE_REPORT_NAME", SERVICE_REPORT_NAME);
            dicSingleTag.Add("TOTAL_PRICE", (double?)TOTAL_PRICE);
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.CastFilter.DATE_FROM ?? 0));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.CastFilter.DATE_TO ?? 0));
            dicSingleTag.Add("TRANSACTION_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.CastFilter.TRANSACTION_TIME_FROM ?? 0));
            dicSingleTag.Add("TRANSACTION_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.CastFilter.TRANSACTION_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ReportTreatment", listRdoTreatment);


        }


    }
}
