using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using GalaSoft.MvvmLight.CommandWpf;

namespace BubbleStart.ViewModels
{
    public abstract class AddEditBase<TWrapper, TEntity> : MyViewModelBaseAsync
        where TEntity : BaseModel, new()
        where TWrapper : ModelWrapper<TEntity>, new()
    {
        #region Constructors

        public AddEditBase(BasicDataManager context)
        {
            AddEntityCommand = new RelayCommand(AddEntity, CanAddEntity);
            UpdateEntitiesCommand = new RelayCommand(async () => { await ReloadEntities(); }, true);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClearEntityCommand = new RelayCommand(ClearEntity, CanClearEntity);
            RemoveEntityCommand = new RelayCommand(RemoveEntity, CanRemoveEntity);

            MainCollection = new ObservableCollection<TWrapper>();

            Context = context;

            SelectedEntity = new TWrapper();
        }

        private void MainCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TWrapper item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TWrapper item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<TWrapper> _MainCollection;

        private string _ResultMessage;

        private TWrapper _SelectedEntity;

        #endregion Fields

        private string _ControlName;

        public string ControlName
        {
            get => _ControlName;

            set
            {
                if (_ControlName == value)
                {
                    return;
                }

                _ControlName = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        /// <summary>
        /// Adds current entity to database and saves
        /// </summary>
        public RelayCommand AddEntityCommand { get; }

        /// <summary>
        /// Creates a new entity clearing all custom values
        /// </summary>
        public RelayCommand ClearEntityCommand { get; }

        public BasicDataManager Context { get; set; }

        /// <summary>
        /// Sets and gets the MainCollection property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<TWrapper> MainCollection
        {
            get => _MainCollection;

            set
            {
                if (_MainCollection == value)
                {
                    return;
                }

                _MainCollection = value;
                MainCollection.CollectionChanged += MainCollectionChanged;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Removes selected entity from Database
        /// </summary>
        public RelayCommand RemoveEntityCommand { get; }

        public string ResultMessage
        {
            get => _ResultMessage;

            set
            {
                if (_ResultMessage == value)
                {
                    return;
                }

                _ResultMessage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Saves changes done to all entities since last udpate
        /// </summary>
        public RelayCommand SaveChangesCommand { get; }

        /// <summary>
        /// Sets and gets the SelectedEntity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public TWrapper SelectedEntity
        {
            get => _SelectedEntity;

            set
            {
                if (_SelectedEntity == value)
                {
                    return;
                }
                if (value == null)
                {
                    _SelectedEntity = new TWrapper();
                }

                _SelectedEntity = value;
                SelectedEntityChanged();
                RaisePropertyChanged();
            }
        }

        public virtual void SelectedEntityChanged()
        {
        }

        /// <summary>
        ///Updates the list of entities with latset Db values
        /// </summary>
        public RelayCommand UpdateEntitiesCommand { get; }

        #endregion Properties

        #region Methods

        public virtual bool CanSaveChanges()
        {
            return Context.HasChanges() && SelectedEntity != null && !SelectedEntity.HasErrors;
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ResultMessage = "";
        }

        public async virtual void SaveChanges()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await Context.SaveAsync();
                SelectedEntity = new TWrapper();
                ResultMessage = "Όι αλλαγές αποθηκεύτηκαν επιτυχώς";
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public virtual void AddedItem(TEntity entity, bool removed)
        {

        }

        private void AddEntity()
        {
            try
            {
                if (SelectedEntity is ExpenseCategoryClassWrapper e && e.Parent?.Id == -1)
                {
                    e.Parent = null;
                }
                Context.Add(SelectedEntity.Model);
                MainCollection.Add(SelectedEntity);
                AddedItem(SelectedEntity.Model, false);
                //ClearEntity();
                ResultMessage = SelectedEntity.Title + " προστέθηκε επιτυχώς";
                RemoveEntityCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public virtual bool CanAddEntity()
        {
            return SelectedEntity != null && !SelectedEntity.HasErrors && SelectedEntity.Id == 0;
        }

        private bool CanClearEntity()
        {
            return SelectedEntity != null;//&& SelectedEntity.HasValues();
        }

        private bool CanRemoveEntity()
        {
            return SelectedEntity != null && SelectedEntity.Id > 0;
        }

        private void ClearEntity()
        {
            SelectedEntity = new TWrapper();
            ResultMessage = "";
        }

        private async Task ReloadEntities()
        {
            try
            {
                ResultMessage = "";
                await Context.Refresh();
                if (SelectedEntity?.Id > 0)
                {
                    SelectedEntity = new TWrapper();
                }
                await LoadAsync();
                ResultMessage = "Η ενημέρωση ολοκληρώθηκε!";
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public virtual async void RemoveEntity()
        {
            try
            {
                if (SelectedEntity is ExpenseCategoryClassWrapper ec && ec.Id > 0 && (
                    ec.Id == 1 ||
                    ((ec.Parent == null || ec.ParentId == 1) && await Context.Context.Context.ExpenseCategoryClasses.AnyAsync(e => e.ParentId == ec.Id)) ||
                    (await Context.Context.Context.Expenses.AnyAsync(e => e.MainCategoryId == ec.Id || e.SecondaryCategoryId == ec.Id))))
                {
                    MessageBox.Show("Δεν μπορεί να διαγραφεί");
                    return;
                }
                else if (SelectedEntity is Item it && it.Id > 0 &&
                    (await Context.Context.Context.ItemPurchases.AnyAsync(e => e.ItemId == it.Id)))
                {
                    MessageBox.Show("Δεν μπορεί να διαγραφεί");
                    return;
                }
                AddedItem(SelectedEntity.Model, true);
                Context.Delete(SelectedEntity.Model);
                MainCollection.Remove(SelectedEntity);
                if (MainCollection.Count > 0)
                {
                    SelectedEntity = MainCollection.Last();
                }
                else
                    SelectedEntity = new TWrapper();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}