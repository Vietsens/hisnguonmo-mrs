using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Common.Treatment;
using System.Reflection;
using Inventec.Common.Repository;

namespace MRS.Processor.Mrs00396
{
    class Mrs00396Processor : AbstractProcessor
    {
        List<Mrs00396RDO> ListRdo = new List<Mrs00396RDO>();
        List<Mrs00396RDO> ListRdoDetail = new List<Mrs00396RDO>();
        CommonParam paramGet = new CommonParam();
        List<long> DepartmentExamIds = new List<long>();
        int DiffDateReport = 0;
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        //private List<MRS.Processor.Mrs00396.HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<MRS.Processor.Mrs00396.HIS_DEPARTMENT_TRAN>();
        //private List<MRS.Processor.Mrs00396.HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<MRS.Processor.Mrs00396.HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_EXECUTE_ROOM> listExecuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        //List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        Mrs00396Filter filter = null;

        public Mrs00396Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00396Filter);
        }


        protected override bool GetData()
        {
            filter = ((Mrs00396Filter)reportFilter);
            bool result = true;
            try
            {
                try
                {
                    DepartmentExamIds = HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM ?? new List<long>();
                    HisDepartmentFilterQuery DepartmentFilter = new HisDepartmentFilterQuery();
                    DepartmentFilter.IS_CLINICAL = true;
                    listDepartment = new HisDepartmentManager().Get(DepartmentFilter);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
                HisTreatmentFilterQuery filterTreatment = new HisTreatmentFilterQuery();
                filterTreatment.IS_PAUSE = false;
                filterTreatment.CLINICAL_IN_TIME_TO = filter.TIME_TO;
                if (filter.IS_THIS_YEAR == true)
                {
                    filterTreatment.CREATE_TIME_FROM = filter.TIME_FROM - filter.TIME_FROM % 10000000000;
                }
                if (filter.IS_WITHIN_A_YEAR == true)
                {
                    filterTreatment.CREATE_TIME_FROM = filter.TIME_FROM - 10000000000;
                }
                var listTreatment = new HisTreatmentManager().Get(filterTreatment);
                ListTreatment.AddRange(listTreatment);
                filterTreatment = new HisTreatmentFilterQuery();
                filterTreatment.CLINICAL_IN_TIME_TO = filter.TIME_TO;
                filterTreatment.IS_PAUSE = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                if (filter.IS_THIS_YEAR == true)
                {
                    filterTreatment.CREATE_TIME_FROM = filter.TIME_FROM - filter.TIME_FROM % 10000000000;
                }
                if (filter.IS_WITHIN_A_YEAR == true)
                {
                    filterTreatment.CREATE_TIME_FROM = filter.TIME_FROM - 10000000000;
                }
                var listTreatmentOut = new HisTreatmentManager().Get(filterTreatment);
                ListTreatment.AddRange(listTreatmentOut);



                var listTreatmentId = ListTreatment.Distinct().Select(o => o.ID).ToList();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterFilterQuery patientAlterFilter = new HisPatientTypeAlterFilterQuery();
                    patientAlterFilter.TREATMENT_IDs = listIDs;
                    var ptas = new HisPatientTypeAlterManager().Get(patientAlterFilter);
                    LisPatientTypeAlter.AddRange(ptas);
                }
                skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery();
                    departmentTranFilter.TREATMENT_IDs = listIDs;
                    var departmentTrans = new HisDepartmentTranManager().Get(departmentTranFilter);
                    ListDepartmentTran.AddRange(departmentTrans);
                }

                //skip = 0;
                //while (listTreatmentId.Count - skip > 0)
                //{
                //    var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //    HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                //    ssFilter.TREATMENT_IDs = listIDs;
                //    var sereServs = new HisSereServManager().Get(ssFilter);
                //    ListSereServ.AddRange(sereServs);
                //}
                /////mạnh comment
                //if (IsNotNullOrEmpty(listTreatmentId))
                //{
                //    //Đối tượng bệnh nhân
                //    var ptas = new ManagerSql().GetPatientTypeAlter(listTreatmentId.Min(), listTreatmentId.Max());
                //    if (ptas != null)
                //    {
                //        LisPatientTypeAlter = ptas.Where(o => listTreatmentId.Contains(o.TREATMENT_ID)).ToList();
                //    }
                //}

                //if (IsNotNullOrEmpty(listTreatmentId))
                //{
                //    //Chuyển khoa
                //    var dpts = new ManagerSql().GetDepartmentTran(listTreatmentId.Min(), listTreatmentId.Max());
                //    if (dpts != null)
                //    {
                //        ListDepartmentTran = dpts.Where(o => listTreatmentId.Contains(o.TREATMENT_ID)).ToList();
                //    }
                //}
                //////mạnh comment
                //if (IsNotNullOrEmpty(listTreatmentId))
                //{
                //    //dich vu giuong
                //    var sss = new ManagerSql().GetHisSereServ(listTreatmentId.Min(), listTreatmentId.Max());
                //    if (sss != null)
                //    {
                //        ListSereServ = sss.Where(o => listTreatmentId.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                //    }
                //}


                this.DiffDateReport = DateDiff.diffDate(filter.TIME_FROM, filter.TIME_TO);
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
            var result = true;
            try
            {
                List<HIS_PATIENT_TYPE_ALTER> LasPatientTypeAlter = LisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                foreach (var trea in ListTreatment)
                {

                    List<HIS_DEPARTMENT_TRAN> departmentTranSub = ListDepartmentTran.OrderBy(p => p.ID).Where(o => o.TREATMENT_ID == trea.ID).ToList();
                    if (departmentTranSub.Count == 0)
                    {
                        continue;
                    }
                    HIS_PATIENT_TYPE_ALTER lastPta = LasPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == trea.ID) ?? new HIS_PATIENT_TYPE_ALTER();

                    HIS_PATIENT_TYPE_ALTER firstPta = LisPatientTypeAlter.OrderBy(o => o.LOG_TIME).FirstOrDefault(o => o.TREATMENT_ID == trea.ID && o.TREATMENT_TYPE_ID == trea.TDL_TREATMENT_TYPE_ID);
                    if (firstPta == null || firstPta.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        continue;
                    }
                    foreach (var dpt in departmentTranSub)
                    {

                        //neu khoa vao truoc khoa chuyen noi tru dau tien thi bo qua
                        if (dpt.ID < firstPta.DEPARTMENT_TRAN_ID)
                        {
                            continue;
                        }
                        HIS_DEPARTMENT_TRAN nextDpt = this.NextDepartment(dpt);
                        HIS_DEPARTMENT_TRAN previousDpt = this.PreviousDepartment(dpt);
                        //BC hiển thị BN nội trú, ngoại trú, chi lay cac vao khoa co thoi gian ra khoa sau time_from
                        if (this.CheckContinue(dpt, nextDpt, trea.CLINICAL_IN_TIME ?? 0, trea.TDL_TREATMENT_TYPE_ID ?? 0))
                        {
                            continue;
                        }

                        //neu thoi gian nhap vien < thoi gian chuyen dieu tri dau tien
                        if (dpt.ID == firstPta.DEPARTMENT_TRAN_ID && firstPta.LOG_TIME > trea.CLINICAL_IN_TIME)
                        {
                            previousDpt = null;
                            dpt.DEPARTMENT_IN_TIME = trea.CLINICAL_IN_TIME;
                        }

                        bool previousIsTreatIn = this.HasTreatIn(previousDpt, dpt, trea.CLINICAL_IN_TIME ?? 0, trea.TDL_TREATMENT_TYPE_ID ?? 0);
                        Mrs00396RDO rdo = new Mrs00396RDO(dpt, previousDpt, nextDpt, trea, lastPta, filter, previousIsTreatIn);
                        rdo.TREATMENT_CODE = trea.TREATMENT_CODE;
                        ListRdoDetail.Add(rdo);
                    }

                }

                ListRdo = GroupByDepartment(ListRdoDetail);
                ListRdo = CreateFullDepartment(ListRdo);

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private long formula(List<long> listTreatmentId)
        {
            long result = 0;
            var filter = ((Mrs00396Filter)reportFilter);
            try
            {
                foreach (var id in listTreatmentId)
                {
                    HIS_TREATMENT treatment = ListTreatment.Where(o => o.ID == id).ToList().First();
                    var intime = treatment.IN_TIME < filter.TIME_FROM ? filter.TIME_FROM : treatment.IN_TIME;
                    var outtime = (treatment.OUT_TIME > filter.TIME_TO || treatment.OUT_TIME == null || treatment.OUT_TIME == 0) ? filter.TIME_TO : treatment.OUT_TIME;
                    var numDay = DateDiff.diffDate(intime, outtime);
                    result = result + numDay;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }
        private bool CheckContinue(HIS_DEPARTMENT_TRAN dpt, HIS_DEPARTMENT_TRAN nextDpt, long clinicalInTime, long tdlTreatmentTypeId)
        {
            if (dpt.DEPARTMENT_IN_TIME != null && dpt.DEPARTMENT_IN_TIME > this.filter.TIME_TO)
            {
                return true;
            }
            if (nextDpt.DEPARTMENT_IN_TIME != null && nextDpt.DEPARTMENT_IN_TIME < this.filter.TIME_FROM)
            {
                return true;
            }

            if (!this.HasTreatIn(dpt, nextDpt, clinicalInTime, tdlTreatmentTypeId))
            {
                return true;
            }
            return false;
        }

        private List<Mrs00396RDO> CreateFullDepartment(List<Mrs00396RDO> ListRdo)
        {
            List<Mrs00396RDO> result = new List<Mrs00396RDO>();
            try
            {

                foreach (var item in listDepartment)
                {
                    Mrs00396RDO rdo = ListRdo.FirstOrDefault(o => o.EXECUTE_DEPARTMENT_ID == item.ID) ?? new Mrs00396RDO();
                    result.Add(rdo);
                    rdo.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                    rdo.NUM_ORDER = item.NUM_ORDER;
                    rdo.DEPARTMENT_CODE = item.DEPARTMENT_CODE;
                    rdo.BED_TRUST = item.REALITY_PATIENT_COUNT ?? 0;
                    rdo.BED_PLAN = item.THEORY_PATIENT_COUNT ?? 0;
                    rdo.EXECUTE_DEPARTMENT_ID = item.ID;
                    if (item.REALITY_PATIENT_COUNT > 0)
                    {
                        rdo.POWER_TRUST = Math.Truncate(100 * rdo.NUM_DAY / (this.DiffDateReport * item.REALITY_PATIENT_COUNT.Value));
                    }
                    if (item.THEORY_PATIENT_COUNT > 0)
                    {
                        rdo.POWER_PLAN = Math.Truncate(100 * rdo.NUM_DAY / (this.DiffDateReport * item.THEORY_PATIENT_COUNT.Value));
                    }
                }

                result = result.OrderBy(o => o.NUM_ORDER ?? 9999).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<Mrs00396RDO>();
            }
            return result;
        }

        private List<Mrs00396RDO> GroupByDepartment(List<Mrs00396RDO> ListRdoDetail)
        {
            string errorField = "";
            List<Mrs00396RDO> result = new List<Mrs00396RDO>();
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.EXECUTE_DEPARTMENT_ID }).ToList();

                result.Clear();
                decimal sum = 0;
                Mrs00396RDO rdo;
                List<Mrs00396RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00396RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00396RDO();
                    listSub = item.ToList<Mrs00396RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT") || field.Name.Contains("DAY"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new List<Mrs00396RDO>();
            }
            return result;
        }

        private Mrs00396RDO IsMeaningful(List<Mrs00396RDO> listSub, PropertyInfo field)
        {
            string errorField = "";
            Mrs00396RDO result = new Mrs00396RDO();
            try
            {
                result = listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00396RDO();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
                return new Mrs00396RDO();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            if (filter.TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            }
            if (filter.TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            }
            if (filter.CLINICAL_DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", listDepartment.First().DEPARTMENT_NAME);
                ListRdo = ListRdo.Where(o => o.EXECUTE_DEPARTMENT_ID == filter.CLINICAL_DEPARTMENT_ID).ToList();
            }
            dicSingleTag.Add("DIFF_DATE_REPORT", this.DiffDateReport);

            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);

        }
        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return ListDepartmentTran.FirstOrDefault(p => p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }

        //khoa truoc
        private HIS_DEPARTMENT_TRAN PreviousDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return ListDepartmentTran.FirstOrDefault(p => p.ID == o.PREVIOUS_ID);

        }

        //Co dieu tri noi tru
        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, HIS_DEPARTMENT_TRAN nextDepartmentTran, long clinicalInTime, long tdlTreatmentTypeId)
        {
            bool result = false;
            try
            {
                if (departmentTran != null && departmentTran.DEPARTMENT_IN_TIME > 0 && clinicalInTime > 0 && tdlTreatmentTypeId == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    if (nextDepartmentTran == null || nextDepartmentTran.DEPARTMENT_IN_TIME == null || (long)nextDepartmentTran.DEPARTMENT_IN_TIME / 100 > (long)clinicalInTime / 100)
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
        //Co dieu tri noi tru (tam thoi cat di va dung clinical_in_time, tdl_treatment_type_id trong his_treatment)
        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
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

    }

}
