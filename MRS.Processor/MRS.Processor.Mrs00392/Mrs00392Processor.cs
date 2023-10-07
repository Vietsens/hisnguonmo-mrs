using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMediStock;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisExpMest; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00392
{
    class Mrs00392Processor : AbstractProcessor
    {
        Mrs00392Filter castFilter = null; 

        List<Mrs00392RDO> listRdo = new List<Mrs00392RDO>(); 

        public Mrs00392Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatment = null; 
        int treatment_count = 0; 

        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00392Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00392Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Info("Mrs00392: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE; 
                treatmentFilter.ORDER_DIRECTION = "ASC"; 
                treatmentFilter.ORDER_FIELD = "TREATMENT_CODE"; 
                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00392"); 
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
                    Dictionary<long, V_HIS_EXP_MEST> dicPresscription = new Dictionary<long, V_HIS_EXP_MEST>(); 
                    List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>(); 
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
                        HisServiceReqFilterQuery sReqFilter = new HisServiceReqFilterQuery(); 
                        sReqFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs; 
                        sReqFilter.SERVICE_REQ_TYPE_IDs = new List<long>(){
                            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK,
                            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM,
                            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT,
                            IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT}; 
                        sReqFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT; 
                        var hisServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(sReqFilter); 
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00392"); 
                        }
                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            var Groups = listPatientTypeAlter.GroupBy(g => g.TREATMENT_ID).ToList(); 
                            foreach (var group in Groups)
                            {
                                var currentPaty = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().First(); 
                                if (currentPaty.PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    continue; 
                                }
                                if (currentPaty.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                {
                                    continue; 
                                }
                                dicPatientTypeAlter[currentPaty.TREATMENT_ID] = currentPaty; 
                            }
                        }

                        if (IsNotNullOrEmpty(hisServiceReqs))
                        {
                            foreach (var item in hisServiceReqs)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    continue; 
                                if (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                    continue; 
                                listServiceReq.Add(item); 
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (listServiceReq.Count > 0)
                    {
                        start = 0; 
                        count = listServiceReq.Count; 
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            var listId = listServiceReq.Skip(start).Take(limit).Select(s => s.ID).ToList(); 
                            HisExpMestViewFilterQuery presFilter = new HisExpMestViewFilterQuery(); 
                            presFilter.SERVICE_REQ_IDs = listId; 
                            var listPres = new HisExpMestManager(paramGet).GetView(presFilter); 
                            if (paramGet.HasException)
                            {
                                throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00392"); 
                            }
                            if (IsNotNullOrEmpty(listPres))
                            {
                                foreach (var item in listPres)
                                {
                                    if (!MANAGER.Config.HisMediStockTypeCFG.MEDI_STOCK_ID_NGOAI_TRU.Contains(item.MEDI_STOCK_ID))
                                        continue; 
                                    dicPresscription[item.TDL_TREATMENT_ID.Value] = item; 
                                }
                            }
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        }
                    }
                    foreach (var treatment in listTreatment)
                    {
                        if (!dicPatientTypeAlter.ContainsKey(treatment.ID))
                            continue; 
                        if (dicPresscription.ContainsKey(treatment.ID))
                            continue; 
                        listTreatmentCode.Add(treatment.TREATMENT_CODE); 
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
                Mrs00392RDO rdo = new Mrs00392RDO(); 
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
