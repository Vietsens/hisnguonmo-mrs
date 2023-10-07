using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00410
{
    public class RDOImpExpMestTypeContext
    {
        public static void Define(ref Dictionary<string, long> dicImpMestType, ref Dictionary<string, long> dicExpMestType)
        {
            //Danh sach loai nhap, loai xuat
            dicExpMestType.Add("ID__BAN_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN);//8;
            dicExpMestType.Add("ID__BCS_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS);//5;
            dicExpMestType.Add("ID__BL_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL);//13;
            dicExpMestType.Add("ID__BCT_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCT);//14;
            dicExpMestType.Add("ID__CK_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK);//3;
            dicExpMestType.Add("ID__DM_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM);//12;
            dicExpMestType.Add("ID__DNT_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT);//9;
            dicExpMestType.Add("ID__DPK_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK);//1;
            dicExpMestType.Add("ID__DTT_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT);//11;
            dicExpMestType.Add("ID__HPKP_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP);//2;
            dicExpMestType.Add("ID__KHAC_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC);//10;
            dicExpMestType.Add("ID__PL_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL);//7;
            dicExpMestType.Add("ID__TNCC_EXP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC);//4;

            dicImpMestType.Add("ID__BCS_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS); //8;
            dicImpMestType.Add("ID__BL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL); //9;
            dicImpMestType.Add("ID__CK_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK); //1;
            dicImpMestType.Add("ID__DK_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DK); //6;
            dicImpMestType.Add("ID__DMTL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL); //12;
            dicImpMestType.Add("ID__DNTTL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL); //10;
            dicImpMestType.Add("ID__DTTTL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL); //11;
            dicImpMestType.Add("ID__HPTL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL); //13;
            dicImpMestType.Add("ID__BCT_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCT); //14;
            dicImpMestType.Add("ID__BTL_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL); //15;
            dicImpMestType.Add("ID__KK_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__KK); //5;
            dicImpMestType.Add("ID__KHAC_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__KHAC); //7;
            dicImpMestType.Add("ID__NCC_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC); //2;
            dicImpMestType.Add("ID__TH_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH); //3;
            dicImpMestType.Add("ID__THT_IMP_AMOUNT", IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT); //4;  
        }
    }
}
