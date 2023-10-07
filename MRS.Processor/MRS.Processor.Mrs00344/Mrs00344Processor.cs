using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
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
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO; 

namespace MRS.Processor.Mrs00344
{
    class Mrs00344Processor : AbstractProcessor
    {
        Mrs00344Filter mrs00344Filter = new Mrs00344Filter(); 
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, List<V_HIS_SERE_SERV>> dicTreatSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>(); 
        List<Mrs00344RDO> lstMrs00344RDO = new List<Mrs00344RDO>(); 
        List<Mrs00344RDO> listRdo = new List<Mrs00344RDO>(); 

        public Mrs00344Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00344Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.mrs00344Filter = (Mrs00344Filter)this.reportFilter; 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu Mrs00344... : " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mrs00344Filter), mrs00344Filter)); 
                CommonParam param = new CommonParam(); 
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = this.mrs00344Filter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = this.mrs00344Filter.TIME_TO; 
                treatmentFilter.IS_PAUSE = true; 
                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentFilter); 
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
                    CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                    int start = 0; 
                    int count = listTreatment.Count; 
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => count), count)); 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listSub = listTreatment.Skip(start).Take(limit).ToList(); 

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList(); 
                        patyAlterFilter.ORDER_DIRECTION = "DESC"; 
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME"; 
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter); 

                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                        ssFilter.TREATMENT_IDs = patyAlterFilter.TREATMENT_IDs; 
                        ssFilter.PATIENT_TYPE_ID = this.mrs00344Filter.SERE_SERV_PATIENT_TYPE_ID; 
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter); 

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh tong lay du lieu MRS00344"); 
                        }
                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            foreach (var item in listPatientTypeAlter)
                            {
                                if (dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID))
                                    continue; 
                                dicCurrentPatyAlter[item.TREATMENT_ID] = item; 
                            }
                        }

                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var item in listSereServ)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_NO_PAY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                {
                                    continue; 
                                }

                                if (!dicTreatSereServ.ContainsKey(item.TDL_TREATMENT_ID.Value))
                                    dicTreatSereServ[item.TDL_TREATMENT_ID.Value] = new List<V_HIS_SERE_SERV>(); 
                                dicTreatSereServ[item.TDL_TREATMENT_ID.Value].Add(item); 
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }


                this.ProcessDataDetail(); 
                this.ProcessSereServRDO(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        void ProcessDataDetail()
        {
            foreach (var treatment in listTreatment)
            {
                if (!treatment.END_DEPARTMENT_ID.HasValue)
                    continue; 
                if (!dicCurrentPatyAlter.ContainsKey(treatment.ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE)); 
                    continue; 
                }
                if (!dicTreatSereServ.ContainsKey(treatment.ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin dich vu: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE)); 
                    continue; 
                }
                var currentPatientType = dicCurrentPatyAlter[treatment.ID]; 
                var hisSereServ = dicTreatSereServ[treatment.ID]; 

                if (this.mrs00344Filter.PATIENT_TYPE_ID.HasValue && currentPatientType.PATIENT_TYPE_ID != this.mrs00344Filter.PATIENT_TYPE_ID.Value)
                    continue; 
                Mrs00344RDO rdo = new Mrs00344RDO(treatment);
                if (currentPatientType.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && treatment.CLINICAL_IN_TIME != null && treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && treatment.OUT_TIME != null)
                {
                    rdo.TOTAL_TREATMENT_DATE = Convert.ToInt64(HIS.Common.Treatment.Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? HIS.Common.Treatment.PatientTypeEnum.TYPE.BHYT : HIS.Common.Treatment.PatientTypeEnum.TYPE.THU_PHI) ?? 0);
                }
                if (rdo.VIR_TOTAL_HEIN_PRICE == null)
                    rdo.VIR_TOTAL_HEIN_PRICE = 0;
                if (rdo.VIR_TOTAL_PATIENT_PRICE == null)
                    rdo.VIR_TOTAL_PATIENT_PRICE = 0;
                if (rdo.VIR_TOTAL_PATIENT_PRICE_BHYT == null)
                    rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = 0;
                if (rdo.VIR_TOTAL_PRICE == null)
                    rdo.VIR_TOTAL_PRICE = 0; 
                this.ProcessDetaiPriceService(rdo, hisSereServ); 
            }
        }

        void ProcessDetaiPriceService(Mrs00344RDO ado, List<V_HIS_SERE_SERV> listSereServ)
        {
            ado.DIC_TOTAL_HEIN_PRICE = new Dictionary<string, decimal>();
            ado.DIC_TOTAL_PATIENT_PRICE_BHYT = new Dictionary<string, decimal>();
            foreach (var itemSS in listSereServ)
            {
                if (itemSS.TDL_HEIN_SERVICE_TYPE_ID != null)
                {
                    if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)//XN
                    {
                        if (ado.VIR_TOTAL_PRICE_TEST == null)
                            ado.VIR_TOTAL_PRICE_TEST = 0; 
                        ado.VIR_TOTAL_PRICE_TEST += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("TEST"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["TEST"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["TEST"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["TEST"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["TEST"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }    
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)//PTTT
                    {
                        if (ado.VIR_TOTAL_PRICE_SURG_MISU == null)
                            ado.VIR_TOTAL_PRICE_SURG_MISU = 0; 
                        ado.VIR_TOTAL_PRICE_SURG_MISU += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("SURG_MISU"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["SURG_MISU"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["SURG_MISU"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["SURG_MISU"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["SURG_MISU"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)//CĐHA - TDCN
                    {
                        if (ado.VIR_TOTAL_PRICE_DIIM_FUEX == null)
                            ado.VIR_TOTAL_PRICE_DIIM_FUEX = 0; 
                        ado.VIR_TOTAL_PRICE_DIIM_FUEX += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("FUEX"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["FUEX"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["FUEX"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["FUEX"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["FUEX"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)//KHAM
                    {
                        if (ado.VIR_TOTAL_PRICE_EXAM == null)
                            ado.VIR_TOTAL_PRICE_EXAM = 0; 
                        ado.VIR_TOTAL_PRICE_EXAM += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("EXAM"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["EXAM"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["EXAM"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["EXAM"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["EXAM"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)//MAU
                    {
                        if (ado.VIR_TOTAL_PRICE_BLOOD == null)
                            ado.VIR_TOTAL_PRICE_BLOOD = 0; 
                        ado.VIR_TOTAL_PRICE_BLOOD += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("BLOOD"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["BLOOD"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["BLOOD"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["BLOOD"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["BLOOD"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT)//GIUONG
                    {
                        if (ado.VIR_TOTAL_PRICE_BED == null)
                            ado.VIR_TOTAL_PRICE_BED = 0; 
                        ado.VIR_TOTAL_PRICE_BED += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("BED"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["BED"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["BED"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["BED"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["BED"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)//Van chuyen
                    {
                        if (ado.VIR_TOTAL_PRICE_TRAN == null)
                            ado.VIR_TOTAL_PRICE_TRAN = 0; 
                        ado.VIR_TOTAL_PRICE_TRAN += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("TRAN"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["TRAN"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["TRAN"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["TRAN"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["TRAN"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)//KTC
                    {
                        if (ado.VIR_TOTAL_PRICE_HIGHTECH == null)
                            ado.VIR_TOTAL_PRICE_HIGHTECH = 0; 
                        ado.VIR_TOTAL_PRICE_HIGHTECH += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("HIGHTECH"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["HIGHTECH"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["HIGHTECH"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["HIGHTECH"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["HIGHTECH"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                    {
                        if (ado.VIR_TOTAL_PRICE_MEDICINE == null)
                            ado.VIR_TOTAL_PRICE_MEDICINE = 0; 
                        ado.VIR_TOTAL_PRICE_MEDICINE += itemSS.VIR_TOTAL_PRICE; 
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("MEDICINE"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["MEDICINE"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["MEDICINE"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["MEDICINE"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["MEDICINE"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                    else if (itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || itemSS.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                    {
                        if (ado.VIR_TOTAL_PRICE_MATERIAL == null)
                            ado.VIR_TOTAL_PRICE_MATERIAL = 0;
                        ado.VIR_TOTAL_PRICE_MATERIAL += itemSS.VIR_TOTAL_PRICE;
                        ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                        ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                        ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                        if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("MATERIAL"))
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["MATERIAL"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["MATERIAL"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                        else
                        {
                            ado.DIC_TOTAL_HEIN_PRICE["MATERIAL"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                            ado.DIC_TOTAL_PATIENT_PRICE_BHYT["MATERIAL"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        }
                    }
                }
                else
                {
                    if (ado.VIR_TOTAL_PRICE_OTHER == null)
                        ado.VIR_TOTAL_PRICE_OTHER = 0; 
                    ado.VIR_TOTAL_PRICE_OTHER += itemSS.VIR_TOTAL_PRICE; 
                    ado.VIR_TOTAL_HEIN_PRICE += itemSS.VIR_TOTAL_HEIN_PRICE; 
                    ado.VIR_TOTAL_PATIENT_PRICE += itemSS.VIR_TOTAL_PATIENT_PRICE;
                    ado.VIR_TOTAL_PATIENT_PRICE_BHYT += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT;
                    ado.VIR_TOTAL_PRICE += itemSS.VIR_TOTAL_PRICE;
                    if (!ado.DIC_TOTAL_HEIN_PRICE.ContainsKey("OTHER"))
                    {
                        ado.DIC_TOTAL_HEIN_PRICE["OTHER"] = itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                        ado.DIC_TOTAL_PATIENT_PRICE_BHYT["OTHER"] = itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                    }
                    else
                    {
                        ado.DIC_TOTAL_HEIN_PRICE["OTHER"] += itemSS.VIR_TOTAL_HEIN_PRICE ?? 0;
                        ado.DIC_TOTAL_PATIENT_PRICE_BHYT["OTHER"] += itemSS.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                    }
                }
            }
            listRdo.Add(ado); 
        }

        void ProcessSereServRDO()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.END_DEPARTMENT_CODE).ThenBy(o => o.TREATMENT_CODE).ToList(); 
                    var Groups = listRdo.GroupBy(g => g.END_DEPARTMENT_ID).ToList(); 
                    var departmentKKB = new List<long>();
                    foreach (var item in HisDepartmentCFG.DEPARTMENTs)
                    {
                        if (HisRoomCFG.HisRooms.Exists(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && item.ID == o.DEPARTMENT_ID))
                        {
                            departmentKKB.Add(item.ID);
                        }
                        
                    }
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00344RDO>(); 
                        Mrs00344RDO rdo = new Mrs00344RDO(); 
                        rdo.END_DEPARTMENT_ID = listSub.First().END_DEPARTMENT_ID; 
                        rdo.END_DEPARTMENT_CODE = listSub.First().END_DEPARTMENT_CODE;
                        rdo.END_DEPARTMENT_NAME = listSub.First().END_DEPARTMENT_NAME;
                        if (departmentKKB.Contains(listSub.First().END_DEPARTMENT_ID ?? 0))
                        {
                            rdo.GROUP_DEPARTMENT_NAME = "KHOA NGOẠI TRÚ";
                        }
                        else
                        {
                            rdo.GROUP_DEPARTMENT_NAME = "KHOA NỘI TRÚ";
                        }

                        rdo.TOTAL_PATIENT_NUMBER = listSub.Count;

                        rdo.TOTAL_TREATMENT_DATE = Convert.ToInt64(listSub.Sum(p => p.TOTAL_TREATMENT_DATE)); 

                        rdo.VIR_TOTAL_HEIN_PRICE = listSub.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.VIR_TOTAL_PRICE_TEST = listSub.Sum(s => s.VIR_TOTAL_PRICE_TEST ?? 0); 
                        rdo.VIR_TOTAL_PRICE_DIIM_FUEX = listSub.Sum(s => s.VIR_TOTAL_PRICE_DIIM_FUEX ?? 0); 
                        rdo.VIR_TOTAL_PRICE_EXAM = listSub.Sum(s => s.VIR_TOTAL_PRICE_EXAM ?? 0); 
                        rdo.VIR_TOTAL_PRICE_SURG_MISU = listSub.Sum(s => s.VIR_TOTAL_PRICE_SURG_MISU ?? 0); 
                        rdo.VIR_TOTAL_PRICE_BLOOD = listSub.Sum(s => s.VIR_TOTAL_PRICE_BLOOD ?? 0); 
                        rdo.VIR_TOTAL_PRICE_MEDICINE = listSub.Sum(s => s.VIR_TOTAL_PRICE_MEDICINE ?? 0); 
                        rdo.VIR_TOTAL_PRICE_MATERIAL = listSub.Sum(s => s.VIR_TOTAL_PRICE_MATERIAL ?? 0); 
                        rdo.VIR_TOTAL_PRICE_TRAN = listSub.Sum(s => s.VIR_TOTAL_PRICE_TRAN ?? 0); 
                        rdo.VIR_TOTAL_PRICE_OTHER = listSub.Sum(s => s.VIR_TOTAL_PRICE_OTHER ?? 0); 
                        rdo.VIR_TOTAL_PRICE_BED = listSub.Sum(s => s.VIR_TOTAL_PRICE_BED ?? 0); 
                        rdo.VIR_TOTAL_PRICE_HIGHTECH = listSub.Sum(s => s.VIR_TOTAL_PRICE_HIGHTECH ?? 0); 
                        rdo.VIR_TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0); 
                        lstMrs00344RDO.Add(rdo); 
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
                dicSingleTag.Add("CREATE_TIME_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(DateTime.Now)); 
                //dicSingleTag.Add("CREATE_TIME_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now)); 
                dicSingleTag.Add("CREATE_DATE_SEPARATE_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateSeparateString(DateTime.Now)); 
                dicSingleTag.Add("CREATE_MONTH_SEPARATE_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToMonthSeparateString(DateTime.Now)); 
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00344Filter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(mrs00344Filter.TIME_TO));
                objectTag.AddObjectData(store, "Report", lstMrs00344RDO);
                objectTag.AddObjectData(store, "Parent", lstMrs00344RDO.GroupBy(o => o.GROUP_DEPARTMENT_NAME).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Parent", "Report", "GROUP_DEPARTMENT_NAME", "GROUP_DEPARTMENT_NAME");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
