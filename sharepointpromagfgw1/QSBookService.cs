using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace sharepointpromagfgw1
{
    public class QSBookService
    {
        static QSBookService instance = new QSBookService();

        const string applicationURL = @"https://sharepointpromagfgw1.azure-mobile.net/";
        const string applicationKey = @"KVEbrfOtyyNEqobEMGZhVPpOumvovM88";
        const string localDbPath = "localstore.db";

        private MobileServiceClient client;
        private IMobileServiceSyncTable<BookItem> bookTable;

        private QSBookService()
        {
            CurrentPlatform.Init();
            SQLitePCL.CurrentPlatform.Init();

            // Initialize the Mobile Service client with your URL and key
            client = new MobileServiceClient(applicationURL, applicationKey);

            // Create an MSTable instance to allow us to work with the BookItem table
            bookTable = client.GetSyncTable<BookItem>();
        }

        public static QSBookService DefaultService
        {
            get
            {
                return instance;
            }
        }

        public List<BookItem> Items { get; private set; }

        public async Task InitializeStoreAsync()
        {
            var store = new MobileServiceSQLiteStore(localDbPath);
            store.DefineTable<BookItem>();

            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store);
        }

        public async Task SyncAsync()
        {
            try
            {
                await client.SyncContext.PushAsync();
                await bookTable.PullAsync("allBookItems", bookTable.CreateQuery()); // query ID is used for incremental sync
            }

            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"Sync Failed: {0}", e.Message);
            }
        }

        public async Task<List<BookItem>> RefreshDataAsync()
        {
            try
            {
                // update the local store
                // all operations on bookTable use the local database, call SyncAsync to send changes
                await SyncAsync();

                // This code refreshes the entries in the list view by querying the local BookItems table.
                // The query excludes completed BookItems
                // I am making a change from bookItem.Complete to bookItem.RepayLoan
                Items = await bookTable
                        .Where(bookItem => bookItem.RepayLoan == false).ToListAsync();

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return null;
            }

            return Items;
        }

        public async Task InsertBookItemAsync(BookItem bookItem)
        {
            try
            {
                await bookTable.InsertAsync(bookItem); // Insert a new BookItem into the local database. 
                await SyncAsync(); // send changes to the mobile service

                Items.Add(bookItem);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }

        public async Task CompleteItemAsync(BookItem item)
        {
            try
            {
                item.Complete = true;
                await bookTable.UpdateAsync(item); // update book item in the local database
                await SyncAsync(); // send changes to the mobile service

                Items.Remove(item);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                Console.Error.WriteLine(@"ERROR {0}", e.Message);
            }
        }
    }
}