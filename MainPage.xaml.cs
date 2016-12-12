using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace Notas
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            List<Materias> items = new List<Materias>();
            items.Add(new Materias() { NomeMateria = "Resistencia dos Materiais", MediaFinal = 8, Faltas = "4 Faltas" });
            items.Add(new Materias() { NomeMateria = "Resistencia dos Materiais", MediaFinal = 9, Faltas = "0 Faltas" });
            items.Add(new Materias() { NomeMateria = "Resistencia dos Materiais", MediaFinal = 7, Faltas = "2 Faltas" });
            items.Add(new Materias() { NomeMateria = "Resistencia dos Materiais", MediaFinal = 6, Faltas = "4 Faltas" });
            items.Add(new Materias() { NomeMateria = "Resistencia dos Materiais", MediaFinal = 8, Faltas = "4 Faltas" });

            lvDataBinding.ItemsSource = items;

            
        }

        // Page navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Detalhes));
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

        public class Materias
        {
            public string NomeMateria { get; set; }

            public int MediaFinal { get; set; }

            public string Faltas { get; set; }
        }

        private void M1_Item_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void M2_Item_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void M3_Item_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

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
