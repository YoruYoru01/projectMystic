using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMystic.Handler
{
    public class UsuarioService
    {
        private readonly FirestoreDb _firestoreDb;

        public UsuarioService()
        {
            _firestoreDb = FirestoreService.FirestoreDb;
        }

        public async Task<Usuario> GetUsuarioAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection("usuarios").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Usuario>();
            }
            return null;
        }

        public async Task AddUsuarioAsync(Usuario usuario)
        {
            DocumentReference docRef = _firestoreDb.Collection("usuarios").Document(usuario.Id);
            await docRef.SetAsync(usuario);
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            DocumentReference docRef = _firestoreDb.Collection("usuarios").Document(usuario.Id);
            await docRef.SetAsync(usuario, SetOptions.Overwrite);
        }

        public async Task DeleteUsuarioAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection("usuarios").Document(id);
            await docRef.DeleteAsync();
        }
    }
}
