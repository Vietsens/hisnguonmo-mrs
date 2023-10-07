using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisDepartment;

namespace MRS.MANAGER.Config
{
    public class HisDepartmentCFG
    {
        private const string HIS_DEPARTMENT_CODE__SUBCLINICAL = "MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__SUBCLINICAL";//Ma cac khoa can lam sang
        private const string HIS_DEPARTMENT_CODE__CLINICAL = "MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__CLINICAL";//Danh sach ma cac khoa lam sang
        private const string HIS_DEPARTMENT_CODE__EXAM = "MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__EXAM";//Danh sach ma cac khoa kham benh
        private const string HIS_DEPARTMENT_CODE__LIST_SURGERY = "MRS.HIS_DEPARTMENT.DEPARTMENT_CODE.LIST_SURGERY";//Danh sach cac ma khoa ngoai
        private const string HIS_DEPARTMENT_CODE__LIST_INTERNAL_MEDICINE = "MRS.HIS_DEPARTMENT.DEPARTMENT_CODE.LIST_INTERNAL_MEDICINE";//Danh sach cac ma khoa noi
        private const string HIS_DEPARTMENT_CODE__LIST_SPECIALTY = "MRS.HIS_DEPARTMENT.DEPARTMENT_CODE.LIST_SPECIALTY";//Danh sach cac ma khoa chuyen khoa
        private const string HIS_DEPARTMENT_CODE__CDHA = "MRS.HIS_RS.HIS_DEPARTMENT.DEPARTMENT_CODE.CDHA";// mã khoa chẩn đoán hình ảnh

        private const string HIS_DEPARTMENT_CODE__YHCT = "MRS.HIS_RS.HIS_DEPARTMENT.HIS_DEPARTMENT_CODE__YHCT";
        private const string HIS_DEPARTMENT_CODE__LIST_RESUSCITATION = "MOS.HIS_DEPARTMENT.RESUSCITATION_DEPARTMENT_CODES";//Danh sach cac ma khoa khong dieu tri

        private static List<long> departmentIdYHCT;
        public static List<long> HIS_DEPARTMENT_ID__YHCT
        {
            get
            {
                if (departmentIdYHCT == null || departmentIdYHCT.Count <= 0)
                {
                    departmentIdYHCT = GetIds(HIS_DEPARTMENT_CODE__YHCT);
                }
                return departmentIdYHCT;
            }
            set
            {
                departmentIdYHCT = value;
            }
        }

        private static List<long> departmentIdCdha;
        public static List<long> DEPARTMENT_ID__CDHA
        {
            get
            {
                if (departmentIdCdha == null || departmentIdCdha.Count == 0)
                {
                    departmentIdCdha = GetIds(HIS_DEPARTMENT_CODE__CDHA);
                }
                return departmentIdCdha;
            }
            set
            {
                departmentIdCdha = value;
            }
        }

        private static List<long> departmentIdSubClinical;
        public static List<long> HIS_DEPARTMENT_ID__SUBCLINICAL
        {
            get
            {
                if (departmentIdSubClinical == null || departmentIdSubClinical.Count == 0)
                {
                    departmentIdSubClinical = GetIds(HIS_DEPARTMENT_CODE__SUBCLINICAL);
                }
                return departmentIdSubClinical;
            }
            set
            {
                departmentIdSubClinical = value;
            }
        }

        private static List<long> departmentIdClinical;
        public static List<long> HIS_DEPARTMENT_ID__CLINICAL
        {
            get
            {
                if (departmentIdClinical == null || departmentIdClinical.Count == 0)
                {
                    departmentIdClinical = GetIds(HIS_DEPARTMENT_CODE__CLINICAL);
                }
                return departmentIdClinical;
            }
            set
            {
                departmentIdClinical = value;
            }
        }

        private static List<long> departmentIdExam;
        public static List<long> HIS_DEPARTMENT_ID__EXAM
        {
            get
            {
                if (departmentIdExam == null || departmentIdExam.Count == 0)
                {
                    departmentIdExam = GetIds(HIS_DEPARTMENT_CODE__EXAM);
                }
                return departmentIdExam;
            }
            set
            {
                departmentIdExam = value;
            }
        }

        private static List<long> departmentIdsGroupCls;
        public static List<long> LIST_DEPARTMENT_ID__GROUP_CLS
        {
            get
            {
                if (departmentIdsGroupCls == null || departmentIdsGroupCls.Count == 0)
                {
                    departmentIdsGroupCls = new List<long>();

                    if (HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM != null)
                        departmentIdsGroupCls.AddRange(HisDepartmentCFG.HIS_DEPARTMENT_ID__SUBCLINICAL);
                }
                return departmentIdsGroupCls;
            }
            set
            {
                departmentIdsGroupCls = value;
            }
        }

