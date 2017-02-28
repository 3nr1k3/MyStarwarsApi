using System;
using System.Collections.Generic;
using System.Linq;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;


namespace MyStarwarsApi.Repo{

    public class CharacterRepository : ICharacterRepository{

        //private static List<Character> _characters = new List<Character>();
        private SqliteDbContext _dbContext;

        public static void FillCharacterRepository(SqliteDbContext context){
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

            if(context.Characters.Count() <= 0 ){
                context.Characters.Add(DarthV);
                context.Characters.Add(ObiWan);

                context.SaveChanges();
            }
        }

        public CharacterRepository(SqliteDbContext context){
            _dbContext = context;
        }

        public void addCharacter(Character character)
        {
            character.id = Guid.NewGuid();
            _dbContext.Characters.Add(character);

            _dbContext.SaveChanges();
        }

        public Character getCharacter(Guid id)
        {
            return _dbContext.Characters.FirstOrDefault(c => c.id == id);
        }

        public List<Character> getCharacters()
        {
            CharacterRepository.FillCharacterRepository(_dbContext);
            return _dbContext.Characters.ToList();
        }

        public List<Character> getCharactersByName(String name){
            return _dbContext.Characters.Where(c => c.name.Contains(name)).ToList();
        }
    }
}
