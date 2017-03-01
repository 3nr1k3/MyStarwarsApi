using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStarwarsApi.Models.ViewModel{
    public class CharacterCreateViewModel{
        [Required]
        public string name{get;set;}
        public string side{get;set;}
        public IEnumerable<Guid> charactersKilled{get;set;}

        public CharacterCreateViewModel(){}
    }
}