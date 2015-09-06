namespace Kassenbuch
{
  using System.Globalization;
  using System.Windows;
  using System.Windows.Markup;

  /// <summary>
  /// Interaktionslogik für "App.xaml"
  /// </summary>
  public partial class App : Application
  {
    #region Methods

    /// <summary>
    /// Wird beim Start der Anwendung ausgeführt
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      // Ensure the current culture passed into bindings is the OS culture.
      // By default, WPF uses en-US as the culture, regardless of the system settings.
      FrameworkElement.LanguageProperty.OverrideMetadata(
    typeof(FrameworkElement),
    new FrameworkPropertyMetadata(
        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }

    #endregion Methods
  }
}