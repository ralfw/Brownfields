namespace Kassenbuch
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Text;
  using System.Threading.Tasks;
  using System.Xml.Serialization;

  /// <summary>
  /// Die Sheet Klasse ist das Viewmodel für ein Kassenblatt.
  /// </summary>
  public class Sheet : INotifyPropertyChanged
  {
    #region Fields

    /// <summary>
    /// Die Einträge.
    /// </summary>
    private readonly ObservableCollection<SheetEntry> entrys = new ObservableCollection<SheetEntry>();

    /// <summary>
    /// Der Gesamtbetrag dieses Blattes.
    /// </summary>
    private decimal balance;

    /// <summary>
    /// Der Übertrag.
    /// </summary>
    private decimal carry;

    /// <summary>
    /// Gibt an ob derzeit die Kassenbestände neu berechnet werden.
    /// </summary>
    private bool currentlyRecalculatingBalance;

    /// <summary>
    /// Das Datum.
    /// </summary>
    private DateTime date;

    /// <summary>
    /// Die Revisionen.
    /// </summary>
    private ObservableCollection<SheetRevision> revisions = new ObservableCollection<SheetRevision>();

    /// <summary>
    /// Der Gesamtkassenbestand dieses Blattes.
    /// </summary>
    private decimal totalAmount;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initialisiert eine neue Instanz der Sheet Klasse.
    /// </summary>
    public Sheet()
    {
      this.entrys.CollectionChanged += this.Entrys_CollectionChanged;
    }

    #endregion Constructors

    #region Events

    /// <summary>
    /// Der Eventhandler der aufgerufen wird wenn sich eine Eigenschaft geändert hat.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    /// <summary>
    /// Der Kassenbestand.
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
    /// Der Übertrag.
    /// </summary>
    public decimal Carry
    {
      get
      {
        return this.carry;
      }

      set
      {
        if (this.carry != value)
        {
          this.carry = value;
          this.RecalculateBalance();
          this.OnPropertyChanged(() => this.Carry);
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
    /// Die Einträge.
    /// </summary>
    public ObservableCollection<SheetEntry> Entrys
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

    /// <summary>
    /// Die Revisionen.
    /// </summary>
    public ObservableCollection<SheetRevision> Revisions
    {
      get
      {
        return this.revisions;
      }

      set
      {
        if (this.revisions != value)
        {
          this.revisions = value;
          this.OnPropertyChanged(() => this.Revisions);
        }
      }
    }

    /// <summary>
    /// Der Gesamtbetrag dieses Blattes.
    /// </summary>
    public decimal TotalAmount
    {
      get
      {
        return this.totalAmount;
      }

      set
      {
        if (this.totalAmount != value)
        {
          this.totalAmount = value;
          this.OnPropertyChanged(() => this.TotalAmount);
        }
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Erstellt eine Revision aus dem Aktuellen zustand.
    /// </summary>
    /// <returns>Die erstellte Revision.</returns>
    public SheetRevision MakeRevision()
    {
      SheetRevision revision = new SheetRevision();

      revision.Date = DateTime.Now;

      revision.Entrys.Clear();
      foreach (var entry in this.entrys)
      {
        revision.Entrys.Add(new SheetRevisionEntry(entry));
      }

      return revision;
    }

    /// <summary>
    /// Stellt eine Revision wieder her
    /// </summary>
    /// <param name="revision">Die wiederherzustellende Revision.</param>
    internal void RestoreRevision(SheetRevision revision)
    {
      this.entrys.Clear();
      foreach (var entry in revision.Entrys)
      {
        this.entrys.Add(new SheetEntry(entry));
      }

      this.Entrys = new ObservableCollection<SheetEntry>(revision.Entrys.Select(o => new SheetEntry(o)));
    }

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
    /// Wird aufgerufen wenn sich die Auflistung der Einträge geändert hat.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Entrys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      this.RecalculateBalance();

      if (e.NewItems != null)
      {
        foreach (SheetEntry entry in e.NewItems)
        {
          entry.PropertyChanged += this.Entry_PropertyChanged;
        }
      }

      this.OnPropertyChanged(() => this.Entrys);
    }

    /// <summary>
    /// Wird aufgerufen wenn sich eine Eigenschaft eines Eintrages geändert hat.
    /// </summary>
    /// <param name="sender">Der Sender.</param>
    /// <param name="e">Die Argumente.</param>
    private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == ((SheetEntry)sender).AmountName)
      {
        this.RecalculateBalance();
      }

      this.OnPropertyChanged(() => this.Entrys);
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

      decimal balance = this.Carry;
      foreach (var entry in this.entrys)
      {
        balance += entry.Amount;
        entry.Balance = balance;
      }

      this.Balance = balance;
      this.TotalAmount = this.Balance - this.Carry;

      this.currentlyRecalculatingBalance = false;
    }

    #endregion Methods
  }
}