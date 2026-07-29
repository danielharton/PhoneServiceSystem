using PhoneServiceSystem.ClassFolder;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class SubscriptionEditDialog : Form
    {
        private Subscription _subscription;
        private List<Client> _clients;
        private List<ExtraOption> _options;

        public SubscriptionEditDialog(Subscription subscription, List<Client> clients, List<ExtraOption> options)
        {
            InitializeComponent();
            _subscription = subscription;
            _clients = clients;
            _options = options;

            cbClients.DataSource = _clients;
            cbClients.DisplayMember = "LastName";
            cbClients.ValueMember = "ClientId";

            cbOptions.DataSource = _options;
            cbOptions.DisplayMember = "Name";
            cbOptions.ValueMember = "ExtraOptionId";

            cbClients.SelectedValue = _subscription.ClientId;
            cbOptions.SelectedValue = _subscription.ExtraOptionId;
            dtpStartDate.Value = _subscription.StartDate;
            dtpEndDate.Value = _subscription.EndDate ?? DateTime.Now;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbClients.SelectedValue == null || cbOptions.SelectedValue == null)
            {
                MessageBox.Show("Please select both a client and an option.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _subscription.ClientId = Convert.ToInt64(cbClients.SelectedValue);
            _subscription.ExtraOptionId = Convert.ToInt64(cbOptions.SelectedValue);
            _subscription.StartDate = dtpStartDate.Value.Date;
            _subscription.EndDate = dtpEndDate.Value.Date;

            this.DialogResult = DialogResult.OK;
        }
    }
}