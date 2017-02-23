using System;
using System.Collections.Generic;
using MyStarwarsApi.Models;

namespace MyStarwarsApi.Repo{
    public interface ICharacterRepository{
        List<Character> getCharacters();
        Character getCharacter(Guid id);
        void addCharacter(Character character);
    }
}