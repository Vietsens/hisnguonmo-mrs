using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00116
{
    public class Mrs00116Processor : AbstractProcessor
    {
        List<Mrs00116RDO> ListSereServRdo = new List<Mrs00116RDO>();
        Mrs00116Filter CastFilter = null;
        List<string> _listDate = new List<string>();
        long _totalPatientReport;
        List<V_HIS_DEPARTMENT_TRAN> listHisDepartmentTrans;
        List<HIS_DEPARTMENT> listHisDepartments;

        public Mrs00116Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00116Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00116Filter)this.reportFilter;
                var paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPARTMENT_TRAN, filter: " +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));
                var metyFilter = new HisDepartmentTranViewFilterQuery
                {
                    DEPARTMENT_IDs = CastFilter.DEPARTMENT_IDs,
                    DEPARTMENT_IN_TIME_FROM = CastFilter.TIME_FROM,
                    DEPARTMENT_IN_TIME_TO = CastFilter.TIME_TO
                };
                listHisDepartmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(metyFilter);
                var lstepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
                lstepartmentTran.AddRange(listHisDepartmentTrans);
                var listHisDepartmentTransId = lstepartmentTran.Select(s => s.DEPARTMENT_ID).Distinct().ToList();

                List<long> iD = new List<long>();
                if (CastFilter.DEPARTMENT_IDs != null && CastFilter.DEPARTMENT_IDs.Count > 0)
                {
                    iD = CastFilter.DEPARTMENT_IDs.Where(s => !listHisDepartmentTransId.Contains(s)).ToList();
                }
                else
                {
                    iD = listHisDepartmentTransId;
                }

                var metyFilters = new HisDepartmentFilterQuery
                {
                    IDs = iD
                };
                listHisDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilters);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu HIS_DEPARTMENT_TRAN, MRS00116." +
                        Inventec.Common.Logging.LogUtil.TraceData(
                            Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterDataDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> listHisDepartment, List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT> listDepartments)
        {
            _totalPatientReport = listHisDepartment.Count;
            AddDateTime();
            //Thêm các khoa chưa có trong list dữ liệu trả về khi lọc theo ngày (không có dữ liệu chỉ thêm khoa vào báo cáo)
            //=====================================================
            var listVirtualDepartment = new List<V_HIS_DEPARTMENT_TRAN>();
            var departmentIds = listDepartments.Select(s => s.ID).ToList();
            var departmentNames = listDepartments.Select(s => s.DEPARTMENT_NAME).ToList();

            for (var i = 0; i < listDepartments.Count; i++)
            {
                var hisDepartmentTran = new V_HIS_DEPARTMENT_TRAN
                {
                    DEPARTMENT_ID = departmentIds[i],
                    DEPARTMENT_NAME = departmentNames[i]
                };
                listVirtualDepartment.Add(hisDepartmentTran);
            }
            listHisDepartment.AddRange(listVirtualDepartment);
            //=====================================================
            var grouplistHisDepartments = listHisDepartment.OrderBy(s => s.DEPARTMENT_NAME).GroupBy(s => s.DEPARTMENT_ID).ToList();
            foreach (var groupDepartments in grouplistHisDepartments)
            {
                var departmentName = groupDepartments.Select(s => s.DEPARTMENT_NAME).First();
                var listTotalPatientInDepartment = new List<string>();
                var listgroupDepartments = new List<string>();
                foreach (var groupDepartment in groupDepartments)
                {
                    var logTimeToString = Inventec.Common.DateTime.Convert.TimeNumberToDateString(groupDepartment.DEPARTMENT_IN_TIME ?? 0);
                    listgroupDepartments.Add(logTimeToString);
                }
                foreach (var totalPatient in _listDate)
                {
                    string totalPatientInDate;
                    if (totalPatient != null)
                        totalPatientInDate = listgroupDepartments.Count(s => s == totalPatient).ToString();
                    else
                        totalPatientInDate = "0";
                    listTotalPatientInDepartment.Add(totalPatientInDate);
                }
                ListSereServRdo.Add(new Mrs00116RDO(listTotalPatientInDepartment, departmentName));
            }
        }

        private void AddDateTime()
        {
            var dateFrom = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(CastFilter.TIME_FROM);
            var dateTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(CastFilter.TIME_TO);
            var date = dateFrom;
            for (var i = 0; i < 31; i++)
            {
                if (date <= dateTo)
                {
                    _listDate.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(date));
                    date = date.Value.AddDays(1);
                }
                else
                    _listDate.Add(null);
            }
        }

        protected override bool ProcessData()
        {
            var result = false;
            try
            {
                ProcessFilterDataDepartmentTran(listHisDepartmentTrans, listHisDepartments);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_TO));
                dicSingleTag.Add("TOTAL_PATIENT", _totalPatientReport);

                dicSingleTag.Add("DATE_1", _listDate[0] ?? "");
                dicSingleTag.Add("DATE_2", _listDate[1] ?? "");
                dicSingleTag.Add("DATE_3", _listDate[2] ?? "");
                dicSingleTag.Add("DATE_4", _listDate[3] ?? "");
                dicSingleTag.Add("DATE_5", _listDate[4] ?? "");
                dicSingleTag.Add("DATE_6", _listDate[5] ?? "");
                dicSingleTag.Add("DATE_7", _listDate[6] ?? "");
                dicSingleTag.Add("DATE_8", _listDate[7] ?? "");
                dicSingleTag.Add("DATE_9", _listDate[8] ?? "");
                dicSingleTag.Add("DATE_10", _listDate[9] ?? "");

                dicSingleTag.Add("DATE_11", _listDate[10] ?? "");
                dicSingleTag.Add("DATE_12", _listDate[11] ?? "");
                dicSingleTag.Add("DATE_13", _listDate[12] ?? "");
                dicSingleTag.Add("DATE_14", _listDate[13] ?? "");
                dicSingleTag.Add("DATE_15", _listDate[14] ?? "");
                dicSingleTag.Add("DATE_16", _listDate[15] ?? "");
                dicSingleTag.Add("DATE_17", _listDate[16] ?? "");
                dicSingleTag.Add("DATE_18", _listDate[17] ?? "");
                dicSingleTag.Add("DATE_19", _listDate[18] ?? "");
                dicSingleTag.Add("DATE_20", _listDate[19] ?? "");

                dicSingleTag.Add("DATE_21", _listDate[20] ?? "");
                dicSingleTag.Add("DATE_22", _listDate[21] ?? "");
                dicSingleTag.Add("DATE_23", _listDate[22] ?? "");
                dicSingleTag.Add("DATE_24", _listDate[23] ?? "");
                dicSingleTag.Add("DATE_25", _listDate[24] ?? "");
                dicSingleTag.Add("DATE_26", _listDate[25] ?? "");
                dicSingleTag.Add("DATE_27", _listDate[26] ?? "");
                dicSingleTag.Add("DATE_28", _listDate[27] ?? "");
                dicSingleTag.Add("DATE_29", _listDate[28] ?? "");
                dicSingleTag.Add("DATE_30", _listDate[29] ?? "");

                dicSingleTag.Add("DATE_31", _listDate[30] ?? "");

                objectTag.AddObjectData(store, "Department", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
