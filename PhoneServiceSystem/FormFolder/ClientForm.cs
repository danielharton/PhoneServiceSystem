using PhoneServiceSystem.ClassFolder;
using PhoneServiceSystem.FormFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneServiceSystem
{
    public partial class ClientForm : Form
    {
        private List<Client> clients;

        public ClientForm()
        {
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            clients = Repository.GetAllClients();
            PopulateListView();
        }

        private void PopulateListView()
        {
            listViewClients.Items.Clear();
            foreach (var c in clients)
            {
                var item = new ListViewItem(c.ClientId.ToString());
                item.SubItems.Add(c.FirstName);
                item.SubItems.Add(c.LastName);
                item.SubItems.Add(c.PhoneNumber);
                item.Tag = c;
                listViewClients.Items.Add(item);
            }
            statusStrip1.Items[0].Text = $"{clients.Count} clients loaded.";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var c = new Client();
            using (var dlg = new ClientEditDialog(c))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(c.FirstName))
                    {
                        MessageBox.Show("First name is required", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(c.LastName))
                    {
                        MessageBox.Show("Last name is required", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(c.PhoneNumber))
                    {
                        MessageBox.Show("Phone number is required", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        Repository.InsertClient(c);
                        clients.Add(c);
                        PopulateListView();
                        statusStrip1.Items[0].Text = "Client added successfully.";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding client: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0) return;
            var item = listViewClients.SelectedItems[0];
            var c = (Client)item.Tag;
            using (var dlg = new ClientEditDialog(c))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PopulateListView();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0) return;
            var item = listViewClients.SelectedItems[0];
            var c = (Client)item.Tag;

            if (MessageBox.Show(
                    $"Delete {c.FirstName} {c.LastName}?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                try
                {
                    Repository.DeleteClient(c.ClientId);

                    clients.Remove(c);
                    PopulateListView();
                    statusStrip1.Items[0].Text = "Client deleted successfully.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error deleting client: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Repository.SaveClients(clients);
                statusStrip1.Items[0].Text = "Clients saved successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving clients: " + ex.Message);
            }
        }
    }
}
