using DatosRH.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosRH.DAO
{
    public class ChecadasDAO:DB
    {
        private string query = "";
        public List<Checada> GetAll()
        {
            query = "SELECT * FROM registros ORDER BY empleado;";
            List<Checada> list = new List<Checada>();

            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Checada()
                            {
                                Id = rd.GetInt32(0),
                                FechaHora = DBNull.Value.Equals(rd["hora"]) ? Convert.ToDateTime(null) : rd.GetDateTime(1),
                                Empleado = DBNull.Value.Equals(rd["empleado"]) ? 0 : rd.GetInt32(2),
                                //Usuario = DBNull.Value.Equals(rd["usuario"]) ? 0 : rd.GetInt32(3),
                                //Dispositivo = DBNull.Value.Equals(rd["dispositivo"]) ? "" : rd.GetString(4)
                            });
                        }
                    }
                }
            }
            return list;
        }

        public int Add(Checada checada)
        {
            int result = 0;
            query = "INSERT INTO registros(hora,empleado)" +
                    "VALUES(?hora,?empleado);" +
                    "SELECT LAST_INSERT_ID()";
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?hora", checada.FechaHora);
                    cmd.Parameters.AddWithValue("?empleado", checada.Empleado);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }

            return result;
        }
    }
}
