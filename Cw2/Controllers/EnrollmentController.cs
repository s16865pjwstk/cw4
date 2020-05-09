using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw2.Models;
using Cw2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw2.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {


        public IStudentsDbService __service;

        public EnrollmentController(IStudentsDbService service)
        {
            __service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollmentRequest enrollment)
        {

            // sprawdzam poprawność danych
            if (enrollment.IndexNumber == "")
                return BadRequest();
            if (enrollment.FirstName == "")
                return BadRequest();
            if (enrollment.LastName == "")
                return BadRequest();
            if (enrollment.BirthDate == "")
                return BadRequest();
            if (enrollment.Studies == "")
                return BadRequest();

            EnrollmentResponse enrollmentResponse = __service.enrollment(enrollment);
            if (enrollmentResponse == null)
                return BadRequest();

            return CreatedAtAction("enroll", enrollmentResponse);

        }

    }
}