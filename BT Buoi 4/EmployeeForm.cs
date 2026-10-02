using System;
using System.Globalization;
using System.Windows.Forms;

using System.ComponentModel;

namespace BT_Buoi_4
{
    public partial class EmployeeForm : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Employee Employee { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnlyId { get; set; }

        // Delegate callback invoked when the user confirms the dialog
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Employee>? OnSaved { get; set; }

        public EmployeeForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Employee != null)
            {
                txtId.Text = Employee.Id;
                txtName.Text = Employee.Name;
                txtSalary.Text = Employee.Salary.ToString(CultureInfo.InvariantCulture);
            }
            txtId.ReadOnly = ReadOnlyId;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Id required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtSalary.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var sal))
            {
                MessageBox.Show("Salary must be a number", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Employee = Employee ?? new Employee();
            Employee.Id = txtId.Text.Trim();
            Employee.Name = txtName.Text.Trim();
            Employee.Salary = sal;

            // invoke delegate callback if provided
            try
            {
                OnSaved?.Invoke(Employee);
            }
            catch
            {
                // swallow exceptions from callback to avoid breaking UI flow
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
