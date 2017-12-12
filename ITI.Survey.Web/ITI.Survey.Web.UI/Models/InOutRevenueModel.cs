using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITI.Survey.Web.UI.Models
{
    public class InOutRevenueModel
    {
        #region Properties
        public long InOutRevenueId { get; set; }
        public long RefId { get; set; }
        public string MovAct { get; set; }
        public string RevenueType { get; set; }
        public string BillTo { get; set; }
        public double LiftOff { get; set; }
        public string LiftOffInfo { get; set; }
        public double LiftOn { get; set; }
        public string LiftOnInfo { get; set; }
        public double Wash { get; set; }
        public string WashInfo { get; set; }
        public double Admin { get; set; }
        public string AdminInfo { get; set; }
        public double Storage { get; set; }
        public string StorageInfo { get; set; }
        public string TextFlag { get; set; }
        public string NoSeri { get; set; }
        public int Qty { get; set; }
        public int IsCanceled { get; set; }
        public string ContactPersonOut { get; set; }
        public double LiftOffSplit { get; set; }
        public double LiftOnSplit { get; set; }
        public double WashSplit { get; set; }
        public string KodeKasir { get; set; }
        public string InvBy { get; set; }
        public string KasirBy { get; set; }
        public string DtmInv { get; set; }
        public string DtmKasir { get; set; }
        public string TakeDef { get; set; }
        public string Take20 { get; set; }
        public string Take40 { get; set; }
        public string Take45 { get; set; }
        public string KasirNote { get; set; }
        public double PrincipalAdmin { get; set; }
        public double PrincipalLiftOff { get; set; }
        public double PrincipalLiftOn { get; set; }
        public double PrincipalWash { get; set; }
        public double PrincipalStorage { get; set; }
        public string SpNote1 { get; set; }
        #endregion

        public InOutRevenueModel()
        {
            InOutRevenueId = 0;
            RefId = 0;
            MovAct = string.Empty;
            RevenueType = string.Empty;
            BillTo = string.Empty;
            LiftOff = 0;
            LiftOffInfo = string.Empty;
            LiftOn = 0;
            LiftOnInfo = string.Empty;
            Wash = 0;
            WashInfo = string.Empty;
            Admin = 0;
            AdminInfo = string.Empty;
            Storage = 0;
            StorageInfo = string.Empty;
            TextFlag = string.Empty;
            NoSeri = string.Empty;
            Qty = 0;
            IsCanceled = 0;
            ContactPersonOut = string.Empty;
            LiftOffSplit = 0;
            LiftOnSplit = 0;
            WashSplit = 0;
            KodeKasir = string.Empty;
            InvBy = string.Empty;
            KasirBy = string.Empty;
            DtmInv = string.Empty;
            DtmKasir = string.Empty;
            TakeDef = string.Empty;
            Take20 = string.Empty;
            Take40 = string.Empty;
            Take45 = string.Empty;
            KasirNote = string.Empty;
            PrincipalAdmin = 0;
            PrincipalLiftOff = 0;
            PrincipalLiftOn = 0;
            PrincipalWash = 0;
            PrincipalStorage = 0;
            SpNote1 = string.Empty;
        }
    }
}