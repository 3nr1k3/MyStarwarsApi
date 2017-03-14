using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Drawing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;

using AutoMapper;

using MyStarwarsApi.Models;
using MyStarwarsApi.Repo.Interfaces;
using MyStarwarsApi.Models.ViewModel;
using MyStarwarsApi.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace MyStarwarsApi.Controllers
{
    /// <summary>  </summary>
    /// <remarks>  </remarks>
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<CharactersController> _logger;
        private readonly IHostingEnvironment _env;

        public CharactersController(
            ILoggerFactory logger,
            ICharacterRepository characterRepository,
            IImageRepository imageRepository,
            IHostingEnvironment env
            ){
            _characterRepository = characterRepository;
            _imageRepository = imageRepository;
            _logger = logger.CreateLogger<CharactersController>();
            _env = env;
        }

        // GET api/characters
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Character>), 200)]
        //public IEnumerable<Character> Get(
        public IActionResult Get(
            [FromHeader] String containsName,
            [FromHeader] String containsSide,
            [FromHeader] int skip,
            [FromHeader] int take
        ){
            
            List<Character> chars = _characterRepository.getCharacters();
            
            _logger.LogInformation($"---------------------->> {HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress}");
            
            if(containsName != null)
                chars = chars.Where(c => c.name.ToLower().Contains(containsName.ToLower())).ToList();

            if(containsSide != null)
                chars = chars.Where(c => c.side.ToLower().Contains(containsSide.ToLower())).ToList();

            chars = chars.Skip(skip).ToList();

            if(take > 0)
                chars = chars.Take(take).ToList();
            else
                chars = chars.Take(100).ToList();

            chars.ForEach(c => c.avatar = _imageRepository.getAvatar(c.avatarId));

            return Json(chars);
        }

        // GET api/characters/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public Character Get(Guid id)
        {
            return _characterRepository.getCharacter(id);
        }

        // POST api/characters
        [HttpPost]
        public HttpResponseMessage Post([FromBody]CharacterCreateViewModel newCharacter)
        {
            List<Character> charsKilled = new List<Character>();
            Character characterToCreate = new Character();

            if(newCharacter.charactersKilled != null){
                newCharacter.charactersKilled.ToList().ForEach(c => {
                    charsKilled.Add(_characterRepository.getCharacter(c));
                });
            }

            characterToCreate = Mapper.Map<CharacterCreateViewModel, Character>(newCharacter);

            if( newCharacter.avatar != "" )
            {
                Bitmap bitImage = ImageHelper.bitmapFromBase64String(newCharacter.avatar);

                Models.Image.Builder img = new Models.Image.Builder(_imageRepository);
                img.setFormat(ImageHelper.getImageFormatFromBase64String(newCharacter.avatar));
                img.setType(image_type.Character);
                img.fromBitmap(bitImage, _env);
                var image = img.build();
                //characterToCreate.avatar = img.build();
                characterToCreate.avatarId = image.id;
                _imageRepository.addImage(image);
            }
            
            characterToCreate.charactersKilled = charsKilled;

            _characterRepository.addCharacter(characterToCreate);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        // PUT api/characters/5
        [HttpPut("{id}")]
        public void Put(int Guid, [FromBody]string value)
        {
        }

        // DELETE api/characters/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
