using Assets.Scripts.Commons;
using Assets.Scripts.Core.Daos;
using Assets.Scripts.Core.Models;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserDAO : Singleton<UserDAO>, IDAO<User, string>
{
    private FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

    public async Task<bool> insert(User entity)
    {
        try
        {
            DocumentReference docRef = firestore.Collection("users").Document(entity.FirebaseId);
            await docRef.SetAsync(entity);
            Debug.Log($"Usuario {entity.Username} insertado correctamente.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al insertar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<User> select(string id)
    {
        try
        {
            DocumentReference docRef = firestore.Collection("users").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                User user = snapshot.ConvertTo<User>();
                return user;
            }
            else
            {
                Debug.LogError("Usuario no encontrado.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al obtener usuario: {ex.Message}");
            return null;
        }
    }

    public async Task<List<User>> selectAll()
    {
        List<User> users = new List<User>();

        try
        {
            QuerySnapshot querySnapshot = await firestore.Collection("users").GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                User user = documentSnapshot.ConvertTo<User>();
                users.Add(user);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al obtener todos los usuarios: {ex.Message}");
        }

        return users;
    }

    public async Task<bool> update(User entity)
    {
        try
        {
            DocumentReference docRef = firestore.Collection("users").Document(entity.FirebaseId);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Username", entity.Username }
            };

            await docRef.UpdateAsync(updates);
            Debug.Log($"Usuario {entity.Username} actualizado correctamente.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al actualizar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> delete(string id)
    {
        try
        {
            DocumentReference docRef = firestore.Collection("users").Document(id);
            await docRef.DeleteAsync();
            Debug.Log($"Usuario con FirebaseId {id} eliminado correctamente.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al eliminar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> exists(string firebaseId, string unityId)
    {
        try
        {
            CollectionReference usersRef = firestore.Collection("users");

            Query query = usersRef
                .WhereEqualTo("FirebaseId", firebaseId)
                .WhereEqualTo("AuthUnityId", unityId);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            return snapshot.Count > 0;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al verificar existencia del usuario: {ex.Message}");
            return false;
        }
    }
}
