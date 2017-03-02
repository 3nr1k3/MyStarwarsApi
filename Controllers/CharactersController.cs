using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MyStarwarsApi.Models;
using MyStarwarsApi.Repo;
using MyStarwarsApi.Models.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;

namespace MyStarwarsApi.Controllers
{
    /// <summary>  </summary>
    /// <remarks>  </remarks>
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(
            ILoggerFactory logger,
            ICharacterRepository characterRepository
            ){
            _characterRepository = characterRepository;
            _logger = logger.CreateLogger<CharactersController>();
        }

        // GET api/characters
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Character>), 200)]
        public IEnumerable<Character> Get(
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

            return chars;
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
