using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace DatosRH.DAO
{
    public class PersonalDAO:DB
    {
        private string query = "";
        public List<Personal> GetAll()
        {
            query = "SELECT * FROM personal ORDER BY num;";

            List<Personal> list = new List<Personal>();

            using (var cnn = ConexionRemoto())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Personal()
                            {
                                Id = rd.GetInt32(0),
                                Num = (DBNull.Value.Equals(rd["num"])) ? 0 : rd.GetInt32(1),
                                Nombre = (DBNull.Value.Equals(rd["nombre"])) ? "" : rd.GetString(2),
                                Apellidos = (DBNull.Value.Equals(rd["apellidos"])) ? "" : rd.GetString(3),
                                //Telefono = (DBNull.Value.Equals(rd["telefono"])) ? "" : rd.GetString(3),
                                //Email = (DBNull.Value.Equals(rd["email"])) ? "" : rd.GetString(4),
                                //Area = (DBNull.Value.Equals(rd["area"])) ? 0 : rd.GetInt32(5),
                                //Calle = (DBNull.Value.Equals(rd["calle"])) ? "" : rd.GetString(6),
                                //Numero = (DBNull.Value.Equals(rd["numero"])) ? 0 : rd.GetInt32(7),
                                //NumInt = (DBNull.Value.Equals(rd["numInt"])) ? "" : rd.GetString(8),
                                //Fracc = (DBNull.Value.Equals(rd["fracc"])) ? "" : rd.GetString(9),
                                //CP = (DBNull.Value.Equals(rd["cp"])) ? 0 : rd.GetInt32(10),
                                //RFC = (DBNull.Value.Equals(rd["rfc"])) ? "" : rd.GetString(11),
                                //CURP = (DBNull.Value.Equals(rd["curp"])) ? "" : rd.GetString(12),
                                //FechaIng = (DBNull.Value.Equals(rd["fechanIng"])) ? Convert.ToDateTime(null) : rd.GetDateTime(13),
                                //Puesto = (DBNull.Value.Equals(rd["puesto"])) ? "" : rd.GetString(14),
                                //Turno = (DBNull.Value.Equals(rd["turno"])) ? 0 : rd.GetInt32(15),
                                //Contrato = (DBNull.Value.Equals(rd["contrato"])) ? "" : rd.GetString(16),
                                //TurnoOpcional = (DBNull.Value.Equals(rd["turnoOpcional"])) ? "" : rd.GetString(17),
                                //DenomPuesto = (DBNull.Value.Equals(rd["denomPuesto"])) ? 0 : rd.GetInt32(18),
                                Huella = (DBNull.Value.Equals(rd["huella"])) ? (byte[])(null) : (byte[])(rd["huella"]),
                                Status = rd["huella"].ToString()
                                //Pass = (DBNull.Value.Equals(rd["pass"])) ? "" : rd.GetString(21),
                                //Servicio = (DBNull.Value.Equals(rd["servicio"])) ? "" : rd.GetString(22),
                                //Cedula = (DBNull.Value.Equals(rd["cedula"])) ? "" : rd.GetString(23)

                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<Personal> GetActivos()
        {
            return GetAll().Select(x => x).Where(x => x.Status == "Y").ToList();
        }

        public int Add(Personal persona)
        {
            int result = 0;
            query = "INSERT INTO personal(num,nombre,apellidos,telefono,email,area,calle,numero,numInt,fracc,cp,rfc,curp,fechaIng,puesto,turno,contrato,turnoOpcional,denomPuesto,huella,servicio,cedula)" +
                    "VALUES(?num,?nombre,?apellidos,?telefono,?email,?area,?calle,?numero,?numInt,?fracc,?cp,?rfc,?curp,?fechaIng,?puesto,?turno,?contrato,?turnoOpcional,?denomPuesto,?huella,?servicio,?cedula);" +
                    "SELECT LAST_INSERT_ID()";
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?num", persona.Num);
                    cmd.Parameters.AddWithValue("?nombre", persona.Nombre);
                    cmd.Parameters.AddWithValue("?apellidos", persona.Apellidos);
                    cmd.Parameters.AddWithValue("?telefono", persona.Telefono);
                    cmd.Parameters.AddWithValue("?email", persona.Email);
                    cmd.Parameters.AddWithValue("?area", persona.Area);
                    cmd.Parameters.AddWithValue("?calle", persona.Calle);
                    cmd.Parameters.AddWithValue("?numero", persona.Numero);
                    cmd.Parameters.AddWithValue("?numInt", persona.NumInt);
                    cmd.Parameters.AddWithValue("?fracc", persona.Fracc);
                    cmd.Parameters.AddWithValue("?cp", persona.CP);
                    cmd.Parameters.AddWithValue("?rfc", persona.RFC);
                    cmd.Parameters.AddWithValue("?curp", persona.CURP);
                    cmd.Parameters.AddWithValue("?fechaIng", persona.FechaIng);
                    cmd.Parameters.AddWithValue("?puesto", persona.Puesto);
                    cmd.Parameters.AddWithValue("?turno", persona.Turno);
                    cmd.Parameters.AddWithValue("?contrato", persona.Contrato);
                    cmd.Parameters.AddWithValue("?turnoOpcional", persona.TurnoOpcional);
                    cmd.Parameters.AddWithValue("?denomPuesto", persona.DenomPuesto);
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?servicio", persona.Servicio);
                    cmd.Parameters.AddWithValue("?cedula", persona.Cedula);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }

            using (var cnn = ConexionRemoto())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?num", persona.Num);
                    cmd.Parameters.AddWithValue("?nombre", persona.Nombre);
                    cmd.Parameters.AddWithValue("?apellidos", persona.Apellidos);
                    cmd.Parameters.AddWithValue("?telefono", persona.Telefono);
                    cmd.Parameters.AddWithValue("?email", persona.Email);
                    cmd.Parameters.AddWithValue("?area", persona.Area);
                    cmd.Parameters.AddWithValue("?calle", persona.Calle);
                    cmd.Parameters.AddWithValue("?numero", persona.Numero);
                    cmd.Parameters.AddWithValue("?numInt", persona.NumInt);
                    cmd.Parameters.AddWithValue("?fracc", persona.Fracc);
                    cmd.Parameters.AddWithValue("?cp", persona.CP);
                    cmd.Parameters.AddWithValue("?rfc", persona.RFC);
                    cmd.Parameters.AddWithValue("?curp", persona.CURP);
                    cmd.Parameters.AddWithValue("?fechaIng", persona.FechaIng);
                    cmd.Parameters.AddWithValue("?puesto", persona.Puesto);
                    cmd.Parameters.AddWithValue("?turno", persona.Turno);
                    cmd.Parameters.AddWithValue("?contrato", persona.Contrato);
                    cmd.Parameters.AddWithValue("?turnoOpcional", persona.TurnoOpcional);
                    cmd.Parameters.AddWithValue("?denomPuesto", persona.DenomPuesto);
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?servicio", persona.Servicio);
                    cmd.Parameters.AddWithValue("?cedula", persona.Cedula);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }
            
            return result;
        }
        public int Update(Personal persona)
        {
            int result = 0;
            query = "UPDATE personal SET num=?,nombre=?,apellidos=?,telefono=?,email=?,area=?,calle=?,numero=?,numInt=?,fracc=?,cp=?,rfc=?,curp=?,fechaIng=?,puesto=?,turno=?,contrato=?,turnoOpcional=?,denomPuesto=?,huella=?,servicio=?,cedula=? " +
                    "WHERE id = ?";
                    
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?num", persona.Num);
                    cmd.Parameters.AddWithValue("?nombre", persona.Nombre);
                    cmd.Parameters.AddWithValue("?apellidos", persona.Apellidos);
                    cmd.Parameters.AddWithValue("?telefono", persona.Telefono);
                    cmd.Parameters.AddWithValue("?email", persona.Email);
                    cmd.Parameters.AddWithValue("?area", persona.Area);
                    cmd.Parameters.AddWithValue("?calle", persona.Calle);
                    cmd.Parameters.AddWithValue("?numero", persona.Numero);
                    cmd.Parameters.AddWithValue("?numInt", persona.NumInt);
                    cmd.Parameters.AddWithValue("?fracc", persona.Fracc);
                    cmd.Parameters.AddWithValue("?cp", persona.CP);
                    cmd.Parameters.AddWithValue("?rfc", persona.RFC);
                    cmd.Parameters.AddWithValue("?curp", persona.CURP);
                    cmd.Parameters.AddWithValue("?fechaIng", persona.FechaIng);
                    cmd.Parameters.AddWithValue("?puesto", persona.Puesto);
                    cmd.Parameters.AddWithValue("?turno", persona.Turno);
                    cmd.Parameters.AddWithValue("?contrato", persona.Contrato);
                    cmd.Parameters.AddWithValue("?turnoOpcional", persona.TurnoOpcional);
                    cmd.Parameters.AddWithValue("?denomPuesto", persona.DenomPuesto);
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?servicio", persona.Servicio);
                    cmd.Parameters.AddWithValue("?cedula", persona.Cedula);
                    cmd.Parameters.AddWithValue("?id", persona.Id);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }

            using (var cnn = ConexionRemoto())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?num", persona.Num);
                    cmd.Parameters.AddWithValue("?nombre", persona.Nombre);
                    cmd.Parameters.AddWithValue("?apellidos", persona.Apellidos);
                    cmd.Parameters.AddWithValue("?telefono", persona.Telefono);
                    cmd.Parameters.AddWithValue("?email", persona.Email);
                    cmd.Parameters.AddWithValue("?area", persona.Area);
                    cmd.Parameters.AddWithValue("?calle", persona.Calle);
                    cmd.Parameters.AddWithValue("?numero", persona.Numero);
                    cmd.Parameters.AddWithValue("?numInt", persona.NumInt);
                    cmd.Parameters.AddWithValue("?fracc", persona.Fracc);
                    cmd.Parameters.AddWithValue("?cp", persona.CP);
                    cmd.Parameters.AddWithValue("?rfc", persona.RFC);
                    cmd.Parameters.AddWithValue("?curp", persona.CURP);
                    cmd.Parameters.AddWithValue("?fechaIng", persona.FechaIng);
                    cmd.Parameters.AddWithValue("?puesto", persona.Puesto);
                    cmd.Parameters.AddWithValue("?turno", persona.Turno);
                    cmd.Parameters.AddWithValue("?contrato", persona.Contrato);
                    cmd.Parameters.AddWithValue("?turnoOpcional", persona.TurnoOpcional);
                    cmd.Parameters.AddWithValue("?denomPuesto", persona.DenomPuesto);
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?servicio", persona.Servicio);
                    cmd.Parameters.AddWithValue("?cedula", persona.Cedula);
                    cmd.Parameters.AddWithValue("?id", persona.Id);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }

            return result;
        }
        public int Remove(int id)
        {
            int result = 0;
            query = "UPDATE personal SET status = 'N' WHERE id = ?";
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?id", id);
                    result = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cnn.Close();
                }
            }
            
            return result;
        }


        public int UpdateHuella(Personal persona)
        {
            int result = 0;
            query = "UPDATE personal SET huella=? WHERE id = ?";
            
            //Guardar en equipo local
            using (var cnn = ConexionLocal())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?id", persona.Id);
                    result = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }
            //Guardar información en base de datos del servidor
            using (var cnn = ConexionRemoto())
            {
                cnn.Open();
                using (var cmd = new MySqlCommand(query, cnn))
                {
                    cmd.Parameters.AddWithValue("?huella", persona.Huella);
                    cmd.Parameters.AddWithValue("?id", persona.Id);
                    result = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }
            
            return result;
        }
    }
}
