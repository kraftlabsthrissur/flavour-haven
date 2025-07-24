using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Models
{
    public class DatatableModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<ColumnModel> Columns { get; set; }
        public SearchModel Search { get; set; }
        public List<OrderModel> Order { get; set; }
        public List<KeyValue> Params { get; set; }

        public string GetValueFromKey(string Key, List<KeyValue> Params)
        {
            return  Params.Where(d => d.Key == Key).Select(d => d.Value).FirstOrDefault();
        }

    }

    public class KeyValue {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ColumnModel
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchModel Search { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
    public class SearchModel
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }
    public class OrderModel
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }
    public class DatatableResult
    {
        public int draw { get; set; }
        public List<object> data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }

}