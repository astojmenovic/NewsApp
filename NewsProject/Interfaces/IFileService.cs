using NewsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Interfaces
{
    public interface IFileService
    {
        void AddNewsJsonFile(News news);
        void UpdateNewsJsonFile(News news);
        void DeleteNewsJsonFile(News news);
    }
}
