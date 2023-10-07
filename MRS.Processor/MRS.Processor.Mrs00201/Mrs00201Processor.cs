using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServTein;
using MOS.MANAGER.HisServiceRetyCat;
using FlexCel.Report;
using MOS.MANAGER.HisKskContract;
using MOS.MANAGER.HisWorkPlace;

namespace MRS.Processor.Mrs00201
{
    internal class Mrs00201Processor : AbstractProcessor
    {
        List<Mrs00201RDO> _listSarReportMrs00201Rdos = new List<Mrs00201RDO>();
        public List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCatViews = new List<V_HIS_SERVICE_RETY_CAT>();
        List<SereServRdo> listSereServViews = new List<SereServRdo>();
        List<V_HIS_SERE_SERV_TEIN> listSereServTeinViews = new List<V_HIS_SERE_SERV_TEIN>();
        List<V_HIS_TEST_INDEX> listTestIndexViews = new List<V_HIS_TEST_INDEX>();
        Mrs00201Filter CastFilter;
        List<TreatmentServiceInfo> listTreatmentServiceInfo = new List<TreatmentServiceInfo>();
        List<HIS_KSK_CONTRACT> listKskContract = new List<HIS_KSK_CONTRACT>();
        List<HIS_WORK_PLACE> listWorkPlace = new List<HIS_WORK_PLACE>();
        string ReportTypeCode = "";

        public Mrs00201Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            this.ReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00201Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00201Filter)this.reportFilter;

                //-------------------------------------------------------------------------------------------------- HIS_KSK_CONTRACT
                var kskContractFilter = new HisKskContractFilterQuery
                {
                    ID = CastFilter.KSK_CONTRACT_ID
                };
                listKskContract = new HisKskContractManager(paramGet).Get(kskContractFilter);
                HisWorkPlaceFilterQuery workPlaceFilter = new HisWorkPlaceFilterQuery();
                listWorkPlace = new HisWorkPlaceManager().Get(workPlaceFilter);
                
              
                //--------------------------------------------------------------------------------------------------V_HIS_SERE_SERV_1
                listSereServViews = new ManagerSql().GetSereServ(CastFilter);
                //
                
                //-------------------------------------------------------------------------------------------------- V_HIS_SERE_SERV_TEIN
                var listSereServIds = listSereServViews.Select(s => s.ID).ToList();
                var skip = 0;
                while (listSereServIds.Count - skip > 0)
                {
                    var listIds = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var sereServTeinFilter = new HisSereServTeinViewFilterQuery
                    {
                        SERE_SERV_IDs = listIds
                    };
                    var sereServTeinViews = new HisSereServTeinManager(paramGet).GetView(sereServTeinFilter);
                    listSereServTeinViews.AddRange(sereServTeinViews);
                }

