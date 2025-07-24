using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ConfigBO
    {
        public string Name { get; set; }
        public string StringValue;
        public string Description { get; set; }

        public int? Value
        {
            get
            {
                try {
                    return Convert.ToInt32(StringValue);
                }
                catch (Exception e) {
                    return null;
                }
            }
        }

        public bool? BoolValue
        {
            get
            {
                try
                {
                    return StringValue == "" ? false : StringValue.Trim() == "" ? false : Convert.ToBoolean(StringValue.Trim());
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public decimal? DecimalValue
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(StringValue);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
