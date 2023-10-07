using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisExpMest;
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

namespace MRS.Processor.Mrs00138
{
    public class Mrs00138Processor : AbstractProcessor
    {
        Mrs00138Filter castFilter = null;
        List<Mrs00138RDO> ListRdo = new List<Mrs00138RDO>();

        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_TREATMENT> ListTreatment;

        public Mrs00138Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00138Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00138Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_TREATMENT, MRS00138 Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                //Hồ sơ điều trị duyệt khóa HFS (ra viện)
                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilter);
                listMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT, MRS00138.");
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
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ProcessListTreatment(ListTreatment);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_IDs = hisTreatments.Select(s => s.ID).ToList();
                        ssFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC;
                        var hisSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                        if (!paramGet.HasException)
                        {
                            ProcessListSereServ(paramGet, hisSereServs, hisTreatments);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00138.");
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00138.");
                    }
                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ListRdo.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).Select(s => new Mrs00138RDO { SERVICE_ID = s.First().SERVICE_ID, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, VIR_PRICE = s.First().VIR_PRICE, EXAM_AMOUNT = s.Sum(s1 => s1.EXAM_AMOUNT), TREAT_AMOUNT = s.Sum(s2 => s2.TREAT_AMOUNT) }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListSereServ(CommonParam paramGet, List<V_HIS_SERE_SERV> ListSereServ, List<V_HIS_TREATMENT> hisTreatments)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    ListSereServ = ListSereServ.Where(o => o.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.AMOUNT > 0).ToList();
                    foreach (var treatment in hisTreatments)
                    {
                        List<HIS_EXP_MEST> listPrescription = null;
                        var hisSereServs = ListSereServ.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                        if (IsNotNullOrEmpty(hisSereServs))
                        {
                            HisExpMestFilterQuery presFilter = new HisExpMestFilterQuery();
                            presFilter.TDL_TREATMENT_ID = treatment.ID;
                            presFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                            listPrescription = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(presFilter);
                        }
                        if (IsNotNullOrEmpty(listPrescription))
                        {
                            HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                            appFilter.TREATMENT_ID = treatment.ID;
                            var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last();
                            if (IsNotNull(currentPatientTypeAlter))
                            {
                                var Group = hisSereServs.GroupBy(o => o.SERVICE_REQ_ID).ToList();
                                foreach (var group in Group)
                                {
                                    var listSub = group.ToList<V_HIS_SERE_SERV>();
                                    var serviceReq = listPrescription.FirstOrDefault(o => o.SERVICE_REQ_ID == listSub.First().SERVICE_REQ_ID);
                                    if (IsNotNull(serviceReq))
                                    {
                                        List<Mrs00138RDO> listData = new List<Mrs00138RDO>();
                                        if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                        {
                                            listData = (from r in listSub select new Mrs00138RDO(r, listMedicineType.FirstOrDefault(o => o.SERVICE_ID == r.SERVICE_ID), true)).ToList();
                                        }
                                        else
                                        {
                                            listData = (from r in listSub select new Mrs00138RDO(r, listMedicineType.FirstOrDefault(o => o.SERVICE_ID == r.SERVICE_ID), false)).ToList();
                                        }
                                        ListRdo.AddRange(listData);
                                    }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                ListRdo = ListRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                objectTag.AddObjectData(store, "Medicines", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
