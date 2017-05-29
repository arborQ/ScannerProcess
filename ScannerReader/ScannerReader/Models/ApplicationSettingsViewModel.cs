using ScannerReader.Converters;
using System;
using System.ComponentModel;
using System.Linq;

namespace ScannerReader.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ModeOptions
    {
        [Description("Numer zamówienia")]
        OrderNumber = 0,
        [Description("Numer tabliczki")]
        BlockNumber = 1,
        [Description("Nr. zam i nr. tab")]
        Both = 2
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OptionEnabled
    {
        [Description("Tak")]
        Yes = 1,
        [Description("Nie")]
        No = 0,
    }

    public class ApplicationSettingsViewModel : BaseObservableModel
    {
        private string _imagePath;
        private string _ipAddress;
        private int _activityTimeout;
        private ModeOptions _selectedScanMode;
        private OptionEnabled _drilEnabled;

        public ApplicationSettingsViewModel()
        {

        }

        public ModeOptions[] Options => Enum.GetValues(typeof(ModeOptions)).Cast<ModeOptions>().ToArray();
        public OptionEnabled[] DrilOptions => Enum.GetValues(typeof(OptionEnabled)).Cast<OptionEnabled>().ToArray();

        public OptionEnabled DrilEnabled
        {
            get { return _drilEnabled; }
            set
            {
                _drilEnabled = value;
                OnPropertyChanged();
            }
        }

        public ModeOptions SelectedMode
        {
            get { return _selectedScanMode; }
            set
            {
                _selectedScanMode = value;
                OnPropertyChanged();
            }
        }

        public int ActivityTimeout
        {
            get { return _activityTimeout; }
            set
            {
                _activityTimeout = value;
                OnPropertyChanged();
            }
        }
        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }
    }
}