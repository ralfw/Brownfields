namespace Kassenbuch
{
  using System.Windows;

  /// <summary>
  /// Interaktionslogik für RevisionDetailsWindow.xaml
  /// </summary>
  public partial class RevisionDetailsWindow : Window
  {
    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Klasse der RevisionDetailsWindow Klasse.
    /// </summary>
    public RevisionDetailsWindow()
    {
      InitializeComponent();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Die SheetRevision des Fensters.
    /// </summary>
    public SheetRevision SheetRevision
    {
      get { return (SheetRevision)DataContext; }
      set { DataContext = value; }
    }

    #endregion Properties
  }
}