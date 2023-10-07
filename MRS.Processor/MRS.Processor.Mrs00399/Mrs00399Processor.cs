using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBedLog;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatientTypeAlter; 
using AutoMapper; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServ; 

namespace MRS.Processor.Mrs00399
{
    public class Mrs00399Processor : AbstractProcessor
    {
        Mrs00399Filter filter = null; 
        private List<Mrs00399RDO> ListRdo = new List<Mrs00399RDO>(); 
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV> ListSereServ3s = new List<V_HIS_SERE_SERV>(); 
        List<MOS.EFMODEL.DataModels.V_HIS_BED_LOG> bedLogs; 

        public Mrs00399Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00399Filter); 
        }

        //20170801000000
        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.filter = ((Mrs00399Filter)reportFilter); 
                //tinh ngay bat dau va ket thuc cua thang
                //long startDayOfMonth = this.filter.MONTH; 
                //DateTime? startDayOfMonthDt = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(startDayOfMonth); 
                //DateTime? endDayOfMonthDt = new DateTime(startDayOfMonthDt.Value.Year, startDayOfMonthDt.Value.Month + 1, 1).AddDays(-1); 
                //long? endDayOfMonth = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(endDayOfMonthDt); 

                // get bedlog
                HisBedLogViewFilterQuery hisBedLogFilter = new HisBedLogViewFilterQuery();
                hisBedLogFilter.BED_ID = this.filter.BED_ID;
                hisBedLogFilter.BED_ROOM_ID = this.filter.EXACT_BED_ROOM_ID; 
                bedLogs = new HisBedLogManager(paramGet).GetView(hisBedLogFilter); 
                if (bedLogs == null || bedLogs.Count == 0)
                {
                    return false; 
                }
                List<long> serviceReqIds = bedLogs.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList(); 


                //Yeu cau
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.INTRUCTION_TIME_FROM = ((long)(this.filter.MONTH / 100000000)) * 100000000;
                reqFilter.INTRUCTION_TIME_TO = ((long)(this.filter.MONTH / 100000000)) * 100000000+31235959; 
                reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G; 
                reqFilter.IDs = serviceReqIds; 
                var ListServiceReq = new HisServiceReqManager().Get(reqFilter); 
                dicServiceReq = ListServiceReq.ToDictionary(o => o.ID); 

                //Yc - dv

                if (IsNotNullOrEmpty(serviceReqIds))
                {
                    var skip = 0; 
                    while (serviceReqIds.Count - skip > 0)
                    {
                        var listIDs = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery filterSs = new HisSereServViewFilterQuery(); 
                        filterSs.SERVICE_REQ_IDs = listIDs; 
                        var listSereServSub = new HisSereServManager(paramGet).GetView(filterSs); 
                        ListSereServ3s.AddRange(listSereServSub); 
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao Mrs00399: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.filter), this.filter)); 

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00399"); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private long GetDayInMonth(long time)
        {
            long result = 0; 
            try
            {
                if (time > 0)
                {
                    //20171208235959
                    string timeStr = time.ToString(); 
                    string day = timeStr.Substring(6, 2); 
                    result = Inventec.Common.TypeConvert.Parse.ToInt64(day); 
                }
            }
            catch (Exception ex)
            {
                result = 0; 
                Inventec.Common.Logging.LogSystem.Warn(ex); 
            }
            return result; 
        }

        private void ProcessRdos(List<V_HIS_SERE_SERV> sereServs)
        {
            try
            {
                var sereServGroupByTreatment = sereServs.GroupBy(o => o.TDL_TREATMENT_ID).ToList(); 

                //set amount
                foreach (var group in sereServGroupByTreatment)
                {
                    
                    var subGroup = group.ToList<V_HIS_SERE_SERV>(); 
                    Mrs00399RDO rdo = new Mrs00399RDO(); 
                    
                    if (subGroup != null && subGroup.Count > 0)
                    {
                        foreach (var item in subGroup)
                        {
                            if (!dicServiceReq.ContainsKey(item.SERVICE_REQ_ID ?? 0)) continue; 
                            rdo.VIR_PATIENT_NAME = dicServiceReq[item.SERVICE_REQ_ID ?? 0].TDL_PATIENT_NAME; 
                            long InstructionDay = GetDayInMonth(dicServiceReq[item.SERVICE_REQ_ID ?? 0].INTRUCTION_TIME); 
                            rdo.VIR_TOTAL_PRICE += (item.VIR_TOTAL_PRICE ?? 0); 
                            rdo.VIR_TOTAL_PATIENT_PRICE += (item.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                            rdo.VIR_TOTAL_HEIN_PRICE += (item.VIR_TOTAL_HEIN_PRICE ?? 0); 

                            if (InstructionDay == 1)
                            {
                                rdo.DAY1 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 2)
                            {
                                rdo.DAY2 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 3)
                            {
                                rdo.DAY3 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 4)
                            {
                                rdo.DAY4 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 5)
                            {
                                rdo.DAY5 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 6)
                            {
                                rdo.DAY6 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 7)
                            {
                                rdo.DAY7 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 8)
                            {
                                rdo.DAY8 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 9)
                            {
                                rdo.DAY9 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 10)
                            {
                                rdo.DAY10 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 11)
                            {
                                rdo.DAY11 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 12)
                            {
                                rdo.DAY12 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 13)
                            {
                                rdo.DAY13 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 14)
                            {
                                rdo.DAY14 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 15)
                            {
                                rdo.DAY15 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 16)
                            {
                                rdo.DAY16 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 17)
                            {
                                rdo.DAY17 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 18)
                            {
                                rdo.DAY18 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 19)
                            {
                                rdo.DAY19 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 20)
                            {
                                rdo.DAY20 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 21)
                            {
                                rdo.DAY21 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 22)
                            {
                                rdo.DAY22 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 23)
                            {
                                rdo.DAY23 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 24)
                            {
                                rdo.DAY24 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 25)
                            {
                                rdo.DAY25 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 26)
                            {
                                rdo.DAY26 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 27)
                            {
                                rdo.DAY27 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 28)
                            {
                                rdo.DAY28 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 29)
                            {
                                rdo.DAY29 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 30)
                            {
                                rdo.DAY30 = item.AMOUNT; 
                            }
                            else if (InstructionDay == 31)
                            {
                                rdo.DAY31 = item.AMOUNT; 
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("Khong lay duoc ngay chi dinh"); 
                            }
                        }
                    }
                    this.ListRdo.Add(rdo); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                ProcessRdos(this.ListSereServ3s); 
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
            if (((Mrs00399Filter)reportFilter).MONTH > 0)
            {
                dicSingleTag.Add("MOUNT_USE_STR", Inventec.Common.DateTime.Convert.TimeNumberToMonthString(((Mrs00399Filter)reportFilter).MONTH)); 
            }

            if (bedLogs != null && bedLogs.Count > 0)
            {
                var bedLog = bedLogs.FirstOrDefault(); 
                dicSingleTag.Add("BED_NAME", bedLog.BED_NAME); 
            }
            bool exportSuccess = true; 
            objectTag.AddObjectData(store, "Report", ListRdo); 
            exportSuccess = exportSuccess && store.SetCommonFunctions(); 
        }

    }
}
