using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStarwarsApi.Models{
    public class Character{
        [Key]
        public Guid id{ get; set; }
        [Required]
        public String name{ get; set; }
        public String side{ get; set; }
        public List<Character> charactersKilled{ get; set; }

        public Character(){}

        public class Builder{
            private Character _character;

            public Builder(){
                if (_character == null)
                    _character = new Character();
            }

            public Builder setId(Guid id){
                _character.id = id;
                return this;
            }
            
            public Builder setName(String name){
                _character.name = name;
                return this;
            }

            public Builder setSide(String side){
                _character.side = side;
                return this;
            }

            public Builder setCharactersKilled(List<Character> characters){
                if(_character.charactersKilled == null)
                    _character.charactersKilled = new List<Character>();

                _character.charactersKilled = characters;
                return this;
            }

            public Builder addCharacterKilled(Character character){
                if(_character.charactersKilled == null)
                    _character.charactersKilled = new List<Character>();
                    
                _character.charactersKilled.Add(character);
                return this;
            }

            public Character build(){
                return _character;
            }
        }
    }
}
