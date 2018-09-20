﻿using CoPiloto.Extentions;
using CoPiloto.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoPiloto.ViewModels
{
    public class ListFlyChartsViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<Approach> MyCharts { get; }

        public Command ChartSelectedCommand { get; }

        Info info;

        #endregion

        public async override Task InitializeAsync(object[] args)
        {
            info      = (Info)args[0];
            var chart = (Charts)args[1];

            if (chart is null)
            {
                await DisplayAlert("Aviso", $"Nenhuma carta encontrada para o {info.Name}");
                await Navigation.PopAsync();
                return;
            }

            var teste = chart.Approach;

            MyCharts.AddRange(chart.Approach);
            MyCharts.AddRange(chart.Sid);
            MyCharts.AddRange(chart.General);
            MyCharts.AddRange(chart.Star);

        }

        protected override void MyChangeCanExecute() => 
            ChartSelectedCommand.ChangeCanExecute();

        public ListFlyChartsViewModel()
        {
            ChartSelectedCommand = new AsyncCommand<string>(ExecuteSelectedChartCommand, IsBusyStatus);
            MyCharts             = new ObservableCollection<Approach>();
        }

        async Task ExecuteSelectedChartCommand(string url)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await Navigation.PushAsync<ChartViewModel>(url);
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Erro", $"Erro:{ex.Message}", "Ok");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            return;
        }
    }
}
