using System;
using SysFile = System.IO.File;
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
using System.Dynamic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;

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
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public CharactersController(
            ILoggerFactory logger,
            ICharacterRepository characterRepository,
            IImageRepository imageRepository,
            IConfiguration configuration,
            IHostingEnvironment env
            ){
            _characterRepository = characterRepository;
            _imageRepository = imageRepository;
            _logger = logger.CreateLogger<CharactersController>();
            _configuration = configuration;
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
            
            _logger.LogInformation($"[controller] - {HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress}");
            
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
            Character character = _characterRepository.getCharacter(id);

            character.avatar = _imageRepository.getAvatar(character.avatarId);
            
            return character;
        }

        // POST api/characters
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody]CharacterCreateViewModel newCharacter)
        {
            List<Character> charsKilled = new List<Character>();
            Character characterToCreate = new Character();

            if (newCharacter.charactersKilled != null)
            {
                newCharacter.charactersKilled.ToList().ForEach(c =>
                {
                    charsKilled.Add(_characterRepository.getCharacter(c));
                });
            }

            characterToCreate = Mapper.Map<CharacterCreateViewModel, Character>(newCharacter);

            if (newCharacter.avatar != "")
            {
                Bitmap bitImage = ImageHelper.bitmapFromBase64String(newCharacter.avatar);

                Models.Image.Builder img = new Models.Image.Builder(_imageRepository);
                img.setFormat(ImageHelper.getImageFormatFromBase64String(newCharacter.avatar));
                img.setType(image_type.Character);
                await img.fromBitmapAsync(bitImage, _env);
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
        public IActionResult DeleteAsync(Guid id)
        {
            dynamic response = new ExpandoObject();
            
            Character toDelete = _characterRepository.getCharacter(id);
            if (_characterRepository.removeCharacter(toDelete) > 0)
            {
                if(SysFile.Exists(toDelete.avatar.imageServerPath(_env.WebRootPath)))
                {
                    _logger.LogInformation("Exists");
                    try
                    {
                        SysFile.Delete(toDelete.avatar.imageServerPath(_env.WebRootPath));
                    }
                    catch (IOException e)
                    {
                        _logger.LogError(e.Message);
                    }
                }

                response.code = 200;
                response.status = "Ok";
                response.copyright = _configuration.GetValue<string>("MyStarwarsApi:Legal:Copyright");
                response.attributionText = _configuration.GetValue<string>("MyStarwarsApi:Legal:AttributionText");
                response.attributionHtml = _configuration.GetValue<string>("MyStarwarsApi:Legal:AttributionHTML");
                response.data = new ExpandoObject();
                response.data.results = new List<Character>();
                response.data.results.Add(toDelete);
            }
            else if( id == null || id == Guid.Empty)
            {
                response.code = 409;
                response.message = "Empty Id parameter.";
            }
            else if (id != null || id != Guid.Empty)
            {
                response.code = 404;
                response.message = "Resource not found.";
            }
            else
            {
                response.code = 460;
                response.message = "Unknown error.";
            }

            return Json(response);
        }
    }
}
