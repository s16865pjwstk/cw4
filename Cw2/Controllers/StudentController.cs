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

    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        
        
       private string conString = "Data Source=db-mssql;Initial Catalog=s16865;Integrated Security=True";


        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id)
        {
            return Ok("Aktualizacja ukończona");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }


        [HttpGet]
        public IActionResult GetStudents (string orderBy)
        {
            var list = new List<StudentInfoDTO>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while(dr.Read())
                {
                    var student = new StudentInfoDTO()
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



        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Malewski");
            }
            return NotFound("Nie znaleziono studenta!");
        }
            
    }
}