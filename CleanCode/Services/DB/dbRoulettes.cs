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
                string query = "insert into Bets(Number,Color,Amount,IdClient.IdRoulette)" +
                               "values(@Number,@Color,@Amount,@IdClient,@IdRoulette)" +
                               "select SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Number", (object)bet.Number ?? DBNull.Value);
                command.Parameters.AddWithValue("@Color", (object)bet.Color ?? DBNull.Value);
                command.Parameters.AddWithValue("@Amount", bet.Amount);
                command.Parameters.AddWithValue("@IdClient", bet.IdClient);
                command.Parameters.AddWithValue("@IdRoulette", bet.IdRoulette);
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

        public List<BetRoulette> ListBets(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select * from Bets where IdRoulette=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                List<BetRoulette> Bets = new List<BetRoulette>();
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var Bet = new BetRoulette();
                        Bet.IdBet = reader.GetInt32(0);
                        if (!reader.IsDBNull(1)) Bet.Number = reader.GetInt32(1);
                        if (!reader.IsDBNull(2)) Bet.Color = reader.GetString(2);
                        Bet.Amount = reader.GetDouble(3);
                        Bet.IdClient = reader.GetInt32(4);
                        Bet.IdRoulette = id;
                        Bets.Add(Bet);
                    }                    
                    reader.Close();
                    connection.Close();
                    return Bets;
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error en la BD: " + ex.Message);
                }
            }
        }

        public void UpdateRoulette(ResponseBets model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "update Roulettes set WinNumber=@WinNumber where IdRoulette=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@WinNumber", model.WinNumber);
                command.Parameters.AddWithValue("@id", model.IdRoulette);
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

        public List<Roulette> GetRoulettes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select * from Roulettes";
                SqlCommand command = new SqlCommand(query, connection);
                List<Roulette> Roulettes = new List<Roulette>();
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var roulette = new Roulette();
                        roulette.IdRoulette = reader.GetInt32(0);
                        if (!reader.IsDBNull(1)) roulette.WinNumber = reader.GetInt32(1);
                        Roulettes.Add(roulette);
                    }
                    reader.Close();
                    connection.Close();
                    return Roulettes;
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error en la BD: " + ex.Message);
                }
            }
        }
    }
}
