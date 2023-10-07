using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisSereServDeposit;
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceType; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00365
{
    class Mrs00365Processor : AbstractProcessor
    {
        Mrs00365Filter castFilter = null; 

        public Mrs00365Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<Mrs00365RDO> listRdo = new List<Mrs00365RDO>(); 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();
        V_HIS_SERVICE parentService = null; 
        HIS_SERVICE_TYPE serviceType = null; 
        HIS_BRANCH brand = null; 

        public override Type FilterType()
        {
            return typeof(Mrs00365Filter); 
        }

        protected override bool GetData()
        {
            bool valid = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00365Filter)this.reportFilter; 
                
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                departmentFilter.BRANCH_ID = this.castFilter.BRANCH_ID; 
                listDepartments = new HisDepartmentManager(paramGet).Get(departmentFilter); 

                List<HIS_SERE_SERV_DEPOSIT> listDereDetails = new List<HIS_SERE_SERV_DEPOSIT>(); 
                var listReqDepartments = listDepartments.Select(s => s.ID).ToList();

                HisSereServViewFilterQuery serServFilter = new HisSereServViewFilterQuery(); 
                serServFilter.INTRUCTION_TIME_FROM = this.castFilter.TIME_FROM; 
                serServFilter.INTRUCTION_TIME_TO = this.castFilter.TIME_TO;
                serServFilter.SERVICE_TYPE_ID = this.castFilter.SERVICE_TYPE_ID;
                serServFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID;
                serServFilter.REQUEST_DEPARTMENT_IDs = listReqDepartments; 
                listSereServs = new HisSereServManager(paramGet).GetView(serServFilter); 


                serviceType = new HisServiceTypeManager(paramGet).GetById(this.castFilter.SERVICE_TYPE_ID); 
                brand = new HisBranchManager(paramGet).GetById(this.castFilter.BRANCH_ID??0)??new HIS_BRANCH(); 

                var listSerServIds = listSereServs.Select(s => s.ID).ToList(); 

                var skip_ = 0; 
                while (listSerServIds.Count - skip_ > 0)
                {
                    var listSerServId = listSerServIds.Skip(skip_).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip_ += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    var dereDetailFilter = new HisSereServDepositFilterQuery
                    {
                        SERE_SERV_IDs = listSerServId,
                    }; 
                    var listDereDetail = new MOS.MANAGER.HisSereServDeposit.HisSereServDepositManager(paramGet).Get(dereDetailFilter); 
                    listDereDetails.AddRange(listDereDetail); 
                }

                var listSerServDereDetails = listDereDetails.Select(s => s.SERE_SERV_ID).ToList();
                listSereServs = listSereServs.Where(s => !listSerServDereDetails.Contains(s.ID)).ToList();
                if (castFilter.IS_WHEN_TREATIN == true)
                {
                    var dicPatientTypeAlter = (new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList()) ?? new List<V_HIS_PATIENT_TYPE_ALTER>()).GroupBy(p => p.TREATMENT_ID).ToDictionary(q => q.Key, q => q.ToList());
                    if (IsNotNullOrEmpty(listSereServs))
                    {
                        listSereServs = listSereServs.Where(o => dicPatientTypeAlter.ContainsKey(o.TDL_TREATMENT_ID ?? 0) && treatmentTypeId(o, dicPatientTypeAlter[o.TDL_TREATMENT_ID ?? 0]) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }
                if (castFilter.IS_NOT_WHEN_OUT == true)
                {
                    List<long> treatmentIdInTreat = this.GetTreatmentIdIn(castFilter)?? new List<long>();
                    if (IsNotNullOrEmpty(listSereServs))
                    {
                        listSereServs = listSereServs.Where(o => treatmentIdInTreat.Contains(o.TDL_TREATMENT_ID??0)).ToList();
                    }
                }
                var patientIds = listSereServs.Select(o => o.TDL_PATIENT_ID.Value).Distinct().ToList(); 

                getPatient(patientIds); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }

        private List<long> GetTreatmentIdIn(Mrs00365Filter castFilter)
        {
            List<long> result = null;
            try
            {

                List<LongClass> treatmentIdIns = new List<LongClass>();
                string query = "\n";
                query += "select\n";
                query += "trea.id value\n";
                query += "from his_sere_serv ss\n";
                query += "join his_treatment trea on trea.id=ss.tdl_treatment_id\n";
                query += "where 1=1 and trea.tdl_treatment_type_id in (3,4)\n";
                query += string.Format("and ss.tdl_intruction_time between {0} and {1}\n",castFilter.TIME_FROM,castFilter.TIME_TO);
                if (castFilter.SERVICE_TYPE_ID > 0)
                {
                    query += string.Format("and ss.tdl_service_type_id ={0}", castFilter.SERVICE_TYPE_ID);
                }
                LogSystem.Info(query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<LongClass>(query);
                result = rs.Select(o => o.Value).ToList();
            }
            catch (Exception ex)
            {
                 Inventec.Common.Logging.LogSystem.Error(ex); 
                result = null; 
            }
            return result;
        }
        private void getPatient(List<long> patientIds)
        {
            try
            {
                List<HIS_PATIENT> LisPatient = new List<HIS_PATIENT>(); 
                CommonParam paramGet = new CommonParam(); 
                if (IsNotNullOrEmpty(patientIds))
                {

                    var skip = 0; 
                    while (patientIds.Count - skip > 0)
                    {
                        var listIDs = patientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery()
                        {
                            IDs = listIDs
                        }; 
                        var LisPatientLib = new MOS.MANAGER.HisPatient.HisPatientManager(paramGet).Get(patientFilter); 
                        LisPatient.AddRange(LisPatientLib); 

                    }
                    LisPatient = LisPatient.OrderByDescending(p => p.ID).ToList(); 
                    foreach (var item in LisPatient)
                    {
                        if (!dicPatient.ContainsKey(item.ID)) dicPatient[item.ID] = item; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override bool ProcessData()
        {
            bool valid = true; 
            try
            {
                listRdo.Clear(); 
                if (IsNotNullOrEmpty(listSereServs))
                {
                    HIS_PATIENT patient = null; 
                    foreach (var sereServ in listSereServs)
                    {
                        patient = dicPatient.ContainsKey(sereServ.TDL_PATIENT_ID.Value) ?
                            dicPatient[sereServ.TDL_PATIENT_ID.Value] : new HIS_PATIENT(); 

                        Mrs00365RDO rdo = new Mrs00365RDO(); 
                        rdo.INSTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(sereServ.TDL_INTRUCTION_TIME); 
                        rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE; 
                        rdo.PATIENT_CODE = patient.PATIENT_CODE; 
                        rdo.VIR_PATIENT_NAME = patient.VIR_PATIENT_NAME; 
                        if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.FEMALE_AGE = patient.DOB; 
                        }
                        if (patient.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = patient.DOB; 
                        }
                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER; 
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                        rdo.AMOUNT = sereServ.AMOUNT; 
                        rdo.PRICE = sereServ.PRICE; 
                        rdo.HEIN_PRICE = sereServ.HEIN_PRICE; 
                        rdo.TOTAL_PATIENT_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE; 
                        rdo.TOTAL_HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE; 

                        listRdo.Add(rdo); 
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {

                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                if (parentService != null)
                {
                    dicSingleTag.Add("SERVICE_CODE", parentService.SERVICE_CODE); 
                    dicSingleTag.Add("SERVICE_NAME", parentService.SERVICE_NAME); 
                    dicSingleTag.Add("SERVICE_TYPE_NAME", parentService.SERVICE_TYPE_NAME); 
                }
                else
                {
                    dicSingleTag.Add("SERVICE_TYPE_NAME", serviceType.SERVICE_TYPE_NAME); 
                }
                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    dicSingleTag.Add("REQUEST_DEPARTMENT_NAME",( HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==castFilter.REQUEST_DEPARTMENT_ID)??new HIS_DEPARTMENT()).DEPARTMENT_NAME); 
                }
                dicSingleTag.Add("BRANCH_NAME", brand.BRANCH_NAME); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new MergeManyRowData()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        #region Cac ham phu tro
        private long treatmentTypeId(V_HIS_SERE_SERV thisData, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            long result = 0; 
            try
            {
                if (thisData == null) return result; 
                if (LisPatientTypeAlter == null) return result; 
                var LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.LOG_TIME <= thisData.CREATE_TIME).ToList(); 
                if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                    result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID; 
                else
                {
                    LisPatientTypeAlterSub = LisPatientTypeAlter.Where(o => o.LOG_TIME >= thisData.CREATE_TIME).ToList(); 
                    if (IsNotNullOrEmpty(LisPatientTypeAlterSub))
                        result = LisPatientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID; 
                }
            }

            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex); 

                result = 0; 
            }
            return result; 
        }
        #endregion
    }
    class MergeManyRowData : FlexCel.Report.TFlexCelUserFunction
    {
        long currentInstructionData; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                long instructionDate = Convert.ToInt64(parameters[0]); 
                if (instructionDate == currentInstructionData)
                {
                    return true; 
                }
                else
                {
                    currentInstructionData = instructionDate; 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return false; 
            }
            throw new NotImplementedException(); 
        }
    }
}