                var serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery
                {
                    REPORT_TYPE_CODE__EXACT = ReportTypeCode
                };
                listServiceRetyCatViews = new HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter);
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
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                var listServiceIdsXquang = listServiceRetyCatViews.Select(s => s.SERVICE_ID).ToList();
                var rowpos = 0;
                foreach (var listSereServView in listSereServViews.GroupBy(s => s.TDL_TREATMENT_ID).ToList())
                {
                    var listSereServIds = listSereServView.Select(s => s.ID).ToList();
                    var dateOfBirthMale = string.Empty;
                    var dateOfBirthFemale = string.Empty;
                    if (listSereServView.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        dateOfBirthMale = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSereServView.First().TDL_PATIENT_DOB??0);
                    if (listSereServView.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        dateOfBirthFemale = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSereServView.First().TDL_PATIENT_DOB??0);

                    var xQuang = listSereServView.Where(s => listServiceIdsXquang.Contains(s.SERVICE_ID)).ToList(); //Xquang
                    var cdhaKhac = listSereServView.Where(s => !listServiceIdsXquang.Contains(s.SERVICE_ID) && s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).ToList(); //cdha khác
                    var thamDoChucNang = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).ToList(); //thăm dò chức năng
                    var superSonic = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList(); //siêu âm

                    var nSoi = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).ToList(); //Nội soi
                    var thuthuat = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList(); //Thủ thuật
                    //var test = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList(); //xét nghiệm
                    var examination = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList(); //khám
                    var listSereServTeins = listSereServTeinViews.Where(s => listSereServIds.Contains(s.SERE_SERV_ID)).ToList(); //kết quả sau xét nghiệm
                    var giaiPhauBenh = listSereServView.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL).ToList(); //giải phẫu bệnh lý
                    var listXetNghiem = new List<XetNghiem>();
                    foreach (var listSereServTein in listSereServTeins.GroupBy(s => s.SERE_SERV_ID))
                    {
                        var sereServViews = listSereServView.First(s => s.ID == listSereServTein.Key);
                        if (CastFilter.IS_NOT_INDEX == true)
                        {
                            var xn = new XetNghiem
                            {
                                TEN_DV = sereServViews.TDL_SERVICE_NAME,
                                GIA_DV = sereServViews.VIR_PRICE,
                                //TEN_CHI_SO = sereServTein.TEST_INDEX_NAME,
                                //CHI_SO = sereServTein.VALUE,
                                //DON_VI_CHI_SO = sereServTein.TEST_INDEX_UNIT_CODE,
                            };
                            listXetNghiem.Add(xn);
                            continue;
                        }
                        foreach (var sereServTein in listSereServTein)
                        {
                            var xn = new XetNghiem
                            {
                                TEN_DV = sereServViews.TDL_SERVICE_NAME,
                                GIA_DV = sereServViews.VIR_PRICE,
                                TEN_CHI_SO = sereServTein.TEST_INDEX_NAME,
                                CHI_SO = sereServTein.VALUE,
                                DON_VI_CHI_SO = sereServTein.TEST_INDEX_UNIT_CODE,
                            };
                            listXetNghiem.Add(xn);
                        }
                    }

                    var listNumber = new List<int>
                    {
                        xQuang.Count,
                        cdhaKhac.Count,
                        thamDoChucNang.Count,
                        superSonic.Count,
                        nSoi.Count,
                        thuthuat.Count,
                        examination.Count,
                        listXetNghiem.Count,
                        giaiPhauBenh.Count
                    };
                    var number = listNumber.Max();
                    if (number>0) rowpos++;
                    for (var i = 0; i < number; i++)
                    {
                        var rdo = new Mrs00201RDO
                        {
                            ROW_POS = rowpos,
                            PATIENT_CODE = listSereServView.First().TDL_PATIENT_CODE,//mã BN
                            TREATMENT_CODE = listSereServView.First().TDL_TREATMENT_CODE,//mã điều trị
                            PATIENT_NAME = listSereServView.First().TDL_PATIENT_NAME,//tên BN
                            DATE_OF_BIRTH_MALE = dateOfBirthMale,//Ngày sinh nếu là nam
                            DATE_OF_BIRTH_FEMALE = dateOfBirthFemale,//Ngày sinh nếu là nữ
                            X_QUANG = xQuang.Count >= i + 1 ? xQuang[i].TDL_SERVICE_NAME : null,//Xquang
                            GIA_X_QUANG = xQuang.Count >= i + 1 ? xQuang[i].VIR_PRICE : null,//giá Xquang
                            CDHA_KHAC = cdhaKhac.Count >= i + 1 ? cdhaKhac[i].TDL_SERVICE_NAME : null,//cdha khác
                            GIA_CDHA_KHAC = cdhaKhac.Count >= i + 1 ? cdhaKhac[i].VIR_PRICE : null,//cdha khác
                            TDCN = thamDoChucNang.Count >= i + 1 ? thamDoChucNang[i].TDL_SERVICE_NAME : null,//Thăm dò chức năng
                            GIA_TDCN = thamDoChucNang.Count >= i + 1 ? thamDoChucNang[i].VIR_PRICE : null,//giá Thăm dò chức năng
                            SIEU_AM = superSonic.Count >= i + 1 ? superSonic[i].TDL_SERVICE_NAME : null,//siêu âm
                            GIA_SIEU_AM = superSonic.Count >= i + 1 ? superSonic[i].VIR_PRICE : null,//giá siêu âm
                            DV_XET_NGHEM = listXetNghiem.Count >= i + 1 ? listXetNghiem[i].TEN_DV : null,//tên chỉ số xét nghiệm
                            GIA_DV_XET_NGHEM = listXetNghiem.Count >= i + 1 ? listXetNghiem[i].GIA_DV : null,//giá tên chỉ số xét nghiệm
                            TEN_CHI_SO_XET_NGHIEM = listXetNghiem.Count >= i + 1 ? listXetNghiem[i].TEN_CHI_SO : null,//tên chỉ số
                            CHI_SO_XET_NGHIEM = listXetNghiem.Count >= i + 1 ? string.Format("{0}/{1}", listXetNghiem[i].CHI_SO, listXetNghiem[i].TEN_CHI_SO) : null,
                            EXAMINATION = examination.Count >= i + 1 ? examination[i].TDL_SERVICE_NAME : null,//khám
                            PRICE_EXAMINATION = examination.Count >= i + 1 ? examination[i].VIR_PRICE : null,//giá khám
                            GIAIPHAU = giaiPhauBenh.Count >= i + 1 ? giaiPhauBenh[i].TDL_SERVICE_NAME : null,//giải phẫu bệnh lý
                            PRICE_GIAIPHAU = giaiPhauBenh.Count >= i + 1 ? giaiPhauBenh[i].VIR_PRICE : null,//giá giải phẫu bệnh lý
                            NOI_SOI = nSoi.Count >= i + 1 ? nSoi[i].TDL_SERVICE_NAME : null, //Nội soi
                            GIA_NOI_SOI = nSoi.Count >= i + 1 ? nSoi[i].VIR_PRICE : null, //giá nội soi
                            THU_THUAT = thuthuat.Count >= i + 1 ? thuthuat[i].TDL_SERVICE_NAME : null, //thủ thuật
                            GIA_THU_THUAT = thuthuat.Count >= i + 1 ? thuthuat[i].VIR_PRICE : null, //giá thủ thuật

                        };
                        _listSarReportMrs00201Rdos.Add(rdo);
                    }
                    var listSereServSub = listSereServView.ToList();
                    foreach (var item in listSereServSub)
                    {
                            TreatmentServiceInfo rdo = new TreatmentServiceInfo();
                            rdo.TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                            rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                            rdo.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB??0;
                            rdo.AMOUNT = item.AMOUNT;
                            rdo.TDL_SERVICE_CODE = item.TDL_SERVICE_CODE;
                            rdo.TDL_SERVICE_NAME = item.TDL_SERVICE_NAME;
                            rdo.VIR_PRICE = item.VIR_PRICE ?? 0;
                            rdo.VIR_TOTAL_PRICE = item.VIR_TOTAL_PRICE ?? 0;
                            listTreatmentServiceInfo.Add(rdo);
                    }
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
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
            if (CastFilter.KSK_CONTRACT_ID != null)
            {
                var kskContract = listKskContract.FirstOrDefault();
                if (kskContract != null && listWorkPlace != null)
                {
                    var workPlace = listWorkPlace.FirstOrDefault(o => o.ID == kskContract.WORK_PLACE_ID) ?? new HIS_WORK_PLACE();
                    dicSingleTag.Add("CONTRACT_WORK_PLACE_NAME", workPlace.WORK_PLACE_NAME);
                }
                else
                {
                    dicSingleTag.Add("CONTRACT_WORK_PLACE_NAME", "");
                }
            }
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00201Rdos);
            objectTag.AddObjectData(store, "TreatmentService", listTreatmentServiceInfo);
            objectTag.AddObjectData(store, "Service", listTreatmentServiceInfo.GroupBy(o => string.Format("{0}_{1}",o.TDL_SERVICE_CODE,o.VIR_PRICE)).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Treatment", listTreatmentServiceInfo.GroupBy(o => o.TREATMENT_CODE).Select(p => new TreatmentServiceInfo(p.ToList())).ToList());
            objectTag.SetUserFunction(store, "FuncSameTitleRowPatientCode", new CustomerFuncMergeSameDataPatientCode(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowPatientName", new CustomerFuncMergeSameDataPatientName(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowDateOfBirthMale", new CustomerFuncMergeSameDataDateOfBirthMale(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowDateOfBirthFemale", new CustomerFuncMergeSameDataDateOfBirthFemale(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowServiceNameTest", new CustomerFuncMergeSameDataServiceNameTest(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowServicePriceTest", new CustomerFuncMergeSameDataServicePriceTest(_listSarReportMrs00201Rdos));
            objectTag.SetUserFunction(store, "FuncSameTitleRowTreatmentCode", new CustomerFuncMergeSameDataTreatmentCode(_listSarReportMrs00201Rdos));
        }

        class CustomerFuncMergeSameDataTreatmentCode : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataTreatmentCode(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].TREATMENT_CODE;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].TREATMENT_CODE;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataPatientCode : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataPatientCode(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].PATIENT_CODE;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].PATIENT_CODE;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataPatientName : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataPatientName(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].PATIENT_NAME;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].PATIENT_NAME;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataDateOfBirthMale : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataDateOfBirthMale(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].DATE_OF_BIRTH_MALE;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].DATE_OF_BIRTH_MALE;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataDateOfBirthFemale : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataDateOfBirthFemale(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].DATE_OF_BIRTH_FEMALE;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].DATE_OF_BIRTH_FEMALE;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataServiceNameTest : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataServiceNameTest(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].DV_XET_NGHEM;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].DV_XET_NGHEM;

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        class CustomerFuncMergeSameDataServicePriceTest : TFlexCelUserFunction
        {
            List<Mrs00201RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameDataServicePriceTest(List<Mrs00201RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    string currentValue = "";
                    string nextValue = "";

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].GIA_DV_XET_NGHEM.ToString();
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].GIA_DV_XET_NGHEM.ToString();

                        if (!String.IsNullOrEmpty((currentValue))
                        && !String.IsNullOrEmpty((nextValue))
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        private class XetNghiem
        {
            public string TEN_DV { get; set; }
            public decimal? GIA_DV { get; set; }
            public string TEN_CHI_SO { get; set; }
            public string CHI_SO { get; set; }
            public string DON_VI_CHI_SO { get; set; }
        }
    }
}
