using System.Collections.Generic;

namespace ITI.Survey.Web.Dll.Helper
{
    public static class ContainerMapping
    {
        #region Fields
        private static Dictionary<string, string> _containerDictionary;
        #endregion

        #region Properties
        public static Dictionary<string, string> ContainerDictionary
        {
            get
            {
                if (_containerDictionary == null)
                {
                    _containerDictionary = new Dictionary<string, string>();
                    // Key: Customer Type; Value: MIT Type
                    _containerDictionary.Add("RQ", "RH");
                }
                return _containerDictionary;
            }
        }
        #endregion
    }
}
