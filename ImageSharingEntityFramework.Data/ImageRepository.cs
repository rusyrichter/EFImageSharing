using Microsoft.EntityFrameworkCore;
using System;

namespace ImageSharingEntityFramework.Data
{
    public class ImageRepository
    {
        private string _connectionString;
        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetImages()
        {
            using var context = new ImageDBContext(_connectionString);
            return context.Images.ToList();
        }
        public void AddImage(Image image)
        {
            using var context = new ImageDBContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();        
        }
        public Image GetById(int id)
        {
            using var context = new ImageDBContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public void UpdateLikes(int id)
        {
            using var context = new ImageDBContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"Update Images set Likes = Likes + 1 Where Id = {id}");
        }
    }
}