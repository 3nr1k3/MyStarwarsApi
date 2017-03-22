using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using MyStarwarsApi.Helpers;
using MyStarwarsApi.Repo.Interfaces;
using Newtonsoft.Json;

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
        [JsonIgnore]
        [Key]
        public Guid id {get;set;}
        [JsonIgnore]
        public String format {get;set;}
        [JsonIgnore]
        public image_type imageType{get;set;}
        public String url{get;set;}

        public Image()
        {
        }
        public Image(IImageRepository repository)
        {
            Guid newId;
            Image avatar;
            var count = 0;

            do
            {
                newId = Guid.NewGuid();
                avatar = repository.getAvatar(newId);
                count++;
            }while( avatar != null && count < 3);

            this.id = newId;
        }

        public String getFullPath()
        {
            if(imageType != image_type.Default)
                return $"./images/{Enum.GetName(typeof(image_type), imageType)}/{id}.{format.ToString()}";
                else
                return $"./images/{id}.{format.ToString()}";
        }

        public String imageServerPath(String webRootPath)
        {
            return $"{webRootPath}/images/{Enum.GetName(typeof(image_type), imageType)}/{id.ToString().Substring(0,8)}.{format}";
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

            public async Task<Builder> fromBitmapAsync(Bitmap avatarBitmap, IHostingEnvironment env)
            {
                String ip = await NetworkHelper.getIpAddress();

                //_image.url = $"http://{ip}:5000/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0, 8)}.{_image.format}";
                _image.url = $"http://172.20.17.7:5000/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0, 8)}.{_image.format}";

                avatarBitmap.Save(
                    $"{env.WebRootPath}/images/{Enum.GetName(typeof(image_type), _image.imageType)}/{_image.id.ToString().Substring(0, 8)}.{_image.format}",
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