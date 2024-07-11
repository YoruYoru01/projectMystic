using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projectMystic.Models;

namespace projectMystic.Handler
{
    public class ComunidadService
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly UsuarioService _usuarioService;

        public ComunidadService()
        {
            _firestoreDb = FirestoreService.FirestoreDb;
            _usuarioService = new UsuarioService();
        }

        public async Task<Comunidad> GetComunidadAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<Comunidad>();
            }
            return null;
        }

        public async Task<List<Comunidad>> GetComunidadesAsync()
        {
            Query query = _firestoreDb.Collection("comunidades");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            List<Comunidad> comunidades = new List<Comunidad>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                comunidades.Add(document.ConvertTo<Comunidad>());
            }
            return comunidades;
        }

        public async Task AddComunidadAsync(Comunidad comunidad)
        {
            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document();
            comunidad.Id = docRef.Id;
            comunidad.FechaDeCreacion = DateTime.UtcNow;
            await docRef.SetAsync(comunidad);
        }

        public async Task UpdateComunidadAsync(Comunidad comunidad)
        {
            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document(comunidad.Id);
            await docRef.SetAsync(comunidad, SetOptions.Overwrite);
        }

        public async Task DeleteComunidadAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document(id);
            await docRef.DeleteAsync();
        }

        public async Task<bool> EsPropietarioOAdminAsync(string comunidadId, string userId)
        {
            Comunidad comunidad = await GetComunidadAsync(comunidadId);
            return comunidad.PropietarioId == userId || (comunidad.Roles.ContainsKey(userId) && comunidad.Roles[userId] == "admin");
        }

        public async Task AñadirMiembroAsync(string comunidadId, string userId, string rol = "miembro")
        {
            if (!await EsPropietarioOAdminAsync(comunidadId, userId))
            {
                throw new UnauthorizedAccessException("No tienes permiso para añadir miembros a esta comunidad.");
            }

            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document(comunidadId);
            Comunidad comunidad = await GetComunidadAsync(comunidadId);

            if (comunidad.MiembrosIds.Contains(userId))
            {
                throw new Exception("El usuario ya es miembro de la comunidad.");
            }

            comunidad.MiembrosIds.Add(userId);
            comunidad.Roles[userId] = rol;
            comunidad.NumeroDeUsuarios = comunidad.MiembrosIds.Count;

            await docRef.SetAsync(comunidad, SetOptions.Overwrite);

            // Actualizar el usuario con la nueva comunidad
            Usuario usuario = await _usuarioService.GetUsuarioAsync(userId);
            if (usuario != null)
            {
                usuario.ComunidadIds.Add(comunidadId);
                await _usuarioService.UpdateUsuarioAsync(usuario);
            }
        }

        public async Task RemoverMiembroAsync(string comunidadId, string userId)
        {
            if (!await EsPropietarioOAdminAsync(comunidadId, userId))
            {
                throw new UnauthorizedAccessException("No tienes permiso para remover miembros de esta comunidad.");
            }

            DocumentReference docRef = _firestoreDb.Collection("comunidades").Document(comunidadId);
            Comunidad comunidad = await GetComunidadAsync(comunidadId);

            if (!comunidad.MiembrosIds.Contains(userId))
            {
                throw new Exception("El usuario no es miembro de la comunidad.");
            }

            comunidad.MiembrosIds.Remove(userId);
            comunidad.Roles.Remove(userId);
            comunidad.NumeroDeUsuarios = comunidad.MiembrosIds.Count;

            await docRef.SetAsync(comunidad, SetOptions.Overwrite);

            // Actualizar el usuario removiendo la comunidad
            Usuario usuario = await _usuarioService.GetUsuarioAsync(userId);
            if (usuario != null)
            {
                usuario.ComunidadIds.Remove(comunidadId);
                await _usuarioService.UpdateUsuarioAsync(usuario);
            }
        }
    }
}
