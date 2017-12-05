using System;
using System.Collections;

namespace ITI.Survey.Web.Dll.Helper
{
    [Serializable]
    public class ActInCollection : ArrayList
    {
        private static ActInCollection _actInCollection;

        public static ActInCollection Value
        {
            get
            {
                if (_actInCollection == null)
                {
                    _actInCollection = new ActInCollection();
                    _actInCollection.Add("IM");
                    _actInCollection.Add("OB");
                    _actInCollection.Add("ON");
                    _actInCollection.Add("TP");
                    _actInCollection.Add("SL");
                    _actInCollection.Add("CFS");
                }
                return _actInCollection;
            }
        }
    }
}
