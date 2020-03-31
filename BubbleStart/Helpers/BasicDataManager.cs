using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BubbleStart.Helpers
{
    public class BasicDataManager : ViewModelBase
    {
        public BasicDataManager(GenericRepository genericRepository)
        {
            Context = genericRepository;
            RefreshCommand = new RelayCommand(async () => await Refresh());
        }


        public bool HasChanges()
        {
            return Context.HasChanges();
        }



        private ObservableCollection<Customer> _Customers;


        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<District> _Districts;


        public ObservableCollection<District> Districts
        {
            get
            {
                return _Districts;
            }

            set
            {
                if (_Districts == value)
                {
                    return;
                }

                _Districts = value;
                RaisePropertyChanged();
            }
        }
        public async Task Refresh()
        {
            var oldContext = Context;
            Context = new GenericRepository();
            oldContext.Dispose();
            await LoadAsync();
        }
        internal void Delete<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Delete(model);
            if (model is Customer c)
            {
                Customers.Remove(c);
            }

        }

        internal async Task SaveAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;

        }
        private ObservableCollection<User> _Users;
        private ObservableCollection<Customer> _TodaysApointments;

        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> TodaysApointments
        {
            get
            {
                return _TodaysApointments;
            }

            set
            {
                if (_TodaysApointments == value)
                {
                    return;
                }

                _TodaysApointments = value;
                RaisePropertyChanged();
            }
        }

        internal void Add<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Add(model);
        }

        public async Task LoadAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Users = new ObservableCollection<User>(await Context.GetAllAsync<User>());
            Districts = new ObservableCollection<District>((await Context.GetAllAsync<District>()).OrderBy(d => d.Name));
            Customers = new ObservableCollection<Customer>((await Context.LoadAllCustomersAsync()));

            StaticResources.User = StaticResources.User != null ? Users.FirstOrDefault(u => u.Id == StaticResources.User.Id) : null;
            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());

            foreach (var c in Customers)
            {
                c.PropertyChanged += C_PropertyChanged;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void C_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        public RelayCommand RefreshCommand { get; set; }
        public GenericRepository Context { get; set; }

        internal void RollBack()
        {
            Context.RollBack();
        }
    }
}