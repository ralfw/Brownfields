namespace Kassenbuch
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  /// <summary>
  /// Interaktionslogik für RevisionsWindow.xaml
  /// </summary>
  public partial class RevisionsWindow : Window
  {
    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der RevisionsWindow Klasse.
    /// </summary>
    public RevisionsWindow()
    {
      this.InitializeComponent();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Das Kassenblatt des Fensters.
    /// </summary>
    public Sheet Sheet
    {
      get { return (Sheet)this.DataContext; }
      set { this.DataContext = value; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Wird beim Klick auf Abbrechen aufgerufen.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Abbrechen_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
      this.Close();
    }

    /// <summary>
    /// Wird ausgeführt wenn eine Zeile geladen wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Revisions_LoadingRow(object sender, DataGridRowEventArgs e)
    {
      e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }

    /// <summary>
    /// Wird ausgeführt wenn eine Tabellenzeile Doppelt geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Revisions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
      SheetRevision contextSheetRevision = (SheetRevision)row.DataContext;

      RevisionDetailsWindow window = new RevisionDetailsWindow();
      window.SheetRevision = contextSheetRevision;
      window.ShowDialog();
    }

    /// <summary>
    /// Wird ausgeführt wenn der Wiederhestellen Button geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Wiederherstellen_Click(object sender, RoutedEventArgs e)
    {
      Sheet.Revisions.Add(Sheet.MakeRevision());

      SheetRevision revisionToRestore = (SheetRevision)this.RevisionsTable.SelectedItem;
      Sheet.Revisions.Remove(revisionToRestore);
      Sheet.RestoreRevision(revisionToRestore);

      this.Close();
    }

    #endregion Methods
  }
}