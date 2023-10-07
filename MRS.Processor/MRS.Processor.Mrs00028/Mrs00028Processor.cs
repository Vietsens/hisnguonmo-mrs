using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00028
{
    public class Mrs00028Processor : AbstractProcessor
    {
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        CommonParam paramGet = new CommonParam();
        Mrs00028Filter castFilter = null;
        List<Mrs00028RDO> ListRdo = new List<Mrs00028RDO>();
        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ = new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();

        public Mrs00028Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00028Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00028Filter)this.reportFilter);
                LoadDataToRam();
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
                ProcessListCurrentSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentSereServ()
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0).ToList();
                    var GroupDepartment = ListCurrentSereServ.GroupBy(g => g.TDL_REQUEST_DEPARTMENT_ID).ToList();
                    foreach (var groupDepartment in GroupDepartment)
                    {
                        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> listgroup = groupDepartment.ToList<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                        if (listgroup != null && listgroup.Count > 0)
                        {
                            var Groups = listgroup.GroupBy(g => g.TDL_REQUEST_ROOM_ID).ToList();
                            foreach (var group in Groups)
                            {
                                List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> listSub = group.ToList<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                                if (listSub != null && listSub.Count > 0)
                                {
                                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub[0].TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                                    Mrs00028RDO dataRDO = new Mrs00028RDO();
                                    dataRDO.REQUEST_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                                    dataRDO.REQUEST_ROOM_CODE = room.ROOM_CODE;
                                    dataRDO.REQUEST_ROOM_NAME = room.ROOM_NAME;
                                    decimal totalPatientPrice = 0;
                                    decimal totalHeinPrice = 0;
                                    foreach (var sereServ in listSub)
                                    {
                                        totalPatientPrice += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                        totalHeinPrice += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                    }
                                    dataRDO.VIR_TOTAL_PATIENT_PRICE = totalPatientPrice;
                                    dataRDO.VIR_TOTAL_HEIN_PRICE = totalHeinPrice;
                                    ListRdo.Add(dataRDO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ListCurrentSereServ = new HisSereServManager().Get(filter);
                //DV - thanh toan
                if (IsNotNullOrEmpty(ListCurrentSereServ))
                {
                    var listTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                            filterSereServBill.TDL_TREATMENT_IDs = listIDs;
                            var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                            if (IsNotNullOrEmpty(listSereServBillSub))
                                ListSereServBill.AddRange(listSereServBillSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentSereServ.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
