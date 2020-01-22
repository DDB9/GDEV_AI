using UnityEngine;
using UnityEditor;

public class AIAjuster : EditorWindow
{
    GameObject human; 
    string name;
    float speed;
    float wanderDuration;
    float turnSpeed;

    bool groupEnabled, proneToDepression, Gluttenous, unfaithful;

    [MenuItem("Window/AI/AI Adjustment Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AIAjuster));
    }

    MonoScript targetObject = null;
    void OnGUI()
    {
        GUILayout.Label("Create AI", EditorStyles.boldLabel);
        name = EditorGUILayout.TextField("Name:", name);

        GUILayout.Space(10);

        speed = EditorGUILayout.Slider("Speed", speed, 0, 20);
        wanderDuration = EditorGUILayout.Slider("Wander Duration", wanderDuration, 50, 500);
        turnSpeed = EditorGUILayout.Slider("Turn speed", turnSpeed, 0, 20);

        GUILayout.Space(10);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Imperfection?", groupEnabled);
            proneToDepression = EditorGUILayout.Toggle("Prone to depression", proneToDepression);
            Gluttenous = EditorGUILayout.Toggle("Gluttenous", Gluttenous);
            unfaithful = EditorGUILayout.Toggle("Unfaithful", unfaithful);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(10);

        if (GUILayout.Button("Instantiate AI"))
        {
            if (speed == 0 || turnSpeed == 0)
                Debug.LogWarning("Some AI values are set to zero!");
            if (name == null)
                Debug.LogWarning("This unit's name has not been set!");

            /*  GameObject ai = Instantiate(human, vector3.zero, Quaternion.Identity);
                humanAI = ai.GetComponent<HumanAI>();

                humanAI.name = name;
                humanAI.speed = speed;
                humanAI.wanderDuration = wanderDuration;
                humanAI.turnSpeed = turnSpeed;

                if (proneToDepression)
                    (Increase chance to get depression, yet to be implemented.)
                if (Gluttenous)
                    (Make the human count twice as much for the food calculation, yet to be implemented.)
                if (unfaithful)                
                    humanAI.IncreaseFaithOverTime(true);    */
        }
    }
}
