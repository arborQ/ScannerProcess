﻿using RepositoryServices.Interfaces;

namespace RepositoryServices.Models
{
    public class Settings : IBaseElement
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public int DefaultTimeout { get; set; }

        public int DrilEnabled { get; set; }

        public int SelectedMode { get; set; }

        public string IpAddress { get; set; }
    }
}