using BusinessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Areas.Sales.Controllers
{
    [Route("Sales/CounterSalesController")]
    public class CounterSalesController : ApiController
    {
        private IEmployeeContract employeeBL;
        private ICounterSalesContract counterSalesBL;
        private IBatchContract batchBL;

        public CounterSalesController()
        {
            employeeBL = new EmployeeBL();
            counterSalesBL = new CounterSalesBL();
            batchBL = new BatchBL();
        }

        //[HttpGet]
        //[Route("GetDoctorsList")]
        //public IEnumerable<EmployeeModel> GetDoctorsList()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    employeeList = employeeBL.GetEmployeeList().Select(a => new EmployeeModel()
        //    {
        //        ID = a.ID,
        //        Code = a.Code,
        //        Name = a.Name,
        //        Place = a.Place

        //    }).ToList();
        //    return (employeeList);
        //}


        //[HttpGet]
        //[Route("GetDoctorsList")]
        //public HttpResponseMessage GetDoctorsList()
        //{
        //    try
        //    {
        //        List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        //employeeList = employeeBL.GetEmployeeList().Select(a => new EmployeeModel()
        //        //{
        //        //    ID = a.ID,
        //        //    Code = a.Code,
        //        //    Name = a.Name,
        //        //    Place = a.Place

        //        //}).ToList();
        //        //return new { Status = "failure", data = employeeList, Message = "Success" };
        //        return Request.CreateResponse(HttpStatusCode.OK, employeeList);
        //    }
        //    catch (Exception e)
        //    {
        //        //return new { Status = "failure", data = "", Message = "Message" });
        //          return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not Found");
        //    }
            
            
        //}

    }
}
