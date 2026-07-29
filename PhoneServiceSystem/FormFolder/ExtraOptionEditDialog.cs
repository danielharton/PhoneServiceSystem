using PhoneServiceSystem.ClassFolder;
using System;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class ExtraOptionEditDialog : Form
    {
        private ExtraOption _option;

        public ExtraOptionEditDialog(ExtraOption o)
        {
            InitializeComponent();
            _option = o;

            tbName.Text = o.Name;
            tbMonthlyCost.Text = o.MonthlyCost.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Name is required", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(tbMonthlyCost.Text, out var cost))
            {
                MessageBox.Show("Monthly cost must be a number", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _option.Name = tbName.Text.Trim();
            _option.MonthlyCost = (float)cost;

            this.DialogResult = DialogResult.OK;
        }
    }
}
