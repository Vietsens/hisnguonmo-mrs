using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisCashierRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00293
{
    class Mrs00293Processor : AbstractProcessor
    {
        Mrs00293Filter castFilter = null;

        public Mrs00293Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<Mrs00293RDO> listSS = new List<Mrs00293RDO>();

        Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>>();

        Dictionary<long, HIS_SERVICE> dicParentService = new Dictionary<long, HIS_SERVICE>();

        string Branch_Name = "";

        public override Type FilterType()
        {
            return typeof(Mrs00293Filter);
        }

        protected override bool GetData()///
        {
            bool valid = true;
            try
            {
                this.castFilter = (Mrs00293Filter)this.reportFilter;
                
                listSS = new Mrs00293RDOManager().GetMrs00293RDO(this.castFilter);
                
                var listTreatmentId = listSS.Select(o => o.TreatmentId ?? 0).Distinct().ToList();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var listHisPatientTypeAlter = GetPatientTypeAlter(listTreatmentId);
                    dicPatientTypeAlter = listHisPatientTypeAlter.GroupBy(g => g.TREATMENT_ID).ToDictionary(p=>p.Key,q=>q.ToList());
                }

                //danh sách dịch vụ cha
                var listService = GetService();
                var parentIds = listService.Select(o => o.PARENT_ID ?? 0).Distinct().ToList();
                var parentService = listService.Where(o => parentIds.Contains(o.ID)).ToList();
                dicParentService = listService.GroupBy(g => g.ID).ToDictionary(p => p.Key, q => parentService.FirstOrDefault(o => o.ID == q.First().PARENT_ID));

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private List<HIS_SERVICE> GetService()
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            try
            {
                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                result = new HisServiceManager().Get(serviceFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_SERVICE>();
            }
           
            return result;
        }

        private List<HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(List<long> listTreatmentId)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = new List<HIS_PATIENT_TYPE_ALTER>();
            try
            {
                int skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listId = listTreatmentId.Skip(skip).Take(500).ToList();
                    skip = skip + 500;

                    //chuyen doi tuong
                    HisPatientTypeAlterFilterQuery patyAlterFilter = new HisPatientTypeAlterFilterQuery();
                    patyAlterFilter.TREATMENT_IDs = listId;
                    var listPatyAlter = new HisPatientTypeAlterManager().Get(patyAlterFilter);

                    if (IsNotNullOrEmpty(listPatyAlter))
                    {
                        result.AddRange(listPatyAlter);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_PATIENT_TYPE_ALTER>();
            }

            return result;
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {

                ProcessDetailData();
                ProcessBranchById();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void ProcessDetailData()
        {
            foreach (var item in listSS)
            {
                item.TDL_INTRUCTION_TIME = item.TRANSACTION_TIME;
                item.INSTRUCTION_DATE = item.TRANSACTION_DATE;

                item.INSTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.TDL_INTRUCTION_TIME);
                item.INSTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_INTRUCTION_TIME);
                
                item.TDL_REQUEST_ROOM_NAME =(HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID== item.TDL_REQUEST_ROOM_ID)??new V_HIS_ROOM()).ROOM_NAME;
                item.TDL_REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                item.REQUEST_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                item.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
               
                if (item.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    item.MALE_AGE = Inventec.Common.DateTime.Calculation.Age(item.DOB);
                }
                else
                {
                    item.FEMALE_AGE = Inventec.Common.DateTime.Calculation.Age(item.DOB);
                }
                var listPatientTypeAlterSub = dicPatientTypeAlter.ContainsKey(item.TreatmentId ?? 0) ? dicPatientTypeAlter[item.TreatmentId ?? 0] : new List<HIS_PATIENT_TYPE_ALTER>();
                var patyAlterLast = listPatientTypeAlterSub.OrderBy(p => p.LOG_TIME).ThenBy(p => p.ID).LastOrDefault(o => o.TREATMENT_ID == item.TreatmentId);
                if (patyAlterLast != null)
                {
                    if (patyAlterLast.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        item.HEIN_CARD_NUMBER = patyAlterLast.HEIN_CARD_NUMBER;
                        item.HEIN_CARD_FROM_TIME = patyAlterLast.HEIN_CARD_FROM_TIME;
                        item.HEIN_CARD_TO_TIME = patyAlterLast.HEIN_CARD_TO_TIME;
                        item.HEIN_MEDI_ORG_CODE = patyAlterLast.HEIN_MEDI_ORG_CODE;
                        item.HEIN_MEDI_ORG_NAME = patyAlterLast.HEIN_MEDI_ORG_NAME;
                        item.HEIN_ADDRESS = patyAlterLast.ADDRESS;
                    }
                }
                var parentService = dicParentService.ContainsKey(item.SERVICE_ID) ? dicParentService[item.SERVICE_ID] : new HIS_SERVICE();
                if (parentService != null)
                {
                    item.PARENT_SERVICE_CODE = parentService.SERVICE_CODE;
                    item.PARENT_SERVICE_NAME = parentService.SERVICE_NAME;
                }
            }
        }

        private void ProcessBranchById()
        {
            try
            {
                var branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID);
                if (branch != null)
                {
                    this.Branch_Name = branch.BRANCH_NAME;
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
                Inventec.Common.Logging.LogSystem.Info("listRdo" + listSS.Count);
                listSS = listSS.OrderBy(o => o.INSTRUCTION_DATE).ThenBy(o => o.TreatmentId).ToList();
                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM??0));
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO??0));
                var ParentService = new HisServiceManager().GetById(castFilter.SERVICE_ID ?? 0);
                var ServiceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == castFilter.SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE();
                if (ParentService != null)
                {
                    dicSingleTag.Add("SERVICE_CODE", ParentService.SERVICE_CODE);
                    dicSingleTag.Add("SERVICE_NAME", ParentService.SERVICE_NAME);
                    dicSingleTag.Add("SERVICE_TYPE_NAME", ServiceType.SERVICE_TYPE_NAME);
                }
                else
                {
                    dicSingleTag.Add("SERVICE_TYPE_NAME", ServiceType.SERVICE_TYPE_NAME);
                }

                dicSingleTag.Add("BRANCH_NAME", this.Branch_Name);
                objectTag.AddObjectData(store, "Report", listSS);
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new MergeManyRowData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
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
