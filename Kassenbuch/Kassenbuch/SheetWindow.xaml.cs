namespace Kassenbuch
{
  using System.Windows;

  /// <summary>
  /// Interaktionslogik für SheetWindow.xaml
  /// </summary>
  public partial class SheetWindow : Window
  {
    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetWindow Klasse.
    /// </summary>
    public SheetWindow()
    {
      this.InitializeComponent();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Das Sheet dieses Fensters.
    /// </summary>
    public Sheet Sheet
    {
      get { return (Sheet)this.DataContext; }
      set { this.DataContext = value; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Wird beim Klick auf Abbrechen augeführt.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Abbrechen_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
      this.Close();
    }

    /// <summary>
    /// Wird beim Klick auf Ok ausgeführt.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Ok_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
      this.Close();
    }

    /// <summary>
    /// Wird beim Klick auf Revisionen ausgeührt.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Revisionen_Click(object sender, RoutedEventArgs e)
    {
      RevisionsWindow window = new RevisionsWindow();
      window.Sheet = Sheet;
      window.ShowDialog();
    }

    #endregion Methods
  }
}