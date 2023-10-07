using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00182
{
    internal class Mrs00182Processor : AbstractProcessor
    {
        Mrs00182Filter castFilter = null;

        List<VSarReportMrs00182RDO> listRdoFee = new List<VSarReportMrs00182RDO>();
        List<VSarReportMrs00182RDO> listRdoBhyt = new List<VSarReportMrs00182RDO>();
        public Mrs00182Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00182Filter);
        }
        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00182Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;

                List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);


                ProcessListTreatment(ListTreatment);
                result = true;

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
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            if (castFilter.TIME_FROM > 0)
            {
                dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            }
            if (castFilter.TIME_TO > 0)
            {
                dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
            }

            listRdoBhyt = listRdoBhyt.OrderBy(o => o.TREATMENT_CODE).ToList();
            listRdoFee = listRdoFee.OrderBy(o => o.TREATMENT_CODE).ToList();


            objectTag.AddObjectData(store, "ReportFee", listRdoFee);
            objectTag.AddObjectData(store, "ReportBhyt", listRdoBhyt);

        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                var TreatmentIDs = ListTreatment.Select(o => o.ID).ToList();
                CommonParam paramGet = new CommonParam();
                HisTransactionViewFilterQuery tf = new HisTransactionViewFilterQuery()
                {
                    TREATMENT_IDs = TreatmentIDs
                };

                var ListTransaction = new HisTransactionManager(paramGet).GetView(tf);
                if (castFilter.CASHIER_ROOM_ID != null)
                {
                    TreatmentIDs = ListTransaction.Where(p => p.CASHIER_ROOM_ID == castFilter.CASHIER_ROOM_ID).Select(o => o.TREATMENT_ID ?? 0).ToList();
                    ListTreatment = ListTreatment.Where(o => TreatmentIDs.Contains(o.ID)).ToList();
                }
                if (IsNotNullOrEmpty(ListTreatment))
                {

                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                        List<long> listTreatmentId = hisTreatments.Select(s => s.ID).ToList();

                        HisPatientTypeAlterViewFilterQuery patientAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patientAlterFilter.TREATMENT_IDs = listTreatmentId;
                        List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientAlterFilter);

                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_IDs = listTreatmentId;
                        List<V_HIS_SERE_SERV> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);

                        if (!paramGet.HasException)
                        {
                            ProcessDetailListTreatment(hisTreatments, ListPatientTypeAlter, ListSereServ);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00182.");
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoBhyt.Clear();
                listRdoFee.Clear();
            }
        }

        private void ProcessDetailListTreatment(List<V_HIS_TREATMENT> hisTreatments, List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter, List<V_HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListPatientTypeAlter) && IsNotNullOrEmpty(ListSereServ))
                {
                    Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
                    Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServHein = new Dictionary<long, List<V_HIS_SERE_SERV>>();
                    Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServFee = new Dictionary<long, List<V_HIS_SERE_SERV>>();

                    ListPatientTypeAlter = ListPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();

                    foreach (var PatientTypeAlter in ListPatientTypeAlter)
                    {
                        if (IsNotNull(PatientTypeAlter))
                        {
                            if (!dicPatientTypeAlter.ContainsKey(PatientTypeAlter.TREATMENT_ID))
                                dicPatientTypeAlter[PatientTypeAlter.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                            dicPatientTypeAlter[PatientTypeAlter.TREATMENT_ID].Add(PatientTypeAlter);
                        }
                    }

                    foreach (var sereServ in ListSereServ)
                    {
                        if (IsNotNull(sereServ))
                        {
                            if (sereServ.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                if (!dicSereServHein.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                                    dicSereServHein[sereServ.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_SERE_SERV>();
                                dicSereServHein[sereServ.TDL_TREATMENT_ID ?? 0].Add(sereServ);
                            }
                            else if (sereServ.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            {
                                if (!dicSereServFee.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                                    dicSereServFee[sereServ.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_SERE_SERV>();
                                dicSereServFee[sereServ.TDL_TREATMENT_ID ?? 0].Add(sereServ);
                            }
                        }
                    }

                    foreach (var treatment in hisTreatments)
                    {
                        bool valid = false;
                        if (dicPatientTypeAlter.ContainsKey(treatment.ID))
                        {
                            var currentPatientTypeAlter = dicPatientTypeAlter[treatment.ID].OrderByDescending(o=>o.LOG_TIME).First();
                            if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                valid = true;
                            }
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co danh sach patient_type_alter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                        }
                        if (valid)
                        {
                            if (dicSereServHein.ContainsKey(treatment.ID))
                            {
                                VSarReportMrs00182RDO rdo = new VSarReportMrs00182RDO();
                                rdo.TREATMENT_ID = treatment.ID;
                                rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                                rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                                rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                                rdo.TOTAL_DEPOSIT_AMOUNT = dicSereServHein[treatment.ID].Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                rdo.TOTAL_WITHDRAW_AMOUNT = 0;
                                //var listWithdraw = dicSereServHein[treatment.ID].Where(o => o.WITHDRAW_ID.HasValue).ToList(); 
                                //if (IsNotNullOrEmpty(listWithdraw))
                                //{
                                //    rdo.TOTAL_WITHDRAW_AMOUNT = listWithdraw.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                //}
                                if (rdo.TOTAL_DEPOSIT_AMOUNT > 0)// || rdo.TOTAL_WITHDRAW_AMOUNT > 0)
                                {
                                    listRdoBhyt.Add(rdo);
                                }
                            }
                            if (dicSereServFee.ContainsKey(treatment.ID))
                            {
                                VSarReportMrs00182RDO rdo = new VSarReportMrs00182RDO();
                                rdo.TREATMENT_ID = treatment.ID;
                                rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                                rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                                rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                                rdo.TOTAL_DEPOSIT_AMOUNT = dicSereServFee[treatment.ID].Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                //var listWithDraw = dicSereServFee[treatment.ID].Where(o => o.WITHDRAW_ID.HasValue).ToList(); 
                                //if (IsNotNullOrEmpty(listWithDraw))
                                //{
                                //    rdo.TOTAL_WITHDRAW_AMOUNT = listWithDraw.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                //}
                                if (rdo.TOTAL_DEPOSIT_AMOUNT > 0 || rdo.TOTAL_WITHDRAW_AMOUNT > 0)
                                {
                                    listRdoFee.Add(rdo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
