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
    }
}
