using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw2.Controllers
{
    [Route("api/studentssemesters")]
    [ApiController]
    public class StudentssemestersController : ControllerBase
    {

        private string conString = "Data Source=db-mssql;Initial Catalog=s16865;Integrated Security=True";

        [HttpGet("{id}")]
        public IActionResult GetStudentsemesters(string id)
        {
            var list = new List<StudentInfoDTO>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = $"select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy where s.IndexNumber = {id}";
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var student = new StudentInfoDTO
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = dr["BirthDate"].ToString(),
                        Name = dr["Name"].ToString(),
                        Semester = dr["Semester"].ToString()
                    };
                    list.Add(student);
                }
            }
            return Ok(list);
        }

    }
}