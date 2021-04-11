using CleanCode.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Services.DB
{
    public class dbRoulettes: IdbRoulettes
    {
        private readonly string connectionString;

        public dbRoulettes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int CreateRoulette()
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {   
                string query = "insert into Roulettes(WinNumber) values(null) select SCOPE_IDENTITY()";
                int id;
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    id = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return id;
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error en la BD: " + ex.Message);
                }
            }                     
        }

        public void CreateBet(BetRoulette bet)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "insert into Bets(Number,Color,Amount,IdClient)" +
                               "values(@Number,@Color,@Amount,@IdClient)" +
                               "select SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Number", (object)bet.Number ?? DBNull.Value);
                command.Parameters.AddWithValue("@Color", (object)bet.Color ?? DBNull.Value);
                command.Parameters.AddWithValue("@Amount", bet.Amount);
                command.Parameters.AddWithValue("@IdClient", bet.IdClient);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error en la BD: " + ex.Message);
                }
            }
        }

        public bool VerifyStatus(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select WinNumber from Roulettes where IdRoulette=@id";
                bool status = false;
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.IsDBNull(0)) status = true;                        
                    reader.Close();
                    connection.Close();
                    return status;
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error en la BD: " + ex.Message);
                }
            }
        }
    }
}
