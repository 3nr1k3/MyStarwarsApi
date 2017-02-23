using System;
using System.Collections.Generic;
using MyStarwarsApi.Models;


namespace MyStarwarsApi.Repo{

    public class CharacterRepository : ICharacterRepository{

        private static List<Character> _characters = new List<Character>();

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

            if(_characters.Count<=0){
                _characters.Add(DarthV);
                _characters.Add(ObiWan);
            }
        }

        public void addCharacter(Character character)
        {
            character.id = Guid.NewGuid();
            _characters.Add(character);
        }

        public Character getCharacter(Guid id)
        {
            return _characters.Find(c => c.id == id);
        }

        List<Character> ICharacterRepository.getCharacters()
        {
            return _characters;
        }
    }
}
