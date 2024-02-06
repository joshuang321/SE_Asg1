using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class User
    {
        static int ID_AUTOINCRM =0;
        string name;
        string username;
        string password;
        int id;
        string role;
        string phoneNumber;

        List<SeasonParking> passes;

        public User(string name, string password, string username,
                string role,
                string phoneNumber)
        {
            id = ++ID_AUTOINCRM;
            this.name = name;
            this.password = password;
            this.username = username;
            this.role = role;
            this.phoneNumber = phoneNumber;
            passes = new List<SeasonParking>();
        }

        public string GetName() => name;
        public string GetUsername() => username;
        public string GetPassword() => password;
        public int GetId() => id;
        public string GetRole() => role;
        public string GetPhoneNumber() => phoneNumber;

        public List<SeasonParking>.Enumerator GetPasses()
        {
            return passes.GetEnumerator();
        }

        public SeasonParking? GetPass(int index)
        {
            if (index<passes.Count)
                return passes[index];
            return null;
        }

        public void AddPass(SeasonParking pass)
        {
            passes.Add(pass);
        }

        public void Remove(SeasonParking pass)
        {
            passes.Remove(pass);
        }
    }
}