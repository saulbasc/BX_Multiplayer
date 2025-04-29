using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class Attributes
    {
        [FirestoreProperty]
        public string FirebaseID { get; private set; }
        [FirestoreProperty]
        public int Speed { get; private set; }
        [FirestoreProperty]
        public int Power { get; private set; }
        [FirestoreProperty]
        public int Weigth { get; private set; }

        public Attributes(string firebaseID, int speed, int power, int weigth)
        {
            FirebaseID = firebaseID;
            Speed = speed;
            Power = power;
            Weigth = weigth;
        }

        public Attributes() { }
    }
}
