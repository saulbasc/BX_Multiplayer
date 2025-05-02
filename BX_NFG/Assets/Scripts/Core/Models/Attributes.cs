
using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class Attributes
    {
        [FirestoreProperty]
        public string FirebaseID { get; private set; }
        [FirestoreProperty]
        public List<Attribute> Attributs { get; private set; }

        public Attributes(string firebaseID, List<Attribute> attributes)
        {
            FirebaseID = firebaseID;
            Attributs = attributes;
        }

        public Attributes(string firebaseID) {
            FirebaseID = firebaseID;
            Attributs = Enum.GetValues(typeof(AttributeEnum))
                .Cast<AttributeEnum>()
                .Select(attrEnum => new Attribute(attrEnum, 3))
                .ToList();
        }
    }
}
