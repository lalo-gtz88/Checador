using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatosRH.DAO;
using DatosRH.DTO;

namespace Presentacion
{
    public partial class FrmLogin : Form
    {
        UsuarioDao dao = new UsuarioDao();
        public FrmLogin()
        {
            InitializeComponent();
        }


        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var textUser = txtUsuario.Text.ToLower();

            var usuarios = dao.GetAll();
            var usuario = usuarios.Select(x => x).Where(x => x.Username == textUser && x.Pass == txtPass.Text).FirstOrDefault();
            if (usuario != null)
            {
                FrmMain frm = new FrmMain();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas, favor de verificar", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
