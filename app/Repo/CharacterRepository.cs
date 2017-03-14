using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;
using MyStarwarsApi.Repo.Interfaces;


namespace MyStarwarsApi.Repo{

    public class CharacterRepository : ICharacterRepository
    {
        private SqliteDbContext _dbContext;

        public CharacterRepository(SqliteDbContext context){
            _dbContext = context;
        }

        public void addCharacter(Character character){
            character.id = Guid.NewGuid();
            _dbContext.Characters.Add(character);

            _dbContext.SaveChanges();
        }

        public Character getCharacter(Guid id){
            return _dbContext.Characters
                .Include(c => c.charactersKilled)
                .FirstOrDefault(c => c.id == id);
        }

        public List<Character> getCharacters()
        {
            return _dbContext.Characters.ToList();
        }

        public List<Character> getCharactersByName(String name){
            return _dbContext.Characters.Where(c => c.name.Contains(name)).ToList();
        }
    }
}
