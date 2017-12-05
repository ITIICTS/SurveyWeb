using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Helper
{
    [Serializable]
    public class ContainerSpecification : ArrayList
    {
        #region Item Class
        [Serializable()]
        public class ContSpecItem
        {
            internal int _containerCount = 0;
            internal string _containerType = string.Empty;
            public bool _isError = true;

            public string ContainerType
            {
                get { return _containerType; }
            }
            public int ContainerCount
            {
                get { return _containerCount; }
            }

            public ContSpecItem() { }

            public ContSpecItem(string input)
            {
                input = input.Trim();
                input = input.Replace(GlobalConstant.STRING_SPACE, string.Empty);
                if (input.Length < 3)
                {
                    return;
                }
                string s1 = input.Substring(0, input.Length - 2);
                string s2 = input.Substring(input.Length - 2, 2);

                try
                {
                    _containerCount = Convert.ToInt32(s1);
                }
                catch (Exception)
                {
                    return;
                }
                _containerType = s2;
                if (!ContTypeList.Value.Contains(_containerType)) return;
                _isError = false;
            }

            public override string ToString()
            {
                return _containerCount.ToString() + _containerType;
            }


        }
        #endregion

        #region Constructor
        public ContainerSpecification(string input)
        {
            FromString(input);
        }
        public ContainerSpecification()
        {
        }
        #endregion

        #region Event
        protected void OnContChanged()
        {
            if (ContChanged == null) return;
            ContChanged(this, new EventArgs());
        }
        #endregion

        #region Method

        public ContSpecItem FindByTypeNullIfNotFound(string type)
        {
            foreach (ContSpecItem item in this)
            {
                if (item.ContainerType == type) return item;
            }
            return null;
        }

        public ContSpecItem FindByType(string type)
        {
            foreach (ContSpecItem item in this)
            {
                if (item.ContainerType == type) return item;
            }
            return new ContSpecItem();
        }

        public int ContainerTotalCount
        {
            get
            {
                int i = 0;
                foreach (ContSpecItem item in this)
                {
                    i += item.ContainerCount;
                }
                return i;
            }
        }
        public int ReeferTotalCount
        {
            get
            {
                int i = 0;
                foreach (ContSpecItem item in this)
                {
                    if (item.ContainerType == "RF" || item.ContainerType == "RH")
                        i += item.ContainerCount;
                }
                return i;
            }
        }


        public void FromString(string input)
        {
            Clear();

            foreach (var item in ContainerMapping.ContainerDictionary)
            {
                if (input.Contains(item.Key))
                {
                    input = input.Replace(item.Key, item.Value);
                }
            }

            string[] split = input.Split(",".ToCharArray());
            foreach (string item in split)
            {
                ContSpecItem obj = new ContSpecItem(item);
                if (!obj._isError)
                {
                    ContSpecItem i1 = this.FindByTypeNullIfNotFound(obj.ContainerType);
                    if (i1 == null) this.Add(obj);
                    else i1._containerCount += obj._containerCount;
                }
            }
            OnContChanged();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            for (int i = 0; i < this.Count; i++)
            {
                ContSpecItem contSpecItem = this[i] as ContSpecItem;
                stringBuilder.Append(contSpecItem.ToString());
                if (i != Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            return stringBuilder.ToString();
        }

        #endregion

        public event EventHandler ContChanged = null;
    }    
}
