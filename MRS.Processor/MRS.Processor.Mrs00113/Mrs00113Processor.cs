using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using FlexCel.Report;
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

namespace MRS.Processor.Mrs00113
{
    public class Mrs00113Processor : AbstractProcessor
    {
        Mrs00113Filter castFilter = null;
        List<Mrs00113RDO> ListRdo = new List<Mrs00113RDO>();
        List<Mrs00113RDO> ListServiceRdo = new List<Mrs00113RDO>();
        List<Mrs00113RDO> ListServiceTypeRdo = new List<Mrs00113RDO>();
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        string Patient_Type_Name;
        List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran;

        public Mrs00113Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00113Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00113Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPARTMENT_TRAN, MRS00113 Filter. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisDepartmentTranViewFilterQuery depaTranFilter = new HisDepartmentTranViewFilterQuery();
                depaTranFilter.DEPARTMENT_IN_TIME_FROM = castFilter.TIME_FROM;
                depaTranFilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO;
                //depaTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                ListDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(depaTranFilter);

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT MRS00113." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => depaTranFilter), depaTranFilter));
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
            bool result = false;
            try
            {
                ProcessListDepartmentTran(ListDepartmentTran);
                GetPatientTypeById();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            try
            {
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    ListDepartmentTran = CheckDepartmentTran(ListDepartmentTran);
                    CommonParam paramGet = new CommonParam();
                    if (castFilter.PATIENT_TYPE_ID.HasValue)
                    {
                        ListDepartmentTran = ListDepartmentTran.Where(o => checkPatientTypeOfTreatment(paramGet, o.TREATMENT_ID)).ToList();
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh xu ly checkPatientTypeOfTreatment, MRS00113.");
                        }
                    }

                    int start = 0;
                    int count = ListDepartmentTran.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_DEPARTMENT_TRAN> hisDepartmentTrans = ListDepartmentTran.Skip(start).Take(limit).ToList();
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_IDs = hisDepartmentTrans.Select(s => s.TREATMENT_ID).ToList();
                        List<V_HIS_SERE_SERV> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);

                        HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                        reqFilter.TREATMENT_IDs = hisDepartmentTrans.Select(s => s.TREATMENT_ID).ToList();
                        dicServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(reqFilter).ToDictionary(o => o.ID);

                        //DV - thanh toan
                        var listSereServId = ListSereServ.Select(s => s.ID).ToList();

