using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public class State
    {
        public Path Path { get; set; }
        public string Hash { get; private set; }

        public State(Path path)
        {
            Path = path;
            Hash = path.ToString();
        }

        public string GetHash()
        {
            return Hash;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is State)) return false;
            return Hash == (obj as State).Hash;
        }

        public override int GetHashCode()
        {
            return (Hash != null ? Hash.GetHashCode() : 0);
        }
    }
}
