using MOS.MANAGER.HisAccidentHurt;
using MOS.MANAGER.HisTreatment;
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
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisAccidentBodyPart;
using MOS.MANAGER.HisDeathCause;
using MOS.MANAGER.HisPatient;
using System.Reflection;
using Inventec.Common.Logging;
using SDA.MANAGER.Manager;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaCommune.Get;

namespace MRS.Processor.Mrs00590
{
    class Mrs00590Processor : AbstractProcessor
    {
        Mrs00590Filter castFilter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_DEPARTMENT_TRAN> lastHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<SDA_COMMUNE> listSdaCommune = new List<SDA_COMMUNE>();
        List<V_SDA_DISTRICT> listSdaDistrict = new List<V_SDA_DISTRICT>();
        List<Mrs00590RDO> listRdo = new List<Mrs00590RDO>();
        Dictionary<string, int> DIC_ICD = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_DIE = new Dictionary<string, int>();
        string thisReportTypeCode = "";
        public Mrs00590Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00590Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00590Filter)this.reportFilter;
                List<string> ProvinceCodes = new List<string>();
                List<string> DistrictCodes = new List<string>();
                if (this.castFilter.DISTRICT_CODEs != null)
                {
                    ProvinceCodes = this.castFilter.DISTRICT_CODEs.Select(o => o.Split('_').First()).ToList();
                    DistrictCodes = this.castFilter.DISTRICT_CODEs.Select(o => o.Split('_').Last()).ToList();
                }
                //Danh sách huyện
                listSdaDistrict = new SdaDistrictManager(new CommonParam()).Get<List<V_SDA_DISTRICT>>(new SdaDistrictViewFilterQuery());
                //Danh sach xã
                if (listSdaDistrict != null)
                {
                    listSdaDistrict = listSdaDistrict.Where(o => ProvinceCodes.Contains(o.PROVINCE_CODE??"") && DistrictCodes.Contains(o.DISTRICT_CODE??"")).ToList();
                    SdaCommuneFilterQuery SdaCommunefilter = new SdaCommuneFilterQuery();
                    SdaCommunefilter.DISTRICT_IDs = listSdaDistrict.Select(o => o.ID).ToList();
                    listSdaCommune = new SdaCommuneManager(new CommonParam()).Get<List<SDA_COMMUNE>>(SdaCommunefilter);
                }
                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter = this.MapFilter<Mrs00590Filter, HisTreatmentFilterQuery>(castFilter, listHisTreatmentfilter);
                listHisTreatment = new HisTreatmentManager(new CommonParam()).Get(listHisTreatmentfilter);
                
