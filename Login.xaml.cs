using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace Notas
{
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            BotaoEntrar.IsEnabled = false;
            CodPessoa.IsEnabled = false;
            Senha.IsEnabled = false;

            string username = CodPessoa.Text;
            string password = Senha.Password;

            if (username == "" || password == "")
            {
                showErrorMessage();
                BotaoEntrar.IsEnabled = true;
                CodPessoa.IsEnabled = true;
                Senha.IsEnabled = true;
                return;
            }

            var loginCredential = GetCredentialFromLocker(username);
            if (loginCredential == null)
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential("Notas", username, password));
            }

            loginCredential.RetrievePassword();

            if (username == loginCredential.UserName && password == loginCredential.Password)
            {
                BotaoEntrar.IsEnabled = true;
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                showErrorMessage();
                BotaoEntrar.IsEnabled = true;
                CodPessoa.IsEnabled = true;
                Senha.IsEnabled = true;
            }
        }

        private PasswordCredential GetCredentialFromLocker(string CodPessoa)
        {

            PasswordCredential credential = null;
            var vault = new PasswordVault();
            var credentialList = vault.FindAllByResource(CodPessoa);
            if (credentialList.Count > 0)
            {
                credential = credentialList[0];
            }

            return credential;
        }

        void showErrorMessage()
        {
            var dialog = new MessageDialog("Verifique se os campos foram preenchidos corretamente.", "Credenciais inválidas");
            dialog.ShowAsync();
        }

        private void BotaoTermos_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (Termos.Visibility == Visibility.Collapsed)
            {
                Termos.Visibility = Visibility.Visible;
                BotaoTermos.Visibility = Visibility.Collapsed;
            } else
            {
                Termos.Visibility = Visibility.Collapsed;
                BotaoTermos.Visibility = Visibility.Visible;
            }

        }

        private void GoToHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
