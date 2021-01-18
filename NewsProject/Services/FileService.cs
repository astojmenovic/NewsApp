using AutoMapper;
using Microsoft.Extensions.Configuration;
using NewsProject.DTOs;
using NewsProject.Interfaces;
using NewsProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public FileService(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public void AddNewsJsonFile(News news)
        {
            string filePath = _config["FilePath"];
            var newsDto = _mapper.Map<NewsDto>(news);
            var newsList = new List<NewsDto>();
            if (System.IO.File.Exists(filePath))
            {
                string text = System.IO.File.ReadAllText(filePath);
                newsList = JsonConvert.DeserializeObject<List<NewsDto>>(text);
                newsList.Add(newsDto);
                string output = JsonConvert.SerializeObject(newsList, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, output);
            }
            else
            {
                newsList.Add(newsDto);
                string output = JsonConvert.SerializeObject(newsList, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, output);
            }
            
        }

        public void DeleteNewsJsonFile(News news)
        {
            string filePath = _config["FilePath"];
            var newsList = new List<NewsDto>();
            var newsDto = _mapper.Map<NewsDto>(news);
            string text = System.IO.File.ReadAllText(filePath);
            newsList = JsonConvert.DeserializeObject<List<NewsDto>>(text);

            var itemToRemove = newsList.SingleOrDefault(r => r.Id == news.Id);
            if (itemToRemove != null)
                newsList.Remove(itemToRemove);

            string output = JsonConvert.SerializeObject(newsList, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, output);
        }

        public void UpdateNewsJsonFile(News news)
        {
            string filePath = _config["FilePath"];
            var newsList = new List<NewsDto>();
            var newsDto = _mapper.Map<NewsDto>(news);
            string text = System.IO.File.ReadAllText(filePath);
            newsList = JsonConvert.DeserializeObject<List<NewsDto>>(text);

            foreach (NewsDto newsToUpdate in newsList)
            {
                if (newsToUpdate.Id == newsDto.Id)
                {
                    newsToUpdate.Category = newsDto.Category;
                    newsToUpdate.Description = newsDto.Description;
                    newsToUpdate.Name = newsDto.Name;
                }
            }
            string output = JsonConvert.SerializeObject(newsList, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, output);
        }
    }
}
