using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using hamzacrud.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Reflection;
using System.Data.Common;
using System.Configuration;
using Humanizer.Configuration;
using CRUD_ADO.NET.Models;

namespace CRUD_ADO.NET.Services
{
    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;
            int userId = 0;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("Sp_login", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    SqlParameter isAuthenticatedParam = new SqlParameter("@IsAuthenticated", SqlDbType.Bit);
                    isAuthenticatedParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(isAuthenticatedParam);
                    command.ExecuteNonQuery();
                    isAuthenticated = Convert.ToBoolean(isAuthenticatedParam.Value);
                }
            }
            return isAuthenticated;
        }
        public bool CreateAccount(UserLogin login)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_Signin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@Password", login.Password);
                    cmd.Parameters.AddWithValue("@Age", login.Age);
                    cmd.Parameters.AddWithValue("@Gender", login.Gender);
                    con.Open();
                    int r = cmd.ExecuteNonQuery();
                    return r > 0;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
        public List<UserModel> GetList()
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employee_Select", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    UserModel user = new UserModel();
                    user.First_Name = Convert.ToString(dr["FirstName"]);
                    user.Last_Name = Convert.ToString(dr["LastName"]);
                    user.Age = Convert.ToInt32(dr["Age"]);
                    user.Gender = Convert.ToString(dr["Gender"]);
                    user.Id = Convert.ToInt32(dr["id"]);
                    users.Add(user);
                }
            }
            return users;
        }
        [HttpPost]
        public bool Create(UserModel model)
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employe_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", model.First_Name);
                cmd.Parameters.AddWithValue("@LastName", model.Last_Name);
                cmd.Parameters.AddWithValue("@Age", model.Age);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                return r > 0;
            }
        }
        public bool Update(UserModel model)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employe_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", model.First_Name);
                cmd.Parameters.AddWithValue("@LastName", model.Last_Name);
                cmd.Parameters.AddWithValue("@Age", model.Age);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                return r > 0;
            }
        }
        public UserModel GetDetails(int id)
        {
            UserModel model = new UserModel();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter adapterr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapterr.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    model.First_Name = Convert.ToString(dr["FirstName"]);
                    model.Last_Name = Convert.ToString(dr["LastName"]);
                    model.Age = Convert.ToInt32(dr["Age"]);
                    model.Gender = Convert.ToString(dr["Gender"]);
                }
            }
            return model;
        }
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("Sp_Employee_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}



