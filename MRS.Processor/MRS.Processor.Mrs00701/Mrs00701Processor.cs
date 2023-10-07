using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00701
{
    class Mrs00701Processor : AbstractProcessor
    {
        List<Mrs00701RDO> ListRdo = new List<Mrs00701RDO>();
        List<Mrs00701RDO> ListRdoDetail = new List<Mrs00701RDO>();
        List<HIS_PATIENT_TYPE> ListColumn = new List<HIS_PATIENT_TYPE>();
        Mrs00701Filter castFilter = null;
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();

        public Mrs00701Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00701Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00701Filter)this.reportFilter;

                ListSereServ = new ManagerSql().GetLisSereServ(castFilter);

                ListService = new ManagerSql().GetLisService(castFilter) ?? new List<HIS_SERVICE>();

                var exts = new ManagerSql().GetLisSereServExt(ListSereServ.Select(s => s.ID).ToList()) ?? new List<HIS_SERE_SERV_EXT>();
                if (IsNotNullOrEmpty(exts))
                {
                    ListSereServExt.AddRange(exts);
                }

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    ListRdoDetail.Clear();
                    var groupSereServ = ListSereServ.GroupBy(o => new { o.SERVICE_ID, o.PATIENT_TYPE_ID, o.VIR_PRICE }).ToList();
                    foreach (var item in groupSereServ)
                    {
                        Mrs00701RDO rdo = new Mrs00701RDO();
                        rdo.SERVICE_ID = item.First().SERVICE_ID;
                        rdo.PATIENT_TYPE_ID = item.First().PATIENT_TYPE_ID;
                        rdo.TDL_SERVICE_CODE = item.First().TDL_SERVICE_CODE;
                        rdo.TDL_SERVICE_NAME = item.First().TDL_SERVICE_NAME;
                        rdo.PRICE = item.First().VIR_PRICE ?? 0;
                        rdo.TOTAL_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                        rdo.AMOUNT = item.Sum(s => s.AMOUNT);
                        var rows = ListRdoDetail.Where(o => o.SERVICE_ID == item.First().SERVICE_ID && o.PATIENT_TYPE_ID == item.First().PATIENT_TYPE_ID).ToList();
                        int rcount = IsNotNullOrEmpty(rows) ? rows.Count + 1 : 1;
                        rdo.KEY = string.Format("{0}_{1}", item.First().SERVICE_ID, rcount);

                        var exts = ListSereServExt.Where(o => item.Select(s => s.ID).Contains(o.SERE_SERV_ID)).ToList();
                        if (IsNotNullOrEmpty(exts))
                        {
                            rdo.NUMBER_OF_FILM = exts.Sum(s => s.NUMBER_OF_FILM ?? 0);
                        }

                        var sv = ListService.FirstOrDefault(o => item.First().SERVICE_ID==o.ID);
                        if (sv!=null)
                        {
                            rdo.SERVICE_NUMBER_OF_FILM = (sv.NUMBER_OF_FILM ?? 0) * item.Sum(s => s.AMOUNT);
                        }

                        ListRdoDetail.Add(rdo);
                    }

                    ListRdo = ListRdoDetail.GroupBy(o => o.KEY).Select(s => new Mrs00701RDO()
                    {
                        SERVICE_ID = s.First().SERVICE_ID,
                        //PATIENT_TYPE_ID = s.First().PATIENT_TYPE_ID,
                        KEY = s.First().KEY,
                        TDL_SERVICE_CODE = s.First().TDL_SERVICE_CODE,
                        TDL_SERVICE_NAME = s.First().TDL_SERVICE_NAME,
                        AMOUNT = s.Sum(t => t.AMOUNT),
                        NUMBER_OF_FILM = s.Sum(t => t.NUMBER_OF_FILM),
                        TOTAL_PRICE = s.Sum(t => t.TOTAL_PRICE),
                        SERVICE_NUMBER_OF_FILM = s.Sum(t => t.SERVICE_NUMBER_OF_FILM)
                    }).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoDetail.Clear();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                ListRdo = ListRdo.OrderBy(o => o.TDL_SERVICE_NAME).ToList();
                ListRdoDetail.Add(new Mrs00701RDO()
                {
                    PATIENT_TYPE_ID = 1,
                    KEY="_"
                });
                ListRdo.Add(new Mrs00701RDO()
                {
                    PATIENT_TYPE_ID = 1,
                    KEY = "_"
                });
                ListColumn = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.Where(o => ListRdoDetail.Select(s => s.PATIENT_TYPE_ID).Contains(o.ID)).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
                objectTag.AddObjectData(store, "ReportCol", ListColumn);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
