using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00548
{
    class Mrs00548Processor : AbstractProcessor
    {
        Mrs00548Filter caseFilter;
        List<Mrs00548RDO> listRdo = new List<Mrs00548RDO>();
        List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listTransactionDeposit = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TREATMENT> listHisTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();

        public Mrs00548Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00548Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                caseFilter = (Mrs00548Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = caseFilter.TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = caseFilter.TIME_TO;
                tranFilter.TRANSACTION_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU
                };
                listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(tranFilter);
                if (IsNotNullOrEmpty(caseFilter.CASHIER_LOGINNAMEs))
                {
                    listTransaction = listTransaction.Where(o => caseFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }
                if (IsNotNullOrEmpty(listTransaction))
                {
                    List<long> treatmentIds = listTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();

                    if (IsNotNullOrEmpty(treatmentIds))
                    {
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var limit = treatmentIds.Skip(skip).Take(500).ToList();
                            skip = skip + 500;

                            HisTreatmentViewFilterQuery HisTreatmentfilter = new HisTreatmentViewFilterQuery();
                            HisTreatmentfilter.IDs = limit;
                            HisTreatmentfilter.ORDER_FIELD = "ID";
                            HisTreatmentfilter.ORDER_DIRECTION = "ASC";
                            var listTreatmentSub = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(HisTreatmentfilter);
                            if (listTreatmentSub == null)
                                Inventec.Common.Logging.LogSystem.Error("listTreatmentSub Get null");
                            else
                                listHisTreatment.AddRange(listTreatmentSub);

                            HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                            patientTypeAlterFilter.TREATMENT_IDs = limit;
                            var patientType = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                            if (IsNotNullOrEmpty(patientType))
                                listPatientTypeAlter.AddRange(patientType);

                            HisTransactionViewFilterQuery depoFilter = new HisTransactionViewFilterQuery();
                            depoFilter.TREATMENT_IDs = limit;
                            depoFilter.TRANSACTION_TYPE_IDs = new List<long>()
                            {
                                IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU
                            };

                            listTransactionDeposit = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(depoFilter);
                        }
                    }
                }

                if (paramGet.HasException)
                {
                    Inventec.Common.Logging.LogSystem.Debug("Co exception tai DAOGET trong qua trinh tong hop du lieu Mrs00548.");
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
                if (IsNotNullOrEmpty(listHisTreatment) && IsNotNullOrEmpty(listTransaction))
                {
                    listRdo.Clear();

                    Dictionary<long, List<V_HIS_TRANSACTION>> dicTransaction = new Dictionary<long, List<V_HIS_TRANSACTION>>();
                    Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatient = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
                    Dictionary<long, List<V_HIS_TRANSACTION>> dicDeposit = new Dictionary<long, List<V_HIS_TRANSACTION>>();
                    foreach (var item in listTransaction)
                    {
                        if (!item.TREATMENT_ID.HasValue)
                            continue;

                        if (!dicTransaction.ContainsKey(item.TREATMENT_ID.Value))
                            dicTransaction[item.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                        dicTransaction[item.TREATMENT_ID.Value].Add(item);
                    }
                    if (IsNotNullOrEmpty(listPatientTypeAlter))
                    {
                        listPatientTypeAlter = listPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                        foreach (var item in listPatientTypeAlter)
                        {
                            if (!dicPatient.ContainsKey(item.TREATMENT_ID))
                                dicPatient[item.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                            dicPatient[item.TREATMENT_ID].Add(item);
                        }
                    }
                    if (IsNotNullOrEmpty(listTransactionDeposit))
                    {
                        foreach (var item in listTransactionDeposit)
                        {
                            if (!item.TREATMENT_ID.HasValue)
                                continue;

                            if (!dicDeposit.ContainsKey(item.TREATMENT_ID.Value))
                                dicDeposit[item.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                            dicDeposit[item.TREATMENT_ID.Value].Add(item);
                        }
                    }

                    foreach (var treat in listHisTreatment)
                    {
                        if (!dicTransaction.ContainsKey(treat.ID))
                            continue;

                        if (caseFilter.PATIENT_TYPE_ID.HasValue &&
                            (!dicPatient.ContainsKey(treat.ID) || dicPatient[treat.ID].First().PATIENT_TYPE_ID != caseFilter.PATIENT_TYPE_ID.Value))
                            continue;

                        if (IsNotNullOrEmpty(caseFilter.PATIENT_TYPE_IDs) &&
                            (!dicPatient.ContainsKey(treat.ID) || caseFilter.PATIENT_TYPE_IDs.Contains(dicPatient[treat.ID].First().PATIENT_TYPE_ID)))
                            continue;

                        Mrs00548RDO rdo = new Mrs00548RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00548RDO>(rdo, treat);

                        if (dicPatient.ContainsKey(treat.ID))
                        {
                            rdo.PATIENT_TYPE_NAME = dicPatient[treat.ID].First().PATIENT_TYPE_NAME;
                        }
                        rdo.CREATOR_REPA = string.Join(",", dicTransaction[treat.ID].Select(o => o.CREATOR).Distinct().ToList());
                        rdo.CASHIER_USERNAME_REPA = string.Join(",", dicTransaction[treat.ID].Select(o => o.CASHIER_USERNAME).Distinct().ToList());

                        foreach (var repay in dicTransaction[treat.ID])
                        {
                            rdo.REPAY_AMOUNT += repay.AMOUNT;
                            if (repay.TRANSACTION_TIME > (rdo.REPAY_TIME ?? 0))
                            {
                                rdo.REPAY_TIME = repay.TRANSACTION_TIME;
                            }
                        }
                        if (dicDeposit.ContainsKey(treat.ID))
                        {
                            rdo.CREATOR_DEPO = string.Join(",", dicDeposit[treat.ID].Select(o => o.CREATOR).Distinct().ToList());
                            rdo.CASHIER_USERNAME_DEPO = string.Join(",", dicDeposit[treat.ID].Select(o => o.CASHIER_USERNAME).Distinct().ToList());

                            foreach (var depo in dicDeposit[treat.ID])
                            {
                                rdo.DEPOSIT_AMOUNT += depo.AMOUNT;
                            }
                        }

                        if (rdo.REPAY_AMOUNT <= 0)
                            continue;
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                listRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(caseFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(caseFilter.TIME_TO));

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.REPAY_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
