using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace projectMystic.Models
{
    [FirestoreData]
    public class Comunidad
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public string Descripcion { get; set; }

        [FirestoreProperty]
        public string PropietarioId { get; set; }

        [FirestoreProperty]
        public List<string> MiembrosIds { get; set; } = new List<string>();

        [FirestoreProperty]
        public int NumeroDeUsuarios { get; set; }

        [FirestoreProperty]
        public DateTime FechaDeCreacion { get; set; }

        [FirestoreProperty]
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
    }
}
