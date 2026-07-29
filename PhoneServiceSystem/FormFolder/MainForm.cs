using PhoneServiceSystem.ClassFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class MainForm : Form
    {
        private List<Client> _clients;
        private List<ExtraOption> _options;
        private List<Subscription> _subs;

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSave.Click += BtnSave_Click;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _clients = Repository.GetAllClients();
            _options = Repository.GetAllExtraOptions();
            _subs = Repository.GetAllSubscriptions();
            PopulateListView();
        }

        private void PopulateListView()
        {
            listView1.Items.Clear();
            foreach (var s in _subs)
            {
                var client = _clients.FirstOrDefault(c => c.ClientId == s.ClientId);
                var option = _options.FirstOrDefault(o => o.ExtraOptionId == s.ExtraOptionId);

                var item = new ListViewItem(s.SubscriptionId.ToString());
                item.SubItems.Add(client?.FirstName ?? string.Empty);
                item.SubItems.Add(client?.LastName ?? string.Empty);
                item.SubItems.Add(option?.Name ?? string.Empty);
                item.SubItems.Add(option != null
                    ? option.MonthlyCost.ToString("C")
                    : string.Empty);
                item.SubItems.Add(client?.PhoneNumber ?? string.Empty);
                item.SubItems.Add(s.StartDate.ToShortDateString());
                item.SubItems.Add(s.EndDate?.ToShortDateString() ?? string.Empty);
                item.Tag = s;
                listView1.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            _clients = Repository.GetAllClients();
            _options = Repository.GetAllExtraOptions();

            var s = new Subscription { StartDate = DateTime.Now };
            using (var dlg = new SubscriptionEditDialog(s, _clients, _options))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.InsertSubscription(s);
                    _subs.Add(s);
                    PopulateListView();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            _clients = Repository.GetAllClients();
            _options = Repository.GetAllExtraOptions();

            var s = (Subscription)listView1.SelectedItems[0].Tag;
            using (var dlg = new SubscriptionEditDialog(s, _clients, _options))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.UpdateSubscription(s);
                    PopulateListView();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var s = (Subscription)listView1.SelectedItems[0].Tag;
            if (MessageBox.Show($"Delete subscription {s.SubscriptionId}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Repository.DeleteSubscription(s.SubscriptionId);
                _subs.Remove(s);
                PopulateListView();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Repository.SaveSubscriptions(_subs);
            PopulateListView();
            MessageBox.Show("Subscriptions saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClientForm().Show();
        }

        private void extraOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExtraOptionForm().Show();
        }

        private void subscriptinosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SubscriptionForm().Show();
        }

        private void clientsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clientsToolStripMenuItem_Click(sender, e);
        }

        private void exporttxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dlg.DefaultExt = "txt";
                dlg.FileName = "PhoneServiceSystemExport.txt";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var sw = new System.IO.StreamWriter(dlg.FileName, false, Encoding.UTF8))
                    {
                        sw.WriteLine("=== Clients ===");
                        foreach (var c in _clients)
                        {
                            sw.WriteLine($"{c.ClientId}\t{c.FirstName}\t{c.LastName}\t{c.PhoneNumber}");
                        }
                        sw.WriteLine();

                        sw.WriteLine("=== Extra Options ===");
                        foreach (var o in _options)
                        {
                            sw.WriteLine($"{o.ExtraOptionId}\t{o.Name}\t{o.MonthlyCost}");
                        }
                        sw.WriteLine();

                        sw.WriteLine("=== Subscriptions ===");
                        foreach (var s in _subs)
                        {
                            sw.WriteLine($"{s.SubscriptionId}\t{s.ClientId}\t{s.ExtraOptionId}\t{s.StartDate:yyyy-MM-dd}\t{(s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd") : "NULL")}");
                        }
                    }

                    MessageBox.Show($"Data exported to:\n{dlg.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data:\n{ex.Message}", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
