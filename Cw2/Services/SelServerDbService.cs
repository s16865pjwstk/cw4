using Cw2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw2.Services
{
    public class SelServerDbService : IStudentsDbService
    {

        private string conString = "Data Source=db-mssql;Initial Catalog=s16865;Integrated Security=True";

        public EnrollmentResponse enrollment(EnrollmentRequest enrollment)
        {
            
            using (SqlConnection connect = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect;
                connect.Open();
                var tran = connect.BeginTransaction();
                command.Transaction = tran;
                try  
                {

                    // sprawdzam, czy sa studia
                    command.CommandText = "select IdStudy from Studies where Name=@StudiesName";
                    command.Parameters.AddWithValue("StudiesName", enrollment.Studies);
                    SqlDataReader dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return null;
                    }
                    int StudiesId = dr.GetInt32(0);
                    dr.Close();
                    // pobieram najnowszy rekord z tabeli enrollments dla studiów i semestru
                    command.CommandText = "select IdEnrollment from Enrollment where Semester=1 AND IdStudy=@IdStudy";
                    command.Parameters.AddWithValue("IdStudy", StudiesId);
                    dr = command.ExecuteReader();
                    int enrollmentId = 0;
                    if (!dr.Read())
                    {
                        dr.Close();

                        // dodaję wpis w tabeli Enrollment
                        DateTime currentDateTime = DateTime.Now;
                        command.CommandText = "INSERT INTO Enrollment(Semester, IdStudy, StartDate) VALUES(1, @IdStudy, @CurrDateTime)";
                        command.Parameters.AddWithValue("IdStudy", StudiesId);
                        command.Parameters.AddWithValue("CurrDateTime", currentDateTime);
                        command.ExecuteNonQuery();

                        command.CommandText = "select IdEnrollment from Enrollment where Semester=1 AND IdStudy=@IdStudy";
                        command.Parameters.Add("@IdStudy");
                        command.Parameters["@IdStudy"].Value = StudiesId;
                        dr = command.ExecuteReader();
                        dr.Read();
                        enrollmentId = dr.GetInt32(0);
                    } else
                    {
                        enrollmentId = dr.GetInt32(0);
                    }
                    dr.Close();

                    // dodaję wpis w tabeli Enrollment
                    command.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@StudentId, @FirstName, @LastName, @BirtyDay, @EnrollmentId)";
                    command.Parameters.AddWithValue("StudentId", enrollment.IndexNumber);
                    command.Parameters.AddWithValue("FirstName", enrollment.FirstName);
                    command.Parameters.AddWithValue("LastName", enrollment.LastName);
                    command.Parameters.AddWithValue("BirtyDay", Convert.ToDateTime(enrollment.BirthDate));
                    command.Parameters.AddWithValue("EnrollmentId", enrollmentId);
                    command.ExecuteNonQuery();
                    tran.Commit();

                    var enrollmentResponse = new EnrollmentResponse();
                    enrollmentResponse.EnrollmentId = enrollmentId;
                    enrollmentResponse.IndexNumber = enrollment.IndexNumber;
                    enrollmentResponse.FirstName = enrollment.FirstName;
                    enrollmentResponse.LastName = enrollment.LastName;
                    enrollmentResponse.BirthDate = enrollment.BirthDate;

                    return enrollmentResponse;
                } catch (SqlException e)
                {
                    tran.Rollback();
                    return null;
                }

            }
        }

    }
}
