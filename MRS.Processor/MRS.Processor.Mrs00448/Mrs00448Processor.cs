using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;

namespace MRS.Processor.Mrs00448
{
    class Mrs00448Processor : AbstractProcessor
    {
        Mrs00448Filter castFilter = null;
        List<Mrs00448RDO> listRdo = new List<Mrs00448RDO>();
        List<Mrs00448RDO> listRdoGroup = new List<Mrs00448RDO>();
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_TREATMENT> listTreatmentss = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();

        public List<long> listServiceAXAMPHY = null;
        public List<long> listServiceBEDALLO = null;
        public List<long> listServiceXQUANGCC = null;
        public List<long> listServiceXQUANG = null;
        public List<long> listServiceCT = null;
        public List<long> listServiceEEG = null;
        public List<long> listServiceECG = null;
        public List<long> listServiceABOR = null;
        public List<long> listServiceMISUALLO = null;
        public List<long> listServiceHIV = null;
        public List<long> listServiceAPHE = null;
        public List<long> listServiceHCG = null;
        public List<long> listServiceMERE = null;
        public List<long> listServiceINJE = null;
        public List<long> listServiceLDLK = null;

        string thisReportTypeCode = "";
        public Mrs00448Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00448Filter);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo);
                //objectTag.AddObjectData(store, "GroupName", listGroupName); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupName", "Report", "GROUP_NAME", "GROUP_NAME"); 

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00448Filter)this.reportFilter;
                var skip = 0;
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00448";
                var listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter);
                var listServiceIds = listServices.Select(s => s.SERVICE_ID).ToList();

                listServiceAXAMPHY = listServices.Where(w => w.CATEGORY_CODE == "448_KSK").Select(s => s.SERVICE_ID).ToList();
                listServiceBEDALLO = listServices.Where(w => w.CATEGORY_CODE == "448_PCG").Select(s => s.SERVICE_ID).ToList();
                listServiceXQUANGCC = listServices.Where(w => w.CATEGORY_CODE == "448_XQCC").Select(s => s.SERVICE_ID).ToList();
                listServiceXQUANG = listServices.Where(w => w.CATEGORY_CODE == "448_XQKCC").Select(s => s.SERVICE_ID).ToList();
                listServiceCT = listServices.Where(w => w.CATEGORY_CODE == "448_CT").Select(s => s.SERVICE_ID).ToList();
                listServiceEEG = listServices.Where(w => w.CATEGORY_CODE == "448_EEG").Select(s => s.SERVICE_ID).ToList();
                listServiceECG = listServices.Where(w => w.CATEGORY_CODE == "448_ECG").Select(s => s.SERVICE_ID).ToList();
                listServiceABOR = listServices.Where(w => w.CATEGORY_CODE == "448_ABOR").Select(s => s.SERVICE_ID).ToList();
                listServiceMISUALLO = listServices.Where(w => w.CATEGORY_CODE == "448_PCTT").Select(s => s.SERVICE_ID).ToList();
                listServiceHIV = listServices.Where(w => w.CATEGORY_CODE == "448_HIV").Select(s => s.SERVICE_ID).ToList();
                listServiceAPHE = listServices.Where(w => w.CATEGORY_CODE == "448_APHE").Select(s => s.SERVICE_ID).ToList();
                listServiceHCG = listServices.Where(w => w.CATEGORY_CODE == "448_HCG").Select(s => s.SERVICE_ID).ToList();
                listServiceMERE = listServices.Where(w => w.CATEGORY_CODE == "448_MERE").Select(s => s.SERVICE_ID).ToList();
                listServiceLDLK = listServices.Where(w => w.CATEGORY_CODE == "448_LDLK").Select(s => s.SERVICE_ID).ToList();
                listServiceINJE = listServices.Where(w => w.CATEGORY_CODE == "448_INJE").Select(s => s.SERVICE_ID).ToList();

                HisTransactionViewFilterQuery transactinonFilter = new HisTransactionViewFilterQuery();
                transactinonFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactinonFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactinonFilter.HAS_SALL_TYPE = false;
                listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactinonFilter);
                listBills = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                skip = 0;
                while (listBills.Count - skip > 0)
                {
                    var listDs = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServBillViewFilterQuery sereServBillFilter = new HisSereServBillViewFilterQuery();
                    sereServBillFilter.BILL_IDs = listDs.Select(s => s.ID).ToList();
                    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillFilter));
                }

                skip = 0;
                while (listSereServBills.Count - skip > 0)
                {
                    var listDs = listSereServBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                    sereServFilter.IDs = listDs.Select(s => s.SERE_SERV_ID).ToList();
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServFilter));
                }

                skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listDs = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterFilter.TREATMENT_IDs = listDs.Select(s => s.TDL_TREATMENT_ID ?? 0).ToList();
                    listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter));
                }

                skip = 0;
                while (listPatientTypeAlters.Count - skip > 0)
                {
                    var listDs = listPatientTypeAlters.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.IDs = listDs.Select(s => s.TREATMENT_ID).ToList();
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentFilter));
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
                CommonParam paramGet = new CommonParam();

                var listTreatment = listTreatments.Distinct();
                var listTreatmentHein = listTreatment.Where(w => w.TDL_HEIN_CARD_NUMBER != null && w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE).ToList();
                var listTreatmentHeinID = listTreatmentHein.Select(s => s.ID);
                listTreatmentss.AddRange(listTreatmentHein);
                var listTreatmentFee = listTreatment.Where(c => !listTreatmentHeinID.Contains(c.ID)).ToList();
                listTreatmentss.AddRange(listTreatmentFee);

                var treatments = listTreatmentss.Select(s => s.ID).ToList();
                var listPatientTypeAlterGroup = listPatientTypeAlters.Where(w => treatments.Contains(w.TREATMENT_ID)).GroupBy(s => s.TREATMENT_ID).ToList();
                foreach (var patientTypeAlterGroup in listPatientTypeAlterGroup)
                {
                    var patientTypeAlters = patientTypeAlterGroup.OrderByDescending(s => s.LOG_TIME).ToList().First();

                    //ngoai tru
                    if (patientTypeAlters.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM || patientTypeAlters.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        #region
                        decimal x = 0;
                        if (patientTypeAlters.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU && patientTypeAlters.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            x += 1;
                        }
                        var listBill = listBills.Where(s => s.TREATMENT_ID == patientTypeAlters.TREATMENT_ID);
                        foreach (var bills in listBill)
                        {
                            Mrs00448RDO rdo = new Mrs00448RDO();
                            rdo.CASHIER_USERNAME = bills.CASHIER_USERNAME;
                            rdo.CASHIER_USERID = bills.CASHIER_LOGINNAME;
                            var listsereSereBill = listSereServBills.Where(s => s.BILL_ID == bills.ID).Select(s => s.SERE_SERV_ID).ToList();
                            var listSereServ = listSereServs.Where(s => listsereSereBill.Contains(s.ID)).ToList();
                            rdo.TOTAL_EXEM = bills.EXEMPTION.Value;
                            decimal TotalHeinNgt5 = 0;
                            decimal TotalHeinNgt20 = 0;
                            decimal TotalExamNgt = 0;
                            decimal TotalExamPhyNgt = 0;
                            decimal TotalBedNgt = 0;
                            decimal TotalBedAlloNgt = 0;
                            decimal TotalTestNgt = 0;
                            decimal TotalMereNgt = 0;
                            decimal TotalXquangccNgt = 0;
                            decimal TotalXquangNgt = 0;
                            decimal TotalCtNgt = 0;
                            decimal TotalSuimNgt = 0;
                            decimal TotalEegNgt = 0;
                            decimal TotalEcgNgt = 0;
                            decimal TotalEndoNgt = 0;
                            decimal TotalAborNgt = 0;
                            decimal TotalMisuNgt = 0;
                            decimal TotalMisuAlloNgt = 0;
                            decimal TotalInjeNgt = 0;
                            decimal TotalMateNgt = 0;
                            decimal TotalMediNgt = 0;
                            decimal TotalHcgNgt = 0;
                            decimal TotalHivNgt = 0;
                            decimal TotalApheNgt = 0;
                            decimal TotalLdlkNgt = 0;
                            foreach (var sereServ in listSereServ)
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE && sereServ.IS_EXPEND == null)
                                {
                                    // tong kham
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && !listServiceAXAMPHY.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalExamNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //kham suc khoe
                                    if (listServiceAXAMPHY.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalExamPhyNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    // giuong
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                                    {
                                        TotalBedNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //phu cap giuong
                                    if (listServiceBEDALLO.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalBedAlloNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    // xetnghem
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                                    {
                                        TotalTestNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //xquang cc
                                    if (listServiceXQUANGCC.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalXquangccNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //xquang 
                                    if (listServiceXQUANG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalXquangNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //Ct
                                    if (listServiceCT.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalCtNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //sieu am
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                                    {
                                        TotalSuimNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //dien nao
                                    if (listServiceEEG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalEegNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //dien tim
                                    if (listServiceECG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalEcgNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }


                                    //noi soi
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                                    {
                                        TotalEndoNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //hut thai
                                    if (listServiceABOR.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalAborNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //thu thuat
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                    {
                                        TotalMisuNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //phu cap thu thuat
                                    if (listServiceMISUALLO.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalMisuAlloNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //tiem
                                    if (listServiceINJE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalInjeNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //vat tu
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                                    {
                                        TotalMateNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //thuoc
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                                    {
                                        TotalMediNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //hcg
                                    if (listServiceHCG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalHcgNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //hiv
                                    if (listServiceHIV.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalHivNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //aphetamin
                                    if (listServiceAPHE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalApheNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //sao benh an
                                    if (listServiceMERE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalMereNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //ldlk
                                    if (listServiceLDLK.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalLdlkNgt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                }

                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && sereServ.IS_EXPEND == null)
                                {
                                    if (sereServ.HEIN_RATIO == Convert.ToDecimal(0.95))
                                    {
                                        TotalHeinNgt5 += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                    if (sereServ.HEIN_RATIO == Convert.ToDecimal(0.8))
                                    {
                                        TotalHeinNgt20 += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                    if (listServiceLDLK.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalLdlkNgt += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                }
                            }

                            rdo.TOTAL_TREAT_OUT = TotalExamNgt + TotalExamPhyNgt + TotalBedNgt + TotalBedAlloNgt + TotalTestNgt + TotalMereNgt + TotalXquangccNgt + TotalXquangNgt + TotalCtNgt + TotalSuimNgt + TotalEegNgt + TotalEcgNgt + TotalEndoNgt + TotalAborNgt + TotalMisuNgt + TotalMisuAlloNgt + TotalInjeNgt + TotalMateNgt + TotalMediNgt + TotalHcgNgt + TotalHivNgt + TotalApheNgt + TotalHeinNgt5 + TotalHeinNgt20; // ngoai tru
                            //rdo.TOTAL_TREAT_IN_OUT   // BN noitru ngoaitru vp
                            rdo.TOTAL_TREAT_IN_OUT = x;    // ngay dieu tri noi tru vp
                            rdo.TOTAL_EXAM = TotalExamNgt;
                            rdo.TOTAL_EXAM_PHY = TotalExamPhyNgt;   //kham suc khoe
                            rdo.TOTAL_BED = TotalBedNgt;   //giuong
                            rdo.TOTAL_BED_ALLO = TotalBedAlloNgt;   //phu cap giuong
                            rdo.TOTAL_TEST = TotalTestNgt;   //xet nghiem
                            rdo.TOTAL_XQUANG = TotalXquangNgt; //xquang
                            rdo.TOTAL_XQUANG_CC = TotalXquangccNgt;   //xquang can chop
                            rdo.TOTAL_CT = TotalCtNgt;    //ct
                            rdo.TOTAL_SUIM = TotalSuimNgt;   //siêu am
                            rdo.TOTAL_EEG = TotalEegNgt;   //dien nao
                            rdo.TOTAL_ECG = TotalEcgNgt;  //dien tim
                            rdo.TOTAL_ENDO = TotalEndoNgt;   //noi soi
                            rdo.TOTAL_ABOR = TotalAborNgt;   //Hut thai
                            rdo.TOTAL_MISU = TotalMisuNgt;  //thu thuat
                            rdo.TOTAL_MISU_ALLO = TotalMisuAlloNgt;  // pcap thu thuat
                            rdo.TOTAL_INJE = TotalInjeNgt;  //tiem
                            rdo.TOTAL_MIDI = TotalMediNgt;  //thuoc
                            rdo.TOTAL_MATE = TotalMateNgt;   //Vat tu
                            rdo.TOTAL_HCG = TotalHcgNgt;   //hcg
                            rdo.TOTAL_HIV = TotalHivNgt;  //hiv 
                            rdo.TOTAL_APHE = TotalApheNgt;   //Aphetamin
                            rdo.TOTAL_MERE = TotalMereNgt;  //sao benh an
                            rdo.TOTAL_HEIN_RATIO_5 = TotalHeinNgt5;   // bao hiem 5
                            rdo.TOTAL_HEIN_RATIO_20 = TotalHeinNgt20;   // bao hiem 20
                            //rdo.TOTAL_TOTAL_PRICE_FEE   // tong tien
                            //rdo.TOTAL_EXEM   // miem giam
                            //rdo.TOTAL_TOTAL_PRICE  
                            rdo.TOTAL_LDLK = TotalLdlkNgt;

                            listRdo.Add(rdo);
                        }
                        #endregion
                    }

                    //noi tru
                    if (patientTypeAlters.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        #region
                        decimal x = 0;
                        if (patientTypeAlters.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && patientTypeAlters.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            x += 1;
                        }
                        var listBill = listBills.Where(s => s.TREATMENT_ID == patientTypeAlters.TREATMENT_ID);
                        foreach (var bills in listBill)
                        {
                            Mrs00448RDO rdo = new Mrs00448RDO();
                            rdo.CASHIER_USERNAME = bills.CASHIER_USERNAME;
                            rdo.CASHIER_USERID = bills.CASHIER_LOGINNAME;
                            rdo.TOTAL_EXEM = bills.EXEMPTION.Value;
                            var listsereSereBill = listSereServBills.Where(s => s.BILL_ID == bills.ID).Select(s => s.SERE_SERV_ID).ToList();
                            var listSereServ = listSereServs.Where(s => listsereSereBill.Contains(s.ID)).ToList();
                            decimal TotalHeinNt5 = 0;
                            decimal TotalHeinNt20 = 0;
                            decimal TotalExamNt = 0;
                            decimal TotalExamPhyNt = 0;
                            decimal TotalBedNt = 0;
                            decimal TotalBedAlloNt = 0;
                            decimal TotalTestNt = 0;
                            decimal TotalMereNt = 0;
                            decimal TotalXquangccNt = 0;
                            decimal TotalXquangNt = 0;
                            decimal TotalCtNt = 0;
                            decimal TotalSuimNt = 0;
                            decimal TotalEegNt = 0;
                            decimal TotalEcgNt = 0;
                            decimal TotalEndoNt = 0;
                            decimal TotalAborNt = 0;
                            decimal TotalMisuNt = 0;
                            decimal TotalMisuAlloNt = 0;
                            decimal TotalInjeNt = 0;
                            decimal TotalMateNt = 0;
                            decimal TotalMediNt = 0;
                            decimal TotalHcgNt = 0;
                            decimal TotalHivNt = 0;
                            decimal TotalApheNt = 0;
                            decimal TotalLdlkNt = 0;
                            long ndt = 0;
                            foreach (var sereServ in listSereServ)
                            {
                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE && sereServ.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                {
                                    // tong kham
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && !listServiceAXAMPHY.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalExamNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //kham suc khoe
                                    if (listServiceAXAMPHY.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalExamPhyNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    // giuong
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                                    {
                                        TotalBedNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //phu cap giuong
                                    if (listServiceBEDALLO.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalBedAlloNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    // xetnghem
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN)
                                    {
                                        TotalTestNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //xquang cc
                                    if (listServiceXQUANGCC.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalXquangccNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //xquang 
                                    if (listServiceXQUANG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalXquangNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //Ct
                                    if (listServiceCT.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalCtNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //sieu am
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA)
                                    {
                                        TotalSuimNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //dien nao
                                    if (listServiceEEG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalEegNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //dien tim
                                    if (listServiceECG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalEcgNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }


                                    //noi soi
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS)
                                    {
                                        TotalEndoNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //hut thai
                                    if (listServiceABOR.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalAborNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //thu thuat
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                                    {
                                        TotalMisuNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //phu cap thu thuat
                                    if (listServiceMISUALLO.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalMisuAlloNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //tiem
                                    if (listServiceINJE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalInjeNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                    //vat tu
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                                    {
                                        TotalMateNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //thuoc
                                    if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                                    {
                                        TotalMediNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //hcg
                                    if (listServiceHCG.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalHcgNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //hiv
                                    if (listServiceHIV.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalHivNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //aphetamin
                                    if (listServiceAPHE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalApheNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    //sao benh an
                                    if (listServiceMERE.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalMereNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }

                                    if (listServiceLDLK.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalLdlkNt += sereServ.VIR_TOTAL_PATIENT_PRICE.Value;
                                    }
                                }

                                if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && sereServ.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                {
                                    if (sereServ.HEIN_RATIO == Convert.ToDecimal(0.95))
                                    {
                                        TotalHeinNt5 += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                    if (sereServ.HEIN_RATIO == Convert.ToDecimal(0.8))
                                    {
                                        TotalHeinNt20 += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                    if (listServiceLDLK.Contains(sereServ.SERVICE_ID))
                                    {
                                        TotalLdlkNt += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT.Value;
                                    }
                                }

                                var treatment = listTreatments.Where(s => s.ID == sereServ.TDL_TREATMENT_ID).Distinct();
                                foreach (var NDT in treatment)
                                {
                                    if (NDT.TDL_HEIN_CARD_NUMBER == null)
                                    {
                                        ndt += HIS.Treatment.DateTime.Calculation.DayOfTreatment(NDT.IN_TIME, NDT.OUT_TIME ?? castFilter.TIME_TO);
                                    }
                                }
                            }
                            rdo.TOTAL_TREAT_IN = TotalExamNt + TotalExamPhyNt + TotalBedNt + TotalBedAlloNt + TotalTestNt + TotalMereNt + TotalXquangccNt + TotalXquangNt + TotalCtNt + TotalSuimNt + TotalEegNt + TotalEcgNt + TotalEndoNt + TotalAborNt + TotalMisuNt + TotalMisuAlloNt + TotalInjeNt + TotalMateNt + TotalMediNt + TotalHcgNt + TotalHivNt + TotalApheNt + TotalHeinNt5 + TotalHeinNt20;
                            rdo.TOTAL_TREAT_IN_OUT = x;   // BN noitru ngoaitru vp
                            rdo.TOTAL_NDTRI = ndt;  // ngay dieu tri noi tru vp
                            rdo.TOTAL_EXAM = TotalExamNt;
                            rdo.TOTAL_EXAM_PHY = TotalExamPhyNt;   //kham suc khoe
                            rdo.TOTAL_BED = TotalBedNt;   //giuong
                            rdo.TOTAL_BED_ALLO = TotalBedAlloNt;   //phu cap giuong
                            rdo.TOTAL_TEST = TotalTestNt;   //xet nghiem
                            rdo.TOTAL_XQUANG = TotalXquangNt; //xquang
                            rdo.TOTAL_XQUANG_CC = TotalXquangccNt;   //xquang can chop
                            rdo.TOTAL_CT = TotalCtNt;    //ct
                            rdo.TOTAL_SUIM = TotalSuimNt;   //siêu am
                            rdo.TOTAL_EEG = TotalEegNt;   //dien nao
                            rdo.TOTAL_ECG = TotalEcgNt;  //dien tim
                            rdo.TOTAL_ENDO = TotalEndoNt;   //noi soi
                            rdo.TOTAL_ABOR = TotalAborNt;   //Hut thai
                            rdo.TOTAL_MISU = TotalMisuNt;  //thu thuat
                            rdo.TOTAL_MISU_ALLO = TotalMisuAlloNt;  // pcap thu thuat
                            rdo.TOTAL_INJE = TotalInjeNt;  //tiem
                            rdo.TOTAL_MIDI = TotalMediNt;  //thuoc
                            rdo.TOTAL_MATE = TotalMateNt;   //Vat tu
                            rdo.TOTAL_HCG = TotalHcgNt;   //hcg
                            rdo.TOTAL_HIV = TotalHivNt;  //hiv 
                            rdo.TOTAL_APHE = TotalApheNt;   //Aphetamin
                            rdo.TOTAL_MERE = TotalMereNt;  //sao benh an
                            rdo.TOTAL_HEIN_RATIO_5 = TotalHeinNt5;   // bao hiem 5
                            rdo.TOTAL_HEIN_RATIO_20 = TotalHeinNt20;   // bao hiem 20
                            //rdo.TOTAL_TOTAL_PRICE_FEE   // tong tien
                            //rdo.TOTAL_EXEM   // miem giam
                            rdo.TOTAL_LDLK = TotalLdlkNt;
                            listRdo.Add(rdo);
                        }
                        #endregion
                    }
                }

                listRdo = listRdo.GroupBy(s => new { s.CASHIER_USERID, s.CASHIER_USERNAME }).Select(s => new Mrs00448RDO
                {
                    CASHIER_USERID = s.First().CASHIER_USERID,
                    CASHIER_USERNAME = s.First().CASHIER_USERNAME,
                    TOTAL_ABOR = s.Sum(o => o.TOTAL_ABOR),
                    TOTAL_APHE = s.Sum(o => o.TOTAL_APHE),
                    TOTAL_BED = s.Sum(o => o.TOTAL_BED),
                    TOTAL_BED_ALLO = s.Sum(o => o.TOTAL_BED_ALLO),
                    TOTAL_CT = s.Sum(o => o.TOTAL_CT),
                    TOTAL_ECG = s.Sum(o => o.TOTAL_ECG),
                    TOTAL_ENDO = s.Sum(o => o.TOTAL_ENDO),
                    TOTAL_EXAM = s.Sum(o => o.TOTAL_EXAM),
                    TOTAL_EXAM_PHY = s.Sum(o => o.TOTAL_EXAM_PHY),
                    TOTAL_EXEM = s.Sum(o => o.TOTAL_EXEM),
                    TOTAL_HCG = s.Sum(o => o.TOTAL_HCG),
                    TOTAL_HEIN_RATIO_20 = s.Sum(o => o.TOTAL_HEIN_RATIO_20),
                    TOTAL_EEG = s.Sum(o => o.TOTAL_EEG),
                    TOTAL_HEIN_RATIO_5 = s.Sum(o => o.TOTAL_HEIN_RATIO_5),
                    TOTAL_HIV = s.Sum(o => o.TOTAL_HIV),
                    TOTAL_INJE = s.Sum(o => o.TOTAL_INJE),
                    TOTAL_MATE = s.Sum(o => o.TOTAL_MATE),
                    TOTAL_MERE = s.Sum(o => o.TOTAL_MERE),
                    TOTAL_MIDI = s.Sum(o => o.TOTAL_MIDI),
                    TOTAL_MISU = s.Sum(o => o.TOTAL_MISU),
                    TOTAL_MISU_ALLO = s.Sum(o => o.TOTAL_MISU_ALLO),
                    TOTAL_SUIM = s.Sum(o => o.TOTAL_SUIM),
                    TOTAL_TEST = s.Sum(o => o.TOTAL_TEST),
                    TOTAL_TREAT_IN = s.Sum(o => o.TOTAL_TREAT_IN),
                    TOTAL_TREAT_OUT = s.Sum(o => o.TOTAL_TREAT_OUT),
                    TOTAL_TREAT_IN_OUT = s.Sum(o => o.TOTAL_TREAT_IN_OUT),
                    TOTAL_XQUANG = s.Sum(o => o.TOTAL_XQUANG),
                    TOTAL_XQUANG_CC = s.Sum(o => o.TOTAL_XQUANG_CC),
                    TOTAL_LDLK = s.Sum(o => o.TOTAL_LDLK),
                }).ToList();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


    }
}
