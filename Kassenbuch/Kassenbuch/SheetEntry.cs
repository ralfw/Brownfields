namespace Kassenbuch
{
  /// <summary>
  /// Die SheetEntry Klasse ist das Viewmodel für
  /// </summary>
  public class SheetEntry : SheetRevisionEntry
  {
    #region Fields

    /// <summary>
    /// Der Kassenbestand.
    /// </summary>
    private decimal balance;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetEntry Klasse.
    /// </summary>
    public SheetEntry()
    {
    }

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetEntry Klasse, 
    /// als Kopie einer SheetEntryRevision Instanz.
    /// </summary>
    /// <param name="entry">Die SheetEntryRevision die als Basis dient.</param>
    public SheetEntry(SheetRevisionEntry entry)
    {
      this.Amount = entry.Amount;
      this.Date = entry.Date;
      this.PaymentType = entry.PaymentType;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Der Name der Betragseingenschaft.
    /// </summary>
    public string AmountName
    {
      get
      {
        return ReflectionHelper.GetPropertyName(() => this.Amount);
      }
    }

    /// <summary>
    /// Der Betrag.
    /// </summary>
    public decimal Balance
    {
      get
      {
        return this.balance;
      }

      set
      {
        if (this.balance != value)
        {
          this.balance = value;
          this.OnPropertyChanged(() => this.Balance);
        }
      }
    }

    #endregion Properties
  }
}