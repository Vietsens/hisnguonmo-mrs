using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisRoomType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00808
{
    public class Mrs00808Processor : AbstractProcessor
    {
        public List<Mrs00808RDO> ListRdo = new List<Mrs00808RDO>();
        public List<Mrs00808RDO> ListRdoDepartment = new List<Mrs00808RDO>();
        public List<Mrs00808RDO> ListRdoRoom = new List<Mrs00808RDO>();
        public Mrs00808Filter filter = null;
        public List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        public List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        //public List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        public List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        public List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        public List<V_HIS_ROOM> ListHisRoom = new List<V_HIS_ROOM>();
        public List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        public List<HIS_TREATMENT_TYPE> ListTreatmentType = new List<HIS_TREATMENT_TYPE>();
        List<HIS_SERVICE> listServiceParent = new List<HIS_SERVICE>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCate = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        public const long THUCLAM = 1;
        public const long CHIDINH = 0;
        public Mrs00808Processor(CommonParam param, string reportTypeName)
            : base(param, reportTypeName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00808Filter);
        }
        List<long> SERVICE_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN
        };
        protected override bool GetData()
        {
            bool result = false;
            filter = (Mrs00808Filter)this.reportFilter;
            try
            {
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                ListDepartment = new HisDepartmentManager(new CommonParam()).Get(departmentFilter);
                HisRoomViewFilterQuery roomFilter = new HisRoomViewFilterQuery();
                //roomFilter.ROOM_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__XL;
                ListHisRoom = new HisRoomManager(new CommonParam()).GetView(roomFilter);
                HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                sereServFilter.TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM;
                sereServFilter.TDL_INTRUCTION_TIME_TO = filter.TIME_TO;
                sereServFilter.TDL_SERVICE_TYPE_IDs = SERVICE_TYPE_IDs;
                sereServFilter.TDL_REQUEST_DEPARTMENT_IDs = filter.REQUEST_DEPARTMENT_IDs;
                sereServFilter.TDL_REQUEST_ROOM_IDs = filter.REQUEST_ROOM_IDs;
                sereServFilter.TDL_EXECUTE_DEPARTMENT_IDs = filter.EXECUTE_DEPARTMENT_IDs;
                sereServFilter.TDL_EXECUTE_ROOM_IDs = filter.EXECUTE_ROOM_IDs;
                ListSereServ = new HisSereServManager(new CommonParam()).Get(sereServFilter);
                
                //hồ sơ điều trị
                GetTreatment();

                //dịch vụ
                GetService();

                //ext
                //GetSereServExt();

                //serviceReq
                GetServiceReq();

                //dịch vụ cha
                GetParentSv();

                //diện điều trị
                GetTreatmentType();

                //nhóm báo cáo
                GetDicCate();

                GetRdoDataCD();
                GetRdoDataTL();
                //if (filter.CHECK_TREATMENT_TYPE == true)
                //{
                //    ListRdo.Where(x => x.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                //}
                //else
                //{
                //    ListRdo.Where(x => x.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList();
                //}
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void GetDicCate()
        {

            HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyFilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
            var listServiceRetyCat = new HisServiceRetyCatManager().GetView(serviceRetyFilter)??new List<V_HIS_SERVICE_RETY_CAT>();
            dicCate = listServiceRetyCat.GroupBy(o => o.SERVICE_ID).ToDictionary(p=>p.Key,q=>q.First());

        }

        private void GetTreatment()
        {
            int skip = 0;
            List<long> ListAllId = ListSereServ.Select(x => x.TDL_TREATMENT_ID??0).Distinct().ToList();
            while (ListAllId.Count - skip > 0)
            {
                var listId = ListAllId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.IDs = listId;
                var treatments = new HisTreatmentManager(new CommonParam()).Get(treatmentFilter);

                if (IsNotNullOrEmpty(treatments))
                {
                    ListTreatment.AddRange(treatments);

                }

            }

        }

        private void GetService()
        {
            int skip = 0;
            List<long> ListAllId = ListSereServ.Select(x => x.SERVICE_ID).Distinct().ToList();
            while (ListAllId.Count - skip > 0)
            {
                var listId = ListAllId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                serviceFilter.IDs = listId;
                var services = new HisServiceManager(new CommonParam()).Get(serviceFilter);

                if (IsNotNullOrEmpty(services))
                {
                    ListService.AddRange(services);

                }

            }

        }

        //private void GetSereServExt()
        //{
        //    int skip = 0;
        //    List<long> ListAllId = ListSereServ.Select(x => x.ID).Distinct().ToList();
        //    while (ListAllId.Count - skip > 0)
        //    {
        //        var listId = ListAllId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
        //        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

        //        HisSereServExtFilterQuery sereServExtFilter = new HisSereServExtFilterQuery();
        //        sereServExtFilter.SERE_SERV_IDs = listId;
        //        var sereServExts = new HisSereServExtManager(new CommonParam()).Get(sereServExtFilter);
        //        if (IsNotNullOrEmpty(sereServExts))
        //        {
        //            ListSereServExt.AddRange(sereServExts);

        //        }

        //    }

        //}

        private void GetServiceReq()
        {
            int skip = 0;
            List<long> ListAllId = ListSereServ.Select(x => x.SERVICE_REQ_ID??0).Distinct().ToList();
            while (ListAllId.Count - skip > 0)
            {
                var listId = ListAllId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisServiceReqFilterQuery serviceReqFilterQuery = new HisServiceReqFilterQuery();
                serviceReqFilterQuery.IDs = listId;
                var serviceReqs = new HisServiceReqManager(new CommonParam()).Get(serviceReqFilterQuery);
                if (IsNotNullOrEmpty(serviceReqs))
                {
                    ListServiceReq.AddRange(serviceReqs);

                }

            }

        }

        private void GetParentSv()
        {
            int skip = 0;
            List<long> ListAllId = ListService.Select(x => x.PARENT_ID ?? 0).Distinct().ToList();
            while (ListAllId.Count - skip > 0)
            {
                var listId = ListAllId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisServiceFilterQuery serviceParentFilter = new HisServiceFilterQuery();
                serviceParentFilter.IDs = listId;
                var listServiceParentSub = new HisServiceManager(new CommonParam()).Get(serviceParentFilter);
                if (IsNotNullOrEmpty(listServiceParentSub))
                {
                    listServiceParent.AddRange(listServiceParentSub);

                }

            }
            
        }

        private void GetTreatmentType()
        {
            ListTreatmentType = new HisTreatmentTypeManager(new CommonParam()).Get(new HisTreatmentTypeFilterQuery());
        }
        private void GetRdoData(List<HIS_SERE_SERV> listData,long type)
        {
            try
            {
                if (IsNotNullOrEmpty(listData))
                {
                    foreach (var item in listData)
                    {
                        Mrs00808RDO rdo = new Mrs00808RDO();
                        rdo.DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID;
                        var depa = ListDepartment.Where(x => x.ID == item.TDL_REQUEST_DEPARTMENT_ID).FirstOrDefault();
                        if (depa != null)
                        {
                            rdo.DEPARTMENT_CODE = depa.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
                        }
                        rdo.ROOM_ID = item.TDL_REQUEST_ROOM_ID;
                        var room = ListHisRoom.Where(x => x.ID == item.TDL_REQUEST_ROOM_ID).FirstOrDefault();
                        if (room != null)
                        {
                            rdo.ROOM_CODE = room.ROOM_CODE;
                            rdo.ROOM_NAME = room.ROOM_NAME;
                        }
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        var service = ListService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault();
                        if (service != null)
                        {
                            rdo.SERVICE_NAME = service.SERVICE_NAME;
                            rdo.SERVICE_CODE = service.SERVICE_CODE;
                            var serviceParent = listServiceParent.Where(x => x.ID == service.PARENT_ID).FirstOrDefault();
                            if (serviceParent != null)
                            {
                                rdo.SERVICE_PARENT_CODE = serviceParent.SERVICE_CODE;
                                rdo.SERVICE_PARENT_NAME = serviceParent.SERVICE_NAME;
                            }
                            if (dicCate.ContainsKey(service.ID))
                            {
                                rdo.CATEGORY_CODE = dicCate[service.ID].CATEGORY_CODE;
                                rdo.CATEGORY_NAME = dicCate[service.ID].CATEGORY_NAME;
                            }
                        }
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.PRICE = item.PRICE;
                        rdo.TREATMENT_ID = item.TDL_TREATMENT_ID ?? 0;
                        var treatmet = ListTreatment.Where(x => x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
                        if (treatmet != null)
                        {
                            rdo.TREATMENT_TYPE_ID = treatmet.TDL_TREATMENT_TYPE_ID ?? 0;
                        }
                        rdo.TYPE = type;
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetRdoDataCD()
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    GetRdoData(ListSereServ,CHIDINH);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void GetRdoDataTL()
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    var listServiceReqTLId = ListServiceReq.Where(o=>/*o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL||*/o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).Select(p=>p.ID).ToList();
                    var listSereServTL = ListSereServ.Where(o => listServiceReqTLId.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();

                    GetRdoData(listSereServTL, THUCLAM);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessRdo()
        {
            try
            {
                var groupByDepartment = ListRdo.Where(x=>x.TREATMENT_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).GroupBy(x => x.DEPARTMENT_ID).ToList();
                foreach (var item in groupByDepartment)
                {
                    Mrs00808RDO rdo = new Mrs00808RDO();
                    rdo.DEPARTMENT_ID = item.First().DEPARTMENT_ID;
                    //key tổng số lượng chỉ định
                    rdo.AMOUNT_TMH = item.Where(o => o.SERVICE_PARENT_CODE == "NSTMH" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HS = item.Where(o => o.SERVICE_PARENT_CODE == "XNHS" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CT = item.Where(o => o.SERVICE_PARENT_CODE == "CLVT" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DTD = item.Where(o => o.SERVICE_PARENT_CODE == "TDCN" && (o.SERVICE_NAME??"").ToLower().Contains("điện tim")&& o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DND = item.Where(o => o.SERVICE_PARENT_CODE == "DND" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_GPB = item.Where(o => o.SERVICE_PARENT_CODE == "GPB" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HH = item.Where(o => o.SERVICE_PARENT_CODE == "XNHH" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DT = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" &&(o.SERVICE_NAME??"").ToLower().Contains("đại tràng") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DD = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("dạ dày") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_POLYP = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("polyp") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_MAU = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("màu") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_THUONG = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("siêu âm") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_VS_PCR = item.Where(o => o.SERVICE_PARENT_CODE == "XNVS" && (o.SERVICE_NAME ?? "").ToLower().Contains("pcr") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_XQ = item.Where(o => o.SERVICE_PARENT_CODE == "XQUANG" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_MRI = item.Where(o => o.SERVICE_PARENT_CODE == "MRI" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.TOTAL_AMOUNT_KH = item.Where(o => o.SERVICE_PARENT_CODE == "KB" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_238 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("238") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_105_100 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("100")||(o.SERVICE_NAME ?? "").ToLower().Contains("105")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_210_109 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("210")||(o.SERVICE_NAME ?? "").ToLower().Contains("109")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_GOP = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 2")||(o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 3")||(o.SERVICE_NAME ?? "").ToLower().Contains("135")||(o.SERVICE_NAME ?? "").ToLower().Contains("130")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_TAI_TRO = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("tài trợ") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_PCR = item.Where(o => o.SERVICE_PARENT_CODE == "XNPCR" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);

                    //key tổng số lượng thực làm

                    rdo.AMOUNT_TMH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTMH" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HS_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNHS" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CT_TL = item.Where(o => o.SERVICE_PARENT_CODE == "CLVT" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DTD_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TDCN" && (o.SERVICE_NAME ?? "").ToLower().Contains("điện tim") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DND_TL = item.Where(o => o.SERVICE_PARENT_CODE == "DND" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_GPB_TL = item.Where(o => o.SERVICE_PARENT_CODE == "GPB" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNHH" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DT_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("đại tràng") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DD_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("dạ dày") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_POLYP_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("polyp") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_MAU_TL = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("màu") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_THUONG_TL = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("siêu âm") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_VS_PCR_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNVS" && (o.SERVICE_NAME ?? "").ToLower().Contains("pcr") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_XQ_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XQUANG" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_MRI_TL = item.Where(o => o.SERVICE_PARENT_CODE == "MRI" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.TOTAL_AMOUNT_KH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "KB" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_238_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("238") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_105_100_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("100") || (o.SERVICE_NAME ?? "").ToLower().Contains("105")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_210_109_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("210") || (o.SERVICE_NAME ?? "").ToLower().Contains("109")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_GOP_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 2") || (o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 3") || (o.SERVICE_NAME ?? "").ToLower().Contains("135") || (o.SERVICE_NAME ?? "").ToLower().Contains("130")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_TAI_TRO_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("tài trợ") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_PCR_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNPCR" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);

                   
                    rdo.DEPARTMENT_CODE = item.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = item.First().DEPARTMENT_NAME;
                    rdo.DIC_SV_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_SV_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_PAR_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_PAR_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_CATE_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_CATE_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));

                    rdo.STR_SV_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_SV_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_PAR_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ cha", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_PAR_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ cha", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_CATE_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.CATEGORY_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().CATEGORY_NAME??"Không có nhóm báo cáo", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_CATE_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.CATEGORY_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().CATEGORY_NAME ?? "Không có nhóm báo cáo", p.Sum(x => x.AMOUNT))).ToList());
                    ListRdoDepartment.Add(rdo);
                }
                var roomIds = ListHisRoom.Where(o=>o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(x=>x.ID).ToList();
                var groupByRoom = ListRdo.Where(x => roomIds.Contains(x.ROOM_ID)).GroupBy(x => x.ROOM_ID).ToList();
                foreach (var item in groupByRoom)
                {
                    Mrs00808RDO rdo = new Mrs00808RDO();
                    //key tổng số lượng chỉ định
                    rdo.AMOUNT_TMH = item.Where(o => o.SERVICE_PARENT_CODE == "NSTMH" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HS = item.Where(o => o.SERVICE_PARENT_CODE == "XNHS" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CT = item.Where(o => o.SERVICE_PARENT_CODE == "CLVT" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DTD = item.Where(o => o.SERVICE_PARENT_CODE == "TDCN" && (o.SERVICE_NAME ?? "").ToLower().Contains("điện tim") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DND = item.Where(o => o.SERVICE_PARENT_CODE == "DND" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_GPB = item.Where(o => o.SERVICE_PARENT_CODE == "GPB" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HH = item.Where(o => o.SERVICE_PARENT_CODE == "XNHH" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DT = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("đại tràng") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DD = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("dạ dày") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_POLYP = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("polyp") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_MAU = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("màu") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_THUONG = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("siêu âm") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_VS_PCR = item.Where(o => o.SERVICE_PARENT_CODE == "XNVS" && (o.SERVICE_NAME ?? "").ToLower().Contains("pcr") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_XQ = item.Where(o => o.SERVICE_PARENT_CODE == "XQUANG" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_MRI = item.Where(o => o.SERVICE_PARENT_CODE == "MRI" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.TOTAL_AMOUNT_KH = item.Where(o => o.SERVICE_PARENT_CODE == "KB" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_238 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("238") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_105_100 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("100") || (o.SERVICE_NAME ?? "").ToLower().Contains("105")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_210_109 = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("210") || (o.SERVICE_NAME ?? "").ToLower().Contains("109")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_GOP = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 2") || (o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 3") || (o.SERVICE_NAME ?? "").ToLower().Contains("135") || (o.SERVICE_NAME ?? "").ToLower().Contains("130")) && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_TAI_TRO = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("tài trợ") && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_PCR = item.Where(o => o.SERVICE_PARENT_CODE == "XNPCR" && o.TYPE == CHIDINH).Sum(x => x.AMOUNT);
      

                    //key tổng số lượng thực làm

                    rdo.AMOUNT_TMH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTMH" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HS_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNHS" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CT_TL = item.Where(o => o.SERVICE_PARENT_CODE == "CLVT" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DTD_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TDCN" && (o.SERVICE_NAME ?? "").ToLower().Contains("điện tim") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_DND_TL = item.Where(o => o.SERVICE_PARENT_CODE == "DND" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_GPB_TL = item.Where(o => o.SERVICE_PARENT_CODE == "GPB" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_HH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNHH" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DT_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("đại tràng") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_DD_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("dạ dày") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_NS_POLYP_TL = item.Where(o => o.SERVICE_PARENT_CODE == "NSTH" && (o.SERVICE_NAME ?? "").ToLower().Contains("polyp") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_MAU_TL = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("màu") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_SA_THUONG_TL = item.Where(o => o.SERVICE_PARENT_CODE == "SA" && (o.SERVICE_NAME ?? "").ToLower().Contains("siêu âm") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_VS_PCR_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNVS" && (o.SERVICE_NAME ?? "").ToLower().Contains("pcr") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_XQ_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XQUANG" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_MRI_TL = item.Where(o => o.SERVICE_PARENT_CODE == "MRI" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.TOTAL_AMOUNT_KH_TL = item.Where(o => o.SERVICE_PARENT_CODE == "KB" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_238_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("238") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_105_100_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("100") || (o.SERVICE_NAME ?? "").ToLower().Contains("105")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_210_109_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("210") || (o.SERVICE_NAME ?? "").ToLower().Contains("109")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_GOP_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && ((o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 2") || (o.SERVICE_NAME ?? "").ToLower().Contains("mẫu gộp 3") || (o.SERVICE_NAME ?? "").ToLower().Contains("135") || (o.SERVICE_NAME ?? "").ToLower().Contains("130")) && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_CV_TAI_TRO_TL = item.Where(o => o.SERVICE_PARENT_CODE == "TNCV" && (o.SERVICE_NAME ?? "").ToLower().Contains("tài trợ") && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);
                    rdo.AMOUNT_PCR_TL = item.Where(o => o.SERVICE_PARENT_CODE == "XNPCR" && o.TYPE == THUCLAM).Sum(x => x.AMOUNT);



                    rdo.DIC_SV_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_SV_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_PAR_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_PAR_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_CATE_CD_AMOUNT = item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));
                    rdo.DIC_CATE_TL_AMOUNT = item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(x => x.AMOUNT));

                    rdo.STR_SV_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_SV_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_PAR_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ cha", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_PAR_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.SERVICE_PARENT_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().SERVICE_PARENT_NAME ?? "Không có dịch vụ cha", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_CATE_CD_AMOUNT = string.Join(";", item.Where(o => o.TYPE == CHIDINH).GroupBy(g => g.CATEGORY_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().CATEGORY_NAME ?? "Không có nhóm báo cáo", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.STR_CATE_TL_AMOUNT = string.Join(";", item.Where(o => o.TYPE == THUCLAM).GroupBy(g => g.CATEGORY_CODE ?? "NONE").Select(p => string.Format("{0}:{1}", p.First().CATEGORY_NAME ?? "Không có nhóm báo cáo", p.Sum(x => x.AMOUNT))).ToList());
                    rdo.ROOM_ID = item.First().ROOM_ID;
                    rdo.ROOM_CODE = item.First().ROOM_CODE;
                    rdo.ROOM_NAME = item.First().ROOM_NAME;
                    ListRdoRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoRoom = null;
                ListRdoDepartment = null;
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessRdo();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Department", ListRdoDepartment);
            objectTag.AddObjectData(store, "Room", ListRdoRoom);
        }
    }
}
