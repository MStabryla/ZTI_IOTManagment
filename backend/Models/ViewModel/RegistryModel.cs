using System.Collections.Generic;

namespace SysOT.Models
{
    class RegistryModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