                        if (IsNotNullOrEmpty(listSereServId))
                        {
                            var skip = 0;
                            while (listSereServId.Count - skip > 0)
                            {
                                var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                HisSereServBillViewFilterQuery filterSereServBill = new HisSereServBillViewFilterQuery();
                                filterSereServBill.SERE_SERV_IDs = listIDs;
                                var listSereServBillSub = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(filterSereServBill);
                                ListSereServBill.AddRange(listSereServBillSub);
                            }
                        }
                        if (!paramGet.HasException)
                        {
                            ProcessListSereServ(ListSereServ, dicServiceReq);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00113.");
                    }
                    ListServiceRdo = ListRdo.GroupBy(g => g.SERVICE_ID).Select(s => new Mrs00113RDO { SERVICE_ID = s.First().SERVICE_ID, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, AMOUNT = s.Sum(s1 => s1.AMOUNT), TOTAL_PRICE = s.Sum(s2 => s2.TOTAL_PRICE) }).ToList();
                    ListServiceTypeRdo = ListRdo.GroupBy(g => g.SERVICE_TYPE_ID).Select(s => new Mrs00113RDO { SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID, SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE, SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                ListServiceRdo.Clear();
                ListServiceTypeRdo.Clear();
            }
        }

        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ();
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new HIS_SERVICE_REQ();
            }
            return result;
        }

        private void ProcessListSereServ(List<V_HIS_SERE_SERV> ListSereServ, Dictionary<long, HIS_SERVICE_REQ> dicServiceReq)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    ListSereServ = ListSereServ.Where(o => !ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) && o.VIR_TOTAL_PATIENT_PRICE > 0 && o.AMOUNT > 0).ToList();
                    var Groups = ListSereServ.GroupBy(g => new { g.TDL_TREATMENT_ID, g.SERVICE_ID }).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        if (IsNotNullOrEmpty(listSub))
                        {
                            ListRdo.Add(new Mrs00113RDO(listSub, req(listSub.First())));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool checkPatientTypeOfTreatment(CommonParam paramGet, long treatmentId)
        {
            bool result = false;
            try
            {
                if (treatmentId > 0)
                {
                    HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                    appFilter.TREATMENT_ID = treatmentId;
                    var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last();
                    if (IsNotNull(currentPatientTypeAlter) && currentPatientTypeAlter.PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID.Value)
                    {
                        result = true;
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

        private List<V_HIS_DEPARTMENT_TRAN> CheckDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            List<V_HIS_DEPARTMENT_TRAN> currentDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
            try
            {
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    List<long> ListTreatmentId = ListDepartmentTran.Select(s => s.TREATMENT_ID).Distinct().ToList();
                    foreach (var treatId in ListTreatmentId)
                    {
                        var listData = ListDepartmentTran.Where(o => o.TREATMENT_ID == treatId).ToList();
                        if (listData != null && listData.Count > 0)
                        {
                            if (listData.Count == 1)
                            {
                                currentDepartmentTran.Add(listData.First());
                            }
                            else
                            {
                                currentDepartmentTran.Add(listData.OrderBy(o => o.DEPARTMENT_IN_TIME).First());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                currentDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
            }
            return currentDepartmentTran;
        }

        private void GetPatientTypeById()
        {
            try
            {
                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    var patientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager().GetById(castFilter.PATIENT_TYPE_ID.Value);
                    if (IsNotNull(patientType))
                    {
                        Patient_Type_Name = patientType.PATIENT_TYPE_NAME;
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
                    dicSingleTag.Add("LOG_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("LOG_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ListRdo = ListRdo.OrderBy(o => o.SERVICE_TYPE_ID).ThenBy(t => t.SERVICE_ID).ThenBy(b => b.TREATMENT_ID).ToList();
                ListServiceRdo = ListServiceRdo.OrderBy(o => o.SERVICE_TYPE_ID).ThenBy(t => t.SERVICE_ID).ToList();
                ListServiceTypeRdo = ListServiceTypeRdo.OrderBy(o => o.SERVICE_TYPE_ID).ToList();

                dicSingleTag.Add("PATIENT_TYPE_NAME", Patient_Type_Name);

                objectTag.AddObjectData(store, "ServiceTypes", ListServiceTypeRdo);
                objectTag.AddObjectData(store, "Services", ListServiceRdo);
                objectTag.AddObjectData(store, "Treatments", ListRdo);
                objectTag.AddRelationship(store, "ServiceTypes", "Services", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
                objectTag.AddRelationship(store, "Services", "Treatments", "SERVICE_ID", "SERVICE_ID");
                objectTag.SetUserFunction(store, "FuncServiceTypeRownumber", new CustomerFuncRownumberServiceType());
                objectTag.SetUserFunction(store, "FuncServiceRownumber", new CustomerFuncRownumberService());
                objectTag.SetUserFunction(store, "FuncTreatmentRownumber", new CustomerFuncRownumberTreatment());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class CustomerFuncRownumberServiceType : TFlexCelUserFunction
    {
        int Num_Order = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                if (serviceTypeId > 0)
                {
                    Num_Order++;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Num_Order;
        }
    }

    class CustomerFuncRownumberService : TFlexCelUserFunction
    {
        long ServiceTypeId = 0;
        int Num_Order = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                long serviceId = Convert.ToInt64(parameters[1]);
                if (serviceTypeId == ServiceTypeId)
                {
                    Num_Order = Num_Order + 1;
                }
                else
                {
                    ServiceTypeId = serviceTypeId;
                    Num_Order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Num_Order;
        }
    }

    class CustomerFuncRownumberTreatment : TFlexCelUserFunction
    {
        long ServiceTypeId = 0;
        long ServiceId = 0;
        int Num_Order = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                long serviceId = Convert.ToInt64(parameters[1]);
                long treatmentId = Convert.ToInt64(parameters[2]);
                if ((serviceTypeId == ServiceTypeId) && (serviceId == ServiceId))
                {
                    Num_Order = Num_Order + 1;
                }
                else
                {
                    ServiceTypeId = serviceTypeId;
                    ServiceId = serviceId;
                    Num_Order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Num_Order;
        }
    }
}
