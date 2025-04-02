using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    // ---싱글톤으로 선언--- //
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
    string GameDataFileName = "GameData.json";

    // --- 저장용 클래스 변수 --- //
    public Data data = new Data();


    // 불러오기
    public void StartGame()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            // 저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print("불러오기 완료");
        }
    }


    // 저장하기
    public void EndGame()
    {
        // 올바르게 저장됐는지 확인 (자유롭게 변형)
        for (int i = 0; i < data.TopScore.Length - 1; i++)
        {
            if(data.TopScore[i] < GameManager.Instance.Score)
            {
                for (int j = 4; j >= i; j--)
                {
                    data.TopScore[j + 1] = data.TopScore[j];
                    if (data.TopScore[j] <= 0) break;
                }
                data.TopScore[i] = GameManager.Instance.Score;

                break;
            }
        }

        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장

        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{i + 1}. " + data.TopScore[i]);
            if (data.TopScore[i] == 0) GameManager.Instance.text[i].GetComponent<Text>().text = $"{i + 1}. (Empty)";

            else GameManager.Instance.text[i].GetComponent<Text>().text = $"{i + 1}. " + data.TopScore[i];
        }

        // 클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        print("저장 완료");
    }
}