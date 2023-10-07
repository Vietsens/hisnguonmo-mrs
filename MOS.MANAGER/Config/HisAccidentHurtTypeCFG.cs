using Inventec.Common.Logging;
using MOS.MANAGER.HisAccidentHurtType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.Config
{
    class HisAccidentHurtTypeCFG
    {
        private const string ACCIDENT_TYPE_CODE__OTHER = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.OTHER";
        private const string ACCIDENT_TYPE_CODE__VIOLENCE = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.VIOLENCE";
        private const string ACCIDENT_TYPE_CODE__SUICIDAL = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.SUICIDAL";
        private const string ACCIDENT_TYPE_CODE__POISONING = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.POISONING";
        private const string ACCIDENT_TYPE_CODE__BURN = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.BURN";
        private const string ACCIDENT_TYPE_CODE__UNDERWATER = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.UNDERWATER";
        private const string ACCIDENT_TYPE_CODE__ANIMAL = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.ANIMAL";
        private const string ACCIDENT_TYPE_CODE__FALL = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.FALL";
        private const string ACCIDENT_TYPE_CODE__LABOR = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.LABOR";
        private const string ACCIDENT_TYPE_CODE__FIGHT = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.FIGHT";
        private const string ACCIDENT_TYPE_CODE__LIFE = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.LIFE";
        private const string ACCIDENT_TYPE_CODE__TRAFFIC = "HIS.HIS_ACCIDENT_HURT_TYPE.ACCIDENT_TYPE_CODE.TRAFFIC";

        private static long accidentTypeIdOther;
        public static long ACCIDENT_TYPE_ID__OTHER
        {
            get
            {
                if (accidentTypeIdOther == 0)
                {
                    accidentTypeIdOther = GetId(ACCIDENT_TYPE_CODE__OTHER);
                }
                return accidentTypeIdOther;
            }
            set
            {
                accidentTypeIdOther = value;
            }
        }

        private static long accidentTypeIdViolence;
        public static long ACCIDENT_TYPE_ID__VIOLENCE
        {
            get
            {
                if (accidentTypeIdViolence == 0)
                {
                    accidentTypeIdViolence = GetId(ACCIDENT_TYPE_CODE__VIOLENCE);
                }
                return accidentTypeIdViolence;
            }
            set
            {
                accidentTypeIdViolence = value;
            }
        }

        private static long accidentTypeIdSucidal;
        public static long ACCIDENT_TYPE_ID__SUICIDAL
        {
            get
            {
                if (accidentTypeIdSucidal == 0)
                {
                    accidentTypeIdSucidal = GetId(ACCIDENT_TYPE_CODE__SUICIDAL);
                }
                return accidentTypeIdSucidal;
            }
            set
            {
                accidentTypeIdSucidal = value;
            }
        }

        private static long accidentTypeIdPoisoning;
        public static long ACCIDENT_TYPE_ID__POISONING
        {
            get
            {
                if (accidentTypeIdPoisoning == 0)
                {
                    accidentTypeIdPoisoning = GetId(ACCIDENT_TYPE_CODE__POISONING);
                }
                return accidentTypeIdPoisoning;
            }
            set
            {
                accidentTypeIdPoisoning = value;
            }
        }

        private static long accidentTypeIdBurn;
        public static long ACCIDENT_TYPE_ID__BURN
        {
            get
            {
                if (accidentTypeIdBurn == 0)
                {
                    accidentTypeIdBurn = GetId(ACCIDENT_TYPE_CODE__BURN);
                }
                return accidentTypeIdBurn;
            }
            set
            {
                accidentTypeIdBurn = value;
            }
        }

        private static long accidentTypeIdUnderwater;
        public static long ACCIDENT_TYPE_ID__UNDERWATER
        {
            get
            {
                if (accidentTypeIdUnderwater == 0)
                {
                    accidentTypeIdUnderwater = GetId(ACCIDENT_TYPE_CODE__UNDERWATER);
                }
                return accidentTypeIdUnderwater;
            }
            set
            {
                accidentTypeIdUnderwater = value;
            }
        }

        private static long accidentTypeIdAnimal;
        public static long ACCIDENT_TYPE_ID__ANIMAL
        {
            get
            {
                if (accidentTypeIdAnimal == 0)
                {
                    accidentTypeIdAnimal = GetId(ACCIDENT_TYPE_CODE__ANIMAL);
                }
                return accidentTypeIdAnimal;
            }
            set
            {
                accidentTypeIdAnimal = value;
            }
        }

        private static long accidentTypeIdFall;
        public static long ACCIDENT_TYPE_ID__FALL
        {
            get
            {
                if (accidentTypeIdFall == 0)
                {
                    accidentTypeIdFall = GetId(ACCIDENT_TYPE_CODE__FALL);
                }
                return accidentTypeIdFall;
            }
            set
            {
                accidentTypeIdFall = value;
            }
        }

        private static long accidentTypeIdFight;
        public static long ACCIDENT_TYPE_ID__FIGHT
        {
            get
            {
                if (accidentTypeIdFight == 0)
                {
                    accidentTypeIdFight = GetId(ACCIDENT_TYPE_CODE__FIGHT);
                }
                return accidentTypeIdFight;
            }
            set
            {
                accidentTypeIdFight = value;
            }
        }

        private static long accidentTypeIdLabor;
        public static long ACCIDENT_TYPE_ID__LABOR
        {
            get
            {
                if (accidentTypeIdLabor == 0)
                {
                    accidentTypeIdLabor = GetId(ACCIDENT_TYPE_CODE__LABOR);
                }
                return accidentTypeIdLabor;
            }
            set
            {
                accidentTypeIdLabor = value;
            }
        }

        private static long accidentTypeIdLife;
        public static long ACCIDENT_TYPE_ID__LIFE
        {
            get
            {
                if (accidentTypeIdLife == 0)
                {
                    accidentTypeIdLife = GetId(ACCIDENT_TYPE_CODE__LIFE);
                }
                return accidentTypeIdLife;
            }
            set
            {
                accidentTypeIdLife = value;
            }
        }

        private static long accidentTypeIdTraffic;
        public static long ACCIDENT_TYPE_ID__TRAFFIC
        {
            get
            {
                if (accidentTypeIdTraffic == 0)
                {
                    accidentTypeIdTraffic = GetId(ACCIDENT_TYPE_CODE__TRAFFIC);
                }
                return accidentTypeIdTraffic;
            }
            set
            {
                accidentTypeIdTraffic = value;
            }
        }

        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisAccidentHurtTypeGet().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Reload()
        {
            var idOther = GetId(ACCIDENT_TYPE_CODE__OTHER);
            var idViolence = GetId(ACCIDENT_TYPE_CODE__VIOLENCE);
            var idSucidal = GetId(ACCIDENT_TYPE_CODE__SUICIDAL);
            var idPoisoning = GetId(ACCIDENT_TYPE_CODE__POISONING);
            var idBurn = GetId(ACCIDENT_TYPE_CODE__BURN);
            var idUnderwater = GetId(ACCIDENT_TYPE_CODE__UNDERWATER);
            var idAnimal = GetId(ACCIDENT_TYPE_CODE__ANIMAL);
            var idFall = GetId(ACCIDENT_TYPE_CODE__FALL);
            var idFight = GetId(ACCIDENT_TYPE_CODE__FIGHT);
            var idLabor = GetId(ACCIDENT_TYPE_CODE__LABOR);
            var idLife = GetId(ACCIDENT_TYPE_CODE__LIFE);
            var idTraffic = GetId(ACCIDENT_TYPE_CODE__TRAFFIC);

            accidentTypeIdOther = idOther;
            accidentTypeIdViolence = idViolence;
            accidentTypeIdSucidal = idSucidal;
            accidentTypeIdPoisoning = idPoisoning;
            accidentTypeIdBurn = idBurn;
            accidentTypeIdUnderwater = idUnderwater;
            accidentTypeIdAnimal = idAnimal;
            accidentTypeIdFall = idFall;
            accidentTypeIdFight = idFight;
            accidentTypeIdLabor = idLabor;
            accidentTypeIdLife = idLife;
            accidentTypeIdTraffic = idTraffic;
        }
    }
}
