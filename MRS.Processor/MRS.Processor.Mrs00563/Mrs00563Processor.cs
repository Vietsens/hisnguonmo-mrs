using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using System.Reflection;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisPatient;


namespace MRS.Processor.Mrs00563
{
    public class Mrs00563Processor : AbstractProcessor
    {
        private List<Mrs00563RDO> ListRdo = new List<Mrs00563RDO>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> listPatientEthnic = new List<HIS_PATIENT>();
        List<V_HIS_SERE_SERV> listHisSereServ = new List<V_HIS_SERE_SERV>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        //List<LastDepartment> ListLastDepartment = new List<LastDepartment>();
        Mrs00563Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00563Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00563Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00563Filter)reportFilter);
            var result = true;
            try
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter = this.MapFilter<Mrs00563Filter, HisTreatmentFilterQuery>(filter, treatmentFilter);
                listHisTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                var listPatientIds = listHisTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                //Danh sách BN
                List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
                if (listPatientIds != null && listPatientIds.Count > 0)
                {
                    var skip = 0;

                    while (listPatientIds.Count - skip > 0)
                    {
                        var limit = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = limit;
                        patientFilter.ORDER_FIELD = "ID";
                        patientFilter.ORDER_DIRECTION = "ASC";

                        var listPatientSub = new HisPatientManager(param).Get(patientFilter);
                        if (listPatientSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientSub Get null");
                        else
                            listPatient.AddRange(listPatientSub);
                    }
                    listPatientEthnic = listPatient.Where(o => (!string.IsNullOrWhiteSpace(o.ETHNIC_NAME)) && o.ETHNIC_NAME.Trim().ToUpper() != "KINH").ToList();
                    //Inventec.Common.Logging.LogSystem.Info("lastPatienttypeAlter" + lastPatienttypeAlter.Count);
                }
                var listTreatmentIds = listHisTreatment.Select(s => s.ID).ToList();
                
                //Lấy danh sách dịch vụ yc
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var sereServFilter = new HisSereServViewFilterQuery
                        {
                            TREATMENT_IDs = listIDs,
                            HAS_EXECUTE = true,
                            IS_EXPEND = false,
                            SERVICE_TYPE_ID=IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                        };
                        var listHisSereServSub = new HisSereServManager(paramGet).GetView(sereServFilter);
                        listHisSereServ.AddRange(listHisSereServSub);
                    }
                }
                 //yeu cau
                HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery();
                reqFilter.FINISH_TIME_FROM = filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.CLINICAL_IN_TIME_FROM;
                reqFilter.FINISH_TIME_TO = filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.CLINICAL_IN_TIME_TO;
                reqFilter.HAS_EXECUTE = true;
                reqFilter.SERVICE_REQ_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA,
                IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA
                };
                var ListServiceReq = new HisServiceReqManager().GetView(reqFilter);

                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    var listServiceReqId = ListServiceReq.Select(s => s.ID).ToList();

                    //dich vu
                    if (IsNotNullOrEmpty(listServiceReqId))
                    {
                        var skip = 0;
                        while (listServiceReqId.Count - skip > 0)
                        {
                            var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                            filterSs.SERVICE_REQ_IDs = listIDs;
                            filterSs.IS_EXPEND = false;
                            var resultSese = new HisSereServManager(paramGet).Get(filterSs);
                            if (IsNotNullOrEmpty(resultSese))
                            {
                                ListSereServ.AddRange(resultSese);
                            }
                        }
                    }
                }
                //ListLastDepartment = new ManagerSql().Get(listHisTreatment.Min(o => o.ID), listHisTreatment.Max(o => o.ID));
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }
        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRdo.Clear();
                List<long> DepartmentIdYhct = HisDepartmentCFG.HIS_DEPARTMENT_ID__YHCT;
                Inventec.Common.Logging.LogSystem.Info("khoa YHCT:" + DepartmentIdYhct);
                ListRdo = (from r in listHisTreatment select new Mrs00563RDO(r, listHisSereServ, listPatientEthnic,  DepartmentIdYhct, filter)).ToList();
                ListRdo = SumTotal(ListRdo);
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<Mrs00563RDO> SumTotal(List<Mrs00563RDO> ListRdo)
        {
            var rdo =  new Mrs00563RDO()
            {
                COUNT_ETHNIC = ListRdo.Sum(s => s.COUNT_ETHNIC),
                COUNT_EXAM = ListRdo.Sum(s => s.COUNT_EXAM),
                COUNT_EXAM_ETHNIC = ListRdo.Sum(s => s.COUNT_EXAM_ETHNIC),
                COUNT_EXAM_FEMALE = ListRdo.Sum(s => s.COUNT_EXAM_FEMALE),
                COUNT_EXAM_BHYT = ListRdo.Sum(s => s.COUNT_EXAM_BHYT),
                COUNT_EXAM_YHCT = ListRdo.Sum(s => s.COUNT_EXAM_YHCT),
                COUNT_EXAM_LESS15 = ListRdo.Sum(s => s.COUNT_EXAM_LESS15),
                COUNT_IN_TREAT = ListRdo.Sum(s => s.COUNT_IN_TREAT),
                COUNT_IN_ETHNIC = ListRdo.Sum(s => s.COUNT_IN_ETHNIC),
                COUNT_IN_TREAT_FEMALE = ListRdo.Sum(s => s.COUNT_IN_TREAT_FEMALE),
                COUNT_IN_TREAT_BHYT = ListRdo.Sum(s => s.COUNT_IN_TREAT_BHYT),
                COUNT_IN_TREAT_YHCT = ListRdo.Sum(s => s.COUNT_IN_TREAT_YHCT),
                COUNT_OUT_TREAT_YHCT = ListRdo.Sum(s => s.COUNT_OUT_TREAT_YHCT),
                COUNT_IN_TREAT_LESS15 = ListRdo.Sum(s => s.COUNT_IN_TREAT_LESS15),
                COUNT_IN_TREAT_DAY = ListRdo.Sum(s => s.COUNT_IN_TREAT_DAY),	
                
        COUNT_DEATH = ListRdo.Sum(s=>s.COUNT_DEATH),

                COUNT_DEATH_LESS1 = ListRdo.Sum(s => s.COUNT_DEATH_LESS1),
                COUNT_DEATH_LESS1_FEMALE = ListRdo.Sum(s => s.COUNT_DEATH_LESS1_FEMALE),
                COUNT_DEATH_LESS1_ETHNIC = ListRdo.Sum(s => s.COUNT_DEATH_LESS1_ETHNIC),

                COUNT_DEATH_LESS5 = ListRdo.Sum(s => s.COUNT_DEATH_LESS5),
                COUNT_DEATH_LESS5_FEMALE = ListRdo.Sum(s => s.COUNT_DEATH_LESS5_FEMALE),
                COUNT_DEATH_LESS5_ETHNIC = ListRdo.Sum(s => s.COUNT_DEATH_LESS5_ETHNIC),

            };

            rdo.DIC_ETHNIC = ListRdo.Where(q=>!string.IsNullOrWhiteSpace(q.ETHNIC_CODE)).GroupBy(o=>o.ETHNIC_CODE).ToDictionary(p=>p.Key,p=>p.Count())??new Dictionary<string,int>();
            //CLS
            rdo.COUNT_TEST = this.CountType(ListSereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN);
            rdo.COUNT_DIIM = this.CountType(ListSereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA);
            rdo.COUNT_SUIM = this.CountType(ListSereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA);
            rdo.COUNT_CT = this.CountType(ListSereServ, 0);
            var result = new List<Mrs00563RDO>();
            result.Add(rdo);
            return result;
        }
        private long CountType(List<HIS_SERE_SERV> listHisSereServSub, long serviceTypeId)
        {
            return listHisSereServSub.Count(o => o.TDL_SERVICE_TYPE_ID == serviceTypeId);
        }

        
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.CLINICAL_IN_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.CLINICAL_IN_TIME_TO ?? 0));

            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
