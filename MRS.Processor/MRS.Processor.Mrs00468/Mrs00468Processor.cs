using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBedLog;
using MOS.MANAGER.HisTreatmentBedRoom;

namespace MRS.Processor.Mrs00468
{
    //báo cáo hoạt động điều trị

    class Mrs00468Processor : AbstractProcessor
    {
        Mrs00468Filter castFilter = null;
        List<Mrs00468RDO> listRdo = new List<Mrs00468RDO>();
        List<Mrs00468RDO> listRdoGroup = new List<Mrs00468RDO>(); 
        List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();

        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();               // danh sách các khoa nội trú
        Dictionary<long, List<DEPARTMENT_468>> dicDepa = new Dictionary<long, List<DEPARTMENT_468>>();

        List<V_HIS_TREATMENT_4> listTreatments = new List<V_HIS_TREATMENT_4>();          // hồ sơ điều trị
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTranAll = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_BED_LOG> listHisBedLog = new List<V_HIS_BED_LOG>();
        List<V_HIS_TREATMENT_BED_ROOM> treatmentBedRooms = new List<V_HIS_TREATMENT_BED_ROOM>();

        Dictionary<long, V_HIS_BED> dicBed = new Dictionary<long, V_HIS_BED>();

        long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;

        public Mrs00468Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00468Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00468Filter)this.reportFilter;
                //khoa lam sang
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IS_CLINICAL = true;
                listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);


                //-------------------------------------------------------------------------------------
                HisTreatmentView4FilterQuery filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IS_PAUSE = false;
                filterTreatment.IN_TIME_TO = castFilter.TIME_TO;
                var listTreatment = new HisTreatmentManager().GetView4(filterTreatment);
                listTreatments.AddRange(listTreatment);
                filterTreatment = new HisTreatmentView4FilterQuery();
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                filterTreatment.IN_TIME_TO = castFilter.TIME_TO;
                filterTreatment.IS_PAUSE = true;
                filterTreatment.OUT_TIME_FROM = castFilter.TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager().GetView4(filterTreatment);
                listTreatments.AddRange(listTreatmentOut);

                List<long> treatmentIds = listTreatments.Select(o => o.ID).Distinct().ToList();

                //
                if (listTreatments != null && listTreatments.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = limit;
                        //HisDepartmentTranfilter.DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        HisDepartmentTranfilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO;// vao khoa truoc Time_to
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub Get null");
                        else
                            listHisDepartmentTranAll.AddRange(listHisDepartmentTranSub);
                    }
                    //listHisDepartmentTran = listHisDepartmentTranAll;
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
                    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisBedLogViewFilterQuery bedFilter = new HisBedLogViewFilterQuery();
                        bedFilter.TREATMENT_IDs = limit;
                        bedFilter.ORDER_FIELD = "ID";
                        bedFilter.ORDER_DIRECTION = "ASC";

                        var listBedLogSub = new HisBedLogManager(param).GetView(bedFilter);
                        if (listBedLogSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listBedLogSub Get null");
                        else
                            listHisBedLog.AddRange(listBedLogSub);
                        HisTreatmentBedRoomViewFilterQuery tbrFilter = new HisTreatmentBedRoomViewFilterQuery();
                        tbrFilter.TREATMENT_IDs = limit;
                        tbrFilter.ORDER_FIELD = "ID";
                        tbrFilter.ORDER_DIRECTION = "ASC";

                        var treatmentBedRoomSubs = new HisTreatmentBedRoomManager(param).GetView(tbrFilter);
                        if (treatmentBedRoomSubs == null)
                            Inventec.Common.Logging.LogSystem.Error("treatmentBedRoomSubs Get null");
                        else
                            treatmentBedRooms.AddRange(treatmentBedRoomSubs);
                    }
                }

                // chuyển khoa
                //BC hiển thị BN nội trú
                listDepartmentTrans = listHisDepartmentTranAll.Where(o => (HasTreatIn(o, listHisPatientTypeAlter))).ToList();

                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();

                listTreatments = listTreatments.Where(p => listDepartmentTrans.Select(o => o.TREATMENT_ID).Contains(p.ID)).ToList() ?? new List<V_HIS_TREATMENT_4>();

                //thông tin giường
                GetBed();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetBed()
        {
            HisBedViewFilterQuery bedFilter = new HisBedViewFilterQuery();
            var listBed = new HisBedManager(param).GetView(bedFilter);
            dicBed = listBed.ToDictionary(o => o.ID);
        }


        //Co dieu tri noi tru
        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                {
                    return false;
                }
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
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

        //Co dieu tri ngoai tru
        private bool HasTreatOut(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                {
                    return false;
                }
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                  && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                if (patientTypeAlterSub != null && patientTypeAlterSub.Count > 0)
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (patientTypeAlter != null && patientTypeAlter.Count > 0)
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
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

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o, List<HIS_DEPARTMENT_TRAN> dptAll)
        {

            return dptAll.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(listTreatments))
                {
                    string[] bedTypeCodeYcs = !string.IsNullOrWhiteSpace(castFilter.BED_TYPE_CODE__YCs) ? castFilter.BED_TYPE_CODE__YCs.Split(',') : null;
                    foreach (var treatment in listTreatments)
                    {
                        var dptAll = listHisDepartmentTranAll.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                        var ptaAll = listHisPatientTypeAlter.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                        var departmentTrans = listDepartmentTrans.Where(w => w.TREATMENT_ID == treatment.ID).ToList();
                        var tbrAll = treatmentBedRooms.Where(w => w.TREATMENT_ID == treatment.ID).ToList();
                        var blAll = listHisBedLog.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                        foreach (var tra in departmentTrans)
                        {
                            if (treatment.CLINICAL_IN_TIME >= castFilter.TIME_TO)
                            {
                                continue;
                            }
                            HIS_DEPARTMENT_TRAN nextDepartmentTran = NextDepartment(tra, dptAll);
                            if ((nextDepartmentTran.DEPARTMENT_IN_TIME ?? 99999999999999) < castFilter.TIME_FROM)
                            {
                                continue;
                            }
                            HIS_DEPARTMENT_TRAN previousDepartmentTran = dptAll.FirstOrDefault(o => o.ID == tra.PREVIOUS_ID);
                            if (castFilter.IS_IN_BED_ROOM == true)
                            {
                                var treatmentBedRoom = tbrAll.OrderBy(p => p.ADD_TIME).LastOrDefault(o => o.TREATMENT_ID == tra.TREATMENT_ID && o.DEPARTMENT_ID == tra.DEPARTMENT_ID && o.ADD_TIME < nextDepartmentTran.DEPARTMENT_IN_TIME);
                                if (treatmentBedRoom == null)
                                {
                                    continue;
                                }
                            }
                            var rdo = new Mrs00468RDO();
                            rdo.NEXT_DEPARTMENT_IN_TIME = nextDepartmentTran.DEPARTMENT_IN_TIME;
                            if (nextDepartmentTran.DEPARTMENT_IN_TIME == null)
                            {
                                nextDepartmentTran.DEPARTMENT_IN_TIME = (treatment.IS_PAUSE == 1 ? treatment.OUT_TIME : null) ?? castFilter.TIME_TO;
                            }
                            if ((long)nextDepartmentTran.DEPARTMENT_IN_TIME / 100 <= (long)treatment.CLINICAL_IN_TIME / 100)
                            {
                                continue;
                            }
                            if ((long)tra.DEPARTMENT_IN_TIME / 100 < (long)treatment.CLINICAL_IN_TIME / 100)
                            {
                                tra.DEPARTMENT_IN_TIME = treatment.CLINICAL_IN_TIME;
                            }
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                            rdo.DEPARTMENT_IN_TIME = tra.DEPARTMENT_IN_TIME;
                            rdo.OUT_TIME = treatment.OUT_TIME;
                            rdo.TREATMENT_TYPE = string.Format("{0}{1}", HasTreatIn(tra, ptaAll)?" Nội trú":"", HasTreatOut(tra, ptaAll)?" Ngoại trú":"");

                            rdo.DEPARTMENT_ID = tra.DEPARTMENT_ID;
                            rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == tra.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            var bl = blAll.Where(o => o.DEPARTMENT_ID == tra.DEPARTMENT_ID && o.START_TIME >= tra.DEPARTMENT_IN_TIME && o.START_TIME <= (nextDepartmentTran.DEPARTMENT_IN_TIME)).ToList();
                            var tbr = tbrAll.Where(o => o.DEPARTMENT_ID == tra.DEPARTMENT_ID && o.ADD_TIME >= tra.DEPARTMENT_IN_TIME && o.ADD_TIME <= (nextDepartmentTran.DEPARTMENT_IN_TIME)).ToList();
                            if (bl.Count > 0)
                            {
                                if (bedTypeCodeYcs != null && bedTypeCodeYcs.Length > 0)
                                {
                                    rdo.PATIENT_BED_YC = bl.Where(o => bedTypeCodeYcs.Contains(o.BED_TYPE_CODE ?? "")).Sum(s => DifferenceTime(s.START_TIME, s.FINISH_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                    rdo.PATIENT_BED_TH = bl.Where(o => !bedTypeCodeYcs.Contains(o.BED_TYPE_CODE ?? "")).Sum(s => DifferenceTime(s.START_TIME, s.FINISH_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                }
                                else
                                {
                                    rdo.PATIENT_BED_YC = 0;
                                    rdo.PATIENT_BED_TH = bl.Sum(s => DifferenceTime(s.START_TIME, s.FINISH_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                }
                            }
                            else if (tbr.Count > 0)
                            {
                                if (bedTypeCodeYcs != null && bedTypeCodeYcs.Length > 0)
                                {
                                    rdo.PATIENT_BED_YC = tbr.Where(o => dicBed.ContainsKey(o.BED_ID ?? 0) && bedTypeCodeYcs.Contains(dicBed[o.BED_ID ?? 0].BED_TYPE_CODE ?? "")).Sum(s => DifferenceTime(s.ADD_TIME, s.REMOVE_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                    rdo.PATIENT_BED_TH = tbr.Where(o => !(dicBed.ContainsKey(o.BED_ID ?? 0) && bedTypeCodeYcs.Contains(dicBed[o.BED_ID ?? 0].BED_TYPE_CODE ?? ""))).Sum(s => DifferenceTime(s.ADD_TIME, s.REMOVE_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                }
                                else
                                {
                                    rdo.PATIENT_BED_YC = 0;
                                    rdo.PATIENT_BED_TH = tbr.Sum(s => DifferenceTime(s.ADD_TIME, s.REMOVE_TIME ?? nextDepartmentTran.DEPARTMENT_IN_TIME??0));
                                }
                            }
                            rdo.PATIENT_BED = rdo.PATIENT_BED_YC + rdo.PATIENT_BED_TH;



                            if (tra.DEPARTMENT_IN_TIME < castFilter.TIME_FROM && nextDepartmentTran.DEPARTMENT_IN_TIME >= castFilter.TIME_FROM)
                                rdo.PATIENT_EX = 1;
                            else if (tra.DEPARTMENT_IN_TIME >= castFilter.TIME_FROM && tra.DEPARTMENT_IN_TIME < castFilter.TIME_TO)
                            {
                                if ((long)tra.DEPARTMENT_IN_TIME / 100 > (long)treatment.CLINICAL_IN_TIME / 100 && (HasTreatIn(previousDepartmentTran, ptaAll) || HasTreatOut(previousDepartmentTran, ptaAll)))
                                {
                                    rdo.IMP_TRAN_DEPA = 1;

                                    if ((castFilter.DEPARTMENT_ID == null || castFilter.DEPARTMENT_ID == tra.DEPARTMENT_ID) && (castFilter.DEPARTMENT_IDs == null || castFilter.DEPARTMENT_IDs.Contains(tra.DEPARTMENT_ID)))
                                    {
                                        if (!dicDepa.ContainsKey(tra.DEPARTMENT_ID))
                                        {
                                            dicDepa[tra.DEPARTMENT_ID] = GetListDepa(tra.DEPARTMENT_ID);
                                        }
                                        var depa = dicDepa[tra.DEPARTMENT_ID].Where(w => previousDepartmentTran != null && w.DEPARTMENT_ID == previousDepartmentTran.DEPARTMENT_ID).ToList();
                                        if (IsNotNullOrEmpty(depa))
                                            depa.First().IMP_TRAN_DEPA++;
                                        //depa.First().IMP_TRAN_DEPA_CODEs +=","+tra.TREATMENT_ID.ToString();
                                    }
                                }
                                else
                                {
                                    rdo.IMP_FIRST = 1;
                                }

                            }


                            if (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && treatment.OUT_TIME < castFilter.TIME_TO && treatment.OUT_TIME >= castFilter.TIME_FROM && nextDepartmentTran.ID == 0)
                            {
                                if ((treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO || treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI) && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN)
                                    rdo.EXP_CURED = 1;
                                else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                                    rdo.EXP_TRANS_HOS = 1;
                                else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                                    rdo.EXP_DEATH = 1;
                                else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                                    rdo.EXP_SICKER = 1;
                                else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                                    rdo.EXP_TRON = 1;
                                else
                                    rdo.EXP_OTHER = 1;
                            }
                            else if (nextDepartmentTran.ID > 0 && nextDepartmentTran.DEPARTMENT_IN_TIME >= castFilter.TIME_FROM && nextDepartmentTran.DEPARTMENT_IN_TIME < castFilter.TIME_TO)
                            {
                                //if (!HasTreatIn(nextDepartmentTran, listHisPatientTypeAlter))
                                //{
                                //    rdo.EXP_TRAN_DEPA_OUT_TREAT = 1; 
                                //}
                                //else
                                {
                                    rdo.EXP_TRAN_DEPA = 1;

                                    if ((castFilter.DEPARTMENT_ID == null || castFilter.DEPARTMENT_ID == tra.DEPARTMENT_ID) && (castFilter.DEPARTMENT_IDs == null || castFilter.DEPARTMENT_IDs.Contains(tra.DEPARTMENT_ID)))
                                    {
                                        if (!dicDepa.ContainsKey(tra.DEPARTMENT_ID))
                                        {
                                            dicDepa[tra.DEPARTMENT_ID] = GetListDepa(tra.DEPARTMENT_ID);
                                        }
                                        var depa = dicDepa[tra.DEPARTMENT_ID].Where(w => w.DEPARTMENT_ID == nextDepartmentTran.DEPARTMENT_ID).ToList();
                                        if (IsNotNullOrEmpty(depa))
                                            depa.First().EXP_TRAN_DEPA++;
                                        //depa.First().EXP_TRAN_DEPA_CODEs += "," + tra.TREATMENT_ID.ToString();
                                    }
                                }

                            }
                            if (nextDepartmentTran.DEPARTMENT_IN_TIME >= castFilter.TIME_TO)
                            {
                                rdo.PATIENT_END = 1;
                                if (treatment.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                                {
                                    rdo.PATIENT_END_BH = 1;
                                }
                                else
                                {
                                    rdo.PATIENT_END_VP = 1;
                                }
                                if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                                {
                                    rdo.PATIENT_END_MALE = 1;
                                }
                                else
                                {
                                    rdo.PATIENT_END_FEMALE = 1;
                                }
                            }
                            listRdo.Add(rdo);
                        }
                    }
                    if (castFilter.DEPARTMENT_ID != null)
                    {
                        listRdo = listRdo.Where(o => o.DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
                    }
                    if (castFilter.DEPARTMENT_IDs != null)
                    {
                        listRdo = listRdo.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                    }
                    if (castFilter.IS_CLINICAL_DEPA == true)
                    {
                        var departmentIds = listDepartments.Select(o => o.ID).ToList();
                        listRdo = listRdo.Where(o => departmentIds.Contains(o.DEPARTMENT_ID)).ToList();
                    }
                    listRdoGroup = listRdo.GroupBy(g => g.DEPARTMENT_ID).Select(s => new Mrs00468RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,

                        PATIENT_EX = s.Sum(su => su.PATIENT_EX),

                        IMP_FIRST = s.Sum(su => su.IMP_FIRST),
                        IMP_TRAN_DEPA = s.Sum(su => su.IMP_TRAN_DEPA),

                        EXP_CURED = s.Sum(su => su.EXP_CURED),
                        EXP_TRANS_HOS = s.Sum(su => su.EXP_TRANS_HOS),
                        EXP_DEATH = s.Sum(su => su.EXP_DEATH),
                        EXP_SICKER = s.Sum(su => su.EXP_SICKER),
                        EXP_TRON = s.Sum(su => su.EXP_TRON),
                        EXP_OTHER = s.Sum(su => su.EXP_OTHER),
                        EXP_TRAN_DEPA = s.Sum(su => su.EXP_TRAN_DEPA),

                        PATIENT_END = s.Sum(su => su.PATIENT_END),

                        PATIENT_END_BH = s.Sum(su => su.PATIENT_END_BH),

                        PATIENT_END_VP = s.Sum(su => su.PATIENT_END_VP),

                        PATIENT_END_MALE = s.Sum(su => su.PATIENT_END_MALE),

                        PATIENT_END_FEMALE = s.Sum(su => su.PATIENT_END_FEMALE),

                        PATIENT_BED = s.Sum(su => su.PATIENT_BED),

                        PATIENT_BED_YC = s.Sum(su => su.PATIENT_BED_YC),

                        PATIENT_BED_TH = s.Sum(su => su.PATIENT_BED_TH)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private double DifferenceTime(long startTime, long finishTime)
        {
            try
            {
                if (startTime != null && finishTime != null)
                {
                    if (startTime < castFilter.TIME_FROM) startTime = castFilter.TIME_FROM;
                    if (finishTime > castFilter.TIME_TO) finishTime = castFilter.TIME_TO;
                    if ( startTime < finishTime)
                    {
                        System.DateTime? dateBefore = System.DateTime.ParseExact(startTime.ToString(), "yyyyMMddHHmmss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        System.DateTime? dateAfter = System.DateTime.ParseExact(finishTime.ToString(), "yyyyMMddHHmmss",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        if (dateBefore != null && dateAfter != null)
                        {
                            TimeSpan difference = dateAfter.Value - dateBefore.Value;
                            return difference.TotalSeconds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return 0;
        }

        private List<DEPARTMENT_468> GetListDepa(long departmentId)
        {

            List<DEPARTMENT_468> listDepas = new List<DEPARTMENT_468>();
            listDepartments = listDepartments.OrderBy(s => s.ID).ToList();
            foreach (var depa in listDepartments)
            {
                var d = new DEPARTMENT_468();
                d.MAIN_DEPARTMENT_ID = departmentId;
                d.MAIN_DEPARTMENT_NAME = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId).DEPARTMENT_NAME;
                d.DEPARTMENT_ID = depa.ID;
                d.DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
                d.DEPA_EXP_KEY = "TRAN_DEPA_FROM_DEPA_" + depa.ID;
                d.DEPA_IMP_KEY = "TRAN_DEPA_TO_DEPA_" + depa.ID;
                d.EXP_TRAN_DEPA = 0;
                d.IMP_TRAN_DEPA = 0;
                //d.EXP_TRAN_DEPA_CODEs = "";
                //d.IMP_TRAN_DEPA_CODEs = "";
                listDepas.Add(d);
            }
            return listDepas;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                //if (IsNotNullOrEmpty(listDepas))
                //{
                //    foreach (var depa in listDepas)
                //    {
                //        dicSingleTag.Add(depa.DEPA_EXP_KEY, depa.EXP_TRAN_DEPA);
                //        dicSingleTag.Add(depa.DEPA_IMP_KEY, depa.IMP_TRAN_DEPA);
                //    }
                //}

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "RdoDetail", listRdo.OrderBy(o => o.DEPARTMENT_NAME).ToList()); 
                objectTag.AddObjectData(store, "Rdo", listRdoGroup.OrderBy(s => s.DEPARTMENT_NAME).ToList());
                objectTag.AddObjectData(store, "RdoDepa", dicDepa.Values.SelectMany(o => o.ToList()).Where(r => r.IMP_TRAN_DEPA > 0 || r.EXP_TRAN_DEPA > 0).OrderBy(p => p.MAIN_DEPARTMENT_NAME).ThenBy(q => q.DEPARTMENT_NAME).ToList());
                //objectTag.AddRelationship(store, "RdoGroup", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
