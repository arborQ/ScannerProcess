using System.Collections.Generic;
using System.Collections.ObjectModel;
using RepositoryServices.Models;

namespace ScannerReader.Models
{
    public class MachineListViewModel : BaseObservableModel
    {
        private List<Machine> _machines;

        public List<Machine> Machines
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