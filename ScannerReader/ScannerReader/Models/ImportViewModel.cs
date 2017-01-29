﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RepositoryServices.Models;

namespace ScannerReader.Models
{
    public class ImportViewModel : BaseObservableModel
    {
        private IList<Machine> _machines;
        private long _processedMachines;

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

        public bool IsDataNotLoaded => Machines == null;
        public bool IsDataLoaded => Machines != null;

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
                    if (Machines.Any(m => string.IsNullOrEmpty(m.EngineCodeA) && string.IsNullOrEmpty(m.EngineCodeB)))
                        return " Empty a i b";
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
                    var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty;

                    var notExistingImages = Machines
                        .SelectMany(m => new[] {m.ImageA, m.ImageB, m.ImageC})
                        .Where(i => !string.IsNullOrEmpty(i))
                        .Distinct()
                        .Where(i => !File.Exists(Path.Combine(directory, i)))
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