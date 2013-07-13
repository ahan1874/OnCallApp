using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using office365dnsoncall.Resources;
using Microsoft.Phone.Tasks;

namespace office365dnsoncall
{
    public class TodoItem
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "oncall")]
        public bool IsOnCall { get; set; }

        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
        // is integrated with your Mobile Service to make it easy to bind your data to the ListView
        private MobileServiceCollection<TodoItem, TodoItem> items;

        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        private TodoItem primary = null;
        private TodoItem secondary = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private async void InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);
        }

        private async void RefreshTodoItems()
        {
            // This code refreshes the entries in the list view be querying the TodoItems table.
            // The query excludes completed TodoItems
            try
            {
                items = await todoTable
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                MessageBox.Show(e.Message, "Error loading items", MessageBoxButton.OK);
            }

            ListItems.ItemsSource = items;
            UpdatePrimary();

        }

        private async void UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);

            // To do: Need to make the checked item as Prinmary
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTodoItems();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var todoItem = new TodoItem { Name = Name.Text, Email = Email.Text, Phone = Phone.Text, Order = Order.Text };
            InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            item.IsOnCall = item.IsOnCall ? false : true;
            UpdateCheckedTodoItem(item);
        }

        private void CallSecondary(object sender, RoutedEventArgs e)
        {
            PhoneCallTask task = new PhoneCallTask();
            task.DisplayName = primary.Name;
            task.PhoneNumber = "4259226398";
            task.Show();
        }

        private void CallPrimary(object sender, RoutedEventArgs e)
        {
            PhoneCallTask task = new PhoneCallTask();
            task.DisplayName = primary.Name;
            task.PhoneNumber = "4259226398";
            task.Show();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshTodoItems();
        }

        private void UpdatePrimary()
        {
            foreach (TodoItem item in items)
            {
                if (item.IsOnCall && primary == null)
                {
                    primary = item;
                }
                else if (item.IsOnCall && secondary == null)
                {
                    secondary = item;
                }
            }

            PrimaryName.Content = "none";
            SecondaryName.Content = "none";
            if (primary != null)
            {
                PrimaryName.Content = primary.Name;
            }

            if (secondary != null)
            {
                SecondaryName.Content = secondary.Name;
            }

        }
    }
}