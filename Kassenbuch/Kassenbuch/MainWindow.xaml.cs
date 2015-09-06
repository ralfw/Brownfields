namespace Kassenbuch
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Xml.Serialization;

  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Fields

    /// <summary>
    /// Der Dateiname des Datenfiles.
    /// </summary>
    private static readonly string DataFilename = "KassenbuchData.xml";

    /// <summary>
    /// Der Dateiname des Exportfiles.
    /// </summary>
    private static readonly string ExportFilename = "KassenbuchData.csv";

    /// <summary>
    /// Der Serialisierer der für das Speichern und Laden verwendet wird.
    /// </summary>
    private readonly XmlSerializer serializer = new XmlSerializer(typeof(Overview));

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der Klasse MainWindow.
    /// </summary>
    public MainWindow()
    {
      this.InitializeComponent();
      NewSheetDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gibt den Overview dieses Fensters aus oder legt diesen fest.
    /// </summary>
    public Overview Overview
    {
      get { return (Overview)this.DataContext; }
      set { this.DataContext = value; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Wird aufgerufen wenn der Hinzufügen Button geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Add_Click(object sender, RoutedEventArgs e)
    {
      Overview.Sheets.Add(new Sheet() { Date = NewSheetDate.SelectedDate.Value });
    }

    /// <summary>
    /// Wird aufgerufen wenn der Exportieren Button geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Exportieren_Click(object sender, RoutedEventArgs e)
    {
      IEnumerable<SheetEntry> allEntrys = this.Overview.Sheets.SelectMany(o => o.Entrys);
      IEnumerable<string> tableRows = allEntrys.Select(o =>
      {
        var ary = new string[] { o.Date.ToString("d"), o.PaymentType, o.Amount.ToString(), o.Balance.ToString() };
        return string.Join(";", ary);
      });

      using (var file = File.CreateText(MainWindow.ExportFilename))
      {
        file.WriteLine("Datum;Art;Betrag;Kassenstand");
        foreach (var row in tableRows)
        {
          file.WriteLine(row);
        }
      }
    }

    /// <summary>
    /// Lädt den Overview, die Hauptklasse des Viewmodels.
    /// </summary>
    /// <returns>Den Geladenen Overview.</returns>
    private bool LoadOverview()
    {
      if (!File.Exists(DataFilename))
      {
        return false;
      }

      using (var file = File.OpenRead(DataFilename))
      {
        this.DataContext = this.serializer.Deserialize(file);
      }

      return true;
    }

    /// <summary>
    /// Wird aufgerufen wenn die Haupt Tabelle Doppelt geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void MainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
      Sheet contextSheet = (Sheet)row.DataContext;

      SheetRevision revision = contextSheet.MakeRevision();

      SheetWindow window = new SheetWindow();
      window.Sheet = contextSheet;

      if (window.ShowDialog() == true)
      {
        if (contextSheet.Revisions.Count > 0 || revision.Entrys.Count > 0)
        {
          contextSheet.Revisions.Add(revision);
        }
      }
      else
      {
        contextSheet.RestoreRevision(revision);
      }
    }

    /// <summary>
    /// Speichert den Overview, die Hauptklasse des Viewmodels.
    /// </summary>
    private void SaveOverview()
    {
      using (var file = File.CreateText(DataFilename))
      {
        this.serializer.Serialize(file, this.DataContext);
      }
    }

    /// <summary>
    /// Wird aufgerufen wenn der Speichern Button geklickt wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Save_Click(object sender, RoutedEventArgs e)
    {
      this.SaveOverview();
      MessageBox.Show("Die Eingaben wurden Gespeichert.");
    }

    /// <summary>
    /// Wird aufgerufen wenn das Fenster Initialisiert wird.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Window_Initialized(object sender, EventArgs e)
    {
      if (!this.LoadOverview())
      {
        this.DataContext = new Overview();
      }
    }

    #endregion Methods
  }
}