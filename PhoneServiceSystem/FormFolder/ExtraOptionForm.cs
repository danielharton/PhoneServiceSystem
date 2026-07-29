using PhoneServiceSystem.ClassFolder;
using PhoneServiceSystem.FormFolder;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class ExtraOptionForm : Form
    {
        private List<ExtraOption> options;

        public ExtraOptionForm()
        {
            InitializeComponent();
        }

        private void ExtraOptionForm_Load(object sender, EventArgs e)
        {
            options = Repository.GetAllExtraOptions();
            PopulateListView();
        }

        private void PopulateListView()
        {
            listViewOptions.Items.Clear();
            foreach (var o in options)
            {
                var item = new ListViewItem(o.ExtraOptionId.ToString());
                item.SubItems.Add(o.Name);
                item.SubItems.Add(o.MonthlyCost.ToString("C"));
                item.Tag = o;
                listViewOptions.Items.Add(item);
            }
            toolStripStatusLabel1.Text = $"{options.Count} options loaded.";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var o = new ExtraOption();
            using (var dlg = new ExtraOptionEditDialog(o))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.InsertExtraOption(o);

                    options.Add(o);
                    PopulateListView();
                    toolStripStatusLabel1.Text = "Option added.";
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewOptions.SelectedItems.Count == 0) return;
            var o = (ExtraOption)listViewOptions.SelectedItems[0].Tag;

            using (var dlg = new ExtraOptionEditDialog(o))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Repository.UpdateExtraOption(o);

                    PopulateListView();
                    toolStripStatusLabel1.Text = "Option updated.";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewOptions.SelectedItems.Count == 0) return;
            var o = (ExtraOption)listViewOptions.SelectedItems[0].Tag;

            if (MessageBox.Show($"Delete '{o.Name}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Repository.DeleteExtraOption(o.ExtraOptionId);

                options.Remove(o);
                PopulateListView();
                toolStripStatusLabel1.Text = "Option deleted.";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Options saved.";
        }
    }
}
