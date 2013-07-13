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
using office365dnsoncall.ViewModels;

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
        private MobileServiceCollection<TodoItem, TodoItem> items;

        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            RefreshTodoItems();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock b = (TextBlock)sender;
            ItemViewModel vm = b.DataContext as ItemViewModel;

            PhoneCallTask task = new PhoneCallTask();
            task.PhoneNumber = vm.Phone;
            task.DisplayName = vm.Name;
            task.Show();
        }

        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock b = (TextBlock)sender;
            ItemViewModel vm = b.DataContext as ItemViewModel;

            EmailComposeTask ect = new EmailComposeTask();
            ect.To = vm.Email;
            ect.Subject = "Hey buddy, you got some shit";
            ect.Show();
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

            App.ViewModel.Items[0].Email = items[0].Email;
            App.ViewModel.Items[0].Name = items[0].Name;

            App.ViewModel.Items[1].Email = items[1].Email;
            App.ViewModel.Items[1].Name = items[1].Name;
        }
    }
    //public partial class MainPage : PhoneApplicationPage
    //{
    //    // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
    //    // is integrated with your Mobile Service to make it easy to bind your data to the ListView
    //    private MobileServiceCollection<TodoItem, TodoItem> items;

    //    private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

    //    private TodoItem primary = null;
    //    private TodoItem secondary = null;

    //    // Constructor
    //    public MainPage()
    //    {
    //        InitializeComponent();
    //    }

    //    private async void InsertTodoItem(TodoItem todoItem)
    //    {
    //        // This code inserts a new TodoItem into the database. When the operation completes
    //        // and Mobile Services has assigned an Id, the item is added to the CollectionView
    //        await todoTable.InsertAsync(todoItem);
    //        items.Add(todoItem);
    //    }

    //    private async void RefreshTodoItems()
    //    {
    //        // This code refreshes the entries in the list view be querying the TodoItems table.
    //        // The query excludes completed TodoItems
    //        try
    //        {
    //            items = await todoTable
    //                .ToCollectionAsync();
    //        }
    //        catch (MobileServiceInvalidOperationException e)
    //        {
    //            MessageBox.Show(e.Message, "Error loading items", MessageBoxButton.OK);
    //        }

    //        ListItems.ItemsSource = items;
    //        UpdatePrimary();

    //    }

    //    private async void UpdateCheckedTodoItem(TodoItem item)
    //    {
    //        // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
    //        // responds, the item is removed from the list 
    //        await todoTable.UpdateAsync(item);

    //        // To do: Need to make the checked item as Prinmary
    //    }

    //    private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
    //    {
    //        RefreshTodoItems();
    //    }

    //    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    //    {
    //        var todoItem = new TodoItem { Name = Name.Text, Email = Email.Text, Phone = Phone.Text, Order = Order.Text };
    //        InsertTodoItem(todoItem);
    //    }

    //    private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
    //    {
    //        CheckBox cb = (CheckBox)sender;
    //        TodoItem item = cb.DataContext as TodoItem;
    //        item.IsOnCall = item.IsOnCall ? false : true;
    //        UpdateCheckedTodoItem(item);
    //    }

    //    private void CallSecondary(object sender, RoutedEventArgs e)
    //    {
    //        PhoneCallTask task = new PhoneCallTask();
    //        task.DisplayName = primary.Name;
    //        task.PhoneNumber = "4259226398";
    //        task.Show();
    //    }

    //    private void CallPrimary(object sender, RoutedEventArgs e)
    //    {
    //        PhoneCallTask task = new PhoneCallTask();
    //        task.DisplayName = primary.Name;
    //        task.PhoneNumber = "4259226398";
    //        task.Show();
    //    }

    //    protected override void OnNavigatedTo(NavigationEventArgs e)
    //    {
    //        RefreshTodoItems();
    //    }

    //    private void UpdatePrimary()
    //    {
    //        foreach (TodoItem item in items)
    //        {
    //            if (item.IsOnCall && primary == null)
    //            {
    //                primary = item;
    //            }
    //            else if (item.IsOnCall && secondary == null)
    //            {
    //                secondary = item;
    //            }
    //        }

    //        PrimaryName.Content = "none";
    //        SecondaryName.Content = "none";
    //        if (primary != null)
    //        {
    //            PrimaryName.Content = primary.Name;
    //        }

    //        if (secondary != null)
    //        {
    //            SecondaryName.Content = secondary.Name;
    //        }

    //    }
    //}
}