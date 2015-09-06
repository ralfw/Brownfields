namespace Kassenbuch
{
  using System;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq.Expressions;

  /// <summary>
  /// Die Overview ist die ViewModel Hauptklasse.
  /// Sie beinhaltet die gesammte Datenstruktur des Programms.
  /// </summary>
  public class Overview : INotifyPropertyChanged
  {
    #region Fields

    /// <summary>
    /// Die Kassen blätter.
    /// </summary>
    private readonly ObservableCollection<Sheet> sheets = new ObservableCollection<Sheet>();

    /// <summary>
    /// Der Kassenbestand.
    /// </summary>
    private decimal balance;

    /// <summary>
    /// Gibt an ob derzeit die Kassenbestände neu berechnet werden.
    /// </summary>
    private bool currentlyRecalculatingBalance;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine nue Instanz der Overview Klasse.
    /// </summary>
    public Overview()
    {
      this.sheets.CollectionChanged += this.Sheets_CollectionChanged;
    }

    #endregion Constructors

    #region Events

    /// <summary>
    /// Wird aufgerufen wenn sich eine eigenschaft der Klasse geändert hat.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gibt den Kassenbestand aus oder legt ihn fest.
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

    /// <summary>
    /// Gibt die Auflisstung der Kassenblätter aus oder legt diese fest.
    /// </summary>
    public ObservableCollection<Sheet> Sheets
    {
      get
      {
        return this.sheets;
      }

      set
      {
        this.sheets.Clear();
        foreach (var sheet in value)
        {
          this.sheets.Add(sheet);
        }

        this.OnPropertyChanged(() => this.Sheets);
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

    /// <summary>
    /// Berechnet die Kassenbestände neu.
    /// </summary>
    private void RecalculateBalance()
    {
      if (this.currentlyRecalculatingBalance)
      {
        return;
      }

      this.currentlyRecalculatingBalance = true;

      decimal balance = 0;
      foreach (var sheet in this.sheets)
      {
        sheet.Carry = balance;
        balance += sheet.TotalAmount;
        sheet.Balance = balance;
      }
      this.Balance = balance;

      this.currentlyRecalculatingBalance = false;
    }

    private void Sheets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      this.RecalculateBalance();

      if (e.NewItems != null)
      {
        foreach (Sheet sheet in e.NewItems)
        {
          sheet.PropertyChanged += this.Sheet_PropertyChanged;
        }
      }

      this.OnPropertyChanged(() => this.Sheets);
    }

    private void Sheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == ReflectionHelper.GetPropertyName(() => ((Sheet)sender).TotalAmount))
      {
        this.RecalculateBalance();
      }

      this.OnPropertyChanged(() => this.Sheets);
    }

    #endregion Methods
  }
}