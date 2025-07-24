using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class ItemMasterBO
    {
        private string _AyurCode;
        private Int64 _ID;
        private string _ItemID;
        private string _ItemName;
        
        private Int64 _UnitID;
        private string _UOM;
        private double _QOM;
        private string _LooseUOM;
        private double _LooseQOM;
        private int _CategoryID;
        private string _CategoryName;
        private Int64 _GroupID;
        private string _ItemGroup;
        private Int64 _ManufacturerID;
        private string _ManufacturerName;
        private Int64 _DistributorID;
        private string _DistributorName;
        private string _Description;
        private Int64 _MinLevel;
        private Int64 _MaxLevel;
        private Int64 _ReOrderLevel;
        private double _TaxPer;
        private decimal? _CSTPer;
        private decimal? _CFormPer;
        private double _Discount;
        private Int16 _LooseUnit;
        private double _LooseCF;
        private double _MinLooseQty;
        private double _FullSellingPrice;
        private double _LooseSellingPrice;
        private double _FullPurPrice;
        private double _WholeSalePrice;
        private double _LPurPrice;
        private string _RackNo;
        private string _BarCode;
        private double _MRP;
        private Int64 _TaxID;
        private int _CSTTaxID;
        private int? _CFormTaxID;
        public static Boolean _ViewPrivilege;
        public static Boolean _InsertPrivilege;
        public static Boolean _UpdatePrivilege;
        public static Boolean _DeletePrivilege;
        public static Boolean _CancelPrivilege;
        public static Boolean _PrintPrivilege;
        public static Boolean _BackDateEditPrivilege;
        public static Boolean VIEWPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._ViewPrivilege);
            }
            set
            {
                ItemMasterBO._ViewPrivilege = value;
            }
        }
        public static Boolean INSERTPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._InsertPrivilege);
            }
            set
            {
                ItemMasterBO._InsertPrivilege = value;
            }

        }
        public static Boolean UPDATEPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._UpdatePrivilege);
            }
            set
            {
                ItemMasterBO._UpdatePrivilege = value;
            }

        }
        public static Boolean DELETEPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._DeletePrivilege);
            }
            set
            {
                ItemMasterBO._DeletePrivilege = value;
            }

        }
        public static Boolean CANCELPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._CancelPrivilege);
            }
            set
            {
                ItemMasterBO._CancelPrivilege = value;
            }

        }
        public static Boolean PRINTPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._PrintPrivilege);
            }
            set
            {
                ItemMasterBO._PrintPrivilege = value;
            }

        }

        public static Boolean BACKDATEEDITPRIVILEGE
        {
            get
            {
                return (ItemMasterBO._BackDateEditPrivilege);
            }
            set
            {
                ItemMasterBO._BackDateEditPrivilege = value;
            }

        }
        public Int64 ID
        {
            get
            {
                return (this._ID);
            }
            set
            {
                try
                {
                    this._ID = value;
                    if (this._ID == 0)
                        throw new Exception("Invalid ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string ItemID
        {
            get
            {
                return (this._ItemID);
            }
            set
            {
                try
                {
                    this._ItemID = value;
                    if (this._ItemID == "")
                        throw new Exception("Invalid Item ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string ItemName
        {
            get
            {
                return (this._ItemName);
            }
            set
            {
                try
                {
                    this._ItemName = value;
                    if (this._ItemName == "")
                        throw new Exception("Invalid Item Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public Int64 UnitID
        {
            get
            {
                return (this._UnitID);
            }
            set
            {
                try
                {
                    this._UnitID = value;
                    if (this._UnitID == 0)
                        throw new Exception("Invalid UnitID ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string UOM
        {
            get
            {
                return (this._UOM);
            }
            set
            {
                try
                {
                    this._UOM = value;
                    if (this._UOM == "")
                        throw new Exception("Invalid Unit Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string LooseUOM
        {
            get
            {
                return (this._LooseUOM);
            }
            set
            {
                try
                {
                    this._LooseUOM = value;
                    if (this._LooseUOM == "")
                        throw new Exception("Invalid Unit Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public double QOM
        {
            get
            {
                return (this._QOM);
            }
            set
            {
                try
                {
                    this._QOM = value;
                    if (this._QOM == 0)
                        throw new Exception("Invalid QOM");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public double LooseQOM
        {
            get
            {
                return (this._LooseQOM);
            }
            set
            {
                try
                {
                    this._LooseQOM = value;
                    if (this._LooseQOM == 0)
                        throw new Exception("Invalid QOM");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public int CategoryID
        {
            get
            {
                return (this._CategoryID);
            }
            set
            {
                try
                {
                    this._CategoryID = value;
                    if (this._CategoryID == 0)
                        throw new Exception("Invalid Category ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string CategoryName
        {
            get
            {
                return (this._CategoryName);
            }
            set
            {
                this._CategoryName = value;
                try
                {
                    this._CategoryName = value;
                    if (this._CategoryName == "")
                        throw new Exception("Invalid Category Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public Int64 GroupID
        {
            get
            {
                return (this._GroupID);
            }
            set
            {
                try
                {
                    this._GroupID = value;
                    if (this._GroupID == 0)
                        throw new Exception("Invalid Group ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }

            }
        }
        public string ItemGroup
        {
            get
            {
                return (this._ItemGroup);
            }
            set
            {
                try
                {
                    this._ItemGroup = value;
                    if (this._ItemGroup == "")
                        throw new Exception("Invalid Item Group");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public Int64 ManufacturerID
        {
            get
            {
                return (this._ManufacturerID);
            }
            set
            {
                try
                {
                    this._ManufacturerID = value;
                    if (this._ManufacturerID == 0)
                        throw new Exception("Invalid Manufacturer ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string ManufacturerName
        {
            get
            {
                return (this._ManufacturerName);
            }
            set
            {
                try
                {
                    this._ManufacturerName = value;
                    if (this._ManufacturerName == "")
                        throw new Exception("Invalid _Manufacturer Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public Int64 DistributorID
        {
            get
            {
                return (this._DistributorID);
            }
            set
            {
                try
                {
                    this._DistributorID = value;
                    if (this._DistributorID == 0)
                        throw new Exception("Invalid Distributor ID");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string DistributorName
        {
            get
            {
                return (this._DistributorName);
            }
            set
            {
                try
                {
                    this._DistributorName = value;
                    if (this._DistributorName == "")
                        throw new Exception("Invalid Distributor Name");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public string Description
        {
            get
            {
                return (this._Description);
            }
            set
            {
                this._Description = value;

            }
        }
        public Int64 MinLevel
        {
            get
            {
                return (this._MinLevel);
            }
            set
            {
                this._MinLevel = value;
            }
        }
        public Int64 MaxLevel
        {
            get
            {
                return (this._MaxLevel);
            }
            set
            {
                this._MaxLevel = value;
            }
        }
        public Int64 ReOrderLevel
        {
            get
            {
                return (this._ReOrderLevel);
            }
            set
            {
                this._ReOrderLevel = value;
            }
        }
        public double TaxPer
        {
            get
            {
                return (this._TaxPer);
            }
            set
            {
                this._TaxPer = value;
            }
        }
        public decimal? CFormPer
        {
            get
            {
                return (this._CFormPer);
            }
            set
            {
                this._CFormPer = value;
            }
        }

        public decimal? CSTPer
        {
            get
            {
                return (this._CSTPer);
            }
            set
            {
                this._CSTPer = value;
            }
        }
        public double Discount
        {
            get
            {
                return (this._Discount);
            }
            set
            {
                this._Discount = value;
            }
        }
        public double LooseCF
        {
            get
            {
                return (this._LooseCF);
            }
            set
            {
                try
                {
                    this._LooseCF = value;
                    if (this._LooseCF == 0)
                        throw new Exception("Invalid Loose CF");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public Int16 LooseUnit
        {
            get
            {
                return (this._LooseUnit);
            }
            set
            {
                try
                {
                    this._LooseUnit = value;
                    if (this._LooseUnit == 0)
                        throw new Exception("Invalid Loose Unit");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public double MinLooseQty
        {
            get
            {
                return this._MinLooseQty;
            }
            set
            {
                this._MinLooseQty = value;
            }
        }
        public double FullSellingPrice
        {
            get
            {
                return (this._FullSellingPrice);
            }
            set
            {
                this._FullSellingPrice = value;
            }
        }
        public double LooseSellingPrice
        {
            get
            {
                return (this._LooseSellingPrice);
            }
            set
            {
                this._LooseSellingPrice = value;
            }
        }
        public double FullPurPrice
        {
            get
            {
                return (this._FullPurPrice);
            }
            set
            {
                this._FullPurPrice = value;
            }
        }
        public double WholeSalePrice
        {
            get
            {
                return (this._WholeSalePrice);
            }
            set
            {
                this._WholeSalePrice = value;
            }
        }
        public double LPurPrice
        {
            get
            {
                return (this._LPurPrice);
            }
            set
            {
                this._LPurPrice = value;
            }
        }
        public string AyurCode
        {
            get
            {
                return this._AyurCode;
            }
            set
            {
                this._AyurCode = value;
            }
        }

        public string RackNo
        {
            get
            {
                return (this._RackNo);
            }
            set
            {
                this._RackNo = value;
            }
        }
        public string BarCode
        {
            get
            {
                return (this._BarCode);
            }
            set
            {
                try
                {
                    this._BarCode = value;
                    if (this._BarCode == "")
                        throw new Exception("Invalid BarCode");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public double MRP
        {
            get
            {
                return (this._MRP);
            }
            set
            {
                this._MRP = value;
            }
        }
        public Int64 TaxId
        {
            get
            {
                return (this._TaxID);
            }
            set
            {
                try
                {
                    this._TaxID = value;
                    if (this._TaxID == 0)
                        throw new Exception("Invalid Tax Id");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public int CSTTaxID
        {
            get
            {
                return (this._CSTTaxID);
            }
            set
            {
                try
                {
                    this._CSTTaxID = value;
                    if (this._CSTTaxID == 0)
                        throw new Exception("Invalid Tax Id");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public int? CFormTaxID
        {
            get
            {
                return (this._CFormTaxID);
            }
            set
            {
                try
                {
                    this._CFormTaxID = value;
                    if (this._CFormTaxID == 0)
                        throw new Exception("Invalid Tax Id");
                }
                catch (Exception Er)
                {
                    throw new Exception(Er.Message.ToString());
                }
            }
        }
        public ItemMasterBO()
        {
            _ID = 0;
            _ItemID = "";
            _ItemName = "";
            _UnitID = 0;
            _UOM = "";
            _QOM = 0;
            _CategoryID = 0;
            _CategoryName = "";
            _GroupID = 0;
            _ItemGroup = "";
            _ManufacturerID = 0;
            _ManufacturerName = "";
            _DistributorID = 0;
            _DistributorName = "";
            _Description = "";
            _MinLevel = 0;
            _MaxLevel = 0;
            _ReOrderLevel = 0;
            _TaxPer = 0;
            _CSTPer = 0;
            _CFormPer = 0;
            _Discount = 0;
            _LooseUnit = 0;
            _LooseCF = 0;
            _FullSellingPrice = 0;
            _LooseSellingPrice = 0;
            _FullSellingPrice = 0;
            _WholeSalePrice = 0;
            _LPurPrice = 0;
            _RackNo = "";
            _BarCode = "";
            _MRP = 0;
            _TaxID = 0;
            _CSTTaxID = 0;
            _CFormTaxID = 0;
        }
    }
}

