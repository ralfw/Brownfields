namespace Kassenbuch
{
  using System;
  using System.Collections.ObjectModel;

  /// <summary>
  /// Die SheetRevision Klasse ist das ViewModel zu einer Kassenblatt Revision.
  /// </summary>
  public class SheetRevision
  {
    #region Fields

    /// <summary>
    /// Die Einträge.
    /// </summary>
    private readonly ObservableCollection<SheetRevisionEntry> entrys = new ObservableCollection<SheetRevisionEntry>();

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetRevision Klasse.
    /// </summary>
    public SheetRevision()
    {
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Das Datum.
    /// </summary>
    public DateTime Date
    {
      get;
      set;
    }

    /// <summary>
    /// Die Einträge.
    /// </summary>
    public ObservableCollection<SheetRevisionEntry> Entrys
    {
      get
      {
        return this.entrys;
      }

      set
      {
        this.entrys.Clear();
        foreach (var entry in value)
        {
          this.entrys.Add(entry);
        }
      }
    }

    #endregion Properties
  }
}