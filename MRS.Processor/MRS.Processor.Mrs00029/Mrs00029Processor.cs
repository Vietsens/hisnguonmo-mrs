using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00029
{
    public class Mrs00029Processor : AbstractProcessor
    {
        Mrs00029Filter castFilter = null;
        CommonParam paramGet = new CommonParam();
        List<Mrs00029RDO> ListRdo = new List<Mrs00029RDO>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> ListCurrentSereServ = new List<HIS_SERE_SERV>();

        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();

        public Mrs00029Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00029Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00029: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                
                castFilter = ((Mrs00029Filter)this.reportFilter);
                LoadDataToRam();
                listRoom = new HisRoomManager(param).GetView(new HisRoomViewFilterQuery());
                result = true;
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
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => 
                        ListSereServBill.Exists(p=>p.SERE_SERV_ID==o.ID)
                        || o.VIR_TOTAL_PATIENT_PRICE == 0).ToList();

                    var Groups = ListCurrentSereServ.OrderBy(o => o.TDL_REQUEST_DEPARTMENT_ID).ToList().GroupBy(g => g.TDL_REQUEST_DEPARTMENT_ID).ToList();

                    foreach (var group in Groups)
                    {
                        List<HIS_SERE_SERV> listSub = group.ToList<HIS_SERE_SERV>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00029RDO dataRDO = new Mrs00029RDO();
                            var room = listRoom.FirstOrDefault(o => o.ID == listSub[0].TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                            dataRDO.REQUEST_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                            dataRDO.REQUEST_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                            foreach (var sereServ in listSub)
                            {
                                dataRDO.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                dataRDO.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            ListRdo.Add(dataRDO);
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
                var listTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList();

                //dich vu - thanh toan
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
