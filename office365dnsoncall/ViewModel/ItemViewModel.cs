using System;

using System.ComponentModel;

namespace office365dnsoncall.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _name;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _phoneNumber;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Phone
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (value != _phoneNumber)
                {
                    _phoneNumber = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }

        private string _emailAddress;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Email
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                if (value != _emailAddress)
                {
                    _emailAddress = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }


        //private int _daysLeftOnCall;
        //public string DaysLeftOnCall
        //{
        //    get
        //    {
        //        return _daysLeftOnCall.ToString();
        //    }
        //    set
        //    {
        //        if (int.Parse(value) != _daysLeftOnCall)
        //        {
        //            _daysLeftOnCall = int.Parse(value);
        //            NotifyPropertyChanged("DaysLeftOnCall");
        //        }
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}