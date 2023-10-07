using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00390
{
    class Mrs00390Processor : AbstractProcessor
    {
        Mrs00390Filter castFilter = null; 

        List<Mrs00390RDO> listRdo = new List<Mrs00390RDO>(); 

        public Mrs00390Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatment = null; 
        int treatment_count = 0; 

        public override Type FilterType()
        {
            return typeof(Mrs00390Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00390Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("MRS00390: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE; 
                treatmentFilter.ORDER_DIRECTION = "ASC"; 
                treatmentFilter.ORDER_FIELD = "TREATMENT_CODE"; 
                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00390"); 
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
                if (IsNotNullOrEmpty(listTreatment))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>(); 
                    foreach (var item in listTreatment)
                    {
                        dicTreatment[item.ID] = item; 
                    }
                    List<string> listTreatmentCode = new List<string>(); 
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = listTreatment.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = listTreatment.Skip(start).Take(limit).ToList(); 
                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList(); 
                        patyAlterFilter.ORDER_DIRECTION = "DESC"; 
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME"; 
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter); 
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00390"); 
                        }

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            var Groups = listPatientTypeAlter.GroupBy(g => g.TREATMENT_ID).ToList(); 
                            foreach (var group in Groups)
                            {
                                var currentPaty = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().First(); 
                                if (castFilter.PATIENT_TYPE_ID.HasValue && currentPaty.PATIENT_TYPE_ID != castFilter.PATIENT_TYPE_ID.Value)
                                {
                                    continue; 
                                }
                                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs) && !castFilter.TREATMENT_TYPE_IDs.Contains(currentPaty.TREATMENT_TYPE_ID))
                                {
                                    continue; 
                                }
                                listTreatmentCode.Add(dicTreatment[currentPaty.TREATMENT_ID].TREATMENT_CODE); 
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (IsNotNullOrEmpty(listTreatmentCode))
                    {
                        this.ProcessDataDetail(listTreatmentCode); 
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

        void ProcessDataDetail(List<string> ListTreatmentCode)
        {
            treatment_count = ListTreatmentCode.Count; 
            int index = treatment_count / 8; 
            int soDu = treatment_count % 8; 
            int index1 = index; 
            int index2 = index; 
            int index3 = index; 
            int index4 = index; 
            int index5 = index; 
            int index6 = index; 
            int index7 = index; 
            int index8 = index; 
            if (soDu > 0)
            {
                for (int i = 0;  i < soDu;  i++)
                {
                    switch (i)
                    {
                        case 0:
                            index1++; 
                            break; 
                        case 1:
                            index2++; 
                            break; 
                        case 2:
                            index3++; 
                            break; 
                        case 3:
                            index4++; 
                            break; 
                        case 4:
                            index5++; 
                            break; 
                        case 5:
                            index6++; 
                            break; 
                        case 6:
                            index7++; 
                            break; 
                        default:
                            break; 
                    }
                }
            }

            int start = 0; 
            var listCode_1 = ListTreatmentCode.Skip(start).Take(index1).ToList(); 
            start += index1; 
            var listCode_2 = ListTreatmentCode.Skip(start).Take(index2).ToList(); 
            start += index2; 
            var listCode_3 = ListTreatmentCode.Skip(start).Take(index3).ToList(); 
            start += index3; 
            var listCode_4 = ListTreatmentCode.Skip(start).Take(index4).ToList(); 
            start += index4; 
            var listCode_5 = ListTreatmentCode.Skip(start).Take(index5).ToList(); 
            start += index5; 
            var listCode_6 = ListTreatmentCode.Skip(start).Take(index6).ToList(); 
            start += index6; 
            var listCode_7 = ListTreatmentCode.Skip(start).Take(index7).ToList(); 
            start += index7; 
            var listCode_8 = ListTreatmentCode.Skip(start).Take(index8).ToList(); 
            for (int i = 0;  i < index1;  i++)
            {
                Mrs00390RDO rdo = new Mrs00390RDO(); 
                rdo.TREATMENT_CODE_1 = listCode_1[i]; 
                if (i < index2)
                {
                    rdo.TREATMENT_CODE_2 = listCode_2[i]; 
                }
                if (i < index3)
                {
                    rdo.TREATMENT_CODE_3 = listCode_3[i]; 
                }
                if (i < index4)
                {
                    rdo.TREATMENT_CODE_4 = listCode_4[i]; 
                }
                if (i < index5)
                {
                    rdo.TREATMENT_CODE_5 = listCode_5[i]; 
                }
                if (i < index6)
                {
                    rdo.TREATMENT_CODE_6 = listCode_6[i]; 
                }
                if (i < index7)
                {
                    rdo.TREATMENT_CODE_7 = listCode_7[i]; 
                }
                if (i < index8)
                {
                    rdo.TREATMENT_CODE_8 = listCode_8[i]; 
                }
                listRdo.Add(rdo); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.PATIENT_TYPE_ID.HasValue)
                {
                    var patientType = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == castFilter.PATIENT_TYPE_ID.Value); 
                    if (patientType != null)
                    {
                        dicSingleTag.Add("PATIENT_TYPE_NAME", patientType.PATIENT_TYPE_NAME); 
                    }
                }
                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    string TreatmentTypeNames = ""; 
                    foreach (var item in castFilter.TREATMENT_TYPE_IDs)
                    {
                        var treatmentType = MANAGER.Config.HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == item); 
                        if (treatmentType != null)
                        {
                            if (TreatmentTypeNames == "")
                            {
                                TreatmentTypeNames = treatmentType.TREATMENT_TYPE_NAME; 
                            }
                            else
                            {
                                TreatmentTypeNames += ", " + treatmentType.TREATMENT_TYPE_NAME; 
                            }
                        }
                    }
                    dicSingleTag.Add("TREATMENT_TYPE_NAMEs", TreatmentTypeNames); 
                }
                dicSingleTag.Add("TREATMENT_COUNT", treatment_count); 
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
