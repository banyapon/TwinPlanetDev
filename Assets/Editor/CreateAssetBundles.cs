using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles by daydev")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}