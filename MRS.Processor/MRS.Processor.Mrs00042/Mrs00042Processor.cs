using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Repository;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;
using System.Reflection;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisServiceMachine;
using MOS.MANAGER.HisMachine;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisDebate;

namespace MRS.Processor.Mrs00042
{
    public class Mrs00042Processor : AbstractProcessor
    {
        Mrs00042Filter castFilter = null;
        private List<Mrs00042RDO> ListRdo = new List<Mrs00042RDO>();
        List<Mrs00042RDO> listEkipRdo = new List<Mrs00042RDO>();
        List<Mrs00042RDO> listCTRdo = new List<Mrs00042RDO>();

        List<REQUEST_FILM_SIZE> listRequestFilmSize = new List<REQUEST_FILM_SIZE>();
        System.Reflection.PropertyInfo[] pFilmSize;
        CommonParam paramGet = new CommonParam();
        private List<MEDI_MATE_EXPEND> ListRdoExpend = new List<MEDI_MATE_EXPEND>();
        private List<HIS_MATERIAL_TYPE> ListFilmType = new List<HIS_MATERIAL_TYPE>();
        private List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>();
        private List<List<string>> listAreas = new List<List<string>>();
        List<long> CLS_SERVICE_TYPE_IDs = new List<long>() { 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT, 
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE .ID__XN
        };
        List<long> CLS_SERVICE_REQ_TYPE_IDs = new List<long>();
        Dictionary<long, V_HIS_SERVICE> dicParentService = new Dictionary<long, V_HIS_SERVICE>();
        List<HIS_PTTT_GROUP> ListPtttGroup = new List<HIS_PTTT_GROUP>();
        List<HIS_SERVICE_MACHINE> ListServiceMachine = new List<HIS_SERVICE_MACHINE>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        Dictionary<long, string> dicMachine = new Dictionary<long, string>();
        Dictionary<long, string> dicMachineCode = new Dictionary<long, string>();
        List<V_HIS_EKIP_USER> listEkipUser = new List<V_HIS_EKIP_USER>();
        List<DEBATE> listDebate = new List<DEBATE>();
        List<long> Holidays = new List<long>();
        public Mrs00042Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00042Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00042Filter)reportFilter);
            var result = true;
            try
            {
                GetHolidays();

                //xử lý tạo điều kiện lọc theo loại dịch vụ
                ProcessFilterServiceType();

                //get nhóm PTTT
                GetPtttGroup();

                //get trường cỡ phim
                GetFieldFilmSize();

                ListRdo = new ManagerSql().GetRdo(castFilter, CLS_SERVICE_TYPE_IDs);

                //get danh sách phim
                GetFilmType();

                //get dịch vụ, dịch vụ cha
                GetServicePar();

                //lọc theo dịch vụ
                FilterService();

                //xử lý điều kiện lọc mô tả
                ProcessFilterDescription();

                //get dữ liệu ekip
                GetEkip();

                //get dữ liệu hội chẩn
                GetDebate();

                //máy
                GetMachine();

                //dịch vụ máy
                GetServiceMachine();

                //lọc theo máy ở danh mục dịch vụ
                FilterMachine();

                //xử lý máy cho dịch vụ
                ProcessMachineForService();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = this.ListServiceMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                ListRdo = ListRdo.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
            }
        }

        private void GetDebate()
        {
            var listDepartmentIds = ListRdo.Select(p => p.EXECUTE_DEPARTMENT_ID ?? 0).Distinct().ToList();
            var skip = 0;
            while (listDepartmentIds.Count - skip > 0)
            {
                var listIds = listDepartmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                string query = string.Format("select dt.*, d.department_id,d.treatment_id from his_debate_type dt join his_debate d on dt.id = d.debate_type_id where d.department_id in ({0}) and d.create_time between {1} and {2}", string.Join(",", listIds), castFilter.TIME_FROM??castFilter.FINISH_TIME_FROM, castFilter.TIME_TO??castFilter.FINISH_TIME_TO);
                var listSub = new MOS.DAO.Sql.SqlDAO().GetSql<DEBATE>(query);
                if (listSub != null)
                {
                    listDebate.AddRange(listSub);
                }
            }
        }

        private void GetEkip()
        {
            var listServiceReqIds = ListRdo.Select(p => p.EKIP_ID ?? 0).Distinct().ToList();
            int skip = 0;
            while (listServiceReqIds.Count - skip > 0)
            {
                var listIds = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisEkipUserViewFilterQuery ekipFilter = new HisEkipUserViewFilterQuery();
                ekipFilter.EKIP_IDs = listIds;
                var listSub = new HisEkipUserManager().GetView(ekipFilter);
                if (listSub != null)
                {
                    listEkipUser.AddRange(listSub);
                }
            }
        }

        private void ProcessFilterDescription()
        {
            if (!string.IsNullOrWhiteSpace(castFilter.DE_AREAs))
            {
                string[] deAreas = castFilter.DE_AREAs.Split(',');
                if (deAreas.Length > 0)
                {
                    for (int i = 0; i < deAreas.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(deAreas[i]))
                        {
                            int index = deAreas[i].IndexOf("->");
                            if (index > 0 && index < deAreas[i].Length - 2)
                            {
                                List<string> area = new List<string>();
                                string Begin = deAreas[i].Substring(0, index);
                                string End = deAreas[i].Substring(index + 2);
                                if (!string.IsNullOrWhiteSpace(Begin) && !string.IsNullOrWhiteSpace(End))
                                {
                                    area.Add(Begin);
                                    area.Add(End);
                                    listAreas.Add(area);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FilterService()
        {
            if (castFilter.EXACT_PARENT_SERVICE_IDs != null)
            {
                ListService = ListService.Where(o => castFilter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID ?? 0) || castFilter.IS_ADD_NULL_PARENT == true && CLS_SERVICE_TYPE_IDs.Contains(o.SERVICE_TYPE_ID) && o.PARENT_ID == null).ToList();
            }
            if (castFilter.SERVICE_IDs != null)
            {
                ListService = ListService.Where(o => castFilter.SERVICE_IDs.Contains(o.ID)).ToList();
            }
            ListRdo = ListRdo.Where(o => ListService.Exists(p => p.ID == o.SERVICE_ID)).ToList();
        }

        private void GetServicePar()
        {
            HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
            ListService = new MOS.MANAGER.HisService.HisServiceManager().GetView(serviceFilter);
            ListService = ListService.Where(o => CLS_SERVICE_TYPE_IDs.Contains(o.SERVICE_TYPE_ID)).ToList();
            dicParentService = ListService.ToDictionary(p => p.ID, q => ListService.FirstOrDefault(o => o.ID == q.PARENT_ID));
        }

        private void GetFilmType()
        {
            var materialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager().Get(new HisMaterialTypeFilterQuery());
            if (IsNotNullOrEmpty(materialType))
            {
                ListFilmType = materialType.Where(o => o.IS_FILM == 1).ToList();
            }
        }

        private void GetFieldFilmSize()
        {
            pFilmSize = Properties.Get<Mrs00042RDO>();
            pFilmSize = pFilmSize.Where(o => o.Name.StartsWith("IS_FILM_SIZE_")).ToArray();
        }

        private void GetPtttGroup()
        {
            ListPtttGroup = new HisPtttGroupManager(paramGet).Get(new HisPtttGroupFilterQuery());
        }

        private void ProcessFilterServiceType()
        {
            if (castFilter.SVT_LIMIT_CODE != null)
            {
                this.CLS_SERVICE_TYPE_IDs = HisServiceTypeCFG.HisServiceTypes.Where(o => string.Format(",{0},", castFilter.SVT_LIMIT_CODE).Contains(string.Format(",{0},", o.SERVICE_TYPE_CODE))).Select(p => p.ID).ToList();

            }
            
            if (castFilter.SERVICE_TYPE_IDs != null)
            {
                this.CLS_SERVICE_TYPE_IDs = this.CLS_SERVICE_TYPE_IDs.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o)).ToList();

            }
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__GPBL);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KHAC);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PHCN);
            if (this.CLS_SERVICE_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN))
                this.CLS_SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN);
        }

        private void ProcessMachineForService()
        {
            this.dicMachine = this.ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => string.Join("; ", this.ListMachine.Where(o => q.Select(r => r.MACHINE_ID).ToList().Contains(o.ID)).Select(s => s.MACHINE_NAME).ToList()));
            this.dicMachineCode = this.ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => string.Join("; ", this.ListMachine.Where(o => q.Select(r => r.MACHINE_ID).ToList().Contains(o.ID)).Select(s => s.MACHINE_CODE).ToList()));
        }

        private void GetServiceMachine()
        {
            HisServiceMachineFilterQuery filterServiceMachine = new HisServiceMachineFilterQuery();
            this.ListServiceMachine = new HisServiceMachineManager().Get(filterServiceMachine);
        }

        private void GetMachine()
        {
            HisMachineFilterQuery filterMachine = new HisMachineFilterQuery();
            this.ListMachine = new HisMachineManager().Get(filterMachine);
        }


        private void GetHolidays()
        {
            try
            {
                List<string> strHolidays = new List<string>() { "1_1", "30_4", "1_5", "2_9" };
                List<string> strLunarHolidays = new List<string>() { "10_3", "30_12", "29_12", "1_1", "2_1", "3_1" };

                List<DateTime> dateHolidays = new List<DateTime>();
                int year = DateTime.Today.Year;
                DateTime startDate = new DateTime(year - 1, 1, 1);
                DateTime endDate = new DateTime(year + 1, 1, 1);
                for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                {
                    if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }

                    //var vcal = new convertSolar2Lunar();
                    //int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    //var tempDay = arr[0] + "_" + arr[1];
                    if (strHolidays.Contains(string.Format("{0}_{1}", i.Day, i.Month)))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }

                    var vcal = new convertSolar2Lunar();
                    int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    var tempDay = arr[0] + "_" + arr[1];
                    if (strLunarHolidays.Contains(tempDay))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
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

                if (ListRdo != null && ListRdo.Count > 0)
                {
                    ListRdo = ListRdo.OrderBy(d => d.TDL_PATIENT_ID).ToList();
                    if (ListRdo != null && ListRdo.Count > 0)
                    {
                        CommonParam param = new CommonParam();
                        int start = 0;
                        int count = ListRdo.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                            List<Mrs00042RDO> sereServs = ListRdo.Skip(start).Take(limit).ToList();
                            var ekipIds = sereServs.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                            List<V_HIS_EKIP_USER> listEkipSub = listEkipUser.Where(o => ekipIds.Contains(o.EKIP_ID)).ToList();
                            ProcessWithPTTTGroup(sereServs);
                            ProcessGroupExecute(sereServs, listEkipSub);
                            ProcessCTServices(sereServs);
                            ProcessOneSereServFromList(sereServs);

                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }

                        ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ToList();
                        List<HIS_SERE_SERV> MediMates = GetMediMate(ListRdo.Select(s => s.TREATMENT_ID??0).Distinct().ToList());
                        List<HIS_SERE_SERV> sereServsExpend = MediMates.Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.PARENT_ID != null).ToList();
                        var dicParent = MediMates.GroupBy(o => o.PARENT_ID ?? 0).ToDictionary(p=>p.Key,q=>q.ToList());

                        Dictionary<long, List<HIS_SERE_SERV>> dicExpend = new Dictionary<long, List<HIS_SERE_SERV>>();
                        if (IsNotNullOrEmpty(sereServsExpend))
                        {
                            foreach (var item in sereServsExpend)
                            {
                                if (!dicExpend.ContainsKey(item.PARENT_ID ?? 0))
                                {
                                    dicExpend[item.PARENT_ID ?? 0] = new List<HIS_SERE_SERV>();
                                }

                                dicExpend[item.PARENT_ID ?? 0].Add(item);
                            }
                        }

                        foreach (var item in ListRdo)
                        {
                            if (dicParent.ContainsKey(item.SS_ID))
                            {
                                item.MEDICINE_PRICE_EXPEND = dicParent[item.SS_ID].Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(o => o.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                                item.MEDICINE_EXPENDs = string.Join(";",dicParent[item.SS_ID].Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Select(o => o.TDL_SERVICE_CODE).Distinct().ToList());
                                item.MATERIAL_PRICE_EXPEND = dicParent[item.SS_ID].Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(o => o.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                                item.MATERIAL_EXPENDs = string.Join(";", dicParent[item.SS_ID].Where(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Select(o => o.TDL_SERVICE_CODE).Distinct().ToList());
                                item.MEDICINE_PRICE = dicParent[item.SS_ID].Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(o => o.VIR_TOTAL_PRICE ?? 0);
                                item.MEDICINEs = string.Join(";", dicParent[item.SS_ID].Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Select(o => o.TDL_SERVICE_CODE).Distinct().ToList());
                                item.MATERIAL_PRICE = dicParent[item.SS_ID].Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(o => o.VIR_TOTAL_PRICE ?? 0);
                                item.MATERIALs = string.Join(";", dicParent[item.SS_ID].Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Select(o => o.TDL_SERVICE_CODE).Distinct().ToList());
                            }
                            if (dicExpend.ContainsKey(item.SS_ID))
                            {
                                List<HIS_SERE_SERV> medicines = dicExpend[item.SS_ID].Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList();
                                List<HIS_SERE_SERV> materials = dicExpend[item.SS_ID].Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                                List<HIS_SERE_SERV> films = new List<HIS_SERE_SERV>();
                                if (IsNotNullOrEmpty(materials))
                                {
                                    films = materials.Where(o => ListFilmType.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                                    materials = materials.Where(o => !films.Select(s => s.ID).Contains(o.ID)).ToList();
                                }

                                int maxCount = medicines.Count;
                                if (materials.Count > maxCount) maxCount = materials.Count;
                                if (films.Count > maxCount) maxCount = films.Count;

                                for (int i = 0; i < maxCount; i++)
                                {
                                    MEDI_MATE_EXPEND ado = new MEDI_MATE_EXPEND();
                                    Inventec.Common.Mapper.DataObjectMapper.Map<MEDI_MATE_EXPEND>(ado, item);

                                    if (i < medicines.Count)
                                    {
                                        ado.MEDICINE_AMOUNT = medicines[i].AMOUNT;
                                        ado.MEDICINE_PRICE = medicines[i].PRICE * (1 + medicines[i].VAT_RATIO);
                                        ado.MEDICINE_TOTAL_PRICE = (ado.MEDICINE_AMOUNT ?? 0) * (ado.MEDICINE_PRICE ?? 0);
                                        ado.MEDICINE_TYPE_CODE = medicines[i].TDL_SERVICE_CODE;
                                        ado.MEDICINE_TYPE_NAME = medicines[i].TDL_SERVICE_NAME;
                                    }

                                    if (i < materials.Count)
                                    {
                                        ado.MATERIAL_AMOUNT = materials[i].AMOUNT;
                                        ado.MATERIAL_PRICE = materials[i].PRICE * (1 + materials[i].VAT_RATIO);
                                        ado.MATERIAL_TOTAL_PRICE = (ado.MATERIAL_AMOUNT ?? 0) * (ado.MATERIAL_PRICE ?? 0);
                                        ado.MATERIAL_TYPE_CODE = materials[i].TDL_SERVICE_CODE;
                                        ado.MATERIAL_TYPE_NAME = materials[i].TDL_SERVICE_NAME;
                                    }

                                    if (i < films.Count)
                                    {
                                        ado.FILM_AMOUNT = films[i].AMOUNT;
                                        ado.FILM_PRICE = films[i].PRICE * (1 + films[i].VAT_RATIO);
                                        ado.FILM_TOTAL_PRICE = (ado.FILM_AMOUNT ?? 0) * (ado.FILM_PRICE ?? 0);
                                        ado.FILM_CODE = films[i].TDL_SERVICE_CODE;
                                        ado.FILM_NAME = films[i].TDL_SERVICE_NAME;
                                    }
                                    if (true)
                                    {

                                    }
                                    ListRdoExpend.Add(ado);
                                }
                            }
                            else
                            {
                                MEDI_MATE_EXPEND ado = new MEDI_MATE_EXPEND();
                                Inventec.Common.Mapper.DataObjectMapper.Map<MEDI_MATE_EXPEND>(ado, item);
                                ListRdoExpend.Add(ado);
                            }
                        }
                        GroupByKey();
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<HIS_SERE_SERV> GetMediMate(List<long> treatmentIds)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            int skip = 0;
            while (treatmentIds.Count - skip > 0)
            {
                var listId = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                ssFilter.TREATMENT_IDs = listId;
                ssFilter.HAS_EXECUTE = true;
                ssFilter.TDL_SERVICE_TYPE_IDs = new List<long>() { 
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, 
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT };
                var ss = new HisSereServManager().Get(ssFilter);

                if (IsNotNullOrEmpty(ss))
                {
                    ss = ss.Where(o => o.SERVICE_REQ_ID != null && o.IS_DELETE == 0 && o.AMOUNT > 0).ToList();
                    result.AddRange(ss);
                }
            }
            return result;
        }

        private void ProcessGroupExecute(List<Mrs00042RDO> listEkipDetailRdo, List<V_HIS_EKIP_USER> listEkipSub)
        {
            try
            {
                #region Tổng hợp theo người có vai trò trong các ekip
                if (listEkipSub != null)
                {
                    foreach (var item in listEkipSub)
                    {
                        var sereServ = listEkipDetailRdo.FirstOrDefault(p => p.EKIP_ID == item.EKIP_ID);
                        Mrs00042RDO rdo = new Mrs00042RDO();
                        rdo.EXECUTE_USERNAME = item.USERNAME;
                        rdo.EXECUTE_LOGINNAME = item.LOGINNAME;
                        rdo.EXECUTE_ROLE_CODE = item.EXECUTE_ROLE_CODE;
                        if (sereServ != null)
                        {
                            var service = ListService.FirstOrDefault(p => p.ID == sereServ.SERVICE_ID && p.PTTT_GROUP_ID != null);
                            if (service != null && !string.IsNullOrEmpty(service.PTTT_GROUP_NAME))
                            {
                                if (service.PTTT_GROUP_NAME.Contains("1"))
                                {
                                    rdo.PTTT_GROUP_NAME = "1";
                                }
                                else if (service.PTTT_GROUP_NAME.Contains("2"))
                                {
                                    rdo.PTTT_GROUP_NAME = "2";
                                }
                                else if (service.PTTT_GROUP_NAME.Contains("3"))
                                {
                                    rdo.PTTT_GROUP_NAME = "3";
                                }
                                else
                                {
                                    rdo.PTTT_GROUP_NAME = "4";
                                }
                            }
                        }
                        listEkipRdo.Add(rdo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessWithPTTTGroup(List<Mrs00042RDO> listEkipDetailRdo)
        {
            try
            {
                #region chi tiết theo mẫu MRS0066212
                if (listEkipDetailRdo != null)
                {
                    foreach (var item in listEkipDetailRdo)
                    {
                        var debate = listDebate.FirstOrDefault(p => p.DEPARTMENT_ID == item.EXECUTE_DEPARTMENT_ID && p.TREATMENT_ID == item.TREATMENT_ID);
                        if (debate != null)
                        {
                            item.DEBATE_TYPE_NAME = debate.DEBATE_TYPE_NAME;
                        }
                        item.DIC_EXECUTE_USERNAME = new Dictionary<string, string>();
                        item.DIC_EXECUTE_USERNAME = listEkipUser.Where(p => p.EKIP_ID == (item.EKIP_ID ?? 0)).GroupBy(p => p.EXECUTE_ROLE_CODE).ToDictionary(y => y.Key, p => string.Join(",", p.Select(o => o.USERNAME).ToList()));
                    }
                }
                #endregion


            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessCTServices(List<Mrs00042RDO> listRdo)
        {
            try
            {
                if (listRdo != null)
                {
                    foreach (var item in listRdo)
                    {
                        var hour = item.INTRUCTION_TIME_NUM - item.INTRUCTION_DATE_NUM;
                        //LogSystem.Info("hour:" + hour);
                        //LogSystem.Info("parent service code:" + item.PARENT_SERVICE_CODE);
                        //LogSystem.Info("END_TIME:" + item.END_TIME);
                        if (!Holidays.Contains(item.INTRUCTION_TIME_NUM))
                        {
                            DateTime? date = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.INTRUCTION_TIME_NUM);
                            //LogSystem.Info("date: " + date.Value);
                            if (date.Value.DayOfWeek != DayOfWeek.Saturday && date.Value.DayOfWeek != DayOfWeek.Sunday)
                            {
                                if ((hour >= 65500 && hour <= 110500) || (hour >= 132500 && hour <= 163500))
                                {
                                    continue;
                                }
                                else
                                {
                                    if (item.PARENT_SERVICE_CODE != "CCLVT" && item.PARENT_SERVICE_CODE != "CCHT")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (item.END_TIME == null)
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.PARENT_SERVICE_CODE != "CCLVT" && item.PARENT_SERVICE_CODE != "CCHT")
                            {
                                continue;
                            }
                            else
                            {
                                if (item.END_TIME == null)
                                {
                                    continue;
                                }
                            }
                        }

                        listCTRdo.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void GroupByKey()
        {
            try
            {

                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SS") && this.dicDataFilter["KEY_GROUP_SS"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SS"].ToString()))
                {
                    GroupByKey(this.dicDataFilter["KEY_GROUP_SS"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void GroupByKey(string GroupKeySS)
        {
            var group = ListRdo.GroupBy(o => string.Format(GroupKeySS, o.TREATMENT_CODE, o.PATIENT_TYPE_CODE, o.REQUEST_LOGINNAME, o.INTRUCTION_TIME, o.START_TIME, o.BEGIN_TIME, o.END_TIME, o.FINISH_TIME, o.SERVICE_ID)).ToList();

            ListRdo.Clear();
            Mrs00042RDO rdo;
            List<Mrs00042RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00042RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00042RDO();
                listSub = item.ToList<Mrs00042RDO>();

                foreach (var field in pi)
                {

                    field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                }
                rdo.AMOUNT = listSub.Sum(o => o.AMOUNT);
                rdo.AMOUNT_TT = listSub.Sum(o => o.AMOUNT_TT);
                rdo.AMOUNT_TTS = listSub.Sum(o => o.AMOUNT_TTS);
                rdo.VIR_TOTAL_HEIN_PRICE = listSub.Sum(o => o.VIR_TOTAL_HEIN_PRICE);
                rdo.VIR_TOTAL_PRICE = listSub.Sum(o => o.VIR_TOTAL_PRICE);
                rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE);
                ListRdo.Add(rdo);
            }
        }

        private Mrs00042RDO IsMeaningful(List<Mrs00042RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").OrderByDescending(p => field.GetValue(p)).FirstOrDefault() ?? new Mrs00042RDO();
        }

        private void ProcessOneSereServFromList(List<Mrs00042RDO> sereServs)
        {
            try
            {
                long PatientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                pFilmSize = pFilmSize.Where(o => o.Name.StartsWith("IS_FILM_SIZE_")).ToArray();

                foreach (var rdo in sereServs)
                {
                    var service = ListService.FirstOrDefault(o => o.ID == rdo.SERVICE_ID) ?? new V_HIS_SERVICE();


                    rdo.EXECUTE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.INTRUCTION_TIME_1 ??0);
                    rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.INTRUCTION_TIME_NUM);
                    var ptttGroup = ListPtttGroup.Where(x => x.ID == service.PTTT_GROUP_ID).FirstOrDefault();
                    rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.FINISH_TIME ?? 0);
                    if (ptttGroup != null)
                    {
                        rdo.PTTT_GROUP_NAME = ptttGroup.PTTT_GROUP_NAME;
                    }
                    rdo.REQUEST_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.EXECUTE_ROOM = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_NAME;
                    rdo.EXECUTE_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_NAME;
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    var tdlPatientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    rdo.TDL_PATIENT_TYPE_CODE = tdlPatientType.PATIENT_TYPE_CODE;
                    rdo.TDL_PATIENT_TYPE_NAME = tdlPatientType.PATIENT_TYPE_NAME;
                    rdo.PATIENT_TYPE_NAME_02 = tdlPatientType.PATIENT_TYPE_NAME;
                    rdo.TIME_DIIM_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.START_TIME ?? 0);
                    

                    rdo.ICD_DIIM = rdo.ICD_NAME + ";" + rdo.ICD_TEXT;
                    rdo.ICD_DIIM_CODE = rdo.ICD_CODE + ";" + rdo.ICD_SUB_CODE;
                    //thêm thông tin nhóm cha
                    if (dicParentService.ContainsKey(rdo.SERVICE_ID) && dicParentService[rdo.SERVICE_ID] != null)
                    {
                        rdo.PARENT_SERVICE_ID = dicParentService[rdo.SERVICE_ID].ID;
                        rdo.PARENT_SERVICE_CODE = dicParentService[rdo.SERVICE_ID].SERVICE_CODE;
                        rdo.PARENT_SERVICE_NAME = dicParentService[rdo.SERVICE_ID].SERVICE_NAME;
                    }

                    CalcuatorAge(rdo);
                    rdo.IS_BHYT = rdo.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt ? "X" : "";
                    rdo.TDL_HEIN_CARD_NUMBER = rdo.TDL_HEIN_CARD_NUMBER;
                    if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        rdo.HEIN_CARD_NUMBER = rdo.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "BHYT";
                    }
                    else if (rdo.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        rdo.PATIENT_TYPE_NAME_01 = "VP";
                    else
                    {
                        rdo.HEIN_CARD_NUMBER = rdo.HEIN_CARD_NUMBER;
                        rdo.PATIENT_TYPE_NAME_01 = "XHH";
                    }

                    if (rdo.FILM_SIZE_ID.HasValue)
                    {
                        this.ProcessorFilmSize(new List<long>() { rdo.FILM_SIZE_ID.Value }, rdo);
                    }
                    if (dicMachine.ContainsKey(rdo.SERVICE_ID))
                    {
                        rdo.MACHINE_NAME = dicMachine[rdo.SERVICE_ID];
                    }
                    if (dicMachineCode.ContainsKey(rdo.SERVICE_ID))
                    {
                        rdo.MACHINE_CODE = dicMachineCode[rdo.SERVICE_ID];
                    }
                    var machine = ListMachine.FirstOrDefault(o => o.ID == rdo.MACHINE_ID);
                    if (machine != null)
                    {
                        rdo.EXECUTE_MACHINE_NAME = machine.MACHINE_NAME;
                        rdo.EXECUTE_MACHINE_CODE = machine.MACHINE_CODE;
                    }
                    rdo.DIIM_TYPE_NAME = service.DIIM_TYPE_NAME;
                    if (this.listAreas.Count > 0 && !string.IsNullOrWhiteSpace(rdo.DESCRIPTION))
                    {
                        foreach (List<string> item in this.listAreas)
                        {
                            int indexBegin = rdo.DESCRIPTION.IndexOf(item[0]);
                            int indexEnd = rdo.DESCRIPTION.IndexOf(item[1], indexBegin > 0 ? indexBegin : 0);
                            if (indexBegin > 0 && indexEnd > 0 && indexBegin < indexEnd)
                            {
                                rdo.DE_AREA += rdo.DESCRIPTION.Substring(indexBegin + item[0].Length, indexEnd - (indexBegin + item[0].Length));
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


        private void ProcessorFilmSize(List<long> _filmSizeIds, Mrs00042RDO rdo)
        {
            if (_filmSizeIds == null)
                return;
            int count = pFilmSize.Length;
            if (HisFilmSizeCFG.FILM_SIZEs != null && count > HisFilmSizeCFG.FILM_SIZEs.Count)
                count = HisFilmSizeCFG.FILM_SIZEs.Count;
            for (int i = 0; i < count; i++)
            {
                if (_filmSizeIds.Contains(HisFilmSizeCFG.FILM_SIZEs[i].ID))
                {
                    pFilmSize[i].SetValue(rdo, "X");
                }
            }
        }

        private void SetKeyFilmSizeName(ref Dictionary<string, object> DicKey)
        {
            if (DicKey == null)
                return;
            int count = HisFilmSizeCFG.FILM_SIZEs.Count;
            for (int i = 0; i < count; i++)
            {
                DicKey[string.Format("FILM_SIZE_{0}_NAME", i + 1)] = HisFilmSizeCFG.FILM_SIZEs[i].FILM_SIZE_NAME;
            }
        }

        private void CalcuatorAge(Mrs00042RDO rdo)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(rdo.TDL_PATIENT_DOB ?? 0);
                if (tuoi >= 0)
                {
                    if (rdo.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(rdo.TDL_PATIENT_DOB ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", ((Mrs00042Filter)this.reportFilter).TIME_FROM != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).TIME_FROM ?? 0) : Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));// + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", ((Mrs00042Filter)this.reportFilter).TIME_TO != null ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).TIME_TO ?? 0) : Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).FINISH_TIME_TO ?? 0));// + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00042Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            this.SetKeyFilmSizeName(ref dicSingleTag);
            if (((Mrs00042Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00042Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }

            if (((Mrs00042Filter)this.reportFilter).EXE_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => ((Mrs00042Filter)this.reportFilter).EXE_ROOM_ID == o.ID);
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
            }

            if (((Mrs00042Filter)this.reportFilter).EXECUTE_ROOM_IDs != null)
            {
                var room = HisRoomCFG.HisRooms.Where(o => ((Mrs00042Filter)this.reportFilter).EXECUTE_ROOM_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("EXECUTE_ROOM_NAME", string.Join(" - ", room.Select(o => o.ROOM_NAME).ToList()));
            }

            if (((Mrs00042Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00042Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }

            //objectTag.AddObjectData(store, "Report", ListRdo.GroupBy(o => o.finish_Time).ToList());
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "ReportExpend", ListRdoExpend);

            objectTag.AddObjectData(store, "ReportEkip", ListRdo.Where(p => !string.IsNullOrEmpty(p.PTTT_GROUP_NAME)).OrderBy(p => p.PATIENT_CODE).ThenBy(P => P.BEGIN_TIME).ToList());
            objectTag.AddObjectData(store, "ReportUser", listEkipRdo.OrderBy(p => p.EXECUTE_LOGINNAME).ToList());
            LogSystem.Info("listCT:" + listCTRdo.Count);
            objectTag.AddObjectData(store, "ReportCT", listCTRdo.OrderBy(p => p.TREATMENT_CODE).ThenBy(p => p.END_TIME).ToList());

            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("EXECUTE_MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.MACHINE_IDs != null)
            {
                var machine = this.ListMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
            }
            if (castFilter.PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
            if (castFilter.TDL_PATIENT_TYPE_IDs != null)
            {
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
            }
        }
    }
}
