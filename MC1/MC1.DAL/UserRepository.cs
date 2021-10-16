using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IdentityModel.Tokens.Jwt;

namespace MC1.DAL
{
    class UserRepository
    {
        private List<User> datas;
        private string path = "Data/user.json";
        private Serializer<List<User>> serializer;



        public UserRepository()
        {
            datas = new List<User>();
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            serializer = new Serializer<List<User>>();
        }

        public void Add(User obj)
        {
            foreach (var data in datas)
                if (data.Email.Equals(obj.Email, StringComparison.InvariantCultureIgnoreCase))
                    throw new DuplicateWaitObjectException("Email already exists!");

            datas.Add(obj);

        }
        public void Set(User oldobj, User newobj)
        {
            var oldIndex = -1;
            for (int i = 0; i < datas.Count; i++)
                if (datas[i].Email.Equals(oldobj.Email, StringComparison.OrdinalIgnoreCase))
                    oldIndex = i;
            if (oldIndex < 0)
                throw new KeyNotFoundException("Email not found!");

            var newIndex = -1;
            for (int i = 0; i < datas.Count; i++)
                if (datas[i].Email.Equals(newobj.Email, StringComparison.OrdinalIgnoreCase))
                    newIndex = i;
            if (newIndex >= 0 && newIndex != oldIndex)
                throw new KeyNotFoundException("Email already exists!");
            datas[oldIndex] = newobj; ;



        }
        public void Delete(User obj)
        {
            var index = -1;
            for (int i = 0; i < datas.Count; i++)
                if (datas[i].Email.Equals(obj.Email, StringComparison.OrdinalIgnoreCase))
                    index = i;
            if (index >= 0)
                datas.RemoveAt(index);
            
        }
        public  User Login(string email, string password)
        {
            foreach (var data in datas)
                if (data.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    data.Password.Equals(password))
                    return data;
            return null;
          
        }
        public List<User> FindByName(string value)
        {
            List<User> list = new List<User>();
            foreach (var data in datas)
                if (data.Fullname.ToLower().Contains(value.ToLower()))
                    list.Add(data);
            return list;
            {

            }
        }
        public void Save()
        {
            serializer.Serialize(datas, path);
        }

        public void Restore()
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists && fi.Length > 0)
                datas = serializer.Deserialize(path);
        }
       
    }
}
