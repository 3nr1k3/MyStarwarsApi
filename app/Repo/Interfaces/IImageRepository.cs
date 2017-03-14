using System;
using System.Collections.Generic;
using MyStarwarsApi.Models;

namespace MyStarwarsApi.Repo.Interfaces
{
    public interface IImageRepository
    {
        /**
            
         */
        void addImage(Image image);
        Image getAvatar(Guid id);
        List<Image> getImagesByParent(Guid id);
        /*Image getParentAvatar(Guid id);*/
    }
}