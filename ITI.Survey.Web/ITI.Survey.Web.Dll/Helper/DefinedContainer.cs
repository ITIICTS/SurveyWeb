using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITI.Survey.Web.Dll.Helper
{
    [Serializable]
    public class DefinedContainer 
    {
        public List<DefinedContainerItem> ListDefinedContainerItem { get; set; }

        #region Item Class
        [Serializable()]
        public class DefinedContainerItem
        {
            private string _container = string.Empty;
            private string _size = string.Empty;
            private string _type = string.Empty;
            private string _dtmIn = string.Empty;
            private string _dtmOut = string.Empty;
            private string _duration = string.Empty;
            private string _customerCode = string.Empty;
            private string _condition = string.Empty;
            private string _specialMessage = string.Empty;

            private bool _checked = true;

            #region property

            public bool Checked
            {
                get { return _checked; }
                set { _checked = value; }
            }

            public string CustomerCode
            {
                get
                {
                    return _customerCode;
                }
            }

            public string Container
            {
                get
                {
                    return _container;
                }
            }
            public string DtmIn
            {
                get
                {
                    return _dtmIn;
                }
            }
            public string DtmOut
            {
                get
                {
                    return _dtmOut;
                }
            }
            public string Duration
            {
                get
                {
                    return _duration;
                }
            }
            public string Size
            {
                get
                {
                    return _size;
                }
            }
            public string Type
            {
                get
                {
                    return _type;
                }
            }
            public string Condition
            {
                get
                {
                    return _condition;
                }
            }

            public string SpecialMessage
            {
                get
                {
                    return _specialMessage;
                }
            }

            #endregion

            #region Constructor
            public DefinedContainerItem(string container)
            {
                ContInOutDAL contInOutDAL = new ContInOutDAL();
                BlackListDAL blackListDAL = new BlackListDAL();

                container = container.Trim();
                container = container.Replace(GlobalConstant.STRING_SPACE, string.Empty);
                if (container.Length != 11) container = "ERRORLENGTH";
                string prefix = container.Substring(0, 4);
                string num1 = container.Substring(4, 3);
                string num2 = container.Substring(7, 3);
                string num3 = container.Substring(10, 1);
                _container = prefix + " " + num1 + " " + num2 + " " + num3;

                try
                {
                    ContInOut contInOut = new ContInOut();
                    contInOut = contInOutDAL.FillContInOutByContainerNumber(_container);
                    if (contInOut.ContInOutId <= 0)
                    {
                        return;
                    }
                    _size = contInOut.Size;
                    _type = contInOut.Type;
                    _dtmIn = contInOut.DtmIn;
                    _dtmOut = contInOut.DtmOut;
                    _customerCode = contInOut.CustomerCode;
                    _condition = contInOut.Condition;
                    if (_dtmOut.Length == 0)
                    {
                        DateTime start = DateTime.Parse(_dtmIn);
                        DateTime end = GlobalWebServiceDAL.GetServerDtm();
                        TimeSpan ts = end.Subtract(start);
                        int days = 1 + ts.Days;
                        _duration = days.ToString();
                    }
                    else
                    {
                        DateTime start = DateTime.Parse(_dtmIn);
                        DateTime end = DateTime.Parse(_dtmOut);
                        TimeSpan ts = end.Subtract(start);
                        int days = 1 + ts.Days;
                        _duration = days.ToString();
                    }

                    List<BlackList> listBlackList = blackListDAL.GetBlackListByContainerNumber(contInOut.Cont);
                    string message = string.Empty;
                    DateTime n = GlobalWebServiceDAL.GetServerDtm();
                    foreach (BlackList blackList in listBlackList)
                    {
                        if (blackList.Disabled) continue;
                        if (blackList.DisabledUntil >= n) continue;
                        if (message.Length > 0) message += " ";
                        message += blackList.Message;
                    }
                    this._specialMessage = message;

                }
                catch (Exception)
                {
                    //ignore error
                }
            }
            #endregion
        }
        #endregion

        #region constructor
        public DefinedContainer(string input)
        {
            this.FromString(input);
        }
        public DefinedContainer()
        {
        }
        #endregion

        #region method

        public string SizeCountInfo()
        {

            int cnt20 = 0;
            int cnt40 = 0;
            int cnt45 = 0;

            foreach (DefinedContainerItem item in ListDefinedContainerItem)
            {
                switch (item.Size)
                {
                    case "20":
                        cnt20++;
                        break;
                    case "40":
                        cnt20++;
                        break;
                    case "45":
                        cnt20++;
                        break;

                }
            }

            return " 20'=" + cnt20 + " 40'=" + cnt40 + " 45'=" + cnt45;
        }

        public void FromString(string input)
        {            
            string[] split = input.Split(",".ToCharArray());
            foreach (string item in split)
            {
                string item1 = item.Trim();
                item1 = item1.Replace(GlobalConstant.STRING_SPACE, string.Empty);
                if (item1.Length != 11)
                {
                    continue;   //protect length
                }
                DefinedContainerItem definedContainerItem = new DefinedContainerItem(item1);
                if (this.IsExist(definedContainerItem.Container))
                {
                    continue;
                }
                ListDefinedContainerItem.Add(definedContainerItem);
            }
            this.DoCount();
            this.OnContChanged();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            for (int i = 0; i < ListDefinedContainerItem.Count; i++)
            {
                DefinedContainerItem definedContainerItem = ListDefinedContainerItem[i];
                stringBuilder.Append(definedContainerItem.Container);
                if (i != ListDefinedContainerItem.Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            return stringBuilder.ToString();
        }

        public void RemoveUnchecked()
        {
            foreach (DefinedContainerItem item in ListDefinedContainerItem)
            {
                if (!item.Checked)
                {
                    ListDefinedContainerItem.Remove(item);
                }
            }
        }
        public bool AdakahYangOut()
        {
            foreach (DefinedContainerItem item in ListDefinedContainerItem)
            {
                if (item.DtmOut.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AdakahYangLainCustomer(string cus)
        {
            foreach (DefinedContainerItem item in ListDefinedContainerItem)
            {
                if (item.CustomerCode != cus) return true;
            }
            return false;
        }

        public bool IsExist(string cont)
        {
            foreach (DefinedContainerItem i in ListDefinedContainerItem)
            {
                if (cont == i.Container) return true;
            }
            return false;
        }

        #endregion

        #region Count
        public int Count20 = 0;
        public int Count40 = 0;
        public int Count45 = 0;
        public int CountUnknown = 0;

        public void DoCount()
        {
            Count20 = 0;
            Count40 = 0;
            Count45 = 0;
            CountUnknown = 0;
            foreach (DefinedContainerItem item in ListDefinedContainerItem)
            {
                switch (item.Size)
                {
                    case "20":
                        Count20++;
                        break;
                    case "40":
                        Count40++;
                        break;
                    case "45":
                        Count45++;
                        break;
                    default:
                        CountUnknown++;
                        break;
                }
            }
        }

        public string GetTypeCount20()
        {
            TypeCountList t = new TypeCountList(this, "20");
            return t.ToString();
        }
        public string GetTypeCount40()
        {
            TypeCountList t = new TypeCountList(this, "40");
            return t.ToString();
        }
        public string GetTypeCount45()
        {
            TypeCountList t = new TypeCountList(this, "45");
            return t.ToString();
        }

        #endregion

        #region TypeCount
        public class TypeCount
        {
            public string mType = string.Empty;
            public int mCount = 0;
        }

        public class TypeCountList : ArrayList
        {

            public TypeCount FindByType(string type)
            {
                foreach (TypeCount t in this)
                {
                    if (t.mType == type) return t;
                }
                TypeCount obj = new TypeCount();
                obj.mType = type;
                Add(obj);
                return obj;
            }

            public TypeCountList(DefinedContainer definedContainer, string size)
            {
                foreach (DefinedContainer.DefinedContainerItem item in definedContainer.ListDefinedContainerItem)
                {
                    if (item.Size != size) continue;
                    this.FindByType(item.Type).mCount++;
                }
            }
            public override string ToString()
            {
                string result = string.Empty;
                foreach (TypeCount t in this) result += "," + t.mCount + t.mType;
                if (result.Length > 0) result = result.Substring(1);
                return result;
            }
        }

        #endregion

        #region Event		

        public event EventHandler ContChanged = null;
        protected void OnContChanged()
        {
            if (ContChanged == null) return;
            ContChanged(this, new EventArgs());
        }
        #endregion
    }
}
