using DatosRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatosRH.DAO
{
    public class UsuarioDao : DB
    {
        string query = "";
        public List<Usuario> GetAll()
        {
            List<Usuario> list = new List<Usuario>();
            using (var cnn = ConexionRemoto())
            {
                cnn.Open();
                query = "SELECT * FROM usuarios;";
                using (var cmd = new MySqlCommand(query,cnn))
                {
                    using(var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Usuario
                            {
                                Id = rd.GetInt32(0),
                                Username = rd.GetString(1),
                                Pass = rd.GetString(2),
                                Nombre = rd.GetString(3),
                            });
                        }
                    }
                }
                cnn.Close();
            }
            return list;
        }
        public int Add(Usuario usuario)
        {
            int result = 0;
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                query = "INSERT INTO usuarios (nombre,username,pass) VALUES(?nombre,?username,?pass);" +
                        "SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("?username", usuario.Username);
                    cmd.Parameters.AddWithValue("?pass", usuario.Pass);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    
                }
                cnn.Close();
            }
            return result;
        }
        public int Update(Usuario usuario)
        {
            int result = 0;
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                query = "UPDATE usuarios SET nombre=?,username=?,pass=? WHERE id=?;";
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("?username", usuario.Username);
                    cmd.Parameters.AddWithValue("?pass", usuario.Pass);
                    cmd.Parameters.AddWithValue("?pass", usuario.Id);
                    result = cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            return result;
        }
        public int Remove(int id)
        {
            int result = 0;
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                query = "DELETE FROM usuarios WHERE id=?;";
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?id", id);
                    result = cmd.ExecuteNonQuery();

                }
                cnn.Close();
            }
            return result;
        }
    }
}
