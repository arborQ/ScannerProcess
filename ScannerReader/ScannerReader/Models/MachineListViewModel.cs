using System.Collections.Generic;
using System.Collections.ObjectModel;
using RepositoryServices.Models;

namespace ScannerReader.Models
{
    public class MachineListViewModel : BaseObservableModel
    {
        private ObservableCollection<Machine> _machines;

        public ObservableCollection<Machine> Machines
        {
            get { return _machines; }
            set
            {
                _machines = value;
                OnPropertyChanged();
            }
        }
    }
}