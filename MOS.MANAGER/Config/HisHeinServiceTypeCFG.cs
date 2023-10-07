using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisHeinServiceType;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisHeinServiceTypeCFG
    {
        //private const string GI_NT_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.BED_INPATIENTS";//giuong noi tru
        //private const string GI_NGT_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.BED_OUTPATIENTS";//giuong ngoai tru
        //private const string MAU_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.BLOOD";//mau va che pham mau
        //private const string CDHA_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.DIIM";//chan doan hinh anh
        //private const string KH_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.EXAM";//kham benh
        //private const string TDCN_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.FUEX";//tham do chuc nang
        //private const string DVKT_TTL_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.HIGHTECH";//dich vu ki thuat cao
        //private const string PTTT_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.SURGMISU";//pttt
        //private const string XN_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.TEST";//xet nghiem
        //private const string VC_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.TRANS";//van chuyen

        //private const string VT_TT_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MATERIAL_VTTT";//vat tu thay the
        //private const string VT_TTL_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MATERIAL_RATIO";//vat thu thanh toan theo ty le
        //private const string VT_TDM_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MATERIAL_IN";//vat tu trong danh muc
        //private const string VT_NDM_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MATERIAL_OUT";//vat tu ngoai danh muc

        //private const string T_DTUT_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MEDICINE_CANCER";//thuoc chong thai ghep, dieu tri ung thu
        //private const string T_TDM_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MEDICINE_IN";//thuoc trong danh muc
        //private const string T_NDM_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MEDICINE_OUT";//thuoc ngoai danh muc
        //private const string T_TTL_CFG = "MOS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE.MEDICINE_RATIO";//thuoc thanh toan theo ty le

        private static List<HIS_HEIN_SERVICE_TYPE> data;
        public static List<HIS_HEIN_SERVICE_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisHeinServiceTypeGet().Get(new HisHeinServiceTypeFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        private static long gint;
        public static long GI_NT
        {
            get
            {
                if (gint == 0)
                {
                    gint = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT;
                }
                return gint;
            }
            set
            {
                gint = value;
            }
        }

        private static long gingt;
        public static long GI_NGT
        {
            get
            {
                if (gingt == 0)
                {
                    gingt = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT;
                }
                return gingt;
            }
            set
            {
                gingt = value;
            }
        }

        private static long mau;
        public static long MAU
        {
            get
            {
                if (mau == 0)
                {
                    mau = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU;
                }
                return mau;
            }
            set
            {
                mau = value;
            }
        }

        private static long cdha;
        public static long CDHA
        {
            get
            {
                if (cdha == 0)
                {
                    cdha = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA;
                }
                return cdha;
            }
            set
            {
                cdha = value;
            }
        }

        private static long kh;
        public static long KH
        {
            get
            {
                if (kh == 0)
                {
                    kh = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH;
                }
                return kh;
            }
            set
            {
                kh = value;
            }
        }

        private static long tdcn;
        public static long TDCN
        {
            get
            {
                if (tdcn == 0)
                {
                    tdcn = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN;
                }
                return tdcn;
            }
            set
            {
                tdcn = value;
            }
        }

        private static long dvktttl;
        public static long DVKT_TTL
        {
            get
            {
                if (dvktttl == 0)
                {
                    dvktttl = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC;
                }
                return dvktttl;
            }
            set
            {
                dvktttl = value;
            }
        }

        private static long pttt;
        public static long PTTT
        {
            get
            {
                if (pttt == 0)
                {
                    pttt = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT;
                }
                return pttt;
            }
            set
            {
                pttt = value;
            }
        }

        private static long xn;
        public static long XN
        {
            get
            {
                if (xn == 0)
                {
                    xn = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN;
                }
                return xn;
            }
            set
            {
                xn = value;
            }
        }

        private static long vc;
        public static long VC
        {
            get
            {
                if (vc == 0)
                {
                    vc = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC;
                }
                return vc;
            }
            set
            {
                vc = value;
            }
        }

        private static long vttt;
        public static long VT_TT
        {
            get
            {
                if (vttt == 0)
                {
                    vttt = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT;
                }
                return vttt;
            }
            set
            {
                vttt = value;
            }
        }

        private static long vtttl;
        public static long VT_TTL
        {
            get
            {
                if (vtttl == 0)
                {
                    vtttl = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL;
                }
                return vtttl;
            }
            set
            {
                vtttl = value;
            }
        }

        private static long vttdm;
        public static long VT_TDM
        {
            get
            {
                if (vttdm == 0)
                {
                    vttdm = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM;
                }
                return vttdm;
            }
            set
            {
                vttdm = value;
            }
        }

        private static long vtndm;
        public static long VT_NDM
        {
            get
            {
                if (vtndm == 0)
                {
                    vtndm = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM;
                }
                return vtndm;
            }
            set
            {
                vtndm = value;
            }
        }

        private static long tdtut;
        public static long T_DTUT
        {
            get
            {
                if (tdtut == 0)
                {
                    tdtut = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT;
                }
                return tdtut;
            }
            set
            {
                tdtut = value;
            }
        }

        private static long ttdm;
        public static long T_TDM
        {
            get
            {
                if (ttdm == 0)
                {
                    ttdm = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM;
                }
                return ttdm;
            }
            set
            {
                ttdm = value;
            }
        }

        private static long tndm;
        public static long T_NDM
        {
            get
            {
                if (tndm == 0)
                {
                    tndm = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM;
                }
                return tndm;
            }
            set
            {
                tndm = value;
            }
        }

        private static long tttl;
        public static long T_TTL
        {
            get
            {
                if (tttl == 0)
                {
                    tttl = IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL;
                }
                return tttl;
            }
            set
            {
                tttl = value;
            }
        }

        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    HIS_HEIN_SERVICE_TYPE heinServiceType = DATA != null ? DATA.Where(o => o.HEIN_SERVICE_TYPE_CODE == value).FirstOrDefault() : null;
                    return heinServiceType != null ? heinServiceType.ID : -1;
                }
                return -1;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Reload()
        {
            //var tmp = new HisHeinServiceTypeGet().Get(new HisHeinServiceTypeFilterQuery());
            //data = tmp;

            //var idBedIn = GetId(GI_NT_CFG);
            //var idBedOut = GetId(GI_NGT_CFG);
            //var idBlood = GetId(MAU_CFG);
            //var idDiim = GetId(CDHA_CFG);
            //var idExam = GetId(KH_CFG);
            //var idFuex = GetId(TDCN_CFG);
            //var idHight = GetId(DVKT_TTL_CFG);
            //var idPttt = GetId(PTTT_CFG);
            //var idXn = GetId(XN_CFG);
            //var idTran = GetId(VC_CFG);
            //var idVttt = GetId(VT_TT_CFG);
            //var idVtTtl = GetId(VT_TTL_CFG);
            //var idVtTdm = GetId(VT_TDM_CFG);
            //var idVtNdm = GetId(VT_NDM_CFG);
            //var idThUt = GetId(T_DTUT_CFG);
            //var idThTdm = GetId(T_TDM_CFG);
            //var idThNdm = GetId(T_NDM_CFG);
            //var idThTtl = GetId(T_TTL_CFG);

            //gint = idBedIn;
            //gingt = idBedOut;
            //mau = idBlood;
            //cdha = idDiim;
            //kh = idExam;
            //tdcn = idFuex;
            //dvktttl = idHight;
            //pttt = idPttt;
            //xn = idXn;
            //vc = idTran;
            //vttt = idVttt;
            //vtttl = idVtTtl;
            //vttdm = idVtTdm;
            //vtndm = idVtNdm;
            //tdtut = idThUt;
            //tttl = idThTtl;
            //ttdm = idThTdm;
            //tndm = idThNdm;

        }
    }
}
