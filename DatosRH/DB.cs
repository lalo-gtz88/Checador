using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatosRH.DAO
{
    public class DB
    {
        private string strConnLocal = "Server=localhost;Database=rh;Uid=root;Pwd=";
        private string strConnRemote = "Server=192.160.205.4;Database=rh;Uid=remoto;Pwd=s0p0rt3t3cn1co";

        protected MySqlConnection ConexionLocal()
        {
            return new MySqlConnection(strConnLocal);
        }
     
        protected MySqlConnection ConexionRemoto()
        {
            return new MySqlConnection(strConnRemote);
        }
    }
}
