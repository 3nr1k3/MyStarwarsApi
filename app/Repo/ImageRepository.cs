using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;
using MyStarwarsApi.Repo.Interfaces;

namespace MyStarwarsApi.Repo
{
    public class ImageRepository : IImageRepository
    {
        private readonly SqliteDbContext _dbContext;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(
            SqliteDbContext context,
            ILoggerFactory logger
        ){
            _dbContext = context;
            _logger = logger.CreateLogger<ImageRepository>();
        }

        public void addImage(Image image){
            _dbContext.Images.Add(image);

            _dbContext.SaveChanges();
        }

        public Image getAvatar(Guid id)
        {
            return _dbContext.Images.FirstOrDefault(i => i.id == id); 
        }

        public List<Image> getImagesByParent(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}