using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDhst;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00541
{
    class Mrs00541Processor : AbstractProcessor
    {
        Mrs00541Filter castFilter = null;
        List<Mrs00541RDO> ListRdo = new List<Mrs00541RDO>();
        List<Mrs00541SereServRDO> ListSereServRdo = new List<Mrs00541SereServRDO>();

        public Mrs00541Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00541Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = ((Mrs00541Filter)this.reportFilter);

                //danh sách bệnh nhân
                var listTreatments = new ManagerSql().GetTreatment(castFilter);
                if (IsNotNullOrEmpty(listTreatments))
                {
                    var skip = 0;
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIDs = listTreatments.Select(o => o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        List<V_HIS_TREATMENT_4> listTreatment = new List<V_HIS_TREATMENT_4>();
                        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
                        List<V_HIS_SERE_SERV_2> ListSereServ = new List<V_HIS_SERE_SERV_2>();
                        List<HIS_DHST> ListDhst = new List<HIS_DHST>();
                        List<V_HIS_PATIENT> ListPatient = new List<V_HIS_PATIENT>();

                        //danh sách HSDT
                        listTreatment = GetListTreatment(listIDs);

                        //thông tin nghề nghiệp dân tộc
                        ListPatient = GetListPatient(listTreatment.Select(s => s.PATIENT_ID).Distinct().ToList());

                        //danh sách đối tượng
                        listPatientTypeAlter = GetPatientTypeAlter(listIDs);

                        //danh sách yêu cầu
                        ListServiceReq = GetServiceReq(listIDs);

                        //chi tiết dv
                        ListSereServ = GetListSereServ(listIDs);

                        //dhst
                        ListDhst = GetListDhst(listIDs);

                        this.ProcessLocal(listTreatment, ListPatient, ListServiceReq, ListSereServ, ListDhst, listPatientTypeAlter);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<V_HIS_TREATMENT_4> GetListTreatment(List<long> list)
        {
            List<V_HIS_TREATMENT_4> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<V_HIS_TREATMENT_4>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentView4FilterQuery filter = new HisTreatmentView4FilterQuery();
                        filter.IDs = listIDs;
                        var treatment = new HisTreatmentManager(new CommonParam()).GetView4(filter);

                        if (IsNotNullOrEmpty(treatment))
                            result.AddRange(treatment);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<V_HIS_PATIENT> GetListPatient(List<long> list)
        {
            List<V_HIS_PATIENT> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<V_HIS_PATIENT>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientViewFilterQuery filter = new HisPatientViewFilterQuery();
                        filter.IDs = listIDs;
                        var patient = new MOS.MANAGER.HisPatient.HisPatientManager(new CommonParam()).GetView(filter);

                        if (IsNotNullOrEmpty(patient))
                            result.AddRange(patient);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_DHST> GetListDhst(List<long> list)
        {
            List<HIS_DHST> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<HIS_DHST>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDhstFilterQuery filter = new HisDhstFilterQuery();
                        filter.TREATMENT_IDs = listIDs;
                        var dhst = new MOS.MANAGER.HisDhst.HisDhstManager(new CommonParam()).Get(filter);

                        if (IsNotNullOrEmpty(dhst))
                            result.AddRange(dhst);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<V_HIS_SERE_SERV_2> GetListSereServ(List<long> list)
        {
            List<V_HIS_SERE_SERV_2> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<V_HIS_SERE_SERV_2>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
                        filter.TREATMENT_IDs = listIDs;
                        filter.HAS_EXECUTE = true;
                        var sereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(new CommonParam()).GetView2(filter);

                        if (IsNotNullOrEmpty(sereServ))
                            result.AddRange(sereServ);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<long> list)
        {
            List<HIS_SERVICE_REQ> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<HIS_SERVICE_REQ>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                        filter.TREATMENT_IDs = listIDs;
                        filter.HAS_EXECUTE = true;
                        filter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        var req = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(new CommonParam()).Get(filter);

                        if (IsNotNullOrEmpty(req))
                            result.AddRange(req);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(List<long> list)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery();
                        filter.TREATMENT_IDs = listIDs;
                        filter.ORDER_FIELD = "LOG_TIME";
                        filter.ORDER_DIRECTION = "DESC";
                        var paty = new HisPatientTypeAlterManager(new CommonParam()).Get(filter);

                        if (IsNotNullOrEmpty(paty))
                            result.AddRange(paty);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            return true;
        }

        private void ProcessLocal(List<V_HIS_TREATMENT_4> listTreatmentLocal, List<V_HIS_PATIENT> listPatientLocal, List<HIS_SERVICE_REQ> listServiceReqLocal, List<V_HIS_SERE_SERV_2> listSereServLocal, List<HIS_DHST> listDhstLocal, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlterLocal)
        {
            try
            {
                if (IsNotNullOrEmpty(listTreatmentLocal))
                {
                    foreach (var item in listTreatmentLocal)
                    {
                        var serviceReqExamSub = listServiceReqLocal.Where(o => o.TREATMENT_ID == item.ID && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();

                        if (this.castFilter.EXAM_ROOM_ID != null)
                        {
                            serviceReqExamSub = serviceReqExamSub.Where(o => o.EXECUTE_ROOM_ID == this.castFilter.EXAM_ROOM_ID || o.REQUEST_ROOM_ID == this.castFilter.EXAM_ROOM_ID).ToList();
                        }

                        if (serviceReqExamSub == null)
                        {
                            continue;
                        }

                        var patientTypeAlter = listPatientTypeAlterLocal.FirstOrDefault(o => o.TREATMENT_ID == item.ID);

                        var patient = listPatientLocal.FirstOrDefault(o => o.ID == item.PATIENT_ID);

                        var dhstSub = listDhstLocal.Where(o => o.TREATMENT_ID == item.ID).ToList();

                        var sereServSub = listSereServLocal.Where(o => o.TDL_TREATMENT_ID == item.ID && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList();

                        foreach (var exam in serviceReqExamSub)
                        {
                            Mrs00541RDO rdo = new Mrs00541RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00541RDO>(rdo, exam);

                            if (exam.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.MALE_YEAR = exam.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                            }
                            else if (exam.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.FEMALE_YEAR = exam.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                            }

                            if (patientTypeAlter != null)
                            {
                                rdo.HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER;
                                rdo.HEIN_CARD_ADDRESS = patientTypeAlter.ADDRESS;
                                rdo.HEIN_CARD_FROM_TIME = patientTypeAlter.HEIN_CARD_FROM_TIME;
                                rdo.HEIN_CARD_TO_TIME = patientTypeAlter.HEIN_CARD_TO_TIME;
                            }

                            rdo.PATIENT = patient ?? new V_HIS_PATIENT();
                            rdo.TREATMENT = item ?? new V_HIS_TREATMENT_4();

                            dhstSub = dhstSub.OrderByDescending(o => o.EXECUTE_TIME ?? 0).ToList();
                            rdo.DHST = dhstSub.FirstOrDefault(o => o.EXECUTE_ROOM_ID == exam.EXECUTE_ROOM_ID) ?? dhstSub.FirstOrDefault() ?? new HIS_DHST();

                            var dvkt = sereServSub.Where(o => !o.MEDICINE_ID.HasValue && !o.MATERIAL_ID.HasValue && !o.BLOOD_ID.HasValue).ToList();
                            var mediMate = sereServSub.Where(o => o.MEDICINE_ID.HasValue || o.MATERIAL_ID.HasValue || o.BLOOD_ID.HasValue).ToList();

                            List<Mrs00541SereServRDO> lstRdo = new List<Mrs00541SereServRDO>();
                            if (IsNotNullOrEmpty(dvkt))
                            {
                                foreach (var ss in dvkt)
                                {
                                    Mrs00541SereServRDO ssRdo = new Mrs00541SereServRDO();
                                    ssRdo.DVKT = ss;
                                    ssRdo.MEDI_MATE = new V_HIS_SERE_SERV_2();
                                    ssRdo.TDL_TREATMENT_ID = ss.TDL_TREATMENT_ID;
                                    lstRdo.Add(ssRdo);
                                }
                            }
                            if (IsNotNullOrEmpty(mediMate))
                            {
                                foreach (var ss in mediMate)
                                {
                                    Mrs00541SereServRDO ssRdo = lstRdo.FirstOrDefault(o => o.MEDI_MATE.ID == 0);
                                    if (ssRdo != null)
                                    {
                                        ssRdo.MEDI_MATE = ss;
                                    }
                                    else
                                    {
                                        ssRdo = new Mrs00541SereServRDO();
                                        ssRdo.DVKT = new V_HIS_SERE_SERV_2();
                                        ssRdo.MEDI_MATE = ss;
                                        ssRdo.TDL_TREATMENT_ID = ss.TDL_TREATMENT_ID;
                                        lstRdo.Add(ssRdo);
                                    }
                                }
                            }
                            ListSereServRdo.AddRange(lstRdo);
                          
                            ListRdo.Add(rdo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                long timeFrom = 0;
                long timeTo = 0;
                if (castFilter.FEE_LOCK_TIME_FROM.HasValue && castFilter.FEE_LOCK_TIME_TO.HasValue)
                {
                    timeFrom = castFilter.FEE_LOCK_TIME_FROM.Value;
                    timeTo = castFilter.FEE_LOCK_TIME_TO.Value;
                }
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(timeFrom));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(timeTo));

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "SereServ", ListSereServRdo);
                objectTag.AddRelationship(store, "Report", "SereServ", "TREATMENT_ID", "TDL_TREATMENT_ID");
                //objectTag.AddObjectData(store, "SereServDv", ListSereServDV);
                //objectTag.AddObjectData(store, "SereServMeMa", ListSereServMediMate);
                //objectTag.AddRelationship(store, "Report", "SereServDv", "TREATMENT_ID", "TDL_TREATMENT_ID");
                //objectTag.AddRelationship(store, "Report", "SereServMeMa", "TREATMENT_ID", "TDL_TREATMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
