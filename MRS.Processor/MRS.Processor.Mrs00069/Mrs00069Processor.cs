using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMediStock;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00069
{
    public class Mrs00069Processor : AbstractProcessor
    {
        Mrs00069Filter castFilter = null;
        List<Mrs00069RDO> ListRdoMedicine = new List<Mrs00069RDO>();
        List<Mrs00069RDO> ListRdoMaterial = new List<Mrs00069RDO>();
        List<V_HIS_EXP_MEST> listPresBhytIn = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> listPresBhytOut = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> ListPrescription;
        string MEDI_STOCK_NAME;

        public Mrs00069Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00069Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00069Filter)this.reportFilter);

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_PRESCRIPTION. MRS00069 filter. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisExpMestViewFilterQuery presFilter = new HisExpMestViewFilterQuery();
                presFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                presFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                presFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                ListPrescription = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(presFilter);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_PRESCRIPTION MRS00069. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
                    throw new DataMisalignedException();
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
                if (IsNotNullOrEmpty(ListPrescription))
                {
                    ListPrescription = ListPrescription.Where(o => o.TDL_TREATMENT_ID.HasValue).ToList();
                    ProcessListPrescription(ListPrescription);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListPrescription(List<V_HIS_EXP_MEST> ListPrescription)
        {
            try
            {
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(ListPrescription))
                {
                    CheckTreatmentIsBhytInOrOut(param, ListPrescription);
                    if (!paramGet.HasException)
                    {
                        ProcessListPrescriptionBhytIn(paramGet);
                        ProcessListPrescriptionBhytOut(paramGet);
                        if (!paramGet.HasException)
                        {
                            ListRdoMedicine = ListRdoMedicine.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.VIR_PRICE }).Select(s => new Mrs00069RDO { MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, VIR_PRICE = s.First().VIR_PRICE, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, OUT_AMOUNT = s.Sum(s1 => s1.OUT_AMOUNT), IN_AMOUNT = s.Sum(s2 => s2.IN_AMOUNT) }).ToList();

                            ListRdoMaterial = ListRdoMaterial.GroupBy(g => new { g.MATERIAL_TYPE_ID, g.VIR_PRICE }).Select(s => new Mrs00069RDO { MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, VIR_PRICE = s.First().VIR_PRICE, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, IN_AMOUNT = s.Sum(s1 => s1.IN_AMOUNT), OUT_AMOUNT = s.Sum(s2 => s2.OUT_AMOUNT) }).ToList();
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00069.");
                        }
                    }
                    else
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00069.");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoMedicine.Clear();
                ListRdoMaterial.Clear();
            }
        }

        //Kiểm tra xem treatment là nội trú hay ngoại trú
        private void CheckTreatmentIsBhytInOrOut(CommonParam paramGet, List<V_HIS_EXP_MEST> ListPrescription)
        {
            try
            {
                List<long> listTreatmentId = ListPrescription.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var Ids = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                    appFilter.TREATMENT_IDs = Ids;
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter);
                    if (IsNotNullOrEmpty(listPatientTypeAlter))
                    {
                        foreach (var treatmentId in Ids)
                        {
                            var currentPatientTypeAlter = listPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last(o => o.TREATMENT_ID == treatmentId);
                            if ((this.castFilter.PATIENT_TYPE_ID ?? 0) != 0)
                            {
                                if (currentPatientTypeAlter.PATIENT_TYPE_ID != this.castFilter.PATIENT_TYPE_ID)
                                {
                                    continue;
                                }
                            }
                            if (currentPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                listPresBhytOut.AddRange(ListPrescription.Where(o => o.TDL_TREATMENT_ID == treatmentId).ToList());
                            }
                            else if (currentPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                listPresBhytIn.AddRange(ListPrescription.Where(o => o.TDL_TREATMENT_ID == treatmentId).ToList());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listPresBhytIn.Clear();
                listPresBhytOut.Clear();
            }
        }

        //Xử lý listPrescription Nội trú
        private void ProcessListPrescriptionBhytIn(CommonParam paramGet)
        {
            try
            {
                if (IsNotNullOrEmpty(listPresBhytIn))
                {
                    int start = 0;
                    int count = listPresBhytIn.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<long> Service_Req_Ids = listPresBhytIn.Skip(start).Take(limit).Select(s => s.SERVICE_REQ_ID ?? 0).ToList();
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.SERVICE_REQ_IDs = Service_Req_Ids;
                        List<V_HIS_SERE_SERV> hisSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                        if (IsNotNullOrEmpty(hisSereServ))
                        {
                            ProcessListSereServBhytIn(hisSereServ);
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListSereServBhytIn(List<V_HIS_SERE_SERV> hisSereServ)
        {
            try
            {
                var listMedicine = hisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.AMOUNT > 0).ToList();
                if (IsNotNullOrEmpty(listMedicine))
                {
                    var GroupMedi = listMedicine.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).ToList();
                    foreach (var group in GroupMedi)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        ListRdoMedicine.Add(new Mrs00069RDO(listSub, true, true));
                    }
                }

                var listMaterial = hisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT && o.AMOUNT > 0).ToList();
                if (IsNotNullOrEmpty(listMaterial))
                {
                    var GroupMate = listMaterial.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).ToList();
                    foreach (var group in GroupMate)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        ListRdoMaterial.Add(new Mrs00069RDO(listSub, false, true));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Xử lý listPrescription Ngoại trú
        private void ProcessListPrescriptionBhytOut(CommonParam paramGet)
        {
            if (IsNotNullOrEmpty(listPresBhytOut))
            {
                int start = 0;
                int count = listPresBhytOut.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    List<long> Service_Req_Ids = listPresBhytOut.Skip(start).Take(limit).Select(s => s.SERVICE_REQ_ID ?? 0).ToList();
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                    ssFilter.SERVICE_REQ_IDs = Service_Req_Ids;
                    List<V_HIS_SERE_SERV> hisSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                    if (IsNotNullOrEmpty(hisSereServ))
                    {
                        ProcessListSereServBhytOut(hisSereServ);
                    }

                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
        }

        private void ProcessListSereServBhytOut(List<V_HIS_SERE_SERV> hisSereServ)
        {
            try
            {
                var listMedicine = hisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.AMOUNT > 0).ToList();
                if (IsNotNullOrEmpty(listMedicine))
                {
                    var GroupMedi = listMedicine.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).ToList();
                    foreach (var group in GroupMedi)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        ListRdoMedicine.Add(new Mrs00069RDO(listSub, true, false));
                    }
                }

                var listMaterial = hisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT && o.AMOUNT > 0).ToList();
                if (IsNotNullOrEmpty(listMaterial))
                {
                    var GroupMate = listMaterial.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).ToList();
                    foreach (var group in GroupMate)
                    {
                        List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                        ListRdoMaterial.Add(new Mrs00069RDO(listSub, false, false));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetMediStockName()
        {
            try
            {
                var mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager().GetById(castFilter.MEDI_STOCK_ID);
                if (IsNotNull(mediStock))
                {
                    MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                GetMediStockName();
                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);

                ListRdoMedicine = ListRdoMedicine.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                ListRdoMaterial = ListRdoMaterial.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
                objectTag.AddObjectData(store, "Medicines", ListRdoMedicine);
                objectTag.AddObjectData(store, "Materials", ListRdoMaterial);
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class RDOCustomerFuncRownumberData : TFlexCelUserFunction
    {
        public RDOCustomerFuncRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            long result = 0;
            try
            {
                long rownumber = Convert.ToInt64(parameters[0]);
                result = (rownumber + 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return result;
        }
    }
}
