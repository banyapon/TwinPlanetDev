// Character Editor V1.2 By VF
using UnityEngine;
namespace AD
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] private GameObject[] hats, faces, glasses, bodies, tops, bottoms, footwears;
        public int hatIndex, faceIndex, glassesIndex, bodyIndex, topIndex, bottomIndex, footwearIndex;

        //private void Start() => Default();

        public int GetHatIndex() => hatIndex;
        public int GetFaceIndex() => faceIndex;
        public int GetGlassesIndex() => glassesIndex;
        public int GetBodyIndex() => bodyIndex;
        public int GetTopIndex() => topIndex;
        public int GetBottomIndex() => bottomIndex;
        public int GetFootwearIndex() => footwearIndex;
        public string GetJSON() => new Data(hatIndex, faceIndex, glassesIndex, bodyIndex, topIndex, bottomIndex, footwearIndex).GetJSON();

        public void NextHat() => SetHat(++hatIndex);
        public void NextFace() => SetFace(++faceIndex);
        public void NextGlasses() => SetGlasses(++glassesIndex);
        public void NextBody() => SetBody(++bodyIndex);
        public void NextTop() => SetTop(++topIndex);
        public void NextBottom() => SetBottom(++bottomIndex);
        public void NextFootwear() => SetFootwear(++footwearIndex);
        public void NextGroup() => SetGroup(++topIndex);
        public void PreviousHat() => SetHat(--hatIndex);
        public void PreviousFace() => SetFace(--faceIndex);
        public void PreviousGlasses() => SetGlasses(--glassesIndex);
        public void PreviousBody() => SetBody(--bodyIndex);
        public void PreviousTop() => SetTop(--topIndex);
        public void PreviousBottom() => SetBottom(--bottomIndex);
        public void PreviousFootwear() => SetFootwear(--footwearIndex);
        public void PreviousGroup() => SetGroup(--topIndex);

        public void SetHat(int index)
        {
            if (index >= hats.Length) hatIndex = 0;
            else if (index < 0) hatIndex = hats.Length - 1;
            else hatIndex = index;
            Show(hats, hatIndex);
        }
        public void SetFace(int index)
        {
            if (index >= faces.Length) faceIndex = 0;
            else if (index < 0) faceIndex = faces.Length - 1;
            else faceIndex = index;
            Show(faces, faceIndex);
        }
        public void SetGlasses(int index)
        {
            if (index >= glasses.Length) glassesIndex = 0;
            else if (index < 0) glassesIndex = glasses.Length - 1;
            else glassesIndex = index;
            Show(glasses, glassesIndex);
        }
        public void SetBody(int index)
        {
            if (index >= bodies.Length) bodyIndex = 0;
            else if (index < 0) bodyIndex = bodies.Length - 1;
            else bodyIndex = index;
            Show(bodies, bodyIndex);
        }
        public void SetTop(int index)
        {
            if (index >= tops.Length) topIndex = 0;
            else if (index < 0) topIndex = tops.Length - 1;
            else topIndex = index;
            Show(tops, topIndex);
        }
        public void SetBottom(int index)
        {
            if (index >= bottoms.Length) bottomIndex = 0;
            else if (index < 0) bottomIndex = bottoms.Length - 1;
            else bottomIndex = index;
            Show(bottoms, bottomIndex);
        }
        public void SetFootwear(int index)
        {
            if (index >= footwears.Length) footwearIndex = 0;
            else if (index < 0) footwearIndex = footwears.Length - 1;
            else footwearIndex = index;
            Show(footwears, footwearIndex);
        }
        public void SetGroup(int index)
        {
            SetBody(index);
            SetTop(index);
            SetBottom(index);
            SetFootwear(index);
        }

        public void Default()
        {
            Load(0, 0, 0, 0, 0, 0, 0);
        }
        public void Load()
        {
            hatIndex = PlayerPrefs.GetInt("hat");
            faceIndex = PlayerPrefs.GetInt("face");
            glassesIndex = PlayerPrefs.GetInt("glasses");
            bodyIndex = PlayerPrefs.GetInt("body");
            topIndex = PlayerPrefs.GetInt("top");
            bottomIndex = PlayerPrefs.GetInt("bottom");
            footwearIndex = PlayerPrefs.GetInt("footwear");
            Show();
        }
        public void Load(int hatIndex, int faceIndex, int glassesIndex, int bodyIndex, int topIndex, int bottomIndex, int footwearIndex)
        {
            this.hatIndex = hatIndex;
            this.faceIndex = faceIndex;
            this.glassesIndex = glassesIndex;
            this.bodyIndex = bodyIndex;
            this.topIndex = topIndex;
            this.bottomIndex = bottomIndex;
            this.footwearIndex = footwearIndex;
            Show();
        }
        public void Load(string json)
        {
            Data data = JsonUtility.FromJson<Data>(json);
            Load(data.hatIndex, data.faceIndex, data.glassesIndex, data.bodyIndex, data.topIndex, data.bottomIndex, data.footwearIndex);
        }
        public void Save()
        {
            PlayerPrefs.SetInt("hat", hatIndex);
            PlayerPrefs.SetInt("face", faceIndex);
            PlayerPrefs.SetInt("glasses", glassesIndex);
            PlayerPrefs.SetInt("body", bodyIndex);
            PlayerPrefs.SetInt("top", topIndex);
            PlayerPrefs.SetInt("bottom", bottomIndex);
            PlayerPrefs.SetInt("footwear", footwearIndex);
        }
        public void Show()
        {
            Show(hats, hatIndex);
            Show(faces, faceIndex);
            Show(glasses, glassesIndex);
            Show(bodies, bodyIndex);
            Show(tops, topIndex);
            Show(bottoms, bottomIndex);
            Show(footwears, footwearIndex);
        }
        private void Show(GameObject[] objs, int index) 
        { 
            foreach (GameObject go in objs) { go.SetActive(false); }
            objs[index].SetActive(true);
        }
        private class Data
        {
            public int hatIndex, faceIndex, glassesIndex, bodyIndex, topIndex, bottomIndex, footwearIndex;

            public Data(int hatIndex, int faceIndex, int glassesIndex, int bodyIndex, int topIndex, int bottomIndex, int footwearIndex)
            {
                this.hatIndex = hatIndex;
                this.faceIndex = faceIndex;
                this.glassesIndex = glassesIndex;
                this.bodyIndex = bodyIndex;
                this.topIndex = topIndex;
                this.bottomIndex = bottomIndex;
                this.footwearIndex = footwearIndex;
            }

            public string GetJSON() { return JsonUtility.ToJson(this); }
        }
    }
}
