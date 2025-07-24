using System;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Models
{
    public class TransactionFormModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }

        public string TransDateString { get; set; }
        private DateTime? _TransDate;
        public DateTime? TransDate
        {
            get
            {
                if (TransDateString != null)
                {
                    _TransDate = General.ToDateTimeNull(TransDateString);
                }
                return _TransDate;
            }
            set
            {
                _TransDate = value;
            }
        }
        private string _TransDateStringEdit;
        public string TransDateStringEdit
        {
            get
            {
                if (TransDate != null)
                {
                    _TransDateStringEdit = General.FormatDateNull(TransDate);
                }
                return _TransDateStringEdit;
            }
            set { _TransDateStringEdit = value; }
        }
        public string TransDateStringView
        {
            get
            {
                return General.FormatDateNull(TransDate, "view");
            }
        }

        public string FromDateString { get; set; }
        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get
            {
                if(FromDateString != null)
                {
                    _FromDate = General.ToDateTimeNull(FromDateString);
                }
                return _FromDate;
            }
            set {
                _FromDate = value;
            }
        }
        private string _FromDateStringEdit;
        public string FromDateStringEdit
        {
            get
            {
                if (FromDate != null)
                {
                    _FromDateStringEdit = General.FormatDateNull(FromDate);
                }
                return _FromDateStringEdit;
            }
            set { _FromDateStringEdit = value; }
        }
        public string FromDateStringView
        {
            get
            {
                return General.FormatDateNull(FromDate, "view");
            }
        }

        public string ToDateString { get; set; }
        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get
            {
                if (ToDateString != null)
                {
                    _ToDate = General.ToDateTimeNull(ToDateString);
                }
                return _ToDate;
            }
            set
            {
                _ToDate = value;
            }
        }
        private string _ToDateStringEdit;
        public string ToDateStringEdit
        {
            get
            {
                if (ToDate != null)
                {
                    _ToDateStringEdit = General.FormatDateNull(ToDate);
                }
                return _ToDateStringEdit;
            }
            set { _ToDateStringEdit = value; }
        }
        public string ToDateStringView
        {
            get
            {
                return General.FormatDateNull(ToDate, "view");
            }
        }

        public string DateString { get; set; }
        private DateTime? _Date;
        public DateTime? Date
        {
            get
            {
                if (DateString != null)
                {
                    _Date = General.ToDateTimeNull(DateString);
                }
                return _Date;
            }
            set
            {
                _Date = value;
            }
        }
        private string _DateStringEdit;
        public string DateStringEdit
        {
            get
            {
                if (Date != null)
                {
                    _DateStringEdit = General.FormatDateNull(Date);
                }
                return _DateStringEdit;
            }
            set { _DateStringEdit = value; }
        }
        public string DateStringView
        {
            get
            {
                return General.FormatDateNull(Date, "view");
            }
        }

        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsProcessed { get; set; }

    }


}