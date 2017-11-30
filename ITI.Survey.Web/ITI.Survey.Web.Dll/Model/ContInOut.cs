namespace ITI.Survey.Web.Dll.Model
{
    public class ContInOut
    {
        #region Property
        public long ContInOutId { get; set; }
        public string Cont { get; set; }
        public string CustomerCode { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Berat { get; set; }
        public string Manufacture { get; set; }
        public string CscPlate { get; set; }
        public string DtmIn { get; set; }
        public string DtmOut { get; set; }
        public string DtmRepaired { get; set; }
        public string DtmPti { get; set; }
        public string WashStatus { get; set; }
        public string Condition { get; set; }
        public string ActIn { get; set; }
        public string ActOut { get; set; }
        public string Location { get; set; }
        public string ExVessel { get; set; }
        public string Consignee { get; set; }
        public string Commodity { get; set; }
        public string NoMobilIn { get; set; }
        public string AngkutanIn { get; set; }
        public string NoMobilOut { get; set; }
        public string AngkutanOut { get; set; }
        public string DoNumber { get; set; }
        public string Shipper { get; set; }
        public string Destination { get; set; }
        public string Seal { get; set; }
        public string VesselVoyage { get; set; }
        public string Remarks { get; set; }
        public string Payload { get; set; }
        public string ExVesselName { get; set; }
        public string VesselVoyageName { get; set; }
        public string EirInContact { get; set; }
        public string DtmEirIn { get; set; }
        public string NoSeriOrOut { get; set; }
        public string EdiIn { get; set; }
        public string EdiOut { get; set; }
        public string EdiWash { get; set; }
        public string EdiApr { get; set; }
        public string EdiCom { get; set; }
        public string EdiPti { get; set; }
        public string EdiSync { get; set; }
        public string EirIn { get; set; }
        public string EirOut { get; set; }
        public string RfMachine { get; set; }
        public string DestinationName { get; set; }
        public string BlNumber { get; set; }
        public string DtmPortOut { get; set; }
        public string DtmOutDepoIn { get; set; }
        public string Tare { get; set; }
        public string CleaningRefNo { get; set; }
        public string CleaningRemark { get; set; }
        public string CleaningLastCargo { get; set; }
        public string CleaningDtmFinish { get; set; }
        public double CleaningCost { get; set; }
        public string BookingAssignment { get; set; }
        public string CleaningKode { get; set; }
        public string CleaningDesc { get; set; }
        public string CleaningAction { get; set; }
        public string DtmShortPti { get; set; }
        public string ExVesselPort { get; set; }
        public string RfEngineCond { get; set; }
        public string RfDtmEngineRepaired { get; set; }
        public string RfEngineEdiIn { get; set; }
        public string RfEngineEdiCom { get; set; }
        public string RfPtiType { get; set; }
        public string RfPtiDtmApproved { get; set; }
        public string RfPtiDtmCompleted { get; set; }
        public string RfPtiRemark { get; set; }
        public double RfPtiCost { get; set; }
        public string RfPtiTemp { get; set; }
        public int RfNeedSwUpdate { get; set; }
        public string RfDtmSwUpdated { get; set; }
        public string Grade { get; set; }
        public string GradeV2 { get; set; }
        public string MddcRemark { get; set; }
        public string BusinessUnit { get; set; }
        public int Ventilation { get; set;}
        public string Humidity { get; set; }
        public string VendorAngkutanIn { get; set; }
        public string RkemIn { get; set; }
        public bool IsFreeUse { get; set; }

        public string Message { get; set; }
        #endregion

        public ContInOut()
        {
            ContInOutId = 0;
            Cont = string.Empty;
            CustomerCode = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            Berat = string.Empty;
            Manufacture = string.Empty;
            CscPlate = string.Empty;
            DtmIn = string.Empty;
            DtmOut = string.Empty;
            DtmRepaired = string.Empty;
            DtmPti = string.Empty;
            WashStatus = string.Empty;
            Condition = string.Empty;
            ActIn = string.Empty;
            ActOut = string.Empty;
            Location = string.Empty;
            ExVessel = string.Empty;
            Consignee = string.Empty;
            Commodity = string.Empty;
            NoMobilIn = string.Empty;
            AngkutanIn = string.Empty;
            NoMobilOut = string.Empty;
            AngkutanOut = string.Empty;
            DoNumber = string.Empty;
            Shipper = string.Empty;
            Destination = string.Empty;
            Seal = string.Empty;
            VesselVoyage = string.Empty;
            Remarks = string.Empty;
            Payload = string.Empty;
            ExVesselName = string.Empty;
            VesselVoyageName = string.Empty;
            EirInContact = string.Empty;
            DtmEirIn = string.Empty;
            NoSeriOrOut = string.Empty;
            EdiIn = string.Empty;
            EdiOut = string.Empty;
            EdiWash = string.Empty;
            EdiApr = string.Empty;
            EdiCom = string.Empty;
            EdiPti = string.Empty;
            EdiSync = string.Empty;
            EirIn = string.Empty;
            EirOut = string.Empty;
            RfMachine = string.Empty;
            DestinationName = string.Empty;
            BlNumber = string.Empty;
            DtmPortOut = string.Empty;
            DtmOutDepoIn = string.Empty;
            Tare = string.Empty;
            CleaningRefNo = string.Empty;
            CleaningRemark = string.Empty;
            CleaningLastCargo = string.Empty;
            CleaningDtmFinish = string.Empty;
            CleaningCost = 0;
            BookingAssignment = string.Empty;
            CleaningKode = string.Empty;
            CleaningDesc = string.Empty;
            CleaningAction = string.Empty;
            DtmShortPti = string.Empty;
            ExVesselPort = string.Empty;
            RfEngineCond = string.Empty;
            RfDtmEngineRepaired = string.Empty;
            RfEngineEdiIn = string.Empty;
            RfEngineEdiCom = string.Empty;
            RfPtiType = string.Empty;
            RfPtiDtmApproved = string.Empty;
            RfPtiDtmCompleted = string.Empty;
            RfPtiRemark = string.Empty;
            RfPtiCost = 0;
            RfPtiTemp = string.Empty;
            RfNeedSwUpdate = 0;
            RfDtmSwUpdated = string.Empty;
            Grade = string.Empty;
            GradeV2 = string.Empty;
            MddcRemark = string.Empty;
            BusinessUnit = string.Empty;
            Ventilation = 0;
            Humidity = string.Empty;
            VendorAngkutanIn = string.Empty;
            RkemIn = string.Empty;
            IsFreeUse = false;
            Message = string.Empty;
        }
    }
}
