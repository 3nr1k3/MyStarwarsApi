using System;
using System.Collections.Generic;
using MyStarwarsApi.Models;

namespace MyStarwarsApi.Repo.Interfaces
{
    public interface ICharacterRepository
    {
        List<Character> getCharacters();
        Character getCharacter(Guid id);
        List<Character> getCharactersByName(String name);
        void addCharacter(Character character);
        int removeCharacter(Character character);
    }
}