namespace ITI.Survey.Web.Dll.Model
{
    public class Blok
    {
        public string Size { get; set; } //if (msize != "" && (!ContSizeList.Value.Contains(msize))) msize = "";
        public int VPad { get; set; } // if (mvpad < 0) mvpad = 0;
        public int XPos { get; set; } // if (mxpos < 0) mxpos = 0;
        public int YPos { get; set; } // if (mypos < 0) mypos = 0;        
        public long BlokId { get; set; }        
        public bool Disabled { get; set; }
        public string Kode { get; set; } // mkode = mkode.ToUpper();
        public string KodeBlok { get; set; }
        public string Row { get; set; }
        public string Tear { get; set; }
        public string Stack { get; set; }
        public int MaxTier { get; set; }
        public string Remark { get; set; }
        public string Cont { get; set; }
        public string Cont2 { get; set; }
        public string ContCheck1 { get; set; }
        public string ContCheck2 { get; set; }

        public Blok()
        {
            Size = string.Empty;
            VPad = 0;
            XPos = 0;
            YPos = 0;
            BlokId = 0;
            Disabled = false;
            Kode = string.Empty;
            KodeBlok = string.Empty;
            Row = string.Empty;
            Tear = string.Empty;
            Stack = string.Empty;
            MaxTier = 0;
            Remark = string.Empty;
            Cont = string.Empty;
            Cont2 = string.Empty;
            ContCheck1 = string.Empty;
            ContCheck2 = string.Empty;
        }
    }
}
