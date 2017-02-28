using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using MyStarwarsApi.Models;
using MyStarwarsApi.Repo;

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
        public IEnumerable<Character> Get()
        {
            var req = Request;
            var headers = req.Headers;

            StringValues    name,
                            side,
                            take,
                            skip;

            List<Character> chars = _characterRepository.getCharacters();

            if(headers.TryGetValue("containsName", out name))
                chars = chars.Where(c => c.name.ToLower().Contains(name.ToString().ToLower())).ToList();

            if(headers.TryGetValue("containsSide", out side))
                chars = chars.Where(c => c.side.ToLower().Contains(side.ToString().ToLower())).ToList();

            if(headers.TryGetValue("skip", out skip)){
                int startIn;
                if(Int32.TryParse(skip, out startIn))
                    chars = chars.Skip(startIn).ToList();
                else
                    _logger.LogError($"Cannot cast (skip){skip} to integer.");

            }

            if(headers.TryGetValue("take", out take)){
                int offset;
                if(Int32.TryParse(take, out offset))
                    chars = chars.Take(offset).ToList();
                else
                    _logger.LogError($"Cannot cast (take){take} to integer.");
            }else
                chars = chars.Take(100).ToList();
                

            return chars;
        }

        // GET api/characters/5
        [HttpGet("{id}")]
        public Character Get(Guid id)
        {
            return _characterRepository.getCharacter(id);
        }

        // POST api/characters
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Character newCharacter)
        {
            List<Character> charsKilled = new List<Character>();

            if(newCharacter.charactersKilled != null){
                newCharacter.charactersKilled.ForEach(c => {
                    charsKilled.Add(_characterRepository.getCharacter(c.id));
                });
                newCharacter.charactersKilled = charsKilled;
            }

            _characterRepository.addCharacter(newCharacter);
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
