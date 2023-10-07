
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00738
{
    class Mrs00737Processor : AbstractProcessor
    {
        List<Mrs00738RDO> ListRdo = new List<Mrs00738RDO>();
        List<TREATMENT_BED_LOG> ListTreatmentBedLog = new List<TREATMENT_BED_LOG>();
        List<SDA_EVENT_LOG> listEvent = new List<SDA_EVENT_LOG>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<SDA_EVENT_LOG> ListEventLog = new List<SDA_EVENT_LOG>();
        Mrs00738Filter filter = null;

        public Mrs00737Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00738Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00738Filter)this.reportFilter;
                
                ListTreatmentBedLog = new ManagerSql().GetTreatmentAndBed(filter);
                
                ListEventLog = new ManagerSql().GetEventLog(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;

        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (ListTreatmentBedLog != null && ListTreatmentBedLog.Count > 0)
                {
                    
                    foreach (var item in ListTreatmentBedLog)
                    {
                        Mrs00738RDO rdo = new Mrs00738RDO();
                        //lấy ra danh sách của bệnh nhân sau khi vào khoa có thời gian bắt đầu sử dụng giường = thời gian kết thúc sự dụng giường trước đó
                        var listCheck = ListTreatmentBedLog.Where(p => p.TREATMENT_CODE == item.TREATMENT_CODE
                                                                    && p.START_TIME < item.START_TIME).OrderBy(p => p.START_TIME).LastOrDefault();


                        string BED_CODE_NEXT = item.BED_CODE; //giường trước đó tính theo thời điểm hiện tại của vòng lặp
                        string BED_CODE_PREVIOUS = "";
                        if (listCheck != null)
                        {
                            BED_CODE_PREVIOUS = listCheck.BED_CODE;
                            if (listCheck.FINISH_TIME == item.START_TIME)
                            {
                                if (BED_CODE_NEXT != BED_CODE_PREVIOUS)
                                {
                                    rdo.DESCRIPTION = "Chuyển giường: " + item.TDL_PATIENT_NAME + ", " + item.TDL_PATIENT_DOB.ToString().Substring(0, 4) + ", " + item.TDL_PATIENT_GENDER_NAME + ", " + item.TREATMENT_CODE + "   " + BED_CODE_PREVIOUS + " -> " + BED_CODE_NEXT;
                                }
                                else
                                {
                                    rdo.DESCRIPTION = "Gia hạn giường: " +
                                                          item.TDL_PATIENT_NAME + ", " +
                                                          item.TDL_PATIENT_DOB.ToString().Substring(0, 4) + ", " +
                                                          item.TDL_PATIENT_GENDER_NAME + ", " + item.TREATMENT_CODE + "  " +
                                                          listCheck.BED_CODE + " " + item.BED_NAME + "  " +
                                                          Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listCheck.FINISH_TIME ?? 0) +
                                                          " -> " +
                                                          Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FINISH_TIME ?? 0);
                                }
                            }
                            else
                            {
                                rdo.DESCRIPTION = "Tiếp nhận vào giường: " +
                                                          item.TDL_PATIENT_NAME + ", " +
                                                          item.TDL_PATIENT_DOB.ToString().Substring(0, 4) + ", " +
                                                          item.TDL_PATIENT_GENDER_NAME + ", " + item.TREATMENT_CODE + "  " +
                                                  "Giường: " + item.BED_CODE + " - " + item.BED_NAME;
                            }

                        }//giường theo thời điểm tiếp theo của vòng lặp trong listCheck
                        else
                        {
                            rdo.DESCRIPTION = "Tiếp nhận vào giường: " +
                                                          item.TDL_PATIENT_NAME + ", " +
                                                          item.TDL_PATIENT_DOB.ToString().Substring(0, 4) + ", " +
                                                          item.TDL_PATIENT_GENDER_NAME + ", " + item.TREATMENT_CODE + "  " +
                                                  "Giường: " + item.BED_CODE + " - " + item.BED_NAME;
                        }


                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.TDL_PATIENT_CODE = item.TDL_PATIENT_CODE;
                        rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.START_TIME = item.START_TIME;
                        rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FINISH_TIME ?? 0);
                        rdo.DEPARTMENT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.DEPARTMENT_IN_TIME ?? 0);
                        rdo.BED_NAME = item.BED_NAME;
                        rdo.BED_ROOM_NAME = item.BED_ROOM_NAME;
                        rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.START_TIME);
                        rdo.CREATOR = item.CREATOR;
                        if (!string.IsNullOrEmpty(item.BED_NAME))
                        {
                            string treatmentCode = "TREATMENT_CODE: " + item.TREATMENT_CODE;
                            string bedRoomName = "Buồng bệnh: " + item.BED_ROOM_NAME;
                            string bedRoomName1 = "Buồng: " + item.BED_ROOM_NAME;
                            var listEvent = ListEventLog.Where(p => (p.DESCRIPTION.Contains(treatmentCode) && p.DESCRIPTION.Contains(bedRoomName)) || (p.DESCRIPTION.Contains(treatmentCode) && p.DESCRIPTION.Contains(bedRoomName1))).OrderBy(p => p.CREATE_TIME).ToList() ?? new List<SDA_EVENT_LOG>();
                            var descriptionTransferDepa = listEvent.Where(p => p.CREATE_TIME >= item.DEPARTMENT_IN_TIME).FirstOrDefault() ?? new SDA_EVENT_LOG();
                            if (descriptionTransferDepa != null)
                            {
                                rdo.LOGIN_NAME = descriptionTransferDepa.LOGIN_NAME;
                                rdo.IP = descriptionTransferDepa.IP;
                            }
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));


                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(p => p.TDL_PATIENT_CODE).ThenBy(p => p.DEPARTMENT_TIME_STR).ThenBy(p => p.START_TIME).ThenBy(p => p.FINISH_TIME_STR).ToList());

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }


}
