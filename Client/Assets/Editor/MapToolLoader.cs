using UnityEngine;
using Newtonsoft.Json;

public class MapToolLoader
{
    public DungeonJsonData dungeonData;
    private string _dungeonName;

    public void LoaderDungeon()
    {
        dungeonData = JsonLoad<DungeonJsonData>(_dungeonName);
    }
    public DungeonInfo GetDungeonInfo(int index)
    {
        return dungeonData.DungeonInfos[index];
    }
    public void DeleteDungeonData(int index)
    {
        dungeonData.DungeonInfos[index].monsterInfos.Clear();
    }

    private T JsonLoad<T>(string fileName)
    {
        TextAsset textAsset = Resources.Load(fileName) as TextAsset;

        string dungeonText = textAsset.text;
        return JsonConvert.DeserializeObject<T>(dungeonText);
    }
}
