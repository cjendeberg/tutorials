using System;

namespace t8
{
    class User
    {
        private string _name;
        public string NormalizedName {get;set;}

        public User(string name)
        {
            _name = name;
        }
    }
}
