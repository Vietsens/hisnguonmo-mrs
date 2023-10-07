using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServExt;

namespace MRS.Processor.Mrs00370
{
    class Mrs00370Processor : AbstractProcessor
    {
        Mrs00370Filter castFilter = null;
        List<Mrs00370RDO> listRdo = new List<Mrs00370RDO>();
        List<Mrs00370RDO> listRdoGroup = new List<Mrs00370RDO>();
        List<HIS_FILM_SIZE> listFilmSize = new List<HIS_FILM_SIZE>();
        public Mrs00370Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = null;
        List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        public string s_Service = "";

        public override Type FilterType()
        {
            return typeof(Mrs00370Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00370Filter)this.reportFilter;
                this.listFilmSize = HisFilmSizeCFG.FILM_SIZEs;
                var skip = 0;
                // v_his_service_rety_cat
                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00370";
                serviceRetyFilter.REPORT_TYPE_CAT_IDs = castFilter.REPORT_TYPE_CAT_IDs;
                listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter);

                        HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery();
                        reqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                        reqFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                        reqFilter.SERVICE_REQ_STT_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT };
                        listServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(reqFilter);
                if (IsNotNullOrEmpty(listServiceRetyCats))
                {
                    s_Service = listServiceRetyCats.First().CATEGORY_NAME.ToUpper();
                    // yeu cau
                    skip = 0;
                    while (listServiceRetyCats.Count - skip > 0)
                    {
                        var listServiceIDs = listServiceRetyCats.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                        // yc-dv
                        if (IsNotNullOrEmpty(listServiceReqs))
                        {

                            skip = 0;
                            while (listServiceReqs.Count - skip > 0)
                            {
                                var listServiceReqIDs = listServiceReqs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                                var ssFilter = new HisSereServViewFilterQuery();
                                ssFilter.SERVICE_IDs = listServiceIDs.Select(s => s.SERVICE_ID).ToList();
                                ssFilter.HAS_EXECUTE = true;
                                ssFilter.SERVICE_REQ_IDs = listServiceReqIDs.Select(s => s.ID).ToList();
                                var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                                if (IsNotNullOrEmpty(listSereServ))
                                {
                                    listSereServs.AddRange(listSereServ);
                                }
                            }
                        }
                    }
                }
                if (IsNotNullOrEmpty(listSereServs))
                {
                    List<long> sereServId = listSereServs.Select(s => s.ID).ToList();

                    int start = 0;
                    int count = sereServId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServExtFilterQuery serExtFilter = new HisSereServExtFilterQuery();
                        serExtFilter.SERE_SERV_IDs = sereServId.Skip(start).Take(limit).ToList();
                        var hisSereServExts = new HisSereServExtManager(paramGet).Get(serExtFilter);
                        if (IsNotNullOrEmpty(hisSereServExts))
                        {
                            ListSereServExt.AddRange(hisSereServExts);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        var serviceReq = listServiceReqs.Where(s => s.ID == sereServ.SERVICE_REQ_ID).ToList();
                        Mrs00370RDO rdo = new Mrs00370RDO();
                        rdo.PATIENT_NAME = serviceReq.First().TDL_PATIENT_NAME;
                        if (serviceReq.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.AGE_FEMALE = serviceReq.First().TDL_PATIENT_DOB;
                        }
                        if (serviceReq.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.AGE_MALE = serviceReq.First().TDL_PATIENT_DOB;
                        }
                        if (sereServ.HEIN_CARD_NUMBER != null)
                        {
                            rdo.IS_HEIN = "X";
                        }
                        rdo.ADDRESS = serviceReq.First().TDL_PATIENT_ADDRESS;
                        rdo.ICD_NAME = serviceReq.First().ICD_NAME;
                        rdo.REQUEST_USERNAME = serviceReq.First().REQUEST_USERNAME;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        if (ListSereServExt != null)
                        {
                            var Ext = ListSereServExt.FirstOrDefault(o => o.SERE_SERV_ID == sereServ.ID);
                            if (Ext != null)
                            {
                                rdo.CONCLUDE = Ext.CONCLUDE;
                                rdo.NUMBER_OF_FILM = Ext.NUMBER_OF_FILM;
                                rdo.NUMBER_OF_FAIL_FILM = Ext.NUMBER_OF_FAIL_FILM;
                                if(Ext.FILM_SIZE_ID!=null)
                                {
                                rdo.FILM_SIZE_ID = Ext.FILM_SIZE_ID;
                                if (this.listFilmSize != null)
                                {
                                    rdo.FILM_SIZE_NAME = (this.listFilmSize.FirstOrDefault(o => o.ID == Ext.FILM_SIZE_ID) ?? new HIS_FILM_SIZE()).FILM_SIZE_NAME;
                                }
                                }
                            }
                        }
                        rdo.EXCUTE_USERNAME = serviceReq.First().EXECUTE_USERNAME;
                        rdo.GROUP_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.First().FINISH_TIME.ToString());

                        listRdo.Add(rdo);
                    }
                    listRdoGroup = listRdo.GroupBy(s => s.GROUP_DATE).Select(s => new Mrs00370RDO
                    {
                        GROUP_DATE = s.First().GROUP_DATE,
                        COUNT_IN_GROUP = s.Count(p => p.PATIENT_NAME != null)
                    }).ToList();
                }
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                dicSingleTag.Add("SERVICE_NAME", s_Service);
                var Gr = listServiceRetyCats.GroupBy(o => o.REPORT_TYPE_CAT_ID).ToList();
                int count = 0;
                foreach (var gr in Gr)
                {
                    count += 1;
                    dicSingleTag.Add(string.Format("REPORT_TYPE_CAT_NAME_{0}", count), string.Format("{0}: {1}", gr.First().CATEGORY_NAME, listRdo.Where(l => gr.Select(o => o.SERVICE_ID).Contains(l.SERVICE_ID)).ToList().Count));
                }
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList());
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
