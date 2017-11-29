using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class ContCard
    {
        public long ContCardID { get; set; }
        public string CardMode { get; set; }
        public long RefID { get; set; }
        public string Cont { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public DateTime? Dtm1 { get; set; }
        public string Loc1 { get; set; }
        public DateTime? Dtm2 { get; set; }
        public string Loc2 { get; set; }
        public string Remark { get; set; }
        public DateTime? Dtm3 { get; set; }
        public string UserID3 { get; set; }
        public long ContInOutID { get; set; }
        public string Seal1 { get; set; }
        public string Seal2 { get; set; }
        public string Seal3 { get; set; }
        public string Seal4 { get; set; }
        public string NoMobilOut { get; set; }
        public string AngkutanOut { get; set; }
        public string EirOutNo { get; set; }
        public string Token { get; set; }
        public bool IsCombo { get; set; }

        public string Seal
        {
            get
            {
                string seal = string.Empty;
                if (Seal1.Length > 0)
                {
                    seal += Seal1;
                }
                if (Seal2.Length > 0)
                {
                    if (seal.Length > 0)
                    {
                        seal += ",";
                    }
                    seal += Seal2;
                }
                if (Seal3.Length > 0)
                {
                    if (seal.Length > 0)
                    {
                        seal += ",";
                    }
                    seal += Seal3;
                }
                if (Seal4.Length > 0)
                {
                    if (seal.Length > 0)
                    {
                        seal += ",";
                    }
                    seal += Seal4;
                }
                return seal;
            }
        }

        public ContCard()
        {
            ContCardID = 0;
            CardMode = string.Empty;
            RefID = 0;
            Cont = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            Dtm1 = null;
            Loc1 = string.Empty;
            Dtm2 = null;
            Loc2 = string.Empty;
            Remark = string.Empty;
            Dtm3 = null;
            UserID3 = string.Empty;
            ContInOutID = 0;

            Seal1 = string.Empty;
            Seal2 = string.Empty;
            Seal3 = string.Empty;
            Seal4 = string.Empty;

            NoMobilOut = string.Empty;
            AngkutanOut = string.Empty;
            EirOutNo = string.Empty;
            Token = string.Empty;
            IsCombo = false;
        }
    }
}
