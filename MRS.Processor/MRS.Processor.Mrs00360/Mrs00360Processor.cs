using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisExecuteRoom;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00360
{
    class Mrs00360Processor : AbstractProcessor
    {
        Mrs00360Filter castFilter = null; 

        const string REPORT_TYPE_CAT__SIEU_AM_DEN_TRANG = "360SUIM_BW"; 
        const string REPORT_TYPE_CAT__SIEU_AM_MAU = "360SUIM_CL"; 
        const string REPORT_TYPE_CAT__DIEN_TIM = "360ECG"; 
        const string REPORT_TYPE_CAT__DIEN_NAO = "360EEG"; 
        const string REPORT_TYPE_CAT__LUU_HUYET = "360REG"; 
        const string REPORT_TYPE_CAT__LASER = "360LASER"; 

        List<Mrs00360RDO> listRdo = new List<Mrs00360RDO>(); 

        public Mrs00360Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERE_SERV> listSereServ = null; 
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null; 
        List<HIS_EXECUTE_ROOM> listExecuteRoom = null; 

        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__SA_BW = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__SA_CL = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__DT = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__DN = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__LH = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicService__LASER = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>(); 

        Dictionary<long, Mrs00360RDO> dicDepartmentRdo = new Dictionary<long, Mrs00360RDO>(); 
        Dictionary<long, Mrs00360RDO> dicExamRoomRdo = new Dictionary<long, Mrs00360RDO>(); 

        public override Type FilterType()
        {
            return typeof(Mrs00360Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00360Filter)this.reportFilter; 
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu Bao cao MRS00360: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                ssFilter.INTRUCTION_DATE_FROM = castFilter.TIME_FROM; 
                ssFilter.INTRUCTION_DATE_TO = castFilter.TIME_TO; 
                //ssFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT; 
                listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter); 

                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00360"; 
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter); 

                HisExecuteRoomFilterQuery exeRoomFilter = new HisExecuteRoomFilterQuery(); 
                exeRoomFilter.IS_EXAM = true; 
                listExecuteRoom = new MOS.MANAGER.HisExecuteRoom.HisExecuteRoomManager(paramGet).Get(exeRoomFilter); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu bao cao MRS00360"); 
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
                Dictionary<long, HIS_EXECUTE_ROOM> dicExamRoom = new Dictionary<long, HIS_EXECUTE_ROOM>(); 
                if (IsNotNullOrEmpty(listExecuteRoom))
                {
                    foreach (var item in listExecuteRoom)
                    {
                        if (item.IS_EXAM != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue; 
                        dicExamRoom[item.ROOM_ID] = item; 
                    }
                }

                if (IsNotNullOrEmpty(listServiceRetyCat))
                {
                    foreach (var item in listServiceRetyCat)
                    {
                        switch (item.CATEGORY_CODE)
                        {
                            case REPORT_TYPE_CAT__SIEU_AM_DEN_TRANG:
                                dicService__SA_BW[item.SERVICE_ID] = item; 
                                break; 
                            case REPORT_TYPE_CAT__SIEU_AM_MAU:
                                dicService__SA_CL[item.SERVICE_ID] = item; 
                                break; 
                            case REPORT_TYPE_CAT__DIEN_NAO:
                                dicService__DN[item.SERVICE_ID] = item; 
                                break; 
                            case REPORT_TYPE_CAT__DIEN_TIM:
                                dicService__DT[item.SERVICE_ID] = item; 
                                break; 
                            case REPORT_TYPE_CAT__LUU_HUYET:
                                dicService__LH[item.SERVICE_ID] = item; 
                                break; 
                            case REPORT_TYPE_CAT__LASER:
                                dicService__LASER[item.SERVICE_ID] = item; 
                                break; 

                            default:
                                break; 
                        }
                    }
                }

                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var sereServ in listSereServ)
                    {
                        if (sereServ.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            continue; 
                        }
                        if (sereServ.MEDICINE_ID.HasValue || sereServ.MATERIAL_ID.HasValue)
                        {
                            continue; 
                        }
                        Mrs00360RDO rdo = null; 
                        if (dicExamRoom.ContainsKey(sereServ.TDL_REQUEST_ROOM_ID))
                        {
                            if (dicExamRoomRdo.ContainsKey(sereServ.TDL_REQUEST_ROOM_ID))
                            {
                                rdo = dicExamRoomRdo[sereServ.TDL_REQUEST_ROOM_ID]; 
                            }
                            else
                            {
                                rdo = new Mrs00360RDO(); 
                                rdo.REQUEST_DEPARTMENT_ID = sereServ.TDL_REQUEST_ROOM_ID; 
                                rdo.REQUEST_DEPARTMENT_CODE = sereServ.REQUEST_ROOM_CODE; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_ROOM_NAME; 
                                dicExamRoomRdo[sereServ.TDL_REQUEST_ROOM_ID] = rdo; 
                            }
                        }
                        else
                        {
                            if (dicDepartmentRdo.ContainsKey(sereServ.TDL_REQUEST_ROOM_ID))
                            {
                                rdo = dicDepartmentRdo[sereServ.TDL_REQUEST_ROOM_ID]; 
                            }
                            else
                            {
                                rdo = new Mrs00360RDO(); 
                                rdo.REQUEST_DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID; 
                                rdo.REQUEST_DEPARTMENT_CODE = sereServ.REQUEST_DEPARTMENT_CODE; 
                                rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME; 
                                dicDepartmentRdo[sereServ.TDL_REQUEST_ROOM_ID] = rdo; 
                            }
                        }

                        if (rdo != null)
                        {
                            if (dicService__SA_CL.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.SUIM_AMOUNT_BHYT_COLOR += sereServ.AMOUNT; 
                                    rdo.SUIM_TOTAL_PRICE_BHYT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                else
                                {
                                    rdo.SUIM_AMOUNT_ND_COLOR += sereServ.AMOUNT; 
                                    rdo.SUIM_TOTAL_PRICE_ND += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                rdo.SUIM_TOTAL_AMOUNT += sereServ.AMOUNT; 
                            }
                            else if (dicService__SA_BW.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.SUIM_AMOUNT_BHYT_BW += sereServ.AMOUNT; 
                                    rdo.SUIM_TOTAL_PRICE_BHYT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                else
                                {
                                    rdo.SUIM_AMOUNT_ND_BW += sereServ.AMOUNT; 
                                    rdo.SUIM_TOTAL_PRICE_ND += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                rdo.SUIM_TOTAL_AMOUNT += sereServ.AMOUNT; 
                            }
                            else if (dicService__DT.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.ECG_AMOUNT_BHYT += sereServ.AMOUNT; 
                                }
                                else
                                {
                                    rdo.ECG_AMOUNT_ND += sereServ.AMOUNT; 
                                }
                                rdo.ECG_TOTAL_AMOUNT += sereServ.AMOUNT; 
                                rdo.ECG_TOTAL_PRICE += sereServ.VIR_TOTAL_PRICE ?? 0; 
                            }
                            else if (dicService__DN.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.EEG_AMOUNT_BHYT += sereServ.AMOUNT; 
                                }
                                else
                                {
                                    rdo.EEG_AMOUNT_ND += sereServ.AMOUNT; 
                                }
                                rdo.EEG_TOTAL_AMOUNT += sereServ.AMOUNT; 
                                rdo.EEG_TOTAL_PRICE += sereServ.VIR_TOTAL_PRICE ?? 0; 
                            }
                            else if (dicService__LH.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.REG_AMOUNT_BHYT += sereServ.AMOUNT; 
                                }
                                else
                                {
                                    rdo.REG_AMOUNT_ND += sereServ.AMOUNT; 
                                }
                                rdo.REG_TOTAL_AMOUNT += sereServ.AMOUNT; 
                                rdo.REG_TOTAL_PRICE += sereServ.VIR_TOTAL_PRICE ?? 0; 
                            }
                            else if (dicService__LASER.ContainsKey(sereServ.SERVICE_ID))
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.LASER_AMOUNT_BHYT += sereServ.AMOUNT; 
                                }
                                else
                                {
                                    rdo.LASER_AMOUNT_ND += sereServ.AMOUNT; 
                                }
                                rdo.LASER_TOTAL_AMOUNT += sereServ.AMOUNT; 
                                rdo.LASER_TOTAL_PRICE += sereServ.VIR_TOTAL_PRICE ?? 0; 
                            }
                        }
                    }
                    this.ProcessRdo(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        void ProcessRdo()
        {
            if (dicExamRoomRdo.Count > 0)
            {
                listRdo.AddRange(dicExamRoomRdo.Select(s => s.Value).Where(o => o.ECG_TOTAL_AMOUNT > 0 || o.EEG_TOTAL_AMOUNT > 0 || o.LASER_TOTAL_AMOUNT > 0 || o.REG_TOTAL_AMOUNT > 0 || o.SUIM_TOTAL_AMOUNT > 0).ToList()); 
            }
            if (dicDepartmentRdo.Count > 0)
            {
                listRdo.AddRange(dicDepartmentRdo.Select(s => s.Value).Where(o => o.ECG_TOTAL_AMOUNT > 0 || o.EEG_TOTAL_AMOUNT > 0 || o.LASER_TOTAL_AMOUNT > 0 || o.REG_TOTAL_AMOUNT > 0 || o.SUIM_TOTAL_AMOUNT > 0).ToList()); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
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
