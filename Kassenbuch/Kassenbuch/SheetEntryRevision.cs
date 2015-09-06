namespace Kassenbuch
{
  using System;
  using System.ComponentModel;
  using System.Linq.Expressions;

  /// <summary>
  /// Die SheetRevisionEntry ist das Viewmodel für einen Eintrag in einer Revision.
  /// </summary>
  public class SheetRevisionEntry : INotifyPropertyChanged
  {
    #region Fields

    /// <summary>
    /// Der Betrag.
    /// </summary>
    private decimal amount;

    /// <summary>
    /// Das Datum.
    /// </summary>
    private DateTime date = DateTime.Now;

    /// <summary>
    /// Die Art des Buchung.
    /// </summary>
    private string paymentType;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetRevisionEntry Klasse.
    /// </summary>
    public SheetRevisionEntry()
    {
    }

    /// <summary>
    /// Initialisiert eine neue Instanz der SheetRevisionEntry Klasse, 
    /// als Kopie einer SheetEntryRevision Instanz.
    /// </summary>
    /// <param name="entry">Die SheetEntryRevision die als Basis dient.</param>
    public SheetRevisionEntry(SheetRevisionEntry entry)
    {
      this.Amount = entry.Amount;
      this.Date = entry.Date;
      this.PaymentType = entry.PaymentType;
    }

    #endregion Constructors

    #region Events

    /// <summary>
    /// Der Eventhandler der aufgerufen wird wenn eine Eigenschaft geändert wird.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    /// <summary>
    /// Der Betrag.
    /// </summary>
    public decimal Amount
    {
      get
      {
        return this.amount;
      }

      set
      {
        if (this.amount != value)
        {
          this.amount = value;
          this.OnPropertyChanged(() => this.Amount);
        }
      }
    }

    /// <summary>
    /// Das Datum.
    /// </summary>
    public DateTime Date
    {
      get
      {
        return this.date;
      }

      set
      {
        if (this.date != value)
        {
          this.date = value;
          this.OnPropertyChanged(() => this.Date);
        }
      }
    }

    /// <summary>
    /// Die Art der Buchung.
    /// </summary>
    public string PaymentType
    {
      get
      {
        return this.paymentType;
      }

      set
      {
        if (this.paymentType != value)
        {
          this.paymentType = value;
          this.OnPropertyChanged(() => this.PaymentType);
        }
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Wird aufgerufen wenn sich eine Eigenschaft geändert hat.
    /// </summary>
    /// <typeparam name="T">Der Typ der Property.</typeparam>
    /// <param name="propertyLambda">Das Lambda das auf die Property verweist.</param>
    protected void OnPropertyChanged<T>(Expression<Func<T>> propertyLambda)
    {
      string propertyName = ReflectionHelper.GetPropertyName(propertyLambda);
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion Methods
  }
}