using PhoneServiceSystem.ClassFolder;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PhoneServiceSystem.FormFolder
{
    public partial class ClientEditDialog : Form
    {
        private Client _client;
        private readonly ErrorProvider _errorProvider;

        public ClientEditDialog(Client client)
        {
            InitializeComponent();
            _client = client;

            _errorProvider = new ErrorProvider
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink,
                ContainerControl = this
            };

            tbFirstName.Text = client.FirstName;
            tbLastName.Text = client.LastName;
            tbPhoneNumber.Text = client.PhoneNumber;

            tbFirstName.Validating += TbFirstName_Validating;
            tbFirstName.Validated += TbFirstName_Validated;
            tbLastName.Validating += TbLastName_Validating;
            tbLastName.Validated += TbLastName_Validated;
        }

        private void TbFirstName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var text = tbFirstName.Text.Trim();
                if (string.IsNullOrWhiteSpace(text))
                    throw new NameValidationException("First name is required.");
                if (text.Length < 2)
                    throw new NameValidationException("First name must be at least 2 characters.");
            }
            catch (NameValidationException ex)
            {
                _errorProvider.SetError(tbFirstName, ex.Message);
                e.Cancel = true;
            }
        }

        private void TbFirstName_Validated(object sender, EventArgs e)
        {
            _errorProvider.SetError(tbFirstName, string.Empty);
        }

        private void TbLastName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var text = tbLastName.Text.Trim();
                if (string.IsNullOrWhiteSpace(text))
                    throw new NameValidationException("Last name is required.");
                if (text.Length < 2)
                    throw new NameValidationException("Last name must be at least 2 characters.");
            }
            catch (NameValidationException ex)
            {
                _errorProvider.SetError(tbLastName, ex.Message);
                e.Cancel = true;
            }
        }

        private void TbLastName_Validated(object sender, EventArgs e)
        {
            _errorProvider.SetError(tbLastName, string.Empty);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _client.FirstName = tbFirstName.Text.Trim();
            _client.LastName = tbLastName.Text.Trim();
            _client.PhoneNumber = tbPhoneNumber.Text.Trim();

            this.DialogResult = DialogResult.OK;
        }
    }
}
public class NameValidationException : Exception
{
    public NameValidationException(string message) : base(message) { }
}