                if (listHisTreatment != null && listHisTreatment.Count > 0)
                {
                    var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                    List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);

                    }
                    lastHisDepartmentTran = listHisDepartmentTran.Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME.Value).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(p => lastHisDepartmentTran.Exists(o => o.TREATMENT_ID == p.ID && castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID))).ToList();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                List<string> listIcdCode = new List<string>();
                listIcdCode.AddRange(this.castFilter.ICD_CODE__ADs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__CUs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__HHTs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__LAs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__LTs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__QBs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__TCs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__TDs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__VGs);
                listIcdCode.AddRange(this.castFilter.ICD_CODE__VPs);

                Inventec.Common.Logging.LogSystem.Info("Not In:" + string.Join("','", listHisTreatment.Where(p => listIcdCode.Contains(p.ICD_CODE) && !listSdaCommune.Exists(q => Commune(p.TDL_PATIENT_ADDRESS, q))).Select(o => o.TREATMENT_CODE).ToList()));
                foreach (var item in listSdaCommune)
                {
                    Mrs00590RDO rdo = new Mrs00590RDO();
                    List<HIS_TREATMENT> listSub = listHisTreatment.Where(o => Commune(o.TDL_PATIENT_ADDRESS, item)).ToList();

                    rdo.COMMUNE_NAME = item.COMMUNE_NAME;
                    
                    bool HasData = false;

                    HasData = LoadToRdo(rdo, listSub);
                    if (HasData)
                    {
                        listRdo.Add(rdo);
                    }
                }

                DIC_ICD = listHisTreatment.GroupBy(r => IcdCode(r.ICD_CODE)).ToDictionary(p => p.Key, q => q.Count());
                DIC_ICD_DIE = listHisTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET).GroupBy(r => IcdCode(r.ICD_CODE)).ToDictionary(p => p.Key, q => q.Count());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string IcdCode(string IcdCode)
        {
            string result = "";
            try
            {
                result = (IcdCode ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private bool LoadToRdo(Mrs00590RDO rdo, List<HIS_TREATMENT> listSub)
        {

            bool result = false;
            try
            {
                if (castFilter.ICD_CODE__HHTs != null)
                {
                    rdo.COUNT_ICD_CODE__HHTs = listSub.Count(o => castFilter.ICD_CODE__HHTs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__HHTs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__VPs != null)
                {
                    rdo.COUNT_ICD_CODE__VPs = listSub.Count(o => castFilter.ICD_CODE__VPs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__VPs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__CUs != null)
                {
                    rdo.COUNT_ICD_CODE__CUs = listSub.Count(o => castFilter.ICD_CODE__CUs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__CUs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__LAs != null)
                {
                    rdo.COUNT_ICD_CODE__LAs = listSub.Count(o => castFilter.ICD_CODE__LAs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__LAs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__LTs != null)
                {
                    rdo.COUNT_ICD_CODE__LTs = listSub.Count(o => castFilter.ICD_CODE__LTs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__LTs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__QBs != null)
                {
                    rdo.COUNT_ICD_CODE__QBs = listSub.Count(o => castFilter.ICD_CODE__QBs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__QBs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__TDs != null)
                {
                    rdo.COUNT_ICD_CODE__TDs = listSub.Count(o => castFilter.ICD_CODE__TDs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__TDs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__TCs != null)
                {
                    rdo.COUNT_ICD_CODE__TCs = listSub.Count(o => castFilter.ICD_CODE__TCs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__TCs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__VGs != null)
                {
                    rdo.COUNT_ICD_CODE__VGs = listSub.Count(o => castFilter.ICD_CODE__VGs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__VGs > 0)
                    {
                        result = true;
                    }
                }
                if (castFilter.ICD_CODE__ADs != null)
                {
                    rdo.COUNT_ICD_CODE__ADs = listSub.Count(o => castFilter.ICD_CODE__ADs.Contains(o.ICD_CODE));
                    if (rdo.COUNT_ICD_CODE__ADs > 0)
                    {
                        result = true;
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

        private bool Commune(string address, SDA_COMMUNE commune)
        {
            bool result = false;
            try
            {
                if (commune == null)
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(address))
                {
                    return false;
                }
                address = address.Replace("Đắc Lắc", "Đắk Lắk");
                address = address.Replace("Đăk Lăk", "Đắk Lắk");
                List<string> addresSsub = address.Split(',').ToList();
                foreach (var addressCommune in addresSsub)
                {
                    if (addressCommune != null)
                    {
                        int i = addresSsub.IndexOf(addressCommune);
                        V_SDA_DISTRICT district = this.listSdaDistrict.FirstOrDefault(o => o.ID == commune.DISTRICT_ID);
                        if (district != null && i >= 0)
                        {
                            
                            
                            if (addresSsub[i].ToUpper().Contains(commune.COMMUNE_NAME.ToUpper()) && addresSsub[i + 1].ToUpper().Contains(district.DISTRICT_NAME.ToUpper()) && addresSsub[i + 2].ToUpper().Contains(district.PROVINCE_NAME.ToUpper()))
                            {
                                result = true;
                                break;
                            }
                        }
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
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DIC_ICD_DIE", DIC_ICD_DIE);
            dicSingleTag.Add("DIC_ICD", DIC_ICD);
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_FROM ?? castFilter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_TO ?? castFilter.OUT_TIME_TO ?? 0));
            if (castFilter.DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(",", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.SetUserFunction(store, "SumKeys", new RDOSumKeys());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }


    }
}
