using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateEnemyTool))]
public class CreateEnemyTool : EditorWindow
{

    public Shells shells;
    public GameObject shellPrefab;
    public Enemies enemieslist;

    public string name;
    public bool rangedEnemy;
    public int attackSpeed;
    public int range;
    public int health;
    public int dmg;
    public int pointsReward;
    public int speed;
    string[] rangeBullets;
    public int selectedBullet = 0;
    int shootingPoints;
    public Sprite sprite;
    GameObject tmp;

    [MenuItem("Window/EnemyCreationTool")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateEnemyTool window = (CreateEnemyTool)EditorWindow.GetWindow(typeof(CreateEnemyTool));
        window.Show();
    }

    void OnGUI()
    {
        initBulletOptions();
        GUILayout.Label("CREATE NEW ENEMY");

        name = EditorGUILayout.TextField("Name: ", name);
        rangedEnemy = EditorGUILayout.Toggle("Ranged: ", rangedEnemy);

        if (rangedEnemy)
        {
            range = EditorGUILayout.IntSlider("Range: ", range, 2, 8);
            attackSpeed = EditorGUILayout.IntSlider("Attack speed: ", attackSpeed, 2, 10);
        }

        health = EditorGUILayout.IntField("HP: ", health);
        speed = EditorGUILayout.IntSlider("Speed: ", speed, 1, 4);
        dmg = EditorGUILayout.IntSlider("Dmg: ", dmg, 1, 10);
        GUILayout.Label("Number of shooting points: ");
        shootingPoints = EditorGUILayout.IntField(shootingPoints);
        GUILayout.Label("Number of reward points: ");
        pointsReward = EditorGUILayout.IntField(pointsReward);

        GUILayout.Label("Bullet type: ");
        selectedBullet = EditorGUILayout.Popup(selectedBullet, rangeBullets);

        var spriteRect = new Rect(10, 10, position.width, position.height);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Sprite: ");
        sprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create new type of enemy"))
        {
            createObject();
        }
    }

    public void initBulletOptions()
    {
        rangeBullets = new string[shells.enemyShellsAndBullets.Count];
        for(int i = 0; i < rangeBullets.Length; i++)
        {
            rangeBullets[i] = shells.enemyShellsAndBullets[i].name;
        }
    }

    public void createObject()
    {

        tmp = new GameObject(name);
        tmp.AddComponent<SpriteRenderer>();
        tmp.GetComponent<SpriteRenderer>().sprite = sprite;
        tmp.AddComponent<Rigidbody2D>();
        tmp.GetComponent<Rigidbody2D>().gravityScale = 0;
        tmp.AddComponent<BoxCollider2D>();
        tmp.AddComponent<Enemy>();
        tmp.tag = "Enemy";
        Enemy enemyScript = tmp.GetComponent<Enemy>();
        //enemyScript.dmg = dmg;
        enemyScript.hp = health;
        enemyScript.can_shooting = rangedEnemy;
        enemyScript.speed = speed;
        enemyScript.dmg = dmg;
        enemyScript.pointsReward = pointsReward;

        if (rangedEnemy)
        {
            enemyScript.sheelPrefab = shellPrefab;
            enemyScript.attackSpeed = attackSpeed;
            enemyScript.range = range;
            enemyScript.sheelPrefab = shellPrefab;
            enemyScript.shellData = shells.returnEnemyShell(rangeBullets[selectedBullet]);
            enemyScript.shooting_points = new List<Transform>();

            for (int i = 0; i < shootingPoints; i++)
            {
                GameObject shooting_point = new GameObject("shooting_point");
                shooting_point.transform.parent = tmp.transform;
                shooting_point.transform.position = new Vector3(0, 0, 0);
                shooting_point.transform.rotation = Quaternion.Euler(new Vector3(0, 0, tmp.transform.rotation.z - 180f));
                enemyScript.shooting_points.Add(shooting_point.transform);
            }
        }
        string path = AssetDatabase.GetAssetPath(shellPrefab);
        Debug.Log(path);

        PrefabUtility.SaveAsPrefabAsset(tmp, "Assets/Prefabs/Enemies/" + tmp.name + ".prefab");

    }

}