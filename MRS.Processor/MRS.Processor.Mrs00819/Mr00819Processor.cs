using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00819
{
    public class Mr00819Processor : AbstractProcessor
    {
        public Mrs00819Filter filter = new Mrs00819Filter();
        public List<Mrs00819RDO> listRdo = new List<Mrs00819RDO>();
        public List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        public List<HIS_TREATMENT> listTreatmentDK = new List<HIS_TREATMENT>();// danh sách bệnh nhân đầu kỳ
        public List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        public List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        public List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        public List<HIS_SERVICE> listServiceParent = new List<HIS_SERVICE>();
        public List<HIS_PTTT_GROUP> listPtttGroup = new List<HIS_PTTT_GROUP>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        public List<V_HIS_BED> ListBed = new List<V_HIS_BED>();
        public List<V_HIS_BLOOD> ListBlood = new List<V_HIS_BLOOD>();
        public List<HIS_PATIENT_TYPE> listPatientType = new List<HIS_PATIENT_TYPE>();
        int DiffDateReport = 1;
        public Mr00819Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00819Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00819Filter)reportFilter;
            bool result = false;
            try
            {

                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.FINISH_TIME_FROM = filter.TIME_FROM;
                serviceReqFilter.FINISH_TIME_TO = filter.TIME_TO;
                listServiceReq = new HisServiceReqManager().Get(serviceReqFilter);
                listServiceReq = listServiceReq.Where(x => x.IS_DELETE == 0).ToList();
                var serviceReqIds = listServiceReq.Select(x => x.ID).ToList();
                var treatmentIds = listServiceReq.Select(x => x.TREATMENT_ID).Distinct().ToList();
                //HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                //treatmentFilter.IDs = treatmentIds;
                //listTreatment = new HisTreatmentManager().Get(treatmentFilter);
                listTreatment = new ManagerSql().GetTreatmentAll(filter);
                HisTreatmentFilterQuery treatDKFilter = new HisTreatmentFilterQuery();
                treatDKFilter.IN_TIME_TO = filter.TIME_FROM;
                treatDKFilter.IS_PAUSE = false;
                listTreatmentDK = new HisTreatmentManager().Get(treatDKFilter);
                var skip = 0;
                while (serviceReqIds.Count - skip > 0)
                {
                    var limit = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                    sereServFilter.SERVICE_REQ_IDs = limit;
                    var sereServs = new HisSereServManager().Get(sereServFilter);
                    sereServs = sereServs.Where(x => x.IS_DELETE == 0).ToList();
                    listSereServ.AddRange(sereServs);
                }
                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                serviceFilter.IDs = listSereServ.Select(x => x.SERVICE_ID).Distinct().ToList();
                listService = new HisServiceManager().Get(serviceFilter);
                HisPtttGroupFilterQuery ptttFilter = new HisPtttGroupFilterQuery();
                listPtttGroup = new HisPtttGroupManager().Get(ptttFilter);
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IS_CLINICAL = true;
                listDepartment = new HisDepartmentManager().Get(departmentFilter);
                var departmentIds = listDepartment.Select(x=>x.ID).Distinct().ToList();

                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                listRoom = listRoom.Where(x =>x.IS_EXAM==1).ToList();
                listRoom = listRoom.Where(x => departmentIds.Contains(x.DEPARTMENT_ID)).ToList();
               
                this.DiffDateReport = DateDiff.diffDate(filter.TIME_FROM, filter.TIME_TO);
                ListBed = new HisBedManager().GetView(new HisBedViewFilterQuery());
                var ServiceIds = listService.Where(x => x.PARENT_ID != null).Select(x => x.PARENT_ID ?? 0).Distinct().ToList();
                HisServiceFilterQuery serviceParentFilter = new HisServiceFilterQuery();
                serviceParentFilter.IDs = ServiceIds;
                listServiceParent = new HisServiceManager().Get(serviceParentFilter);
                var bloodIds = listSereServ.Where(x => x.BLOOD_ID != null).Select(x => x.BLOOD_ID ?? 0).ToList();
                HisBloodViewFilterQuery bloodFilter = new HisBloodViewFilterQuery();
                bloodFilter.IDs = bloodIds;
                ListBlood = new HisBloodManager().GetView(bloodFilter);
                listPatientType = new HisPatientTypeManager().Get(new HisPatientTypeFilterQuery());
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                foreach (var item in listSereServ)
                {
                    Mrs00819RDO rdo = new Mrs00819RDO();
                    var trea = listTreatment.Where(x => x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
                    if (trea != null)
                    {
                        rdo.HEIN_CARD_NUMBER = trea.TDL_HEIN_CARD_NUMBER;
                    }

                    var lstService = listService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault();
                    if (lstService != null)
                    {
                        var parentService = listServiceParent.Where(x => x.ID == lstService.PARENT_ID).FirstOrDefault();
                        if (parentService != null)
                        {
                            if (parentService.SERVICE_CODE.Equals("XNHH"))
                            {
                                rdo.COUNT_HH = item.AMOUNT;
                            }
                            if (parentService.SERVICE_CODE.Equals("XNSH"))
                            {
                                rdo.COUNT_SH = item.AMOUNT;
                            }
                            if (parentService.SERVICE_CODE.Equals("XNVS"))
                            {
                                rdo.COUNT_VS = item.AMOUNT;
                            }
                            if (parentService.SERVICE_CODE.Equals("XNGPB"))
                            {
                                rdo.COUNT_GPB = item.AMOUNT;
                            }

                            if (parentService.SERVICE_CODE.Equals("XQSH"))
                            {
                                rdo.COUNT_XQ_KTS = item.AMOUNT;
                            }

                            if (parentService.SERVICE_CODE.Equals("CCLVT") || parentService.SERVICE_CODE.Equals("CCHT"))
                            {
                                rdo.COUNT_CT_SCANNER = item.AMOUNT;
                            }
                            if (parentService.SERVICE_CODE.Equals("XQTQ"))
                            {
                                rdo.COUNT_XQ = item.AMOUNT;
                            }
                        }
                    }
                    rdo.EXECUTE_DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID;
                    var department = listDepartment.Where(x => x.ID == item.TDL_REQUEST_DEPARTMENT_ID).FirstOrDefault();
                    if (department != null)
                    {
                        rdo.EXECUTE_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                        rdo.EXECUTE_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    }
                    if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                    {
                        var blood = ListBlood.Where(x => x.ID == item.BLOOD_ID).FirstOrDefault();
                        if (blood != null)
                        {
                            rdo.COUNT_MAU = blood.VOLUME;
                        }
                    }
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL)
                    {
                        rdo.COUNT_GPB = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                    {
                        rdo.COUNT_XN = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                    {
                        rdo.COUNT_SA = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.Contains("HIV"))
                    {
                        rdo.COUNT_HIV = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("điện tâm đồ") || item.TDL_SERVICE_NAME.ToLower().Contains("điện tim thường"))
                    {
                        rdo.COUNT_DTD = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("điện tâm đồ gắng sức"))
                    {
                        rdo.COUNT_DTDGS = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("điện cơ đồ"))
                    {
                        rdo.COUNT_DCD = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("điện não đồ"))
                    {
                        rdo.COUNT_DN = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("nội soi tiêu hóa")&&item.TDL_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                    {
                        rdo.COUNT_NS_TH = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("nội soi tai mũi họng") && item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                    {
                        rdo.COUNT_NS_TMH = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("thận nhân tạo"))
                    {
                        rdo.COUNT_TNT = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("đo hô hấp"))
                    {
                        rdo.COUNT_DHH = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("abi"))
                    {
                        rdo.COUNT_ABI = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("đo loãng xương"))
                    {
                        rdo.COUNT_DLX = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("đàn hồi gan"))
                    {
                        rdo.COUNT_SA_GAN = item.AMOUNT;
                    }
                    if (item.TDL_SERVICE_NAME.ToLower().Contains("coronavirus"))
                    {
                        if (rdo.HEIN_CARD_NUMBER != null)
                        {
                            if ((rdo.HEIN_CARD_NUMBER.Contains("CA")))
                            {
                                rdo.COUNT_COVI_PRC_BHYT_CA = item.AMOUNT;
                            }
                            else
                            {
                                rdo.COUNT_COVI_PRC_BHYT = item.AMOUNT;
                            }
                        }

                        else
                        {
                            rdo.COUNT_COVI_PRC_DVYC = item.AMOUNT;
                        }
                    }
                    var ptttId = listService.Where(x => x.ID == item.SERVICE_ID).FirstOrDefault();
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT)
                    {
                        if (ptttId != null)
                        {
                            var pttt = listPtttGroup.Where(x => x.ID == ptttId.PTTT_GROUP_ID).FirstOrDefault();
                            if (pttt != null)
                            {
                                if (pttt.PTTT_GROUP_NAME.Contains("1"))
                                {
                                    rdo.COUNT_PT_1 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("2"))
                                {
                                    rdo.COUNT_PT_2 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("3"))
                                {
                                    rdo.COUNT_PT_3 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("đặc biệt"))
                                {
                                    rdo.COUNT_PT_DB = item.AMOUNT;
                                }
                            }
                            else
                            {
                                rdo.COUNT_PT_0 = item.AMOUNT;
                            }
                        }
                    }
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                    {
                        if (ptttId != null)
                        {
                            var pttt = listPtttGroup.Where(x => x.ID == ptttId.PTTT_GROUP_ID).FirstOrDefault();
                            if (pttt != null)
                            {
                                if (pttt.PTTT_GROUP_NAME.Contains("1"))
                                {
                                    rdo.COUNT_TT_1 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("2"))
                                {
                                    rdo.COUNT_TT_2 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("3"))
                                {
                                    rdo.COUNT_TT_3 = item.AMOUNT;
                                }
                                if (pttt.PTTT_GROUP_NAME.Contains("đặc biệt"))
                                {
                                    rdo.COUNT_TT_DB = item.AMOUNT;
                                }
                            }
                            else
                            {
                                rdo.COUNT_PT_0 = item.AMOUNT;
                            }
                        }
                    }
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        if (item.HEIN_CARD_NUMBER != null)
                        {
                            if (item.HEIN_CARD_NUMBER.Contains("CA"))
                            {
                                rdo.COUNT_KH_BHYT_CA = item.AMOUNT;
                            }
                            else
                            {
                                rdo.COUNT_KH_BHYT = item.AMOUNT;
                            }
                        }
                        else
                        {
                            rdo.COUNT_KH_DVYC = item.AMOUNT;
                        }
                    }
                    //if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                    //{
                    //    rdo.NUM_DAY = item.AMOUNT;
                    //}
                    var roomCount = listRoom.Where(x => x.DEPARTMENT_ID == item.TDL_REQUEST_DEPARTMENT_ID).Count();
                    rdo.COUNT_ROOM = roomCount;
                   
                    //rdo.BED_PLAN = bedCount.Count();
                    //rdo.BED_TRUST = bedCount.Where(x => x.IS_ACTIVE == 1 && x.IS_DELETE == 0).Count();
                    
                    listRdo.Add(rdo);
                }
                foreach (var item in listDepartment)
                {
                    Mrs00819RDO rdo = new Mrs00819RDO();
                    var bedCount = ListBed.Where(x => x.DEPARTMENT_ID == item.ID).ToList();
                    rdo.EXECUTE_DEPARTMENT_ID = item.ID;
                    rdo.EXECUTE_DEPARTMENT_CODE = item.DEPARTMENT_CODE;
                    rdo.EXECUTE_DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                    rdo.BED_TRUST = item.REALITY_PATIENT_COUNT??0;
                    rdo.BED_PLAN = item.THEORY_PATIENT_COUNT??0;
                    var roomCount = listRoom.Where(x => x.DEPARTMENT_ID == item.ID&& x.ROOM_CODE.Substring(0,2)=="PK").ToList().Count;
                    rdo.COUNT_ROOM = roomCount;
                    rdo.BED_CT = bedCount.Where(x => x.BED_CODE.Contains("H")).Count();
                    Inventec.Common.Logging.LogSystem.Info(rdo.EXECUTE_DEPARTMENT_CODE + ": " + rdo.BED_TRUST.ToString());
                    listRdo.Add(rdo);
                }
                var group = listRdo.Where(x => x.EXECUTE_DEPARTMENT_NAME != null).GroupBy(x => x.EXECUTE_DEPARTMENT_NAME).ToList();
                listRdo.Clear();
                foreach (var item in group)
                {
                    Mrs00819RDO rdo = new Mrs00819RDO();
                    rdo.EXECUTE_DEPARTMENT_ID = item.First().EXECUTE_DEPARTMENT_ID;
                    rdo.EXECUTE_DEPARTMENT_CODE = item.First().EXECUTE_DEPARTMENT_CODE;
                    rdo.EXECUTE_DEPARTMENT_NAME = item.First().EXECUTE_DEPARTMENT_NAME;
                    if (item.Where(x=>x.BED_PLAN>0).FirstOrDefault()!=null)
                    {
                        rdo.BED_PLAN = item.Where(x => x.BED_PLAN > 0).FirstOrDefault().BED_PLAN ?? 0;
                    }
                    else
                    {
                        rdo.BED_PLAN = 0;
                    }
                    if (item.Where(x => x.BED_TRUST > 0).FirstOrDefault() != null)
                    {
                        rdo.BED_TRUST = rdo.BED_TRUST = item.Where(x => x.BED_TRUST > 0).FirstOrDefault().BED_TRUST;
                    }
                    else
                    {
                        rdo.BED_TRUST = 0;
                    }
                    if (item.Where(x => x.BED_CT > 0).FirstOrDefault() != null)
                    {
                        rdo.BED_CT = item.Where(x => x.BED_CT > 0).FirstOrDefault().BED_CT ?? 0;
                    }
                    else
                    {
                        rdo.BED_CT = 0;
                    }
                    

                    rdo.COUNT_ABI = item.Sum(x => x.COUNT_ABI);
                    rdo.COUNT_COVI_PRC_BHYT = item.Sum(x => x.COUNT_COVI_PRC_BHYT) - item.Sum(x => x.COUNT_COVI_PRC_BHYT_CA);
                    if (rdo.COUNT_COVI_PRC_BHYT < 0)
                    {
                        rdo.COUNT_COVI_PRC_BHYT = 0;
                    }
                    Inventec.Common.Logging.LogSystem.Info(rdo.EXECUTE_DEPARTMENT_CODE + ": " + rdo.BED_TRUST.ToString());
                    rdo.COUNT_COVI_PRC_BHYT_CA = item.Sum(x => x.COUNT_COVI_PRC_BHYT_CA);
                    rdo.COUNT_COVI_PRC_DVYC = item.Sum(x => x.COUNT_COVI_PRC_DVYC);
                    rdo.COUNT_CT_SCANNER = item.Sum(x => x.COUNT_CT_SCANNER);
                    rdo.COUNT_DCD = item.Sum(x => x.COUNT_DCD);
                    rdo.COUNT_DHH = item.Sum(x => x.COUNT_DHH);
                    rdo.COUNT_DLX = item.Sum(x => x.COUNT_DLX);
                    rdo.COUNT_DN = item.Sum(x => x.COUNT_DN);
                    rdo.COUNT_DTD = item.Sum(x => x.COUNT_DTD);
                    rdo.COUNT_DTDGS = item.Sum(x => x.COUNT_DTDGS);
                    rdo.COUNT_GPB = item.Sum(x => x.COUNT_GPB);
                    rdo.COUNT_HH = item.Sum(x => x.COUNT_HH);
                    rdo.COUNT_HIV = item.Sum(x => x.COUNT_HIV);
                    rdo.COUNT_KH_BHYT_CA = item.Sum(x => x.COUNT_KH_BHYT_CA);

                    rdo.COUNT_KH_BHYT = item.Sum(x => x.COUNT_KH_BHYT) - item.Sum(x => x.COUNT_KH_BHYT_CA);
                    if (rdo.COUNT_KH_BHYT<0)
                    {
                        rdo.COUNT_KH_BHYT = 0; 
                    }
                    rdo.COUNT_KH_DVYC = item.Sum(x => x.COUNT_KH_DVYC);
                    rdo.COUNT_MAU = item.Sum(x => x.COUNT_MAU);
                    rdo.COUNT_NS_TH = item.Sum(x => x.COUNT_NS_TH);
                    rdo.COUNT_NS_TMH = item.Sum(x => x.COUNT_NS_TMH);
                    rdo.COUNT_PT_0 = item.Sum(x => x.COUNT_PT_0);
                    rdo.COUNT_PT_1 = item.Sum(x => x.COUNT_PT_1);
                    rdo.COUNT_PT_2 = item.Sum(x => x.COUNT_PT_2);
                    rdo.COUNT_PT_3 = item.Sum(x => x.COUNT_PT_3);
                    rdo.COUNT_PT_DB = item.Sum(x => x.COUNT_PT_DB);
                    rdo.COUNT_ROOM = item.First().COUNT_ROOM;
                    rdo.COUNT_SA = item.Sum(x => x.COUNT_SA);
                    rdo.COUNT_SA_GAN = item.Sum(x => x.COUNT_SA_GAN);
                    rdo.COUNT_SH = item.Sum(x => x.COUNT_SH);
                    rdo.COUNT_TNT = item.Sum(x => x.COUNT_TNT);
                    var listTreatmentDepart = listTreatment.Where(x => x.END_DEPARTMENT_ID == rdo.EXECUTE_DEPARTMENT_ID).ToList();
                    var treatmentDepartmentBHYT = listTreatmentDepart.Where(x => x.TDL_PATIENT_TYPE_ID ==1).ToList();/// viện 199
                    var treatmentDepartmentBHYT_CA = treatmentDepartmentBHYT.Where(x => x.TDL_HEIN_CARD_NUMBER.Contains("CA")).ToList();
                    rdo.COUNT_TOTAL_TREATMENT_NGOAITRU = listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Count();
                                                         

                    rdo.COUNT_TREATMENT_BHYT_CA_NT = treatmentDepartmentBHYT_CA.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                    rdo.COUNT_TREATMENT_BHYT_NT = treatmentDepartmentBHYT.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                    rdo.COUNT_TREATMENT_DAY_NGOAITRU = listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && x.TREATMENT_DAY_COUNT != null).Sum(x => (long)x.TREATMENT_DAY_COUNT);
                    rdo.COUNT_TREATMENT_DAY_NT = listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && x.TREATMENT_DAY_COUNT != null).Sum(x => (long)x.TREATMENT_DAY_COUNT);
                    if (listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count() > 0)
                    {
                        rdo.COUNT_TREATMENT_DAY_TB_NT = rdo.COUNT_TREATMENT_DAY_NT / listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                    }
                    rdo.COUNT_TREATMENT_DVYC_NT = listTreatmentDepart.Where(x => x.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && x.TDL_PATIENT_TYPE_ID!=1).Count();

                    rdo.COUNT_TREATMENT_END = listTreatmentDepart.Where(x => x.IS_PAUSE == null).Count();
                    rdo.COUNT_TREATMENT_IN = listTreatmentDepart.Where(x => x.IN_TIME >= filter.TIME_FROM && x.IN_TIME <= filter.TIME_TO).Count();
                    rdo.COUNT_TREATMENT_BD = listTreatmentDepart.Where(x => x.CLINICAL_IN_TIME < filter.TIME_FROM && x.IS_PAUSE==null).Count();//listTreatmentDK.Where(x => x.IN_DEPARTMENT_ID == rdo.EXECUTE_DEPARTMENT_ID).Count();
                    rdo.COUNT_TT_0 = item.Sum(x => x.COUNT_TT_0);
                    rdo.COUNT_TT_1 = item.Sum(x => x.COUNT_TT_1);
                    rdo.COUNT_TT_2 = item.Sum(x => x.COUNT_TT_2);
                    rdo.COUNT_TT_3 = item.Sum(x => x.COUNT_TT_3);
                    rdo.COUNT_TT_DB = item.Sum(x => x.COUNT_TT_DB);
                    rdo.COUNT_VS = item.Sum(x => x.COUNT_VS);
                    rdo.COUNT_XN = item.Sum(x => x.COUNT_XN);
                    rdo.COUNT_XQ = item.Sum(x => x.COUNT_XQ);
                    rdo.COUNT_XQ_KTS = item.Sum(x => x.COUNT_XQ_KTS);
                    decimal bed =1;
                    if (rdo.BED_PLAN == 0)
                    {
                        bed = 1;
                    }
                    else
                    {
                        bed = rdo.BED_PLAN??0;
                    }
                    rdo.NUM_DAY = listTreatmentDepart.Sum(x => x.TREATMENT_DAY_COUNT ?? 0) / bed;
                    if (rdo.BED_TRUST > 0)
                    {
                        rdo.POWER_TRUST = Math.Truncate(100 * rdo.NUM_DAY / (this.DiffDateReport * rdo.BED_TRUST??1));
                    }
                    if (rdo.BED_PLAN > 0)
                    {
                        rdo.POWER_PLAN = Math.Truncate(100 * rdo.NUM_DAY / (this.DiffDateReport * rdo.BED_PLAN??1));
                    }
                    listRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
