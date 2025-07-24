using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var FinYear = identity.Claims.FirstOrDefault(c => c.Type == "FinYear").Value;
            var F = GeneralBO.FinYear.ToString();
            return new string[] { "value1", System.Web.HttpContext.Current.User.Identity.Name, FinYear, F };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
