using RepositoryServices.Interfaces;

namespace RepositoryServices.Models
{
    public class Settings : IBaseElement
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public int DefaultTimeout { get; set; }
    }
}