        private static List<long> departmentIdsGroupLs;
        public static List<long> LIST_DEPARTMENT_ID__GROUP_LS
        {
            get
            {
                if (departmentIdsGroupLs == null || departmentIdsGroupLs.Count == 0)
                {
                    departmentIdsGroupLs = new List<long>();
                    if (HisDepartmentCFG.HIS_DEPARTMENT_ID__CLINICAL != null)
                    {
                        departmentIdsGroupLs.AddRange(HisDepartmentCFG.HIS_DEPARTMENT_ID__CLINICAL);
                    }
                }
                return departmentIdsGroupLs;
            }
            set
            {
                departmentIdsGroupLs = value;
            }
        }

        private static List<long> departmentIdsGroupKkb;
        public static List<long> LIST_DEPARTMENT_ID__GROUP_KKB
        {
            get
            {
                if (departmentIdsGroupKkb == null || departmentIdsGroupKkb.Count == 0)
                {
                    departmentIdsGroupKkb = new List<long>();
                    if(HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM!=null)
                    departmentIdsGroupKkb.AddRange(HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM);
                }
                return departmentIdsGroupKkb;
            }
            set
            {
                departmentIdsGroupKkb = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT> departments;
        public static List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT> DEPARTMENTs
        {
            get
            {
                if (departments == null || departments.Count == 0)
                {
                    departments = new List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT>();
                    departments.AddRange(GetAll());
                }
                return departments;
            }
            set
            {
                departments = value;
            }
        }

        private static List<long> departmentIdListSurgery;
        public static List<long> HIS_DEPARTMENT_ID__LIST_SURGERY
        {
            get
            {
                if (departmentIdListSurgery == null || departmentIdListSurgery.Count == 0)
                {
                    departmentIdListSurgery = GetIds(HIS_DEPARTMENT_CODE__LIST_SURGERY);
                }
                return departmentIdListSurgery;
            }
            set
            {
                departmentIdListSurgery = value;
            }
        }

        private static List<long> departmentIdInternalMedicines;
        public static List<long> HIS_DEPARTMENT_ID__LIST_INTERNAL_MEDICINE
        {
            get
            {
                if (departmentIdInternalMedicines == null || departmentIdInternalMedicines.Count == 0)
                {
                    departmentIdInternalMedicines = GetIds(HIS_DEPARTMENT_CODE__LIST_INTERNAL_MEDICINE);
                }
                return departmentIdInternalMedicines;
            }
            set
            {
                departmentIdInternalMedicines = value;
            }
        }

        private static List<long> departmentIdSpecialties;
        public static List<long> HIS_DEPARTMENT_ID__LIST_SPECIALTY
        {
            get
            {
                if (departmentIdSpecialties == null || departmentIdSpecialties.Count == 0)
                {
                    departmentIdSpecialties = GetIds(HIS_DEPARTMENT_CODE__LIST_SPECIALTY);
                }
                return departmentIdSpecialties;
            }
            set
            {
                departmentIdSpecialties = value;
            }
        }

        private static List<long> departmentIdListResuscitation;
        public static List<long> HIS_DEPARTMENT_ID__LIST_RESUSCITATION 
        {
            get
            {
                if (departmentIdListResuscitation == null || departmentIdListResuscitation.Count == 0)
                {
                    departmentIdListResuscitation = GetIds(HIS_DEPARTMENT_CODE__LIST_RESUSCITATION);
                }
                return departmentIdListResuscitation;
            }
            set
            {
                departmentIdListResuscitation = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                //filter.DEPARTMENT_CODE = value;//TODO
                var data = new HisDepartmentManager().Get(filter).FirstOrDefault(o => o.DEPARTMENT_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = null;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                //filter.DEPARTMENT_CODE = value;//TODO
                var data = new HisDepartmentManager().Get(filter).Where(o => value.Contains(o.DEPARTMENT_CODE)).ToList();
                if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_DEPARTMENT> result = null;
            try
            {
                HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                result = new HisDepartmentManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                departmentIdYHCT = null;
                departmentIdCdha = null;
                departmentIdSubClinical = null;
                departmentIdClinical = null;
                departmentIdExam = null;
                departmentIdsGroupCls = null;
                departmentIdsGroupLs = null;
                departmentIdsGroupKkb = null;
                departments = null;
                departmentIdListSurgery = null;
                departmentIdInternalMedicines = null;
                departmentIdSpecialties = null;
                departmentIdListResuscitation = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
