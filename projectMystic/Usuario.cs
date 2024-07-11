using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMystic
{
    public class Usuario
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Correo { get; set; }

        [FirestoreProperty]
        public string Rol { get; set; } // Propietario, Admin, Miembro

        [FirestoreProperty]
        public List<string> ComunidadIds { get; set; } = new List<string>();
    }
}
