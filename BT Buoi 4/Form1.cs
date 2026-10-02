using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BT_Buoi_4
{
    public partial class Form1 : Form
    {
        private BindingList<Employee> employees = new BindingList<Employee>();

        public Form1()
        {
            InitializeComponent();

            

            dgvEmployees.AutoGenerateColumns = false;
            dgvEmployees.DataSource = employees;

            // configure columns
            var colId = new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "MSNV", Width = 120 };
            var colName = new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Ten NV", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colSalary = new DataGridViewTextBoxColumn { DataPropertyName = "Salary", HeaderText = "Luong CB", Width = 120, DefaultCellStyle = { Format = "N0" } };
            dgvEmployees.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colSalary });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // set a new id (simple incremental)
            int next = 1;
            if (employees.Count > 0)
            {
                var last = employees.OrderBy(emp => emp.Id).LastOrDefault();
                if (last != null && last.Id.Length > 2 && int.TryParse(last.Id.Substring(2), out var n)) next = n + 1;
            }

            var emp = new Employee { Id = "NV" + next.ToString("D3") };
            var f = new EmployeeForm { Employee = emp };
            // use delegate to receive the saved employee
            f.OnSaved = e => employees.Add(e);
            f.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null) return;
            var emp = dgvEmployees.CurrentRow.DataBoundItem as Employee;
            if (emp == null) return;
            // open editor with the existing employee; use delegate to apply changes
            var f = new EmployeeForm { Employee = new Employee { Id = emp.Id, Name = emp.Name, Salary = emp.Salary }, ReadOnlyId = true };
            f.OnSaved = updated =>
            {
                emp.Name = updated.Name;
                emp.Salary = updated.Salary;
                dgvEmployees.Refresh();
            };
            f.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null) return;
            var emp = dgvEmployees.CurrentRow.DataBoundItem as Employee;
            if (emp == null) return;
            var r = MessageBox.Show($"Delete {emp.Name} (" + emp.Id + ")?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) employees.Remove(emp);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
