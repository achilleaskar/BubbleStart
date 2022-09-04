using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class ItemsManagement_ViewModel : AddEditBase<ItemWrapper, Item>
    {
        public ItemsManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Προϊόντων";
        }

        //public override void AddedItem(Item entity, bool removed)
        //{
        //    base.AddedItem(entity, removed);
        //    if (entity is Item e)
        //    {
        //        if (removed)
        //        {
        //            if (Parents.Contains(e))
        //            {
        //                Parents.Remove(e);
        //            }
        //        }
        //        else
        //        {
        //            Parents.Add(e);
        //        }
        //    }
        //}

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<ItemWrapper>(Context.Items.Select(p => new ItemWrapper(p)));
        }

        public override async Task ReloadAsync()
        {
            MainCollection = new ObservableCollection<ItemWrapper>(Context.Items.Where(p => p.Id > 1).Select(p => new ItemWrapper(p)));

        }
    }
}