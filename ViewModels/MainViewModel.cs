using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using Date_task1.Models;

namespace Date_task1.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private DateModel _dateModel;
    private string _inputDate;
    private string _errorMessage;
    private int _numberOfUnits; 

    public int NumberOfUnits
    {
        get => _numberOfUnits;
        set
        {
            if (_numberOfUnits != value)
            {
                _numberOfUnits = value;
                OnPropertyChanged();
            }
        }
    }
    public string InputDate
    {
        get => _inputDate;
        set
        {
            if (_inputDate != value)
            {
                _inputDate = value;
                OnPropertyChanged();
            }
        }
    }

    public string CurrentDate
    {
        get
        {
            if (_dateModel != null)
                return $"{_dateModel.Day:D2}.{_dateModel.Month:D2}.{_dateModel.Year:D4}";
            return string.Empty;
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SetDateCommand { get; }
    public ICommand AddDayCommand { get; }
    public ICommand AddMonthCommand { get; }
    public ICommand AddYearCommand { get; }

    public MainViewModel()
    {
        SetDateCommand = new RelayCommand(SetDate);
        AddDayCommand = new RelayCommand(AddDay);
        AddMonthCommand = new RelayCommand(AddMonth);
        AddYearCommand = new RelayCommand(AddYear);
    }

    private void SetDate()
    {
        ErrorMessage = null; // Clear any previous error messages

        try
        {
            var parts = InputDate.Split('.');
            if (parts.Length != 3) 
            {
                ErrorMessage = "Invalid date format. Please use dd.MM.yyyy.";
                return; 
            }

            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);

            if (!DateModel.IsValidDate(day, month, year))
            {
                ErrorMessage = "Invalid date. Please enter a valid date.";
                return;
            }

            _dateModel = new DateModel(day, month, year);
        }
        catch (FormatException)
        {
            ErrorMessage = "Invalid date format. Please use dd.MM.yyyy.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message; // Or a more user-friendly message
        }

        OnPropertyChanged(nameof(CurrentDate));
    }


    private async void AddDay()
    {
        await Dispatcher.UIThread.InvokeAsync(() => 
        { 
            _dateModel?.AddDays(NumberOfUnits); 
            OnPropertyChanged(nameof(CurrentDate));
        });
    }

    private async void AddMonth()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _dateModel?.AddMonths(NumberOfUnits);
            OnPropertyChanged(nameof(CurrentDate));
        });
    }

    private async void AddYear()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _dateModel?.AddYears(NumberOfUnits); 
            OnPropertyChanged(nameof(CurrentDate));
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

