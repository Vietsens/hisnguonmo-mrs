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
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisVitaminA;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisBaby;
using MRS.MANAGER.Core.MrsReport.RDO;

namespace MRS.Processor.Mrs00614
{
    class Mrs00614Processor : AbstractProcessor
    {
        Mrs00614Filter castFilter = null;
        List<Mrs00614RDO> listRdo = new List<Mrs00614RDO>();
        List<Mrs00614RDO> listPrint = new List<Mrs00614RDO>();

        string thisReportTypeCode = "";
        public Mrs00614Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00614Filter);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.YEAR > 0)
                {
                    dicSingleTag.Add("YEAR_STR", castFilter.YEAR.ToString().Substring(0, 4));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listPrint);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00614Filter)this.reportFilter;
                this.listRdo = new Mrs00614RDOManager().GetRdo(this.castFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public static int? CalculateAge_YEAR(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                    if (thang > 0)
                    {
                        tuoi = thang;
                    }
                    else
                    {
                        tuoi = 0;
                    }
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        public static int? CalculateAge_MONTH(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                tuoi = nam * 12 + thang;
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        public static bool? IsYearOld(long ageNumber)
        {
            bool? result = null;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    result = null;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (this.castFilter.YEAR > 0 && this.castFilter.YEAR.ToString().Length == 14)
                {

                }
                Int32 year = Int32.Parse(this.castFilter.YEAR.ToString().Substring(0, 4));
                if (year <= 0)
                {
                    Inventec.Common.Logging.LogSystem.Info("Mrs00614 YEAR is <= 0");
                    return false;
                }

                if (this.listRdo == null || this.listRdo.Count() == 0)
                {
                    Inventec.Common.Logging.LogSystem.Info("Mrs00614 this.listRdo == null || this.listRdo.Count() == 0");
                    return false;
                }

                var groupRdo = this.listRdo.GroupBy(o => o.BRANCH_ID);
                foreach (var itemList in groupRdo)
                {
                    var firtItem = itemList.FirstOrDefault();
                    Mrs00614RDO mrs00614RDO = new Mrs00614.Mrs00614RDO();
                    AutoMapper.Mapper.CreateMap<Mrs00614RDO, Mrs00614RDO>();
                    mrs00614RDO = AutoMapper.Mapper.Map<Mrs00614RDO>(firtItem);

                    for (int i = 1; i <= 12; i++)
                    {
                        int daysInMonth = System.DateTime.DaysInMonth(year, i);
                        long beginDate = Int64.Parse(String.Format("{0:0000}{1:00}{2:00}{3}", year, i, "01", "000000"));
                        long endDate = Int64.Parse(String.Format("{0:0000}{1:00}{2:00}{3}", year, i, daysInMonth, "235959"));

                        if (i == 6 || i == 12)
                        {
                            var rdoInMonth6And12 = itemList.Where(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME >= beginDate && o.EXECUTE_TIME <= endDate);
                            if (rdoInMonth6And12 != null && rdoInMonth6And12.Count() > 0)
                            {
                                var amount6To12 = rdoInMonth6And12.Where(o => o.CASE_TYPE == 1 &&
                                    CalculateAge_MONTH(o.TDL_PATIENT_DOB) != null && 6 <= CalculateAge_MONTH(o.TDL_PATIENT_DOB) && CalculateAge_MONTH(o.TDL_PATIENT_DOB) <= 12);
                                var amount13To36 = rdoInMonth6And12.Where(o => o.CASE_TYPE == 1 && CalculateAge_MONTH(o.TDL_PATIENT_DOB) != null && 13 <= CalculateAge_MONTH(o.TDL_PATIENT_DOB) && CalculateAge_MONTH(o.TDL_PATIENT_DOB) <= 36);

                                var amount5 = rdoInMonth6And12.Where(o => o.CASE_TYPE == 1 && CalculateAge_MONTH(o.TDL_PATIENT_DOB) != null && 36 <= CalculateAge_MONTH(o.TDL_PATIENT_DOB) && CalculateAge_MONTH(o.TDL_PATIENT_DOB) <= 60);
                                var amountMother = rdoInMonth6And12.Where(o => o.CASE_TYPE == 2);

                                if (i == 6)
                                {
                                    if (amount6To12 != null)
                                    {
                                        mrs00614RDO.T61_AMOUNT = amount6To12.Count();
                                    }

                                    if (amount13To36 != null)
                                    {
                                        amount13To36.Count();
                                    }

                                    if (amount5 != null)
                                    {
                                        mrs00614RDO.T63_AMOUNT = amount5.Count();
                                    }

                                    if (amountMother != null)
                                    {
                                        mrs00614RDO.T64_AMOUNT = amountMother.Count();
                                    }

                                }
                                else if (i == 12)
                                {
                                    if (amount6To12 != null)
                                    {
                                        mrs00614RDO.T121_AMOUNT = amount6To12.Count();
                                    }
                                    if (amount13To36 != null)
                                    {
                                        mrs00614RDO.T122_AMOUNT = amount13To36.Count();
                                    }

                                    if (amount5 != null)
                                    {
                                        mrs00614RDO.T123_AMOUNT = amount5.Count();
                                    }

                                    if (amountMother != null)
                                    {
                                        mrs00614RDO.T124_AMOUNT = amountMother.Count();
                                    }
                                }
                            }
                        }
                        else
                        {
                            var rdoInMonth = itemList.Where(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME >= beginDate && o.EXECUTE_TIME <= endDate && o.CASE_TYPE == 2);
                            if (rdoInMonth != null)
                            {
                                decimal amount = rdoInMonth.Count();
                                if (i == 1)
                                {
                                    mrs00614RDO.T1_AMOUNT = amount;
                                }
                                else if (i == 2)
                                {
                                    mrs00614RDO.T2_AMOUNT = amount;
                                }
                                else if (i == 3)
                                {
                                    mrs00614RDO.T3_AMOUNT = amount;
                                }
                                else if (i == 4)
                                {
                                    mrs00614RDO.T4_AMOUNT = amount;
                                }
                                else if (i == 5)
                                {
                                    mrs00614RDO.T5_AMOUNT = amount;
                                }

                                else if (i == 7)
                                {
                                    mrs00614RDO.T7_AMOUNT = amount;
                                }
                                else if (i == 8)
                                {
                                    mrs00614RDO.T8_AMOUNT = amount;
                                }
                                else if (i == 9)
                                {
                                    mrs00614RDO.T9_AMOUNT = amount;
                                }
                                else if (i == 10)
                                {
                                    mrs00614RDO.T10_AMOUNT = amount;
                                }
                                else if (i == 11)
                                {
                                    mrs00614RDO.T11_AMOUNT = amount;
                                }
                            }
                        }
                    }

                    listPrint.Add(mrs00614RDO);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


    }
}
