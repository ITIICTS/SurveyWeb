using ITI.Survey.Web.Dll.DAL;
using System;

namespace ITI.Survey.Web.Dll.Model
{
    public class TruckInDepo
    {
        private string _duration = string.Empty;

        #region PROPS
        public long TruckInDepoId { get; set; }
        public string NoMobil { get; set; }
        public DateTime DtmIn { get; set; }
        public DateTime? DtmOut { get; set; }
        public string Remark { get; set; }
        public string RefNo { get; set; }
        public string Muatan { get; set; }
        public string Tipe { get; set; }
        public string Angkutan { get; set; }
        public string Note { get; set; }

        public string Durasi
        {
            get
            {
                if(_duration.Length > 0)
                {
                    return _duration;
                }

                int minute = 0;
                if (DtmOut.HasValue)
                {
                    minute = (int)GlobalWebServiceDAL.GetServerDtm().Subtract(DtmIn).TotalMinutes;
                }
                else
                {
                    minute = (int)DtmOut.Value.Subtract(DtmIn).TotalMinutes;
                }

                _duration = minute.ToString() + " min";
                if (minute > 120 && minute <= 180)
                {
                    _duration = "> 2 hours";
                }
                else if (minute > 180 && minute <= 240)
                {
                    _duration = "> 3 hours";
                }
                else if (minute > 240 && minute <= 300)
                {
                    _duration =  "> 4 hours";
                }
                else if (minute > 300)
                {
                    _duration = "> 5 hours";
                }
                return _duration;
            }
        }

        #endregion

        public TruckInDepo()
        {
            TruckInDepoId = 0;
            NoMobil = string.Empty;
            DtmIn = DateTime.MinValue;
            DtmOut = null;
            Remark = string.Empty;
            RefNo = string.Empty;
            Muatan = string.Empty;
            Tipe = string.Empty;
            Angkutan = string.Empty;
            Note = string.Empty;
        }
    }
}
