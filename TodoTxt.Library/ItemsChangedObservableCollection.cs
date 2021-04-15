using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace TodoTxt.Library
{
    /// <summary>
    ///     This class adds the ability to refresh the list when any property of
    ///     the objects changes in the list which implements the INotifyPropertyChanged. 
    /// </summary>
    /// <typeparam name="T">
    public class ItemsChangedObservableCollection<T> : 
           ObservableCollection<T> where T : INotifyPropertyChanged
    {
        #region Destructor

        ~ItemsChangedObservableCollection()
        {
            UnRegisterPropertyChanged(this);
        }

        #endregion

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RegisterPropertyChanged(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                UnRegisterPropertyChanged(e.OldItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                UnRegisterPropertyChanged(e.OldItems);
                RegisterPropertyChanged(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                UnRegisterPropertyChanged(this);
                RegisterPropertyChanged(this);
            }
            
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            UnRegisterPropertyChanged(this);
            base.ClearItems();
        }

        protected void RegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        protected void UnRegisterPropertyChanged(IList items)
        {
            foreach (INotifyPropertyChanged item in items)
            {
                if (item != null)
                {
                    item.PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        protected virtual void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void ReplaceItem(T itemToReplace, T replacementItem)
        {
            var index = this.IndexOf(itemToReplace);
            this.Items[index] = replacementItem;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, this.Items[index], replacementItem));
        }

        public virtual void ReplaceItems(IList<T> itemsToReplace, IList<T> replacementItems)
        {
            if (itemsToReplace == null)
            {
                throw new ArgumentNullException("itemsToReplace");
            }

            if (replacementItems == null)
            {
                throw new ArgumentNullException("replacementItems");
            }

            if (itemsToReplace.Count != replacementItems.Count)
            {
                throw new OverflowException("itemsToReplace count does not equal replacementItems count");
            }

            // These lists are to be passed to the OnCollectionChanged event handler.
            var newItems = new List<T>(replacementItems.Count);
            var oldItems = new List<T>(itemsToReplace.Count);

            for (int i = 0; i < itemsToReplace.Count; i++)
            {
                var index = this.IndexOf((T)itemsToReplace[i]);
                oldItems.Add(this.Items[index]);
                this.Items[index] = replacementItems[i];
                newItems.Add(this.Items[index]);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldItems, newItems));
        }
    }
}
