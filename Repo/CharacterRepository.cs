using System;
using System.Collections.Generic;
using System.Linq;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;


namespace MyStarwarsApi.Repo{

    public class CharacterRepository : ICharacterRepository{

        //private static List<Character> _characters = new List<Character>();
        private SqliteDbContext _dbContext;

        public CharacterRepository(){
            Character DarthV = new Character.Builder()
                .setId(Guid.NewGuid())
                .setName("Darth Vader")
                .setSide("Dark Side")
                .setCharactersKilled(null)
                .build();

            Character ObiWan = new Character.Builder()
                .setId(Guid.NewGuid())
                .setName("Obi Wan Kenobi")
                .setSide("Light Side")
                .addCharacterKilled(DarthV)
                .build();

            if(_dbContext.Characters.Count() <= 0 ){
                _dbContext.Characters.Add(DarthV);
                _dbContext.Characters.Add(ObiWan);

                _dbContext.SaveChanges();
            }
        }

        public CharacterRepository(SqliteDbContext context){
            _dbContext = context;
        }

        public void addCharacter(Character character)
        {
            character.id = Guid.NewGuid();
            //_characters.Add(character);
            _dbContext.Characters.Add(character);

            _dbContext.SaveChanges();
        }

        public Character getCharacter(Guid id)
        {
            return _dbContext.Characters.FirstOrDefault(c => c.id == id);
        }

        List<Character> ICharacterRepository.getCharacters()
        {
            return _dbContext.Characters.ToList();
        }
    }
}