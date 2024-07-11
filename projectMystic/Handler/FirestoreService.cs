using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace projectMystic.Handler
{
    public static class FirestoreService
    {
        private static FirestoreDb _firestoreDb;

        public static FirestoreDb FirestoreDb
        {
            get
            {
                if (_firestoreDb == null)
                {
                    string path = "Credentials/proyectmystic-7e93c-firebase-adminsdk-klbr5-dea7209fb5.json";
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                    _firestoreDb = FirestoreDb.Create("proyectmystic-7e93c"); // ID del proyecto de Firebase
                }
                return _firestoreDb;
            }
        }
    }
}
