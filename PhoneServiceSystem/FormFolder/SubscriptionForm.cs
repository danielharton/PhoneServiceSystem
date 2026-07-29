using PhoneServiceSystem.ClassFolder;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class SubscriptionForm : Form
    {
        private List<Subscription> subs;
        private List<Client> clients;
        private List<ExtraOption> options;

        public SubscriptionForm()
        {
            InitializeComponent();
        }

        private void SubscriptionForm_Load(object sender, EventArgs e)
        {
            clients = Repository.GetAllClients();
            options = Repository.GetAllExtraOptions();
            subs = Repository.GetAllSubscriptions();
            PopulateListView();
        }

        private void PopulateListView()
        {
            listViewSubs.Items.Clear();
            foreach (var s in subs)
            {
                var client = clients.Find(c => c.ClientId == s.ClientId);
                var opt = options.Find(o => o.ExtraOptionId == s.ExtraOptionId);
                var item = new ListViewItem(s.SubscriptionId.ToString());
                item.SubItems.Add(client?.LastName ?? "");
                item.SubItems.Add(opt?.Name ?? "");
                item.SubItems.Add(s.StartDate.ToShortDateString());
                item.SubItems.Add(s.EndDate?.ToShortDateString() ?? "");
                item.Tag = s;
                listViewSubs.Items.Add(item);
            }
            statusStrip1.Items[0].Text = $"{subs.Count} subscriptions loaded.";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var s = new Subscription { StartDate = DateTime.Now };
            using (var dlg = new SubscriptionEditDialog(s, clients, options))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.InsertSubscription(s);

                    subs.Add(s);
                    PopulateListView();
                    statusStrip1.Items[0].Text = "Subscription added.";
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewSubs.SelectedItems.Count == 0) return;
            var s = (Subscription)listViewSubs.SelectedItems[0].Tag;
            using (var dlg = new SubscriptionEditDialog(s, clients, options))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.UpdateSubscription(s);
                    PopulateListView();
                    statusStrip1.Items[0].Text = "Subscription updated.";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewSubs.SelectedItems.Count == 0) return;
            var s = (Subscription)listViewSubs.SelectedItems[0].Tag;
            if (MessageBox.Show("Delete this subscription?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Repository.DeleteSubscription(s.SubscriptionId);
                subs.Remove(s);
                PopulateListView();
                statusStrip1.Items[0].Text = "Subscription deleted.";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Repository.SaveSubscriptions(subs);
            PopulateListView();
            statusStrip1.Items[0].Text = "Subscriptions saved.";
        }
    }
}
