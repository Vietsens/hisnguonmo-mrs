using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using ACS.Filter;
using FlexCel.Report;
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
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00284
{
    class Mrs00284Processor : AbstractProcessor
    {
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00284RDO> ListRdo = new List<Mrs00284RDO>();
        public Mrs00284Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00284Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00284Filter)reportFilter);
            bool result = true;
            try
            {
               
                //vao vien truoc time_to & chua ra vien
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IS_OUT = false;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                var lstTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                listTreatment.AddRange(lstTreatment);

                //ra vien sau time_from
                filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IS_OUT = true;
                filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                listTreatment.AddRange(listTreatmentOut);

                var listTreatmentId = listTreatment.Select(o => o.ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                   
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        //Lấy các log chuyển đối tượng
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        listPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                        //Lấy các log chuyển khoa
                        HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var ListDepartmentTranLib = new HisDepartmentTranManager(paramGet).Get(departmentTranFilter);
                        listDepartmentTran.AddRange(ListDepartmentTranLib);
                    }
                }
                //khoa
                HisDepartmentFilterQuery filterDepartment = new HisDepartmentFilterQuery();
                filterDepartment.IS_CLINICAL = true;
                listDepartment = new HisDepartmentManager(paramGet).Get(filterDepartment);
                if (IsNotNullOrEmpty(listDepartment) && IsNotNullOrEmpty(HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM))
                {
                    listDepartment = listDepartment.Where(o => HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM.Contains(o.ID)).ToList();
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
            var result = true;
            try
            {
                ListRdo.Clear();

                if (IsNotNullOrEmpty(listDepartment))
                {
                    DateTime DateFrom = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime((long)((Mrs00284Filter)reportFilter).TIME_FROM);
                    DateFrom = DateFrom.Date;
                    List<long> Date = new List<long>();
                    for (int count = 0; count <= 31; count++)
                    {
                        long date = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateFrom) ?? 0;
                        // Thêm khoảng thời gian.
                        DateFrom = DateFrom.Add(new System.TimeSpan(1, 0, 0, 0));
                        Date.Add(date);
                    }

                    foreach (var department in listDepartment)
                    {
                        Mrs00284RDO rdo = new Mrs00284RDO();
                        rdo.DEPARTMENT_ID = department.ID;
                        rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        rdo.AMOUNT_DATE_0 = AmountOfDepartment(department, ((Mrs00284Filter)reportFilter).TIME_FROM ?? 0, Date[1]);
                        rdo.AMOUNT_DATE_1 = AmountOfDepartment(department, Date[1], Date[2]);
                        rdo.AMOUNT_DATE_2 = AmountOfDepartment(department, Date[2], Date[3]);
                        rdo.AMOUNT_DATE_3 = AmountOfDepartment(department, Date[3], Date[4]);
                        rdo.AMOUNT_DATE_4 = AmountOfDepartment(department, Date[4], Date[5]);
                        rdo.AMOUNT_DATE_5 = AmountOfDepartment(department, Date[5], Date[6]);
                        rdo.AMOUNT_DATE_6 = AmountOfDepartment(department, Date[6], Date[7]);
                        rdo.AMOUNT_DATE_7 = AmountOfDepartment(department, Date[7], Date[8]);
                        rdo.AMOUNT_DATE_8 = AmountOfDepartment(department, Date[8], Date[9]);
                        rdo.AMOUNT_DATE_9 = AmountOfDepartment(department, Date[9], Date[10]);
                        rdo.AMOUNT_DATE_10 = AmountOfDepartment(department, Date[10], Date[11]);
                        rdo.AMOUNT_DATE_11 = AmountOfDepartment(department, Date[11], Date[12]);
                        rdo.AMOUNT_DATE_12 = AmountOfDepartment(department, Date[12], Date[13]);
                        rdo.AMOUNT_DATE_13 = AmountOfDepartment(department, Date[13], Date[14]);
                        rdo.AMOUNT_DATE_14 = AmountOfDepartment(department, Date[14], Date[15]);
                        rdo.AMOUNT_DATE_15 = AmountOfDepartment(department, Date[15], Date[16]);
                        rdo.AMOUNT_DATE_16 = AmountOfDepartment(department, Date[16], Date[17]);
                        rdo.AMOUNT_DATE_17 = AmountOfDepartment(department, Date[17], Date[18]);
                        rdo.AMOUNT_DATE_18 = AmountOfDepartment(department, Date[18], Date[19]);
                        rdo.AMOUNT_DATE_19 = AmountOfDepartment(department, Date[19], Date[20]);
                        rdo.AMOUNT_DATE_20 = AmountOfDepartment(department, Date[20], Date[21]);
                        rdo.AMOUNT_DATE_21 = AmountOfDepartment(department, Date[21], Date[22]);
                        rdo.AMOUNT_DATE_22 = AmountOfDepartment(department, Date[22], Date[23]);
                        rdo.AMOUNT_DATE_23 = AmountOfDepartment(department, Date[23], Date[24]);
                        rdo.AMOUNT_DATE_24 = AmountOfDepartment(department, Date[24], Date[25]);
                        rdo.AMOUNT_DATE_25 = AmountOfDepartment(department, Date[25], Date[26]);
                        rdo.AMOUNT_DATE_26 = AmountOfDepartment(department, Date[26], Date[27]);
                        rdo.AMOUNT_DATE_27 = AmountOfDepartment(department, Date[27], Date[28]);
                        rdo.AMOUNT_DATE_28 = AmountOfDepartment(department, Date[28], Date[29]);
                        rdo.AMOUNT_DATE_29 = AmountOfDepartment(department, Date[29], Date[30]);
                        rdo.AMOUNT_DATE_30 = AmountOfDepartment(department, Date[30], Date[31]);
                        ListRdo.Add(rdo);
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {


            if (((Mrs00284Filter)reportFilter).TIME_FROM > 0)
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00284Filter)reportFilter).TIME_FROM));
                DateTime DateFrom = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(((Mrs00284Filter)reportFilter).TIME_FROM ?? 0);
                dicSingleTag.Add("DATE_0", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                // Một khoảng thời gian.
                // 1 ngày
                TimeSpan aInterval = new System.TimeSpan(1, 0, 0, 0);

                // Thêm khoảng thời gian.
                DateFrom = DateFrom.Add(aInterval);

                dicSingleTag.Add("DATE_1", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_2", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_3", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_4", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_5", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_6", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_7", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_8", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_9", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_10", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_11", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_12", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_13", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_14", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_15", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_16", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_17", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_18", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_19", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_20", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_21", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_22", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_23", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_24", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_25", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_26", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_27", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_28", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_29", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
                dicSingleTag.Add("DATE_30", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateFrom));
                DateFrom = DateFrom.Add(aInterval);
            }
            if (((Mrs00284Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00284Filter)reportFilter).TIME_TO));
            }



            objectTag.AddObjectData(store, "Report", ListRdo);

        }

        #region số BN ĐTNT trong ngày đó
        private Decimal AmountOfDepartment(HIS_DEPARTMENT department, long TimeFrom, long TimeTo)
        {
            Decimal result = 0;
            try
            {

                //Nếu hồ sơ điều trị thỏa mãn là điều trị nội trú trong khoa thì thêm vào
                result = listTreatment.Where(o => TreatInDepartment(department, o, TimeFrom, TimeTo)).ToList().Count;
                if (result > 0)
                    Inventec.Common.Logging.LogSystem.Info("departmentId:" + department.ID.ToString() + String.Join(", ", listTreatment.Where(o => TreatInDepartment(department, o, TimeFrom, TimeTo)).Select(p => p.TREATMENT_CODE).ToList()));
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);

            }
            return result;

        }
        #endregion

        #region có điều trị nội trú trong khoa không
        private bool TreatInDepartment(HIS_DEPARTMENT department, V_HIS_TREATMENT treatment, long TimeFrom, long TimeTo)
        {
            bool result = false;
            try
            {
                if (TimeFrom > ((Mrs00284Filter)reportFilter).TIME_TO) return false;
                result = listDepartmentTran.Where(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_ID == department.ID && o.TREATMENT_ID == treatment.ID && IsTreatIn(o, listPatientTypeAlter) && IsStayingDepartment(o, NextDepartment(o), TimeFrom, TimeTo)).ToList().Count > 0;

            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;

        }
        #endregion

        private bool IsTreatIn(HIS_DEPARTMENT_TRAN o, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            bool Result = false;
            try
            {

                var listInTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (p.LOG_TIME < NextDepartment(o).DEPARTMENT_IN_TIME || NextDepartment(o).DEPARTMENT_IN_TIME <= 0)).ToList();
                if (IsNotNullOrEmpty(listInTreat))
                {

                    Result = listInTreat.OrderBy(p => p.LOG_TIME).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                Result = false;
            }
            return Result;
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return listDepartmentTran.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }

        private bool IsStayingDepartment(HIS_DEPARTMENT_TRAN inDepartment, HIS_DEPARTMENT_TRAN outDepartment, long TimeFrom, long TimeTo)
        {
            bool result = false;
            try
            {
                // ra viện sau TimeFrom và vào viện trước TimeTo
                result = inDepartment.DEPARTMENT_IN_TIME <= TimeTo && (outDepartment.DEPARTMENT_IN_TIME >= TimeFrom || outDepartment.DEPARTMENT_IN_TIME == 0) ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
