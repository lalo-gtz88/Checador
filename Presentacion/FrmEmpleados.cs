using DatosRH;
using DatosRH.DAO;
using DPFP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    delegate void Function();
    public partial class FrmEmpleados : Form
    {
        private DPFP.Template Template;
        PersonalDAO dao = new PersonalDAO();
        int ID = 0;
        public FrmEmpleados()
        {
            InitializeComponent();
        }

        private void Refrescar(string valor)
        {
            valor = valor.ToLower();
            var list = dao.GetAll();
            var empleados = (from l in list
                             where
                             l.Status != "N" && (l.Nombre.ToLower().Contains(valor) || l.Apellidos.ToLower().Contains(valor) || Convert.ToString(l.Num) == valor)
                             select new
                             {
                                 ID = l.Id,
                                 Num = l.Num,
                                 Nombre = l.Apellidos + " " + l.Nombre

                             }).OrderBy(x => x.Num).ToList();
            dataGridView1.DataSource = empleados;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["nombre"].Width = 2500;

        }

        private void Limpiar()
        {
            ID = 0;
            txtNum.Clear();
            txtNombre.Clear();
            btnGuardar.Enabled = false;
            btnEnrolar.Enabled = false;
            btnCancel.Enabled = false;
            panel1.Enabled = true;
        }

        private void Guardar()
        {
            if(Template != null)
            {
                var list = dao.GetAll();
                var persona = new Personal
                {
                    Id = ID,
                    Huella = Template.Bytes
                };

                var result = dao.UpdateHuella(persona);
                if (result > 0)
                    MessageBox.Show("Se registró la huella exitosamente!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Se requiere capturar una huella dactilar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void lbBack_Click(object sender, EventArgs e)
        {
            FrmMain frm = new FrmMain();
            frm.Show();
            this.Close();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            Refrescar(txtBuscar.Text);
        }

        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            
            Refrescar(txtBuscar.Text);
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if(txtBuscar.Text == "")
            {
                Refrescar("");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Refrescar(txtBuscar.Text);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            txtNum.Text = dataGridView1.CurrentRow.Cells["num"].Value.ToString();
            txtNombre.Text = dataGridView1.CurrentRow.Cells["nombre"].Value.ToString();
            btnGuardar.Enabled = true;
            btnEnrolar.Enabled = true;
            btnCancel.Enabled = true;
            panel1.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEnrolar_Click(object sender, EventArgs e)
        {
            EnrollmentForm Enroller = new EnrollmentForm();
            Enroller.OnTemplate += this.OnTemplate;
            Enroller.ShowDialog();
        }

        private void OnTemplate(Template template)
        {
            this.Invoke(new Function(delegate ()
            {
                Template = template;
                //VerifyButton.Enabled = SaveButton.Enabled = (Template != null);
                if (Template != null)
                    MessageBox.Show("Se enroló la huella correctamente. Guarda para que surgan efecto los cambios", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("La huella dactilar no es valida. Repite el proceso de Enrolamiento por favor", "Alerta",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }));
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
            Limpiar();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Refrescar(txtBuscar.Text);
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                e.Handled = true;
            }
        }
    }
}
