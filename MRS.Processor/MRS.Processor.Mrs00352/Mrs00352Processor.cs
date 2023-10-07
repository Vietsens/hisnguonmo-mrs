using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00352
{
    public class Mrs00352Processor : AbstractProcessor
    {
        Mrs00352Filter castFilter = null; 

        List<Mrs00352RDO> ListRdo = new List<Mrs00352RDO>(); 

        List<HIS_TREATMENT> treatments = new List<HIS_TREATMENT>(); 
        List<HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_SERE_SERV_3> sereServs = new List<V_HIS_SERE_SERV_3>(); 

        List<long> listHeinServiceTypeId; 

        public Mrs00352Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00352Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            CommonParam paramGet = new CommonParam(); 
            try
            {
                castFilter = (Mrs00352Filter)this.reportFilter; 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, MRS00352, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                List<HIS_TREATMENT> tmp = this.GetTreatment(); 

                ///lay du lieu patient_type_alter va treament
                this.GetPatientTypeAlterAndTreatment(tmp, ref this.treatments, ref this.patientTypeAlters); 

                this.sereServs = this.GetSereServ(this.treatments); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu, MRS00352"); 
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
                if (IsNotNullOrEmpty(this.patientTypeAlters))
                {
                    listHeinServiceTypeId = new List<long>(); 
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM); 
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM); 
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL); 
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT); 
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU); 

                    }
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__VTYT)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC); 
                    }

                    GeneralDataByListPatientTypeAlter(this.patientTypeAlters, this.sereServs); 

                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ProcessListRDO(ListRdo); 
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

        private void GeneralDataByListPatientTypeAlter(List<HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters, List<V_HIS_SERE_SERV_3> listSereServ)
        {
            try
            {
                Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>(); 
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    foreach (var hisPatientTypeAlter in hisPatientTypeAlters)
                    {
                        dicPatientTypeAlter[hisPatientTypeAlter.TREATMENT_ID] = hisPatientTypeAlter; 
                    }
                }

                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var sereServ in listSereServ)
                    {
                        if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sereServ.AMOUNT <= 0 || sereServ.TDL_HEIN_SERVICE_TYPE_ID == null)
                            continue; 
                        if (!listHeinServiceTypeId.Contains(sereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                            continue; 

                        Mrs00352RDO rdo = new Mrs00352RDO(); 
                        rdo.SERVICE_ID = sereServ.SERVICE_ID; 
                        rdo.MATERIAL_CODE_DMBYT = sereServ.TDL_HEIN_SERVICE_BHYT_CODE; 
                        rdo.MATERIAL_CODE_DMBYT_1 = sereServ.TDL_MATERIAL_GROUP_BHYT ; 
                        rdo.MATERIAL_STT_DMBYT = sereServ.TDL_HEIN_ORDER; 
                        rdo.MATERIAL_TYPE_NAME_BYT = sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MATERIAL_TYPE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.MATERIAL_QUYCACH_NAME = sereServ.MATERIAL_PACKING_TYPE_NAME;
                        rdo.BID_PACKAGE_CODE = sereServ.MATERIAL_BID_PACKAGE_CODE;
                        rdo.IMP_PRICE = sereServ.MATERIAL_IMP_PRICE;
                        rdo.VIR_PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(sereServ.ORIGINAL_PRICE > 0 ? (sereServ.HEIN_LIMIT_PRICE.HasValue ? (sereServ.HEIN_LIMIT_PRICE.Value / (sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO))) * 100 : (sereServ.PRICE / sereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.MATERIAL_UNIT_NAME = sereServ.SERVICE_UNIT_NAME; 
                        if (dicPatientTypeAlter.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                        {
                            if (dicPatientTypeAlter[sereServ.TDL_TREATMENT_ID ?? 0].TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.AMOUNT_NGOAITRU += sereServ.AMOUNT; 
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU += sereServ.AMOUNT; 
                            }
                            rdo.VIR_TOTAL_PRICE += sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO) * sereServ.AMOUNT;
                            ListRdo.Add(rdo); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private List<Mrs00352RDO> ProcessListRDO(List<Mrs00352RDO> listRDO)
        {
            List<Mrs00352RDO> listCurrent = new List<Mrs00352RDO>(); 
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.IMP_PRICE, o.VIR_PRICE,o.BHYT_PAY_RATE }).ToList(); 
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00352RDO> listsub = group.ToList<Mrs00352RDO>(); 
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00352RDO rdo = new Mrs00352RDO(); 
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID; 
                            rdo.MATERIAL_CODE_DMBYT = listsub[0].MATERIAL_CODE_DMBYT;
                            rdo.MATERIAL_CODE_DMBYT_1 = listsub[0].MATERIAL_CODE_DMBYT_1; 
                            rdo.MATERIAL_STT_DMBYT = listsub[0].MATERIAL_STT_DMBYT; 
                            rdo.MATERIAL_TYPE_NAME_BYT = listsub[0].MATERIAL_TYPE_NAME_BYT;
                            rdo.MATERIAL_TYPE_NAME = listsub[0].MATERIAL_TYPE_NAME;
                            rdo.MATERIAL_QUYCACH_NAME = listsub[0].MATERIAL_QUYCACH_NAME;
                            rdo.BID_PACKAGE_CODE = listsub[0].BID_PACKAGE_CODE;
                            rdo.IMP_PRICE = listsub[0].IMP_PRICE;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE; 
                            rdo.MATERIAL_UNIT_NAME = listsub[0].MATERIAL_UNIT_NAME; 
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU; 
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU; 
                                rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE; 
                            }

                            listCurrent.Add(rdo); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                listCurrent.Clear(); 
            }
            return listCurrent.OrderBy(o => o.MATERIAL_STT_DMBYT).ToList(); 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM)); 
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO)); 
                }
                if (ListRdo.Count > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(ListRdo.Sum(o => o.VIR_TOTAL_PRICE).ToString()) + " đồng"); 
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng"); 
                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SereServ", ListRdo); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private List<HIS_TREATMENT> GetTreatment()
        {
            ///lay du lieu treatment
            HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery(); 
            treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
            treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
            treatmentFilter.IS_PAUSE = true; 
            treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE; 
            return new MOS.MANAGER.HisTreatment.HisTreatmentManager().Get(treatmentFilter); 
        }

        private List<V_HIS_SERE_SERV_3> GetSereServ(List<HIS_TREATMENT> treatments)
        {
            List<V_HIS_SERE_SERV_3> result = new List<V_HIS_SERE_SERV_3>(); 
            int start = 0; 
            int count = treatments.Count; 
            while (count > 0)
            {
                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                var tmp = treatments.Skip(start).Take(limit).ToList(); 

                HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery(); 
                ssFilter.TREATMENT_IDs = tmp.Select(s => s.ID).ToList(); 
                ssFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                List<V_HIS_SERE_SERV_3> sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView3(ssFilter); 
                if (IsNotNullOrEmpty(sereServs))
                {
                    result.AddRange(sereServs); 
                }
                start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
            }
            return result; 
        }

        /// <summary>
        /// Lay patient_type_alter dua vao treatmen_id
        /// </summary>
        /// <param name="treatmentIds"></param>
        /// <returns></returns>
        private void GetPatientTypeAlterAndTreatment(List<HIS_TREATMENT> treatments, ref List<HIS_TREATMENT> treatmentsToUse, ref List<HIS_PATIENT_TYPE_ALTER> patientTypeAlterToUse)
        {
            patientTypeAlterToUse = new List<HIS_PATIENT_TYPE_ALTER>(); 
            treatmentsToUse = new List<HIS_TREATMENT>(); 
            try
            {
                if (IsNotNullOrEmpty(treatments))
                {
                    CommonParam paramGet = new CommonParam(); 
                    int start = 0; 
                    int count = treatments.Count; 
                    List<HIS_PATIENT_TYPE_ALTER> tmp = new List<HIS_PATIENT_TYPE_ALTER>(); 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var lst = treatments.Skip(start).Take(limit).ToList(); 

                        HisPatientTypeAlterFilterQuery filter = new HisPatientTypeAlterFilterQuery(); 
                        filter.TREATMENT_IDs = lst.Select(o => o.ID).ToList(); 
                        filter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                        List<HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).Get(filter); 

                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            tmp.AddRange(patientTypeAlters); 
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }

                    if (IsNotNullOrEmpty(tmp))
                    {
                        foreach (HIS_TREATMENT treatment in treatments)
                        {
                            HIS_PATIENT_TYPE_ALTER p = tmp
                                .Where(o => o.TREATMENT_ID == treatment.ID)
                                .OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).FirstOrDefault(); 
                            if (p != null)
                            {
                                patientTypeAlterToUse.Add(p); 
                                treatmentsToUse.Add(treatment); 
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
    }
}
