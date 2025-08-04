using UnityEngine;
using UnityEditor;

public class AirplaneProcessModel : AssetPostprocessor
{
    private void OnPostprocessModel(GameObject gameObject)
    {
        if (gameObject.name != "Enemy2b")
        {
            return;
        }
        

        ModelImporter importer = assetImporter as ModelImporter;
        string assetPath = importer.assetPath;

        EditorApplication.delayCall += () =>
        {
            GameObject tar = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            GameObject prefab = PrefabUtility.CreatePrefab("Assets/Prefabs/Airplane/Enemy2c.prefab", tar);
            prefab.tag = "Enemy";

            foreach(Transform obj in prefab.GetComponentsInChildren<Transform>())
            {
                if(obj.name == "col")
                {
                    MeshRenderer r = obj.GetComponent<MeshRenderer>();
                    r.enabled = false;

                    if(obj.gameObject.GetComponent<MeshCollider>() == null)
                    {
                        obj.gameObject.AddComponent<MeshCollider>();

                        obj.tag = "Enemy";
                    }
                }
            }

            Rigidbody rigid = prefab.AddComponent<Rigidbody>();
            rigid.useGravity = false;
            rigid.isKinematic = true;

            prefab.AddComponent<AudioSource>();

            GameObject rocket = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Airplane/AirplaneEnemyRocket.prefab");
            GameObject fx = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FX/Explosion.prefab");

            AirplaneSuperEnemy enemy = prefab.AddComponent<AirplaneSuperEnemy>();
            enemy.m_life = 50;
            enemy.m_rocket = rocket.transform;
            enemy.m_explosionFX = fx.transform;
        };
    }
}