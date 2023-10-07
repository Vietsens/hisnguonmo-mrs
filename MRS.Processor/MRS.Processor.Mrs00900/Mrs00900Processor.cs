using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPtttCatastrophe;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00900
{
    class Mrs00900Processor : AbstractProcessor
    {
        Mrs00900Filter filter = null;
        List<LIST_FOR_PROCESS> listData = new List<LIST_FOR_PROCESS>();
        List<Mrs00900RDO> listRdo = new List<Mrs00900RDO>();
        List<HIS_TRAN_PATI_TECH> listTranPatiTech = new List<HIS_TRAN_PATI_TECH>();
        List<HIS_TREATMENT_END_TYPE> listTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_TREATMENT_TYPE> listTreatmentType = new List<HIS_TREATMENT_TYPE>();
        List<HIS_PTTT_GROUP> listPtttGroup = new List<HIS_PTTT_GROUP>();
        List<HIS_PTTT_CATASTROPHE> listPtttCatastrophe = new List<HIS_PTTT_CATASTROPHE>();
        List<HIS_PATIENT_TYPE> listPatientType = new List<HIS_PATIENT_TYPE>();
        List<Mrs00900RDO> listService = new List<Mrs00900RDO>();

        long PatientTypeIdFEE = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        long PatientTypeIdBHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        long PatientTypeIdIS_FREE = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
        long PatientTypeIdKSK = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;

        public Mrs00900Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00900Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00900Filter)this.reportFilter;
            bool result = true;
            try
            {
                listData = new ManagerSql().GetListData(filter);
                listTranPatiTech = new ManagerSql().GetHIS_TRAN_PATI_TECH();
                listTreatmentEndType = new HisTreatmentEndTypeManager().Get(new HisTreatmentEndTypeFilterQuery());
                listTreatmentType = new HisTreatmentTypeManager().Get(new HisTreatmentTypeFilterQuery());
                listPtttGroup = new HisPtttGroupManager().Get(new HisPtttGroupFilterQuery());
                listService = new ManagerSql().GetService();
                listPtttCatastrophe = new HisPtttCatastropheManager().Get(new HisPtttCatastropheFilterQuery());
                listPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (listData != null)
                {
                    foreach (var item in listData)
                    {
                        item.AGE = Inventec.Common.DateTime.Calculation.Age(item.TDL_PATIENT_DOB ?? 0);
                        if(item.TDL_PATIENT_TYPE_ID != null)
                        {
                            
                            if (item.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT)
                            {
                                item.TDL_PATIENT_TYPE_NAME = "BHYT";
                            }
                            else if (item.TDL_PATIENT_TYPE_ID == PatientTypeIdKSK)
                            {
                                item.TDL_PATIENT_TYPE_NAME = "KSK";
                            }
                            else if (item.TDL_PATIENT_TYPE_ID == PatientTypeIdFEE)
                            {
                                item.TDL_PATIENT_TYPE_NAME = "VP";
                            }
                            else if (item.TDL_PATIENT_TYPE_ID == PatientTypeIdIS_FREE)
                            {
                                item.TDL_PATIENT_TYPE_NAME = "MP";
                            }
                            else
                            {
                                item.TDL_PATIENT_TYPE_NAME = "DV";
                            }
                            item.TDL_PATIENT_TYPE_NAME_OTHER = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == item.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME.ToLower();
                        }

                        var treatmentEndType = listTreatmentEndType.FirstOrDefault(p => p.ID == item.TREATMENT_END_TYPE_ID);
                        if (treatmentEndType != null)
                        {
                            item.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME.ToLower();
                        }

                        var treatmentType = listTreatmentType.FirstOrDefault(p => p.ID == item.TDL_TREATMENT_TYPE_ID);
                        if (treatmentType != null)
                        {
                            item.TDL_TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                        }

                        var ptttGroup = listPtttGroup.FirstOrDefault(p => p.ID == item.PTTT_GROUP_ID);
                        if (ptttGroup != null)
                        {
                            if (ptttGroup.PTTT_GROUP_NAME.ToLower().Contains("phẫu thuật"))
                            {
                                if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                                {
                                    item.PT_GROUP_NAME = "PT1";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                                {
                                    item.PT_GROUP_NAME = "PT2";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                                {
                                    item.PT_GROUP_NAME = "PT3";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                                {
                                    item.PT_GROUP_NAME = "PTDB";
                                }
                            }
                            else if(ptttGroup.PTTT_GROUP_NAME.ToLower().Contains("thủ thuật"))
                            {
                                if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1)
                                {
                                    item.TT_GROUP_NAME = "TT1";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2)
                                {
                                    item.TT_GROUP_NAME = "TT2";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3)
                                {
                                    item.TT_GROUP_NAME = "TT3";
                                }
                                else if (ptttGroup.ID == HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4)
                                {
                                    item.TT_GROUP_NAME = "TTDB";
                                }
                            }
                        }

                        var service = listService.FirstOrDefault(p => p.SERVICE_ID == item.SERVICE_ID);
                        if (service != null)
                        {
                            if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                            {
                                if (service.PARENT_SERVICE_NAME.ToLower().Contains("sinh hóa"))
                                {
                                    item.CATEGORY_CODE = "XNSH";
                                }
                                else if (service.PARENT_SERVICE_NAME.ToLower().Contains("huyết học"))
                                {
                                    item.CATEGORY_CODE = "XNHH";
                                }
                                else if (service.PARENT_SERVICE_NAME.ToLower().Contains("vi sinh"))
                                {
                                    item.CATEGORY_CODE = "XNVS";
                                }
                                else if (service.PARENT_SERVICE_NAME.ToLower().Contains("giải phẫu bệnh"))
                                {
                                    item.CATEGORY_CODE = "GPBL";
                                }
                            }
                        }

                        var ptttCatastrophe = listPtttCatastrophe.FirstOrDefault(p => p.ID == item.PTTT_CATASTROPHE_ID);
                        if (ptttCatastrophe != null)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            if(listData != null)
            {
                objectTag.AddObjectData(store, "Report", listData);
            }
            else
            {
                objectTag.AddObjectData(store, "Report", new List<LIST_FOR_PROCESS>());
            }
        }
    }
}
