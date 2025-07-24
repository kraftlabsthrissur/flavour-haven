using System;

namespace BusinessObject
{
    public class DesignationBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
