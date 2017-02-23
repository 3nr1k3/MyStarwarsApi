using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

using MyStarwarsApi.Models;
using MyStarwarsApi.Repo;

namespace MyStarwarsApi.Controllers
{
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ICharacterRepository _characterRepository;

        public CharactersController(ICharacterRepository characterRepository){
            _characterRepository = characterRepository;
        }

        // GET api/characters
        [HttpGet]
        public IEnumerable<Character> Get()
        {
            return _characterRepository.getCharacters();
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/characters/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
