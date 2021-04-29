using System;
using System.Collections.Generic;

namespace t8
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> users = new List<User>(){
                new User("nisse"){NormalizedName = "NISSE"},
                new User("adam"){NormalizedName = "ADAM"},
            };
            
            foreach (User user in users) {
                System.Console.Out.WriteLine(user.NormalizedName);
            }
        }
    }
}
