using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NekojumSpawnTime : MonoBehaviour
{
    [SerializeField]private GameObject nekoGroup;
    [SerializeField]private float coolDown;
    void Start(){
        nekoGroup.SetActive(false);


        CheckTime();
    }
    void CheckTime(){
        StopCoroutine(Checked());
        StartCoroutine(Checked());
    }
    void SpawnNekojum(bool onSpawn){
        nekoGroup.SetActive(onSpawn);
    }
    IEnumerator Checked(){     

        string uri = "https://twinplanetonline.com/api/bot.php?api_key=twinplanet0x156xxee011";

        UnityWebRequest www = UnityWebRequest.Get(uri); // อ่าน
        yield return www.Send();
 
        if(www.isNetworkError) {
            Debug.Log(www.error);

            SpawnNekojum(false);
        }
        else {
            string json = www.downloadHandler.text;
            NekoTime newNeko = new NekoTime();
            newNeko = JsonUtility.FromJson<NekoTime>(json);

            Debug.Log("Import Item:"+newNeko.status_code);
            Debug.Log("Import Item:"+newNeko.message);
            Debug.Log("Import Item:"+newNeko.npc);
           
           if(newNeko.npc == "true"){
                SpawnNekojum(true);
           }else{
               SpawnNekojum(false);
           }
        }

        yield return new WaitForSeconds(coolDown);
        CheckTime();
    }
}
public class NekoTime
{
        public string status_code;
        public string message;
        public string npc;
}
