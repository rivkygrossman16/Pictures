using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pictures.Data
{
    public class PictureDataRepo
    {
        private string _connectionString;

        public PictureDataRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Picture picture)
        {
            using var context = new PictureDataContext(_connectionString);
            context.Pictures.Add(picture);
            context.SaveChanges();
        }
        public List<Picture> AllPictures()
        {
            using var context = new PictureDataContext(_connectionString);
            return context.Pictures.ToList();
        }
        public Picture GetById(int id)
        {
            using var context = new PictureDataContext(_connectionString);
            return context.Pictures.FirstOrDefault(p => p.Id == id);
        }

        public void Update(int id)
        {
            using var context = new PictureDataContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"Update Pictures Set NumberOfLikes=NumberOfLikes+1 WHERE Id={id}");
            context.SaveChanges();
        }
    }
}
