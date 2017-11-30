﻿using LibraryAppCore.XF.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraryAppCore.XF.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BooksListPage : ContentPage
	{
        BooksListViewModel viewModel;

        public BooksListPage()
        {
            InitializeComponent();
            viewModel = new BooksListViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await viewModel.LoadBooks();
            base.OnAppearing();
        }

        private async Task CheckLocalDataSynced(object sender, EventArgs e)
        {
            viewModel.LoadLocalData();

            if (viewModel.BooksLocalData.Count > 0 && App.ConnectionType != "No Connection")
            {
                bool answer = await DisplayAlert("Sync local data", "You are have " + viewModel.BooksLocalData.Count + " not synced items", "Sync Now", "Sync Later");

                if (answer)
                {
                    viewModel.SyncedLocalData();
                }
            }
            else
            {
                await DisplayAlert("Save Data", "You don't have internet connection, or local data is empty! Please try again leater", "OK");
            }
        }

    }
}