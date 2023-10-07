using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisFilmSize;

namespace MRS.MANAGER.Config
{
    public class HisFilmSizeCFG
    {
        private const string FILM_SIZE_CODE__RETIRED_01 = "MRS.HIS_FILM_SIZE.FILM_SIZE_CODE.01";

        private static long careerIdRetiredMilitary;
        public static long FILM_SIZE_ID__RETIRED_01
        {
            get
            {
                if (careerIdRetiredMilitary == 0)
                {
                    careerIdRetiredMilitary = GetId(FILM_SIZE_CODE__RETIRED_01);
                }
                return careerIdRetiredMilitary;
            }
            set
            {
                careerIdRetiredMilitary = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_FILM_SIZE> FilmSizes;
        public static List<MOS.EFMODEL.DataModels.HIS_FILM_SIZE> FILM_SIZEs
        {
            get
            {
                if (FilmSizes == null)
                {
                    FilmSizes = GetAll();
                }
                return FilmSizes;
            }
            set
            {
                FilmSizes = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_FILM_SIZE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_FILM_SIZE> result = null;
            try
            {
                HisFilmSizeFilterQuery filter = new HisFilmSizeFilterQuery();
                result = new HisFilmSizeManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisFilmSizeManager().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                careerIdRetiredMilitary = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
