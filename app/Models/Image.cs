using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using MyStarwarsApi.Helpers;
using MyStarwarsApi.Repo.Interfaces;

namespace MyStarwarsApi.Models
{
    public enum image_type
    {
        Default = 0,
        Character,
        Ship,
        Planet
    }

    public class Image
    {
        [Key]
        public Guid id {get;set;}
        public String format {get;set;}
        public image_type imageType{get;set;}
        public String url{get;set;}

        public Image()
        {
        }
        public Image(IImageRepository repository)
        {
            Guid newId;
            var count = 0;
            do
            {
                newId = Guid.NewGuid();
                count++;
            }while(repository.getAvatar(newId) != null || count < 3);

            this.id = newId;
        }

        public String getFullPath()
        {
            if(imageType != image_type.Default)
                return $"./images/{Enum.GetName(typeof(image_type), imageType)}/{id}.{format.ToString()}";
                else
                return $"./images/{id}.{format.ToString()}";
        }

        public class Builder
        {
            private Image _image;
            public Builder(IImageRepository repository)
            {
                if(_image == null)
                    _image = new Image(repository);
            }

            public Builder setType(image_type type)
            {
                _image.imageType = type;
                return this;
            }

            public Builder setFormat(ImageFormat ext)
            {
                _image.format = ext.ToString();
                return this;
            }

            public Builder fromBitmap(Bitmap avatarBitmap, IHostingEnvironment env)
            {
                Console.WriteLine("*************************************************************");
                Console.WriteLine(Directory.Exists($"{env.WebRootPath}"));
                Console.WriteLine($"{env.WebRootPath}/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0,10)}");
                Console.WriteLine(ImageHelper.getImageFormatFromString(_image.format));
                Console.WriteLine("*************************************************************");

                _image.url = $"http://172.20.17.7:5000/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0,10)}.{_image.format}";

                avatarBitmap.Save(
                    $"{env.WebRootPath}/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0,10)}.{_image.format}",
                    ImageHelper.getImageFormatFromString(_image.format)
                );
                return this;
            }

            public Image build()
            {
                return _image;
            }
        }
    }
}