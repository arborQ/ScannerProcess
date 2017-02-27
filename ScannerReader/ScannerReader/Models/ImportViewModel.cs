using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RepositoryServices.Models;
using ScannerReader.Resources;

namespace ScannerReader.Models
{
    public class ImportViewModel : BaseObservableModel
    {
        private IList<Machine> _machines;
        private long _processedMachines;
        private string _imageBasePath;

        public IList<Machine> Machines
        {
            get { return _machines; }
            set
            {
                _machines = value;
                OnPropertyChanged(nameof(IsDataLoaded));
                OnPropertyChanged(nameof(ImportErrors));
                OnPropertyChanged(nameof(ValidateCodeIsUniq));
                OnPropertyChanged(nameof(ValidateEngineCodes));
                OnPropertyChanged(nameof(ValidateImagesExists));
                OnPropertyChanged(nameof(IsModelValid));
                OnPropertyChanged(nameof(ProcessedMachines));
                OnPropertyChanged(nameof(MachineCount));
            }
            
        }

        public IEnumerable<string> ImportErrors
        {
            get
            {
                if (IsDataLoaded)
                    return ValidateModel();
                return Enumerable.Empty<string>();
            }
        }

        public string ImageBasePath
        {
            get { return _imageBasePath; }
            set
            {
                _imageBasePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValidateImagesExists));
                OnPropertyChanged(nameof(IsDataLoaded));
            }
        }

        public bool IsDataNotLoaded => Machines == null;
        public bool IsDataLoaded => Machines != null && !string.IsNullOrEmpty(ImageBasePath);

        public bool IsModelValid => IsDataLoaded && !ImportErrors.Any();

        public string ValidateCodeIsUniq
        {
            get
            {
                if (IsDataLoaded)
                {
                    var grouped = Machines
                        .GroupBy(m => m.Code)
                        .Where(a => a.Count() > 1)
                        .ToList();

                    if (grouped.Any())
                        return string.Join(", ", grouped.Select(g => g.Key));
                }

                return string.Empty;
            }
        }

        public string ValidateEngineCodes
        {
            get
            {
                if (IsDataLoaded)
                {
                    var withoutCode =
                        Machines.Where(m => string.IsNullOrEmpty(m.EngineCodeA) && string.IsNullOrEmpty(m.EngineCodeB))
                            .ToList();

                    if (withoutCode.Any())
                        return string.Format(MachineImportResources.NoEngineCodeProvided, nameof(Machine.EngineCodeA),
                            nameof(Machine.EngineCodeB));
                }
                return string.Empty;
            }
        }

        public long ProcessedMachines
        {
            get { return _processedMachines; }
            set
            {
                _processedMachines = value;
                OnPropertyChanged();
            }
        }

        public long MachineCount => IsDataLoaded ? Machines.Count : 0;

        public string ValidateImagesExists
        {
            get
            {

                if (IsDataLoaded)
                {
                    var filePaths = Machines
                        .SelectMany(m => new[] {m.ImageA, m.ImageB, m.ImageC})
                        .Where(i => !string.IsNullOrEmpty(i))
                        .Distinct()
                        .Select(i => Path.Combine(ImageBasePath, i))
                        .ToList();

                       var notExistingImages = filePaths.Where(i => !File.Exists(i))
                        .ToList();

                    if (notExistingImages.Any())
                        return string.Join(",", notExistingImages);
                }
                return string.Empty;
            }
        }

        private IEnumerable<string> ValidateModel()
        {
            return new[]
                {
                    ValidateCodeIsUniq, ValidateEngineCodes, ValidateImagesExists
                }
                .Where(s => !string.IsNullOrEmpty(s));
            ;
        }
    }
}