using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Web.Http;
using Windows.UI;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Notas
{
    public sealed partial class Detalhes : Page
    {
        public Detalhes()
        {
            this.InitializeComponent();

        }

        // Page navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            if (Frame.CanGoBack)
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            currentView.BackRequested += backButton_Tapped;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested -= backButton_Tapped;
        }
        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }


        private HttpClient httpClient;
        private HttpResponseMessage response;

        private async void Reload_Click(object sender, RoutedEventArgs e)
        {
            httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");

            response = new HttpResponseMessage();
            
            // The value of 'InputAddress' is set by the user and is therefore untrusted input. 
            // If we can't create a valid URI, 
            // We notify the user about the incorrect input.

            Materia.Text = "Test if URI is valid";

            string url = "https://wikipedia.com";

            Uri resourceUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out resourceUri))
            {
                Materia.Text = "Invalid URI, please re-enter a valid URI";
                return;
            }
            if (resourceUri.Scheme != "http" && resourceUri.Scheme != "https")
            {
                Materia.Text = "Only 'http' and 'https' schemes supported. Please re-enter URI";
                return;
            }

            string responseBodyAsText = "";
            Materia.Text = "Waiting for response ...";

            try
            {
                response = await httpClient.GetAsync(resourceUri);


                response.EnsureSuccessStatusCode();

                responseBodyAsText = await response.Content.ReadAsStringAsync();
                Materia.Text = "Agora vai!";
            }
            catch (Exception ex)
            {
                // Need to convert int HResult to hex string
                Campo.Text = "Error = " + ex.HResult.ToString("X") +
                    "  Message: " + ex.Message;
                Campo.Text = "ops";
            }

            Campo.Text = responseBodyAsText;

            Materia.Text = response.StatusCode + " " + response.ReasonPhrase;
        }

        private async void M1_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            M1_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 5, 85, 156));
            M2_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));
            M3_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));

            M1_Content.Visibility = Visibility.Visible;
            M2_Content.Visibility = Visibility.Collapsed;
            M3_Content.Visibility = Visibility.Collapsed;

            await GetjsonStream();
        }

        private async void M2_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            M1_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));
            M2_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 5, 85, 156));
            M3_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));

            M1_Content.Visibility = Visibility.Collapsed;
            M2_Content.Visibility = Visibility.Visible;
            M3_Content.Visibility = Visibility.Collapsed;
            await GetjsonStream();
        }

        public async Task<string> GetjsonStream()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri("http://www.filltext.com/?rows=10&fname={firstName}&lname={lastName}&tel={phone|format}&address={streetAddress}&city={city}&state={usState|abbr}&zip={zip}&pretty=true");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                string content = await response.Content.ReadAsStringAsync();

                Campo.Text = content;

                return content;
            }
            catch
            {
                Campo.Text = "Deu ruim. :(";
            }
            return "0";
        }

        private void M3_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            M1_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));
            M2_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 7, 58, 114));
            M3_Grid.Background = new SolidColorBrush(Color.FromArgb(255, 5, 85, 156));

            M1_Content.Visibility = Visibility.Collapsed;
            M2_Content.Visibility = Visibility.Collapsed;
            M3_Content.Visibility = Visibility.Visible;

            // UpdateTile("Nova nota!", "Resistência dos materiais: 8");
            // simpleToast("Nova nota!", "Resistência dos materiais: 8");
        }

        private static void UpdateTile(string titleString, string detailString)
        {
            // create the instance of Tile Updater, which enables you to change the appearance of the calling app's tile
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();

            // enables the tile to queue up to five notifications
            updater.EnableNotificationQueue(true);
            updater.Clear();
            // get the XML content of one of the predefined tile templates, so that, you can customize it
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText01);

            XmlElement tmp = tileXml.GetElementsByTagName("visual")[0] as XmlElement;
            tmp.SetAttribute("branding", "name");

            tileXml.GetElementsByTagName("text")[0].InnerText = titleString;
            tileXml.GetElementsByTagName("text")[1].InnerText = detailString;

            // Create a new tile notification. 
            updater.Update(new TileNotification(tileXml));
        }

        private void simpleToast(string titleString, string detailString)
        {
            ToastTemplateType toastType = ToastTemplateType.ToastText02;

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastType);

            XmlNodeList toastTextElement = toastXml.GetElementsByTagName("text");
            toastTextElement[0].AppendChild(toastXml.CreateTextNode(titleString));
            toastTextElement[1].AppendChild(toastXml.CreateTextNode(detailString));

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void HamburgerConfig_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Config));
        }
    }
}
