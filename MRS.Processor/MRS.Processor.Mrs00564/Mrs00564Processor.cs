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

using MRS.MANAGER.Config;
using System.Reflection;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;
using System.IO;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisBlood;

namespace MRS.Processor.Mrs00564
{
    public class Mrs00564Processor : AbstractProcessor
    {
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_REPORT_TYPE_CAT> reportTypeCats = new List<HIS_REPORT_TYPE_CAT>();
        List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<HIS_SERVICE> listHisParentService = new List<HIS_SERVICE>();
        List<V_HIS_BLOOD> listHisBlood = new List<V_HIS_BLOOD>();
        List<CountService> listRdoService = new List<CountService>();
        Dictionary<string, decimal> DIC_AMOUNT = new Dictionary<string, decimal>();
        //Dictionary<string, decimal> DIC_AMOUNT_IN = new Dictionary<string, decimal>();

        List<HIS_TREATMENT_TYPE> listHisTreatmentType = new List<HIS_TREATMENT_TYPE>();
        List<Mrs00564RDO> ListRdo = new List<Mrs00564RDO>();
        Mrs00564Filter filter = null;
        CommonParam paramGet = new CommonParam();
        public Mrs00564Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00564Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00564Filter)reportFilter);
            var result = true;
            try
            {

                List<long> SERVICE_TYPE_IDs = new List<long>()
        {
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
        };
                //Danh sách yêu cầu
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                //serviceReqFilter = this.MapFilter<Mrs00564Filter, HisServiceReqFilterQuery>(filter, serviceReqFilter);
                serviceReqFilter.FINISH_TIME_FROM = filter.INTRUCTION_TIME_FROM;
                serviceReqFilter.FINISH_TIME_TO = filter.INTRUCTION_TIME_TO;
                //serviceReqFilter.SERVICE_REQ_TYPE_IDs = SERVICE_TYPE_IDs;
                listHisServiceReq = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                listHisServiceReq = listHisServiceReq.Where(x => x.IS_DELETE == 0).ToList();
                var listTreatmentIds = listHisServiceReq.Select(s => s.TREATMENT_ID).Distinct().ToList();
                var listServiceReqIds = listHisServiceReq.Select(s => s.ID).Distinct().ToList();
                //Danh sách HSDT
                if (listTreatmentIds != null && listTreatmentIds.Count > 0)
                {
                    var skip = 0;

                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        treatmentFilter.IDs = limit;
                        treatmentFilter.ORDER_FIELD = "ID";
                        treatmentFilter.ORDER_DIRECTION = "ASC";

                        var listTreatmentSub = new HisTreatmentManager(param).Get(treatmentFilter);
                        if (listTreatmentSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listTreatmentSub Get null");
                        else
                            listHisTreatment.AddRange(listTreatmentSub);
                    }
                }
                //danh sách sereServ
                if (listServiceReqIds != null && listServiceReqIds.Count > 0)
                {
                    var skip = 0;
                    while (listServiceReqIds.Count - skip > 0)
                    {
                        var max = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.SERVICE_REQ_IDs = max;
                        sereServFilter.TDL_SERVICE_TYPE_IDs = SERVICE_TYPE_IDs;
                        sereServFilter.HAS_EXECUTE = true;
                        var sereServs = new HisSereServManager().Get(sereServFilter);
                        sereServs = sereServs.Where(x => x.IS_DELETE == 0).ToList();
                        listHisSereServ.AddRange(sereServs);
                    }
                }
                //danh sách dịch vụ
                var serviceIds = listHisSereServ.Select(x => x.SERVICE_ID).Distinct().ToList();
                if (serviceIds != null && serviceIds.Count > 0)
                {
                    var skip = 0;
                    while (serviceIds.Count - skip > 0)
                    {
                        var limit = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                        serviceFilter.IDs = limit;
                        var services = new HisServiceManager().Get(serviceFilter);
                        ListService.AddRange(services);
                    }
                }
                // danh sách dịch vụ cha
                var parentServiceIds = ListService.Select(x => x.PARENT_ID ?? 0).Distinct().ToList();
                if (parentServiceIds != null || parentServiceIds.Count > 0)
                {
                    var skip = 0;
                    while (parentServiceIds.Count - skip > 0)
                    {
                        var limit = parentServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceFilterQuery parentFilter = new HisServiceFilterQuery();
                        parentFilter.IDs = limit;
                        var serviceParents = new HisServiceManager().Get(parentFilter);
                        listHisParentService.AddRange(serviceParents);
                    }
                }
                //danh sách máu
                var bloodIds = listHisSereServ.Where(x => x.BLOOD_ID != null).Select(x => x.BLOOD_ID ?? 0).Distinct().ToList();
                HisBloodViewFilterQuery bloodFilter = new HisBloodViewFilterQuery();
                bloodFilter.IDs = bloodIds;
                listHisBlood = new HisBloodManager().GetView(bloodFilter);

                //lấy đối tượng điều trị
                listHisTreatmentType = new HisTreatmentTypeManager().Get(new HisTreatmentTypeFilterQuery());


                //Lấy danh sách dịch vụ yc
                //if (listTreatmentIds != null && listTreatmentIds.Count > 0)
                //{
                //    var skip = 0;
                //    while (listTreatmentIds.Count - skip > 0)
                //    {
                //        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //        var sereServFilter = new HisSereServFilterQuery
                //        {
                //            TREATMENT_IDs = listIDs,
                //            HAS_EXECUTE = true,
                //            IS_EXPEND = false
                //        };
                //        var listHisSereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                //        listHisSereServ.AddRange(listHisSereServSub);
                //    }
                //    listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                //}
                HisReportTypeCatFilterQuery HisReportTypeCatfilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00564"
                };
                reportTypeCats = new HisReportTypeCatManager().Get(HisReportTypeCatfilter);
                var reportTypeCatIds = reportTypeCats.Select(o => o.ID).ToList() ?? new List<long>();

                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CAT_IDs = reportTypeCatIds
                };
                listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
                var serviceIdRetycats = listHisServiceRetyCat.Select(s => s.SERVICE_ID).Distinct().ToList();


                if (IsNotNullOrEmpty(serviceIdRetycats))
                {
                    var skip = 0;
                    while (serviceIdRetycats.Count - skip > 0)
                    {
                        var listIDs = serviceIdRetycats.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //DV
                        HisServiceFilterQuery filterSv = new HisServiceFilterQuery();
                        filterSv.IDs = listIDs;
                        var listServiceSub = new HisServiceManager(paramGet).Get(filterSv);
                        if (IsNotNullOrEmpty(listServiceSub))
                            listHisService.AddRange(listServiceSub);
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }
        #region MyRegion comment code
        private void ProsessCLS()
        {
            List<Mrs00564RDO> listR = new List<Mrs00564RDO>();
            try
            {
                if (IsNotNullOrEmpty(listHisSereServ))
                {
                    foreach (var item in listHisSereServ)
                    {
                        Mrs00564RDO rdo = new Mrs00564RDO();
                        rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        var treat = listHisTreatment.Where(x => x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
                        if (treat != null)
                        {
                            rdo.TREATMENT_TYPE_ID = treat.TDL_TREATMENT_TYPE_ID ?? 0;
                            var treatmentType = listHisTreatmentType.Where(x => x.ID == treat.TDL_TREATMENT_TYPE_ID).FirstOrDefault();
                            if (treatmentType != null)
                            {
                                rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
                            }
                        }
                        var prServiceId = ListService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault().PARENT_ID;
                        if (prServiceId != null)
                        {
                            var prService = listHisParentService.Where(x => x.ID == prServiceId).FirstOrDefault();
                            if (prService != null)
                            {
                                rdo.SERVICE_PARENT_CODE = prService.SERVICE_CODE;
                                rdo.SERVICE_PARENT_NAME = prService.SERVICE_NAME;
                            }
                        }
                        rdo.BLOOD_ID = item.BLOOD_ID;
                        rdo.AMOUNT = item.AMOUNT;
                        listR.Add(rdo);
                    }
                    foreach (var item in listR)
                    {
                        CountService rdoService = new CountService();
                        rdoService.ID = 1;
                        if (item.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            if (item.SERVICE_PARENT_CODE != null)
                            {
                                if (item.SERVICE_PARENT_CODE == "XNHH")
                                {
                                    rdoService.AMOUNT_HH_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNSH")
                                {
                                    rdoService.AMOUNT_SH_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNNV")
                                {
                                    rdoService.AMOUNT_XNNV_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNVS")
                                {
                                    rdoService.AMOUNT_VS_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNNT")
                                {
                                    rdoService.AMOUNT_XNNT_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNSH")
                                {
                                    rdoService.AMOUNT_SH_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XQTQ" || item.SERVICE_PARENT_CODE == "XQSH")
                                {
                                    rdoService.AMOUNT_XQ_NT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "CCLVT" || item.SERVICE_PARENT_CODE == "CCHT")
                                {
                                    rdoService.AMOUNT_CT_Scanner_NT = item.AMOUNT;
                                }
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                            {
                                //var blood = listHisBlood.Where(x => x.ID == item.BLOOD_ID).FirstOrDefault();
                                //if (blood != null)
                                //{
                                //    rdoService.AMOUNT_MAU_NT = blood.VOLUME;
                                //}
                                rdoService.AMOUNT_MAU_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_PARENT_CODE == "XNGPB" || item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL)
                            {
                                rdoService.AMOUNT_GPB_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                            {
                                rdoService.AMOUNT_SA_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                            {
                                rdoService.AMOUNT_NS_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("số hóa"))
                            {
                                rdoService.AMOUNT_XQ_KTS = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("điện tim"))
                            {
                                rdoService.AMOUNT_DT_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("siêu âm đàn hồi mô"))
                            {
                                rdoService.AMOUNT_SA_DHM_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("đo loãng xương") && item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                            {
                                rdoService.AMOUNT_DLX_NT = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("thận nhân tạo"))
                            {
                                rdoService.AMOUNT_THAN_NT_NT = 1;
                            }
                            if (item.SERVICE_NAME.Contains("HIV"))
                            {
                                rdoService.AMOUNT_HIV_NT = item.AMOUNT;
                            }
                        }
                        if (item.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            if (item.SERVICE_PARENT_CODE != null)
                            {
                                if (item.SERVICE_PARENT_CODE == "XNHH")
                                {
                                    rdoService.AMOUNT_HH = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNSH")
                                {
                                    rdoService.AMOUNT_SH = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNVS")
                                {
                                    rdoService.AMOUNT_VS = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNNV")
                                {
                                    rdoService.AMOUNT_XNNV = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNNT")
                                {
                                    rdoService.AMOUNT_XNNT = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XNGPB")
                                {
                                    rdoService.AMOUNT_GPB = item.AMOUNT;
                                }
                                if (item.SERVICE_PARENT_CODE == "XQTQ" || item.SERVICE_PARENT_CODE == "XQSH")
                                {
                                    rdoService.AMOUNT_XQ += item.AMOUNT;
                                }

                                if (item.SERVICE_PARENT_CODE == "CCLVT" || item.SERVICE_PARENT_CODE == "CCHT")
                                {
                                    rdoService.AMOUNT_CT_Scanner = item.AMOUNT;
                                }
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                            {
                                //var blood = listHisBlood.Where(x => x.ID == item.BLOOD_ID).FirstOrDefault();
                                //if (blood != null)
                                //{
                                //    rdoService.AMOUNT_MAU = blood.VOLUME;
                                //}
                                rdoService.AMOUNT_MAU = item.AMOUNT;
                            }
                            if (item.SERVICE_PARENT_CODE == "XNGPB" || item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL)
                            {
                                rdoService.AMOUNT_GPB = item.AMOUNT;
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                            {
                                rdoService.AMOUNT_SA = item.AMOUNT;
                            }
                            if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                            {
                                rdoService.AMOUNT_NS = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("số hóa"))
                            {
                                rdoService.AMOUNT_XQ_KTS = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("điện tim"))
                            {
                                rdoService.AMOUNT_DT = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("siêu âm đàn hồi mô"))
                            {
                                rdoService.AMOUNT_SA_DHM = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("đo loãng xương") && item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                            {
                                rdoService.AMOUNT_DLX = item.AMOUNT;
                            }
                            if (item.SERVICE_NAME.ToLower().Contains("thận nhân tạo"))
                            {
                                rdoService.AMOUNT_THAN_NT = 1;
                            }
                            if (item.SERVICE_NAME.Contains("HIV"))
                            {
                                rdoService.AMOUNT_HIV = item.AMOUNT;
                            }
                        }
                        listRdoService.Add(rdoService);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private void ProcessorGroupServiceRdo()
        {
            try
            {
                var group = listRdoService.GroupBy(x => x.ID).ToList();
                listRdoService.Clear();
                foreach (var item in group)
                {
                    CountService rdo = new CountService();
                    rdo.AMOUNT_CT_Scanner = item.Sum(x => x.AMOUNT_CT_Scanner);
                    rdo.AMOUNT_CT_Scanner_NT = item.Sum(x => x.AMOUNT_CT_Scanner_NT);
                    rdo.AMOUNT_DLX = item.Sum(x => x.AMOUNT_DLX);
                    rdo.AMOUNT_DLX_NT = item.Sum(x => x.AMOUNT_DLX_NT);
                    rdo.AMOUNT_DT = item.Sum(x => x.AMOUNT_DT);
                    rdo.AMOUNT_DT_NT = item.Sum(x => x.AMOUNT_DT_NT);
                    rdo.AMOUNT_GPB = item.Sum(x => x.AMOUNT_GPB);
                    rdo.AMOUNT_GPB_NT = item.Sum(x => x.AMOUNT_GPB_NT);
                    rdo.AMOUNT_HH = item.Sum(x => x.AMOUNT_HH);
                    rdo.AMOUNT_HH_NT = item.Sum(x => x.AMOUNT_HH_NT);
                    rdo.AMOUNT_HIV = item.Sum(x => x.AMOUNT_HIV);
                    rdo.AMOUNT_HIV_NT = item.Sum(x => x.AMOUNT_HIV_NT);
                    rdo.AMOUNT_MAU = item.Sum(x => x.AMOUNT_MAU);
                    rdo.AMOUNT_MAU_NT = item.Sum(x => x.AMOUNT_MAU_NT);
                    rdo.AMOUNT_NS = item.Sum(x => x.AMOUNT_NS);
                    rdo.AMOUNT_NS_NT = item.Sum(x => x.AMOUNT_NS_NT);
                    rdo.AMOUNT_SA_DHM = item.Sum(x => x.AMOUNT_SA_DHM);
                    rdo.AMOUNT_SA_DHM_NT = item.Sum(x => x.AMOUNT_SA_DHM_NT);
                    rdo.AMOUNT_SH = item.Sum(x => x.AMOUNT_SH);
                    rdo.AMOUNT_SH_NT = item.Sum(x => x.AMOUNT_SH_NT);
                    rdo.AMOUNT_THAN_NT = item.Sum(x => x.AMOUNT_THAN_NT);
                    rdo.AMOUNT_THAN_NT_NT = item.Sum(x => x.AMOUNT_THAN_NT_NT);
                    rdo.AMOUNT_VS = item.Sum(x => x.AMOUNT_VS);
                    rdo.AMOUNT_VS_NT = item.Sum(x => x.AMOUNT_VS_NT);
                    rdo.AMOUNT_XN = item.Sum(x => x.AMOUNT_XN);
                    rdo.AMOUNT_XN_NT = item.Sum(x => x.AMOUNT_XN_NT);
                    rdo.AMOUNT_XQ = item.Sum(x => x.AMOUNT_XQ) - item.Sum(x => x.AMOUNT_XQ_KTS);
                    rdo.AMOUNT_XQ_KTS = item.Sum(x => x.AMOUNT_XQ_KTS);
                    rdo.AMOUNT_XQ_KTS_NT = item.Sum(x => x.AMOUNT_XQ_KTS_NT);
                    rdo.AMOUNT_XQ_NT = item.Sum(x => x.AMOUNT_XQ_NT) - item.Sum(x => x.AMOUNT_XQ_KTS_NT);
                    rdo.AMOUNT_SA = item.Sum(x => x.AMOUNT_SA) - item.Sum(x => x.AMOUNT_SA_DHM) - item.Sum(x => x.AMOUNT_DLX);
                    rdo.AMOUNT_SA_NT = item.Sum(x => x.AMOUNT_SA_NT) - item.Sum(x => x.AMOUNT_SA_DHM_NT) - item.Sum(x => x.AMOUNT_DLX_NT);
                    listRdoService.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        #endregion// comment code
#region dictionary
        //private void ProsessCLS()
        //{
        //    List<Mrs00564RDO> listR = new List<Mrs00564RDO>();
        //    try
        //    {
        //        if (IsNotNullOrEmpty(listHisSereServ))
        //        {
        //            foreach (var item in listHisSereServ)
        //            {
        //                Mrs00564RDO rdo = new Mrs00564RDO();
        //                rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
        //                rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
        //                rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
        //                rdo.SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
        //                rdo.SERVICE_ID = item.SERVICE_ID;
        //                var treat = listHisTreatment.Where(x => x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
        //                if (treat != null)
        //                {
        //                    rdo.TREATMENT_TYPE_ID = treat.TDL_TREATMENT_TYPE_ID ?? 0;
        //                    var treatmentType = listHisTreatmentType.Where(x => x.ID == treat.TDL_TREATMENT_TYPE_ID).FirstOrDefault();
        //                    if (treatmentType != null)
        //                    {
        //                        rdo.TREATMENT_TYPE_NAME = treatmentType.TREATMENT_TYPE_NAME;
        //                    }
        //                }
        //                var prServiceId = ListService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault().PARENT_ID;
        //                if (prServiceId != null)
        //                {
        //                    var prService = listHisParentService.Where(x => x.ID == prServiceId).FirstOrDefault();
        //                    if (prService != null)
        //                    {
        //                        rdo.SERVICE_PARENT_CODE = prService.SERVICE_CODE;
        //                        rdo.SERVICE_PARENT_NAME = prService.SERVICE_NAME;
        //                    }
        //                }
        //                rdo.BLOOD_ID = item.BLOOD_ID;
        //                rdo.AMOUNT = item.AMOUNT;
        //                listR.Add(rdo);
        //            }
        //        }
        //        var noiTru = listR.Where(x=>x.TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
        //        var ngoaiTru = listR.Where(x=>x.TREATMENT_TYPE_ID!=IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
        //        CountService service = new CountService();
        //        service.DIC_PARENT_SERVICE_AMOUNT_NGTRU = ngoaiTru.GroupBy(x=>x.SERVICE_PARENT_CODE).ToDictionary(q=>q.Key, p=>p.Sum(x=>x.AMOUNT));
        //        service.DIC_SERVICE_AMOUNT_NGTRU = ngoaiTru.GroupBy(x => x.SERVICE_CODE).ToDictionary(q => q.Key, p => p.Sum(x => x.AMOUNT));
        //        service.DIC_SERVICE_TYPE_AMOUNT_NGTRU = ngoaiTru.GroupBy(x => x.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, p => p.Sum(x => x.AMOUNT));

        //        service.DIC_PARENT_SERVICE_AMOUNT_NT = noiTru.GroupBy(x => x.SERVICE_PARENT_CODE).ToDictionary(q => q.Key, p => p.Sum(x => x.AMOUNT));
        //        service.DIC_SERVICE_AMOUNT_NT = noiTru.GroupBy(x => x.SERVICE_CODE).ToDictionary(q => q.Key, p => p.Sum(x => x.AMOUNT));
        //        service.DIC_SERVICE_TYPE_AMOUNT_NT = noiTru.GroupBy(x => x.SERVICE_TYPE_CODE).ToDictionary(q => q.Key, p => p.Sum(x => x.AMOUNT));
        //        listRdoService.Add(service);
        //    }
        //    catch(Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //    }
        //}
        #endregion
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                //ProcessorSereServ();
                ProsessCLS();
                ProcessorGroupServiceRdo();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private void ProcessorSereServ()
        {

            try
            {
                foreach (var r in listHisSereServ)
                {
                    
                    var serviceReq = listHisServiceReq.FirstOrDefault(o => o.ID == r.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                    var treatment = listHisTreatment.FirstOrDefault(o => o.ID == r.TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                    var serviceRetyCat = listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == r.SERVICE_ID) ?? new V_HIS_SERVICE_RETY_CAT();
                    var reportTypeCat = reportTypeCats.FirstOrDefault(o => o.ID == serviceRetyCat.REPORT_TYPE_CAT_ID) ?? new HIS_REPORT_TYPE_CAT();
                    if (String.IsNullOrWhiteSpace(reportTypeCat.CATEGORY_CODE))
                    {
                        continue;
                    }
                    if (r.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU)
                    {
                        var blood = listHisBlood.Where(x => x.ID == r.BLOOD_ID).FirstOrDefault();
                        if (blood != null)
                        {
                            AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE, blood.VOLUME);
                            if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {

                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_IN", blood.VOLUME);
                                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_INBHYT", blood.VOLUME);
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_BHYT", blood.VOLUME);
                                }
                                else
                                {
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_INVP", blood.VOLUME);
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_VP", blood.VOLUME);
                                }
                            }
                            else
                            {
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUT", blood.VOLUME);
                                if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUTBHYT", blood.VOLUME);
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_BHYT", blood.VOLUME);
                                }
                                else
                                {
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUTVP", blood.VOLUME);
                                    AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_VP", blood.VOLUME);
                                }
                            }
                        }
                    }
                    else
                    {

                        AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE, NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                        if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {

                            AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_IN", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_INBHYT", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_BHYT", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            }
                            else
                            {
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_INVP", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_VP", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            }
                        }
                        else
                        {
                            AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUT", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUTBHYT", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_BHYT", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            }
                            else
                            {
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_OUTVP", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                                AddToDic(ref this.DIC_AMOUNT, reportTypeCat.CATEGORY_CODE + "_VP", NumberOfFilm(r.SERVICE_ID) ?? r.AMOUNT);
                            }
                        }
                    }
                }

                foreach (var item in HisRoomCFG.HisRooms)
                {
                    Mrs00564RDO rdo = new Mrs00564RDO();
                    List<HIS_SERE_SERV> listSub = listHisSereServ.Where(o => o.TDL_REQUEST_ROOM_ID == item.ID).ToList();
                    rdo.TYPE = item.ROOM_TYPE_CODE + "_";
                    rdo.CODE = item.ROOM_CODE;
                    rdo.NAME = item.ROOM_NAME;
                    if (listHisServiceRetyCat.Count > 0)
                    {
                        rdo.DIC_GROUP_AMOUNT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => NumberOfFilm(s.SERVICE_ID) ?? s.AMOUNT));
                    }
                    ListRdo.Add(rdo);
                }


                foreach (var item in HisDepartmentCFG.DEPARTMENTs)
                {
                    Mrs00564RDO rdo = new Mrs00564RDO();
                    List<HIS_SERE_SERV> listSub = listHisSereServ.Where(o => o.TDL_REQUEST_DEPARTMENT_ID == item.ID).ToList();
                    rdo.TYPE = "";
                    rdo.CODE = item.DEPARTMENT_CODE;
                    rdo.NAME = item.DEPARTMENT_NAME;
                    if (listHisServiceRetyCat.Count > 0)
                    {
                        rdo.DIC_GROUP_AMOUNT = listSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => NumberOfFilm(s.SERVICE_ID) ?? s.AMOUNT));
                    }
                    ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AddToDic( ref Dictionary<string, decimal> dictionary, string key, decimal value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] += value;
            }
        }
        private decimal? NumberOfFilm(long serviceId)
        {
            var service = listHisService.FirstOrDefault(o => o.ID == serviceId);
            if (service != null && service.NUMBER_OF_FILM > 0)
            {
                return service.NUMBER_OF_FILM;
            }
            else
            {
                return null;
            }
        }

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? 0));

            foreach (var item in this.DIC_AMOUNT)
            {
                if (!dicSingleTag.ContainsKey(item.Key))
                {
                    dicSingleTag.Add(item.Key, item.Value);
                }
                else
                {
                    dicSingleTag.Add(item.Key, item.Value);
                }
            }

           
            List<string> DepartmentCodes = Find(filter.DEPARTMENT_CODEs);

            List<string> RoomCodes = Find(filter.ROOM_CODEs);
            if (RoomCodes != null && DepartmentCodes != null)
            {
                ListRdo = ListRdo.Where(o => DepartmentCodes.Contains(o.TYPE + o.CODE) || RoomCodes.Contains(o.TYPE + o.CODE)).ToList();
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Service", listRdoService);

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        private List<string> Find(string codes)
        {
            try
            {
                return codes == null ? null : codes.Split(',').ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }

        }


    }
}
