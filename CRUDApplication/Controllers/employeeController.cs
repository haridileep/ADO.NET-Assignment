using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRUDApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        public string connectionString;
        public EmployeeController()
            {
            connectionString = "Server=localhost; Database=employee; Trusted_Connection=True; MultipleActiveResultSets=True;";
            }
        [HttpGet]
        public ActionResult EmployeeDisplay()
            {
            List<EmployeeModel> employeedetails = new List<EmployeeModel>();
            string query = "Select * from employeedata";
            SqlCommand command = new SqlCommand(query);
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                connection.Open();
                command.Connection = connection;
                SqlDataReader data = command.ExecuteReader();
                if (data.HasRows)
                    {
                    while (data.Read())
                        {
                        EmployeeModel employee = new EmployeeModel();
                        employee.Id = data["id"].ToString();
                        employee.Name = data["name"].ToString();
                        employee.Salary = data["salary"].ToString();
                        employee.Email = data["email"].ToString();
                        employee.Age = data["age"].ToString();
                        employeedetails.Add(employee);
                        }   
                    }
                }
                return Ok(employeedetails);
            }
        [HttpPost]
        public ActionResult Insert([FromBody]EmployeeModel model)
            {
            string query = "insert into employeedata values(@id,@name,@salary,@email,@age)";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", model.Id));
            command.Parameters.Add(new SqlParameter("@name", model.Name));
            command.Parameters.Add(new SqlParameter("@salary", model.Salary));
            command.Parameters.Add(new SqlParameter("@email", model.Email));
            command.Parameters.Add(new SqlParameter("@age", model.Age));
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
                }
            return Ok("Row inserted");
            }
        [HttpDelete]
        public ActionResult Delete([FromBody]EmployeeModel model)
            {
            string query = "delete employeedata where id=@id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", model.Id));
            using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                 connection.Open();
                 command.Connection = connection;
                 command.ExecuteNonQuery();
                 }
            return Ok("Row deleted");
            }
        [HttpPut]
        public ActionResult Update([FromBody]EmployeeModel model)
            {
            using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                 connection.Open();
                 SqlCommand command = new SqlCommand("dbo.updatesalary", connection);
                 command.CommandType = CommandType.StoredProcedure;
                 command.Parameters.Add(new SqlParameter("@salary", model.Salary));
                 command.Parameters.Add(new SqlParameter("@id", model.Id));   
                 command.ExecuteScalar();
                 }
            return Ok("One row is updated");
            }

    }
}
