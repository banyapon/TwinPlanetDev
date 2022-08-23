using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneOpener : Editor
{
    [MenuItem("Scenes/Payload")]
    static void Payload()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Payload.unity");
    }

    [MenuItem("Scenes/Login")]
    static void Login()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Login.unity");
    }

    [MenuItem("Scenes/CharacterM")]
    static void CharacterM()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/CharacterM.unity");
    }

    [MenuItem("Scenes/CharacterF")]
    static void CharacterF()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/CharacterF.unity");
    }

    [MenuItem("Scenes/Fortune")]
    static void Fortune()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Fortune.unity");
    }

    [MenuItem("Scenes/GalleriesLafayette")]
    static void GalleriesLafayette()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GalleriesLafayette.unity");
    }

    [MenuItem("Scenes/GemPavilion")]
    static void GemPavilion()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GemPavilion.unity");
    }

    [MenuItem("Scenes/HeroPom")]
    static void HeroPom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/HeroPom.unity");
    }

    [MenuItem("Scenes/ParisRoad_short")]
    static void ParisRoad_short()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/ParisRoad_short.unity");
    }

    [MenuItem("Scenes/MissLily")]
    static void MissLily()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MissLily.unity");
    }

    [MenuItem("Scenes/Gallery333")]
    static void Gallery333()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Gallery333.unity");
    }

    [MenuItem("Scenes/Rapee")]
    static void Rapee()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Rapee.unity");
    }

    [MenuItem("Scenes/VIPRoom")]
    static void VIPRoom()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/VIPRoom.unity");
    }

    [MenuItem("Scenes/Photobooth")]
    static void Photobooth()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Photobooth.unity");
    }

    [MenuItem("Scenes/MiniGame")]
    static void MiniGame()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MiniGame.unity");
    }
}
